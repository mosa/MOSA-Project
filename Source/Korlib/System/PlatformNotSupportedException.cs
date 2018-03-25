// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	[Serializable]
	public class PlatformNotSupportedException : NotSupportedException
	{
		public PlatformNotSupportedException()
		{
		}

		public PlatformNotSupportedException(String message)
			: base(message)
		{
		}
	}
}
