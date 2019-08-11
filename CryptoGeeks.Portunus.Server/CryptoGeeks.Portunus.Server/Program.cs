﻿using CryptoGeeks.Portunus.CommunicationFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoGeeks.Portunus.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Workflow workflow = new Workflow();

            workflow.StartListener(Helper.GetMachineIp(), 7001);


            while (true)
            {
                System.Threading.Thread.Sleep(100);
            }
        }
    }
}
