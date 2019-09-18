using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Xml;
using System.IO;
using System.Net.NetworkInformation;

namespace CryptoGeeks.Portunus.Comm
{
    public class NAT
    {
        static TimeSpan _timeout = new TimeSpan(0, 0, 0, 3);
        public static TimeSpan TimeOut
        {
            get { return _timeout; }
            set { _timeout = value; }
        }
        static string _descUrl, _serviceUrl, _eventUrl, _hostAddress;
        public static bool Discover()
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            string reqTemp = "M-SEARCH * HTTP/1.1\r\n" +
            "HOST: {0}:1900\r\n" +
            "ST:upnp:rootdevice\r\n" +
            "MAN:\"ssdp:discover\"\r\n" +
            "MX:3\r\n\r\n";
            IPEndPoint ipe = new IPEndPoint(IPAddress.Broadcast, 1900);
            //byte[] buffer = new byte[0x1000];
            byte[] buffer = new byte[0x1000];

            DateTime start = DateTime.Now;

            List<string> url = new List<string>();
            List<string> respUrl = new List<string>();

            NetworkInterface[] nics = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces(); //get all network interfaces of the computer

            foreach (NetworkInterface adapter in nics)
            {
                // Only select interfaces that are Ethernet type and support IPv4 (important to minimize waiting time)
                // if (adapter.NetworkInterfaceType != NetworkInterfaceType.Ethernet) { continue; }
                if (adapter.Supports(NetworkInterfaceComponent.IPv4) == false) { continue; }
                try
                {
                    IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                    foreach (var ua in adapterProperties.UnicastAddresses)
                    {
                        if (ua.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            string req = string.Format(reqTemp, ua.Address.ToString());
                            byte[] data = Encoding.ASCII.GetBytes(req);

                            //SEND BROADCAST IN THE ADAPTER
                            //1) Set the socket as UDP Client
                            Socket bcSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); //broadcast socket
                                                                                                                          //2) Set socker options
                            bcSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                            bcSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                            bcSocket.ReceiveTimeout = 200; //receive timout 200ms
                                                           //3) Bind to the current selected adapter

                            _hostAddress = ua.Address.ToString();

                            string ip = ua.Address.ToString();
                            ip = ip.Remove(ip.LastIndexOf('.') + 1, ip.Length - (ip.LastIndexOf('.') + 1)) + "255";
                            IPEndPoint myLocalEndPoint = new IPEndPoint(IPAddress.Parse(ip), 1900);
                            //bcSocket.Bind(myLocalEndPoint);
                            //4) Send the broadcast data
                            bcSocket.SendTo(data, myLocalEndPoint);

                            //RECEIVE BROADCAST IN THE ADAPTER
                            int BUFFER_SIZE_ANSWER = 1024;
                            byte[] bufferAnswer = new byte[0x1000];
                           // do
                           // {
                                try
                                {
                                    bcSocket.Receive(bufferAnswer);
                                    // DevicesList.AddGetMyDevice(bufferAnswer)); //Corresponding functions to get the devices information. Depends on the application.
                                    // AddGetMyDevice(bufferAnswer)

                                    string resp = Encoding.ASCII.GetString(bufferAnswer);

                                    resp = resp.Substring(resp.ToLower().IndexOf("location:") + 9);
                                    resp = resp.Substring(0, resp.IndexOf("\r")).Trim();

                                    if (!url.Contains(resp))
                                    {
                                        url.Add(resp);
                                        if (!string.IsNullOrEmpty(_serviceUrl = GetServiceUrl(resp)))
                                        {
                                            _descUrl = resp;

                                            respUrl.Add(resp);

                                            _serviceUrl = resp;
                                            return true;
                                        }
                                    }

                                    //Operation on non-blocking socket would block

                                }
                                catch (Exception ex)
                                {
                                    string e = ex.Message;
                                    Console.WriteLine(ex.Message); break;
                                }

                           // } while (bcSocket.ReceiveTimeout != 0); //fixed receive timeout for each adapter that supports our broadcast
                            bcSocket.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    Console.WriteLine(ex.Message);
                }
            }


            return false;
        }

        private static string GetServiceUrl(string resp)
        {
            try
            {

                XmlDocument desc = new XmlDocument();
                WebRequest req = WebRequest.Create(resp);
                req.Timeout = 3000;
                desc.Load(req.GetResponse().GetResponseStream());
                XmlNamespaceManager nsMgr = new XmlNamespaceManager(desc.NameTable);
                nsMgr.AddNamespace("tns", "urn:schemas-upnp-org:device-1-0");
                XmlNode typen = desc.SelectSingleNode("//tns:device/tns:deviceType/text()", nsMgr);
                if (!typen.Value.Contains("InternetGatewayDevice"))
                    return null;
                XmlNode node = desc.SelectSingleNode("//tns:service[tns:serviceType=\"urn:schemas-upnp-org:service:WANIPConnection:1\"]/tns:controlURL/text()", nsMgr);
                if (node == null)
                    return null;
                XmlNode eventnode = desc.SelectSingleNode("//tns:service[tns:serviceType=\"urn:schemas-upnp-org:service:WANIPConnection:1\"]/tns:eventSubURL/text()", nsMgr);
                _eventUrl = CombineUrls(resp, eventnode.Value);
                return CombineUrls(resp, node.Value);
            }
            catch { return null; }

        }

        private static string CombineUrls(string resp, string p)
        {
            int n = resp.IndexOf("://");
            n = resp.IndexOf('/', n + 3);
            return resp.Substring(0, n) + p;
        }

        public static void ForwardPort(int port, ProtocolType protocol, string description)
        {
            _serviceUrl = "http://192.168.1.254:14909";
            _hostAddress = "239.255.255.250";
          //  port = 14909;

            if (string.IsNullOrEmpty(_serviceUrl))
                throw new Exception("No UPnP service available or Discover() has not been called");
            XmlDocument xdoc = SOAPRequest(_serviceUrl, "<u:AddPortMapping xmlns:u=\"urn:schemas-upnp-org:service:WANIPConnection:1\">" +
                "<NewRemoteHost></NewRemoteHost><NewExternalPort>" + port.ToString() + "</NewExternalPort><NewProtocol>" + protocol.ToString().ToUpper() + "</NewProtocol>" +
                "<NewInternalPort>" + port.ToString() + "</NewInternalPort><NewInternalClient>" + _hostAddress +
                "</NewInternalClient><NewEnabled>1</NewEnabled><NewPortMappingDescription>" + description +
            "</NewPortMappingDescription><NewLeaseDuration>0</NewLeaseDuration></u:AddPortMapping>", "AddPortMapping");
        }

        public static void DeleteForwardingRule(int port, ProtocolType protocol)
        {
            if (string.IsNullOrEmpty(_serviceUrl))
                throw new Exception("No UPnP service available or Discover() has not been called");
            XmlDocument xdoc = SOAPRequest(_serviceUrl,
            "<u:DeletePortMapping xmlns:u=\"urn:schemas-upnp-org:service:WANIPConnection:1\">" +
            "<NewRemoteHost>" +
            "</NewRemoteHost>" +
            "<NewExternalPort>" + port + "</NewExternalPort>" +
            "<NewProtocol>" + protocol.ToString().ToUpper() + "</NewProtocol>" +
            "</u:DeletePortMapping>", "DeletePortMapping");
        }

        public static IPAddress GetExternalIP()
        {
            if (string.IsNullOrEmpty(_serviceUrl))
                throw new Exception("No UPnP service available or Discover() has not been called");
            XmlDocument xdoc = SOAPRequest(_serviceUrl, "<u:GetExternalIPAddress xmlns:u=\"urn:schemas-upnp-org:service:WANIPConnection:1\">" +
            "</u:GetExternalIPAddress>", "GetExternalIPAddress");
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xdoc.NameTable);
            nsMgr.AddNamespace("tns", "urn:schemas-upnp-org:device-1-0");
            string IP = xdoc.SelectSingleNode("//NewExternalIPAddress/text()", nsMgr).Value;
            return IPAddress.Parse(IP);
        }

        private static XmlDocument SOAPRequest(string url, string soap, string function)
        {
            string req = "<?xml version=\"1.0\"?>" +
            "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" +
            "<s:Body>" +
            soap +
            "</s:Body>" +
            "</s:Envelope>";
            WebRequest r = HttpWebRequest.Create(url);
            r.Method = "POST";
            byte[] b = Encoding.UTF8.GetBytes(req);
            r.Headers.Add("SOAPACTION", "\"urn:schemas-upnp-org:service:WANIPConnection:1#" + function + "\"");
            r.ContentType = "text/xml; charset=\"utf-8\"";
            r.ContentLength = b.Length;
            r.GetRequestStream().Write(b, 0, b.Length);
            XmlDocument resp = new XmlDocument();
            WebResponse wres = r.GetResponse();
            Stream ress = wres.GetResponseStream();
            resp.Load(ress);
            return resp;
        }
    }
}
