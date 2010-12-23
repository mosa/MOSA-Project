using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

namespace Mosa.Test.Runtime.CompilerFramework.Compiler
{
	[TestFixture]
	public class StaticAllocationFixture : TestCompilerAdapter
	{
		[Test]
		public void MustCompileStaticAllocation()
		{
			settings.CodeSource = TestCode;
			int result = Run<int>(string.Empty, @"StaticAllocationTestCode", @"GetData");
			Assert.AreEqual(0x7AADF00D, result);
		}
		
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
		";
	}
}
