using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace textBox
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static VideoPlayer videoPlayer;
        public static formSHOW formHUDShow;
        [STAThread] 
        
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            formHUDShow = new formSHOW();
            videoPlayer = new VideoPlayer();
            Application.Run(new Form1());
            
        }
    }
}
