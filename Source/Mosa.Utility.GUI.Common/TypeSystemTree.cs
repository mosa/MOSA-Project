// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Mosa.Utility.GUI.Common
{
	public static class TypeSystemTree
	{
		public static void UpdateTree(TreeView treeView, TypeSystem typeSystem, MosaTypeLayout typeLayout, bool showSizes)
		{
			treeView.BeginUpdate();
			treeView.Nodes.Clear();

			foreach (var module in typeSystem.Modules)
			{
				var namespaces = new List<TreeNode>();

				var moduleNode = new TreeNode(module.Name);
				treeView.Nodes.Add(moduleNode);

				var typeList = (new List<MosaType>(module.Types.Values)).OrderBy(o => o.FullName).ToList();

				foreach (var type in typeList)
				{
					TreeNode namespaceNode = null;
					string @namespace = (string.IsNullOrWhiteSpace(type.Namespace)) ? "[No Namespace]" : type.Namespace;
					foreach (var node in namespaces)
					{
						if (node.Text.Equals(@namespace))
						{
							namespaceNode = node;
							break;
						}
					}

					if (namespaceNode == null)
					{
						namespaceNode = new TreeNode(@namespace);
						moduleNode.Nodes.Add(namespaceNode);
						namespaces.Add(namespaceNode);
					}

					var typeNode = new TreeNode(type.FullName);
					namespaceNode.Nodes.Add(typeNode);

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
						if (showSizes)
							fieldsNode.Text = fieldsNode.Text + " (Count: " + type.Fields.Count.ToString() + " - Size: " + typeLayout.GetTypeSize(type).ToString() + ")";

						typeNode.Nodes.Add(fieldsNode);

						foreach (var field in type.Fields)
						{
							var fieldNode = new TreeNode(field.ShortName);
							fieldsNode.Nodes.Add(fieldNode);

							if (field.IsStatic)
								fieldNode.Text += " [Static]";

							if (showSizes)
							{
								fieldNode.Text = fieldNode.Text + " (Size: " + typeLayout.GetFieldSize(field).ToString();

								if (!field.IsStatic)
									fieldNode.Text = fieldNode.Text + " - Offset: " + typeLayout.GetFieldOffset(field).ToString();

								fieldNode.Text += ")";
							}
						}
					}

					if (type.Properties.Count != 0)
					{
						var propertiesNode = new TreeNode("Properties");
						if (showSizes)
							propertiesNode.Text = propertiesNode.Text + " (Count: " + type.Properties.Count.ToString() + ")";

						typeNode.Nodes.Add(propertiesNode);

						foreach (var property in type.Properties)
						{
							var propertyNode = new ViewNode<MosaProperty>(property, property.ShortName);
							propertiesNode.Nodes.Add(propertyNode);

							if (property.GetterMethod != null)
							{
								var getterNode = new ViewNode<MosaMethod>(property.GetterMethod, property.GetterMethod.ShortName);
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
										TreeNode GenericParameterNode = new TreeNode(genericParameter.Name);
										genericParameterNodes.Nodes.Add(GenericParameterNode);
									}
								}
							}

							if (property.SetterMethod != null)
							{
								var setterNode = new ViewNode<MosaMethod>(property.SetterMethod, property.SetterMethod.ShortName);
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

					if (type.Methods.Count != 0)
					{
						var methodsNode = new TreeNode("Methods");
						typeNode.Nodes.Add(methodsNode);

						foreach (MosaMethod method in type.Methods)
						{
							if (method.ShortName.StartsWith("set_") || method.ShortName.StartsWith("get_")) continue;

							var methodNode = new ViewNode<MosaMethod>(method, method.ShortName);
							methodsNode.Nodes.Add(methodNode);

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
						}
					}

					if (typeLayout.GetMethodTable(type) != null)
					{
						var methodTableNode = new TreeNode("Method Table");
						typeNode.Nodes.Add(methodTableNode);

						foreach (var method in typeLayout.GetMethodTable(type))
						{
							var methodNode = new ViewNode<MosaMethod>(method, method.ShortName);
							methodTableNode.Nodes.Add(methodNode);
						}
					}
				}
			}

			treeView.EndUpdate();
		}
	}
}
