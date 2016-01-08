// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.AppSystem
{
	/// <summary>
	///
	/// </summary>
	public interface IConsoleApp
	{
		AppConsole Console { get; }

		/// <summary>
		/// Starts this application
		/// </summary>
		/// <returns>
		/// error code
		/// </returns>
		int Start(string parameters);

		AppManager AppManager { get; set; }
	}
}
