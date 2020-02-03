// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Linker;
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

		public MosaMethod Method { get; }

		public LinkerSymbol Symbol { get; set; }

		public Counters Counters { get; }

		public bool IsLinkerGenerated { get; set; }

		public bool HasProtectedRegions { get; set; }

		public bool HasDoNotInlineAttribute { get; set; }

		public bool HasAggressiveInliningAttribute { get; set; }

		public bool AggressiveInlineRequested { get; set; }

		public bool IsMethodImplementationReplaced { get; set; }

		public bool HasLoops { get; set; }

		public bool HasAddressOfInstruction { get; set; }

		public int IRInstructionCount { get; set; }

		public int IRStackParameterInstructionCount { get; set; }

		public int NonIRInstructionCount { get; set; }

		public bool IsDevirtualized { get; set; }

		public int Version { get; set; }

		public int ParameterStackSize { get; set; }

		public List<int> ParameterSizes { get; set; }

		public List<int> ParameterOffsets { get; set; }

		public int LocalMethodStackSize { get; set; }

		public List<LabelRegion> LabelRegions { get; }

		public long ElapsedTicks { get; set; }

		public bool DoNotInline { get; set; }

		public bool HasMethodPointerReferenced { get; set; }

		public bool HasCode { get; set; }

		public bool Inlined { get { lock (_lock) { return InlineMethodData.IsInlined; } } }

		public MosaMethod ReplacedBy { get; set; }

		public bool IsInvoked { get; set; }

		#endregion Properties

		#region Data Members

		private readonly object _lock = new object();

		private InlineMethodData InlineMethodData;

		#endregion Data Members

		public MethodData(MosaMethod mosaMethod)
		{
			Method = mosaMethod;

			LabelRegions = new List<LabelRegion>();
			Counters = new Counters();
			Version = 0;
			DoNotInline = false;
			InlineMethodData = new InlineMethodData(null, 0);
			IsDevirtualized = false;
			HasMethodPointerReferenced = false;
			HasCode = false;
			IsInvoked = false;
			AggressiveInlineRequested = false;
		}

		#region Methods

		public void AddLabelRegion(int label, int start, int length)
		{
			LabelRegions.Add(new LabelRegion()
			{
				Label = label,
				Start = start,
				Length = length,
			});
		}

		public InlineMethodData GetInlineMethodDataForUseBy(MosaMethod method)
		{
			lock (_lock)
			{
				InlineMethodData.AddReference(method);
				return InlineMethodData;
			}
		}

		public InlineMethodData SwapInlineMethodData(BasicBlocks basicBlocks)
		{
			lock (_lock)
			{
				var tmp = InlineMethodData;

				InlineMethodData = new InlineMethodData(basicBlocks, Version);

				return tmp;
			}
		}

		public InlineMethodData GetInlineMethodData()
		{
			lock (_lock)
			{
				return InlineMethodData;
			}
		}

		#endregion Methods
	}
}
