/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.Compiler.Trace.BuiltIn
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
			Console.Write(@"[Compiling]  ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(info);
		}

		private void TraceEvent(CompilerEvent compilerStage, string info)
		{
			switch (compilerStage)
			{
				case CompilerEvent.CompilingMethod:
					{
						if (Quiet)
						{
							supressed = info;
							break;
						}
						DisplayCompilingMethod(info);
						break;
					}

				case CompilerEvent.CompilingType:
					{
						if (Quiet) break;
						Console.ForegroundColor = ConsoleColor.Yellow;
						Console.Write(@"[Compiling]  ");
						Console.ForegroundColor = ConsoleColor.White;
						Console.WriteLine(info);
						break;
					}

				case CompilerEvent.SchedulingType:
					{
						if (Quiet) break;
						Console.ForegroundColor = ConsoleColor.Blue;
						Console.Write(@"[Scheduling]  ");
						Console.ForegroundColor = ConsoleColor.White;
						Console.WriteLine(info);
						break;
					}

				case CompilerEvent.SchedulingMethod:
					{
						if (Quiet) break;
						Console.ForegroundColor = ConsoleColor.Blue;
						Console.Write(@"[Scheduling]  ");
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
						Console.Write(@"[Exception]  ");
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
						Console.Write(@"[Error]  ");
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
						Console.Write(@"[Warning]  ");
						Console.ForegroundColor = ConsoleColor.White;
						Console.WriteLine(info);
						break;
					}

				default: break;
			}
		}

		void ITraceListener.OnNewCompilerTraceEvent(CompilerEvent compilerStage, string info, int threadID)
		{
			TraceEvent(compilerStage, info);
		}

		void ITraceListener.OnUpdatedCompilerProgress(int totalMethods, int completedMethods)
		{
		}

		void ITraceListener.OnNewTraceLog(TraceLog traceLog)
		{
		}
	}
}
