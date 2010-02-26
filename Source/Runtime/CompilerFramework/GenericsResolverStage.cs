/*
 * (c) 2010 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.Vm;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// This stage is used to resolve methods inside generic classes or structs.
	/// For every type instantiation, a new method is created with the
	/// generic parameter substituted with the corresponding type.
	/// </summary>
	public class GenericsResolverStage : IAssemblyCompilerStage, IMethodCompilerBuilder, IPipelineStage
	{
		void IAssemblyCompilerStage.Run (AssemblyCompiler compiler)
		{
			ReadOnlyRuntimeTypeListView types = RuntimeBase.Instance.TypeLoader.GetTypesFromModule (compiler.Assembly);
			foreach (RuntimeType type in types)
			{
				foreach (RuntimeMethod method in type.Methods)
				{
					if (!HasGenericParameters(method))
						continue;
				}
			}
		}

		IEnumerable<MethodCompilerBase> IMethodCompilerBuilder.Scheduled
		{
			get
			{
				return null;
			}
		}

		string IPipelineStage.Name
		{
			get
			{
				return @"Generics Resolver";
			}
		}

		private bool HasGenericParameters (RuntimeMethod method)
		{
			foreach (SigType parameter in method.Signature.Parameters)
			{
				if (IsGenericParameter(parameter))
					return true;
			}
			return false;
		}

		private bool IsGenericParameter(SigType parameter)
		{
			return parameter.Type == CilElementType.Var;
		}
	}
}
