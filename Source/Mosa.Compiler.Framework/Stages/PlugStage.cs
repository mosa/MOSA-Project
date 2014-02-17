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
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Searches for plug declarations
	/// </summary>
	public class PlugStage : BaseCompilerStage, ICompilerStage
	{
		#region Data members

		protected MosaType plugTypeAttribute;
		protected MosaType plugMethodAttribute;

		#endregion Data members

		#region ICompilerStage members

		const string PlugTypeAttribute = "Mosa.Internal.Plug.PlugTypeAttribute";
		const string PlugMethodAttribute = "Mosa.Internal.Plug.PlugMethodAttribute";

		void ICompilerStage.Setup(BaseCompiler compiler)
		{
			base.Setup(compiler);
		}

		void ICompilerStage.Run()
		{
			if (plugTypeAttribute == null | plugMethodAttribute == null)
				return;

			foreach (var type in typeSystem.AllTypes)
			{
				string plugTypeTarget = null;

				object[] typeAttribute = type.GetAttribute(PlugTypeAttribute);

				if (typeAttribute != null)
				{
					plugTypeTarget = (string)typeAttribute[0];
				}

				foreach (var method in type.Methods)
				{
					if (!method.IsStatic)
						continue;

					string plugMethodTarget = null;

					object[] methodAttribute = method.GetAttribute(PlugMethodAttribute);

					if (methodAttribute != null)
					{
						plugMethodTarget = (string)methodAttribute[0];
					}

					if (plugTypeTarget != null || plugMethodTarget != null)
					{
						string targetModuleName;
						string targetFullTypeName;
						string targetMethodName;

						if (plugMethodTarget != null)
						{
							targetModuleName = ParseModule(plugMethodTarget);
							targetFullTypeName = ParseFullTypeName(plugMethodTarget);
							targetMethodName = ParseMethod(plugMethodTarget);
						}
						else
						{
							targetModuleName = ParseModule(plugTypeTarget);
							targetFullTypeName = RemoveModule(plugTypeTarget);
							targetMethodName = method.Name;
						}

						string targetNameSpace = ParseNameSpace(targetFullTypeName);
						string targetTypeName = ParseType(targetFullTypeName);

						MosaType targetType;

						if (targetModuleName != null)
							targetType = typeSystem.GetTypeByName(targetModuleName, targetNameSpace, targetTypeName);
						else
							targetType = typeSystem.GetTypeByName(targetNameSpace, targetTypeName);

						if (targetType == null)
						{
							Trace(InternalTrace.CompilerEvent.Warning,
								String.Format("Plug target type {0} not found. Ignoring plug.",
								targetModuleName != null ? (targetFullTypeName + "(in " + targetModuleName + ")") : targetFullTypeName));
							continue;
						}

						MosaMethod targetMethod = null;

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

		private void Patch(MosaMethod targetMethod, MosaMethod method)
		{
			Trace(InternalTrace.CompilerEvent.Plug, targetMethod.FullName + " with " + method.FullName);
			compiler.PlugSystem.CreatePlug(method, targetMethod);
		}

		private bool MatchesWithStaticThis(MosaMethod targetMethod, MosaMethod plugMethod)
		{
			if (!targetMethod.ReturnType.Matches(plugMethod.ReturnType))
				return false;

			if (targetMethod.Parameters.Count != plugMethod.Parameters.Count - 1)
				return false;

			if (plugMethod.Parameters[0].Type.IsValueType && !plugMethod.Parameters[0].Type.IsManagedPointer)
				return false;

			// TODO: Compare plug.Parameters[0].Type to the target's type

			for (int i = 0; i < targetMethod.Parameters.Count; i++)
			{
				if (!targetMethod.Parameters[i].Matches(plugMethod.Parameters[i + 1]))
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

		private string ParseModule(string target)
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