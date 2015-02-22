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
using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	///
	/// </summary>
	public sealed class CompilerMethodData
	{
		#region Data Members

		public object mylock1 = new object();
		public object mylock2 = new object();

		#endregion Data Members

		#region Properties

		public MosaMethod MosaMethod { get; private set; }

		public bool IsMethodInvoked { get; set; }

		public bool IsCompiled { get; set; }

		public bool IsCILDecoded { get; set; }

		public bool IsLinkerGenerated { get; set; }

		public bool HasProtectedRegions { get; set; }

		public bool CanInline { get; set; }

		public bool HasDoNotInlineAttribute { get; set; }

		public bool IsPlugged { get; set; }

		public bool HasLoops { get; set; }

		public HashSet<MosaMethod> Calls { get; set; }

		public HashSet<MosaMethod> CalledBy { get; set; }

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

			MosaMethod = mosaMethod;

			this.Calls = new HashSet<MosaMethod>();
			this.CalledBy = new HashSet<MosaMethod>();
		}

		public void AddCall(MosaMethod method)
		{
			lock (mylock1)
			{
				Calls.AddIfNew(method);
			}
		}

		public void AddCalledBy(MosaMethod method)
		{
			lock (mylock2)
			{
				CalledBy.AddIfNew(method);
			}
		}

		#endregion Methods
	}
}
