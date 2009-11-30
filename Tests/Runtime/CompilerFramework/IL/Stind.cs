/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Bruce Markham (<mailto:illuminus86@gmail.com>)
 *  
 */

using System;
using System.Runtime.InteropServices;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public unsafe class Stind : CodeDomTestRunner
    {
        #region DereffedVoidPtrAssign

        private static string CreateDereferencedVoidPointerAssignmentTestCode(string pointerType, string methodNameSuffix)
        {
            return @"
                    static class Test {
                    public unsafe static bool DereffedVoidPtrAssign_" + methodNameSuffix + "(" + pointerType + @" value, void* address)
                    {
                        *(" + pointerType + @"*)address = value;
                        return true;
                    }
                    }";
        }

        #region I1

        unsafe delegate bool I1(sbyte value, void* address);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Row(sbyte.MinValue)]
        [Row(-1)]
        [Row(0)]
        [Row(1)]
        [Row(sbyte.MaxValue)]
        [Test, Author("illuminus", "illuminus86@gmail.com")]
        public unsafe void DereffedVoidPtrAssign_I1(sbyte a)
        {
            CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("sbyte","I1");
            UnsafeCode = true;

            var address = (void*)Marshal.AllocHGlobal(sizeof(sbyte));
            var runResult = (bool)Run<I1>("", "Test", "DereffedVoidPtrAssign_I1", a, (IntPtr)address);
            var success = (*(sbyte*)address == a);
            Marshal.FreeHGlobal((IntPtr)address);

            Assert.IsTrue(success, "DereffedVoidPtrAssign_I1");
        }

        #endregion I1

        #region I2

        delegate bool I2(short value, void* address);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Row(short.MinValue)]
        [Row(-1)]
        [Row(0)]
        [Row(1)]
        [Row(short.MaxValue)]
        [Test, Author("illuminus", "illuminus86@gmail.com")]
        public unsafe void DereffedVoidPtrAssign_I2(short a)
        {
            CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("short", "I2");
            UnsafeCode = true;

            void* address = (void*)Marshal.AllocHGlobal(sizeof(short));
            bool runResult = (bool)Run<I2>("", "Test", "DereffedVoidPtrAssign_I2", a, (IntPtr)address);
            bool success = (*(short*)address == a);
            Marshal.FreeHGlobal((IntPtr)address);

            Assert.IsTrue(success, "DereffedVoidPtrAssign_I2");
        }

        #endregion I2

        #region I4

        delegate bool I4(int value, void* address);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Row(int.MinValue)]
        [Row(-1)]
        [Row(0)]
        [Row(1)]
        [Row(int.MaxValue)]
        [Test, Author("illuminus", "illuminus86@gmail.com")]
        public unsafe void DereffedVoidPtrAssign_I4(int a)
        {
            CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("int", "I4");
            UnsafeCode = true;

            void* address = (void*)Marshal.AllocHGlobal(sizeof(int));
            bool runResult = (bool)Run<I4>("", "Test", "DereffedVoidPtrAssign_I4", a, (IntPtr)address);
            bool success = (*(int*)address == a);
            Marshal.FreeHGlobal((IntPtr)address);

            Assert.IsTrue(success, "DereffedVoidPtrAssign_I4");
        }

        #endregion I4

        #region I8

        delegate bool I8(long value, void* address);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Row(int.MinValue)]
        [Row(-1)]
        [Row(0)]
        [Row(1)]
        [Row(int.MaxValue)]
        [Test, Author("illuminus", "illuminus86@gmail.com")]
        public unsafe void DereffedVoidPtrAssign_I8(long a)
        {
            CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("long","I8");
            UnsafeCode = true;

            void* address = (void*)Marshal.AllocHGlobal(sizeof(long));
            bool runResult = (bool)Run<I8>("", "Test", "DereffedVoidPtrAssign_I8", a, (IntPtr)address);
            bool success = (*(int*)address == a);
            Marshal.FreeHGlobal((IntPtr)address);

            Assert.IsTrue(success, "DereffedVoidPtrAssign_I8");
        }

        #endregion I8

        #region U1

        delegate bool U1(byte value, void* address);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Row(byte.MinValue)]
        [Row(125)]
        [Row(127)]
        [Row(byte.MaxValue)]
        [Test, Author("illuminus", "illuminus86@gmail.com")]
        public unsafe void DereffedVoidPtrAssign_U1(byte a)
        {
            CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("byte","U1");
            UnsafeCode = true;

            void* address = (void*)Marshal.AllocHGlobal(sizeof(byte));
            bool runResult = (bool)Run<U1>("", "Test", "DereffedVoidPtrAssign_U1", a, (IntPtr)address);
            bool success = (*(byte*)address == a);
            Marshal.FreeHGlobal((IntPtr)address);

            Assert.IsTrue(success, "DereffedVoidPtrAssign_U1");
        }

        #endregion U1

        #region U2

        delegate bool U2(ushort value, void* address);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Row(ushort.MinValue)]
        [Row((ushort.MaxValue / 2) - 1)]
        [Row((ushort.MaxValue / 2) + 1)]
        [Row(1)]
        [Row(ushort.MaxValue)]
        [Test, Author("illuminus", "illuminus86@gmail.com")]
        public unsafe void DereffedVoidPtrAssign_U2(ushort a)
        {
            CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("ushort","U2");
            UnsafeCode = true;

            void* address = (void*)Marshal.AllocHGlobal(sizeof(ushort));
            bool runResult = (bool)Run<U2>("", "Test", "DereffedVoidPtrAssign_U2", a, (IntPtr)address);
            bool success = (*(ushort*)address == a);
            Marshal.FreeHGlobal((IntPtr)address);

            Assert.IsTrue(success, "DereffedVoidPtrAssign_U2");
        }

        #endregion U2

        #region U4

        delegate bool U4(uint value, void* address);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Row(uint.MinValue)]
        [Row(uint.MaxValue / 2)]
        [Row((uint.MaxValue / 2) + 1)]
        [Row(uint.MaxValue)]
        [Test, Author("illuminus", "illuminus86@gmail.com")]
        public void DereffedVoidPtrAssign_U4(uint a)
        {
            CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("uint","U4");
            UnsafeCode = true;

            var address = (void*)Marshal.AllocHGlobal(sizeof(uint));
            var runResult = (bool)Run<U4>("", "Test", "DereffedVoidPtrAssign_U4", a, (IntPtr)address);
            var success = (*(uint*)address == a);
            Marshal.FreeHGlobal((IntPtr)address);

            Assert.IsTrue(success, "DereffedVoidPtrAssign_U4");
        }

        #endregion U4

        #region U8

        delegate bool U8(ulong value, void* address);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Row(ulong.MinValue)]
        [Row((ulong.MaxValue / 2)+1)]
        [Row(ulong.MaxValue)]
        [Test, Author("illuminus", "illuminus86@gmail.com")]
        public unsafe void DereffedVoidPtrAssign_U8(ulong a)
        {
            CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("ulong","U8");
            UnsafeCode = true;

            void* address = (void*)Marshal.AllocHGlobal(sizeof(ulong));
            bool runResult = (bool)Run<U8>("", "Test", "DereffedVoidPtrAssign_U8", a, (IntPtr)address);
            bool success = (*(ulong*)address == a);
            Marshal.FreeHGlobal((IntPtr)address);

            Assert.IsTrue(success, "DereffedVoidPtrAssign_U8");
        }

        #endregion U8

        #endregion
    }
}
