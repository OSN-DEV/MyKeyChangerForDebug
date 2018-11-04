namespace MyKeyChangerForDebug {
    partial class NotifyWrapper {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotifyWrapper));
            this.cNotify = new System.Windows.Forms.NotifyIcon(this.components);
            this.cMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cWatch = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenu_Watch = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuMode = new System.Windows.Forms.ToolStripComboBox();
            this.cMenu.SuspendLayout();
            // 
            // cNotify
            // 
            this.cNotify.ContextMenuStrip = this.cMenu;
            this.cNotify.Icon = ((System.Drawing.Icon)(resources.GetObject("cNotify.Icon")));
            this.cNotify.Text = "notifyIcon1";
            this.cNotify.Visible = true;
            // 
            // cMenu
            // 
            this.cMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cMenu_Watch,
            this.cMenuMode,
            this.cMenuSeparator1,
            this.cMenuExit});
            this.cMenu.Name = "cMenu";
            this.cMenu.Size = new System.Drawing.Size(182, 81);
            // 
            // cWatch
            // 
            this.cWatch.Checked = true;
            this.cWatch.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.cWatch.Name = "cWatch";
            this.cWatch.Size = new System.Drawing.Size(108, 22);
            this.cWatch.Text = "Watch";
            // 
            // cMenu_Watch
            // 
            this.cMenu_Watch.Name = "cMenu_Watch";
            this.cMenu_Watch.Size = new System.Drawing.Size(181, 22);
            this.cMenu_Watch.Text = "toolStripMenuItem1";
            // 
            // cMenuSeparator1
            // 
            this.cMenuSeparator1.Name = "cMenuSeparator1";
            this.cMenuSeparator1.Size = new System.Drawing.Size(178, 6);
            // 
            // cMenuExit
            // 
            this.cMenuExit.Name = "cMenuExit";
            this.cMenuExit.Size = new System.Drawing.Size(181, 22);
            this.cMenuExit.Text = "toolStripMenuItem1";
            // 
            // cMenuMode
            // 
            this.cMenuMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cMenuMode.Items.AddRange(new object[] {
            "Android",
            "Visual Studio"});
            this.cMenuMode.Name = "cMenuMode";
            this.cMenuMode.Size = new System.Drawing.Size(121, 23);
            this.cMenu.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip cMenu;
        internal System.Windows.Forms.NotifyIcon cNotify;
        private System.Windows.Forms.ToolStripMenuItem cWatch;
        private System.Windows.Forms.ToolStripMenuItem cMenu_Watch;
        private System.Windows.Forms.ToolStripComboBox cMenuMode;
        private System.Windows.Forms.ToolStripSeparator cMenuSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cMenuExit;
    }
}
