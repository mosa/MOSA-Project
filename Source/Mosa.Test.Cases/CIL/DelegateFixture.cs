/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Kai Patrick Reisert <kpreisert@googlemail.com> 
 */

using System;
using MbUnit.Framework;

using Mosa.Test.System;

namespace Mosa.Test.Cases.CIL
{
	public class DelegateFixture : TestCompilerAdapter
	{
		public DelegateFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
			settings.AddReference("Mosa.Platform.x86.Intrinsic.dll");
		}

		[Test]
		public void DefineDelegate()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DelegateTests", "DefineDelegate"));
		}

		[Test]
		public void CallDelegateVoid()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DelegateTests", "CallDelegateVoid"));
		}
	}
}