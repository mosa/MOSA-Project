/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Runtime.Vm;
using Mosa.Runtime.Memory;
using Mosa.Runtime.Loader;
using System.Runtime.CompilerServices;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime {
	
    /// <summary>
    /// Provides central runtime entry points for various features.
    /// </summary>
    public abstract class RuntimeBase : IDisposable
    {
        #region Static data members

        /// <summary>
        /// Holds the static instance of the runtime.
        /// </summary>
        private static RuntimeBase _instance = null;        

        #endregion // Static data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeBase"/> class.
        /// </summary>
        protected RuntimeBase()
        {
            _instance = this;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Retrieves the memory manager.
        /// </summary>
        /// <value>The memory manager.</value>
        public abstract IMemoryPageManager MemoryManager
        {
            get;
        }

        /// <summary>
        /// Retrieves the type loader of the runtime.
        /// </summary>
        /// <value>The type loader.</value>
        public abstract ITypeSystem TypeLoader
        {
            get;
        }

        /// <summary>
        /// Gets the assembly loader.
        /// </summary>
        /// <value>The assembly loader.</value>
        public abstract IAssemblyLoader AssemblyLoader
        {
            get;
        }

        /// <summary>
        /// Gets the JIT service.
        /// </summary>
        /// <value>The JIT service.</value>
        public abstract IJitService JitService
        {
            get;
        }

        /// <summary>
        /// Gets the instance of the runtime.
        /// </summary>
        /// <value>The instance.</value>
        public static RuntimeBase Instance
        {
            get
            {
                if (null == _instance)
                    throw new InvalidOperationException(@"Don't have a runtime.");

                return _instance;
            }
        }

        #endregion // Properties

        #region Internal Call Prototypes

        [VmCall(VmCall.AllocateObject)]
        public static unsafe IntPtr AllocateObject(int moduleLoadIndex, TokenTypes token)
        {
            IMetadataModule module = Instance.AssemblyLoader.GetModule(moduleLoadIndex);
            RuntimeType classType = Instance.TypeLoader.GetType(DefaultSignatureContext.Instance, module, token);

            // HACK: Add compiler architecture to the runtime
            uint nativeIntSize = 4;

            //
            // An object has the following memory layout:
            //   - IntPtr MTable
            //   - IntPtr SyncBlock
            //   - 0 .. n object data fields
            //
            ulong allocationSize = (ulong)((2 * nativeIntSize) + classType.Size);

            IntPtr memory = Instance.MemoryManager.Allocate(IntPtr.Zero, allocationSize, PageProtectionFlags.Read | PageProtectionFlags.Write | PageProtectionFlags.WriteCombine);
            if (memory == IntPtr.Zero)
            {
                throw new OutOfMemoryException();
            }

            int* destination = (int*)memory.ToInt32();
            Memset((byte*)destination, 0, (int)allocationSize);
            destination[0] = 0; // FIXME: Insert method table of the class here.
            destination[1] = 0; // No sync block initially

            return memory;            
        }

        /// <summary>
        /// This function requests allocation of a specific runtime type.
        /// </summary>
        /// <param name="moduleLoadIndex">The load index of the module providing the scope for the token to allocate.</param>
        /// <param name="token">The token of the type to allocate.</param>
        /// <param name="elements">The number of elements to allocate of the type.</param>
        /// <returns>A ptr to the allocated memory.</returns>
        /// <remarks>
        /// The allocated object is not constructed, e.g. the caller must invoke
        /// the appropriate constructor in order to obtain a real object. The object header
        /// has been set.
        /// </remarks>
        [VmCall(VmCall.AllocateArray)]
        public static unsafe IntPtr AllocateArray(int moduleLoadIndex, TokenTypes token, int elements)
        {
            if (elements < 0)
            {
                throw new OverflowException();
            }

            IMetadataModule module = Instance.AssemblyLoader.GetModule(moduleLoadIndex);
            RuntimeType elementType = Instance.TypeLoader.GetType(DefaultSignatureContext.Instance, module, token);

            // HACK: Add compiler architecture to the runtime
            uint nativeIntSize = 4;

            uint elementSize;
            if (elementType.IsValueType == true)
            {
                elementSize = (uint)elementType.Size;

                // HACK: Can't retrieve the size of value types deterministically right now, type layout must be done by the runtime.
                // Things like System.Int32 don't provide a size right now.
                if (elementSize == 0)
                {
                    elementSize = 16;
                }
            }
            else
            {
                elementSize = nativeIntSize;
            }

            //
            // An array has the following memory layout:
            //   - IntPtr MTable
            //   - IntPtr SyncBlock
            //   - int length
            //   - ElementType[length] elements
            //
            ulong allocationSize = (ulong)((3 * nativeIntSize) + (elements * elementSize));

            IntPtr memory = Instance.MemoryManager.Allocate(IntPtr.Zero, allocationSize, PageProtectionFlags.Read | PageProtectionFlags.Write | PageProtectionFlags.WriteCombine);
            if (memory == IntPtr.Zero)
            {
                throw new OutOfMemoryException();
            }

            int* destination = (int*)memory.ToInt32();
            Memset((byte*)(destination + 3), 0, (int)allocationSize);
            destination[0] = 0; // FIXME: Insert method table of Array here.
            destination[1] = 0; // No sync block initially
            destination[2] = elements;

            return memory;
        }

        /// <summary>
        /// Boxes the specified value type.
        /// </summary>
        /// <param name="valueType">Type of the value.</param>
        /// <returns>The boxed value type.</returns>
        [VmCall(VmCall.Box)]
        public static object Box(ValueType valueType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function performs the cast operation and type checking.
        /// </summary>
        /// <param name="obj">The object to cast.</param>
        /// <param name="typeHandle">Handle to the type to cast to.</param>
        /// <returns>
        /// The cast object if type checks were successful.
        /// </returns>
        [VmCall(VmCall.Castclass)]
        public static object Castclass(object obj, UIntPtr typeHandle)
        {
            throw new InvalidCastException();
        }

        /// <summary>
        /// This function performs the isinst operation and type checking.
        /// </summary>
        /// <param name="obj">The object to cast.</param>
        /// <param name="typeHandle">Handle to the type to cast to.</param>
        /// <returns>
        /// The cast object if type checks were successful. Otherwise null.
        /// </returns>
        [VmCall(VmCall.IsInstanceOfType)]
        public static bool IsInstanceOfType(object obj, UIntPtr typeHandle)
        {
            return false;
        }

        /// <summary>
        /// Copies bytes From the source to destination.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="source">The source.</param>
        /// <param name="count">The number of bytes to copy.</param>
        [VmCall(VmCall.Memcpy)]
        public unsafe static void Memcpy(byte* destination, byte* source, int count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fills the destination with <paramref name="value"/>.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="value">The value.</param>
        /// <param name="count">The number of bytes to fill.</param>
        [VmCall(VmCall.Memset)]
        public unsafe static void Memset(byte* destination, byte value, int count)
        {
            // FIXME: Forward this to the architecture
        }

        /// <summary>
        /// Rethrows the current exception.
        /// </summary>
        [VmCall(VmCall.Rethrow)]
        public static void Rethrow()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        [VmCall(VmCall.Throw)]
        public static void Throw(object exception)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Unboxes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="valueType">The value type to unbox.</param>
        [VmCall(VmCall.Unbox)]
        public static void Unbox(object obj, ValueType valueType)
        {
            throw new NotImplementedException();
        }

        #endregion // Virtual Machine Call Prototypes

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            IAssemblyLoader al = AssemblyLoader;
            foreach (IMetadataModule module in al.Modules)
                al.Unload(module);

            _instance = null;
        }

        #endregion // IDisposable Members
    }
}