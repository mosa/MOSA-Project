/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Metadata;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Windows.Forms;

namespace Mosa.Tool.TinySimulator
{
	public partial class AssembliesView : SimulatorDockContent
	{
		private bool showTokenValues = true;

		public AssembliesView()
		{
			InitializeComponent();
		}

		public void UpdateTree()
		{
			var typeSystem = MainForm.TypeSystem;
			var typeLayout = MainForm.TypeLayout;
			bool showSizes = true;

			if (typeSystem == null)
				return;

			treeView.BeginUpdate();

			treeView.Nodes.Clear();

			foreach (var assembly in typeSystem.AllAssemblies)
			{
				TreeNode moduleNode = new TreeNode(assembly.Name);
				treeView.Nodes.Add(moduleNode);

				foreach (var type in typeSystem.AllTypes)
				{
					if (type.Assembly != assembly)
						continue;

					TreeNode typeNode = new TreeNode(FormatMosaType(type));
					moduleNode.Nodes.Add(typeNode);

					if (type.BaseType != null)
					{
						TreeNode baseTypeNode = new TreeNode("Base Type: " + FormatMosaType(type.BaseType));
						typeNode.Nodes.Add(baseTypeNode);
					}

					if (type.GenericBaseType != null)
					{
						TreeNode genericBaseTypeNode = new TreeNode("Generic Base Type: " + FormatMosaType(type.GenericBaseType));
						typeNode.Nodes.Add(genericBaseTypeNode);
					}

					//CilGenericType genericOpenType = typeSystem.GetOpenGeneric(type);
					//if (genericOpenType != null)
					//{
					//	TreeNode genericOpenTypeNode = new TreeNode("Open Generic Type: " + FormatMosaType(genericOpenType));
					//	typeNode.Nodes.Add(genericOpenTypeNode);
					//}

					if (type.GenericParameters.Count != 0)
					{
						TreeNode genericParameterNodes = new TreeNode("Generic Parameters");
						typeNode.Nodes.Add(genericParameterNodes);

						foreach (var genericParameter in type.GenericParameters)
						{
							TreeNode GenericParameterNode = new TreeNode(genericParameter.Name);
							genericParameterNodes.Nodes.Add(GenericParameterNode);
						}
					}

					if (type.Interfaces.Count != 0)
					{
						TreeNode interfacesNodes = new TreeNode("Interfaces");
						typeNode.Nodes.Add(interfacesNodes);

						foreach (MosaType interfaceType in type.Interfaces)
						{
							TreeNode interfaceNode = new TreeNode(FormatMosaType(interfaceType));
							interfacesNodes.Nodes.Add(interfaceNode);
						}
					}

					if (type.Fields.Count != 0)
					{
						TreeNode fieldsNode = new TreeNode("Fields");
						if (showSizes)
							fieldsNode.Text = fieldsNode.Text + " (Count: " + type.Fields.Count.ToString() + " - Size: " + typeLayout.GetTypeSize(type).ToString() + ")";

						typeNode.Nodes.Add(fieldsNode);

						foreach (MosaField field in type.Fields)
						{
							TreeNode fieldNode = new TreeNode(FormatRuntimeMember(field));
							fieldsNode.Nodes.Add(fieldNode);

							if (field.IsStaticField)
								fieldNode.Text = fieldNode.Text + " [Static]";

							if (showSizes)
							{
								fieldNode.Text = fieldNode.Text + " (Size: " + typeLayout.GetFieldSize(field).ToString();

								if (!field.IsStaticField)
									fieldNode.Text = fieldNode.Text + " - Offset: " + typeLayout.GetFieldOffset(field).ToString();

								fieldNode.Text = fieldNode.Text + ")";
							}
						}
					}

					if (type.Methods.Count != 0)
					{
						TreeNode methodsNode = new TreeNode("Methods");
						typeNode.Nodes.Add(methodsNode);

						foreach (MosaMethod method in type.Methods)
						{
							TreeNode methodNode = new ViewNode<MosaMethod>(method, FormatRuntimeMember(method));
							methodsNode.Nodes.Add(methodNode);

							if (method.IsStatic)
								methodNode.Text = methodNode.Text + " [Static]";

							if (method.IsAbstract)
								methodNode.Text = methodNode.Text + " [Abstract]";
						}
					}

					if (typeLayout.GetMethodTable(type) != null)
					{
						TreeNode methodTableNode = new TreeNode("Method Table");
						typeNode.Nodes.Add(methodTableNode);

						foreach (MosaMethod method in typeLayout.GetMethodTable(type))
						{
							TreeNode methodNode = new ViewNode<MosaMethod>(method, FormatRuntimeMember(method));
							methodTableNode.Nodes.Add(methodNode);
						}
					}
				}
			}

			treeView.EndUpdate();
		}

		protected string TokenToString(Token token)
		{
			return token.ToInt32().ToString("X8");
		}

		protected string FormatToString(Token token)
		{
			if (!showTokenValues)
				return string.Empty;

			return "[" + TokenToString(token) + "] ";
		}

		protected string FormatRuntimeMember(MosaField field)
		{
			return field.Name;
		}

		protected string FormatRuntimeMember(MosaMethod method)
		{
			return method.Name;
		}

		protected string FormatMosaType(MosaType type)
		{
			return type.Namespace + Type.Delimiter + type.Name;
		}

		public class ViewNode<T> : TreeNode
		{
			public T Type;

			public ViewNode(T type, string name)
				: base(name)
			{
				this.Type = type;
			}

			public override string ToString()
			{
				return Name;
			}
		}
	}
}