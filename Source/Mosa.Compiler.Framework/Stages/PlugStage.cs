/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.MosaTypeSystem;
using System;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Searches for plug declarations
	/// </summary>
	public class PlugStage : BaseCompilerStage
	{
		#region Data members

		protected MosaType plugTypeAttribute;
		protected MosaType plugMethodAttribute;

		#endregion Data members

		private const string PlugTypeAttribute = "Mosa.Internal.Plug.PlugTypeAttribute";
		private const string PlugMethodAttribute = "Mosa.Internal.Plug.PlugMethodAttribute";

		protected override void Run()
		{
			foreach (var type in TypeSystem.AllTypes)
			{
				string plugTypeTarget = null;

				var typeAttribute = type.FindCustomAttribute(PlugTypeAttribute);

				if (typeAttribute != null)
				{
					plugTypeTarget = (string)typeAttribute.Arguments[0].Value;
				}

				foreach (var method in type.Methods)
				{
					if (!method.IsStatic)
						continue;

					string plugMethodTarget = null;

					var methodAttribute = method.FindCustomAttribute(PlugMethodAttribute);

					if (methodAttribute != null)
					{
						plugMethodTarget = (string)methodAttribute.Arguments[0].Value;
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
							targetFullTypeName = RemoveModule(plugTypeTarget);
							targetMethodName = method.Name;
						}

						string targetNameSpace = ParseNameSpace(targetFullTypeName);
						string targetTypeName = ParseType(targetFullTypeName);

						MosaType targetType;

						if (targetAssemblyName != null)
							targetType = TypeSystem.GetTypeByName(TypeSystem.GetModuleByAssembly(targetAssemblyName), targetNameSpace, targetTypeName);
						else
							targetType = TypeSystem.GetTypeByName(targetNameSpace, targetTypeName);

						if (targetType == null)
						{
							Trace(InternalTrace.CompilerEvent.Warning,
								String.Format("Plug target type {0} not found. Ignoring plug.",
								targetAssemblyName != null ? (targetFullTypeName + "(in " + targetAssemblyName + ")") : targetFullTypeName));
							continue;
						}

						MosaMethod targetMethod = null;

						foreach (var targetMethodCandidate in targetType.Methods)
						{
							if (targetMethodCandidate.Name == targetMethodName)
							{
								if (targetMethodCandidate.IsStatic)
								{
									if (targetMethodCandidate.Equals(method))
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

		private void Patch(MosaMethod targetMethod, MosaMethod method)
		{
			Trace(InternalTrace.CompilerEvent.Plug, targetMethod.FullName + " with " + method.FullName);
			Compiler.PlugSystem.CreatePlug(method, targetMethod);
		}

		private bool MatchesWithStaticThis(MosaMethod targetMethod, MosaMethod plugMethod)
		{
			if (!targetMethod.Signature.ReturnType.Equals(plugMethod.Signature.ReturnType))
				return false;

			if (targetMethod.Signature.Parameters.Count != plugMethod.Signature.Parameters.Count - 1)
				return false;

			if (plugMethod.Signature.Parameters[0].ParameterType.IsValueType && !plugMethod.Signature.Parameters[0].ParameterType.IsManagedPointer)
				return false;

			// TODO: Compare plug.Parameters[0].Type to the target's type

			for (int i = 0; i < targetMethod.Signature.Parameters.Count; i++)
			{
				if (!targetMethod.Signature.Parameters[i].Equals(plugMethod.Signature.Parameters[i + 1]))
					return false;
			}

			return true;
		}

		private string RemoveModule(string target)
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
			target = RemoveModule(target);

			int pos = target.LastIndexOf('.');

			return target.Substring(0, pos);
		}

		private string ParseMethod(string target)
		{
			target = RemoveModule(target);

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