/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.Metadata.Tables;

namespace Mosa.Compiler.TypeSystem
{
	/// <summary>
	/// Performs memory layout of a type for compilation.
	/// </summary>
	public sealed class TypeLayout : ITypeLayout
	{
		#region Data members

		/// <summary>
		/// Holds the type system
		/// </summary>
		private ITypeSystem typeSystem;

		/// <summary>
		/// Holds the Native Pointer Size
		/// </summary>
		private int nativePointerSize;

		/// <summary>
		/// Holds the Native Pointer Alignment
		/// </summary>
		private int nativePointerAlignment;

		/// <summary>
		/// Holds a set of types
		/// </summary>
		private HashSet<RuntimeType> typeSet = new HashSet<RuntimeType>();

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

		/// <summary>
		/// Holds the size of each type
		/// </summary>
		private Dictionary<RuntimeType, int> typeSizes = new Dictionary<RuntimeType, int>();

		/// <summary>
		/// Holds the size of each field
		/// </summary>
		private Dictionary<RuntimeField, int> fieldSizes = new Dictionary<RuntimeField, int>();

		/// <summary>
		/// Holds the offset for each field
		/// </summary>
		private Dictionary<RuntimeField, int> fieldOffets = new Dictionary<RuntimeField, int>();

		/// <summary>
		/// Holds a list of methods for each type
		/// </summary>
		private Dictionary<RuntimeType, List<RuntimeMethod>> typeMethodTables = new Dictionary<RuntimeType, List<RuntimeMethod>>();

		#endregion // Data members

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeLayout"/> class.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="nativePointerSize">Size of the native pointer.</param>
		/// <param name="nativePointerAlignment">The native pointer alignment.</param>
		public TypeLayout(ITypeSystem typeSystem, int nativePointerSize, int nativePointerAlignment)
		{
			this.nativePointerAlignment = nativePointerAlignment;
			this.nativePointerSize = nativePointerSize;
			this.typeSystem = typeSystem;

			ResolveLayouts();
		}

		#region ITypeLayout members

		/// <summary>
		/// Gets the type system.
		/// </summary>
		/// <value>The type system.</value>
		ITypeSystem ITypeLayout.TypeSystem { get { return typeSystem; } }

		/// <summary>
		/// Gets the size of the native pointer.
		/// </summary>
		/// <value>The size of the native pointer.</value>
		int ITypeLayout.NativePointerSize { get { return nativePointerSize; } }

		/// <summary>
		/// Gets the native pointer alignment.
		/// </summary>
		/// <value>The native pointer alignment.</value>
		int ITypeLayout.NativePointerAlignment { get { return nativePointerAlignment; } }

		/// <summary>
		/// Gets the method table offset.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <returns></returns>
		int ITypeLayout.GetMethodTableOffset(RuntimeMethod method)
		{
			ResolveType(method.DeclaringType);
			return methodTableOffsets[method];
		}

		/// <summary>
		/// Gets the interface slot offset.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		int ITypeLayout.GetInterfaceSlotOffset(RuntimeType type)
		{
			ResolveType(type);
			return interfaceSlots[type];
		}

		/// <summary>
		/// Gets the size of the type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		int ITypeLayout.GetTypeSize(RuntimeType type)
		{
			ResolveType(type);

			var size = 0;
			typeSizes.TryGetValue(type, out size);

			return size;
		}

		/// <summary>
		/// Gets the size of the field.
		/// </summary>
		/// <param name="field">The field.</param>
		/// <returns></returns>
		int ITypeLayout.GetFieldSize(RuntimeField field)
		{
			var size = 0;

			if (fieldSizes.TryGetValue(field, out size))
			{
				return size;
			}
			else
			{
				ResolveType(field.DeclaringType);
			}

			// If the field is another struct, we have to dig down and compute its size too.
			if (field.SignatureType.Type == CilElementType.ValueType)
			{
				size = ((ITypeLayout)this).GetTypeSize(field.DeclaringType);
			}
			else
			{
				size = GetMemorySize(field.SignatureType);
			}

			fieldSizes.Add(field, size);

			return size;
		}

		/// <summary>
		/// Gets the size of the field.
		/// </summary>
		/// <param name="field">The field.</param>
		/// <returns></returns>
		int ITypeLayout.GetFieldOffset(RuntimeField field)
		{
			ResolveType(field.DeclaringType);

			var offset = 0;

			fieldOffets.TryGetValue(field, out offset);

			return offset;
		}

		/// <summary>
		/// Gets the type methods.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		IList<RuntimeMethod> ITypeLayout.GetMethodTable(RuntimeType type)
		{
			if (type.ContainsOpenGenericParameters)
				return null;

			if (type.IsModule || type.IsGeneric)
				return null;

			ResolveType(type);

			return typeMethodTables[type];
		}

		/// <summary>
		/// Gets the interface table.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="interfaceType">Type of the interface.</param>
		/// <returns></returns>
		RuntimeMethod[] ITypeLayout.GetInterfaceTable(RuntimeType type, RuntimeType interfaceType)
		{
			if (type.Interfaces.Count == 0)
				return null;

			ResolveType(type);

			RuntimeMethod[] methodTable = new RuntimeMethod[interfaceType.Methods.Count];

			// Implicit Interface Methods
			for (int slot = 0; slot < interfaceType.Methods.Count; slot++)
				methodTable[slot] = FindInterfaceMethod(type, interfaceType.Methods[slot]);

			// Explicit Interface Methods
			ScanExplicitInterfaceImplementations(type, interfaceType, methodTable);

			return methodTable;
		}

		/// <summary>
		/// Get a list of interfaces
		/// </summary>
		IList<RuntimeType> ITypeLayout.Interfaces { get { return interfaces.AsReadOnly(); } }

		#endregion // ITypeLayout

		#region Internal - Layout

		private void ResolveLayouts()
		{
			// Enumerate all types and do an appropriate type layout
			foreach (RuntimeType type in typeSystem.GetAllTypes())
			{
				ResolveType(type);
			}
		}

		private void ResolveType(RuntimeType type)
		{
			if (type.ContainsOpenGenericParameters)
				return;

			if (type.IsModule || type.IsGeneric)
				return;

			if (typeSet.Contains(type))
				return;

			typeSet.Add(type);

			if (type.BaseType != null)
				ResolveType(type.BaseType);

			if (type.IsInterface)
			{
				ResolveInterfaceType(type);
				CreateMethodTable(type);
				return;
			}

			foreach (RuntimeType interfaceType in type.Interfaces)
			{
				ResolveInterfaceType(interfaceType);
			}

			if (type.IsExplicitLayoutRequestedByType)
			{
				ResolveExplicitLayout(type);
			}
			else
			{
				CreateSequentialLayout(type);
			}

			CreateMethodTable(type);
		}

		/// <summary>
		/// Builds a list of interfaces and assigns interface a unique index number
		/// </summary>
		/// <param name="type">The type.</param>
		private void ResolveInterfaceType(RuntimeType type)
		{
			Debug.Assert(type.IsInterface);

			if (type.ContainsOpenGenericParameters)
				return;

			if (type.IsModule || type.IsGeneric)
				return;

			if (interfaces.Contains(type))
				return;

			interfaces.Add(type);
			interfaceSlots.Add(type, interfaceSlots.Count);
		}

		private void CreateSequentialLayout(RuntimeType type)
		{
			Debug.Assert(type != null, @"No type given.");

			if (typeSizes.ContainsKey(type))
				return;

			// Instance size
			int typeSize = 0;

			// Receives the size/alignment
			int packingSize = type.Pack;

			if (type.BaseType != null)
			{
				if (!type.IsValueType)
				{
					((ITypeLayout)(this)).GetTypeSize(type.BaseType);
					typeSize = typeSizes[type.BaseType];
				}
			}

			foreach (RuntimeField field in type.Fields)
			{
				if (!field.IsStaticField)
				{
					int fieldSize = GetMemorySize(field.SignatureType);
					//int fieldAlignment = GetAlignmentSize(field.SignatureType);

					// Pad the field in the type
					if (packingSize != 0)
					{
						int padding = (typeSize % packingSize);
						typeSize += padding;
					}

					// Set the field address
					fieldOffets.Add(field, typeSize);

					typeSize += fieldSize;
				}
			}

			typeSizes.Add(type, typeSize);
		}

		/// <summary>
		/// Applies the explicit layout to the given type.
		/// </summary>
		/// <param name="type">The type.</param>
		private void ResolveExplicitLayout(RuntimeType type)
		{
			Debug.Assert(type != null, @"No type given.");
			Debug.Assert(type.BaseType.LayoutSize != 0, @"Type size not set for explicit layout.");

			foreach (RuntimeField field in type.Fields)
			{
				// Explicit layout assigns a physical offset from the start of the structure
				// to the field. We just assign this offset.
				Debug.Assert(fieldSizes[field] != 0, @"Non-static field doesn't have layout!");
			}
		}

		#endregion

		#region Internal - Interface

		private void ScanExplicitInterfaceImplementations(RuntimeType type, RuntimeType interfaceType, RuntimeMethod[] methodTable)
		{
			//TODO: rewrite so that access directly to metadata is not required, type system should assist instead
			var metadata = type.Module.MetadataModule.Metadata;
			var maxToken = metadata.GetMaxTokenValue(TableType.MethodImpl);

			foreach (var token in new Token(TableType.MethodImpl, 1).Upto(maxToken))
			{
				MethodImplRow row = metadata.ReadMethodImplRow(token);
				if (row.@Class == type.Token)
				{
					int slot = 0;
					foreach (var interfaceMethod in interfaceType.Methods)
					{
						if (interfaceMethod.Token == row.MethodDeclaration)
						{
							methodTable[slot] = FindMethodByToken(type, row.MethodBody);
						}
						slot++;
					}
				}
			}
		}

		private RuntimeMethod FindMethodByToken(RuntimeType type, Token methodToken)
		{
			foreach (var method in type.Methods)
			{
				if (method.Token == methodToken)
				{
					return method;
				}
			}

			throw new InvalidOperationException(@"Failed to find explicit interface method implementation.");
		}

		private RuntimeMethod FindInterfaceMethod(RuntimeType type, RuntimeMethod interfaceMethod)
		{
			var cleanInterfaceMethodName = GetCleanMethodName(interfaceMethod.Name);

			foreach (var method in type.Methods)
			{
				string cleanMethodName = GetCleanMethodName(method.Name);

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
				return FindInterfaceMethod(type.BaseType, interfaceMethod);
			}

			throw new InvalidOperationException(@"Failed to find implicit interface implementation for type " + type + " and interface method " + interfaceMethod);
		}

		private string GetCleanMethodName(string fullName)
		{
			if (!fullName.Contains("."))
				return fullName;
			return fullName.Substring(fullName.LastIndexOf(".") + 1);
		}

		#endregion

		#region Internal

		private List<RuntimeMethod> CreateMethodTable(RuntimeType type)
		{
			List<RuntimeMethod> methodTable;

			if (typeMethodTables.TryGetValue(type, out methodTable))
			{
				return methodTable;
			}

			methodTable = GetMethodTableFromBaseType(type);

			foreach (var method in type.Methods)
			{
				if ((method.Attributes & MethodAttributes.Virtual) == MethodAttributes.Virtual)
				{
					if ((method.Attributes & MethodAttributes.NewSlot) != MethodAttributes.NewSlot)
					{
						int slot = FindOverrideSlot(methodTable, method);
						methodTable[slot] = method;
						methodTableOffsets.Add(method, slot);
					}
					else
					{
						int slot = methodTable.Count;
						methodTable.Add(method);
						methodTableOffsets.Add(method, slot);
					}
				}
				else
				{
					if (((method.Attributes & MethodAttributes.Static) != MethodAttributes.Static)
						&& ((method.Attributes & MethodAttributes.RTSpecialName) != MethodAttributes.RTSpecialName))
					{
						int slot = methodTable.Count;
						methodTable.Add(method);
						methodTableOffsets.Add(method, slot);
					}
				}
			}

			typeMethodTables.Add(type, methodTable);

			return methodTable;
		}

		private List<RuntimeMethod> GetMethodTableFromBaseType(RuntimeType type)
		{
			var methodTable = new List<RuntimeMethod>();

			if (type.BaseType != null)
			{
				List<RuntimeMethod> baseMethodTable;

				if (!typeMethodTables.TryGetValue(type, out baseMethodTable))
				{
					// Method table for the base type has not been create yet, so create it now
					baseMethodTable = CreateMethodTable(type.BaseType);
				}

				methodTable = new List<RuntimeMethod>(baseMethodTable);
			}

			return methodTable;
		}

		private int FindOverrideSlot(IList<RuntimeMethod> methodTable, RuntimeMethod method)
		{
			foreach (var baseMethod in methodTable)
			{
				if (baseMethod.Name.Equals(method.Name) && baseMethod.Signature.Matches(method.Signature))
				{
					return methodTableOffsets[baseMethod];
				}
			}

			throw new InvalidOperationException(@"Failed to find override method slot.");
		}

		/// <summary>
		/// Gets the type memory requirements.
		/// </summary>
		/// <param name="signatureType">The signature type.</param>
		/// <returns></returns>
		private int GetMemorySize(SigType signatureType)
		{
			if (signatureType == null)
				throw new ArgumentNullException("signatureType");

			switch (signatureType.Type)
			{
				case CilElementType.U1: return 4;
				case CilElementType.U2: return 4;
				case CilElementType.U4: return 4;
				case CilElementType.U8: return 8;
				case CilElementType.I1: return 4;
				case CilElementType.I2: return 4;
				case CilElementType.I4: return 4;
				case CilElementType.I8: return 8;
				case CilElementType.R4: return 4;
				case CilElementType.R8: return 8;
				case CilElementType.Boolean: return 4;
				case CilElementType.Char: return 4;

				// Platform specific
				case CilElementType.Ptr: return nativePointerSize;
				case CilElementType.I: return nativePointerSize;
				case CilElementType.U: return nativePointerSize;

				default: return 4;
			}
		}

		/// <summary>
		/// Gets the size of the type alignment.
		/// </summary>
		/// <param name="signatureType">The signature type.</param>
		/// <returns></returns>
		private int GetAlignmentSize(SigType signatureType)
		{
			if (signatureType == null)
				throw new ArgumentNullException("signatureType");

			switch (signatureType.Type)
			{
				case CilElementType.U1: return 4;
				case CilElementType.U2: return 4;
				case CilElementType.U4: return 4;
				case CilElementType.U8: return 4;
				case CilElementType.I1: return 4;
				case CilElementType.I2: return 4;
				case CilElementType.I4: return 4;
				case CilElementType.I8: return 4;
				case CilElementType.R4: return 4;
				case CilElementType.R8: return 8;
				case CilElementType.Boolean: return 4;
				case CilElementType.Char: return 4;

				// Platform specific
				case CilElementType.Ptr: return nativePointerAlignment;
				case CilElementType.I: return nativePointerAlignment;
				case CilElementType.U: return nativePointerAlignment;

				default: return 4;
			}
		}

		#endregion

	}
}
