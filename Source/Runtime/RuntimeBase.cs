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

        /// <summary>
        /// This function requests allocation of a specific runtime type.
        /// </summary>
        /// <param name="type">The type of object to allocate.</param>
        /// <param name="elements">The number of elements to allocate.</param>
        /// <returns></returns>
        /// <remarks>
        /// The allocated object is not constructed, e.g. the caller must invoke
        /// the appropriate constructor in order to obtain a real object.
        /// </remarks>
        [VmCall(VmCall.Allocate)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static IntPtr Allocate(RuntimeType type, int elements);

        /// <summary>
        /// Boxes the specified value type.
        /// </summary>
        /// <param name="valueType">Type of the value.</param>
        /// <returns>The boxed value type.</returns>
        [VmCall(VmCall.Box)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object Box(ValueType valueType);

        /// <summary>
        /// This function performs the cast operation and type checking.
        /// </summary>
        /// <param name="obj">The object to cast.</param>
        /// <param name="typeHandle">Handle to the type to cast to.</param>
        /// <returns>
        /// The cast object if type checks were successful.
        /// </returns>
        [VmCall(VmCall.Castclass)]
        [MethodImpl(MethodImplOptions.InternalCall)]
		public extern static object Castclass(object obj, UIntPtr typeHandle);

        /// <summary>
        /// This function performs the isinst operation and type checking.
        /// </summary>
        /// <param name="obj">The object to cast.</param>
        /// <param name="typeHandle">Handle to the type to cast to.</param>
        /// <returns>
        /// The cast object if type checks were successful. Otherwise null.
        /// </returns>
        [VmCall(VmCall.IsInstanceOfType)]
        [MethodImpl(MethodImplOptions.InternalCall)]
		public extern static bool IsInstanceOfType(object obj, UIntPtr typeHandle);

        /// <summary>
        /// Copies bytes From the source to destination.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="source">The source.</param>
        /// <param name="count">The number of bytes to copy.</param>
        [VmCall(VmCall.Memcpy)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern unsafe static void Memcpy(byte* destination, byte* source, int count);

        /// <summary>
        /// Fills the destination with <paramref name="value"/>.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="value">The value.</param>
        /// <param name="count">The number of bytes to fill.</param>
        [VmCall(VmCall.Memset)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern unsafe static void Memset(byte* destination, byte value, int count);

        /// <summary>
        /// Rethrows the current exception.
        /// </summary>
        [VmCall(VmCall.Rethrow)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void Rethrow();

        /// <summary>
        /// Throws the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        [VmCall(VmCall.Throw)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void Throw(object exception);

        /// <summary>
        /// Unboxes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="valueType">The value type to unbox.</param>
        [VmCall(VmCall.Unbox)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void Unbox(object obj, ValueType valueType);

        #endregion // Internal Call Prototypes

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
