using System;
using System.Windows.Forms;
using Laba1.View;
using CommonModel;

namespace Laba1
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
            var mainForm = new MainForm();
            var presenter = new Presenter.Presenter(mainForm, new Model());
            Application.Run(mainForm);
        }
    }
}
