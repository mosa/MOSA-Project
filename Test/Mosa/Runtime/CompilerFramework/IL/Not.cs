/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class Not : MosaCompilerTestRunner
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool I4_I1(sbyte expect, sbyte a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Row(0)]
        [Row(1)]
        [Row(2)]
        [Row(5)]
        [Row(10)]
        [Row(11)]
        [Row(100)]
        [Row(-0)]
        [Row(-1)]
        [Row(-2)]
        [Row(-5)]
        [Row(-10)]
        [Row(-11)]
        [Row(-100)]
        [Row(sbyte.MinValue)]
        [Row(sbyte.MaxValue)]
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void NotI1(sbyte a)
        {
            CodeSource = "static class Test { static bool NotI1(int expect, sbyte a) { return expect == (~a); } }";
            Assert.IsTrue((bool)Run<I4_I1>("", "Test", "NotI1", (sbyte)~a, (sbyte)a));
        }
    }
}
