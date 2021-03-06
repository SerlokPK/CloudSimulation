﻿using cont1;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Containers
{
    public class Cont1
    {
        public static string port;
        static void Main(string[] args)
        {
            port = args[0];
            Start(port);
            //Console.WriteLine($"{port}");
            
            Console.ReadKey(true);
        }

        private static ServiceHost host;
        private static void Start(string port)
        {
            NetTcpBinding binding = new NetTcpBinding();
            host = new ServiceHost(typeof(Container));
            //Default timeout je 60 sekundi pa puca na proxy tokom debug-a
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            //Path.currentIndex++;
            host.AddServiceEndpoint(typeof(IContainer), binding, new Uri($"net.tcp://localhost:{port}/IContainer"));

            ServiceDebugBehavior debug = host.Description.Behaviors.Find<ServiceDebugBehavior>();

            // if not found - add behavior with setting turned on 
            if (debug == null)
            {
                host.Description.Behaviors.Add(
                     new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
            }
            else
            {
                // make sure setting is turned ON
                if (!debug.IncludeExceptionDetailInFaults)
                {
                    debug.IncludeExceptionDetailInFaults = true;
                }
            }
            int p = Int32.Parse(port) % 10;
            try
            {
                host.Open();
                Console.WriteLine($"Container {p+1} started.");
            }catch(Exception e)
            {
                Console.WriteLine($"Container  didn't start - error: {e.Message}");
            }
            
        }

        private static void Stop()
        {
            try
            {
                host.Close();
                Console.WriteLine($"Container  stopped.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Container  didn't stop - error: {e.Message}");
            }
        }
    }
}
