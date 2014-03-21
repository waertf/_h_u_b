using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ConsoleApplication1AVLS_Test
{
    class Program
    {
        static void Main(string[] args)
        {
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
                avlSbytes.Add(Convert.ToSByte(DateTime.Now.Hour));
                avlSbytes.Add(Convert.ToSByte(DateTime.Now.Minute));
                avlSbytes.Add(Convert.ToSByte(DateTime.Now.Second));
                for (int i = 0; i < 10; i++)
                {
                    avlSbytes.Add(48);
                }
                avlSbytes.Add(69);
                string lat = "121.522894";
                ConvertLocation(avlSbytes, lat);
                avlSbytes.Add(78);
                string lon = "25.063547";
                ConvertLocation(avlSbytes, lon);
                for (int i = 0; i < 5; i++)
                {
                    avlSbytes.Add(0);
                }
                sbyte crc=avlSbytes[0];
                for (int i = 1; i < avlSbytes.Count; i++)
                {
                    crc ^= avlSbytes[i];
                }
                avlSbytes.Add(crc);
                avlSbytes.Add(13);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
            finally
            {
                byte[] unsigned = new byte[avlSbytes.Count];
                Buffer.BlockCopy(avlSbytes.ToArray(), 0, unsigned, 0, avlSbytes.Count);
                TcpClient avlsTcpClient = new TcpClient("192.168.1.29",7000);
                NetworkStream networkStream = avlsTcpClient.GetStream();
                networkStream.Write(unsigned,0,unsigned.Length);
                avlsTcpClient.Close();
                networkStream.Close();
            }
        }

        private static void ConvertLocation(List<sbyte> avlSbytes, string lat)
        {
            string[] result = lat.Split(new char[] { '.' });
            string firstResult = result[1].Substring(0, 2);
            string secondResult = result[1].Substring(2, 2);
            string thridResult = result[1].Substring(4, 2);
            avlSbytes.Add((sbyte)(Convert.ToByte(result[0])));
            avlSbytes.Add((sbyte)(Convert.ToByte(firstResult)));
            avlSbytes.Add((sbyte)(Convert.ToByte(secondResult)));
            avlSbytes.Add((sbyte)(Convert.ToByte(thridResult)));
        }
    }
}
