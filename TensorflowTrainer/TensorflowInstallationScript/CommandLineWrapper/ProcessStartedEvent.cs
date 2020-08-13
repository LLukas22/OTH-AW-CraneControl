using System.Diagnostics;
using Prism.Events;

namespace TensorflowInstallationScript.CommandLineWrapper
{
	public class ProcessStartedEvent : PubSubEvent<Process>
	{
	}
}