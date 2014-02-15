/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Performs memory layout of a type for compilation.
	/// </summary>
	public class MosaTypeLayout
	{
		#region Data members

		/// <summary>
		/// Holds the type system
		/// </summary>
		private Mosa.Compiler.MosaTypeSystem.TypeSystem typeSystem;

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
		private HashSet<MosaType> typeSet = new HashSet<MosaType>();

		/// <summary>
		/// Holds a list of interfaces
		/// </summary>
		private List<MosaType> interfaces = new List<MosaType>();

		/// <summary>
		/// Holds the method table offsets value for each method
		/// </summary>
		private Dictionary<MosaMethod, int> methodTableOffsets = new Dictionary<MosaMethod, int>();

		/// <summary>
		/// Holds the slot value for each interface
		/// </summary>
		private Dictionary<MosaType, int> interfaceSlots = new Dictionary<MosaType, int>();

		/// <summary>
		/// Holds the size of each type
		/// </summary>
		private Dictionary<MosaType, int> typeSizes = new Dictionary<MosaType, int>();

		/// <summary>
		/// Holds the size of each field
		/// </summary>
		private Dictionary<MosaField, int> fieldSizes = new Dictionary<MosaField, int>();

		/// <summary>
		/// Holds the offset for each field
		/// </summary>
		private Dictionary<MosaField, int> fieldOffets = new Dictionary<MosaField, int>();

		/// <summary>
		/// Holds a list of methods for each type
		/// </summary>
		private Dictionary<MosaType, List<MosaMethod>> typeMethodTables = new Dictionary<MosaType, List<MosaMethod>>();

		#endregion Data members

		/// <summary>
		/// Initializes a new instance of the <see cref="MosaTypeLayout" /> class.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="nativePointerSize">Size of the native pointer.</param>
		/// <param name="nativePointerAlignment">The native pointer alignment.</param>
		public MosaTypeLayout(TypeSystem typeSystem, int nativePointerSize, int nativePointerAlignment)
		{
			this.nativePointerAlignment = nativePointerAlignment;
			this.nativePointerSize = nativePointerSize;
			this.typeSystem = typeSystem;

			ResolveLayouts();
		}

		/// <summary>
		/// Gets the size of the native pointer.
		/// </summary>
		/// <value>The size of the native pointer.</value>
		public int NativePointerSize { get { return nativePointerSize; } }

		/// <summary>
		/// Gets the native pointer alignment.
		/// </summary>
		/// <value>The native pointer alignment.</value>
		public int NativePointerAlignment { get { return nativePointerAlignment; } }

		/// <summary>
		/// Gets the method table offset.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <returns></returns>
		public int GetMethodTableOffset(MosaMethod method)
		{
			ResolveType(method.DeclaringType);
			return methodTableOffsets[method];
		}

		/// <summary>
		/// Gets the interface slot offset.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public int GetInterfaceSlotOffset(MosaType type)
		{
			ResolveType(type);
			return interfaceSlots[type];
		}

		/// <summary>
		/// Gets the size of the type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public int GetTypeSize(MosaType type)
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
		public int GetFieldSize(MosaField field)
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
			if (field.DeclaringType.IsValueType)
			{
				size = GetTypeSize(field.DeclaringType);
			}
			else
			{
				size = GetMemorySize(field.Type);
			}

			fieldSizes.Add(field, size);

			return size;
		}

		/// <summary>
		/// Gets the size of the field.
		/// </summary>
		/// <param name="field">The field.</param>
		/// <returns></returns>
		public int GetFieldOffset(MosaField field)
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
		public IList<MosaMethod> GetMethodTable(MosaType type)
		{
			if (type.IsModule)
				return null;

			if (type.IsBaseGeneric || type.IsOpenGenericType)
				return null;

			if (!(type.IsObject || type.IsValueType || type.IsEnum || type.IsString || type.IsInterface || type.IsLinkerGenerated))
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
		public MosaMethod[] GetInterfaceTable(MosaType type, MosaType interfaceType)
		{
			if (type.Interfaces.Count == 0)
				return null;

			ResolveType(type);

			MosaMethod[] methodTable = new MosaMethod[interfaceType.Methods.Count];

			// Implicit Interface Methods
			for (int slot = 0; slot < interfaceType.Methods.Count; slot++)
			{
				methodTable[slot] = FindInterfaceMethod(type, interfaceType.Methods[slot]);
			}

			// Explicit Interface Methods
			ScanExplicitInterfaceImplementations(type, interfaceType, methodTable);

			return methodTable;
		}

		/// <summary>
		/// Get a list of interfaces
		/// </summary>
		public IList<MosaType> Interfaces { get { return interfaces; } }

		#region Internal - Layout

		private void ResolveLayouts()
		{
			// Enumerate all types and do an appropriate type layout
			foreach (MosaType type in typeSystem.AllTypes)
			{
				ResolveType(type);
			}
		}

		private void ResolveType(MosaType type)
		{
			if (type.IsModule)
				return;

			if (type.IsBaseGeneric || type.IsOpenGenericType)
				return;

			if (!(type.IsObject || type.IsValueType || type.IsEnum || type.IsString || type.IsInterface || type.IsLinkerGenerated))
				return;

			if (typeSet.Contains(type))
				return;

			typeSet.Add(type);

			if (type.BaseType != null)
			{
				ResolveType(type.BaseType);
			}

			if (type.IsInterface)
			{
				ResolveInterfaceType(type);
				CreateMethodTable(type);
				return;
			}

			foreach (MosaType interfaceType in type.Interfaces)
			{
				ResolveInterfaceType(interfaceType);
			}

			if (type.IsExplicitLayout)
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
		private void ResolveInterfaceType(MosaType type)
		{
			Debug.Assert(type.IsInterface);

			if (type.IsModule)
				return;

			if (type.IsBaseGeneric || type.IsOpenGenericType)
				return;

			if (!(type.IsObject || type.IsValueType || type.IsEnum || type.IsString || type.IsInterface || type.IsLinkerGenerated))
				return;

			if (interfaces.Contains(type))
				return;

			interfaces.Add(type);
			interfaceSlots.Add(type, interfaceSlots.Count);
		}

		private void CreateSequentialLayout(MosaType type)
		{
			Debug.Assert(type != null, @"No type given.");

			if (typeSizes.ContainsKey(type))
				return;

			// Instance size
			int typeSize = 0;

			// Receives the size/alignment
			int packingSize = type.PackingSize;

			if (type.BaseType != null)
			{
				if (!type.IsValueType)
				{
					typeSize = GetTypeSize(type.BaseType);
				}
			}

			foreach (MosaField field in type.Fields)
			{
				if (!field.IsStaticField)
				{
					int fieldSize = GetMemorySize(field.Type);

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
		private void ResolveExplicitLayout(MosaType type)
		{
			Debug.Assert(type != null, @"No type given.");
			//Debug.Assert(type.BaseType.LayoutSize != 0, @"Type size not set for explicit layout.");

			foreach (MosaField field in type.Fields)
			{
				// Explicit layout assigns a physical offset from the start of the structure
				// to the field. We just assign this offset.
				Debug.Assert(fieldSizes[field] != 0, @"Non-static field doesn't have layout!");
			}

			typeSizes.Add(type, type.Size);
		}

		#endregion Internal - Layout

		#region Internal - Interface

		private void ScanExplicitInterfaceImplementations(MosaType type, MosaType interfaceType, MosaMethod[] methodTable)
		{
			foreach (var pair in type.InheritanceOveride)
			{
				int slot = 0;
				foreach (var interfaceMethod in interfaceType.Methods)
				{
					if (pair.Key == interfaceMethod)
					{
						methodTable[slot] = pair.Value;
					}
					slot++;
				}
			}
		}

		private MosaMethod FindInterfaceMethod(MosaType type, MosaMethod interfaceMethod)
		{
			var cleanInterfaceMethodName = GetCleanMethodName(interfaceMethod.Name);

			foreach (var method in type.Methods)
			{
				string cleanMethodName = GetCleanMethodName(method.Name);

				if (cleanInterfaceMethodName.Equals(cleanMethodName))
				{
					if (interfaceMethod.Matches(method))
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

		#endregion Internal - Interface

		#region Internal

		private List<MosaMethod> CreateMethodTable(MosaType type)
		{
			List<MosaMethod> methodTable;

			if (typeMethodTables.TryGetValue(type, out methodTable))
			{
				return methodTable;
			}

			methodTable = GetMethodTableFromBaseType(type);

			foreach (var method in type.Methods)
			{
				if (method.IsVirtual)
				{
					if (method.IsNewSlot)
					{
						int slot = methodTable.Count;
						methodTable.Add(method);
						methodTableOffsets.Add(method, slot);
					}
					else
					{
						int slot = FindOverrideSlot(methodTable, method);
						methodTable[slot] = method;
						methodTableOffsets.Add(method, slot);
					}
				}
				else
				{
					if (method.IsStatic && method.IsRTSpecialName)
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

		private List<MosaMethod> GetMethodTableFromBaseType(MosaType type)
		{
			var methodTable = new List<MosaMethod>();

			if (type.BaseType != null)
			{
				List<MosaMethod> baseMethodTable;

				if (!typeMethodTables.TryGetValue(type, out baseMethodTable))
				{
					// Method table for the base type has not been create yet, so create it now
					baseMethodTable = CreateMethodTable(type.BaseType);
				}

				methodTable = new List<MosaMethod>(baseMethodTable);
			}

			return methodTable;
		}

		private int FindOverrideSlot(IList<MosaMethod> methodTable, MosaMethod method)
		{
			int slot = -1;

			foreach (var baseMethod in methodTable)
			{
				if (baseMethod.Name.Equals(method.Name) && baseMethod.Matches(method))
				{
					if (baseMethod.GenericArguments.Count == 0)
						return methodTableOffsets[baseMethod];
					else
						slot = methodTableOffsets[baseMethod];
				}
			}

			if (slot >= 0) // non generic methods are more exact
				return slot;

			throw new InvalidOperationException(@"Failed to find override method slot.");
		}

		/// <summary>
		/// Gets the type memory requirements.
		/// </summary>
		/// <param name="type">The signature type.</param>
		/// <returns></returns>
		private int GetMemorySize(MosaType type)
		{
			if (type.IsDouble)
				return 8;
			else if (type.IsLong)
				return 8;
			else if (type.IsPointer || type.IsUnmanagedPointerType || type.IsManagedPointerType)
				return nativePointerSize;
			else if (type.IsNativeInteger)
				return nativePointerSize;
			else
				return type.Size != 0 ? type.Size : 4;
		}

		/// <summary>
		/// Gets the size of the type alignment.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		private int GetAlignmentSize(MosaType type)
		{
			if (type.IsDouble)
				return 8;
			else if (type.IsLong)
				return 8;
			else if (type.IsPointer)
				return nativePointerAlignment;
			else if (type.IsPointer || type.IsUnmanagedPointerType || type.IsManagedPointerType)
				return nativePointerAlignment;
			else if (type.IsNativeInteger)
				return nativePointerAlignment;

			return 4;
		}

		#endregion Internal
	}
}