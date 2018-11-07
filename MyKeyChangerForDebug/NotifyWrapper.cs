using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyKeyChangerForDebug {
    public partial class NotifyWrapper : Component {
        #region Declaration


        public class ObserveStateChangedEventArgs : EventArgs {
            public bool Observerd { set;  get; }
            public ObserveStateChangedEventArgs(bool observed) {
                this.Observerd = observed;
            }
        }
        public delegate void ObserveStateChangedHandler(object sender, ObserveStateChangedEventArgs e);
        public event ObserveStateChangedHandler OnObserveStateChanged;


        #endregion

        #region Constructor
        public NotifyWrapper() {
            InitializeComponent();
            this.Initialize();
        }

        public NotifyWrapper(IContainer container) {
            container.Add(this);
            InitializeComponent();
            this.Initialize();
        }
        #endregion

        #region Private Method
        /// <summary>
        /// initialize this component
        /// </summary>
        private void Initialize() {
            this.cMenu.SuspendLayout();
            // create context menu by code, bacuase I can't find the way to create nested menu. 
            this.cMenu.Items.Clear();

            ToolStripMenuItem itemObserve = new ToolStripMenuItem();
            itemObserve.Text = "observe";
            itemObserve.ToolTipText = "observe to input ten key";
            itemObserve.CheckedChanged += OnObserveCheckedChanged;
            this.cMenu.Items.Add(itemObserve);

            ToolStripMenuItem itemMode = new ToolStripMenuItem();
            itemMode.Text = "mode";
            this.cMenu.Items.Add(itemMode);

            ToolStripMenuItem itemModeAndroid = new ToolStripMenuItem();
            itemModeAndroid.Text = "Android";
            itemModeAndroid.ToolTipText = "Key Assign is for Android";
            itemMode.DropDown.Items.Add(itemModeAndroid);

            ToolStripMenuItem itemModeDotNet = new ToolStripMenuItem();
            itemModeDotNet.Text = ".Net";
            itemModeDotNet.ToolTipText = "Key Assign is for .Net";
            itemMode.DropDown.Items.Add(itemModeDotNet);

            ToolStripSeparator separator = new ToolStripSeparator();
            this.cMenu.Items.Add(separator);

            ToolStripMenuItem itemExit = new ToolStripMenuItem();
            itemExit.Text = "Exit";
            itemExit.ToolTipText = "Exit Application";
            this.cMenu.Items.Add(itemExit);
            this.cMenu.ResumeLayout();
        }
        #endregion

        #region Event
        private void OnObserveCheckedChanged(object sender, EventArgs e) {
            if (null != this.OnObserveStateChanged) {
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                this.OnObserveStateChanged(this, new ObserveStateChangedEventArgs(item.Checked));
            }

        }
        #endregion


        // notifyIcon1.Icon = Icon.FromHandle(((Bitmap)imageList1.Images[0]).GetHicon());
    }
}
