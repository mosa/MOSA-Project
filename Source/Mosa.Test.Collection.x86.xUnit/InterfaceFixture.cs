/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *
 */

using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class InterfaceFixture : X86TestFixture
	{
		[Fact]
		public void InterfaceTest1()
		{
			Assert.Equal(InterfaceTests.InterfaceTest1(), Run<int>("Mosa.Test.Collection.InterfaceTests.InterfaceTest1"));
		}

		[Fact]
		public void InterfaceTest2()
		{
			Assert.Equal(InterfaceTests.InterfaceTest2(), Run<int>("Mosa.Test.Collection.InterfaceTests.InterfaceTest2"));
		}

		[Fact]
		public void InterfaceTest3()
		{
			Assert.Equal(InterfaceTests.InterfaceTest3(), Run<int>("Mosa.Test.Collection.InterfaceTests.InterfaceTest3"));
		}
	}
}