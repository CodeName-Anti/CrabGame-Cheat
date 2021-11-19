using JNNJMods.CrabGameCheat.Translators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SteamworksNative;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace JNNJMods.CrabGameCheat.Util
{
    public class MetricsCommunication
    {
        private const string BaseUrl = "https://crabmod.com/api/v1/incoming/";

        public bool Connected { get; private set; }

        private bool running;

        public void Start()
        {
            running = true;
            SendConnect();

            HeartBeat();
        }

        private async void HeartBeat()
        {
            while (running)
            {
                if (!Connected)
                {
                    SendConnect();
                }

                SendHeartBeat();
                await Task.Delay(2 * 60 * 1000);
            }
        }

        public void Stop()
        {
            running = false;
        }

        private void SendConnect()
        {
            if (!Connected)
            {
                JObject request = new JObject
                {
                    ["steamID"] = SteamUser.GetSteamID().m_SteamID.ToString()
                };

                JObject response = SendPost(request, "init");

                Connected = response.GetValue("success").ToObject<bool>();

                if (!Connected)
                {
                    string message = response.GetValue("message").ToObject<string>();

                    if (message.Equals("This user is already init"))
                    {
                        Connected = true;
                        return;
                    }

                    CheatLog.Warning("Server connect failed with message: \"" + message + "\"");
                }
            }
        }

        public void SendHeartBeat()
        {
            SendHeartBeat(Instances.GameManager == null ? 0 : 1);
        }

        public void SendHeartBeat(int status)
        {
            if (Connected)
            {
                JObject request = new JObject
                {
                    ["steamID"] = SteamUser.GetSteamID().m_SteamID.ToString(),
                    ["status"] = status.ToString()
                };

                JObject response = SendPost(request, "heartbeat");

                if (!response.GetValue("success").ToObject<bool>())
                {
                    CheatLog.Warning("Heartbeat failed with message: \"" + response.GetValue("message").ToObject<string>() + "\"");

                    SendConnect();
                }
            }
        }

        private static JObject SendPost(JObject json, string url)
        {
            try
            {
                return JObject.Parse(SendPost(json.ToString(Formatting.Indented), url));
            }
            catch (Exception e)
            {
                CheatLog.Warning("POST Request failed(THIS IS NOT AN ERROR) Code: " + e.ToString());

                if (e is WebException)
                {
                    WebException ex = e as WebException;

                    if (ex.Message.Contains("Bad Request") || ex.Message.Contains("400"))
                    {
                        return new JObject
                        {
                            ["success"] = false,
                            ["message"] = "HTTP request error!"
                        };
                    }

                }

                return new JObject
                {
                    ["success"] = false,
                    ["message"] = "HTTP request error!"
                };
            }
        }

        private static string SendPost(string json, string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(BaseUrl + url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }

    }
}
