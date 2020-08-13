using System.Diagnostics;
using Prism.Events;

namespace TensorflowInstallationScript.CommandLineWrapper
{
	public class ProcessCompletedEvent : PubSubEvent<Process>
	{
	}
}