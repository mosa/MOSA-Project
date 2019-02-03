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
		private readonly object _lock = new object();

		#region Properties

		public Counters Counters { get; }

		public MosaMethod Method { get; }

		public bool InvokesAnyMethod { get { return Calls.Count != 0; } }

		public bool IsCompiled { get; set; }

		public bool IsCILDecoded { get; set; }

		public bool IsLinkerGenerated { get; set; }

		public bool HasProtectedRegions { get; set; }

		public bool CanInline { get; set; }

		public bool HasDoNotInlineAttribute { get; set; }

		public bool HasAggressiveInliningAttribute { get; set; }

		public bool IsMethodImplementationReplaced { get; set; }

		public bool HasLoops { get; set; }

		public bool HasAddressOfInstruction { get; set; }

		public List<MosaMethod> Calls { get; set; }

		public List<MosaMethod> CalledBy { get; set; }

		public BasicBlocks BasicBlocks { get; set; }

		public int IRInstructionCount { get; set; }

		public int IRStackParameterInstructionCount { get; set; }

		public int NonIRInstructionCount { get; set; }

		public bool IsVirtual { get; set; }

		public bool IsDevirtualized { get; set; }

		public int CompileCount { get; set; }

		public int ParameterStackSize { get; set; }

		public int LocalMethodStackSize { get; set; }

		public List<LabelRegion> LabelRegions { get; }

		public long CompileTimeStartTick { get; set; }

		public long CompileTimeEndTick { get; set; }

		public long CompileTimeTicks { get { return CompileTimeEndTick - CompileTimeStartTick; } }

		#endregion Properties

		public MethodData(MosaMethod mosaMethod)
		{
			Method = mosaMethod ?? throw new ArgumentNullException(nameof(mosaMethod));

			Calls = new List<MosaMethod>();
			CalledBy = new List<MosaMethod>();
			LabelRegions = new List<LabelRegion>();
			Counters = new Counters();
			CompileCount = 0;
		}

		#region Methods

		public void AddCalledBy(MosaMethod method)
		{
			lock (_lock)
			{
				CalledBy.AddIfNew(method);
			}
		}

		public void AddLabelRegion(int label, int start, int length)
		{
			LabelRegions.Add(new LabelRegion()
			{
				Label = label,
				Start = start,
				Length = length
			});
		}

		#endregion Methods
	}
}
