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

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	public class SpecificTestFixture : TestCompilerAdapter
	{
		public SpecificTestFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void Test1([I4Small] uint value)
		{
			Assert.AreEqual(SpecificTests.Test1(value), Run<uint>("Mosa.Test.Collection", "SpecificTests", "Test1", value));
		}

		[Test]
		public void R4ToI4([R4FitsI4] float value)
		{
			Assert.AreEqual<int>(SpecificTests.R4ToI4(value), Run<int>("Mosa.Test.Collection", "SpecificTests", "R4ToI4", value));
		}

		[Test]
		public void R8ToI4([R8FitsI4] double value)
		{
			Assert.AreEqual<int>(SpecificTests.R8ToI4(value), Run<int>("Mosa.Test.Collection", "SpecificTests", "R8ToI4", value));
		}

		[Test]
		public void IsNaN()
		{
			Assert.AreEqual<bool>(SpecificTests.IsNaN(), Run<bool>("Mosa.Test.Collection", "SpecificTests", "IsNaN"));
		}

		[Test]
		public void CeqR8R8()
		{
			Assert.AreEqual<bool>(SpecificTests.CeqR8R8(), Run<bool>("Mosa.Test.Collection", "SpecificTests", "CeqR8R8"));
		}

		[Test]
		public void R8IsPositiveInfinity1()
		{
			Assert.AreEqual<bool>(SpecificTests.R8IsPositiveInfinity1(), Run<bool>("Mosa.Test.Collection", "SpecificTests", "R8IsPositiveInfinity1"));
		}

		[Test]
		public void R8R8IsNegativeInfinity1()
		{
			Assert.AreEqual<bool>(SpecificTests.R8IsNegativeInfinity1(), Run<bool>("Mosa.Test.Collection", "SpecificTests", "R8IsNegativeInfinity1"));
		}

		[Test]
		public void R8IsPositiveInfinity2()
		{
			Assert.AreEqual<bool>(SpecificTests.R8IsPositiveInfinity2(), Run<bool>("Mosa.Test.Collection", "SpecificTests", "R8IsPositiveInfinity2"));
		}

		[Test]
		public void R8R8IsNegativeInfinity2()
		{
			Assert.AreEqual<bool>(SpecificTests.R8IsNegativeInfinity2(), Run<bool>("Mosa.Test.Collection", "SpecificTests", "R8IsNegativeInfinity2"));
		}
	}
}