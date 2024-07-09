using SharpPcap;
using System.Windows.Forms;

namespace TrayTool
{
    public partial class Form1 : Form
    {
        // https://stackoverflow.com/questions/7625421/minimize-app-to-system-tray

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.ShowInTaskbar = false;
                notifyIcon1.Visible = true;

                //notifyIcon1.ShowBalloonTip(500);
                //this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                this.ShowInTaskbar = true;
                notifyIcon1.Visible = false;
                //this.Show();
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = false;
        }

        private ILiveDevice? _loopback;

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (sender is CheckBox check)
                {
                    if (check.Checked)
                    {
                        var devices = CaptureDeviceList.Instance;
                        var targetDevice = @"\Device\NPF_Loopback";
                        _loopback = devices[targetDevice];
                        _loopback.Open();
                        toggleEnableLoopbackToolStripMenuItem.Text = "Disable Loopback";
                    }
                    else
                    {
                        _loopback?.Close();
                        _loopback?.Dispose();
                        _loopback = null;
                        toggleEnableLoopbackToolStripMenuItem.Text = "Enable Loopback";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Tray Tool!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toggleEnableLoopbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = !checkBox1.Checked;
        }
    }
}
