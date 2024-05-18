using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly struct YieldAwaitable
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public readonly struct YieldAwaiter : ICriticalNotifyCompletion, INotifyCompletion
	{
		public bool IsCompleted
		{
			get
			{
				throw null;
			}
		}

		public void GetResult()
		{
		}

		public void OnCompleted(Action continuation)
		{
		}

		public void UnsafeOnCompleted(Action continuation)
		{
		}
	}

	public YieldAwaiter GetAwaiter()
	{
		throw null;
	}
}
