﻿using System;
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
        [STAThread] 
        
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            videoPlayer = new VideoPlayer();
            Application.Run(new Form1());
            
        }
    }
}
