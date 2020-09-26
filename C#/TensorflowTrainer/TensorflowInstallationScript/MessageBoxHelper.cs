using System;
using System.Windows.Forms;

namespace TensorflowInstallationScript
{
	public static class MessageBoxHelper
	{
		public static void Info(string text)
		{
			MessageBox.Show(text, "Info", MessageBoxButtons.OK,
				MessageBoxIcon.Information);
		}

		public static void Error(Exception exception)
		{
			MessageBox.Show($"A Error ocurred: {exception.Message}", "Error", MessageBoxButtons.OK,
				MessageBoxIcon.Error);
		}
	}
}