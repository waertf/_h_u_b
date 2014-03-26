using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace textBox
{
    public partial class VideoPlayer : Form
    {
        public string VideoPlayerState { get; set; }
        public VideoPlayer()
        {
            InitializeComponent();
            axWindowsMediaPlayer1.settings.autoStart = false;
            axWindowsMediaPlayer1.URL = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["videoFile"];
            axWindowsMediaPlayer1.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(axWindowsMediaPlayer1_PlayStateChange);
            //axWindowsMediaPlayer1.uiMode = "none";
            //axWindowsMediaPlayer1.Ctlcontrols.play();
            
            
        }

        void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            switch (e.newState)
            {
                case 0:    // Undefined
                    VideoPlayerState = "Undefined";
                    break;

                case 1:    // Stopped
                    VideoPlayerState = "Stopped";
                    break;

                case 2:    // Paused
                    VideoPlayerState = "Paused";
                    break;

                case 3:    // Playing
                    VideoPlayerState = "Playing";
                    break;

                case 4:    // ScanForward
                    VideoPlayerState = "ScanForward";
                    break;

                case 5:    // ScanReverse
                    VideoPlayerState = "ScanReverse";
                    break;

                case 6:    // Buffering
                    VideoPlayerState = "Buffering";
                    break;

                case 7:    // Waiting
                    VideoPlayerState = "Waiting";
                    break;

                case 8:    // MediaEnded
                    VideoPlayerState = "MediaEnded";
                    break;

                case 9:    // Transitioning
                    VideoPlayerState = "Transitioning";
                    break;

                case 10:   // Ready
                    VideoPlayerState = "Ready";
                    break;

                case 11:   // Reconnecting
                    VideoPlayerState = "Reconnecting";
                    break;

                case 12:   // Last
                    VideoPlayerState = "Last";
                    break;

                default:
                    VideoPlayerState = ("Unknown State: " + e.newState.ToString());
                    break;
            }
        }

        public void Play()
        {
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        public void Stop()
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        public void VideoMax()
        {
            axWindowsMediaPlayer1.uiMode = "none";
            axWindowsMediaPlayer1.fullScreen = true;
        }

        public string Position()
        {
            return axWindowsMediaPlayer1.Ctlcontrols.currentPositionString;
        }
    }
}
