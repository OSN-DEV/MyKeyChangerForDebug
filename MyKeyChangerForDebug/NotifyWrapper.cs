using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyKeyChangerForDebug {
    public partial class NotifyWrapper : Component {
        public NotifyWrapper() {
            InitializeComponent();
        }

        public NotifyWrapper(IContainer container) {
            container.Add(this);

            InitializeComponent();
        }


        // notifyIcon1.Icon = Icon.FromHandle(((Bitmap)imageList1.Images[0]).GetHicon());
    }
}
