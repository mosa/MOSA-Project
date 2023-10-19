// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework;

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
	private readonly Dictionary<MosaMethod, uint> methodSlots = new Dictionary<MosaMethod, uint>();

	/// <summary>
	/// Holds the slot value for each interface
	/// </summary>
	private readonly Dictionary<MosaType, uint> interfaceSlots = new Dictionary<MosaType, uint>();

	/// <summary>
	/// Holds the size of each type
	/// </summary>
	private readonly Dictionary<MosaType, uint> typeLayoutSize = new Dictionary<MosaType, uint>();

	/// <summary>
	/// Holds the size of each field
	/// </summary>
	private readonly Dictionary<MosaField, uint> fieldSizes = new Dictionary<MosaField, uint>();

	/// <summary>
	/// Holds the offset for each field
	/// </summary>
	private readonly Dictionary<MosaField, uint> fieldOffsets = new Dictionary<MosaField, uint>();

	/// <summary>
	/// Holds a list of methods for each type
	/// </summary>
	private readonly Dictionary<MosaType, List<MosaMethod>> typeMethodTables = new Dictionary<MosaType, List<MosaMethod>>();

	/// <summary>
	/// The parameter stack size
	/// </summary>
	private readonly Dictionary<MosaMethod, MethodInfo> methodData = new Dictionary<MosaMethod, MethodInfo>();

	/// <summary>
	/// The overridden methods
	/// </summary>
	private readonly HashSet<MosaMethod> overriddenMethods = new HashSet<MosaMethod>();

	/// <summary>
	/// The derived children of the base type
	/// </summary>
	private readonly Dictionary<MosaType, List<MosaType>> derivedTypes = new Dictionary<MosaType, List<MosaType>>();

	private readonly object _lock = new object();

	#endregion Data Members

	#region Nested Class

	public class MethodInfo
	{
		public bool HasThis { get; set; }

		public int ReturnSize { get; set; }

		public bool ReturnInRegister { get; set; }

		public int ParameterStackSize { get; set; }

		public List<uint> ParameterOffsets { get; set; }

		public List<uint> ParameterSizes { get; set; }

		public int ParameterCount => ParameterSizes.Count;
	}

	#endregion Nested Class

	#region Properties

	/// <summary>
	/// Gets the type system associated with this instance.
	/// </summary>
	/// <value>The type system.</value>
	public TypeSystem TypeSystem { get; }

	public bool Is32BitPlatform { get; }

	/// <summary>
	/// Gets the size of the native pointer.
	/// </summary>
	/// <value>The size of the native pointer.</value>
	public uint NativePointerSize { get; }

	/// <summary>
	/// Gets the native pointer alignment.
	/// </summary>
	/// <value>The native pointer alignment.</value>
	public uint NativePointerAlignment { get; }

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
	public MosaTypeLayout(TypeSystem typeSystem, bool is32BitPlatform, uint nativePointerSize, uint nativePointerAlignment)
	{
		Debug.Assert(nativePointerSize is 4 or 8);

		TypeSystem = typeSystem;
		Is32BitPlatform = is32BitPlatform;

		NativePointerAlignment = nativePointerAlignment;
		NativePointerSize = nativePointerSize;

		ResolveLayouts();
	}

	#region Public Methods

	/// <summary>
	/// Gets the method slot number
	/// </summary>
	/// <param name="method">The method.</param>
	/// <returns></returns>
	public uint GetMethodSlot(MosaMethod method)
	{
		lock (_lock)
		{
			ResolveType(method.DeclaringType);
			return methodSlots[method];
		}
	}

	public MosaMethod GetMethodBySlot(MosaType type, uint slot)
	{
		lock (_lock)
		{
			ResolveType(type);

			var methodTable = GetMethodTable(type);

			return methodTable[(int)slot];
		}
	}

	/// <summary>
	/// Gets the interface slot number
	/// </summary>
	/// <param name="type">The type.</param>
	/// <returns></returns>
	public uint GetInterfaceSlot(MosaType type)
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
	public uint GetTypeLayoutSize(MosaType type)
	{
		lock (_lock)
		{
			ResolveType(type);

			Debug.Assert(typeLayoutSize.ContainsKey(type));

			return typeLayoutSize[type];
		}
	}

	/// <summary>
	/// Gets the size of the field.
	/// </summary>
	/// <param name="field">The field.</param>
	/// <returns></returns>
	public uint GetFieldSize(MosaField field)
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
	public uint GetFieldOffset(MosaField field)
	{
		lock (_lock)
		{
			ResolveType(field.DeclaringType);

			Debug.Assert(fieldOffsets.ContainsKey(field));

			return fieldOffsets[field];
		}
	}

	/// <summary>
	/// Gets the type methods.
	/// </summary>
	/// <param name="type">The type.</param>
	/// <returns></returns>
	public List<MosaMethod> GetMethodTable(MosaType type)
	{
		if (type.IsModule || type.HasOpenGenericParams)
			return null;

		if (type.Modifier != null)   // For types having modifiers, use the method table of element type instead.
			return GetMethodTable(type.ElementType);

		lock (_lock)
		{
			ResolveType(type);

			typeMethodTables.TryGetValue(type, out var table);

			return table;
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
			for (var slot = 0; slot < interfaceType.Methods.Count; slot++)
			{
				methodTable[slot] = FindInterfaceMethod(type, interfaceType.Methods[slot]);
			}

			// Explicit Interface Methods
			ScanExplicitInterfaceImplementations(type, interfaceType, methodTable);

			return methodTable;
		}
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

			var slot = methodSlots[method];
			var type = method.DeclaringType.BaseType;

			while (type != null)
			{
				var table = typeMethodTables[type];

				if (slot >= table.Count)
					return false;

				method = table[(int)slot];

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

	public MethodInfo GetMethodInfo(MosaMethod method)
	{
		lock (_lock)
		{
			if (methodData.TryGetValue(method, out MethodInfo value))
			{
				return value;
			}

			return ResolveMethodParameters(method);
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

	public static bool IsUnderlyingPrimitive(MosaType type)
	{
		var basetype = GetUnderlyingType(type);

		var fits = IsPrimitive(basetype);

		return fits;
	}

	public static bool IsPrimitive(MosaType underlyingType)
	{
		if (underlyingType == null)
			return false;

		if (underlyingType.TypeCode is MosaTypeCode.ValueType or MosaTypeCode.Var)
			return false; // no search

		return true;
	}

	public static List<MosaField> GetImmediateNonStaticFields(MosaType type)
	{
		return type.Fields.Where(x => !x.IsStatic).ToList();
	}

	public static MosaType GetUnderlyingType(MosaType type)
	{
		if (type.IsValueType && type.Fields != null)
		{
			var nonStaticFields = GetImmediateNonStaticFields(type);

			if (nonStaticFields.Count != 1)
				return type;

			var basetype = nonStaticFields[0].FieldType;

			if (!basetype.IsUserValueType)
				return basetype;

			var result = GetUnderlyingType(basetype);

			return result;
		}

		return type;
	}

	#endregion Public Methods

	#region Internal - Layout

	private static bool HasNonStaticField(MosaType type)
	{
		foreach (var field in type.Fields)
		{
			if (!field.IsStatic)
				return true;
		}

		if (type.BaseType == null)
			return false;

		return HasNonStaticField(type.BaseType);
	}

	private static List<MosaField> GetAllNonStaticFields(MosaType type)
	{
		var fields = new List<MosaField>();
		var at = type;

		while (at != null)
		{
			foreach (var field in at.Fields)
			{
				if (field.IsStatic)
					continue;

				fields.Add(field);
			}

			at = at.BaseType;
		}

		return fields;
	}

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
		if (type.IsModule || type.HasOpenGenericParams)
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

			AddChildren(type.BaseType, type);
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

		CreateMethodTable(type);

		if (type.IsVoid)
			return;

		var layoutSize = type.IsExplicitLayout
			? ComputeExplicitLayout(type)
			: ComputeSequentialLayout(type);

		typeLayoutSize.Add(type, layoutSize);
	}

	private uint ComputeSequentialLayout(MosaType type)
	{
		var typeSize = 0u;

		if (type.BaseType != null && !type.IsValueType)
		{
			typeSize = GetTypeLayoutSize(type.BaseType);
		}

		var nonStaticFields = GetImmediateNonStaticFields(type);
		var packingSize = (uint?)type.PackingSize ?? NativePointerAlignment;

		foreach (var field in nonStaticFields)
		{
			// Set the field address
			fieldOffsets.Add(field, typeSize);

			var fieldSize = ComputeFieldSize(field);
			typeSize += fieldSize;

			// Pad the field in the type
			if (packingSize != 0)
			{
				var padding = (packingSize - typeSize % packingSize) % packingSize;
				typeSize += padding;
			}
		}

		return type.ClassSize is null or -1 ? typeSize : (uint)type.ClassSize;
	}

	private uint ComputeExplicitLayout(MosaType type)
	{
		var nonStaticFields = GetImmediateNonStaticFields(type);
		var size = 0u;

		foreach (var field in nonStaticFields)
		{
			var offset = field.Offset;

			if (offset == null)
				continue;

			fieldOffsets.Add(field, offset.Value);

			var fieldSize = ComputeFieldSize(field);

			size = Math.Max(size, fieldSize + offset.Value);
		}

		return type.ClassSize is null or -1 ? size : (uint)type.ClassSize;
	}

	private uint ComputeFieldSize(MosaField field)
	{
		if (fieldSizes.TryGetValue(field, out var size))
			return size;

		ResolveType(field.DeclaringType);
		ResolveType(field.FieldType);

		size = GetSize(field.FieldType);

		fieldSizes.Add(field, size);

		return size;
	}

	private uint GetSize(MosaType type)
	{
		var elementType = MethodCompiler.GetElementType(type, Is32BitPlatform);

		if (elementType == ElementType.ValueType)
		{
			return typeLayoutSize[type];
		}
		else
		{
			return MethodCompiler.GetSize(elementType, Is32BitPlatform);
		}
	}

	/// <summary>
	/// Resolves the method parameters.
	/// </summary>
	/// <param name="method">The method.</param>
	private MethodInfo ResolveMethodParameters(MosaMethod method)
	{
		if (method.HasOpenGenericParams)
			return null;

		if (methodData.ContainsKey(method))
			return null;

		var parameters = method.Signature.Parameters;
		var stacksize = 0u;

		var offsets = new List<uint>(parameters.Count + (method.HasThis ? 1 : 0));
		var sizes = new List<uint>(parameters.Count + (method.HasThis ? 1 : 0));

		if (method.HasThis)
		{
			offsets.Add(0);
			sizes.Add(NativePointerSize);

			stacksize = NativePointerSize;  // already aligned
		}

		foreach (var parameter in parameters)
		{
			var size = parameter.ParameterType.IsValueType ? GetTypeLayoutSize(parameter.ParameterType) : NativePointerAlignment;

			offsets.Add(stacksize);
			sizes.Add(size);

			stacksize += Alignment.AlignUp(size, NativePointerAlignment);
		}

		var returnType = method.Signature.ReturnType;
		var returnSize = 0u;

		if (!returnType.IsVoid)
		{
			ResolveType(returnType);

			typeLayoutSize.TryGetValue(returnType, out returnSize);
		}

		var methodInfo = new MethodInfo
		{
			ReturnSize = (int)returnSize,
			ParameterOffsets = offsets,
			ParameterSizes = sizes,
			ParameterStackSize = (int)stacksize,
			ReturnInRegister = !returnType.IsVoid && IsPrimitive(returnType),
			HasThis = method.HasThis
		};

		methodData.Add(method, methodInfo);

		return methodInfo;
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
		interfaceSlots.Add(type, (uint)interfaceSlots.Count);
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

				var slot = 0;
				foreach (var interfaceMethod in interfaceType.Methods)
				{
					// Get clean name for interfaceMethod
					var cleanInterfaceMethodName = GetNonExplicitMethodName(interfaceMethod);

					// Check that the signatures match, the types and the names
					if (overrideTarget.SameSignature(interfaceMethod) && overrideTarget.DeclaringType.Equals(interfaceType) && cleanOverrideTargetName.Equals(cleanInterfaceMethodName))
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

			var cleanMethodName = GetNonExplicitMethodName(method);

			if (cleanInterfaceMethodName.Equals(cleanMethodName))
			{
				if (interfaceMethod.SameSignature(method))
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

		throw new InvalidOperationCompilerException($"Failed to find implicit interface implementation for type {type} and interface method {interfaceMethod}");
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

			var cleanMethodName = GetNonExplicitMethodName(method);

			if (cleanInterfaceMethodName.Equals(cleanMethodName) && interfaceMethod.SameSignature(method))
			{
				return method;
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

		var pos = method.Name.LastIndexOf(".");

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
			var slot = methodTable.Count;

			if (method.IsVirtual && !method.IsNewSlot)
			{
				var newSlot = FindOverrideSlot(methodTable, method);
				if (newSlot != -1)
				{
					SetMethodOverridden(method, newSlot);
					slot = newSlot;
				}
			}
			else if (!method.IsInternal && !method.IsExternal)
			{
				// HACK
				if (methodSlots.ContainsKey(method))
					continue;
			}

			if (methodTable.Count > slot)
				methodTable[slot] = method;
			else
				methodTable.Add(method);

			methodSlots[method] = (uint)slot;
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
		var slot = -1;

		foreach (var baseMethod in methodTable)
		{
			if (method.SameNameAndSignature(baseMethod))
			{
				if (baseMethod.GenericArguments.Count == 0)
					return (int)methodSlots[baseMethod];
				else
					slot = (int)methodSlots[baseMethod];
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

	private void AddChildren(MosaType baseType, MosaType child)
	{
		if (!derivedTypes.TryGetValue(baseType, out List<MosaType> children))
		{
			children = new List<MosaType>();
			derivedTypes.Add(baseType, children);
		}

		children.Add(child);
	}

	#endregion Internal
}
