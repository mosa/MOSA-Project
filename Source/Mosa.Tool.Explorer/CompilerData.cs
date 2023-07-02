// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Framework.Trace;

namespace Mosa.Tool.Explorer;

public class CompilerData
{
	public readonly Dictionary<string, List<string>> Logs = new Dictionary<string, List<string>>();
	public readonly List<string> LogSections = new List<string>();

	public Stopwatch Stopwatch = new Stopwatch();

	public bool DirtyLog = true;
	public bool DirtyLogSections = true;

	public void ClearAllLogs()
	{
		lock (Logs)
		{
			Logs.Clear();
			LogSections.Clear();
		}

		UpdateLog("Compiler", null);
	}

	public void UpdateLog(string section, List<string> lines, bool dirty)
	{
		lock (Logs)
		{
			if (!Logs.TryGetValue(section, out List<string> log))
			{
				log = new List<string>(100);
				Logs.Add(section, log);
				LogSections.Add(section);
				DirtyLogSections = true;
			}

			lock (log)
			{
				log.AddRange(lines);
			}

			DirtyLog = dirty;
		}
	}

	private void UpdateLog(string section, string line)
	{
		lock (Logs)
		{
			if (!Logs.TryGetValue(section, out List<string> log))
			{
				log = new List<string>(100);
				Logs.Add(section, log);
				LogSections.Add(section);
				DirtyLogSections = true;
			}

			lock (log)
			{
				log.Add(line);
			}

			DirtyLog = true;
		}
	}

	public List<string> GetLog(string section)
	{
		lock (Logs)
		{
			if (Logs.TryGetValue(section, out List<string> log))
				return log;

			return null;
		}
	}

	public void AddTraceEvent(CompilerEvent compilerEvent, string message, int threadID)
	{
		if (compilerEvent == CompilerEvent.Counter)
		{
			UpdateLog("Counters", message);
			return;
		}

		var part = string.IsNullOrWhiteSpace(message) ? string.Empty : ": " + message;
		var msg = $"{compilerEvent.ToText()}{part}";

		var timelog = $"{Stopwatch.Elapsed.TotalSeconds:00.00} | [{threadID}] {msg}";

		if (compilerEvent == CompilerEvent.Error)
		{
			UpdateLog("Error", msg);
			UpdateLog("Compiler", timelog);
		}
		if (compilerEvent == CompilerEvent.Exception)
		{
			UpdateLog("Exception", msg);
			UpdateLog("Compiler", timelog);
		}
		else
		{
			UpdateLog("Compiler", timelog);
		}
	}

	public void SortLog(string section)
	{
		lock (Logs)
		{
			if (Logs.ContainsKey(section))
			{
				var lines = Logs[section];
				lines.Sort();
				Logs[section] = lines;
			}
		}
	}
}
