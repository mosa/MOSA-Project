// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Tool.Explorer.Common;

public abstract class TypeSystemTreeBase<TTree, TNode>
	where TNode : class
{
	public TTree TreeView { get; }

	public TypeSystem TypeSystem { get; }

	public MosaTypeLayout TypeLayout { get; }

	private readonly bool showSizes;

	private readonly Dictionary<object, TNode> map = new Dictionary<object, TNode>();
	private readonly HashSet<MosaUnit> included;

	public bool HasFilter => included != null && included.Count != 0;

	protected TypeSystemTreeBase(TTree treeView, TypeSystem typeSystem, MosaTypeLayout typeLayout, bool showSizesCheck = true, HashSet<MosaUnit> includedSet = null)
	{
		TreeView = treeView;
		TypeSystem = typeSystem;
		TypeLayout = typeLayout;
		showSizes = showSizesCheck;
		included = includedSet;

		BeginUpdate();
		ClearTree();
		CreateTreeBase();
		CreateInitialTree();
		EndUpdate();
	}

	public void Update()
	{
		BeginUpdate();

		foreach (var type in TypeSystem.AllTypes)
		{
			if (included != null && !included.Contains(type))
				continue;

			foreach (var method in type.Methods)
				if (included == null || included.Contains(method))
					GetOrCreateNode(method);
		}

		EndUpdate();
	}

	public void Update(MosaMethod method)
	{
		if (included == null || included.Contains(method))
			GetOrCreateNode(method);
	}

	protected abstract void BeginUpdate();

	protected abstract void EndUpdate();

	protected abstract void ClearTree();

	protected abstract TNode CreateNode(string header, object tag = null);

	protected abstract string GetHeader(TNode node);

	protected abstract void SetHeader(TNode node, string header);

	protected abstract void AddRootNode(TNode node);

	protected abstract void AddChildNode(TNode parent, TNode node);

	protected abstract IEnumerable<TNode> GetChildren(TNode node);

	private void CreateTreeBase()
	{
		foreach (var module in TypeSystem.Modules)
			if (included == null || included.Contains(module))
				GetOrCreateNode(module);
	}

	private void CreateInitialTree()
	{
		var typeList = TypeSystem.AllTypes.OrderBy(o => o.FullName).ToList();

		foreach (var type in typeList)
			if (included == null || included.Contains(type))
				GetOrCreateNode(type);
	}

	private void AddToTree(TNode node, TNode parent)
	{
		if (parent == null)
			AddRootNode(node);
		else
			AddChildNode(parent, node);
	}

	private TNode GetOrCreateNode(MosaModule module)
	{
		if (map.TryGetValue(module, out var moduleNode))
			return moduleNode;

		moduleNode = CreateNode(module.Name, module);

		map.Add(module, moduleNode);

		AddToTree(moduleNode, null);

		return moduleNode;
	}

	private TNode GetOrCreateNode(string name, MosaModule module)
	{
		name = string.IsNullOrWhiteSpace(name) ? "[No Namespace]" : name;

		var index = module + ":" + name;

		if (map.TryGetValue(index, out var namespaceNode))
			return namespaceNode;

		namespaceNode = CreateNode(name);

		map.Add(index, namespaceNode);

		var moduleNode = GetOrCreateNode(module);
		AddToTree(namespaceNode, moduleNode);

		return namespaceNode;
	}

	private TNode GetOrCreateNode(MosaType type)
	{
		if (map.TryGetValue(type, out var typeNode))
			return typeNode;

		typeNode = CreateNode(type.FullName, type);

		map.Add(type, typeNode);

		var namespaceNode = GetOrCreateNode(type.Namespace, type.Module);
		AddToTree(typeNode, namespaceNode);

		if (type.BaseType != null)
			AddChildNode(typeNode, CreateNode($"Base Type: {type.BaseType.FullName}"));

		if (type.DeclaringType != null)
			AddChildNode(typeNode, CreateNode($"Declaring Type: {type.DeclaringType.FullName}"));

		if (type.ElementType != null)
			AddChildNode(typeNode, CreateNode($"Element Type: {type.ElementType.FullName}"));

		if (type.Interfaces.Count != 0)
		{
			var interfacesNodes = CreateNode("Interfaces");
			AddChildNode(typeNode, interfacesNodes);

			foreach (var interfaceType in type.Interfaces)
				AddChildNode(interfacesNodes, CreateNode(interfaceType.FullName));
		}

		if (type.Fields.Count != 0)
		{
			var fieldsNode = CreateNode("Fields");

			if (showSizes && !type.HasOpenGenericParams)
				SetHeader(fieldsNode, GetHeader(fieldsNode) + " (Count: " + type.Fields.Count + " - Size: " + TypeLayout.GetTypeLayoutSize(type) + ")");

			AddChildNode(typeNode, fieldsNode);

			foreach (var field in type.Fields)
			{
				var fieldNode = CreateNode(field.ShortName);
				AddChildNode(fieldsNode, fieldNode);

				if (field.IsStatic)
					SetHeader(fieldNode, GetHeader(fieldNode) + " [Static]");

				if (!showSizes || field.HasOpenGenericParams)
					continue;

				SetHeader(fieldNode, GetHeader(fieldNode) + " (Size: " + TypeLayout.GetFieldSize(field));

				if (!field.IsStatic)
					SetHeader(fieldNode, GetHeader(fieldNode) + " - Offset: " + TypeLayout.GetFieldOffset(field));

				SetHeader(fieldNode, GetHeader(fieldNode) + ")");
			}
		}

		if (type.Properties.Count != 0)
		{
			var propertiesNode = CreateNode("Properties");

			if (showSizes)
				SetHeader(propertiesNode, GetHeader(propertiesNode) + " (Count: " + type.Properties.Count + ")");

			AddChildNode(typeNode, propertiesNode);

			foreach (var property in type.Properties)
			{
				if (included != null && !included.Contains(property))
					continue;

				var propertyNode = CreateNode(property.ShortName, property);
				AddChildNode(propertiesNode, propertyNode);

				if (property.GetterMethod != null)
				{
					var getterNode = CreateNode(property.GetterMethod.ShortName, property.GetterMethod);
					AddChildNode(propertyNode, getterNode);

					if (property.GetterMethod.IsStatic)
						SetHeader(getterNode, GetHeader(getterNode) + " [Static]");

					if (property.GetterMethod.IsAbstract)
						SetHeader(getterNode, GetHeader(getterNode) + " [Abstract]");

					if (property.GetterMethod.IsNewSlot)
						SetHeader(getterNode, GetHeader(getterNode) + " [NewSlot]");

					if (property.GetterMethod.IsVirtual)
						SetHeader(getterNode, GetHeader(getterNode) + " [Virtual]");

					if (property.GetterMethod.IsFinal)
						SetHeader(getterNode, GetHeader(getterNode) + " [Final]");

					if (property.GetterMethod.IsSpecialName)
						SetHeader(getterNode, GetHeader(getterNode) + " [SpecialName]");

					if (property.GetterMethod.IsRTSpecialName)
						SetHeader(getterNode, GetHeader(getterNode) + " [RTSpecialName]");

					if (property.GetterMethod.GenericArguments.Count != 0)
					{
						var genericParameterNodes = CreateNode("Generic Arguments Types");
						AddChildNode(getterNode, genericParameterNodes);

						foreach (var genericParameter in property.GetterMethod.GenericArguments)
							AddChildNode(genericParameterNodes, CreateNode(genericParameter.Name));
					}
				}

				if (property.SetterMethod != null)
				{
					var setterNode = CreateNode(property.SetterMethod.ShortName, property.SetterMethod);
					AddChildNode(propertyNode, setterNode);

					if (property.SetterMethod.IsStatic)
						SetHeader(setterNode, GetHeader(setterNode) + " [Static]");

					if (property.SetterMethod.IsAbstract)
						SetHeader(setterNode, GetHeader(setterNode) + " [Abstract]");

					if (property.SetterMethod.IsNewSlot)
						SetHeader(setterNode, GetHeader(setterNode) + " [NewSlot]");

					if (property.SetterMethod.IsVirtual)
						SetHeader(setterNode, GetHeader(setterNode) + " [Virtual]");

					if (property.SetterMethod.IsFinal)
						SetHeader(setterNode, GetHeader(setterNode) + " [Final]");

					if (property.SetterMethod.IsSpecialName)
						SetHeader(setterNode, GetHeader(setterNode) + " [SpecialName]");

					if (property.SetterMethod.IsRTSpecialName)
						SetHeader(setterNode, GetHeader(setterNode) + " [RTSpecialName]");

					if (property.SetterMethod.GenericArguments.Count == 0)
						continue;

					var genericParameterNodes = CreateNode("Generic Arguments Types");
					AddChildNode(setterNode, genericParameterNodes);

					foreach (var genericParameter in property.SetterMethod.GenericArguments)
						AddChildNode(genericParameterNodes, CreateNode(genericParameter.Name));
				}
			}
		}

		if (type.Methods.Count != 0)
		{
			AddChildNode(typeNode, CreateNode("Methods"));

			var methodList = type.Methods.OrderBy(o => o.ShortName).ToList();

			foreach (var method in methodList)
				if (included == null || included.Contains(method))
					GetOrCreateNode(method);
		}

		if (TypeLayout.GetMethodTable(type) != null)
		{
			var methodTableNode = CreateNode("Method Table");
			AddChildNode(typeNode, methodTableNode);

			var methodList = TypeLayout.GetMethodTable(type).OrderBy(o => o.ShortName).ToList();

			foreach (var method in methodList)
			{
				if (included != null && !included.Contains(method))
					continue;

				AddChildNode(methodTableNode, CreateNode(method.ShortName, method));
			}
		}

		return typeNode;
	}

	private TNode GetOrCreateNode(MosaMethod method)
	{
		if (map.TryGetValue(method, out var methodNode))
			return methodNode;

		var type = method.DeclaringType;
		if (type == null)
			return null;

		if (!map.TryGetValue(type, out var typeNode))
			typeNode = GetOrCreateNode(type);

		if (method.ShortName != null && (method.ShortName.StartsWith("set_") || method.ShortName.StartsWith("get_")))
			return null;

		TNode methodsNode = null;

		foreach (var node in GetChildren(typeNode))
		{
			if (GetHeader(node) != "Methods")
				continue;

			methodsNode = node;
			break;
		}

		if (methodsNode == null)
		{
			methodsNode = CreateNode("Methods");
			AddChildNode(typeNode, methodsNode);
		}

		methodNode = CreateNode(method.ShortName, method);

		map.Add(method, methodNode);

		AddToTree(methodNode, methodsNode);

		if (method.IsStatic)
			SetHeader(methodNode, GetHeader(methodNode) + " [Static]");

		if (method.IsAbstract)
			SetHeader(methodNode, GetHeader(methodNode) + " [Abstract]");

		if (method.IsNewSlot)
			SetHeader(methodNode, GetHeader(methodNode) + " [NewSlot]");

		if (method.IsVirtual)
			SetHeader(methodNode, GetHeader(methodNode) + " [Virtual]");

		if (method.IsFinal)
			SetHeader(methodNode, GetHeader(methodNode) + " [Final]");

		if (method.IsSpecialName)
			SetHeader(methodNode, GetHeader(methodNode) + " [SpecialName]");

		if (method.IsRTSpecialName)
			SetHeader(methodNode, GetHeader(methodNode) + " [RTSpecialName]");

		if (method.GenericArguments.Count == 0)
			return methodNode;

		var genericParameterNodes = CreateNode("Generic Arguments Types");
		AddChildNode(methodNode, genericParameterNodes);

		foreach (var genericParameter in method.GenericArguments)
			AddChildNode(genericParameterNodes, CreateNode(genericParameter.Name));

		return methodNode;
	}
}
