using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Prism.Events;

namespace TensorflowInstallationScript.CommandLineWrapper
{
	public class CommandLineWrapper
	{
		private readonly IEventAggregator eventAggregator;

		private readonly ProcessStartInfo startinfo;


		public CommandLineWrapper(IEventAggregator eventAggregator)
		{
			this.eventAggregator = eventAggregator;
			startinfo = new ProcessStartInfo
			{
				WindowStyle = ProcessWindowStyle.Hidden,
				FileName = "cmd.exe",
				RedirectStandardInput = true,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true
			};
		}

		public async Task Execute(List<string> Commands)
		{
			await Task.Run(() =>
			{
				var process = new Process
				{
					StartInfo = startinfo
				};
				process.Start();
				eventAggregator.GetEvent<ProcessStartedEvent>().Publish(process);
				process.BeginOutputReadLine();
				process.BeginErrorReadLine();
				process.OutputDataReceived += ProcessOnOutputDataReceived;
				process.ErrorDataReceived += ProcessOnErrorDataReceived;
				var cmd = process.StandardInput;

				if (cmd.BaseStream.CanWrite)
					foreach (var command in Commands)
						cmd.WriteLine(command);


				process.WaitForExit();
				eventAggregator.GetEvent<ProcessCompletedEvent>().Publish(process);
			});
		}

		private async void ProcessOnErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			await Task.Run(() => { eventAggregator.GetEvent<ErrorChangedEvent>().Publish(e.Data); });
		}

		private async void ProcessOnOutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			await Task.Run(() => { eventAggregator.GetEvent<OutputChangedEvent>().Publish(e.Data); });
		}
	}
}