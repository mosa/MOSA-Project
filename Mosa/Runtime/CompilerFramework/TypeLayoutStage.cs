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
using Mosa.Runtime.Vm;
using Mosa.Runtime.Metadata;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Performs memory layout of a type for compilation.
    /// </summary>
    public sealed class TypeLayoutStage : IAssemblyCompilerStage
    {
        #region Data members

        /// <summary>
        /// Holds the architecture during compilation.
        /// </summary>
        private IArchitecture architecture;

        /// <summary>
        /// Holds the current type system during compilation.
        /// </summary>
        private ITypeSystem typeSystem;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeLayoutStage"/> class.
        /// </summary>
        public TypeLayoutStage()
        {
        }

        #endregion // Construction

        #region IAssemblyCompilerStage members

        string IAssemblyCompilerStage.Name
        {
            get { return @"Type Layout"; }
        }

        void IAssemblyCompilerStage.Run(AssemblyCompiler compiler)
        {
            // The compilation target architecture
            this.architecture = compiler.Architecture;
            // The type system
            this.typeSystem = RuntimeBase.Instance.TypeLoader;

            // Enumerate all types and do an appropriate type layout
            ReadOnlyRuntimeTypeListView types = this.typeSystem.GetTypesFromModule(compiler.Assembly);
            foreach (RuntimeType type in types)
            {
                if (0 == type.Size)
                {
                    CreateSequentialLayout(type);
                }
            }
        }

        private void CreateSequentialLayout(RuntimeType type)
        {
            // Receives the size/alignment
            int size, alignment;
            int packingSize = type.Pack;
            // Instance size
            int typeSize = 0;

            RuntimeType baseType = null;
            if (0 != type.Extends)
                baseType = this.typeSystem.Types[type.Extends];
            if (null != baseType)
                typeSize = baseType.Size;

            foreach (RuntimeField field in type.Fields)
            {
                this.architecture.GetTypeRequirements(field.Type, out size, out alignment);
                if ((field.Attributes & FieldAttributes.Static) == FieldAttributes.Static)
                {
                    // FIXME: Reserve space for this static field...
                }
                else
                {
                    // Pad the field in the type
                    if (0 != packingSize)
                    {
                        int padding = (typeSize % packingSize);
                        typeSize += padding;
                    }

                    // Set the field address
                    field.Address = new IntPtr(typeSize);
                    typeSize += size;
                }
            }

            type.Size = typeSize;
        }

        private void CreateExplicitLayout()
        {
        }

        #endregion // IAssemblyCompilerStage members
    }
}
