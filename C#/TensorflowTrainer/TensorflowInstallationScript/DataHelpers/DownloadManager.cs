using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Prism.Events;

namespace TensorflowInstallationScript.DataHelpers
{
	public class DownloadManager
	{
		private Label activeLabel;
		private bool downloadActive;
		private IEventAggregator eventAggregator;

		private readonly Dictionary<string, DownloadManagerFileInfo> filesToDownload =
			new Dictionary<string, DownloadManagerFileInfo>();

		private readonly WebClient webClient = new WebClient();

		public DownloadManager(IEventAggregator eventAggregator)
		{
			this.eventAggregator = eventAggregator;
			webClient.DownloadProgressChanged += OnDownloadProgressChanged;
			webClient.DownloadFileCompleted += WebClientOnDownloadFileCompleted;
			Directory.CreateDirectory(@"Downloads");
		}

		private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			activeLabel.Text = $"Status: {e.ProgressPercentage}%";
		}

		private void WebClientOnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			activeLabel.Text = "Status: OK";
			downloadActive = false;
		}


		public void Download(string filename)
		{
			if (!downloadActive)
				if (filesToDownload.ContainsKey(filename))
				{
					activeLabel = filesToDownload[filename].Label;

					if (File.Exists(filename))
					{
						activeLabel.Text = "Status: OK";
						return;
					}

					downloadActive = true;
					webClient.DownloadFileAsync(filesToDownload[filename].Url, filesToDownload[filename].Name);
				}
		}

		public void RegisterFile(string filename, Uri url, Label label)
		{
			if (!filesToDownload.ContainsKey(filename))
				filesToDownload.Add(filename, new DownloadManagerFileInfo
				{
					Label = label,
					Name = filename,
					Url = url
				});
			else
				filesToDownload[filename] = new DownloadManagerFileInfo
				{
					Label = label,
					Name = filename,
					Url = url
				};
		}

		public void CheckFile(string filename)
		{
			if (filesToDownload.ContainsKey(filename))
				if (File.Exists(filename))
					filesToDownload[filename].Label.Text = "Status: OK";
		}
	}
}