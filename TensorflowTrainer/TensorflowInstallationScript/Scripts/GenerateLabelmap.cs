using System;
using System.Collections.Generic;

namespace TensorflowInstallationScript.Scripts
{
	public class GenerateLabelmap : BaseScript
	{
		public GenerateLabelmap(List<string> Objects)
		{
			ScriptName = "labelmap.pbtxt";

			foreach (var entity in Objects)
				Script += "item {\n" + $"\tid: {Objects.IndexOf(entity) + 1}{Environment.NewLine}" +
				          $"\tname: '{entity}'{Environment.NewLine}" + "}\n\n";
		}
	}
}