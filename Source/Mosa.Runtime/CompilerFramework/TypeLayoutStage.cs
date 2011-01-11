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
using System.Collections.Generic;

using Mosa.Compiler.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Vm;
using Mosa.Runtime.Metadata.Signatures;

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
				if (type.ContainsOpenGenericParameters)
					continue;

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
				if (type.ContainsOpenGenericParameters)
					continue;

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
					BuildTypeInterfaceBitmap(type);
					BuildTypeInterfaceTables(type);
				}

				AllocateStaticFields(type);
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

		int ITypeLayout.GetTypeSize(RuntimeType type)
		{
			int size = 0;
			foreach (RuntimeField field in type.Fields)
			{
				if (!field.IsStaticField)
					size = size + ((ITypeLayout)this).GetFieldSize(field);
			}

			return size;
		}

		int ITypeLayout.GetFieldSize(RuntimeField field)
		{
			// If the field is another struct, we have to dig down and compute its size too.
			if (field.SignatureType.Type == CilElementType.ValueType)
			{
				return ((ITypeLayout)this).GetTypeSize(field.DeclaringType);
			}

			int size, alignment;
			architecture.GetTypeRequirements(field.SignatureType, out size, out alignment);

			return size;
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

		private void BuildTypeInterfaceBitmap(RuntimeType type)
		{
			if (type.Interfaces.Count == 0)
				return;

			byte[] bitmap = new byte[(((interfaces.Count - 1) / sizeof(byte)) + 1)];

			int at = 0;
			byte bit = 0;
			foreach (RuntimeType interfaceType in interfaces)
			{
				if (type.Interfaces.Contains(interfaceType))
					bitmap[at] = (byte)(bitmap[at] | (byte)(1 << bit));

				bit++;
				if (bit == sizeof(byte))
				{
					bit = 0;
					at++;
				}
			}

			AskLinkerToCreateArray(type.FullName + @"$ibitmap", bitmap);
		}

		private void BuildInterfaceTable(RuntimeType type, RuntimeType interfaceType)
		{
			if (type.Interfaces.Count == 0)
				return;
			
			RuntimeMethod[] methodTable = new RuntimeMethod[interfaceType.Methods.Count];

			// Implicit Interface Methods
			for (int slot = 0; slot < interfaceType.Methods.Count; slot++)
				methodTable[slot] = FindInterfaceMethod(type, interfaceType.Methods[slot]);

			// Explicit Interface Methods
			ScanExplicitInterfaceImplementations(type, interfaceType.Methods, methodTable);

			AskLinkerToCreateMethodTable(type.FullName + @"$mtable$" + interfaceType.FullName, methodTable, null);
		}

		private void ScanExplicitInterfaceImplementations(RuntimeType type, IList<RuntimeMethod> interfaceMethods, RuntimeMethod[] methodTable)
		{
			IMetadataProvider metadata = type.MetadataModule.Metadata;
			TokenTypes maxToken = metadata.GetMaxTokenValue(TokenTypes.MethodImpl);

			for (TokenTypes token = TokenTypes.MethodImpl + 1; token <= maxToken; token++)
			{
				MethodImplRow row = metadata.ReadMethodImplRow(token);
				if (row.ClassTableIdx == (TokenTypes)type.Token)
				{
					int slot = 0;
					foreach (RuntimeMethod interfaceMethod in interfaceMethods)
					{
						if ((TokenTypes)interfaceMethod.Token == (row.MethodDeclarationTableIdx & TokenTypes.RowIndexMask))
						{
							methodTable[slot] = FindMethodByToken(type, row.MethodBodyTableIdx);
						}
						slot++;
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

		private void AddImplicitInterfaceImplementations(RuntimeType type, IList<RuntimeMethod> interfaceMethods, IList<RuntimeMethod> methodTable)
		{
			for (int slot = 0; slot < methodTable.Count; slot++)
			{
				if (methodTable[slot] == null)
				{
					methodTable[slot] = FindInterfaceMethod(type, interfaceMethods[slot]);
				}
			}
		}

		private RuntimeMethod FindInterfaceMethod(RuntimeType type, RuntimeMethod interfaceMethod)
		{
			foreach (RuntimeMethod method in type.Methods)
			{				
				string cleanInterfaceMethodName = GetCleanMethodName (interfaceMethod.Name);
				string cleanMethodName = GetCleanMethodName (method.Name);
				
				if (cleanInterfaceMethodName.Equals(cleanMethodName))
				{
					if (interfaceMethod.Signature.Matches(method.Signature))
					{
						return method;
					}
				}
			}
			
			if (type.BaseType != null)
			{
				System.Console.WriteLine("Descending from " + type + " to " + type.BaseType);
				return FindInterfaceMethod(type.BaseType, interfaceMethod);
			}
			throw new InvalidOperationException(@"Failed to find implicit interface implementation for type " + type + " and interface method " + interfaceMethod);
		}
		
		private string GetCleanMethodName (string fullName)
		{
			if (!fullName.Contains("."))
				return fullName;
			return fullName.Substring(fullName.LastIndexOf(".") + 1);
		}

		/// <summary>
		/// Builds the method table.
		/// </summary>
		/// <param name="type">The type.</param>
		public void BuildMethodTable(RuntimeType type)
		{
			IList<RuntimeMethod> methodTable = CreateMethodTable(type);

			// HINT: The method table is offset by a four pointers:
			// 1. interface dispatch table pointer
			// 2. type pointer - contains the type information pointer, used to realize object.GetType().
			// 3. interface bitmap
			// 4. parent type (if any)
			List<string> headerlinks = new List<string>();

			// 1. interface dispatch table pointer
			if (type.Interfaces.Count == 0)
				headerlinks.Add(null);
			else
				headerlinks.Add(type.FullName + @"$itable");

			// 2. type pointer - contains the type information pointer, used to realize object.GetType().
			headerlinks.Add(null); // TODO: GetType()

			// 3. interface bitmap
			if (type.Interfaces.Count == 0)
				headerlinks.Add(null);
			else
				headerlinks.Add(type.FullName + @"$ibitmap");

			// 4. parent type (if any)
			if (type.BaseType == null)
				headerlinks.Add(null);
			else
				headerlinks.Add(type.BaseType + @"$mtable");

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
				if ((method.Attributes & MethodAttributes.Virtual) == MethodAttributes.Virtual)
				{
					int slot = methodTable.Count;

					if ((method.Attributes & MethodAttributes.NewSlot) != MethodAttributes.NewSlot)
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

			Debug.WriteLine("Method Table: " + methodTableName);

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
						Debug.WriteLine("  # " + (offset / nativePointerSize).ToString() + " " + link);
						linker.Link(LinkType.AbsoluteAddress | LinkType.I4, methodTableName, offset, 0, link, IntPtr.Zero);
					}
					else
					{
						Debug.WriteLine("  # " + (offset / nativePointerSize).ToString() + " [null]");
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
						Debug.WriteLine("  # " + (offset / nativePointerSize).ToString() + " " + method.ToString());
						linker.Link(LinkType.AbsoluteAddress | LinkType.I4, methodTableName, offset, 0, method.ToString(), IntPtr.Zero);
					}
					else
					{
						Debug.WriteLine("  # " + (offset / nativePointerSize).ToString() + " [null]");
					}
					offset += nativePointerSize;
				}
			}
		}

		private void AskLinkerToCreateArray(string tableName, byte[] array)
		{
			int size = array.Length;

			using (Stream stream = linker.Allocate(tableName, SectionKind.Text, size, nativePointerAlignment))
			{
				foreach (byte b in array)
					stream.WriteByte(b);

				stream.Position = size;
			}
		}

		/// <summary>
		/// Performs a sequential layout of the type.
		/// </summary>
		/// <param name="type">The type.</param>
		public void CreateSequentialLayout(RuntimeType type)
		{
			Debug.Assert(type != null, @"No type given.");

			// Receives the size/alignment
			int packingSize = type.Pack;
			// Instance size
			int typeSize = 8;

			int fieldSize;
			int typeAlignment;

			RuntimeType baseType = type.BaseType;
			if (baseType != null)
			{
				CreateSequentialLayout(baseType);
				typeSize = baseType.Size - 8;
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

			size = (int)((ITypeLayout)this).GetFieldSize(field);

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
