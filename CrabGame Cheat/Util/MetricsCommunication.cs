using Il2CppSystem.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SteamworksNative;
using System;

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
                Thread.Sleep(2 * 60
                    //* 1000
                    );
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

                JObject response = SendGet(request, "init");

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

                JObject response = SendGet(request, "heartbeat");

                if (!response.GetValue("success").ToObject<bool>())
                {
                    CheatLog.Warning("Heartbeat failed with message: \"" + response.GetValue("message").ToObject<string>() + "\"");

                    SendConnect();
                }
            }
        }

        private static JObject SendGet(JObject json, string url)
        {
            try
            {
                return JObject.Parse(SendGet(json.ToString(Formatting.Indented), url));

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

        private static string SendGet(string json, string url)
        {
            throw new NotImplementedException();
            /*
            var client = new RestClient(BaseUrl + url)
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.GET);
            request.AddParameter("application/json", json, ParameterType.GetOrPost);

            IRestResponse response = client.Execute(request);

            CheatLog.Warning("[" + response.StatusCode + "]" + response.Content);

            if (!response.IsSuccessful)
                throw response.ErrorException;

            return response.Content;*/
        }

    }
}
