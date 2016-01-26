// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	///
	/// </summary>
	public sealed class CompilerMethodData
	{
		public Counters Counters { get; private set; }

		#region Properties

		public MosaMethod Method { get; private set; }

		public bool InvokesAnyMethod { get { return Calls.Count != 0; } }

		public bool IsCompiled { get; set; }

		public bool IsCILDecoded { get; set; }

		public bool IsLinkerGenerated { get; set; }

		public bool HasProtectedRegions { get; set; }

		public bool CanInline { get; set; }

		public bool HasDoNotInlineAttribute { get; set; }

		public bool IsPlugged { get; set; }

		public bool HasLoops { get; set; }

		public bool HasAddressOfInstruction { get; set; }

		public List<MosaMethod> Calls { get; set; }

		public List<MosaMethod> CalledBy { get; set; }

		public BasicBlocks BasicBlocks { get; set; }

		public int IRInstructionCount { get; set; }

		public int NonIRInstructionCount { get; set; }

		public bool IsVirtual { get; set; }

		public int CompileCount { get; set; }

		#endregion Properties

		public CompilerMethodData(MosaMethod mosaMethod)
		{
			if (mosaMethod == null)
				throw new ArgumentNullException("mosaMethod");

			Method = mosaMethod;

			Calls = new List<MosaMethod>();
			CalledBy = new List<MosaMethod>();
			Counters = new Counters();
			CompileCount = 0;
		}

		#region Methods

		public void AddCalledBy(MosaMethod method)
		{
			lock (this)
			{
				CalledBy.AddIfNew(method);
			}
		}

		#endregion Methods
	}
}
