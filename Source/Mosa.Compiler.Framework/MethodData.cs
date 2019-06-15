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

		public bool IsDevirtualized { get; set; }

		public int CompileCount { get; set; }

		public int ParameterStackSize { get; set; }

		public int LocalMethodStackSize { get; set; }

		public List<LabelRegion> LabelRegions { get; }

		public long ElapsedTicks { get; set; }

		public bool DoNotInline { get; set; }

		public int InlineTimestamp
		{
			get { lock (_lock) { return inlineTimestamp; } }
			set { lock (_lock) { inlineTimestamp = value; } }
		}

		public int InlineDependencyUpdateTimestamp
		{
			get { lock (_lock) { return inlineDependencyUpdateTimestamp; } }
			set { lock (_lock) { inlineDependencyUpdateTimestamp = Math.Max(inlineDependencyUpdateTimestamp, value); } }
		}

		public InlineMethodData InlineMethodData
		{
			get { lock (_lock) { return inlineMethodData; } }
			set { lock (_lock) { inlineMethodData = value; } }
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

		private InlineMethodData inlineMethodData;

		private int inlineTimestamp;

		private int inlineDependencyUpdateTimestamp;

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
			inlineMethodData = null;
			IsDevirtualized = false;
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
