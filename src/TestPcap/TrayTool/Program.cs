namespace TrayTool
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var m = new Mutex(true, "OOBDev.TrayTool", out var createdNew);

            if (!createdNew)
            {
                // myApp is already running...
                MessageBox.Show("Tray Tool is already running!", "Multiple Instances");
                return;
            }

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}