﻿using System.Diagnostics;

namespace System.System.Collections
{
	//TODO [DebuggerDisplay("{value}", Name = "[{key}]", Type = "")]
	internal class KeyValuePairs
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object key;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object value;

		public KeyValuePairs(object key, object value)
		{
			this.value = value;
			this.key = key;
		}

		public object Key
		{
			get { return key; }
		}

		public object Value
		{
			get { return value; }
		}
	}
}
