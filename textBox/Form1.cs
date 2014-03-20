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
        private Thread requestTaskNumberThread;
        static Random rand = new Random();
        public Form1()
        {
            InitializeComponent();
            Program.vi.Show();
            Program.vi.Play();
            sbyte[] signed = { -2, -1, 0, 1, 2 };
            byte[] unsigned = new byte[signed.Length];
            Buffer.BlockCopy(signed, 0, unsigned, 0, signed.Length); 
            //textBoxTask.Text = "0";

            autoWebServiceRequestTimer =
                    new System.Timers.Timer((int)uint.Parse(ConfigurationManager.AppSettings["autorequest_interval"]));
            autoWebServiceRequestTimer.Elapsed += (sender, e) => { WebServiceRequest(); };
            autoWebServiceRequestTimer.Enabled = true;

            requestTaskNumberThread = new Thread(()=>TaskNumberRequest());
            requestTaskNumberThread.Start();
            this.Closed += new EventHandler(Form1_Closed);
        }

        void Form1_Closed(object sender, EventArgs e)
        {
            autoWebServiceRequestTimer.Enabled = false;
            requestTaskNumberThread.Abort();
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
        private BufferedStream _avlsBufferedStream = null;
        private uint sid = 1;
        
        private void WebServiceRequest()
        {
            //http://192.168.1.35/carinfo.php?sid=1
            //sid range:1 to 5257
            this.InvokeEx(f => f.textBoxRoadType1.Clear());
            this.InvokeEx(f => f.textBoxRoadType2.Clear());
            this.InvokeEx(f => f.textBoxRoadType3.Clear());
            this.InvokeEx(f => f.Invalidate());
            this.InvokeEx(f => f.Update());
            Chilkat.Xml xml = new Chilkat.Xml();
            xml.Encoding = "utf-8";
            //string[] file_list = Directory.GetFiles(Environment.CurrentDirectory, "*.xml", SearchOption.TopDirectoryOnly);
            //Debug.WriteLine(ReadTextFromUrl(@"http://" + ConfigurationManager.AppSettings["STUPID_IP_ADDRESS"] + @"/carinfo.php?sid=" + rand.Next(1,5258)));
            //xml.LoadXmlFile(file_list[rand.Next(0,file_list.Length)]);
            xml.LoadXml(
                ReadTextFromUrl(@"http://" + ConfigurationManager.AppSettings["STUPID_IP_ADDRESS"] +
                                @"/carinfo.php?sid=" + sid));
            if (sid.Equals(5257))
                sid = 1;
            else
            {
                sid++;
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
            

            try
            {
                string lat_str = gps_lat, long_str = gps_long;
                ConvertLocToAvlsLoc(ref lat_str, ref long_str);
                string Temp = "NA";
                string Status = "00000000";
                string time = DateTime.UtcNow.ToString("yyMMddHHmmss");
                string Speed = speed;
                string Dir = "0";
                string uid = "0";
                string gps = "A";
                string _event = "0";
                string message = "null";
                string loc = "N" + lat_str + "E" + long_str;
                string package = "%%" + uid + "," +
                              gps + "," +
                              time + "," +
                              loc + "," +
                              Speed + "," +
                              Dir + "," +
                              Temp + "," +
                              Status + "," +
                              _event + "," +
                              message + Environment.NewLine;
                byte[] sendByte = Encoding.UTF8.GetBytes(package);
                if(_avlsTcpClient==null)
                _avlsTcpClient = new TcpClient(ConfigurationManager.AppSettings["AVLS_SERVER_IP"],
                    int.Parse(ConfigurationManager.AppSettings["AVLS_SERVER_PORT"]));
                if (_avlsNetworkStream == null)
                {
                    _avlsNetworkStream = _avlsTcpClient.GetStream();
                    _avlsBufferedStream = new BufferedStream(_avlsNetworkStream);
                }
                
                if (_avlsBufferedStream.CanWrite)
                {
                    _avlsBufferedStream.Write(sendByte, 0, sendByte.Length);
                    _avlsBufferedStream.Flush();
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
