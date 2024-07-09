namespace TrayTool
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            notifyIcon1 = new NotifyIcon(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            optionsToolStripMenuItem = new ToolStripMenuItem();
            toggleEnableLoopbackToolStripMenuItem = new ToolStripMenuItem();
            checkBox1 = new CheckBox();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // notifyIcon1
            // 
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.BalloonTipText = "Tray Tool";
            notifyIcon1.BalloonTipTitle = "Tray Tool";
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "notifyIcon1";
            notifyIcon1.Visible = true;
            notifyIcon1.DoubleClick += notifyIcon1_DoubleClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { optionsToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(117, 26);
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toggleEnableLoopbackToolStripMenuItem });
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(116, 22);
            optionsToolStripMenuItem.Text = "Options";
            // 
            // toggleEnableLoopbackToolStripMenuItem
            // 
            toggleEnableLoopbackToolStripMenuItem.Name = "toggleEnableLoopbackToolStripMenuItem";
            toggleEnableLoopbackToolStripMenuItem.Size = new Size(202, 22);
            toggleEnableLoopbackToolStripMenuItem.Text = "Toggle Enable Loopback";
            toggleEnableLoopbackToolStripMenuItem.Click += toggleEnableLoopbackToolStripMenuItem_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(19, 23);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(110, 19);
            checkBox1.TabIndex = 2;
            checkBox1.Text = "Open Loopback";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            ContextMenuStrip = contextMenuStrip1;
            Controls.Add(checkBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Tray Tool";
            Resize += Form1_Resize;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private CheckBox checkBox1;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem toggleEnableLoopbackToolStripMenuItem;
    }
}
