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
using System.Diagnostics;
using System.IO;

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
        /// Holds the assembly compiler.
        /// </summary>
        private AssemblyCompiler compiler;

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
            // Save the compiler
            this.compiler = compiler;
            // The compilation target architecture
            this.architecture = compiler.Architecture;
            // The type system
            this.typeSystem = RuntimeBase.Instance.TypeLoader;

            // Enumerate all types and do an appropriate type layout
            ReadOnlyRuntimeTypeListView types = this.typeSystem.GetTypesFromModule(compiler.Assembly);
            foreach (RuntimeType type in types)
            {
                switch (type.Attributes & TypeAttributes.LayoutMask)
                {
                    case TypeAttributes.AutoLayout: goto case TypeAttributes.SequentialLayout;

                    case TypeAttributes.SequentialLayout:
                        CreateSequentialLayout(type);
                        break;

                    case TypeAttributes.ExplicitLayout:
                        CreateExplicitLayout(type);
                        break;
                }
            }
        }

        /// <summary>
        /// Performs a sequential layout of the type.
        /// </summary>
        /// <param name="type">The type.</param>
        private void CreateSequentialLayout(RuntimeType type)
        {
            Debug.Assert(type != null, @"No type given.");

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
                if ((field.Attributes & FieldAttributes.Static) == FieldAttributes.Static)
                {
                    // Assign a memory slot to the static & initialize it, if there's initial data set
                    CreateStaticField(field);
                }
                else
                {
                    this.architecture.GetTypeRequirements(field.Type, out size, out alignment);

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

        /// <summary>
        /// Applies the explicit layout to the given type.
        /// </summary>
        /// <param name="type">The type.</param>
        private void CreateExplicitLayout(RuntimeType type)
        {
            Debug.Assert(type != null, @"No type given.");
            Debug.Assert(type.Size != 0, @"Type size not set for explicit layout.");
            foreach (RuntimeField field in type.Fields)
            {
                if ((field.Attributes & FieldAttributes.Static) == FieldAttributes.Static)
                {
                    // Assign a memory slot to the static & initialize it, if there's initial data set
                    CreateStaticField(field);
                }
                else
                {
                    // Explicit layout assigns a physical offset from the start of the structure
                    // to the field. We just assign this offset.
                    Debug.Assert(field.Address.ToInt64() != 0, @"Non-static field doesn't have layout!");
                }
            }
        }

        /// <summary>
        /// Allocates memory for the static field and initializes it.
        /// </summary>
        /// <param name="field">The field.</param>
        private void CreateStaticField(RuntimeField field)
        {
            Debug.Assert(field != null, @"No field given.");

            // Determine the size of the type & alignment requirements
            int size, alignment;
            this.architecture.GetTypeRequirements(field.Type, out size, out alignment);

            // Retrieve the linker
            IAssemblyLinker linker = compiler.Pipeline.Find<IAssemblyLinker>();
            // The linker section to move this field into
            LinkerSection section;
            // Does this field have an RVA?
            if (IntPtr.Zero != field.RVA)
            {
                // FIXME: Move a static field into ROData, if it is read-only and can be initialized
                // using static analysis
                section = LinkerSection.Data;
            }
            else
            {
                section = LinkerSection.BSS;
            }

            // Allocate space in the respective section
            using (Stream stream = linker.Allocate(field, section, size, alignment))
            {
                field.Address = new IntPtr(stream.Position);
                if (IntPtr.Zero != field.RVA)
                {
                    // Initialize the static value from the RVA
                    using (Stream source = compiler.Assembly.GetDataSection(field.RVA.ToInt64()))
                    {
                        byte[] data = new byte[size];
                        source.Read(data, 0, size);
                        stream.Write(data, 0, size);
                    }
                }
                else
                {
                    // Write dummy bytes...
                    stream.Write(new byte[size], 0, size);
                }
            }
        }

        #endregion // IAssemblyCompilerStage members
    }
}
