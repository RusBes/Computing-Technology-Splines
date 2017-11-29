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
            try
            {
                Application.Run(mainForm);
            }
            catch (NotImplementedException ex)
            {
                mainForm.ShowErrorMessage("Неможливо виконати дію. Дана можливість ще не реалізована.");
                //mainForm.ShowErrorMessage((ex.Message != null && ex.Message.Length > 0) ? ex.Message : "Неможливо виконати дію. Дана можливість ще не реалізована.");
            }
            catch (Exception ex)
            {
                mainForm.ShowErrorMessage(ex.Message);
            }

        }
    }
}
