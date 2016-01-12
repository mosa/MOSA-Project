// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Runtime
{
	/// <summary>
	/// Holds information about a single stacktrace entry
	/// </summary>
	public struct SimpleStackTraceEntry
	{
		private string methodName;
		public unsafe MetadataMethodStruct* MethodDefinition;
		public uint Offset;

		unsafe public string MethodName
		{
			get
			{
				if (MethodDefinition == null)
					return null;
				if (methodName == null)
					methodName = Internal.GetMethodDefinitionName(MethodDefinition);
				return methodName;
			}
		}

		/// <summary>
		/// Returns a human readable text of this entry
		/// </summary>
		/// <returns></returns>
		unsafe public StringBuffer ToStringBuffer()
		{
			var buf = new StringBuffer();

			buf.Append("0x");
			buf.Append((uint)MethodDefinition->Method, "X");
			buf.Append("+0x");
			buf.Append(Offset, "X");
			buf.Append(" ");

			var idx = MethodName.IndexOf(' ') + 1; //Skip return type
			buf.Append(MethodName, idx);
			return buf;
		}

		/// <summary>
		/// Skip defines, if this entry should be displayed, or not.
		/// </summary>
		public bool Skip
		{
			get
			{
				{
					if (!Valid) return true;
					if (MethodName == null)
						return true;
					return MethodName.IndexOf("System.Void Mosa.Kernel.x86.Panic::") >= 0;
				}
			}
		}

		public bool Valid
		{
			get { return MethodName != null; }
		}
	}
}
