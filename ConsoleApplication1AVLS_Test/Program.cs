using System;
using System.Collections.Generic;
using System.Linq;
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
                sbyte fiveSbyte = Convert.ToSByte(DateTime.Now.Year/256);
                avlSbytes.Add(fiveSbyte);
                sbyte sixSbyte = Convert.ToSByte(DateTime.Now.Year%256);
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
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
            finally
            {
                byte[] unsigned = new byte[avlSbytes.Count];
                Buffer.BlockCopy(avlSbytes.ToArray(), 0, unsigned, 0, avlSbytes.Count);
            }
        }
    }
}
