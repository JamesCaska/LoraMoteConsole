using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Diagnostics;

namespace LoraMoteConsole
{
    class Program
    {
        static SerialPort Serial;


        private static void ABP()
        {
            Console.WriteLine("EUI: ");                                                                                  
            Serial.WriteLine("sys get hweui");
            readReponse();
           
            Console.WriteLine("EUI: ");                                                                                   
            Serial.WriteLine("mac set deveui BE5D9A3942AF451E");
            readReponse();
             
            Console.WriteLine("addr: ");                                                                                  
            Serial.WriteLine("mac set devaddr A80E4AD9");
             readReponse();

            Console.WriteLine("appedui: ");
            Serial.WriteLine("mac set appeui BE5D9A3942AF451F");                                   
            readReponse();
             
            Console.WriteLine("nwkskey: ");                                                                             
            Serial.WriteLine("mac set nwkskey B44207ADB2A409C8D3057722E4285996");
            readReponse();
             
            Console.WriteLine("appskey: ");
            Serial.WriteLine("mac set appskey 847048638932C4C27E08C778E7953F8C");                                  
            readReponse();
  
            Console.WriteLine("set rx2: ");
            Serial.WriteLine("mac set rx2 3 869525000");                                   
            readReponse();
     
            Console.WriteLine("adr: ");
            Serial.WriteLine("mac set adr off");
            readReponse();
 
            Console.WriteLine("Join: ");
            Serial.WriteLine("mac join abp");
    
        }
      
        private static void OTAA()
        {
   
            Console.WriteLine("EUI: ");                                                                    
            Serial.WriteLine("mac set deveui 8306050e77f15e78");
            readReponse();
  
            Console.WriteLine("appkey: ");
            Serial.WriteLine("mac set appkey 75b3c6f167a74fc7bf2268d2f10cc495");                             
            readReponse();

            Console.WriteLine("appedui: ");
            Serial.WriteLine("mac set appeui c80c86972373840e");                                  
            readReponse();

            Console.WriteLine("set rx2: ");
            Serial.WriteLine("mac set rx2 3 869525000");                                 
            readReponse();
             
            Console.WriteLine("adr: ");
            Serial.WriteLine("mac set adr off");
            readReponse();
             
            Console.WriteLine("Join: ");
            Serial.WriteLine("mac join otaa");
            readReponse();

            WaitForResponse(10000, "accepted,denied");
        

        }

        private static string toHex(String s)
        {
            byte[] b1 = System.Text.Encoding.UTF8.GetBytes(s);
            return ByteArrayToString(b1);
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }



        static void Main(string[] args)
        {
           
            Serial = new SerialPort("COM3", 57600);
            Serial.NewLine = "\r\n";
            Serial.Open();
            Console.WriteLine("Reset");
   
            Serial.WriteLine ("sys reset\r\n");
            readReponse(2000);
             
   
            bool otta = true;

            if( otta)
            {
                OTAA();
            }
            else
            {
                ABP();
            }
           
            int index = 0;
            int msgId = 0;
            while (true)
            {
                Console.WriteLine("Sending Data: ");
                Serial.WriteLine("mac tx cnf 1 " + toHex("Hello World" + msgId.ToString() ) );               
                index = ( index + 1 ) % 10;

                if (otta)
                {
                    if (WaitForResponse(10000, "mac_rx"))
                    {
                        int pos = response.IndexOf("mac_rx") + 9;
                        int endPos = response.IndexOf('\r', pos);
                        string msg = response.Substring(pos, endPos - pos);

                        string m = FromHex(msg);
                        Console.WriteLine("Received : " + m);

                    }
                }
                else
                {
                    WaitForResponse(10000, "mac_tx_ok");
                }

                System.Threading.Thread.Sleep(5000);
                msgId++;
            }
        }

       
    public static string FromHex(string hex)
    {
         
        byte[] raw = new byte[hex.Length / 2];
        for (int i = 0; i < raw.Length; i++)
        {
            raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        }
        return  Encoding.ASCII.GetString(raw);   
    }
    static string response = "";
        private static void readReponse() {
            readReponse(250);
        }
        private static bool   WaitForResponse(int maxdelay,string expected)
        {
            response = "";
            Stopwatch sw = Stopwatch.StartNew();

            while (maxdelay>0)
            {
                System.Threading.Thread.Sleep(100);
                maxdelay -= 100;
                response += Serial.ReadExisting();
                 
                if (response != null && !response.Equals(""))
                {
                    foreach(string s in expected.Split(','))
                    {
                        if (response.Contains(s))
                        {
                            System.Threading.Thread.Sleep(100);
                            response += Serial.ReadExisting();
                            Console.WriteLine("Time = " + sw.ElapsedMilliseconds.ToString());
                            Console.WriteLine(response);
                            return true;
                        }
                    }
                   
                }
            }
            Console.WriteLine(response);

            Console.WriteLine("TIMEOUT!");

            return false;
        }

        private static void readReponse(int delay)
        {
            System.Threading.Thread.Sleep(delay);
            string response = Serial.ReadExisting();
            if(response!=null && !response.Equals(""))
            {
                Console.WriteLine(response);
            }
          
        }
    }
}
