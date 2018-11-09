using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MyKeyChangerForDebug {
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application {

        #region Declaration
        private TaskTrayMenu _taskTrayMenu;
        #endregion

        #region Application
        /// <summary>
        /// application startup event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            _taskTrayMenu = new TaskTrayMenu();
            _taskTrayMenu.SetMode(AppSettingData.GetInstance().Mode);
            _taskTrayMenu.SetObserveChecked(AppSettingData.GetInstance().Observered);
            _taskTrayMenu.OnObserveStateChanged += _taskTrayMenu_OnObserveStateChanged;
            _taskTrayMenu.OnMappingModeChanged += _taskTrayMenu_OnMappingModeChanged;
            _taskTrayMenu.OnExitClicked += _taskTrayMenu_OnExitClicked;

            if (AppSettingData.GetInstance().Observered) {
                KeymappingHandler.Start(AppSettingData.GetInstance().Mode);
            }
        }

        /// <summary>
        /// application exit event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e) {
            base.OnExit(e);
            _taskTrayMenu.Dispose();
            KeymappingHandler.Stop();
        }
        #endregion

        #region TaskTrayMenuEvent
        private void _taskTrayMenu_OnObserveStateChanged(object sender, TaskTrayMenu.ObserveStateChangedEventArgs e) {
            AppSettingData.GetInstance().Observered = e.Observerd;
            AppSettingData.GetInstance().Save();

            if (e.Observerd) {
                KeymappingHandler.Start(AppSettingData.GetInstance().Mode);
            } else {
                KeymappingHandler.Stop();
            }
        }

        private void _taskTrayMenu_OnMappingModeChanged(object sender, TaskTrayMenu.MappingModeChangedEventArgs e) {
            AppSettingData.GetInstance().Mode = e.Mode;
            AppSettingData.GetInstance().Save();

            KeymappingHandler.ChangeMapping(e.Mode);
        }

        private void _taskTrayMenu_OnExitClicked(object sender, EventArgs e) {
            KeymappingHandler.Stop();
            base.Shutdown();
        }
        #endregion


    }
}
