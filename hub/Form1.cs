using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Configuration;

namespace hub
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

            //Thread webServiceRequesThread = new Thread(() => WebServiceRequest());
            //webServiceRequesThread.Start();
        }

        private void WebServiceRequest()
        {
            Chilkat.Xml xml = new Chilkat.Xml();
            xml.LoadXmlFile("data.xml");
            // Navigate to the first company record.
            xml.FirstChild2();
            string gps_long = xml.GetChildContent("gps_long");
            string gps_lat = xml.GetChildContent("gps_lat");
            string speed = xml.GetChildContent("speed");
            string electricity = xml.GetChildContent("electricity");
            xml.NextSibling2();
            xml.FirstChild2();
            string roadtype1 = xml.GetChildContent("roadtype");
            xml.NextSibling2();
            string roadtype2 = xml.GetChildContent("roadtype");
            Invoke((MethodInvoker)delegate
            {
                speedLabel.Text = speed;
                batteryLabel.Text = electricity;
            });

            //start to send to avls server
            TcpClient avlsTcpClient = null;
            NetworkStream networkStream = null;
            try
            {
                string Temp = "NA";
                string Status = "00000000";
                string time = DateTime.UtcNow.ToString("yyMMddHHmmss");
                string Speed = speed;
                string Dir = "0";
                string uid = "0";
                string gps = "A";
                string _event = "0";
                string message = "null";
                string loc = "null";
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
                networkStream.Write(sendByte, 0, sendByte.Length);
                networkStream.Flush();
            }
            catch (Exception)
            {

                //throw;
            }
            finally
            {
                if(networkStream!=null)
                    networkStream.Close();
                if(avlsTcpClient!=null)
                    avlsTcpClient.Close();
            }
            

        }
    }
}
