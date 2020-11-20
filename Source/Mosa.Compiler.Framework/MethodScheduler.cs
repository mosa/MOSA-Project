// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Channels;

namespace Mosa.Compiler.Framework
{
	public class CompilerResult
	{
		public MosaMethod Method { get; set; }
		public MosaMethod CompiledMethod { get; set; }
		public string Result { get; set; }
		public int Attemps { get; set; }
	}

	/// <summary>
	/// Schedules compilation of types/methods.
	/// </summary>
	public sealed class MethodScheduler
	{
		#region Data Members

		public Compiler Compiler;

		private readonly Channel<CompilerResult> _channel;
		private int _totalMethods;
		private int _totalQueued;

		#endregion Data Members

		#region Properties

		public int PassCount { get; }

		/// <summary>
		/// Gets the total methods.
		/// </summary>
		/// <value>
		/// The total methods.
		/// </value>
		public int TotalMethods => _totalMethods;

		/// <summary>
		/// Gets the queued methods.
		/// </summary>
		/// <value>
		/// The queued methods.
		/// </value>
		public int TotalQueuedMethods => _totalQueued;

		#endregion Properties

		public MethodScheduler(Compiler compiler)
		{
			_channel = Channel.CreateUnbounded<CompilerResult>(new UnboundedChannelOptions { SingleWriter = false, SingleReader = false });
			Compiler = compiler;
			PassCount = 0;
		}

		public void ScheduleAll(TypeSystem typeSystem)
		{
			foreach (var type in typeSystem.AllTypes)
			{
				Schedule(type);
			}
		}

		public bool IsCompilable(MosaType type)
		{
			if (type.IsModule)
				return false;

			if (type.IsInterface)
				return false;

			if (type.HasOpenGenericParams || type.IsPointer)
				return false;

			return true;
		}

		public bool IsCompilable(MosaMethod method)
		{
			if (method.IsAbstract && !method.HasImplementation)
				return false;

			if (method.HasOpenGenericParams)
				return false;

			if (method.IsCompilerGenerated)
				return false;

			return true;
		}

		public void Schedule(MosaType type)
		{
			if (!IsCompilable(type))
				return;

			foreach (var method in type.Methods)
			{
				Schedule(method);
			}
		}

		public void Schedule(MosaMethod method)
		{
			if (!IsCompilable(method))
				return;

			AddToQueue(method);
		}

		private void AddToQueue(MosaMethod method)
		{
			AddToQueue(new CompilerResult { Method = method });
		}

		public void AddToQueue(CompilerResult compilerResult)
		{
			if (compilerResult.Attemps == 0) //Count the method only on first try
				Interlocked.Increment(ref _totalMethods);

			if (compilerResult.Attemps++ > 5)
				compilerResult.Result = "Method exceeded compile attemps: " + compilerResult.Result;
			else
			{
				Interlocked.Increment(ref _totalQueued);
				if (!_channel.Writer.TryWrite(compilerResult))
				{
					Debug.Assert(false);
				}
			}
		}

		public CompilerResult GetMethodToCompile()
		{
			return GetScheduledMethod();
		}

		private CompilerResult GetScheduledMethod()
		{
			if (_channel.Reader.TryRead(out var result))
			{
				Interlocked.Decrement(ref _totalQueued);
			};

			return result;
		}

		public void AddToRecompileQueue(HashSet<MosaMethod> methods)
		{
			foreach (var method in methods)
				AddToQueue(method);
		}

		public void AddToRecompileQueue(MosaMethod method)
		{
			AddToQueue(method);
		}
	}
}
