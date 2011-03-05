using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.TypeSystem.Generic;

namespace Mosa.Tools.TypeExplorer
{
	public partial class Main : Form
	{
		ITypeSystem typeSystem = new TypeSystem();
		ITypeLayout typeLayout;

		public Main()
		{
			InitializeComponent();
		}

		private void Main_Load(object sender, EventArgs e)
		{
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				LoadAssembly(openFileDialog.FileName);
			}
		}

		protected string TokenToString(TokenTypes token)
		{
			return ((int)token).ToString("X8");
		}

		protected string FormatToString(TokenTypes token)
		{
			if (!showTokenValues.Checked)
				return string.Empty;

			return "[" + TokenToString(token) + "] ";
		}

		protected string FormatRuntimeMember(RuntimeMember member)
		{
			if (!showTokenValues.Checked)
				return member.Name;

			return "[" + TokenToString(member.Token) + "] " + member.Name;
		}

		protected string FormatRuntimeType(RuntimeType type)
		{
			if (!showTokenValues.Checked)
				return type.Namespace + Type.Delimiter + type.Name;

			return "[" + TokenToString(type.Token) + "] " + type.Namespace + Type.Delimiter + type.Name;
		}

		protected void LoadAssembly(string filename)
		{
			IAssemblyLoader assemblyLoader = new AssemblyLoader();
			assemblyLoader.AddPrivatePath(System.IO.Path.GetDirectoryName(filename));

			assemblyLoader.LoadModule(filename);

			typeSystem = new TypeSystem();
			typeSystem.LoadModules(assemblyLoader.Modules);

			typeLayout = new TypeLayout(typeSystem, 4, 4);

			Update();
		}

		protected void Update()
		{
			treeView.BeginUpdate();
			treeView.Nodes.Clear();

			foreach (ITypeModule module in typeSystem.TypeModules)
			{
				TreeNode moduleNode = new TreeNode(module.Name);
				treeView.Nodes.Add(moduleNode);

				foreach (RuntimeType type in module.GetAllTypes())
				{
					TreeNode typeNode = new TreeNode(FormatRuntimeType(type));
					moduleNode.Nodes.Add(typeNode);

					if (type.BaseType != null)
					{
						TreeNode baseTypeNode = new TreeNode("Base Type: " + FormatRuntimeType(type.BaseType));
						typeNode.Nodes.Add(baseTypeNode);
					}

					CilGenericType genericType = type as CilGenericType;
					if (genericType != null)
					{
						if (genericType.BaseGenericType != null)
						{
							TreeNode genericBaseTypeNode = new TreeNode("Generic Base Type: " + FormatRuntimeType(genericType.BaseGenericType));
							typeNode.Nodes.Add(genericBaseTypeNode);
						}
					}

					if (type.Interfaces.Count != 0)
					{
						TreeNode interfacesNodes = new TreeNode("Interfaces");
						typeNode.Nodes.Add(interfacesNodes);

						foreach (RuntimeType interfaceType in type.Interfaces)
						{
							TreeNode interfaceNode = new TreeNode(FormatRuntimeType(interfaceType));
							interfacesNodes.Nodes.Add(interfaceNode);
						}
					}

					if (type.Fields.Count != 0)
					{
						TreeNode fieldsNode = new TreeNode("Fields");
						if (showSizes.Checked)
							fieldsNode.Text = fieldsNode.Text + " (Size: " + typeLayout.GetTypeSize(type).ToString() + ")";
						typeNode.Nodes.Add(fieldsNode);

						foreach (RuntimeField field in type.Fields)
						{
							TreeNode fieldNode = new TreeNode(FormatRuntimeMember(field));
							fieldsNode.Nodes.Add(fieldNode);

							if (field.IsStaticField)
								fieldNode.Text = fieldNode.Text + " [Static]";

							if (showSizes.Checked)
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

						foreach (RuntimeMethod method in type.Methods)
						{
							TreeNode methodNode = new TreeNode(FormatRuntimeMember(method));
							methodsNode.Nodes.Add(methodNode);

							if ((method.Attributes & MethodAttributes.Static) == MethodAttributes.Static)
								methodNode.Text = methodNode.Text + " [Static]";

							if (method.IsAbstract)
								methodNode.Text = methodNode.Text + " [Abstract]";
						}
					}

					if (typeLayout.GetMethodTable(type) != null)
					{
						TreeNode methodTableNode = new TreeNode("Method Table");
						typeNode.Nodes.Add(methodTableNode);

						foreach (RuntimeMethod method in typeLayout.GetMethodTable(type))
						{
							TreeNode methodNode = new TreeNode(FormatRuntimeMember(method));
							methodTableNode.Nodes.Add(methodNode);
						}
					}
				}
			}

			treeView.EndUpdate();
		}

		private void quitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void showTokenValues_Click(object sender, EventArgs e)
		{
			Update();
		}

		private void showSizes_Click(object sender, EventArgs e)
		{
			Update();
		}

	}
}
