﻿using CryptoGeeks.Portunus.Comm;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoGeeks.Portunus.CommunicationFramework
{
    public class Workflow
    {
        public delegate void OnNewMessageHandler(object source, Payload payload, String message);
        public event OnNewMessageHandler OnNewMessage;

        public void OnNewMessageProxy(object source, Payload payload, String message)
        {
            if (OnNewMessage != null)
                OnNewMessage(source, payload, message);
        }

        public void StartListener(int port)
        {
            Thread t = new Thread(delegate ()
            {
                // replace the IP with your system IP Address...
                //Listener myserver = new Listener("192.168.***.***", 13000);
                Listener myserver = new Listener( port);
                myserver.OnNewMessage += (object source, Payload messagePayload, string message) => { OnNewMessageProxy(source, messagePayload, message); };
                myserver.StartListening();

                LoggerHelper.AddLog("listening on port " + port);
            });
            t.Start();

            OnNewMessageProxy(this, null, "Server Started on " + port);
        }

        
        public void TransmitData(string serverIp, int port, Payload payload, bool bind)
        {
            Thread t = new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Transmitter transmitter = new Transmitter();
                transmitter.OnNewMessage += (object source, Payload messagePayload, string message) => { OnNewMessageProxy(source, messagePayload, message); };
                //transmitter.Connect("192.168.***.***", payload);
                LoggerHelper.AddLog("Connecting to " + serverIp + ":" + port);
                transmitter.Connect(serverIp, port, payload, bind);


            });
            
            t.Start();
        }


        public List<UserStatusCompact> GetUsersConnection()
        {
            PayloadProcessor proc = new PayloadProcessor();
            CancellationToken tok = new CancellationToken();

            Task<List<UserStatusCompact>> t = proc.GetUsersConnection(tok);
            t.Wait();

            return t.Result;
        }
    }
}
