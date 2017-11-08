using System;
using Sparrow.Core;

namespace Stardust_Library_Sample
{
    public class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            System.Windows.Application app = new System.Windows.Application();
            app.Run(new DesktopViewController(typeof(SampleApp), 960, 640));
        }
    }
}