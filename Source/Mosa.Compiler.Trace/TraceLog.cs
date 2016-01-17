// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Compiler.Trace
{
	public enum TraceType { InstructionList, DebugTrace };

	public class TraceLog
	{
		public TraceType Type { get; private set; }

		public MosaMethod Method { get; private set; }

		public string Stage { get; private set; }

		public string Section { get; private set; }

		public bool Active { get; internal set; }

		public List<string> Lines { get; private set; }

		protected TraceLog(TraceType type)
		{
			Type = type;
			Active = true;
			Lines = new List<string>();
		}

		public TraceLog(TraceType type, MosaMethod method, string stage, bool active)
			: this(type)
		{
			Stage = stage;
			Method = method;
			Active = active;
		}

		public TraceLog(TraceType type, MosaMethod method, string stage, string section, bool active)
			: this(type)
		{
			Stage = stage;
			Section = section;
			Method = method;
			Active = active;
		}

		public TraceLog(TraceType type, MosaMethod method, string stage, TraceFilter filter)
			: this(type)
		{
			Stage = stage;
			Method = method;
			Active = filter.IsMatch(Method, Stage);
		}

		public void Log()
		{
			Log(string.Empty);
		}

		public void Log(string line)
		{
			if (!Active)
				return;

			Lines.Add(line);
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
