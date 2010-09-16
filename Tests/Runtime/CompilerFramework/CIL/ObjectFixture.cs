/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (aka grover, <mailto:sharpos@michaelruck.de>)
 *  
 */

using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.CIL
{
	[TestFixture]
	[Importance(Importance.Critical)]
	[Category(@"Object support")]
	[Description(@"Tests new operator, type checking and virtual method calls.")]
	public class ObjectFixture : CodeDomTestRunner
	{
		public static readonly string TestCode = @"
			public class Base
			{
				public int Test()
				{
					return 5;
				}
			}

			public class Derived : Base
			{
				private readonly int int32;

				public Derived()
				{
					this.int32 = 0;
				}

				public Derived(int value)
				{
					this.int32 = value;
				}

				public Derived(int v1, int v2)
				{
					this.int32 = v1 + v2;
				}

				public Derived(int v1, int v2, int v3)
				{
					this.int32 = (v1 * v2) + v3;
				}

				public static bool NewobjTest()
				{
					Derived d = new Derived();
					return d != null;
				}

				public static bool NewobjTestWithOneArg()
				{
					Derived d = new Derived(42);
					return (d.int32 == 42);
				}

				public static bool NewobjTestWithTwoArgs()
				{
					Derived d = new Derived(42, 3);
					return (d.int32 == 45);
				}


				public static bool NewobjTestWithThreeArgs()
				{
					Derived d = new Derived(21, 2, 7);
					return (d.int32 == 49);
				}
			}
		" + Code.ObjectClassDefinition;

		public delegate bool TestCodeDelegate();

		[Test]
		public void TestNewobjWithoutArgs()
		{
			this.EnsureCodeSourceIsSet();
			bool result = (bool)this.Run<TestCodeDelegate>(@"", @"Derived", @"NewobjTest");
			Assert.IsTrue(result);
		}

		[Test]
		public void TestNewobjWithOneArg()
		{
			this.EnsureCodeSourceIsSet();
			bool result = (bool)this.Run<TestCodeDelegate>(@"", @"Derived", @"NewobjTestWithOneArg");
			Assert.IsTrue(result);
		}

		[Test]
		public void TestNewobjWithTwoArgs()
		{
			this.EnsureCodeSourceIsSet();
			bool result = (bool)this.Run<TestCodeDelegate>(@"", @"Derived", @"NewobjTestWithTwoArgs");
			Assert.IsTrue(result);
		}

		[Test]
		public void TestNewobjWithThreeArgs()
		{
			this.EnsureCodeSourceIsSet();
			bool result = (bool)this.Run<TestCodeDelegate>(@"", @"Derived", @"NewobjTestWithThreeArgs");
			Assert.IsTrue(result);
		}

		private void EnsureCodeSourceIsSet()
		{
			if (CodeSource == null)
			{
				CodeSource = TestCode;
			}
		}
	}
}
