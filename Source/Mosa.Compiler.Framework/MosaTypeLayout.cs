// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Performs memory layout of a type for compilation.
	/// </summary>
	public class MosaTypeLayout
	{
		#region Data Members

		/// <summary>
		/// Holds a set of types
		/// </summary>
		private readonly HashSet<MosaType> resolvedTypes = new HashSet<MosaType>();

		/// <summary>
		/// Holds a list of interfaces
		/// </summary>
		private readonly List<MosaType> interfaces = new List<MosaType>();

		/// <summary>
		/// Holds the method table slot index
		/// </summary>
		private readonly Dictionary<MosaMethod, int> methodSlots = new Dictionary<MosaMethod, int>(new MosaMethodFullNameComparer());

		/// <summary>
		/// Holds the slot value for each interface
		/// </summary>
		private readonly Dictionary<MosaType, int> interfaceSlots = new Dictionary<MosaType, int>();

		/// <summary>
		/// Holds the size of each type
		/// </summary>
		private readonly Dictionary<MosaType, int> typeSizes = new Dictionary<MosaType, int>();

		/// <summary>
		/// Holds the size of each field
		/// </summary>
		private readonly Dictionary<MosaField, int> fieldSizes = new Dictionary<MosaField, int>();

		/// <summary>
		/// Holds the offset for each field
		/// </summary>
		private readonly Dictionary<MosaField, int> fieldOffsets = new Dictionary<MosaField, int>();

		/// <summary>
		/// Holds a list of methods for each type
		/// </summary>
		private readonly Dictionary<MosaType, List<MosaMethod>> typeMethodTables = new Dictionary<MosaType, List<MosaMethod>>();

		/// <summary>
		/// The method stack sizes
		/// </summary>
		private readonly Dictionary<MosaMethod, int> methodStackSizes = new Dictionary<MosaMethod, int>(new MosaMethodFullNameComparer());

		/// <summary>
		/// The parameter offsets
		/// </summary>
		private readonly Dictionary<MosaMethod, List<int>> parameterOffsets = new Dictionary<MosaMethod, List<int>>(new MosaMethodFullNameComparer());

		/// <summary>
		/// The parameter stack size
		/// </summary>
		private readonly Dictionary<MosaMethod, int> parameterStackSize = new Dictionary<MosaMethod, int>(new MosaMethodFullNameComparer());

		/// <summary>
		/// The overridden methods
		/// </summary>
		private readonly HashSet<MosaMethod> overriddenMethods = new HashSet<MosaMethod>(new MosaMethodFullNameComparer());

		/// <summary>
		/// The derived children of the base type
		/// </summary>
		private readonly Dictionary<MosaType, List<MosaType>> derivedTypes = new Dictionary<MosaType, List<MosaType>>();

		private readonly object _lock = new object();

		#endregion Data Members

		#region Properties

		/// <summary>
		/// Gets the type system associated with this instance.
		/// </summary>
		/// <value>The type system.</value>
		public TypeSystem TypeSystem { get; }

		/// <summary>
		/// Gets the size of the native pointer.
		/// </summary>
		/// <value>The size of the native pointer.</value>
		public int NativePointerSize { get; }

		/// <summary>
		/// Gets the native pointer alignment.
		/// </summary>
		/// <value>The native pointer alignment.</value>
		public int NativePointerAlignment { get; }

		/// <summary>
		/// Get a list of interfaces
		/// </summary>
		/// <value>
		/// The interfaces.
		/// </value>
		public IList<MosaType> Interfaces
		{
			get
			{
				lock (_lock)
				{
					return interfaces.ToArray();
				}
			}
		}

		#endregion Properties

		/// <summary>
		/// Initializes a new instance of the <see cref="MosaTypeLayout" /> class.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="nativePointerSize">Size of the native pointer.</param>
		/// <param name="nativePointerAlignment">The native pointer alignment.</param>
		public MosaTypeLayout(TypeSystem typeSystem, int nativePointerSize, int nativePointerAlignment)
		{
			Debug.Assert(nativePointerSize == 4 || nativePointerSize == 8);

			NativePointerAlignment = nativePointerAlignment;
			NativePointerSize = nativePointerSize;
			TypeSystem = typeSystem;

			ResolveLayouts();
		}

		#region Public Methods

		/// <summary>
		/// Gets the method slot number
		/// </summary>
		/// <param name="method">The method.</param>
		/// <returns></returns>
		public int GetMethodSlot(MosaMethod method)
		{
			lock (_lock)
			{
				ResolveType(method.DeclaringType);
				return methodSlots[method];
			}
		}

		public MosaMethod GetMethodBySlot(MosaType type, int slot)
		{
			lock (_lock)
			{
				ResolveType(type);

				var methodTable = GetMethodTable(type);

				return methodTable[slot];
			}
		}

		/// <summary>
		/// Gets the interface slot number
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public int GetInterfaceSlot(MosaType type)
		{
			lock (_lock)
			{
				ResolveType(type);
				return interfaceSlots[type];
			}
		}

		/// <summary>
		/// Gets the size of the type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public int GetTypeSize(MosaType type)
		{
			lock (_lock)
			{
				ResolveType(type);

				typeSizes.TryGetValue(type, out int size);

				return size;
			}
		}

		/// <summary>
		/// Gets the size of the field.
		/// </summary>
		/// <param name="field">The field.</param>
		/// <returns></returns>
		public int GetFieldSize(MosaField field)
		{
			lock (_lock)
			{
				return ComputeFieldSize(field);
			}
		}

		/// <summary>
		/// Gets the size of the field.
		/// </summary>
		/// <param name="field">The field.</param>
		/// <returns></returns>
		public int GetFieldOffset(MosaField field)
		{
			lock (_lock)
			{
				ResolveType(field.DeclaringType);

				fieldOffsets.TryGetValue(field, out int offset);

				return offset;
			}
		}

		/// <summary>
		/// Gets the type methods.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public List<MosaMethod> GetMethodTable(MosaType type)
		{
			if (type.IsModule)
				return null;

			if (type.BaseType == null && !type.IsInterface && type.FullName != "System.Object")   // ghost types like generic params, function ptr, etc.
				return null;

			if (type.Modifier != null)   // For types having modifiers, use the method table of element type instead.
				return GetMethodTable(type.ElementType);

			lock (_lock)
			{
				ResolveType(type);
				return typeMethodTables[type];
			}
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

			lock (_lock)
			{
				ResolveType(type);

				// TODO: Cache this

				var methodTable = new MosaMethod[interfaceType.Methods.Count];

				// Implicit Interface Methods
				for (int slot = 0; slot < interfaceType.Methods.Count; slot++)
				{
					methodTable[slot] = FindInterfaceMethod(type, interfaceType.Methods[slot]);
				}

				// Explicit Interface Methods
				ScanExplicitInterfaceImplementations(type, interfaceType, methodTable);

				return methodTable;
			}
		}

		/// <summary>
		/// Determines whether [is compound type] [the specified type].
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		///   <c>true</c> if [is compound type] [the specified type]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsCompoundType(MosaType type)
		{
			// i.e. whether copying of the type requires multiple move
			int? primitiveSize = type.GetPrimitiveSize(NativePointerSize);

			if (primitiveSize != null && primitiveSize > 8)
				return true;

			if (!type.IsUserValueType)
				return false;

			int typeSize = GetTypeSize(type);
			return typeSize > NativePointerSize;
		}

		/// <summary>
		/// Determines whether [is method overridden] [the specified method].
		/// </summary>
		/// <param name="method">The method.</param>
		/// <returns>
		///   <c>true</c> if [is method overridden] [the specified method]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsMethodOverridden(MosaMethod method)
		{
			var originalMethod = method;

			lock (_lock)
			{
				if (overriddenMethods.Contains(method))
					return true;

				int slot = methodSlots[method];
				var type = method.DeclaringType.BaseType;

				while (type != null)
				{
					var table = typeMethodTables[type];

					if (slot >= table.Count)
						return false;

					method = table[slot];

					if (overriddenMethods.Contains(method))
					{
						// cache this for next time
						overriddenMethods.Add(originalMethod);
						return true;
					}

					type = type.BaseType;
				}

				return false;
			}
		}

		public MosaType[] GetDerivedTypes(MosaType baseType)
		{
			lock (_lock)
			{
				if (derivedTypes.TryGetValue(baseType, out List<MosaType> derivedList))
				{
					return derivedList.ToArray();
				}
			}

			return null;
		}

		public static bool CanFitInRegister(MosaType type)
		{
			var basetype = GetTypeForRegister(type);

			var fits = FitsInRegister(basetype);

			return fits;
		}

		public static MosaType GetTypeForRegister(MosaType type)
		{
			if (type.IsValueType)
			{
				if (type.Fields != null)
				{
					var nonStaticFields = type.Fields.Where(x => !x.IsStatic).ToList();

					if (nonStaticFields.Count != 1)
						return null;

					var basetype = nonStaticFields[0].FieldType;

					if (!basetype.IsUserValueType)
						return basetype;

					var result = GetTypeForRegister(basetype);

					return result;
				}
			}

			return type;
		}

		#endregion Public Methods

		#region Internal - Layout

		/// <summary>
		/// Resolves the layouts.
		/// </summary>
		private void ResolveLayouts()
		{
			// Enumerate all types and do an appropriate type layout
			foreach (var type in TypeSystem.AllTypes)
			{
				ResolveType(type);
			}
		}

		/// <summary>
		/// Resolves the type.
		/// </summary>
		/// <param name="type">The type.</param>
		private void ResolveType(MosaType type)
		{
			if (type.IsModule)
				return;

			if (type.BaseType == null && !type.IsInterface && type.FullName != "System.Object")   // ghost types like generic params, function ptr, etc.
				return;

			if (type.Modifier != null)   // For types having modifiers, resolve the element type instead.
			{
				ResolveType(type.ElementType);
				return;
			}

			if (resolvedTypes.Contains(type))
				return;

			resolvedTypes.Add(type);

			if (type.BaseType != null)
			{
				ResolveType(type.BaseType);

				Addchildren(type.BaseType, type);
			}

			if (type.IsInterface)
			{
				ResolveInterfaceType(type);
				CreateMethodTable(type);
				return;
			}

			foreach (var interfaceType in type.Interfaces)
			{
				ResolveInterfaceType(interfaceType);
			}

			if (type.GetPrimitiveSize(NativePointerSize) != null)
			{
				typeSizes[type] = type.GetPrimitiveSize(NativePointerSize).Value;
			}
			else if (type.IsExplicitLayout)
			{
				ComputeExplicitLayout(type);
			}
			else
			{
				ComputeSequentialLayout(type);
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

			if (type.HasOpenGenericParams)
				return;

			if (interfaces.Contains(type))
				return;

			interfaces.Add(type);
			interfaceSlots.Add(type, interfaceSlots.Count);
		}

		private void ComputeSequentialLayout(MosaType type)
		{
			Debug.Assert(type != null, "No type given.");

			if (typeSizes.ContainsKey(type))
				return;

			// Instance size
			int typeSize = 0;

			// Receives the size/alignment
			int packingSize = type.PackingSize ?? NativePointerAlignment;

			if (type.BaseType != null)
			{
				if (!type.IsValueType)
				{
					typeSize = GetTypeSize(type.BaseType);
				}
			}

			foreach (var field in type.Fields)
			{
				if (!field.IsStatic)
				{
					// Set the field address
					fieldOffsets.Add(field, typeSize);

					int fieldSize = GetFieldSize(field);
					typeSize += fieldSize;

					// Pad the field in the type
					if (packingSize != 0)
					{
						int padding = (packingSize - (typeSize % packingSize)) % packingSize;
						typeSize += padding;
					}
				}
			}

			typeSizes.Add(type, (type.ClassSize == null || type.ClassSize == -1) ? typeSize : (int)type.ClassSize);
		}

		/// <summary>
		/// Applies the explicit layout to the given type.
		/// </summary>
		/// <param name="type">The type.</param>
		private void ComputeExplicitLayout(MosaType type)
		{
			Debug.Assert(type != null, "No type given.");

			//Debug.Assert(type.BaseType.LayoutSize != 0, @"Type size not set for explicit layout.");

			int size = 0;
			foreach (var field in type.Fields)
			{
				if (field.Offset == null)
					continue;

				int offset = (int)field.Offset.Value;
				fieldOffsets.Add(field, offset);
				size = Math.Max(size, offset + ComputeFieldSize(field));

				// Explicit layout assigns a physical offset from the start of the structure
				// to the field. We just assign this offset.
				Debug.Assert(fieldSizes[field] != 0, "Non-static field doesn't have layout!");
			}

			typeSizes.Add(type, (type.ClassSize == null || type.ClassSize == -1) ? size : (int)type.ClassSize);
		}

		private int ComputeFieldSize(MosaField field)
		{
			if (fieldSizes.TryGetValue(field, out int size))
			{
				return size;
			}
			else
			{
				ResolveType(field.DeclaringType);
			}

			// If the field is another struct, we have to dig down and compute its size too.
			if (field.FieldType.IsValueType)
			{
				size = GetTypeSize(field.FieldType);
			}
			else
			{
				size = NativePointerSize;
			}

			fieldSizes.Add(field, size);

			return size;
		}

		#endregion Internal - Layout

		#region Internal - Interface

		private void ScanExplicitInterfaceImplementations(MosaType type, MosaType interfaceType, MosaMethod[] methodTable)
		{
			foreach (var method in type.Methods)
			{
				foreach (var overrideTarget in method.Overrides)
				{
					// Get clean name for overrideTarget
					var cleanOverrideTargetName = GetNonExplicitMethodName(overrideTarget);

					int slot = 0;
					foreach (var interfaceMethod in interfaceType.Methods)
					{
						// Get clean name for interfaceMethod
						var cleanInterfaceMethodName = GetNonExplicitMethodName(interfaceMethod);

						// Check that the signatures match, the types and the names
						if (overrideTarget.Equals(interfaceMethod) && overrideTarget.DeclaringType.Equals(interfaceType) && cleanOverrideTargetName.Equals(cleanInterfaceMethodName))
						{
							methodTable[slot] = method;
						}
						slot++;
					}
				}
			}
		}

		private MosaMethod FindInterfaceMethod(MosaType type, MosaMethod interfaceMethod)
		{
			MosaMethod methodFound = null;

			if (type.BaseType != null)
			{
				methodFound = FindImplicitInterfaceMethod(type.BaseType, interfaceMethod);
			}

			var cleanInterfaceMethodName = GetNonExplicitMethodName(interfaceMethod);

			foreach (var method in type.Methods)
			{
				if (method.HasOpenGenericParams)
					continue;

				if (IsExplicitInterfaceMethod(method) && methodFound != null)
					continue;

				string cleanMethodName = GetNonExplicitMethodName(method);

				if (cleanInterfaceMethodName.Equals(cleanMethodName))
				{
					if (interfaceMethod.Equals(method))
					{
						return method;
					}
				}
			}

			if (type.BaseType != null)
			{
				methodFound = FindInterfaceMethod(type.BaseType, interfaceMethod);
			}

			if (methodFound != null)
				return methodFound;

			throw new InvalidOperationException($"Failed to find implicit interface implementation for type {type} and interface method {interfaceMethod}");
		}

		private MosaMethod FindImplicitInterfaceMethod(MosaType type, MosaMethod interfaceMethod)
		{
			MosaMethod methodFound = null;

			var cleanInterfaceMethodName = GetNonExplicitMethodName(interfaceMethod);

			foreach (var method in type.Methods)
			{
				if (method.HasOpenGenericParams)
					continue;

				if (IsExplicitInterfaceMethod(method))
					continue;

				string cleanMethodName = GetNonExplicitMethodName(method);

				if (cleanInterfaceMethodName.Equals(cleanMethodName))
				{
					if (interfaceMethod.Equals(method))
					{
						return method;
					}
				}
			}

			if (type.BaseType != null)
			{
				methodFound = FindImplicitInterfaceMethod(type.BaseType, interfaceMethod);
			}

			return methodFound;
		}

		private string GetNonExplicitMethodName(MosaMethod method)
		{
			if (!IsExplicitInterfaceMethod(method))
				return method.Name;

			int pos = method.Name.LastIndexOf(".");

			var name = method.Name.Substring(pos + 1);

			return name;
		}

		private bool IsExplicitInterfaceMethod(MosaMethod method)
		{
			if (method.IsConstructor || method.IsTypeConstructor)
				return false;

			return method.Name.Contains(".");
		}

		#endregion Internal - Interface

		#region Internal

		private List<MosaMethod> CreateMethodTable(MosaType type)
		{
			if (typeMethodTables.TryGetValue(type, out List<MosaMethod> methodTable))
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
						methodSlots.Add(method, slot);
					}
					else
					{
						int slot = FindOverrideSlot(methodTable, method);
						if (slot != -1)
						{
							methodTable[slot] = method;
							methodSlots.Add(method, slot);
							SetMethodOverridden(method, slot);
						}
						else
						{
							slot = methodTable.Count;
							methodTable.Add(method);
							methodSlots.Add(method, slot);
						}
					}
				}
				else
				{
					if (method.IsStatic && method.IsRTSpecialName)
					{
						int slot = methodTable.Count;
						methodTable.Add(method);
						methodSlots.Add(method, slot);
					}
					else if (!method.IsInternal && !method.IsExternal)
					{
						// HACK
						if (methodSlots.ContainsKey(method))
							continue;

						int slot = methodTable.Count;
						methodTable.Add(method);

						methodSlots.Add(method, slot);
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
				if (!typeMethodTables.TryGetValue(type.BaseType, out List<MosaMethod> baseMethodTable))
				{
					// Method table for the base type has not been create yet, so create it now
					baseMethodTable = CreateMethodTable(type.BaseType);
				}

				methodTable = new List<MosaMethod>(baseMethodTable);
			}

			return methodTable;
		}

		private int FindOverrideSlot(List<MosaMethod> methodTable, MosaMethod method)
		{
			int slot = -1;

			foreach (var baseMethod in methodTable)
			{
				if (baseMethod.Name.Equals(method.Name) && baseMethod.Equals(method))
				{
					if (baseMethod.GenericArguments.Count == 0)
						return methodSlots[baseMethod];
					else
						slot = methodSlots[baseMethod];
				}
			}

			if (slot >= 0) // non generic methods are more exact
				return slot;

			//throw new InvalidOperationException(@"Failed to find override method slot.");
			return -1;
		}

		private void SetMethodOverridden(MosaMethod method, int slot)
		{
			// this method is overridden (obviously)
			overriddenMethods.Add(method);

			// Note: this method does not update other parts of the inheritance chain
			// however, when trying to determine if a method was overridden a searched
			// up to the root method will be performed at that time

			// go up the inheritance chain
			var type = method.DeclaringType.BaseType;
			while (type != null)
			{
				var table = typeMethodTables[type];

				if (slot >= table.Count)
					return;

				method = table[slot];

				if (overriddenMethods.Contains(method))
					return;

				overriddenMethods.Add(method);

				type = type.BaseType;
			}
		}

		private void Addchildren(MosaType baseType, MosaType child)
		{
			if (!derivedTypes.TryGetValue(baseType, out List<MosaType> children))
			{
				children = new List<MosaType>();
				derivedTypes.Add(baseType, children);
			}

			children.Add(child);
		}

		private static bool FitsInRegister(MosaType type)
		{
			if (type == null)
				return false;

			var typeCode = type.TypeCode;

			if (typeCode == MosaTypeCode.ValueType)
			{
				return false; // no search
			}

			switch (typeCode)
			{
				case MosaTypeCode.Void: return true;
				case MosaTypeCode.Var: return false;
				case MosaTypeCode.MVar: return true;
				case MosaTypeCode.Boolean: return true;
				case MosaTypeCode.Char: return true;
				case MosaTypeCode.I1: return true;
				case MosaTypeCode.U1: return true;
				case MosaTypeCode.I2: return true;
				case MosaTypeCode.U2: return true;
				case MosaTypeCode.I4: return true;
				case MosaTypeCode.U4: return true;
				case MosaTypeCode.I8: return true;
				case MosaTypeCode.U8: return true;
				case MosaTypeCode.R4: return true;
				case MosaTypeCode.R8: return true;
				case MosaTypeCode.String: return true;
				case MosaTypeCode.UnmanagedPointer: return true;
				case MosaTypeCode.ManagedPointer: return true;
				case MosaTypeCode.ReferenceType: return true;
				case MosaTypeCode.Array: return true;
				case MosaTypeCode.TypedRef: return true;
				case MosaTypeCode.I: return true;
				case MosaTypeCode.U: return true;
				case MosaTypeCode.FunctionPointer: return true;
				case MosaTypeCode.Object: return true;
				case MosaTypeCode.SZArray: return true;
				default: return false;
			}
		}

		#endregion Internal
	}
}
