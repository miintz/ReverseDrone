using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RobotThing
{
    class Program
    {
        static SerialPort COM;
        static Thread HydraThread;
        static void Main(string[] args)
        {
            COM = new SerialPort("COM4", 9600, Parity.None, 8);
            COM.Open();
            
            if (COM.IsOpen)
            {
                COM.DataReceived += COM_DataReceived;
                COM.Write("9");

                Console.WriteLine("COM opening.");
            }
            else            
                Console.WriteLine("COM failed to open. Press enter to exit.");

            Console.WriteLine("Initializing Hydra using HydraControl class");
            int status = HydraControl.sixenseInit();
            //status lijkt altijd 0 te zijn. er komt een bericht uit de sixense dll als de connectie klopt, anders niet
            Console.WriteLine("Press enter to start");
            Console.WriteLine();
            Console.ReadLine();

            ThreadStart start = new ThreadStart(HydraAction);
            Console.WriteLine("Starting HydraAction thread");
            HydraThread = new Thread(start);
            HydraThread.Start();

            Console.ReadLine();
            
            KeepGoing = false;
            HydraThread.Join();

            HydraControl.sixenseExit();
            COM.Close();

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

        static void COM_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] buffer = new byte[COM.BytesToRead];
            COM.Read(buffer, 0, COM.BytesToRead);

            string a = Encoding.ASCII.GetString(buffer);

            switch (a)
            { 
                case "v":
                    Console.WriteLine("DEBUG: Hello, this is Arduino speaking from " + COM.PortName + ". Ready to transmit.");
                    break;
            }
        }
        static bool KeepGoing = true;
        static bool Initial = true;
        static HydraControl.sixenseAllControllerData HydraData;
        public static void HydraAction()
        {
            if (Initial)
            {
                Initial = false;
                Console.WriteLine("HydraAction thread up and running. Press enter to stop.");
            }

            while (KeepGoing) {
                Thread.Sleep(500);
                HydraControl.sixenseGetAllData(0, ref HydraData);

                //Docked? skip this iteration
                if (HydraData.controllers[0].is_docked == 2 || HydraData.controllers[1].is_docked == 2)
                {
                    Console.WriteLine("DEBUG: Controller(s) docked. Skipping.");
                    continue;
                }
                    
                                
            }
        }
    }
}