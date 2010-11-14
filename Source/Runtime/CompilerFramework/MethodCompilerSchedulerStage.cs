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

using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Schedules compilation of types/methods.
	/// </summary>
	public class MethodCompilerSchedulerStage : BaseAssemblyCompilerStage, IAssemblyCompilerStage, ICompilationSchedulerStage, IPipelineStage
	{
		private readonly Queue<RuntimeMethod> methodQueue;

		private readonly Queue<RuntimeType> typeQueue;

		#region IPipelineStage

		string IPipelineStage.Name { get { return @"MethodCompilerSchedulerStage"; } }

		#endregion // IPipelineStage

		public MethodCompilerSchedulerStage()
		{
			methodQueue = new Queue<RuntimeMethod>();
			typeQueue = new Queue<RuntimeType>();
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

			Console.WriteLine(@"Compiling " + type.FullName);
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
			Console.WriteLine(@"Compiling " + method.ToString());
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

		public void ScheduleTypeForCompilation(RuntimeType type)
		{
			if (type == null)
				throw new ArgumentNullException(@"type");

			if (type.IsCompiled)
			{
				return;
			}

			if (!type.IsGeneric)
			{
				Console.WriteLine(@"Scheduling {0}", type.FullName);
				Debug.WriteLine(String.Format(@"Scheduling {0}", type.FullName));

				typeQueue.Enqueue(type);
				type.IsCompiled = true;
			}
		}

		public void ScheduleMethodForCompilation(RuntimeMethod method)
		{
			if (method == null)
				throw new ArgumentNullException(@"method");

			if (!method.IsGeneric)
			{
				Console.WriteLine(@"Scheduling {1}.{0}", method.Name, method.DeclaringType.FullName);
				Debug.WriteLine(String.Format(@"Scheduling {1}.{0}", method.Name, method.DeclaringType.FullName));

				methodQueue.Enqueue(method);
			}
		}
	}
}
