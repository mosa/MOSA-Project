// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Avalonia.Controls;
using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Tool.Explorer.Avalonia;

public class TypeSystemTree
{
	public TreeView TreeView { get; set; }
	public readonly TypeSystem TypeSystem;
	public readonly MosaTypeLayout TypeLayout;

	private readonly bool showSizes;

	private readonly Dictionary<object, TreeViewItem> map = new Dictionary<object, TreeViewItem>();
	private readonly HashSet<object> contains = new HashSet<object>();
	private readonly HashSet<MosaUnit> included;

	public bool HasFilter => included != null && included.Count != 0;

	public TypeSystemTree(TreeView treeView, TypeSystem typeSystem, MosaTypeLayout typeLayout, bool showSizesCheck = true, HashSet<MosaUnit> includedSet = null)
	{
		TreeView = treeView;

		TypeSystem = typeSystem;
		TypeLayout = typeLayout;
		showSizes = showSizesCheck;
		included = includedSet;

		TreeView.Items.Clear();

		CreateTreeBase();
		CreateInitialTree();
	}

	public void Update()
	{
		foreach (var type in TypeSystem.AllTypes)
		{
			if (included != null && !included.Contains(type))
				continue;

			foreach (var method in type.Methods)
				if (included == null || included.Contains(method))
					GetOrCreateNode(method);
		}
	}

	public void Update(MosaMethod method)
	{
		if (included == null || included.Contains(method))
			GetOrCreateNode(method);
	}

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

	private void AddToTree(TreeViewItem node, TreeViewItem parent)
	{
		if (contains.Contains(node))
			return;

		if (parent == null)
			TreeView.Items.Add(node);
		else
			parent.Items.Add(node);
	}

	private TreeViewItem GetOrCreateNode(MosaModule module)
	{
		if (map.TryGetValue(module, out TreeViewItem moduleNode))
			return moduleNode;

		moduleNode = new TreeViewItem { Header = module.Name, Tag = module };

		map.Add(module, moduleNode);

		AddToTree(moduleNode, null);

		return moduleNode;
	}

	private TreeViewItem GetOrCreateNode(string name, MosaModule module)
	{
		name = (string.IsNullOrWhiteSpace(name)) ? "[No Namespace]" : name;

		var index = module + ":" + name;

		if (map.TryGetValue(index, out TreeViewItem namespaceNode))
			return namespaceNode;

		namespaceNode = new TreeViewItem { Header = name };

		map.Add(index, namespaceNode);

		var moduleNode = GetOrCreateNode(module);
		AddToTree(namespaceNode, moduleNode);

		return namespaceNode;
	}

	private TreeViewItem GetOrCreateNode(MosaType type)
	{
		if (map.TryGetValue(type, out TreeViewItem typeNode))
			return typeNode;

		typeNode = new TreeViewItem { Header = type.FullName, Tag = type };

		map.Add(type, typeNode);

		var namespaceNode = GetOrCreateNode(type.Namespace, type.Module);

		AddToTree(typeNode, namespaceNode);

		if (type.BaseType != null)
		{
			var baseTypeNode = new TreeViewItem { Header = $"Base Type: {type.BaseType.FullName}" };
			typeNode.Items.Add(baseTypeNode);
		}

		if (type.DeclaringType != null)
		{
			var declaringTypeNode = new TreeViewItem { Header = $"Declaring Type: {type.DeclaringType.FullName}" };
			typeNode.Items.Add(declaringTypeNode);
		}

		if (type.ElementType != null)
		{
			var elementTypeNode = new TreeViewItem { Header = $"Element Type: {type.ElementType.FullName}" };
			typeNode.Items.Add(elementTypeNode);
		}

		if (type.Interfaces.Count != 0)
		{
			var interfacesNodes = new TreeViewItem { Header = "Interfaces" };
			typeNode.Items.Add(interfacesNodes);

			foreach (var interfaceType in type.Interfaces)
			{
				var interfaceNode = new TreeViewItem { Header = interfaceType.FullName };
				interfacesNodes.Items.Add(interfaceNode);
			}
		}

		if (type.Fields.Count != 0)
		{
			var fieldsNode = new TreeViewItem { Header = "Fields" };

			if (showSizes && !type.HasOpenGenericParams)
				fieldsNode.Header = fieldsNode.Header + " (Count: " + type.Fields.Count + " - Size: " + TypeLayout.GetTypeLayoutSize(type) + ")";

			typeNode.Items.Add(fieldsNode);

			foreach (var field in type.Fields)
			{
				var fieldNode = new TreeViewItem { Header = field.ShortName };
				fieldsNode.Items.Add(fieldNode);

				if (field.IsStatic)
					fieldNode.Header += " [Static]";

				if (!showSizes || field.HasOpenGenericParams)
					continue;

				fieldNode.Header = fieldNode.Header + " (Size: " + TypeLayout.GetFieldSize(field);

				if (!field.IsStatic)
					fieldNode.Header = fieldNode.Header + " - Offset: " + TypeLayout.GetFieldOffset(field);

				fieldNode.Header += ")";
			}
		}

		if (type.Properties.Count != 0)
		{
			var propertiesNode = new TreeViewItem { Header = "Properties" };

			if (showSizes)
				propertiesNode.Header = propertiesNode.Header + " (Count: " + type.Properties.Count + ")";

			typeNode.Items.Add(propertiesNode);

			foreach (var property in type.Properties)
			{
				if (included != null && !included.Contains(property))
					continue;

				var propertyNode = new TreeViewItem { Header = property.ShortName, Tag = property };
				propertiesNode.Items.Add(propertyNode);

				if (property.GetterMethod != null)
				{
					var getterNode = new TreeViewItem { Header = property.GetterMethod.ShortName, Tag = property.GetterMethod };

					propertyNode.Items.Add(getterNode);

					if (property.GetterMethod.IsStatic)
						getterNode.Header += " [Static]";

					if (property.GetterMethod.IsAbstract)
						getterNode.Header += " [Abstract]";

					if (property.GetterMethod.IsNewSlot)
						getterNode.Header += " [NewSlot]";

					if (property.GetterMethod.IsVirtual)
						getterNode.Header += " [Virtual]";

					if (property.GetterMethod.IsFinal)
						getterNode.Header += " [Final]";

					if (property.GetterMethod.IsSpecialName)
						getterNode.Header += " [SpecialName]";

					if (property.GetterMethod.IsRTSpecialName)
						getterNode.Header += " [RTSpecialName]";

					if (property.GetterMethod.GenericArguments.Count != 0)
					{
						var genericParameterNodes = new TreeViewItem { Header = "Generic Arguments Types" };
						getterNode.Items.Add(genericParameterNodes);

						foreach (var genericParameter in property.GetterMethod.GenericArguments)
						{
							var genericParameterNode = new TreeViewItem { Header = genericParameter.Name };
							genericParameterNodes.Items.Add(genericParameterNode);
						}
					}
				}

				if (property.SetterMethod != null)
				{
					var setterNode = new TreeViewItem { Header = property.SetterMethod.ShortName, Tag = property.SetterMethod };
					propertyNode.Items.Add(setterNode);

					if (property.SetterMethod.IsStatic)
						setterNode.Header += " [Static]";

					if (property.SetterMethod.IsAbstract)
						setterNode.Header += " [Abstract]";

					if (property.SetterMethod.IsNewSlot)
						setterNode.Header += " [NewSlot]";

					if (property.SetterMethod.IsVirtual)
						setterNode.Header += " [Virtual]";

					if (property.SetterMethod.IsFinal)
						setterNode.Header += " [Final]";

					if (property.SetterMethod.IsSpecialName)
						setterNode.Header += " [SpecialName]";

					if (property.SetterMethod.IsRTSpecialName)
						setterNode.Header += " [RTSpecialName]";

					if (property.SetterMethod.GenericArguments.Count != 0)
					{
						var genericParameterNodes = new TreeViewItem { Header = "Generic Arguments Types" };
						setterNode.Items.Add(genericParameterNodes);

						foreach (var genericParameter in property.SetterMethod.GenericArguments)
						{
							var genericParameterNode = new TreeViewItem { Header = genericParameter.Name };
							genericParameterNodes.Items.Add(genericParameterNode);
						}
					}
				}
			}
		}

		if (type.Methods.Count != 0)
		{
			typeNode.Items.Add(new TreeViewItem { Header = "Methods" });

			var methodList = type.Methods.OrderBy(o => o.ShortName).ToList();

			// Methods
			foreach (var method in methodList)
				if (included == null || included.Contains(method))
					GetOrCreateNode(method);
		}

		//  Method Table
		if (TypeLayout.GetMethodTable(type) != null)
		{
			var methodTableNode = new TreeViewItem { Header = "Method Table" };
			typeNode.Items.Add(methodTableNode);

			var methodList = TypeLayout.GetMethodTable(type).OrderBy(o => o.ShortName).ToList();

			foreach (var method in methodList)
			{
				if (included == null || included.Contains(method))
				{
					var methodNode = new TreeViewItem { Header = method.ShortName, Tag = method };
					methodTableNode.Items.Add(methodNode);
				}
			}
		}

		return typeNode;
	}

	private TreeViewItem GetOrCreateNode(MosaMethod method)
	{
		if (map.TryGetValue(method, out TreeViewItem methodNode))
			return methodNode;

		var type = method.DeclaringType;
		if (type == null)
			return null;

		if (!map.TryGetValue(type, out TreeViewItem typeNode))
			typeNode = GetOrCreateNode(type);

		if (method.ShortName != null && (method.ShortName.StartsWith("set_") || method.ShortName.StartsWith("get_")))
			return null;

		// find "Methods" node
		TreeViewItem methodsNode = null;

		foreach (var node in typeNode.Items)
		{
			if ((string)((TreeViewItem)node).Header != "Methods")
				continue;

			methodsNode = (TreeViewItem)node;
			break;
		}

		if (methodsNode == null)
		{
			methodsNode = new TreeViewItem { Header = "Methods" };
			typeNode.Items.Add(methodsNode);
		}

		methodNode = new TreeViewItem { Header = method.ShortName, Tag = method };

		map.Add(method, methodNode);

		AddToTree(methodNode, methodsNode);

		if (method.IsStatic)
			methodNode.Header += " [Static]";

		if (method.IsAbstract)
			methodNode.Header += " [Abstract]";

		if (method.IsNewSlot)
			methodNode.Header += " [NewSlot]";

		if (method.IsVirtual)
			methodNode.Header += " [Virtual]";

		if (method.IsFinal)
			methodNode.Header += " [Final]";

		if (method.IsSpecialName)
			methodNode.Header += " [SpecialName]";

		if (method.IsRTSpecialName)
			methodNode.Header += " [RTSpecialName]";

		if (method.GenericArguments.Count == 0)
			return methodNode;

		var genericParameterNodes = new TreeViewItem { Header = "Generic Arguments Types" };
		methodNode.Items.Add(genericParameterNodes);

		foreach (var genericParameter in method.GenericArguments)
		{
			var genericParameterNode = new TreeViewItem { Header = genericParameter.Name };
			genericParameterNodes.Items.Add(genericParameterNode);
		}

		return methodNode;
	}
}
