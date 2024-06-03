using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Reflection;

namespace JNNJMods.CrabCheat.Util;

public static class UpdateChecker
{
	private static readonly bool init;
	public static bool UpdateAvailable;

	static UpdateChecker()
	{
		if (init)
			return;

		try
		{
			using HttpClient client = new();

			HttpRequestMessage request = new(HttpMethod.Get, Constants.ReleasesAPI);

			request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:93.0) Gecko/20100101 Firefox/93.0");

			// Some random user agent because with others it responds with 403
			string json = client.Send(request).Content.ReadAsStringAsync().GetAwaiter().GetResult();

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
		}
		catch (Exception)
		{
			CheatLog.Warning("Couldn't fetch Updates from GitHub!");
		}
	}

}
