/*
 * (c) 2010 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phillip Webster (mincus) <phillmwebster@gmail.com>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using MbUnit.Framework;

using Mosa.Test.Runtime.CompilerFramework;

namespace Mosa.Test.Cases.OLD.IL
{
	[TestFixture]
	public class While : TestCompilerAdapter
	{
		#region WhileIncI4 test
		// Tests basic increment loop

		[Row(0, 20)]
		[Row(-20, 0)]
		[Row(-100, 100)]
		[Test]
		public void WhileIncI4(int start, int limit)
		{
			settings.CodeSource = @"static class Test {
				static int WhileIncI4(int start, int limit)
				{
					int count = 0;

					while(start < limit)
					{
						++count;
						++start;
					}

					return count;
				}
			}";
			Assert.AreEqual<int>(limit - start, Run<int>(string.Empty, "Test", "WhileIncI4", start, limit));
		}

		#endregion

		#region WhileDecI4 test
		// Tests basic decrement loop.

		[Row(20, 0)]
		[Row(0, -20)]
		[Row(100, -100)]
		[Test]
		public void WhileDecI4(int start, int limit)
		{
			settings.CodeSource = @"static class Test {
				static int WhileDecI4(int start, int limit)
				{
					int count = 0;

					while(start > limit)
					{
						++count;
						--start;
					}

					return count;
				}
			}";
			Assert.AreEqual<int>(start - limit, Run<int>(string.Empty, "Test", "WhileDecI4", start, limit));
		}

		#endregion

		#region WhileFalse() test
		// Tests while(false)
		// Ensures "unreachable code" is never reached.

		[Row()]
		[Test]
		public void WhileFalse()
		{
			settings.CodeSource = @"static class Test {
				static bool WhileFalse()
				{
					bool called = false;

					while(false)
					{
						called = true;
					}

					return called;
				}
			}";

			Assert.IsFalse(Run<bool>(string.Empty, "Test", "WhileFalse"));
		}

		#endregion

		#region WhileContinueBreak() test

		// Tests while(true)
		// Tests break;
		// Tests continue;
		// Tests "unreachable code" after a continue or break is never reached.

		[Row()]
		[Test]
		public void WhileContinueBreak()
		{
			settings.CodeSource = @"static class Test {
				static bool WhileContinueBreak()
				{
					bool called = false;
					int start = 0;
					int limit = 20;
					int count = 0;
			
					while(true)
					{
						++count;
						++start;
					
						if(start == limit)
						{
							break;
						}
						else
						{
							continue;
						}
				
						called = true;
					}

					return !called && start == limit && count == 20;
				}
			}";
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "WhileContinueBreak"));
		}

		#endregion

		#region WhileOverflowIncI1 test

		// Tests overflowing a variable during an increment loop.

		[Row(254, 1)]
		[Row(byte.MaxValue, byte.MinValue)]
		[Test]
		public void WhileOverflowIncI1(byte start, byte limit)
		{
			settings.CodeSource = @"static class Test {
				static int WhileOverflowIncI1(byte start, byte limit)
				{
					int count = 0;

					while(start != limit)
					{
						++start;
						++count;
					}
					
					return count;
				}
			}";
			Assert.AreEqual<int>((256 + (int)limit) - start, Run<int>(string.Empty, "Test", "WhileOverflowIncI1", start, limit));
		}

		#endregion

		#region WhileOverflowDecI1 test

		// Tests overflowing a variable in decrement loops.

		[Row(1, 254)]
		[Row(byte.MinValue, byte.MaxValue)]
		[Test]
		public void WhileOverflowDecI1(byte start, byte limit)
		{
			settings.CodeSource = @"static class Test {
				static int WhileOverflowDecI1(byte start, byte limit)
				{
					int count = 0;

					while(start != limit)
					{
						--start;
						++count;
					}
					
					return count;
				}
			}";

			Assert.AreEqual<int>((256 + (int)start) - limit, Run<int>(string.Empty, "Test", "WhileOverflowDecI1", start, limit));
		}

		#endregion

		#region WhileNestedEqualsI4 test

		// Tests nested looping (basic increment loop internally)
		// Tests == operator on external loop

		[Row(2, 3, 0, 20)]
		[Row(0, 1, 100, 200)]
		[Row(1, 0, -100, 100)]
		[Row(int.MaxValue, int.MinValue, -2, 3)]
		[Test]
		public void WhileNestedEqualsI4(int initialStatus, int wantedStatus, int start, int limit)
		{
			settings.CodeSource = @"static class Test {
				static int WhileNestedEqualsI4(int initialStatus, int wantedStatus, int start, int limit)
				{
					int count = 0;
					int start2 = start;
					int status = initialStatus;

					while (status == initialStatus)
					{
						start2 = start;

						while (start2 < limit)
						{
							++start2;
							++count;
						}

						++start;

						if (start == limit)
						{
							status = wantedStatus;
						}
					}

					return count;
				}
			}";

			int count = limit - start;
			Assert.AreEqual<int>((int)((count * count) - ((count / 2.0f) * count) + (count / 2.0f)), Run<int>(string.Empty, "Test", "WhileNestedEqualsI4", initialStatus, wantedStatus, start, limit));
		}

		#endregion
	}
}
