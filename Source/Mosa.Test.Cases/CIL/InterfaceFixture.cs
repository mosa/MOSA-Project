/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *
 */

using MbUnit.Framework;

using Mosa.Test.Runtime.CompilerFramework;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	[Importance(Importance.Critical)]
	[Category(@"Compiler")]
	[Description(@"Tests support for interfaces.")]
	public class InterfaceFixture : TestCompilerAdapter
	{

		public InterfaceFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void MustCompileInterfaces()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "TestClass", "MustCompileWithInterfaces"));
		}

		[Test]
		public void MustReturn3FromB()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "TestClass", "MustReturn3FromB"));
		}
	}
}
