using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using GameLibrary.GameLogic;
using GameLibrary.Graphics.Camera;

namespace GameLibrary.Helpers
{
    public static class ErrorReport
    {
        public static void GenerateReport(string error, EventArgs e)
        {
            string time = DateTime.Now.ToString("dd.MM-hh.mm.ss");
            string filename = "error_" + time + ".txt";
            int num = 1;

            while (File.Exists(string.Format(filename, num)))
            {
                num++;
            }

            filename = string.Format(filename, num);
            TextWriter textWriter = File.CreateText(filename);

            textWriter.WriteLine("Spin Doctor Crash Report\n");
            textWriter.WriteLine(error);
            textWriter.WriteLine("\n");
            StackTrace stackTrace = new StackTrace(true);

            for (int i = 1; i < stackTrace.FrameCount; i++)
            {
                StackFrame frame = stackTrace.GetFrame(i);
                textWriter.WriteLine("0x{0:x4} {1}->{2}.{3}", new object[]
				{
					frame.GetILOffset(), 
					frame.GetMethod().Module, 
					frame.GetMethod().DeclaringType.FullName, 
					frame.GetMethod().Name
				});
            }

            textWriter.Close();
            textWriter.Dispose();

            MessageBox.Show("Uh oh, you've crashed.\nAn error report has been put alongside this executable.");

            if (!Debugger.IsAttached)
            {
                Process.GetCurrentProcess().Kill();
            }

            //  ...don't judge me. I'm desperate.
            Camera.Instance.GetGameScreen().ScreenManager.Game.Exit();
        }
    }
}
