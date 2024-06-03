using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using ShellProgressBar;
using System.Diagnostics;
using System.Net.Http.Handlers;

namespace CrabGame_Cheat_Installer;

internal static class Utils
{
	internal static ProgressBarOptions BaseProgressOptions => new()
	{
		ForegroundColor = ConsoleColor.White,
		ForegroundColorDone = ConsoleColor.Green,
	};

	internal static ProgressBarOptions DownloadProgressOptions => new()
	{
		ForegroundColor = ConsoleColor.White,
		ForegroundColorDone = ConsoleColor.Green,
		ShowEstimatedDuration = true
	};

	// Zip Utils

	private static void HandleZipProgress(
		ProgressEventArgs args,
		string displayName,
		Stopwatch watch,
		ref ProgressBarBase progressBar,
		ProgressBarBase parentProgressBar)
	{
		if (progressBar == null)
		{
			string message = $"Extracting {displayName}";

			if (parentProgressBar != null)
				progressBar = parentProgressBar.Spawn((int)args.Target, message);
			else
				progressBar = new ProgressBar((int)args.Target, message, BaseProgressOptions);
		}

		if (args.Processed == 0)
			return;

		double speed = DownloadSpeed(args.Processed, watch);
		TimeSpan estimatedTimeRemaining = CalculateETA(args.Processed, args.Target, speed);

		progressBar.EstimatedDuration = estimatedTimeRemaining;
		progressBar.Tick((int)args.Processed);
	}

	internal static void Unzip(string zipFile, string outPath, string displayName, ProgressBarBase parentProgressBar = null)
	{
		FastZipEvents events = new();

		ProgressBarBase progressBar = null;
		Stopwatch watch = new();

		events.Progress += new ProgressHandler((sender, args) => HandleZipProgress(args, displayName, watch, ref progressBar, parentProgressBar));

		FastZip fZip = new(events);

		watch.Start();
		fZip.ExtractZip(zipFile, outPath, null);
		watch.Stop();

		parentProgressBar.Tick();
	}

	internal static async Task DownloadAndUnzip(string url, string outPath, string displayName, ProgressBarBase parent)
	{
		string tempFileName = Path.GetTempFileName();
		string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
		string tempFile = Path.Combine(tempPath, Path.GetFileName(tempFileName));

		Directory.CreateDirectory(tempPath);

		await DownloadFileAsync(url, tempFile, displayName, parent);

		await Task.Run(() => Unzip(tempFile, tempPath, displayName, parent));

		await CopyDirectoryAsync(tempPath, outPath);
	}

	// File Utils

	internal static void DeleteFile(string basePath, string file)
	{
		file = Path.Combine(basePath, file);

		if (File.Exists(file))
			File.Delete(file);
	}

	internal static void DeleteDirectory(string basePath, string directory, bool recursive)
	{
		directory = Path.Combine(basePath, directory);

		if (Directory.Exists(directory))
			Directory.Delete(directory, recursive);
	}

	internal static async Task CopyDirectoryAsync(string sourceDir, string destinationDir)
	{
		foreach (string dirPath in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
			Directory.CreateDirectory(dirPath.Replace(sourceDir, destinationDir));

		foreach (string filePath in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
			await CopyFileAsync(filePath, Path.Combine(destinationDir, filePath.Substring(sourceDir.Length + 1)));
	}

	internal static async Task CopyFileAsync(string sourceFile, string destinationFile)
	{
		using Stream source = File.Open(sourceFile, FileMode.Open);
		using Stream destination = File.Create(destinationFile);
		await source.CopyToAsync(destination);
	}

	// Http Utils

	internal static async Task<HttpResponseMessage> SendGetAsync(string url, HttpClient client = null)
	{
		client ??= new();

		HttpRequestMessage request = new(HttpMethod.Get, url);
		request.Headers.UserAgent.Clear();
		request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:93.0) Gecko/20100101 Firefox/93.0");

		return await client.SendAsync(request);
	}

	private static string FormatBytes(double bytes)
	{
		string[] sizes = { "B", "KB", "MB", "GB", "TB" };
		int order = 0;
		while (bytes >= 1024 && order < sizes.Length - 1)
		{
			order++;
			bytes /= 1024;
		}
		return $"{bytes:0.##} {sizes[order]}";
	}

	private static double DownloadSpeed(long bytesTransferred, Stopwatch watch)
	{
		return bytesTransferred / watch.Elapsed.TotalSeconds;
	}

	private static TimeSpan CalculateETA(long bytesTransferred, long totalBytes, double downloadSpeed)
	{
		return TimeSpan.FromSeconds((totalBytes - bytesTransferred) / downloadSpeed);
	}

	internal static async Task DownloadFileAsync(string url, string path, string displayName, ProgressBarBase parentProgressBar = null)
	{
		ProgressMessageHandler progressHandler = new(new HttpClientHandler());

		ProgressBarBase progressBar = null;

		Stopwatch watch = new();

		progressHandler.HttpReceiveProgress += (sender, args) =>
		{
			if (args.TotalBytes == null)
				return;

			if (progressBar == null || progressBar.CurrentTick == progressBar.MaxTicks)
			{
				string message = $"Downloading {displayName}";

				if (parentProgressBar != null)
					progressBar = parentProgressBar.Spawn((int)args.TotalBytes, message);
				else
					progressBar = new ProgressBar((int)args.TotalBytes, message, DownloadProgressOptions);
			}

			if (progressBar == null)
				return;

			if (args.BytesTransferred == 0)
				return;

			double downloadSpeed = DownloadSpeed(args.BytesTransferred, watch);
			TimeSpan estimatedTimeRemaining = CalculateETA(args.BytesTransferred, args.TotalBytes.Value, downloadSpeed);

			progressBar.EstimatedDuration = estimatedTimeRemaining;
			progressBar.Tick((int)args.BytesTransferred, $"Downloading {displayName} ({FormatBytes(downloadSpeed)}/s)");
		};

		HttpClient client = new(progressHandler);

		watch.Start();
		HttpResponseMessage response = await SendGetAsync(url, client);

		Stream stream = await response.Content.ReadAsStreamAsync();

		using FileStream file = new(path, FileMode.OpenOrCreate);
		await stream.CopyToAsync(file);

		await file.FlushAsync();
		file.Close();

		watch.Stop();

		parentProgressBar?.Tick();
	}

}
