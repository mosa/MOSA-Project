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

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    [TestFixture]
    public class While : CodeDomTestRunner
    {
        #region WhileIncI4 test

        delegate int I4_I4([MarshalAs(UnmanagedType.I4)]int start, [MarshalAs(UnmanagedType.I4)]int limit);

        [Row(0, 20)]
        [Row(-20, 0)]
        [Row(-100, 100)]
        [Test, Author("mincus", "phillmwebster@gmail.com")]
        public void WhileIncI4(int start, int limit)
        {
            CodeSource = @"static class Test {
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
            Assert.AreEqual<int>(limit - start, (int)Run<I4_I4>("", "Test", "WhileIncI4", start, limit));
        }

        #endregion

        #region WhileDecI4 test

        [Row(20, 0)]
        [Row(0, -20)]
        [Row(100, -100)]
        [Test, Author("mincus", "phillmwebster@gmail.com")]
        public void WhileDecI4(int start, int limit)
        {
            CodeSource = @"static class Test {
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
            Assert.AreEqual<int>(start - limit, (int)Run<I4_I4>("", "Test", "WhileDecI4", start, limit));
        }

        #endregion

        #region WhileFalse() test

        delegate bool BV();

        [Row()]
        [Test, Author("mincus", "phillmwebster@gmail.com")]
        public void WhileFalse()
        {
            CodeSource = @"static class Test {
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

            Assert.IsFalse((bool)Run<BV>("", "Test", "WhileFalse", null));
        }

        #endregion

        #region WhileContinueBreak() test

        [Row()]
        [Test, Author("mincus", "phillmwebster@gmail.com")]
        public void WhileContinueBreak()
        {
            CodeSource = @"static class Test {
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
            Assert.IsTrue((bool)Run<BV>("", "Test", "WhileContinueBreak", null));
        }

        #endregion

        #region WhileOverflowIncI1 test

        [Row(254, 1)]
        [Test, Author("mincus", "phillmwebster@gmail.com")]
        public void WhileOverflowIncI1(byte start, byte limit)
        {
            CodeSource = @"static class Test {
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
            Assert.AreEqual<int>((256 + (int)limit) - start, (int)Run<U1_U1>("", "Test", "WhileOverflowIncI1", start, limit));
        }

        #endregion

        #region WhileOverflowDecI1 test

        delegate int U1_U1([MarshalAs(UnmanagedType.U1)]byte start, [MarshalAs(UnmanagedType.U1)]byte limit);

        [Row(1, 254)]
        [Test, Author("mincus", "phillmwebster@gmail.com")]
        public void WhileOverflowDecI1(byte start, byte limit)
        {
            CodeSource = @"static class Test {
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
            Assert.AreEqual<int>((256 + (int)start) - limit, (int)Run<U1_U1>("", "Test", "WhileOverflowDecI1", start, limit));
        }

        #endregion
    }
}
