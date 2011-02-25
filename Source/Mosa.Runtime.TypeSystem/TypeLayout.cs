/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.TypeSystem
{
	/// <summary>
	/// Performs memory layout of a type for compilation.
	/// </summary>
	public sealed class TypeLayout : ITypeLayout
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private ITypeSystem typeSystem;

		/// <summary>
		/// 
		/// </summary>
		private int nativePointerAlignment;

		/// <summary>
		/// 
		/// </summary>
		private int nativePointerSize;

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
		private Dictionary<RuntimeType, IList<RuntimeMethod>> typeMethodTables = new Dictionary<RuntimeType, IList<RuntimeMethod>>();

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

			int size = 0;
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
			int size = 0;

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
				return ((ITypeLayout)this).GetTypeSize(field.DeclaringType);
			}

			size = GetMemorySize(field.SignatureType);

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

			int size = 0;

			fieldOffets.TryGetValue(field, out size);

			return size;
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

			if (type.IsModule || type.IsGeneric || type.IsDelegate)
				return null;

			//if (type.IsInterface)
			//    return null;

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
			ScanExplicitInterfaceImplementations(type, interfaceType.Methods, methodTable);

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

			if (type.IsModule || type.IsGeneric || type.IsDelegate)
				return;

			if (typeSizes.ContainsKey(type))
				return;

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

			if (type.IsModule || type.IsGeneric || type.IsDelegate)
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
				((ITypeLayout)(this)).GetTypeSize(type.BaseType);
				typeSize = typeSizes[type.BaseType];
			}

			foreach (RuntimeField field in type.Fields)
			{
				if (!field.IsStaticField)
				{
					int fieldSize = GetMemorySize(field.SignatureType);
					int fieldAlignment = GetAlignmentSize(field.SignatureType);

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

		private void ScanExplicitInterfaceImplementations(RuntimeType type, IList<RuntimeMethod> interfaceMethods, RuntimeMethod[] methodTable)
		{
			//TODO: rewrite so that access directly to metadata is not required
			IMetadataProvider metadata = type.Module.MetadataModule.Metadata;
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

		private RuntimeMethod FindInterfaceMethod(RuntimeType type, RuntimeMethod interfaceMethod)
		{
			string cleanInterfaceMethodName = GetCleanMethodName(interfaceMethod.Name);

			foreach (RuntimeMethod method in type.Methods)
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
				System.Console.WriteLine("Descending from " + type + " to " + type.BaseType);
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

		private IList<RuntimeMethod> CreateMethodTable(RuntimeType type)
		{
			IList<RuntimeMethod> methodTable;

			if (typeMethodTables.TryGetValue(type, out methodTable))
			{
				return methodTable;
			}

			methodTable = GetMethodTableFromBaseType(type);

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

			typeMethodTables.Add(type, methodTable);

			return methodTable;
		}

		private List<RuntimeMethod> GetMethodTableFromBaseType(RuntimeType type)
		{
			List<RuntimeMethod> methodTable = new List<RuntimeMethod>();

			if (type.BaseType != null)
			{
				IList<RuntimeMethod> baseMethodTable;

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
			foreach (RuntimeMethod baseMethod in methodTable)
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
