/*
 * (c) 2015 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	///
	/// </summary>
	public sealed class CompilerMethodData
	{
		#region Properties

		public MosaMethod Method { get; private set; }

		public bool IsMethodInvoked { get; set; }

		public bool IsCompiled { get; set; }

		public bool IsCILDecoded { get; set; }

		public bool IsLinkerGenerated { get; set; }

		public bool HasProtectedRegions { get; set; }

		public bool CanInline { get; set; }

		public bool HasDoNotInlineAttribute { get; set; }

		public bool IsPlugged { get; set; }

		public bool HasLoops { get; set; }

		public List<MosaMethod> Calls { get; set; }

		public List<MosaMethod> CalledBy { get; set; }

		public BasicBlocks BasicBlocks { get; set; }

		public int IRInstructionCount { get; set; }

		public int IROtherInstructionCount { get; set; }

		public bool IsVirtual { get; set; }

		#endregion Properties

		#region Methods

		public CompilerMethodData(MosaMethod mosaMethod)
		{
			if (mosaMethod == null)
				throw new ArgumentNullException("mosaMethod");

			Method = mosaMethod;

			this.Calls = new List<MosaMethod>();
			this.CalledBy = new List<MosaMethod>();
		}

		public void ClearCallList()
		{
			lock (this)
			{
				Calls.Clear();
			}
		}

		public void ClearCalledByList()
		{
			lock (this)
			{
				CalledBy.Clear();
			}
		}

		public void AddCall(MosaMethod method)
		{
			lock (this)
			{
				Calls.AddIfNew(method);
			}
		}

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
