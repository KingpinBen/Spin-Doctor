using System;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using GameLibrary.System;

namespace SpinDoctor
{
#if WINDOWS || XBOX || DEBUG
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Application.ExecutablePath));
            AppDomain.CurrentDomain.UnhandledException += WriteErrorReport;

            using (Game1 game = new Game1())
            {
                game.SetArgs(args);
                game.Run();
            }
        }

        private static void WriteErrorReport(object sender, UnhandledExceptionEventArgs e)
        {
            string value = e.ExceptionObject.ToString();
            ErrorReport.GenerateReport(value, e);
        }
    }
#endif
}

