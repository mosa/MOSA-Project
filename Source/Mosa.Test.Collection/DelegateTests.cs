/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Kai Patrick Reisert <kpreisert@googlemail.com> 
 */

using System;

namespace Mosa.Test.Collection
{
	public static class DelegateTests
	{
		delegate void DelegateVoid();

		public static bool DefineDelegate()
		{
			DelegateVoid d = DelegateVoidTarget;
			return d != null;
		}

		public static bool CallDelegateVoid()
		{
			DelegateVoid d = DelegateVoidTarget;
			d();
			return true;
		}

		public static void DelegateVoidTarget()
		{

		}
	}
}