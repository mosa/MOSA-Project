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
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Searches for plug declarations
	/// </summary>
	public class PlugStage : BaseCompilerStage, ICompilerStage
	{
		#region Data members

		protected RuntimeType plugTypeAttribute;
		protected RuntimeType plugMethodAttribute;

		#endregion Data members

		#region ICompilerStage members

		void ICompilerStage.Setup(BaseCompiler compiler)
		{
			base.Setup(compiler);

			plugTypeAttribute = typeSystem.GetType("Mosa.Internal.Plug", "Mosa.Internal.Plug", "PlugTypeAttribute");
			plugMethodAttribute = typeSystem.GetType("Mosa.Internal.Plug", "Mosa.Internal.Plug", "PlugMethodAttribute");
		}

		void ICompilerStage.Run()
		{
			if (plugTypeAttribute == null | plugMethodAttribute == null)
				return;

			foreach (RuntimeType type in this.typeSystem.GetAllTypes())
			{
				string plugTypeTarget = null;

				RuntimeAttribute typeAttribute = GetAttribute(type.CustomAttributes, plugTypeAttribute);

				if (typeAttribute != null)
				{
					object[] parameters = CustomAttributeParser.Parse(typeAttribute.Blob, typeAttribute.CtorMethod);

					if (parameters.Length >= 1)
					{
						plugTypeTarget = (string)parameters[0];
					}
				}

				foreach (RuntimeMethod method in type.Methods)
				{
					if (!method.IsStatic)
						continue;

					string plugMethodTarget = null;

					RuntimeAttribute methodAttribute = GetAttribute(method.CustomAttributes, plugMethodAttribute);

					if (methodAttribute != null)
					{
						object[] parameters = CustomAttributeParser.Parse(methodAttribute.Blob, methodAttribute.CtorMethod);

						if (parameters.Length >= 1)
						{
							plugMethodTarget = (string)parameters[0];
						}
					}

					if (plugTypeTarget != null || plugMethodTarget != null)
					{
						string targetAssemblyName;
						string targetFullTypeName;
						string targetMethodName;

						if (plugMethodTarget != null)
						{
							targetAssemblyName = ParseAssembly(plugMethodTarget);
							targetFullTypeName = ParseFullTypeName(plugMethodTarget);
							targetMethodName = ParseMethod(plugMethodTarget);
						}
						else
						{
							targetAssemblyName = ParseAssembly(plugTypeTarget);
							targetFullTypeName = RemoveAssembly(plugTypeTarget);
							targetMethodName = method.Name;
						}

						string targetNameSpace = ParseNameSpace(targetFullTypeName);
						string targetTypeName = ParseType(targetFullTypeName);

						RuntimeType targetType;

						if (targetAssemblyName != null)
							targetType = typeSystem.GetType(targetAssemblyName, targetNameSpace, targetTypeName);
						else
							targetType = typeSystem.GetType(targetNameSpace, targetTypeName);

						if (targetType == null)
						{
							Trace(InternalTrace.CompilerEvent.Warning,
								String.Format("Plug target type {0} not found. Ignoring plug.",
								targetAssemblyName != null ? (targetFullTypeName + "(in " + targetAssemblyName + ")") : targetFullTypeName));
							continue;
						}

						RuntimeMethod targetMethod = null;

						foreach (var targetMethodCandidate in targetType.Methods)
						{
							if (targetMethodCandidate.Name == targetMethodName)
							{
								if (targetMethodCandidate.IsStatic)
								{
									if (targetMethodCandidate.Matches(method))
									{
										targetMethod = targetMethodCandidate;
										break;
									}
								}
								else
								{
									if (MatchesWithStaticThis(targetMethodCandidate, method))
									{
										targetMethod = targetMethodCandidate;
										break;
									}
								}
							}
						}

						if (targetMethod != null)
						{
							Patch(targetMethod, method);
						}
						else
						{
							Trace(InternalTrace.CompilerEvent.Warning,
								String.Format("No matching plug target method {0} found in type {1}. Ignoring plug.",
								targetMethodName, targetType.ToString()));
						}
					}
				}
			}
		}

		#endregion ICompilerStage members

		private void Patch(RuntimeMethod targetMethod, RuntimeMethod method)
		{
			Trace(InternalTrace.CompilerEvent.Plug, targetMethod.FullName + " with " + method.FullName);
			compiler.PlugSystem.CreatePlug(method, targetMethod);
		}

		private RuntimeAttribute GetAttribute(List<RuntimeAttribute> attributes, RuntimeType plugAttribute)
		{
			foreach (RuntimeAttribute attribute in attributes)
			{
				if (attribute.Type == plugAttribute)
					return attribute;
			}

			return null;
		}

		private bool MatchesWithStaticThis(RuntimeMethod targetMethod, RuntimeMethod plugMethod)
		{
			if (!targetMethod.ReturnType.Matches(plugMethod.ReturnType))
				return false;

			if (targetMethod.SigParameters.Length != plugMethod.SigParameters.Length - 1)
				return false;

			if (plugMethod.SigParameters[0].Type != Metadata.CilElementType.ByRef)
				return false;

			// TODO: Compare plug.Parameters[0].Type to the target's type

			for (int i = 0; i < targetMethod.SigParameters.Length; i++)
			{
				if (!targetMethod.SigParameters[i].Matches(plugMethod.SigParameters[i + 1]))
					return false;
			}

			return true;
		}

		private string RemoveAssembly(string target)
		{
			int pos = target.IndexOf(',');

			if (pos < 0)
				return target;

			return target.Substring(target.Length - pos - 1).Trim(' ');
		}

		private string ParseAssembly(string target)
		{
			int pos = target.IndexOf(',');

			if (pos < 0)
				return null;

			return target.Substring(pos + 1).Trim(' ');
		}

		private string ParseFullTypeName(string target)
		{
			target = RemoveAssembly(target);

			int pos = target.LastIndexOf('.');

			return target.Substring(0, pos);
		}

		private string ParseMethod(string target)
		{
			target = RemoveAssembly(target);

			int pos = target.LastIndexOf('.');

			return target.Substring(pos + 1);
		}

		private string ParseNameSpace(string type)
		{
			int pos = type.LastIndexOf('.');

			if (pos < 0)
				return string.Empty;

			return type.Substring(0, pos);
		}

		private string ParseType(string target)
		{
			int pos = target.LastIndexOf('.');

			return target.Substring(pos + 1);
		}
	}
}