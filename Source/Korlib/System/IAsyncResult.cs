/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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