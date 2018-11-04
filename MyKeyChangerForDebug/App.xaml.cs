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
        private NotifyWrapper _notify;

        /// <summary>
        /// application startup event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            this._notify = new NotifyWrapper();
            // this._notify.Icon = new Icon(Resources.IconMain, new Size(16, 16));
        }

        /// <summary>
        /// application exit event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e) {
            base.OnExit(e);
            this._notify.Dispose();
        }

    }
}
