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
using Mosa.Runtime.Metadata.Signatures;
using System.Collections.Generic;

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
		private AssemblyCompiler compiler;

        private IAssemblyLinker linker;

        private int nativePointerAlignment;

        private int nativePointerSize;

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
		public string Name 
		{ 
			get 
			{ 
				return @"Type Layout"; 
			} 
		}
		
		public void Setup(AssemblyCompiler compiler)
		{
			this.compiler = compiler;
			this._architecture = compiler.Architecture;
            this.linker = compiler.Pipeline.FindFirst<IAssemblyLinker>();
            this._typeSystem = RuntimeBase.Instance.TypeLoader;

            Debug.Assert(this.linker != null, @"Failed to retrieve linker from assembly compiler.");
			

            this._architecture.GetTypeRequirements(BuiltInSigType.IntPtr, out this.nativePointerSize, out this.nativePointerAlignment);
		}
		
		public void Run()
		{
			// Enumerate all types and do an appropriate type layout
			ReadOnlyRuntimeTypeListView types = _typeSystem.GetTypesFromModule(this.compiler.Assembly);
			foreach (RuntimeType type in types) 
			{
				if (type.IsGeneric == true || type.IsDelegate == true)
					continue;
				
				switch (type.Attributes & TypeAttributes.LayoutMask) 
				{
					case TypeAttributes.AutoLayout:
                        this.CreateSequentialLayout(type);
                        break;

					case TypeAttributes.SequentialLayout:
						this.CreateSequentialLayout(type);
						break;

					case TypeAttributes.ExplicitLayout:
						this.CreateExplicitLayout(type);
						break;
				}

                this.BuildMethodTable(type);
			}
		}

        private void BuildMethodTable(RuntimeType type)
        {
            IList<RuntimeMethod> methodTable = this.CreateMethodTable(type);
            this.AskLinkerToCreateMethodTable(type, methodTable);
        }

        private List<RuntimeMethod> CreateMethodTable(RuntimeType type)
        {
            List<RuntimeMethod> methodTable = this.CreateMethodTableList(type);

            foreach (RuntimeMethod method in type.Methods)
            {
                MethodAttributes attributes = method.Attributes;
                if ((attributes & MethodAttributes.Virtual) == MethodAttributes.Virtual)
                {
                    int slot = methodTable.Count;

                    if ((attributes & MethodAttributes.NewSlot) != MethodAttributes.NewSlot)
                    {
                        slot = FindOverrideSlot(methodTable, method);
                    }

                    method.MethodTableSlot = slot;
                    if (slot == methodTable.Count)
                    {
                        methodTable.Add(method);
                    }
                    else
                    {
                        methodTable[slot] = method;
                    }
                }                
            }

            return methodTable;
        }

        private int FindOverrideSlot(List<RuntimeMethod> methodTable, RuntimeMethod method)
        {
            int slot = methodTable.Count;

            foreach (RuntimeMethod baseMethod in methodTable)
            {
                if (baseMethod.Name.Equals(method.Name) && baseMethod.Signature.Matches(method.Signature))
                {
                    slot = baseMethod.MethodTableSlot;
                    break;
                }
            }

            return slot;
        }

        private List<RuntimeMethod> CreateMethodTableList(RuntimeType type)
        {
            List<RuntimeMethod> methodTable;

            if (type.BaseType == null)
            {
                methodTable = new List<RuntimeMethod>();
            }
            else
            {
                IList<RuntimeMethod> baseMethodTable = type.BaseType.MethodTable;
                if (baseMethodTable == null)
                {
                    baseMethodTable = this.CreateMethodTable(type.BaseType);
                }

                methodTable = new List<RuntimeMethod>(baseMethodTable);
            }

            type.MethodTable = methodTable;
            return methodTable;
        }

        private void AskLinkerToCreateMethodTable(RuntimeType type, IList<RuntimeMethod> methodTable)
        {
            // HINT: The method table is offset by a single pointer, which contains the type information 
            // pointer. Used to realize object.GetType()

            string methodTableSymbolName = type.FullName + @"$mtable";
            int methodTableSize = this.nativePointerSize + methodTable.Count * this.nativePointerSize;

            using (Stream stream = this.linker.Allocate(methodTableSymbolName, SectionKind.Text, methodTableSize, this.nativePointerAlignment))
            {
                stream.Position = methodTableSize;
            }

            int offset = this.nativePointerSize;

            foreach (RuntimeMethod method in methodTable)
            {
                string methodSymbol = method.ToString();

                this.linker.Link(LinkType.AbsoluteAddress | LinkType.I4, methodTableSymbolName, offset, 0, methodSymbol, IntPtr.Zero);
                offset += this.nativePointerSize;
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
            {
                typeSize = baseType.Size;
            }
            else
            {
                typeSize = CalculateInitialFieldOffset(type);
            }

			foreach (RuntimeField field in type.Fields) {
				if ((field.Attributes & FieldAttributes.Static) == FieldAttributes.Static) {
					// Assign a memory slot to the static & initialize it, if there's initial data set
                    this.CreateStaticField(field);
				}
				else {
					int size;
					int alignment;
                    this._architecture.GetTypeRequirements(field.SignatureType, out size, out alignment);

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

        private int CalculateInitialFieldOffset(RuntimeType type)
        {
            int offset = 0;
            if (type.IsValueType == false)
            {
                //
                // We make 8 bytes room at the start of an object to accomodate the method table pointer
                // and a ptr to runtime/GC data.
                //
                offset = 2 * this.nativePointerSize;
            }

            return offset;
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
                    this.CreateStaticField(field);
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
            this._architecture.GetTypeRequirements(field.SignatureType, out size, out alignment);

            if (field.SignatureType.Type == CilElementType.ValueType)
                size = ObjectModelUtility.ComputeTypeSize(field.DeclaringType, (field.SignatureType as Metadata.Signatures.ValueTypeSigType).Token, this.compiler.Metadata, _architecture);

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

			this.AllocateSpace(field, section, size, alignment);
		}

		private void AllocateSpace(RuntimeField field, SectionKind section, int size, int alignment)
		{
			using (Stream stream = this.linker.Allocate(field.ToString(), section, size, alignment)) {
                if (IntPtr.Zero != field.RVA)
                {
                    this.InitializeStaticValueFromRVA(stream, size, field);
                }
                else
                {
                    WriteDummyBytes(stream, size);
                }
			}
		}

		private void InitializeStaticValueFromRVA(Stream stream, int size, RuntimeField field)
		{
			using (Stream source = this.compiler.Assembly.GetDataSection(field.RVA.ToInt64())) {
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
