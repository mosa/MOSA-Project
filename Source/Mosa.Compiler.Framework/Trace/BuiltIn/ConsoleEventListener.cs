// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System;

namespace Mosa.Compiler.Framework.Trace.BuiltIn
{
	public sealed class ConsoleEventListener : ITraceListener
	{
		public bool Quiet = true;

		private string supressed = null;

		private void DisplayCompilingMethod(string info)
		{
			if (string.IsNullOrEmpty(info))
				return;

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write("[Compiling]  ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(info);
		}

		private void TraceEvent(CompilerEvent compilerStage, string info)
		{
			switch (compilerStage)
			{
				case CompilerEvent.MethodCompileStart:
					{
						if (Quiet)
						{
							supressed = info;
							break;
						}
						DisplayCompilingMethod(info);
						break;
					}

				case CompilerEvent.MethodScheduled:
					{
						if (Quiet) break;
						Console.ForegroundColor = ConsoleColor.Blue;
						Console.Write("[Scheduling]  ");
						Console.ForegroundColor = ConsoleColor.White;
						Console.WriteLine(info);
						break;
					}

				case CompilerEvent.Exception:
					{
						if (Quiet && !string.IsNullOrEmpty(supressed))
						{
							DisplayCompilingMethod(supressed);
							supressed = null;
						}

						Console.ForegroundColor = ConsoleColor.Red;
						Console.Write("[Exception]  ");
						Console.ForegroundColor = ConsoleColor.White;
						Console.WriteLine(info);
						break;
					}

				case CompilerEvent.Error:
					{
						if (Quiet && !string.IsNullOrEmpty(supressed))
						{
							DisplayCompilingMethod(supressed);
							supressed = null;
						}

						Console.ForegroundColor = ConsoleColor.Red;
						Console.Write("[Error]  ");
						Console.ForegroundColor = ConsoleColor.White;
						Console.WriteLine(info);
						break;
					}

				case CompilerEvent.Warning:
					{
						if (Quiet && !string.IsNullOrEmpty(supressed))
						{
							DisplayCompilingMethod(supressed);
							supressed = null;
						}

						Console.ForegroundColor = ConsoleColor.Yellow;
						Console.Write("[Warning]  ");
						Console.ForegroundColor = ConsoleColor.White;
						Console.WriteLine(info);
						break;
					}

				default: break;
			}
		}

		void ITraceListener.OnCompilerEvent(CompilerEvent compilerEvent, string message, int threadID)
		{
			TraceEvent(compilerEvent, message);
		}

		void ITraceListener.OnProgress(int totalMethods, int completedMethods)
		{
		}

		void ITraceListener.OnTraceLog(TraceLog traceLog)
		{
		}

		void ITraceListener.OnMethodCompiled(MosaMethod method)
		{
		}
	}
}
