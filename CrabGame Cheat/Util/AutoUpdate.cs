﻿using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Reflection;

namespace JNNJMods.CrabGameCheat.Util
{
    public static class AutoUpdate
    {
        private static bool init;
        public static bool UpdateAvailable;

        public static void Update()
        {
            string downloadPath = DownloadUpdate();



        }

        private static string DownloadUpdate()
        {
            if(UpdateAvailable)
            {
                using var client = new WebClient();
                //Some random user agent because with others it responds with 403
                client.Headers.Add("User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:93.0) Gecko/20100101 Firefox/93.0");

                string downloadPath = Path.GetTempFileName();

                client.DownloadFile(Constants.DownloadURL, downloadPath);

                return downloadPath;
            }

            return null;
        }

        static AutoUpdate()
        {
            if(!init)
            {
                try
                {
                    using var client = new WebClient();
                    // Some random user agent because with others it responds with 403
                    client.Headers.Add("User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:93.0) Gecko/20100101 Firefox/93.0");
                    string json = client.DownloadString(Constants.ReleasesAPI);

                    JArray jArr = JArray.Parse(json);

                    string stringVersion = jArr[0].ToObject<JObject>().GetValue("tag_name").ToObject<string>();

                    // Compare GitHub and Local Version
                    Version git = new(stringVersion);
                    Version current = Assembly.GetExecutingAssembly().GetName().Version;

                    int result = current.CompareTo(git);

                    if (result < 0)
                    {
                        UpdateAvailable = true;
                    }

                    init = true;
                } catch(Exception)
                {
                    CheatLog.Warning("Couldn't fetch Updates from GitHub!");
                }
            }
        }

    }
}
