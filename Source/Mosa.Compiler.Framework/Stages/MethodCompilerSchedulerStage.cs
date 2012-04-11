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
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Schedules compilation of types/methods.
	/// </summary>
	public class MethodCompilerSchedulerStage : BaseAssemblyCompilerStage, IAssemblyCompilerStage, ICompilationSchedulerStage, IPipelineStage
	{

		#region Data Members

		private readonly Queue<RuntimeMethod> methodQueue;

		private readonly Queue<RuntimeType> typeQueue;

		private readonly Dictionary<RuntimeType, RuntimeType> compiled;

		private readonly HashSet<string> alreadyCompiled;

		#endregion // Data Members

		public MethodCompilerSchedulerStage()
		{
			methodQueue = new Queue<RuntimeMethod>();
			typeQueue = new Queue<RuntimeType>();
			compiled = new Dictionary<RuntimeType, RuntimeType>();
			alreadyCompiled = new HashSet<string>();
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

		#region ICompilationSchedulerStage members

		void ICompilationSchedulerStage.ScheduleMethodForCompilation(RuntimeMethod method)
		{
			if (method == null)
				throw new ArgumentNullException(@"method");

			if (!method.IsGeneric)
			{
				Trace(CompilerEvent.SchedulingMethod, method.ToString());
				methodQueue.Enqueue(method);
			}
		}

		void ICompilationSchedulerStage.ScheduleTypeForCompilation(RuntimeType type)
		{
			if (type == null)
				throw new ArgumentNullException(@"type");

			if (compiled.ContainsKey(type) || alreadyCompiled.Contains(type.ToString()))
				return;

			if (!type.IsGeneric)
			{
				Trace(CompilerEvent.SchedulingType, type.FullName);

				typeQueue.Enqueue(type);
				if (compiled.ContainsKey(type))
					compiled.Remove(type);
				compiled.Add(type, type);
				alreadyCompiled.Add(type.ToString());
			}
		}

		#endregion // ICompilationSchedulerStage members

		private void CompileType(RuntimeType type)
		{
			Trace(CompilerEvent.CompilingMethod, type.FullName);

			if (type.ContainsOpenGenericParameters)
				return;

			//if (type.IsDelegate && type.FullName != "System.Delegate" && type.FullName != "System.MulticastDelegate")
			//{
			//    typeSystem.DelegateTypePatcher.PatchType(type);
			//}

			foreach (RuntimeMethod method in type.Methods)
			{
				if (method.IsGeneric)
				{
					Trace(CompilerEvent.DebugInfo, "Skipping generic method: " + type + "." + method.Name);
					Trace(CompilerEvent.DebugInfo, "Generic method will not be available in compiled image.");
					continue;
				}

				if (method.IsNative)
				{
					Trace(CompilerEvent.DebugInfo, "Skipping native method: " + type + "." + method.Name);
					Trace(CompilerEvent.DebugInfo, "Method will not be available in compiled image.");
					continue;
				}

				((ICompilationSchedulerStage)this).ScheduleMethodForCompilation(method);
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
			Trace(CompilerEvent.CompilingMethod, method.ToString());

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
			// TODO
		}

	}
}
