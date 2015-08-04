// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	///
	/// </summary>
	public interface IAsyncResult
	{
		object AsyncState
		{
			get;
		}

		WaitHandle AsyncWaitHandle
		{
			get;
		}

		bool CompletedSynchronously
		{
			get;
		}

		bool IsCompleted
		{
			get;
		}
	}
}
