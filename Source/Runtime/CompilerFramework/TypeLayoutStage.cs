/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Diagnostics;
using System.IO;

using Mosa.Runtime.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Performs memory layout of a type for compilation.
	/// </summary>
	public sealed class TypeLayoutStage : IAssemblyCompilerStage, IPipelineStage
	{
		#region Data members

		/// <summary>
		/// Holds the Architecture during compilation.
		/// </summary>
		private IArchitecture _architecture;

		/// <summary>
		/// Holds the assembly Compiler.
		/// </summary>
		private AssemblyCompiler _compiler;

		/// <summary>
		/// Holds the current type system during compilation.
		/// </summary>
		private ITypeSystem _typeSystem;

		#endregion // Data members

		#region IAssemblyCompilerStage members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"Type Layout"; } }

		void IAssemblyCompilerStage.Run(AssemblyCompiler compiler)
		{
			// Save the Compiler
			_compiler = compiler;
			// The compilation target Architecture
			_architecture = compiler.Architecture;
			// The type system
			_typeSystem = RuntimeBase.Instance.TypeLoader;

			// Enumerate all types and do an appropriate type layout
			ReadOnlyRuntimeTypeListView types = _typeSystem.GetTypesFromModule(compiler.Assembly);
			foreach (RuntimeType type in types) {
				switch (type.Attributes & TypeAttributes.LayoutMask) {
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
			int packingSize = type.Pack;
			// Instance size
			int typeSize = 0;

			RuntimeType baseType = type.BaseType;
			if (null != baseType)
				typeSize = baseType.Size;

			foreach (RuntimeField field in type.Fields) {
				if ((field.Attributes & FieldAttributes.Static) == FieldAttributes.Static) {
					// Assign a memory slot to the static & initialize it, if there's initial data set
					CreateStaticField(field);
				}
				else {
					int size;
					int alignment;
					_architecture.GetTypeRequirements(field.Type, out size, out alignment);

					// Pad the field in the type
					if (0 != packingSize) {
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
			Debug.Assert(type.BaseType.Size != 0, @"Type size not set for explicit layout.");
			foreach (RuntimeField field in type.Fields) {
				if ((field.Attributes & FieldAttributes.Static) == FieldAttributes.Static) {
					// Assign a memory slot to the static & initialize it, if there's initial data set
					CreateStaticField(field);
				}
				else {
					// Explicit layout assigns a physical offset From the start of the structure
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
			_architecture.GetTypeRequirements(field.Type, out size, out alignment);

            if (field.Type.Type == CilElementType.ValueType)
                size = ObjectModelUtility.ComputeTypeSize((field.Type as Metadata.Signatures.ValueTypeSigType).Token, _compiler.Metadata, _architecture);

			// Retrieve the linker
			IAssemblyLinker linker = _compiler.Pipeline.FindFirst<IAssemblyLinker>();
			// The linker section to move this field into
			SectionKind section;
			// Does this field have an RVA?
			if (IntPtr.Zero != field.RVA) {
				// FIXME: Move a static field into ROData, if it is read-only and can be initialized
				// using static analysis
				section = SectionKind.Data;
			}
			else {
				section = SectionKind.BSS;
			}

			AllocateSpace(linker, field, section, size, alignment);
		}

		private void AllocateSpace(IAssemblyLinker linker, RuntimeField field, SectionKind section, int size, int alignment)
		{
			using (Stream stream = linker.Allocate(field, section, size, alignment)) {
				if (IntPtr.Zero != field.RVA)
					InitializeStaticValueFromRVA(stream, size, field);
				else
					WriteDummyBytes(stream, size);
			}
		}

		private void InitializeStaticValueFromRVA(Stream stream, int size, RuntimeField field)
		{
			using (Stream source = _compiler.Assembly.GetDataSection(field.RVA.ToInt64())) {
				byte[] data = new byte[size];
				source.Read(data, 0, size);
				stream.Write(data, 0, size);
			}
		}

		private static void WriteDummyBytes(Stream stream, int size)
		{
			stream.Write(new byte[size], 0, size);
		}

		#endregion // IAssemblyCompilerStage members
	}
}
