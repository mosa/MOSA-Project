using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.Compiler
{
	[TestFixture]
	public class StaticAllocationFixture : CodeDomTestRunner
	{
		[Test]
		public void MustCompileStaticAllocation()
		{
			CodeSource = TestCode;
			int result = (int)this.Run<I4_V>(@"", @"StaticAllocationTestCode", @"GetData");
			Assert.AreEqual(0x7AADF00D, result);
		}

		private delegate int I4_V();

		private const string TestCode = @"
			public class StaticallyAllocatedType
			{
				public StaticallyAllocatedType()
				{
					this.dataField = 0x7AADF00D;
				}

				public int dataField;
			}

			public class StaticAllocationTestCode
			{
				private static readonly StaticallyAllocatedType allocatedObject = new StaticallyAllocatedType();

				public static int GetData()
				{
					return allocatedObject.dataField;
				}                
			}
		" + Code.AllTestCode;
	}
}
