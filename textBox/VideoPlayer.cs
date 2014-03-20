using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace textBox
{
    public partial class VideoPlayer : Form
    {
        public VideoPlayer()
        {
            InitializeComponent();
            axWindowsMediaPlayer1.URL = AppDomain.CurrentDomain.BaseDirectory + "test.wmv";
            axWindowsMediaPlayer1.Ctlcontrols.play();
            axWindowsMediaPlayer1.uiMode = "none";
            
        }

        public void Play()
        {
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        public void Stop()
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }
    }
}
