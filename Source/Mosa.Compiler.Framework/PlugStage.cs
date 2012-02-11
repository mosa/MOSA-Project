/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;

using Mosa.Compiler.Common;
using Mosa.Compiler.Linker;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Emits metadata for assemblies and types
	/// </summary>
	public class PlugStage : BaseAssemblyCompilerStage, IAssemblyCompilerStage
	{
		#region Data members

		private IAssemblyLinker linker;

		protected Dictionary<RuntimeMethod, string> plugMethods = new Dictionary<RuntimeMethod, string>();

		protected RuntimeType plugTypeAttribute;
		protected RuntimeType plugMethodAttribute;

		#endregion // Data members

		#region IAssemblyCompilerStage members

		void IAssemblyCompilerStage.Setup(AssemblyCompiler compiler)
		{
			base.Setup(compiler);
			this.linker = RetrieveAssemblyLinkerFromCompiler();

			plugTypeAttribute = typeSystem.GetType("Mosa.Internal", "PlugTypeAttribute");
			plugMethodAttribute = typeSystem.GetType("Mosa.Internal", "PlugMethodAttribute");
		}

		void IAssemblyCompilerStage.Run()
		{
			foreach (RuntimeType type in this.typeSystem.GetAllTypes())
			{
				bool plugType = false;

				RuntimeAttribute typeAttribute = GetAttribute(type.CustomAttributes, plugTypeAttribute);

				// TODO: Read attribute, what type is being plugged?
				if (typeAttribute != null)
				{
					object[] parameters = CustomAttributeParser.Parse(typeAttribute.Blob, typeAttribute.CtorMethod);

					int len = parameters.Length;
				}

				foreach (RuntimeMethod method in type.Methods)
				{
					bool plugMethod = plugType;

					RuntimeAttribute methodAttribute = GetAttribute(method.CustomAttributes, plugMethodAttribute);

					// TODO: Read attribute, what method is being plugged?

					//CustomAttributeParser
				}
			}
		}

		protected RuntimeAttribute GetAttribute(List<RuntimeAttribute> attributes, RuntimeType plugAttribute)
		{
			foreach (RuntimeAttribute attribute in attributes)
			{
				if (attribute.Type == plugAttribute)
					return attribute;
			}

			return null;
		}

		#endregion // IAssemblyCompilerStage members


	}
}

