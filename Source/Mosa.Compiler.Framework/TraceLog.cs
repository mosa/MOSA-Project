// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Compiler.Framework.Trace
{
	public enum TraceType { MethodInstructions, MethodDebug, MethodCounters, GlobalDebug }

	public sealed class TraceLog
	{
		public TraceType Type { get; }

		public MosaMethod Method { get; }

		public string Stage { get; }

		public string Section { get; }

		public int Version { get; }

		public List<string> Lines { get; }

		private TraceLog(TraceType type)
		{
			Type = type;
			Lines = new List<string>();
		}

		public TraceLog(TraceType type, MosaMethod method, string stage, int version = 0)
			: this(type)
		{
			Stage = stage;
			Method = method;
			Version = version;
		}

		public TraceLog(TraceType type, MosaMethod method, string stage, string section, int version = 0)
			: this(type, method, stage)
		{
			Section = section;
			Version = version;
		}

		public void Log()
		{
			Log(string.Empty);
		}

		public void Log(string line)
		{
			Lines.Add(line);
		}

		public void Log(IEnumerable<string> lines)
		{
			foreach (var line in lines)
			{
				Lines.Add(line);
			}
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			foreach (var line in Lines)
			{
				sb.AppendLine(line);
			}

			return sb.ToString();
		}
	}
}
