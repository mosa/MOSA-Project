// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.Launcher
{
	public interface IBuilderEvent
	{
		void NewStatus(string status);

		void UpdateProgress(int total, int at);
	}
}
