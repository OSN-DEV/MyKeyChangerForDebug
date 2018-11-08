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
        private KeymappingHandler _keymappingHandler;
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

            _keymappingHandler = new KeymappingHandler();
            if (AppSettingData.GetInstance().Observered) {

            }

        }

        /// <summary>
        /// application exit event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e) {
            base.OnExit(e);
            _taskTrayMenu.Dispose();
        }
        #endregion

        #region TaskTrayMenuEvent
        private void _taskTrayMenu_OnMappingModeChanged(object sender, TaskTrayMenu.MappingModeChangedEventArgs e) {
            throw new NotImplementedException();
        }

        private void _taskTrayMenu_OnObserveStateChanged(object sender, TaskTrayMenu.ObserveStateChangedEventArgs e) {
            throw new NotImplementedException();
        }

        private void _taskTrayMenu_OnExitClicked(object sender, EventArgs e) {
            throw new NotImplementedException();
        }
        #endregion


    }
}
