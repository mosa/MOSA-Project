/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.RegisterAllocator
{

	public class LiveRange
	{
		public LiveInterval LiveInterval { get; private set; }
		public Interval Interval { get; private set; }
		public int SpillCosts { get; set; }

		public LiveRange(LiveInterval liveInterval, Interval interval)
		{
			this.Interval = interval;
			this.LiveInterval = liveInterval;
			this.SpillCosts = 0;
		}
	}

}

