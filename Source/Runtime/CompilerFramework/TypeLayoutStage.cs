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
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Vm;
using Mosa.Runtime.Metadata.Signatures;
using System.Collections.Generic;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Performs memory layout of a type for compilation.
	/// </summary>
	public sealed class TypeLayoutStage : BaseAssemblyCompilerStage, IAssemblyCompilerStage, ITypeLayout
	{
		#region Data members

		private IAssemblyLinker linker;

		private int nativePointerAlignment;

		private int nativePointerSize;

		/// <summary>
		/// Holds a list of methods for each type
		/// </summary>
		private Dictionary<RuntimeType, IList<RuntimeMethod>> typeMethods = new Dictionary<RuntimeType, IList<RuntimeMethod>>();

		/// <summary>
		/// Holds a list of interfaces
		/// </summary>
		private List<RuntimeType> interfaces = new List<RuntimeType>();

		/// <summary>
		/// Holds the method table offsets value for each method
		/// </summary>
		private Dictionary<RuntimeMethod, int> methodTableOffsets = new Dictionary<RuntimeMethod, int>();

		/// <summary>
		/// Holds the slot value for each interface
		/// </summary>
		private Dictionary<RuntimeType, int> interfaceSlots = new Dictionary<RuntimeType, int>();

		#endregion // Data members

		#region IPipelineStage members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"TypeLayoutStage"; } }

		#endregion // IPipelineStage

		#region IAssemblyCompilerStage members

		void IAssemblyCompilerStage.Setup(AssemblyCompiler compiler)
		{
			base.Setup(compiler);

			linker = RetrieveAssemblyLinkerFromCompiler();

			architecture.GetTypeRequirements(BuiltInSigType.IntPtr, out nativePointerSize, out nativePointerAlignment);
		}

		void IAssemblyCompilerStage.Run()
		{

			// Enumerate all types and do an appropriate type layout
			foreach (RuntimeType type in typeSystem.GetCompiledTypes())
			{
				if (type.IsModule || type.IsGeneric || type.IsDelegate)
					continue;

				if (type.IsInterface)
				{
					// Builds the interface list and interface index values
					BuildInterfaceType(type);
				}
			}

			// Enumerate all types and do an appropriate type layout
			foreach (RuntimeType type in typeSystem.GetCompiledTypes())
			{
				if (type.IsModule || type.IsGeneric || type.IsDelegate)
					continue;

				if (type.IsInterface)
				{
					CreateInterfaceMethodTable(type);
				}
				else
				{
					if (type.IsExplicitLayoutRequestedByType)
					{
						CreateExplicitLayout(type);
					}
					else
					{
						CreateSequentialLayout(type);
					}

					BuildMethodTable(type);
					BuildTypeInterfaceSlots(type);
					BuildTypeInterfaceTables(type);
				}

				AllocateStaticFields(type);

				//int i = 0;
				//Debug.WriteLine("Type: " + type.ToString());
				//foreach (RuntimeMethod method in GetMethodTable(type))
				//{
				//    Debug.WriteLine("    " + i.ToString() + ":" + method.ToString());
				//    i++;
				//}
			}
		}

		#endregion // IAssemblyCompilerStage members

		#region ITypeLayout members

		/// <summary>
		/// Gets the method table offset.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <returns></returns>
		int ITypeLayout.GetMethodTableOffset(RuntimeMethod method)
		{
			return methodTableOffsets[method];
		}

		int ITypeLayout.GetInterfaceSlotOffset(RuntimeType type)
		{
			return interfaceSlots[type];
		}

		#endregion // ITypeLayout

		/// <summary>
		/// Builds a list of interfaces and assigns interface a unique index number
		/// </summary>
		/// <param name="type">The type.</param>
		private void BuildInterfaceType(RuntimeType type)
		{
			Debug.Assert(type.IsInterface);

			interfaces.Add(type);
			interfaceSlots.Add(type, interfaceSlots.Count);
		}

		private void BuildTypeInterfaceTables(RuntimeType type)
		{
			foreach (RuntimeType interfaceType in type.Interfaces)
			{
				BuildInterfaceTable(type, interfaceType);
			}
		}

		private void BuildTypeInterfaceSlots(RuntimeType type)
		{
			if (type.Interfaces.Count == 0)
				return;

			List<string> slots = new List<string>(interfaces.Count);

			foreach (RuntimeType interfaceType in interfaces)
			{
				if (type.Interfaces.Contains(interfaceType))
					slots.Add(type.FullName + @"$mtable$" + interfaceType.FullName);
				else
					slots.Add(null);
			}

			AskLinkerToCreateMethodTable(type.FullName + @"$itable", null, slots);
		}

		private void BuildInterfaceTable(RuntimeType type, RuntimeType interfaceType)
		{
			if (type.Interfaces.Count == 0)
				return;

			List<RuntimeMethod> methodTable = new List<RuntimeMethod>();

			ScanExplicitInterfaceImplementations(type, interfaceType.Methods, methodTable);
			AddImplicitInterfaceImplementations(type, interfaceType.Methods, methodTable);

			AskLinkerToCreateMethodTable(type.FullName + @"$mtable$" + interfaceType.FullName, methodTable, null);
		}

		private void ScanExplicitInterfaceImplementations(RuntimeType type, IList<RuntimeMethod> interfaceType, IList<RuntimeMethod> methodTable)
		{
			IMetadataProvider metadata = type.MetadataModule.Metadata;
			TokenTypes maxToken = metadata.GetMaxTokenValue(TokenTypes.MethodImpl);
			for (TokenTypes token = TokenTypes.MethodImpl + 1; token <= maxToken; token++)
			{
				MethodImplRow row = metadata.ReadMethodImplRow(token);
				if (row.ClassTableIdx == (TokenTypes)type.Token)
				{
					foreach (RuntimeMethod interfaceMethod in interfaceType)
					{
						if (interfaceMethod != null && (TokenTypes)interfaceMethod.Token == (row.MethodDeclarationTableIdx & TokenTypes.RowIndexMask))
						{
							methodTable.Add(FindMethodByToken(type, row.MethodBodyTableIdx));
						}
						else
						{
							methodTable.Add(null);
						}
					}
				}
			}
		}

		private RuntimeMethod FindMethodByToken(RuntimeType type, TokenTypes methodToken)
		{
			foreach (RuntimeMethod method in type.Methods)
			{
				if ((TokenTypes)method.Token == (methodToken & TokenTypes.RowIndexMask))
				{
					return method;
				}
			}

			throw new InvalidOperationException(@"Failed to find explicit interface method implementation.");
		}

		private void AddImplicitInterfaceImplementations(RuntimeType type, IList<RuntimeMethod> interfaceType, IList<RuntimeMethod> methodTable)
		{
			for (int slot = 0; slot < methodTable.Count; slot++)
			{
				if (methodTable[slot] == null)
				{
					methodTable[slot] = FindInterfaceMethod(type, interfaceType[slot]);
				}
			}
		}

		private RuntimeMethod FindInterfaceMethod(RuntimeType type, RuntimeMethod interfaceMethod)
		{
			foreach (RuntimeMethod method in type.Methods)
			{
				if (interfaceMethod.Name.Equals(method.Name) && interfaceMethod.Signature.Matches(method.Signature))
				{
					return method;
				}
			}

			throw new InvalidOperationException(@"Failed to find implicit interface implementation.");
		}

		/// <summary>
		/// Builds the method table.
		/// </summary>
		/// <param name="type">The type.</param>
		private void BuildMethodTable(RuntimeType type)
		{
			IList<RuntimeMethod> methodTable = CreateMethodTable(type);

			// HINT: The method table is offset by a two pointers, type pointer and interface dispatch points. 
			// The type pointer contains the type information pointer, used to realize object.GetType().
			List<string> headerlinks = new List<string>();

			if (type.Interfaces.Count == 0)
				headerlinks.Add(null);
			else
				headerlinks.Add(type.FullName + @"$itable");

			headerlinks.Add(null); // TODO: GetType()

			AskLinkerToCreateMethodTable(type.FullName + @"$mtable", methodTable, headerlinks);
		}

		/// <summary>
		/// Creates the interface method table.
		/// </summary>
		/// <param name="type">The type.</param>
		private void CreateInterfaceMethodTable(RuntimeType type)
		{
			CreateMethodTable(type);
		}

		/// <summary>
		/// Creates the method table.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		private IList<RuntimeMethod> CreateMethodTable(RuntimeType type)
		{
			IList<RuntimeMethod> methodTable = GetMethodTable(type);

			if (methodTable != null)
				return methodTable;

			methodTable = CreateMethodTableFromBaseType(type);

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

					methodTableOffsets.Add(method, slot);

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

		private int FindOverrideSlot(IList<RuntimeMethod> methodTable, RuntimeMethod method)
		{
			foreach (RuntimeMethod baseMethod in methodTable)
			{
				if (baseMethod.Name.Equals(method.Name) && baseMethod.Signature.Matches(method.Signature))
				{
					return methodTableOffsets[baseMethod];
				}
			}

			throw new InvalidOperationException(@"Failed to find override method slot.");
		}

		private IList<RuntimeMethod> GetMethodTable(RuntimeType type)
		{
			IList<RuntimeMethod> methods;

			if (typeMethods.TryGetValue(type, out methods))
				return methods;
			else
				return null;
		}

		/// <summary>
		/// Creates the method table from the base type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		private List<RuntimeMethod> CreateMethodTableFromBaseType(RuntimeType type)
		{
			List<RuntimeMethod> methodTable = new List<RuntimeMethod>();

			if (type.BaseType != null)
			{
				IList<RuntimeMethod> baseMethodTable = GetMethodTable(type.BaseType);

				if (baseMethodTable == null)
				{
					// Method table for the base type has not been create yet, so create it now
					baseMethodTable = CreateMethodTable(type.BaseType);
				}

				methodTable = new List<RuntimeMethod>(baseMethodTable);
			}

			typeMethods.Add(type, methodTable);
			return methodTable;
		}

		private void AskLinkerToCreateMethodTable(string methodTableName, IList<RuntimeMethod> methodTable, IList<string> headerlinks)
		{
			int methodTableSize = ((headerlinks == null ? 0 : headerlinks.Count) + (methodTable == null ? 0 : methodTable.Count)) * nativePointerSize;

			using (Stream stream = linker.Allocate(methodTableName, SectionKind.Text, methodTableSize, nativePointerAlignment))
			{
				stream.Position = methodTableSize;
			}

			int offset = 0;

			if (headerlinks != null)
			{
				foreach (string link in headerlinks)
				{
					if (!string.IsNullOrEmpty(link))
					{
						linker.Link(LinkType.AbsoluteAddress | LinkType.I4, methodTableName, offset, 0, link, IntPtr.Zero);
					}
					offset += nativePointerSize;
				}
			}

			if (methodTable != null)
			{
				foreach (RuntimeMethod method in methodTable)
				{
					if (!method.IsAbstract)
					{
						linker.Link(LinkType.AbsoluteAddress | LinkType.I4, methodTableName, offset, 0, method.ToString(), IntPtr.Zero);
					}
					offset += nativePointerSize;
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

			int fieldSize;
			int typeAlignment;

			RuntimeType baseType = type.BaseType;
			if (baseType != null)
			{
				typeSize = baseType.Size;
			}

			foreach (RuntimeField field in type.Fields)
			{
				architecture.GetTypeRequirements(field.SignatureType, out fieldSize, out typeAlignment);

				// Pad the field in the type
				if (packingSize != 0)
				{
					int padding = (typeSize % packingSize);
					typeSize += padding;
				}

				// Set the field address
				field.Address = new IntPtr(typeSize);
				typeSize += fieldSize;
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

			foreach (RuntimeField field in type.Fields)
			{
				// Explicit layout assigns a physical offset From the start of the structure
				// to the field. We just assign this offset.
				Debug.Assert(field.Address.ToInt64() != 0, @"Non-static field doesn't have layout!");
			}
		}

		private void AllocateStaticFields(RuntimeType type)
		{
			foreach (RuntimeField field in type.Fields)
			{
				if (field.IsStaticField && !field.IsLiteralField)
				{
					// Assign a memory slot to the static & initialize it, if there's initial data set
					CreateStaticField(field);
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
			architecture.GetTypeRequirements(field.SignatureType, out size, out alignment);

			if (field.SignatureType.Type == CilElementType.ValueType)
				size = ObjectModelUtility.ComputeTypeSize(field.DeclaringType, (field.SignatureType as Metadata.Signatures.ValueTypeSigType).Token, field.ModuleTypeSystem, architecture);

			// The linker section to move this field into
			SectionKind section;
			// Does this field have an RVA?
			if (field.RVA != 0)
			{
				// FIXME: Move a static field into ROData, if it is read-only and can be initialized
				// using static analysis
				section = SectionKind.Data;
			}
			else
			{
				section = SectionKind.BSS;
			}

			this.AllocateSpace(field, section, size, alignment);
		}

		private void AllocateSpace(RuntimeField field, SectionKind section, int size, int alignment)
		{
			using (Stream stream = linker.Allocate(field.ToString(), section, size, alignment))
			{
				if (field.RVA != 0)
				{
					InitializeStaticValueFromRVA(stream, size, field);
				}
				else
				{
					WriteDummyBytes(stream, size);
				}
			}
		}

		private void InitializeStaticValueFromRVA(Stream stream, int size, RuntimeField field)
		{
			using (Stream source = field.MetadataModule.GetDataSection((long)field.RVA))
			{
				byte[] data = new byte[size];
				source.Read(data, 0, size);
				stream.Write(data, 0, size);
			}
		}

		private static void WriteDummyBytes(Stream stream, int size)
		{
			stream.Write(new byte[size], 0, size);
		}

	}
}
