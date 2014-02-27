/*
 * (c) 2010 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 *
 */

using MbUnit.Framework;
using Mosa.Test.Collection;
using Mosa.Test.System;

namespace Mosa.Test.Collection.MbUnit
{
	[TestFixture]
	public class RegisterAllocatorFixture : TestCompilerAdapter
	{
		public RegisterAllocatorFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void RegisterPressure8()
		{
			Assert.AreEqual(RegisterAllocatorTests.Pressure8(), Run<int>("Mosa.Test.Collection", "RegisterAllocatorTests", "Pressure8"));
		}

		[Test]
		public void RegisterPressure7([FewScatteredAI4]int a, [FewScatteredBI4]int b, [FewScatteredCI4]int c, [FewScatteredAI4]int d, [FewScatteredBI4]int e, [FewScatteredCI4]int f, [FewScatteredAI4]int g)
		{
			Assert.AreEqual(RegisterAllocatorTests.Pressure7(a, b, c, d, e, f, g), Run<int>("Mosa.Test.Collection", "RegisterAllocatorTests", "Pressure7", a, b, c, d, e, f, g));
		}

		[Test]
		public void RegisterPressure7B([FewScatteredAI4]int a, [FewScatteredBI4]int b, [FewScatteredCI4]int c, [FewScatteredAI4]int d, [FewScatteredBI4]int e, [FewScatteredCI4]int f, [FewScatteredAI4]int g)
		{
			Assert.AreEqual(RegisterAllocatorTests.Pressure7B(a, b, c, d, e, f, g), Run<int>("Mosa.Test.Collection", "RegisterAllocatorTests", "Pressure7B", a, b, c, d, e, f, g));
		}


		[Test]
		public void RegisterPressure7C([FewScatteredAI4]int a, [FewScatteredBI4]int b, [FewScatteredCI4]int c, [FewScatteredAI4]int d, [FewScatteredBI4]int e, [FewScatteredCI4]int f, [FewScatteredAI4]int g)
		{
			Assert.AreEqual(RegisterAllocatorTests.Pressure7C(a, b, c, d, e, f, g), Run<int>("Mosa.Test.Collection", "RegisterAllocatorTests", "Pressure7C", a, b, c, d, e, f, g));
		}
	}
}