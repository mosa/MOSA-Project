/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System.Threading;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Schedules compilation of types/methods.
	/// </summary>
	public class MethodCompilerSchedulerStage : BaseCompilerStage
	{
		private int queuedMethods;
		private object methodLock = new object();
		private object traceLock = new object();

		protected override void Run()
		{
			ManualResetEvent doneEvent = new ManualResetEvent(false);
			ThreadPool.SetMinThreads(4, 4);
			ThreadPool.SetMaxThreads(8, 8);
			queuedMethods = CompilationScheduler.TotalMethods;

			for (int i = 0; i < CompilationScheduler.TotalMethods; i++)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(CompileMethod), doneEvent);
			}

			doneEvent.WaitOne();
			doneEvent.Dispose();
		}

		private void CompileMethod(object doneEventObject)
		{
			MosaTypeSystem.MosaMethod method = null;

			lock (methodLock)
			{
				method = CompilationScheduler.GetMethodToCompile();
			}

			//lock (traceLock)
			//{
				Compiler.CompileMethod(method, null, null);

				CompilerTrace.CompilerEventListener.SubmitMethodStatus(
					CompilationScheduler.TotalMethods,
					CompilationScheduler.TotalMethods - CompilationScheduler.TotalQueuedMethods
				);
			//}

			if (Interlocked.Decrement(ref queuedMethods) == 0)
			{
				ManualResetEvent doneEvent = (ManualResetEvent)doneEventObject;
				doneEvent.Set();
			}
		}
	}
}