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
                    static unsafe " + typeOut + " AddPtr(" + typeOut + " expect, " + typeInA + " a, " + typeInB + @" b)
                    {
                        return (a + b);
                    }
                }";
        }
        
        private static string CreateConstantTestCode(string typeIn, string typeOut, string constantLeft, string constantRight)
        {
            if (constantLeft == null)
            {
                return @"
                    static class Test
                    {
                        static unsafe " + typeOut + " AddPtr(" + typeOut + " expect, " + typeIn + @" a)
                        {
                            return (a + " + constantRight + @");
                        }
                    }";
            }
            else if (constantRight == null)
            {
                return @"
                    static class Test
                    {
                        static unsafe " + typeOut + " AddPtr(" + typeOut + " expect, " + typeIn + @" a)
                        {
                            return (" + constantLeft + @" + a);
                        }
                    }";
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        delegate int I4_I4_I4(int expect, int a, int b);
        delegate int I4_I4_I8(int expect, int a, long b);
        delegate int I4_I8_I4(int expect, long a, int b);
        delegate int I4_I4_Constant(int expect, int x);

        #region C
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddCPtrI4Left(int a, int b)
        {
            char* pa = (char*)a;
            this.CodeSource = CreateTestCode("char*", "int", "char*");
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_I4>("", "Test", "AddPtr", (int)(pa + b), a, b));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddCPtrI8Left(int a, long b)
        {
            char* pa = (char*)a;
            this.CodeSource = CreateTestCode("char*", "long", "char*");
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_I8>("", "Test", "AddPtr", (int)(pa + b), a, b));
        }
        
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddCPtrI4Right(int a, int b)
        {
            char* pa = (char*)a;
            this.CodeSource = CreateTestCode("int", "char*", "char*");
            this.UnsafeCode = true;
            Assert.AreEqual((int)(b + pa), Run<I4_I4_I4>("", "Test", "AddPtr", (int)(b + pa), b, a));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddCPtrI8Right(int a, long b)
        {
            char* pa = (char*)a;
            this.CodeSource = CreateTestCode("long", "char*", "char*");
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I8_I4>("", "Test", "AddPtr", (int)(pa + b), b, a));
        }
        
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddConstantCPtrI4Left(int a, int b)
        {
            char* pa = (char*)a;
            this.CodeSource = CreateConstantTestCode("char*", "char*", null, b.ToString());
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_Constant>("", "Test", "AddPtr", (int)(pa + b), a));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddConstantCPtrI8Left(int a, long b)
        {
            char* pa = (char*)a;
            this.CodeSource = CreateConstantTestCode("char*", "char*", null, b.ToString());
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_Constant>("", "Test", "AddPtr", (int)(pa + b), a));
        }
        
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddConstantCPtrI4Right(int a, int b)
        {
            char* pa = (char*)a;
            this.CodeSource = CreateConstantTestCode("char*", "char*", b.ToString(), null);
            this.UnsafeCode = true;
            Assert.AreEqual((int)(b + pa), Run<I4_I4_Constant>("", "Test", "AddPtr", (int)(b + pa), a));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddConstantCPtrI8Right(int a, long b)
        {
            char* pa = (char*)a;
            this.CodeSource = CreateConstantTestCode("char*", "char*", b.ToString(), null);
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_Constant>("", "Test", "AddPtr", (int)(pa + b), a));
        }
        #endregion
        
        #region U1
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddU1PtrI4Left(int a, int b)
        {
            byte* pa = (byte*)a;
            this.CodeSource = CreateTestCode("byte*", "int", "byte*");
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_I4>("", "Test", "AddPtr", (int)(pa + b), a, b));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddU1PtrI8Left(int a, int b)
        {
            byte* pa = (byte*)a;
            this.CodeSource = CreateTestCode("byte*", "long", "byte*");
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_I8>("", "Test", "AddPtr", (int)(pa + b), a, b));
        }
        
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddU1PtrI4Right(int a, int b)
        {
            byte* pa = (byte*)a;
            this.CodeSource = CreateTestCode("int", "byte*", "byte*");
            this.UnsafeCode = true;
            Assert.AreEqual((int)(b + pa), Run<I4_I4_I4>("", "Test", "AddPtr", (int)(b + pa), b, a));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddU1PtrI8Right(int a, int b)
        {
            byte* pa = (byte*)a;
            this.CodeSource = CreateTestCode("long", "byte*", "byte*");
            this.UnsafeCode = true;
            Assert.AreEqual((int)(b + pa), Run<I4_I8_I4>("", "Test", "AddPtr", (int)(b + pa), b, a));
        }
        
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddConstantU1PtrI4Left(int a, int b)
        {
            byte* pa = (byte*)a;
            this.CodeSource = CreateConstantTestCode("byte*", "byte*", null, b.ToString());
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_Constant>("", "Test", "AddPtr", (int)(pa + b), a));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddConstantU1PtrI8Left(int a, long b)
        {
            byte* pa = (byte*)a;
            this.CodeSource = CreateConstantTestCode("byte*", "byte*", null, b.ToString());
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_Constant>("", "Test", "AddPtr", (int)(pa + b), a));
        }
        
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddConstantU1PtrI4Right(int a, int b)
        {
            byte* pa = (byte*)a;
            this.CodeSource = CreateConstantTestCode("byte*", "byte*", b.ToString(), null);
            this.UnsafeCode = true;
            Assert.AreEqual((int)(b + pa), Run<I4_I4_Constant>("", "Test", "AddPtr", (int)(b + pa), a));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddConstantU1PtrI8Right(int a, long b)
        {
            byte* pa = (byte*)a;
            this.CodeSource = CreateConstantTestCode("byte*", "byte*", b.ToString(), null);
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_Constant>("", "Test", "AddPtr", (int)(pa + b), a));
        }
        #endregion
        
        #region I4
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddI4PtrI4Left(int a, int b)
        {
            int* pa = (int*)a;
            this.CodeSource = CreateTestCode("int*", "int", "int*");
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_I4>("", "Test", "AddPtr", (int)(pa + b), a, b));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddI4PtrI8Left(int a, int b)
        {
            int* pa = (int*)a;
            this.CodeSource = CreateTestCode("int*", "long", "int*");
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_I8>("", "Test", "AddPtr", (int)(pa + b), a, b));
        }
        
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddI4PtrI4Right(int a, int b)
        {
            int* pa = (int*)a;
            this.CodeSource = CreateTestCode("int", "int*", "int*");
            this.UnsafeCode = true;
            Assert.AreEqual((int)(b + pa), Run<I4_I4_I4>("", "Test", "AddPtr", (int)(b + pa), b, a));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddI4PtrI8Right(int a, int b)
        {
            int* pa = (int*)a;
            this.CodeSource = CreateTestCode("long", "int*", "int*");
            this.UnsafeCode = true;
            Assert.AreEqual((int)(b + pa), Run<I4_I8_I4>("", "Test", "AddPtr", (int)(b + pa), b, a));
        }
        
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddConstantI4PtrI4Left(int a, int b)
        {
            int* pa = (int*)a;
            this.CodeSource = CreateConstantTestCode("int*", "int*", null, b.ToString());
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_Constant>("", "Test", "AddPtr", (int)(pa + b), a));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddConstantI4PtrI8Left(int a, long b)
        {
            int* pa = (int*)a;
            this.CodeSource = CreateConstantTestCode("int*", "int*", null, b.ToString());
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_Constant>("", "Test", "AddPtr", (int)(pa + b), a));
        }
        
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddConstantI4PtrI4Right(int a, int b)
        {
            int* pa = (int*)a;
            this.CodeSource = CreateConstantTestCode("int*", "int*", b.ToString(), null);
            this.UnsafeCode = true;
            Assert.AreEqual((int)(b + pa), Run<I4_I4_Constant>("", "Test", "AddPtr", (int)(b + pa), a));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddConstantI4PtrI8Right(int a, long b)
        {
            int* pa = (int*)a;
            this.CodeSource = CreateConstantTestCode("int*", "int*", b.ToString(), null);
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_Constant>("", "Test", "AddPtr", (int)(pa + b), a));
        }
        #endregion
        
        #region I8
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddI8PtrI4Left(int a, int b)
        {
            long* pa = (long*)a;
            this.CodeSource = CreateTestCode("long*", "int", "long*");
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_I4>("", "Test", "AddPtr", (int)(pa + b), a, b));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddI8PtrI8Left(int a, int b)
        {
            long* pa = (long*)a;
            this.CodeSource = CreateTestCode("long*", "long", "long*");
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_I8>("", "Test", "AddPtr", (int)(pa + b), a, b));
        }
        
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddI8PtrI4Right(int a, int b)
        {
            long* pa = (long*)a;
            this.CodeSource = CreateTestCode("int", "long*", "long*");
            this.UnsafeCode = true;
            Assert.AreEqual((int)(b + pa), Run<I4_I4_I4>("", "Test", "AddPtr", (int)(b + pa), b, a));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddI8PtrI8Right(int a, int b)
        {
            long* pa = (long*)a;
            this.CodeSource = CreateTestCode("long", "long*", "long*");
            this.UnsafeCode = true;
            Assert.AreEqual((int)(b + pa), Run<I4_I8_I4>("", "Test", "AddPtr", (int)(b + pa), b, a));
        }
        
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddConstantI8PtrI4Left(int a, int b)
        {
            long* pa = (long*)a;
            this.CodeSource = CreateConstantTestCode("long*", "long*", null, b.ToString());
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_Constant>("", "Test", "AddPtr", (int)(pa + b), a));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddConstantI8PtrI8Left(int a, long b)
        {
            long* pa = (long*)a;
            this.CodeSource = CreateConstantTestCode("long*", "long*", null, b.ToString());
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_Constant>("", "Test", "AddPtr", (int)(pa + b), a));
        }
        
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddConstantI8PtrI4Right(int a, int b)
        {
            long* pa = (long*)a;
            this.CodeSource = CreateConstantTestCode("long*", "long*", b.ToString(), null);
            this.UnsafeCode = true;
            Assert.AreEqual((int)(b + pa), Run<I4_I4_Constant>("", "Test", "AddPtr", (int)(b + pa), a));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddConstantI8PtrI8Right(int a, long b)
        {
            long* pa = (long*)a;
            this.CodeSource = CreateConstantTestCode("long*", "long*", b.ToString(), null);
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_Constant>("", "Test", "AddPtr", (int)(pa + b), a));
        }
        #endregion
        
        #region R8
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddR8PtrI4Left(int a, int b)
        {
            double* pa = (double*)a;
            this.CodeSource = CreateTestCode("double*", "int", "double*");
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_I4>("", "Test", "AddPtr", (int)(pa + b), a, b));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddR8PtrI8Left(int a, int b)
        {
            long* pa = (long*)a;
            this.CodeSource = CreateTestCode("double*", "long", "double*");
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_I8>("", "Test", "AddPtr", (int)(pa + b), a, b));
        }
        
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddR8PtrI4Right(int a, int b)
        {
            double* pa = (double*)a;
            this.CodeSource = CreateTestCode("long", "double*", "double*");
            this.UnsafeCode = true;
            Assert.AreEqual((int)(b + pa), Run<I4_I4_I4>("", "Test", "AddPtr", (int)(b + pa), b, a));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddR8PtrI8Right(int a, int b)
        {
            double* pa = (double*)a;
            this.CodeSource = CreateTestCode("long", "double*", "double*");
            this.UnsafeCode = true;
            Assert.AreEqual((int)(b + pa), Run<I4_I8_I4>("", "Test", "AddPtr", (int)(b + pa), b, a));
        }
        
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddConstantR8PtrI4Left(int a, int b)
        {
            double* pa = (double*)a;
            this.CodeSource = CreateConstantTestCode("double*", "double*", null, b.ToString());
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_Constant>("", "Test", "AddPtr", (int)(pa + b), a));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddConstantR8PtrI8Left(int a, long b)
        {
            double* pa = (double*)a;
            this.CodeSource = CreateConstantTestCode("double*", "double*", null, b.ToString());
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_Constant>("", "Test", "AddPtr", (int)(pa + b), a));
        }
        
        /// <summary>
        /// Tests addition of an integer to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The integer to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddConstantR8PtrI4Right(int a, int b)
        {
            double* pa = (double*)a;
            this.CodeSource = CreateConstantTestCode("double*", "double*", b.ToString(), null);
            this.UnsafeCode = true;
            Assert.AreEqual((int)(b + pa), Run<I4_I4_Constant>("", "Test", "AddPtr", (int)(b + pa), a));
        }
        
        /// <summary>
        /// Tests addition of a long to a pointer.
        /// </summary>
        /// <param name="a">The pointer value.</param>
        /// <param name="b">The long to add to the pointer.</param>
        [Row(0, 42)]
        [Row(int.MaxValue, 42)]
        [Row(42, 0)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public unsafe void AddConstantR8PtrI8Right(int a, long b)
        {
            double* pa = (double*)a;
            this.CodeSource = CreateConstantTestCode("double*", "double*", b.ToString(), null);
            this.UnsafeCode = true;
            Assert.AreEqual((int)(pa + b), Run<I4_I4_Constant>("", "Test", "AddPtr", (int)(pa + b), a));
        }
        #endregion
    }
}
