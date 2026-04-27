using System;
using System.Windows.Forms;

namespace SkillBridgeApp
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialize DataStore with test data
            DataStore.Initialize();

            LoginForm loginForm = new LoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new MainForm());
            }
            else
            {
                Application.Exit();
            }
        }
    }
}
