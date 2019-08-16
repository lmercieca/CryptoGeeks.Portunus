using CryptoGeeks.Portunus.CommunicationFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoGeeks.Portunus.Comm
{
    public partial class Ping
    {
        public int Id { get; set; }
        public int User_Fk { get; set; }
        public System.DateTime Time { get; set; }
        public string SourceIp { get; set; }

        public virtual User User { get; set; }
    }

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.Pings = new HashSet<Ping>();
        }

        public int Id { get; set; }
        public string DisplayName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ping> Pings { get; set; }
    }

    public partial class UserStatusCompact
    {
        public int Id { get; set; }
        public Nullable<short> Status { get; set; }
        public string SourceIp { get; set; }
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
            ping.SourceIp = payload.FromIp;

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

        public async Task<List<UserStatusCompact>> GetUsersConnection(CancellationToken cancellationToken)
        {
            string BaseUrl = "https://portunus.azurewebsites.net/api";
            string Url = BaseUrl + "/UserStatusCompact/GetUserStatusCompacts";

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, Url))
            using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                    return DeserializeJsonFromStream<List<UserStatusCompact>>(stream);

                var content = await StreamToStringAsync(stream);
            }

            return await Task.FromResult(new List<UserStatusCompact>() { });
        }


        private static T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return default(T);

            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                var js = new JsonSerializer();
                var searchResult = js.Deserialize<T>(jtr);
                return searchResult;
            }
        }

        private static async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
                using (var sr = new StreamReader(stream))
                    content = await sr.ReadToEndAsync();

            return content;
        }


    }

}
