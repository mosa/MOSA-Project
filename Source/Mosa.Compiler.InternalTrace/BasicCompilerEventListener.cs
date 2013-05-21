﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Diagnostics;

namespace Mosa.Compiler.InternalTrace
{
	public class BasicCompilerEventListener : ICompilerEventListener
	{
		public bool DebugOutput = false;
		public bool ConsoleOutput = true;
		public bool Quiet = true;

		protected string supressed = null;

		protected void DisplayCompilingMethod(string info)
		{
			if (string.IsNullOrEmpty(info))
				return;

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write(@"[Compiling]  ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(info);
		}

		void ICompilerEventListener.SubmitTraceEvent(CompilerEvent compilerStage, string info)
		{
			if (DebugOutput)
				Debug.WriteLine(compilerStage.ToString() + ": " + info);

			if (!ConsoleOutput)
				return;

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
	}
}