// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Compiler Metho dData
	/// </summary>
	public sealed class MethodData
	{
		#region Properties

		public Counters Counters { get; }

		public MosaMethod Method { get; }

		public bool IsCompiled { get; set; }

		public bool IsLinkerGenerated { get; set; }

		public bool HasProtectedRegions { get; set; }

		public bool Inlined { get; set; }

		public bool HasDoNotInlineAttribute { get; set; }

		public bool HasAggressiveInliningAttribute { get; set; }

		public bool IsMethodImplementationReplaced { get; set; }

		public bool HasLoops { get; set; }

		public bool HasAddressOfInstruction { get; set; }

		public int IRInstructionCount { get; set; }

		public int IRStackParameterInstructionCount { get; set; }

		public int NonIRInstructionCount { get; set; }

		public bool IsVirtual { get; set; }

		public bool IsDevirtualized { get; set; }

		public int CompileCount { get; set; }

		public int ParameterStackSize { get; set; }

		public int LocalMethodStackSize { get; set; }

		public List<LabelRegion> LabelRegions { get; }

		public long ElapsedTicks { get; set; }

		public bool DoNotInline { get; set; }

		public int InlinedTimestamp
		{
			get { lock (_lock) { return inlinedTimestamp; } }
			set { lock (_lock) { inlinedTimestamp = value; } }
		}

		public int LastInlineDependencyReferenceTimestamp
		{
			get { lock (_lock) { return lastInlineDependencyReferenceTimestamp; } }
			set { lock (_lock) { lastInlineDependencyReferenceTimestamp = Math.Max(lastInlineDependencyReferenceTimestamp, value); } }
		}

		public BasicBlocks InlineBasicBlocks
		{
			get { lock (_lock) { return inlineBasicBlocks; } }
			set { lock (_lock) { inlineBasicBlocks = value; } }
		}

		public List<MosaMethod> Callers
		{
			get
			{
				lock (callers)
				{
					if (cachedCallers == null)
					{
						cachedCallers = new List<MosaMethod>(callers);
					}

					return cachedCallers;
				}
			}
		}

		#endregion Properties

		#region Data Members

		private readonly object _lock = new object();

		private BasicBlocks inlineBasicBlocks;

		private int inlinedTimestamp;

		private int lastInlineDependencyReferenceTimestamp;

		private List<MosaMethod> callers;

		private List<MosaMethod> cachedCallers;

		#endregion Data Members

		public MethodData(MosaMethod mosaMethod)
		{
			Method = mosaMethod;

			callers = new List<MosaMethod>();
			LabelRegions = new List<LabelRegion>();
			Counters = new Counters();
			CompileCount = 0;
			DoNotInline = false;
			InlineBasicBlocks = null;
		}

		#region Methods

		public void AddCaller(MosaMethod method)
		{
			lock (callers)
			{
				callers.AddIfNew(method);
				cachedCallers = null;
			}
		}

		public void AddLabelRegion(int label, int start, int length)
		{
			LabelRegions.Add(new LabelRegion()
			{
				Label = label,
				Start = start,
				Length = length,
			});
		}

		#endregion Methods
	}
}
