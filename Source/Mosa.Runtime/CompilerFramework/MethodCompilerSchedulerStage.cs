/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.InternalLog;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Schedules compilation of types/methods.
	/// </summary>
	public class MethodCompilerSchedulerStage : BaseAssemblyCompilerStage, IAssemblyCompilerStage, ICompilationSchedulerStage, IPipelineStage
	{
		private readonly Queue<RuntimeMethod> methodQueue;

		private readonly Queue<RuntimeType> typeQueue;

		private readonly Dictionary<RuntimeType, RuntimeType> compiled;

		#region IPipelineStage

		string IPipelineStage.Name { get { return @"MethodCompilerSchedulerStage"; } }

		#endregion // IPipelineStage

		public MethodCompilerSchedulerStage()
		{
			methodQueue = new Queue<RuntimeMethod>();
			typeQueue = new Queue<RuntimeType>();
			compiled = new Dictionary<RuntimeType, RuntimeType>();
		}

		#region IAssemblyCompilerStage members

		void IAssemblyCompilerStage.Run()
		{
			while (typeQueue.Count != 0)
			{
				RuntimeType type = typeQueue.Dequeue();
				CompileType(type);
			}

			CompilePendingMethods();
		}

		#endregion // IAssemblyCompilerStage members

		private void CompileType(RuntimeType type)
		{
			if (type.IsDelegate)
			{
				Console.WriteLine(@"Skipping delegate type " + type);
				return;
			}

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write(@"[Compiling]  ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(type.FullName);
			Debug.WriteLine(@"Compiling " + type.FullName);
			foreach (RuntimeMethod method in type.Methods)
			{
				if (method.IsGeneric)
				{
					Debug.WriteLine("Skipping generic method: " + type + "." + method.Name);
					Debug.WriteLine("Generic method will not be available in compiled image.");
					continue;
				}

				if (method.IsNative)
				{
					Debug.WriteLine("Skipping native method: " + type + "." + method.Name);
					Debug.WriteLine("Method will not be available in compiled image.");
					continue;
				}

				ScheduleMethodForCompilation(method);
			}
			
			CompilePendingMethods();
		}

		private void CompilePendingMethods()
		{
			while (methodQueue.Count > 0)
			{
				RuntimeMethod method = methodQueue.Dequeue();
				CompileMethod(method);
			}
		}

		private void CompileMethod(RuntimeMethod method)
		{
			compiler.InternalLog.CompilerStatusListener.NotifyCompilerStatus(CompilerStage.CompilingMethod, method.ToString());

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write(@"[Compiling]  ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(method.ToString());
			Debug.WriteLine(@"Compiling " + method.ToString());

			using (IMethodCompiler mc = compiler.CreateMethodCompiler(this, method.DeclaringType, method))
			{
				mc.Compile();

				//try
				//{
				//    mc.Compile();
				//}
				//catch (Exception e)
				//{
				//    HandleCompilationException(e);
				//    throw;
				//}
			}
		}

		protected virtual void HandleCompilationException(Exception e)
		{
		}

		void ICompilationSchedulerStage.ScheduleTypeForCompilation(RuntimeType type)
		{
			ScheduleTypeForCompilation(type);
		}

		public void ScheduleTypeForCompilation(RuntimeType type)
		{
			if (type == null)
				throw new ArgumentNullException(@"type");

			if (compiled.ContainsKey(type))
				return;

			if (!type.IsGeneric)
			{
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.Write(@"[Scheduling] ");
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine(type.FullName);
				Debug.WriteLine(String.Format(@"Scheduling {0}", type.FullName));

				typeQueue.Enqueue(type);
				compiled.Add(type, type);
			}
		}

		protected void ScheduleMethodForCompilation(RuntimeMethod method)
		{
			if (method == null)
				throw new ArgumentNullException(@"method");

			if (!method.IsGeneric)
			{
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.Write(@"[Scheduling] ");
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine("{1}.{0}", method.Name, method.DeclaringType.FullName);
				Debug.WriteLine(String.Format(@"Scheduling {1}.{0}", method.Name, method.DeclaringType.FullName));

				methodQueue.Enqueue(method);
			}
		}
	}
}
