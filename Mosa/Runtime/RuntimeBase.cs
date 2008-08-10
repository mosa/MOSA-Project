/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Vm;
using Mosa.Kernel.Memory;
using Mosa.Runtime.Loader;

namespace Mosa.Runtime {
	
    /// <summary>
    /// Provides central runtime entry points for various features.
    /// </summary>
    public abstract class RuntimeBase : IDisposable
    {
        #region Static data members

        private static RuntimeBase s_instance = null;        

        #endregion // Static data members

        #region Construction

        protected RuntimeBase()
        {
            s_instance = this;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Retrieves the memory manager.
        /// </summary>
        public abstract IMemoryPageManager MemoryManager
        {
            get;
        }

        /// <summary>
        /// Retrieves the type loader of the runtime.
        /// </summary>
        public abstract ITypeSystem TypeLoader
        {
            get;
        }

        public abstract IAssemblyLoader AssemblyLoader
        {
            get;
        }

        public abstract IJitService JitService
        {
            get;
        }

        public static RuntimeBase Instance
        {
            get
            {
                if (null == s_instance)
                    throw new InvalidOperationException(@"Don't have a runtime.");

                return s_instance;
            }
        }

        #endregion // Properties

        #region OpCode Support Functions

        /// <summary>
		/// This function performs the cast operation and type checking.
		/// </summary>
		/// <param name="obj">The object to cast.</param>
		/// <param name="typeHandle">Handle to the type to cast to.</param>
		/// <returns>The cast object if type checks were successful.</returns>
        [RuntimeSupport(CompilerFramework.IL.OpCode.Castclass)]
		public static object CastObjectToType(object obj, UIntPtr typeHandle)
		{
			object result = IsInstanceOf(obj, typeHandle);
			if (null == result)
				throw new InvalidCastException();

			return result;
		}

		/// <summary>
		/// This function performs the isinst operation and type checking.
		/// </summary>
		/// <param name="obj">The object to cast.</param>
		/// <param name="typeHandle">Handle to the type to cast to.</param>
		/// <returns>The cast object if type checks were successful. Otherwise null.</returns>
        [RuntimeSupport(CompilerFramework.IL.OpCode.Isinst)]
		public static object IsInstanceOf(object obj, UIntPtr typeHandle)
		{
			// FIXME: Perform the type check
			return null;
		}

		/// <summary>
		/// This function requests allocation of a specific runtime type.
		/// </summary>
		/// <param name="type">The type of object to allocate.</param>
		/// <param name="size">The number of elements to allocate.</param>
		/// <returns></returns>
        /// <remarks>
        /// The allocated object is not constructed, e.g. the caller must invoke
        /// the appropriate constructor in order to obtain a real object.
        /// </remarks>
        [
            RuntimeSupport(CompilerFramework.IL.OpCode.Newobj),
            RuntimeSupport(CompilerFramework.IL.OpCode.Newarr)
        ]
		public static object Allocate(RuntimeType type, int elements)
		{
			throw new NotImplementedException();
        }

        #endregion // OpCode Support Functions

        #region IDisposable Members

        public void Dispose()
        {
            IAssemblyLoader al = this.AssemblyLoader;
            foreach (IMetadataModule module in al.Modules)
                al.Unload(module);

            s_instance = null;
        }

        #endregion // IDisposable Members
    }
}
