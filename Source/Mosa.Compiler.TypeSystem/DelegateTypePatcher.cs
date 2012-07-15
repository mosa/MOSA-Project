/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System.Collections.Generic;
using Mosa.Compiler.Metadata;

using Mosa.Compiler.TypeSystem.Cil;

namespace Mosa.Compiler.TypeSystem
{
	/// <summary>
	/// 
	/// </summary>
	public class DelegateTypePatcher
	{
		/// <summary>
		/// 
		/// </summary>
		private HashSet<RuntimeType> patched = new HashSet<RuntimeType>();
		/// <summary>
		/// 
		/// </summary>
		private readonly string ConstructorName = ".ctor";
		/// <summary>
		/// 
		/// </summary>
		private readonly string InvokeMethodName = "Invoke";

		/// <summary>
		/// 
		/// </summary>
		private RuntimeType delegateStub = null;

		/// <summary>
		/// 
		/// </summary>
		private ITypeLayout typeLayout;

		/// <summary>
		/// Initializes a new instance of the <see cref="DelegateTypePatcher"/> class.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="platform">The platform.</param>
		public DelegateTypePatcher(ITypeSystem typeSystem, ITypeLayout typeLayout, string platform)
		{
			this.typeLayout = typeLayout;

			delegateStub = LoadDelegateStub(typeSystem, platform);
		}

		/// <summary>
		/// Patches the type.
		/// </summary>
		/// <param name="type">The type.</param>
		public void PatchType(RuntimeType type)
		{
			if (patched.Contains(type))
				return;

			GenerateAndReplaceMethods(type);

			patched.Add(type);
		}

		/// <summary>
		/// Loads the delegate stub.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="platform">The platform.</param>
		/// <returns></returns>
		private static RuntimeType LoadDelegateStub(ITypeSystem typeSystem, string platform)
		{
			foreach (var t in typeSystem.GetAllTypes())
			{
				if (t.FullName == "Mosa.Platform." + platform + ".Intrinsic.DelegateStub")
				{
					return t;
				}
			}
			return null;
		}

		/// <summary>
		/// Generates and replace methods.
		/// </summary>
		/// <param name="type">The type.</param>
		private void GenerateAndReplaceMethods(RuntimeType type)
		{
			GenerateAndReplaceConstructor(type);

			if (type.Methods[1].Signature.ReturnType.Type == CilElementType.Void)
				GenerateAndReplaceInvokeMethod(type);
			else
				GenerateAndReplaceInvokeWithReturnMethod(type);
		}

		/// <summary>
		/// Generates and replace constructor.
		/// </summary>
		/// <param name="type">The type.</param>
		private void GenerateAndReplaceConstructor(RuntimeType type)
		{
			RuntimeParameter[] parameters = new RuntimeParameter[type.Methods[0].Parameters.Count];
			type.Methods[0].Parameters.CopyTo(parameters, 0);
			var stubConstructor = delegateStub.Methods[0];

			var constructor = new CilRuntimeMethod(delegateStub.Module, ConstructorName,
				type.Methods[0].Signature, stubConstructor.Token, type, stubConstructor.Attributes, stubConstructor.ImplAttributes, stubConstructor.Rva);

			foreach (var parameter in parameters)
				constructor.Parameters.Add(parameter);

			SearchAndReplaceMethod(type, ConstructorName, constructor);
		}

		/// <summary>
		/// Generates and replace invoke method.
		/// </summary>
		/// <param name="type">The type.</param>
		private void GenerateAndReplaceInvokeMethod(RuntimeType type)
		{
			RuntimeParameter[] parameters = new RuntimeParameter[type.Methods[1].Parameters.Count];
			type.Methods[1].Parameters.CopyTo(parameters, 0);
			var stubInvoke = delegateStub.Methods[1];

			var invokeMethod = new CilRuntimeMethod(delegateStub.Module, InvokeMethodName,
				type.Methods[1].Signature, stubInvoke.Token, type, stubInvoke.Attributes, stubInvoke.ImplAttributes, stubInvoke.Rva);
			foreach (var parameter in parameters)
				invokeMethod.Parameters.Add(parameter);

			SearchAndReplaceMethod(type, InvokeMethodName, invokeMethod);
		}

		/// <summary>
		/// Generates the and replace invoke with return method.
		/// </summary>
		/// <param name="type">The type.</param>
		private void GenerateAndReplaceInvokeWithReturnMethod(RuntimeType type)
		{
			RuntimeParameter[] parameters = new RuntimeParameter[type.Methods[1].Parameters.Count];
			type.Methods[1].Parameters.CopyTo(parameters, 0);
			var stubInvoke = delegateStub.Methods[2];

			var invokeMethod = new CilRuntimeMethod(delegateStub.Module, InvokeMethodName,
				type.Methods[1].Signature, stubInvoke.Token, type, stubInvoke.Attributes, stubInvoke.ImplAttributes, stubInvoke.Rva);
			foreach (var parameter in parameters)
				invokeMethod.Parameters.Add(parameter);

			SearchAndReplaceMethod(type, InvokeMethodName, invokeMethod);
		}
		
		/// <summary>
		/// Searches and replaces the method.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="methodName">Name of the method.</param>
		/// <param name="patchedMethod">The method to replace.</param>
		private void SearchAndReplaceMethod(RuntimeType type, string methodName, RuntimeMethod patchedMethod)
		{
			for (int i = 0; i < type.Methods.Count; ++i)
			{
				if (type.Methods[i].Name == methodName)
				{
					type.Methods[i] = patchedMethod;
					return;
				}
			}
		}
	}
}
