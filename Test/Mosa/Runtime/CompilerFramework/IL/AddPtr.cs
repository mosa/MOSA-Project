/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Kai P. Reisert (<mailto:kpreisert@googlemail.com>)
 *
 */

using System;
using System.Runtime.InteropServices;
using Gallio.Framework;
using MbUnit.Framework;
using Test.Mosa.Runtime.CompilerFramework.BaseCode;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Testcase for the AddInstruction on pointers
    /// </summary>
    [TestFixture]
    public class AddPtr : CodeDomTestRunner
    {
        private static string CreateTestCode(string typeInA, string typeInB, string typeOut)
        {
            return @"
                static class Test
                {
                    static unsafe bool AddPtr(" + typeOut + " expect, " + typeInA + " a, " + typeInB + @" b)
                    {
                        return expect == (a + b);
                    }
                }";
        }
        
        delegate bool I4_I4_I4(int expect, int a, int b);
        delegate bool I4_I4_I8(int expect, int a, long b);

        #region C
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 1)]
        [Row('a', 5)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddCPtrI4Left(char a, int b)
        {
            int* pa = (int*)(int)a;
            this.CodeSource = CreateTestCode("char*", "int", "char*");
            this.UnsafeCode = true;
            Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "AddPtr", (int)(pa + b), a, b));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 1)]
        [Row('a', 5)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddCPtrI8Left(char a, long b)
        {
            int* pa = (int*)(int)a;
            this.CodeSource = CreateTestCode("char*", "long", "char*");
            this.UnsafeCode = true;
            Assert.IsTrue((bool)Run<I4_I4_I8>("", "Test", "AddPtr", (int)(pa + b), a, b));
        }
        #endregion
        
        #region U1
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 1)]
        [Row(100, 5)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddU1PtrI4Left(byte a, int b)
        {
            int* pa = (int*)(int)a;
            this.CodeSource = CreateTestCode("byte*", "int", "byte*");
            this.UnsafeCode = true;
            Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "AddPtr", (int)(pa + b), a, b));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 1)]
        [Row(100, 5)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddU1PtrI8Left(byte a, long b)
        {
            int* pa = (int*)(int)a;
            this.CodeSource = CreateTestCode("byte*", "long", "byte*");
            this.UnsafeCode = true;
            Assert.IsTrue((bool)Run<I4_I4_I8>("", "Test", "AddPtr", (int)(pa + b), a, b));
        }
        #endregion
        
        #region I4
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 1)]
        [Row(10, 42)]
        [Test, Author("grover", "sharpos@michaelruck.de")]
        public unsafe void AddI4PtrI4Left(int a, int b)
        {
            int* pa = (int*)a;
            this.CodeSource = CreateTestCode("int*", "int", "int*");
            this.UnsafeCode = true;
            Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "AddPtr", (int)(pa + b), a, b));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 1)]
        [Row(10, 42)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddI4PtrI8Left(int a, long b)
        {
            int* pa = (int*)a;
            this.CodeSource = CreateTestCode("int*", "long", "int*");
            this.UnsafeCode = true;
            Assert.IsTrue((bool)Run<I4_I4_I8>("", "Test", "AddPtr", (int)(pa + b), a, b));
        }
        #endregion
        
        #region I8
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 1)]
        [Row(10, 42)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddI8PtrI4Left(long a, int b)
        {
            int* pa = (int*)a;
            this.CodeSource = CreateTestCode("long*", "int", "long*");
            this.UnsafeCode = true;
            Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "AddPtr", (int)(pa + b), a, b));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 1)]
        [Row(10, 42)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddI8PtrI8Left(long a, long b)
        {
            int* pa = (int*)a;
            this.CodeSource = CreateTestCode("long*", "long", "long*");
            this.UnsafeCode = true;
            Assert.IsTrue((bool)Run<I4_I4_I8>("", "Test", "AddPtr", (int)(pa + b), a, b));
        }
        #endregion
    }
}
