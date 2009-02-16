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
using Gallio.Framework;
using MbUnit.Framework;
using Test.Mosa.Runtime.CompilerFramework.BaseCode;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public unsafe class Stind : CodeDomTestRunner
    {
        #region DereffedVoidPtrAssign

        private static string CreateDereferencedVoidPointerAssignmentTestCode(string pointerType)
        {
            return @"
                    public class Test {
                    public unsafe static bool AssignValueToPointer(" + pointerType + @" value, void* address)
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
            CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("sbyte");
            UnsafeCode = true;

            void* address = (void*)Marshal.AllocHGlobal(sizeof(sbyte));
            bool runResult = (bool)Run<I1>("", "Test", "AssignValueToPointer", a, (IntPtr)address);
            bool success = (*(sbyte*)address == a);
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
            CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("short");
            UnsafeCode = true;

            void* address = (void*)Marshal.AllocHGlobal(sizeof(short));
            bool runResult = (bool)Run<I2>("", "Test", "AssignValueToPointer", a, (IntPtr)address);
            bool success = (*(short*)address == a);
            Marshal.FreeHGlobal((IntPtr)address);

            Assert.IsTrue(success, "DereffedVoidPtrAssign_I2");
        }

        #endregion I2

        #region I3

        delegate bool I3(int value, void* address);

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
        public unsafe void DereffedVoidPtrAssign_I3(int a)
        {
            CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("int");
            UnsafeCode = true;

            void* address = (void*)Marshal.AllocHGlobal(sizeof(int));
            bool runResult = (bool)Run<I3>("", "Test", "AssignValueToPointer", a, (IntPtr)address);
            bool success = (*(int*)address == a);
            Marshal.FreeHGlobal((IntPtr)address);

            Assert.IsTrue(success, "DereffedVoidPtrAssign_I3");
        }

        #endregion I3

        #region I4

        delegate bool I4(long value, void* address);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Row(long.MinValue)]
        [Row(-1)]
        [Row(0)]
        [Row(1)]
        [Row(long.MaxValue)]
        [Test, Author("illuminus", "illuminus86@gmail.com")]
        public unsafe void DereffedVoidPtrAssign_I4(long a)
        {
            CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("long");
            UnsafeCode = true;

            void* address = (void*)Marshal.AllocHGlobal(sizeof(long));
            bool runResult = (bool)Run<I4>("", "Test", "AssignValueToPointer", a, (IntPtr)address);
            bool success = (*(int*)address == a);
            Marshal.FreeHGlobal((IntPtr)address);

            Assert.IsTrue(success, "DereffedVoidPtrAssign_I4");
        }

        #endregion I4

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
            CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("byte");
            UnsafeCode = true;

            void* address = (void*)Marshal.AllocHGlobal(sizeof(byte));
            bool runResult = (bool)Run<U1>("", "Test", "AssignValueToPointer", a, (IntPtr)address);
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
            CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("ushort");
            UnsafeCode = true;

            void* address = (void*)Marshal.AllocHGlobal(sizeof(ushort));
            bool runResult = (bool)Run<U2>("", "Test", "AssignValueToPointer", a, (IntPtr)address);
            bool success = (*(ushort*)address == a);
            Marshal.FreeHGlobal((IntPtr)address);

            Assert.IsTrue(success, "DereffedVoidPtrAssign_U2");
        }

        #endregion U2

        #region U3

        delegate bool U3(uint value, void* address);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Row(uint.MinValue)]
        [Row((uint.MaxValue / 2))]
        [Row((uint.MaxValue / 2) + 1)]
        [Row(uint.MaxValue)]
        [Test, Author("illuminus", "illuminus86@gmail.com")]
        public unsafe void DereffedVoidPtrAssign_U3(uint a)
        {
            CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("uint");
            UnsafeCode = true;

            void* address = (void*)Marshal.AllocHGlobal(sizeof(uint));
            bool runResult = (bool)Run<U3>("", "Test", "AssignValueToPointer", a, (IntPtr)address);
            bool success = (*(uint*)address == a);
            Marshal.FreeHGlobal((IntPtr)address);

            Assert.IsTrue(success, "DereffedVoidPtrAssign_U3");
        }

        #endregion U3

        #region U4

        delegate bool U4(ulong value, void* address);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Row(ulong.MinValue)]
        [Row((ulong.MaxValue / 2)+1)]
        [Row(ulong.MaxValue)]
        [Test, Author("illuminus", "illuminus86@gmail.com")]
        public unsafe void DereffedVoidPtrAssign_u4(ulong a)
        {
            CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("ulong");
            UnsafeCode = true;

            void* address = (void*)Marshal.AllocHGlobal(sizeof(ulong));
            bool runResult = (bool)Run<U4>("", "Test", "AssignValueToPointer", a, (IntPtr)address);
            bool success = (*(ulong*)address == a);
            Marshal.FreeHGlobal((IntPtr)address);

            Assert.IsTrue(success, "DereffedVoidPtrAssign_U4");
        }

        #endregion U4

        #endregion
    }
}
