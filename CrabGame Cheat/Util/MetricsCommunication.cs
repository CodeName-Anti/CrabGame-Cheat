using Il2CppSystem.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SteamworksNative;
using System;
using System.IO;
using System.Net;

namespace JNNJMods.CrabGameCheat.Util
{
    public class MetricsCommunication
    {
        private const string BaseUrl = "https://crabmod.com/api/v1/incoming/";

        public bool Connected { get; private set; }

        private bool running;

        private Thread heartbeatThread;

        public void Start()
        {
            running = true;
            SendConnect();

            if(heartbeatThread == null)
            {
                heartbeatThread = new Thread((ThreadStart)HeartbeatThread);
            }

            heartbeatThread.Start();

        }

        private void HeartbeatThread()
        {
            while (running)
            {
                Thread.Sleep(2 * 60 * 1000);
                if (!Connected)
                {
                    SendConnect();
                }

                SendHeartBeat();
            }
        }

        public void Stop()
        {
            running = false;
        }

        private void SendConnect()
        {
            if(!Connected)
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
            SendHeartBeat(GameManager.Instance == null ? 0 : 1);
        }

        public void SendHeartBeat(int status)
        {
            if(Connected)
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

            } catch(Exception e)
            {
                CheatLog.Error("GET Request failed Error: " + e.ToString());

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
