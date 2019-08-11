using CryptoGeeks.Portunus.CommunicationFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CryptoGeeks.Portunus.Comm
{
    public partial class Ping
    {
        public int User_Fk { get; set; }
        public System.DateTime Time { get; set; }        
    }

    public class PayloadProcessor
    {
        public void HandlePayload(ref Payload payload)
        {
            switch (payload.MessageType)
            {
                case MessageType.Ping:
                    MarkPing(payload).Wait();
                    break;

                case MessageType.RequestForOpen:
                    break;

                case MessageType.RequestForChannel:
                    break;
            }
        }

        public async Task<bool> MarkPing(Payload payload)
        {
            string url = "https://portunus.azurewebsites.net/api" + @"/pings/PostPing";

            Ping ping = new Ping();
            ping.User_Fk = payload.OwnerUserId;
            ping.Time = DateTime.Now;

            using (HttpClientHandler ClientHandler = new HttpClientHandler())
            {
                ClientHandler.AllowAutoRedirect = true;
                ClientHandler.UseDefaultCredentials = true;


                using (HttpClient Client = new HttpClient(ClientHandler))
                {
                    var myContent = JsonConvert.SerializeObject(ping);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                    await Client.PostAsync(url, byteContent);

                }
            }

            return await Task.FromResult(true);
        }
    }   

}
