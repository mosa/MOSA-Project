// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.MosaTypeSystem;
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

		public int InlineTimestamp
		{
			get { lock (_lock) { return inlinedTimestamp; } }
			set { lock (_lock) { inlinedTimestamp = value; } }
		}

		public int InlineEvalulationTimestamp
		{
			get { lock (_lock) { return inlineEvalulationTimestamp; } }
			set { lock (_lock) { inlineEvalulationTimestamp = value; } }
		}

		public BasicBlocks BasicBlocks
		{
			get { lock (_lock) { return inlinedBasicBlocks; } }
			set { lock (_lock) { inlinedBasicBlocks = value; } }
		}

		public List<MosaMethod> CalledBy
		{
			get
			{
				lock (calledBy)
				{
					if (cachedCallBy == null)
					{
						cachedCallBy = new List<MosaMethod>(calledBy);
					}

					return cachedCallBy;
				}
			}
		}

		#endregion Properties

		#region Data Members

		private readonly object _lock = new object();

		private BasicBlocks inlinedBasicBlocks;

		private int inlinedTimestamp;

		private int inlineEvalulationTimestamp;

		private List<MosaMethod> calledBy;

		private List<MosaMethod> cachedCallBy;

		#endregion Data Members

		public MethodData(MosaMethod mosaMethod)
		{
			Method = mosaMethod;

			calledBy = new List<MosaMethod>();
			LabelRegions = new List<LabelRegion>();
			Counters = new Counters();
			CompileCount = 0;
			DoNotInline = false;
			BasicBlocks = null;
		}

		#region Methods

		public void AddCalledBy(MosaMethod method)
		{
			lock (calledBy)
			{
				calledBy.AddIfNew(method);
				cachedCallBy = null;
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
