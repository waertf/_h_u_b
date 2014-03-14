using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace textBox
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var autoSendFromSqlTableTimer =
                    new System.Timers.Timer((int)uint.Parse(ConfigurationManager.AppSettings["autorequest_interval"]));
            autoSendFromSqlTableTimer.Elapsed += (sender, e) => { WebServiceRequest(); };
            autoSendFromSqlTableTimer.Enabled = true;
        }
        private void WebServiceRequest()
        {
            Chilkat.Xml xml = new Chilkat.Xml();
            xml.Encoding = "utf-8";
            xml.LoadXmlFile("data.xml");
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
            TcpClient avlsTcpClient = null;
            NetworkStream networkStream = null;

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
                avlsTcpClient = new TcpClient(ConfigurationManager.AppSettings["AVLS_SERVER_IP"],
                    int.Parse(ConfigurationManager.AppSettings["AVLS_SERVER_PORT"]));
                networkStream = avlsTcpClient.GetStream();
                BufferedStream bs = new BufferedStream(networkStream);
                bs.Write(sendByte, 0, sendByte.Length);
                bs.Flush();
                //networkStream.Write(sendByte, 0, sendByte.Length);
                //networkStream.Flush();
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex);
            }
            finally
            {
                if (networkStream != null)
                    networkStream.Close();
                if (avlsTcpClient != null)
                    avlsTcpClient.Close();
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
