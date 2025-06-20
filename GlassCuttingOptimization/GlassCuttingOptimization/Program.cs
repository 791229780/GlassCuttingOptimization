using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using GlassCuttingOptimization.Utils;
using GlassCuttingOptimization.Views;
namespace GlassCuttingOptimization
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
     
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            GlobalExceptionHandler.HandleException(e.Exception, "UI Thread");
        }
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            GlobalExceptionHandler.HandleException(e.ExceptionObject as Exception, "Application Domain");
        }
    }
}
