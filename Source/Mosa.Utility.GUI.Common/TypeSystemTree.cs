// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Mosa.Utility.GUI.Common
{
	public class TypeSystemTree
	{
		public TreeView TreeView { get; set; }
		public TypeSystem TypeSystem;
		public MosaTypeLayout TypeLayout;

		private bool ShowSizes = true;

		private Dictionary<object, TreeNode> map = new Dictionary<object, TreeNode>();
		private HashSet<object> contains = new HashSet<object>();

		private HashSet<MosaUnit> Include;

		public bool HasFilter { get { return Include != null && Include.Count != 0; } }

		public TypeSystemTree(TreeView treeView, TypeSystem typeSystem, MosaTypeLayout typeLayout, bool showSizes = true, HashSet<MosaUnit> include = null)
		{
			TreeView = treeView;

			TypeSystem = typeSystem;
			TypeLayout = typeLayout;
			ShowSizes = showSizes;
			Include = include;

			TreeView.BeginUpdate();

			TreeView.Nodes.Clear();

			CreateTreeBase();

			CreateInitialTree();

			TreeView.EndUpdate();
		}

		public void Update()
		{
			TreeView.BeginUpdate();

			foreach (var type in TypeSystem.AllTypes)
			{
				if (Include == null || Include.Contains(type))
				{
					foreach (var method in type.Methods)
					{
						if (Include == null || Include.Contains(method))
						{
							GetOrCreateNode(method);
						}
					}
				}
			}

			TreeView.EndUpdate();
		}

		public void Update(MosaMethod method)
		{
			//TreeView.BeginUpdate();

			if (Include == null || Include.Contains(method))
			{
				GetOrCreateNode(method);
			}

			//TreeView.EndUpdate();
		}

		private void CreateTreeBase()
		{
			foreach (var module in TypeSystem.Modules)
			{
				if (Include == null || Include.Contains(module))
				{
					GetOrCreateNode(module);
				}
			}
		}

		private void CreateInitialTree()
		{
			var typeList = (new List<MosaType>(TypeSystem.AllTypes)).OrderBy(o => o.FullName).ToList();

			foreach (var type in typeList)
			{
				if (Include == null || Include.Contains(type))
				{
					GetOrCreateNode(type);
				}
			}
		}

		private void AddToTree(TreeNode node, TreeNode parent)
		{
			if (contains.Contains(node))
				return;

			if (parent == null)
			{
				TreeView.Nodes.Add(node);
			}
			else
			{
				parent.Nodes.Add(node);
			}
		}

		private TreeNode GetOrCreateNode(MosaModule module)
		{
			if (!map.TryGetValue(module, out TreeNode moduleNode))
			{
				moduleNode = new TreeNode(module.Name) { Tag = module };

				map.Add(module, moduleNode);

				AddToTree(moduleNode, null);
			}

			return moduleNode;
		}

		private TreeNode GetOrCreateNode(string name, MosaModule module)
		{
			name = (string.IsNullOrWhiteSpace(name)) ? "[No Namespace]" : name;

			string index = module + ":" + name;

			if (!map.TryGetValue(index, out TreeNode namespaceNode))
			{
				namespaceNode = new TreeNode(name);

				map.Add(index, namespaceNode);

				var moduleNode = GetOrCreateNode(module);

				AddToTree(namespaceNode, moduleNode);
			}

			return namespaceNode;
		}

		private TreeNode GetOrCreateNode(MosaType type)
		{
			if (map.TryGetValue(type, out TreeNode typeNode))
			{
				return typeNode;
			}

			typeNode = new TreeNode(type.FullName) { Tag = type };

			map.Add(type, typeNode);

			var namespaceNode = GetOrCreateNode(type.Namespace, type.Module);

			AddToTree(typeNode, namespaceNode);

			if (type.BaseType != null)
			{
				var baseTypeNode = new TreeNode("Base Type: " + type.BaseType.FullName);
				typeNode.Nodes.Add(baseTypeNode);
			}

			if (type.DeclaringType != null)
			{
				var declaringTypeNode = new TreeNode("Declaring Type: " + type.DeclaringType.FullName);
				typeNode.Nodes.Add(declaringTypeNode);
			}

			if (type.ElementType != null)
			{
				var elementTypeNode = new TreeNode("Element Type: " + type.ElementType.FullName);
				typeNode.Nodes.Add(elementTypeNode);
			}

			if (type.Interfaces.Count != 0)
			{
				var interfacesNodes = new TreeNode("Interfaces");
				typeNode.Nodes.Add(interfacesNodes);

				foreach (var interfaceType in type.Interfaces)
				{
					var interfaceNode = new TreeNode(interfaceType.FullName);
					interfacesNodes.Nodes.Add(interfaceNode);
				}
			}

			if (type.Fields.Count != 0)
			{
				var fieldsNode = new TreeNode("Fields");
				if (ShowSizes)
					fieldsNode.Text = fieldsNode.Text + " (Count: " + type.Fields.Count.ToString() + " - Size: " + TypeLayout.GetTypeSize(type).ToString() + ")";

				typeNode.Nodes.Add(fieldsNode);

				foreach (var field in type.Fields)
				{
					var fieldNode = new TreeNode(field.ShortName);
					fieldsNode.Nodes.Add(fieldNode);

					if (field.IsStatic)
						fieldNode.Text += " [Static]";

					if (ShowSizes)
					{
						fieldNode.Text = fieldNode.Text + " (Size: " + TypeLayout.GetFieldSize(field).ToString();

						if (!field.IsStatic)
							fieldNode.Text = fieldNode.Text + " - Offset: " + TypeLayout.GetFieldOffset(field).ToString();

						fieldNode.Text += ")";
					}
				}
			}

			if (type.Properties.Count != 0)
			{
				var propertiesNode = new TreeNode("Properties");

				if (ShowSizes)
					propertiesNode.Text = propertiesNode.Text + " (Count: " + type.Properties.Count.ToString() + ")";

				typeNode.Nodes.Add(propertiesNode);

				foreach (var property in type.Properties)
				{
					if (Include == null || Include.Contains(property))
					{
						var propertyNode = new TreeNode(property.ShortName) { Tag = property };
						propertiesNode.Nodes.Add(propertyNode);

						if (property.GetterMethod != null)
						{
							var getterNode = new TreeNode(property.GetterMethod.ShortName) { Tag = property.GetterMethod };

							propertyNode.Nodes.Add(getterNode);

							if (property.GetterMethod.IsStatic)
								getterNode.Text += " [Static]";

							if (property.GetterMethod.IsAbstract)
								getterNode.Text += " [Abstract]";

							if (property.GetterMethod.IsNewSlot)
							{
								getterNode.Text += " [NewSlot]";
							}

							if (property.GetterMethod.IsVirtual)
								getterNode.Text += " [Virtual]";

							if (property.GetterMethod.IsFinal)
							{
								getterNode.Text += " [Final]";
							}

							if (property.GetterMethod.IsSpecialName)
								getterNode.Text += " [SpecialName]";

							if (property.GetterMethod.IsRTSpecialName)
								getterNode.Text += " [RTSpecialName]";

							if (property.GetterMethod.GenericArguments.Count != 0)
							{
								var genericParameterNodes = new TreeNode("Generic Arguments Types");
								getterNode.Nodes.Add(genericParameterNodes);

								foreach (var genericParameter in property.GetterMethod.GenericArguments)
								{
									var GenericParameterNode = new TreeNode(genericParameter.Name);
									genericParameterNodes.Nodes.Add(GenericParameterNode);
								}
							}
						}

						if (property.SetterMethod != null)
						{
							var setterNode = new TreeNode(property.SetterMethod.ShortName) { Tag = property.SetterMethod };
							propertyNode.Nodes.Add(setterNode);

							if (property.SetterMethod.IsStatic)
								setterNode.Text += " [Static]";

							if (property.SetterMethod.IsAbstract)
								setterNode.Text += " [Abstract]";

							if (property.SetterMethod.IsNewSlot)
								setterNode.Text += " [NewSlot]";

							if (property.SetterMethod.IsVirtual)
								setterNode.Text += " [Virtual]";

							if (property.SetterMethod.IsFinal)
								setterNode.Text += " [Final]";

							if (property.SetterMethod.IsSpecialName)
								setterNode.Text += " [SpecialName]";

							if (property.SetterMethod.IsRTSpecialName)
								setterNode.Text += " [RTSpecialName]";

							if (property.SetterMethod.GenericArguments.Count != 0)
							{
								var genericParameterNodes = new TreeNode("Generic Arguments Types");
								setterNode.Nodes.Add(genericParameterNodes);

								foreach (var genericParameter in property.SetterMethod.GenericArguments)
								{
									var GenericParameterNode = new TreeNode(genericParameter.Name);
									genericParameterNodes.Nodes.Add(GenericParameterNode);
								}
							}
						}
					}
				}
			}

			if (type.Methods.Count != 0)
			{
				typeNode.Nodes.Add(new TreeNode("Methods"));

				var methodList = (new List<MosaMethod>(type.Methods)).OrderBy(o => o.ShortName).ToList();

				// Methods
				foreach (var method in type.Methods)
				{
					if (Include == null || Include.Contains(method))
					{
						var methonode = GetOrCreateNode(method);
					}
				}
			}

			//  Method Table
			if (TypeLayout.GetMethodTable(type) != null)
			{
				var methodTableNode = new TreeNode("Method Table");
				typeNode.Nodes.Add(methodTableNode);

				var methodList = (new List<MosaMethod>(TypeLayout.GetMethodTable(type))).OrderBy(o => o.ShortName).ToList();

				foreach (var method in methodList)
				{
					if (Include == null || Include.Contains(method))
					{
						var methodNode = new TreeNode(method.ShortName) { Tag = method };
						methodTableNode.Nodes.Add(methodNode);
					}
				}
			}

			return typeNode;
		}

		private TreeNode GetOrCreateNode(MosaMethod method)
		{
			if (map.TryGetValue(method, out TreeNode methodNode))
			{
				return methodNode;
			}

			var type = method.DeclaringType;

			if (!map.TryGetValue(type, out TreeNode typeNode))
			{
				typeNode = GetOrCreateNode(type);
			}

			if (method.ShortName.StartsWith("set_") || method.ShortName.StartsWith("get_"))
				return null;

			// find "Methods" node
			TreeNode methodsNode = null;

			foreach (var node in typeNode.Nodes)
			{
				if (((TreeNode)node).Text == "Methods")
				{
					methodsNode = (TreeNode)node;
					break;
				}
			}

			if (methodsNode == null)
			{
				methodsNode = new TreeNode("Methods");
				typeNode.Nodes.Add(methodsNode);
			}

			methodNode = new TreeNode(method.ShortName) { Tag = method };

			map.Add(method, methodNode);

			AddToTree(methodNode, methodsNode);

			if (method.IsStatic)
				methodNode.Text += " [Static]";

			if (method.IsAbstract)
				methodNode.Text += " [Abstract]";

			if (method.IsNewSlot)
				methodNode.Text += " [NewSlot]";

			if (method.IsVirtual)
				methodNode.Text += " [Virtual]";

			if (method.IsFinal)
				methodNode.Text += " [Final]";

			if (method.IsSpecialName)
				methodNode.Text += " [SpecialName]";

			if (method.IsRTSpecialName)
				methodNode.Text += " [RTSpecialName]";

			if (method.GenericArguments.Count != 0)
			{
				var genericParameterNodes = new TreeNode("Generic Arguments Types");
				methodNode.Nodes.Add(genericParameterNodes);

				foreach (var genericParameter in method.GenericArguments)
				{
					var GenericParameterNode = new TreeNode(genericParameter.Name);
					genericParameterNodes.Nodes.Add(GenericParameterNode);
				}
			}

			return methodNode;
		}
	}
}
