﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Tool.Explorer.Avalonia;

public class CompilerData
{
	public readonly Dictionary<string, List<string>> Logs = new Dictionary<string, List<string>>();
	public readonly List<string> LogSections = new List<string>();

	public readonly Stopwatch Stopwatch = new Stopwatch();

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
				log.AddRange(lines);

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
				log.Add(line);

			DirtyLog = true;
		}
	}

	public List<string> GetLog(string section)
	{
		lock (Logs)
			return Logs.GetValueOrDefault(section);
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

		var timeLog = $"{Stopwatch.Elapsed.TotalSeconds:00.00} | [{threadID}] {msg}";

		if (compilerEvent == CompilerEvent.Error)
		{
			UpdateLog("Error", msg);
			UpdateLog("Compiler", timeLog);
		}
		if (compilerEvent == CompilerEvent.Exception)
		{
			UpdateLog("Exception", msg);
			UpdateLog("Compiler", timeLog);
		}
		else
		{
			UpdateLog("Compiler", timeLog);
		}
	}

	public void SortLog(string section)
	{
		lock (Logs)
		{
			if (!Logs.TryGetValue(section, out List<string> lines))
				return;

			lines.Sort();
			Logs[section] = lines;
		}
	}
}
