using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace textBox
{
    public partial class Form1 : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private System.Timers.Timer autoWebServiceRequestTimer;
        private Thread requestTaskNumberThread,videoPlayerThread,roadTypeThread,timeTickThread;
        static Random rand = new Random();
        private string[] _roadTypeStrings = new[]
        {
            @"塞車時速10km/h",
            @"塞車時速20km/h",
            @"塞車時速30km/h",
            @"塞車時速40km/h",
            @"大排長龍",
            @"車多擁擠",
            @"事故造成交通緩慢",
            @"翻車造成交通緩慢",
            @"施工造成交通緩慢",
            @"交通事故",
            @"嚴重交通事故",
            @"車輛故障",
            @"交通事件",
            @"交通管制",
            @"道路有障礙物注意安全",
            @"路面不平或有坑洞",
            @"燈號不亮請注意",
            ""            
        };
        public Form1()
        {
            InitializeComponent();
            label6.Visible = false;
            textBoxRoadType2.Visible = false;
            label7.Visible = false;
            textBoxRoadType3.Visible = false;
            sbyte[] signed = { -2, -1, 0, 1, 2 };
            byte[] unsigned = new byte[signed.Length];
            Buffer.BlockCopy(signed, 0, unsigned, 0, signed.Length); 
            //textBoxTask.Text = "0";

            //autoWebServiceRequestTimer =
                    //new System.Timers.Timer(double.Parse(ConfigurationManager.AppSettings["autorequest_interval"]));
            //autoWebServiceRequestTimer.Elapsed += (sender, e) => { WebServiceRequest(); };
            

            requestTaskNumberThread = new Thread(()=>TaskNumberRequest());
            videoPlayerThread = new Thread(()=>VideoPlayerThread());
            roadTypeThread = new Thread(()=>roadTypeFunction());
            timeTickThread = new Thread(()=>timeTick());
            videoPlayerThread.Start();
            this.Closed += new EventHandler(Form1_Closed);
            this.TextChanged += new EventHandler(Form1_TextChanged);
        }

        void Form1_TextChanged(object sender, EventArgs e)
        {
            WebServiceRequest();
        }

        private void timeTick()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (true)
            {
                this.InvokeEx(f => f.Text = Program.videoPlayer.Position());
                this.InvokeEx(f => f.Invalidate());
                this.InvokeEx(f => f.Update());
                Thread.Sleep(100);
            }
        }

        private void roadTypeFunction()
        {
            while (true)
            {
                this.InvokeEx(f => f.textBoxRoadType1.Text = _roadTypeStrings[rand.Next(0, _roadTypeStrings.Length)]);
                Thread.Sleep(rand.Next(10,16)*1000);
            }
        }

        private void VideoPlayerThread()
        {
            Program.videoPlayer.Invoke((Action)delegate
            {
                Program.videoPlayer.Show();
                Program.videoPlayer.Play();
            });
            while (true)
            {
                if (Program.videoPlayer.VideoPlayerState!=null)
                if (Program.videoPlayer.VideoPlayerState.Equals("Playing"))
                {
                    //Thread.Sleep(int.Parse(ConfigurationManager.AppSettings["delayTime"]));
                    //autoWebServiceRequestTimer.Enabled = true;
                    requestTaskNumberThread.Start();
                    //roadTypeThread.Start();
                    timeTickThread.Start();
                    break;
                }

            }
            
        }

        void Form1_Closed(object sender, EventArgs e)
        {
            //autoWebServiceRequestTimer.Enabled = false;
            requestTaskNumberThread.Abort();
            videoPlayerThread.Abort();
            //roadTypeThread.Abort();
            Environment.Exit(0);
        }

        private ThreadStart TaskNumberRequest()
        {
            TcpClient taskTcpClient = null;
            NetworkStream taskNetStream = null;
            BufferedStream taskBs = null;
            while (true)
            {
                string data = string.Empty;
                try
                {
                    if(taskTcpClient==null)
                        taskTcpClient = new TcpClient(ConfigurationManager.AppSettings["TASK_SERVER_IP"],
                    int.Parse(ConfigurationManager.AppSettings["TASK_SERVER_PORT"]));
                    if (taskNetStream == null)
                    {
                        taskNetStream = taskTcpClient.GetStream();
                    }
                    while (true)
                    {
                        byte[] bytes = new byte[3];
                        if (taskNetStream.CanRead)
                        {
                            taskNetStream.Read(bytes, 0, bytes.Length);
                            data += Encoding.UTF8.GetString(bytes);
                            if (data.IndexOf("#") > -1)
                            {
                                taskNetStream.Flush();
                                var splitData = data.Split(new char[] {'#'});
                                string task = splitData[0];
                                this.InvokeEx(f => f.textBoxTask.Text = task);
                                this.InvokeEx(f => f.Invalidate());
                                this.InvokeEx(f => f.Update());
                                break;
                            }
                        }
                    }

                        
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    if(taskTcpClient!=null)
                        taskTcpClient.Close();
                    if(taskNetStream!=null)
                        taskNetStream.Close();
                    taskTcpClient = null;
                    taskNetStream = null;
                }
            }
        }

        string ReadTextFromUrl(string url)
        {
            // WebClient is still convenient
            // Assume UTF8, but detect BOM - could also honor response charset I suppose
            using (var client = new WebClient())
            using (var stream = client.OpenRead(url))
            using (var textReader = new StreamReader(stream, Encoding.UTF8, true))
            {
                return textReader.ReadToEnd();
            }
        }
        TcpClient _avlsTcpClient = null;
        NetworkStream _avlsNetworkStream = null;
        private uint sid = 1;
        private uint timecount = 0;
        private void WebServiceRequest()
        {
            
            //http://192.168.1.35/carinfo.php?sid=1
            //sid range:1 to 5257
            /*
            this.InvokeEx(f => f.textBoxRoadType1.Clear());
            this.InvokeEx(f => f.textBoxRoadType2.Clear());
            this.InvokeEx(f => f.textBoxRoadType3.Clear());
            this.InvokeEx(f => f.Invalidate());
            this.InvokeEx(f => f.Update());
            */
            Chilkat.Xml xml = new Chilkat.Xml();
            xml.Encoding = "utf-8";
            //string[] file_list = Directory.GetFiles(Environment.CurrentDirectory, "*.xml", SearchOption.TopDirectoryOnly);
            //Debug.WriteLine(ReadTextFromUrl(@"http://" + ConfigurationManager.AppSettings["STUPID_IP_ADDRESS"] + @"/carinfo.php?sid=" + rand.Next(1,5258)));
            //xml.LoadXmlFile(file_list[rand.Next(0,file_list.Length)]);
            xml.LoadXml(
                ReadTextFromUrl(@"http://" + ConfigurationManager.AppSettings["STUPID_IP_ADDRESS"] +
                                @"/carinfo.php?sid=" + sid));
            if (sid.Equals(36))
            {
                sid = 1;
                timecount = 0;
                this.InvokeEx(f => f.textBoxSpeed.Text = "0");
                this.InvokeEx(f => f.textBoxBattery.Text = "");
                this.InvokeEx(f => f.textBoxDirection.Text = "");
                Program.videoPlayer.Invoke((Action)delegate
                {
                    Program.videoPlayer.Stop();
                    //timeTickThread.Abort();
                });
                //autoWebServiceRequestTimer.Enabled = false;
                while (true)
                {
                    Thread.Sleep(3000);
                    if (Program.videoPlayer.VideoPlayerState != null)
                        if (Program.videoPlayer.VideoPlayerState.Equals("Stopped"))
                        {
                            Program.videoPlayer.Invoke((Action)delegate
                            {
                                Program.videoPlayer.Play();
                                //Thread.Sleep(int.Parse(ConfigurationManager.AppSettings["delayTime"]));
                                //timeTickThread = new Thread(() => timeTick());
                                //timeTickThread.Start();
                            });
                            //autoWebServiceRequestTimer.Enabled = true;
                            break;
                        }

                }
            }
            else
            {
                sid++;
                timecount++;
            }
            // Navigate to the first company record.
            xml.FirstChild2();
            string gps_long = xml.GetChildContent("gps_long");
            string gps_lat = xml.GetChildContent("gps_lat");
            string speed = xml.GetChildContent("speed");
            string electricity = xml.GetChildContent("electricity");
            string direction = xml.GetChildContent("direction");
            xml.NextSibling2();
            int roadCount = xml.NumChildrenHavingTag("record");
            xml.FirstChild2();
            List<string> roadTypeList = new List<string>();
            for (int i = 0; i < roadCount; i++)
            {
                roadTypeList.Add(xml.GetChildContent("roadtype"));
                xml.NextSibling2();
            }
            xml.Dispose();
            //string roadtype1 = xml.GetChildContent("roadtype");
            //xml.NextSibling2();
            //string roadtype2 = xml.GetChildContent("roadtype");
            this.InvokeEx(f => f.textBoxSpeed.Text = speed);
            this.InvokeEx(f => f.textBoxBattery.Text = electricity);
            this.InvokeEx(f => f.textBoxDirection.Text = direction);
            
            for (int i = 0; i < roadTypeList.Count(); i++)
            {
                switch (i)
                {
                    case 0:
                        this.InvokeEx(f => f.textBoxRoadType1.Text = roadTypeList[0]);
                        break;
                    case 1:
                        this.InvokeEx(f => f.textBoxRoadType2.Text = roadTypeList[1]);
                        break;
                    case 2:
                        this.InvokeEx(f => f.textBoxRoadType3.Text = roadTypeList[2]);
                        break;
                }
            }
            
            this.InvokeEx(f => f.Invalidate());
            this.InvokeEx(f => f.Update());


            //start to send to avls server

            List<sbyte> avlSbytes = new List<sbyte>();
            try
            {
                avlSbytes.Add(-88);
                avlSbytes.Add(-88);
                avlSbytes.Add(-128);
                avlSbytes.Add(0);
                avlSbytes.Add(34);
                sbyte fiveSbyte = (sbyte)(Convert.ToByte(DateTime.Now.Year / 256));
                avlSbytes.Add(fiveSbyte);
                sbyte sixSbyte = (sbyte)(Convert.ToByte(DateTime.Now.Year % 256));
                avlSbytes.Add(sixSbyte);
                avlSbytes.Add(Convert.ToSByte(DateTime.Now.Month));
                avlSbytes.Add(Convert.ToSByte(DateTime.Now.Day));
                avlSbytes.Add(Convert.ToSByte(DateTime.Now.ToUniversalTime().Hour));
                avlSbytes.Add(Convert.ToSByte(DateTime.Now.Minute));
                avlSbytes.Add(Convert.ToSByte(DateTime.Now.Second));
                
                avlSbytes.Add(0x30);
                avlSbytes.Add(0x39);
                avlSbytes.Add(0x31);
                avlSbytes.Add(0x32);
                avlSbytes.Add(0x33);
                avlSbytes.Add(0x34);
                avlSbytes.Add(0x35);
                avlSbytes.Add(0x36);
                avlSbytes.Add(0x37);
                avlSbytes.Add(0x38);
               
                avlSbytes.Add(69);
                ConvertLocation(avlSbytes, gps_long);
                avlSbytes.Add(78);
                ConvertLocation(avlSbytes, gps_lat);
                for (int i = 0; i < 5; i++)
                {
                    avlSbytes.Add(0);
                }
                sbyte crc = avlSbytes[0];
                for (int i = 1; i < avlSbytes.Count; i++)
                {
                    crc ^= avlSbytes[i];
                }
                avlSbytes.Add(crc);
                avlSbytes.Add(13);

                byte[] unsigned = new byte[avlSbytes.Count];
                Buffer.BlockCopy(avlSbytes.ToArray(), 0, unsigned, 0, avlSbytes.Count);

                if(_avlsTcpClient==null)
                _avlsTcpClient = new TcpClient(ConfigurationManager.AppSettings["AVLS_SERVER_IP"],
                    int.Parse(ConfigurationManager.AppSettings["AVLS_SERVER_PORT"]));
                if (_avlsNetworkStream == null)
                {
                    _avlsNetworkStream = _avlsTcpClient.GetStream();
                }

                if (_avlsNetworkStream.CanWrite)
                {
                    _avlsNetworkStream.Write(unsigned, 0, unsigned.Length);
                    _avlsNetworkStream.Flush();
                }
                
                //avlsNetworkStream.Write(sendByte, 0, sendByte.Length);
                //avlsNetworkStream.Flush();
            }
            catch (Exception ex)
            {

                log.Error(ex);
                if (_avlsTcpClient != null)
                    _avlsTcpClient.Close();
                if (_avlsNetworkStream != null)
                    _avlsNetworkStream.Close();
                _avlsTcpClient = null;
                _avlsNetworkStream = null;

            }
            finally
            {
                
            }


        }
        private void ConvertLocation(List<sbyte> avlSbytes, string lat)
        {
            string[] result = lat.Split(new char[] { '.' });
            avlSbytes.Add((sbyte)(Convert.ToByte(result[0])));

            string firstResult = result[1].Substring(0, 2);
            avlSbytes.Add((sbyte)(Convert.ToByte(firstResult)));
            string secondResult = result[1].Substring(2, 2);
            avlSbytes.Add((sbyte)(Convert.ToByte(secondResult)));
            string thridResult = "0";
            avlSbytes.Add((sbyte)(Convert.ToByte(thridResult)));
            
            
        }
        private void ConvertLocToAvlsLoc(ref string lat, ref string lon)
        {
            double tmpLat = double.Parse(lat);
            double tmpLon = double.Parse(lon);
            double latInt = Math.Truncate(tmpLat);
            double lonInt = Math.Truncate(tmpLon);
            double latNumberAfterPoint = tmpLat - latInt;
            double lonNumberAfterPoint = tmpLon - lonInt;
            lat = ((latNumberAfterPoint * 60 / 100 + latInt) * 100).ToString();
            lon = ((lonNumberAfterPoint * 60 / 100 + lonInt) * 100).ToString();
        }
        
    }
    public static class ISynchronizeInvokeExtensions
    {
        public static void InvokeEx<T>(this T @this, Action<T> action) where T : ISynchronizeInvoke
        {
            if (@this.InvokeRequired)
            {
                @this.Invoke(action, new object[] { @this });
            }
            else
            {
                action(@this);
            }
        }
    }
}
