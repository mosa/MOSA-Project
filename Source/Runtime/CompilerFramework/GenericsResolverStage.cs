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
		private AssemblyCompiler compiler;
		
		public void Setup(AssemblyCompiler compiler)
		{
			this.compiler = compiler;
		}
		
		public void Run()
		{
			ReadOnlyRuntimeTypeListView types = RuntimeBase.Instance.TypeLoader.GetTypesFromModule(this.compiler.Assembly);
			foreach (RuntimeType type in types)
			{
				foreach (RuntimeMethod method in type.Methods)
				{
					if (!HasGenericParameters(method))
						continue;

					// TODO:
					// Scan for every instantiation for this method 
					// and create new methods for every instantiation type.
					// Then add them to a HashMap Type -> Method
					// and also add them to the method list.

					// So, the method has generic parameters, so we compile it for every instantiation type
					List<TokenTypes> typeList = GetTokenTypesForMethod(compiler, type, method);
					List<RuntimeMethod> methods = RecompileMethods(compiler, typeList, method);
					ReinsertMethods(methods, method, type);
				}
			}
		}

		#region IMethodCompilerBuilder
		IEnumerable<MethodCompilerBase> IMethodCompilerBuilder.Scheduled
		{
			get
			{
				return null;
			}
		}
		#endregion

		#region IPipelineStage
		string IPipelineStage.Name
		{
			get
			{
				return @"Generics Resolver";
			}
		}
		#endregion

		/// <summary>
		/// Determines if the given method is a method 
		/// of a generic class that uses a generic parameter.
		/// </summary>
		/// <param name="method">The method to check</param>
		/// <returns>True if the method relies upon generic parameters</returns>
		public static bool HasGenericParameters (RuntimeMethod method)
		{
			// Check return type
			if (IsGenericParameter(method.Signature.ReturnType))
				return true;

			// Check parameters
			foreach (SigType parameter in method.Signature.Parameters)
			{
				if (IsGenericParameter(parameter))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Determines if the given SigType is generic
		/// </summary>
		/// <param name="parameter">The given SigType of the parameter</param>
		/// <returns>True if the parameter is generic.</returns>
		public static bool IsGenericParameter(SigType parameter)
		{
			return parameter.Type == CilElementType.Var;
		}

		private static List<TokenTypes> GetTokenTypesForMethod(AssemblyCompiler compiler, RuntimeType type, RuntimeMethod method)
		{
			throw new NotImplementedException();
		}

		private static List<RuntimeMethod> RecompileMethods(AssemblyCompiler compiler, List<TokenTypes> types, RuntimeMethod method)
		{
			throw new NotImplementedException();
		}

		private static void ReinsertMethods (List<RuntimeMethod> methods, RuntimeMethod method, RuntimeType type)
		{
			throw new NotImplementedException();
		}
	}
}
