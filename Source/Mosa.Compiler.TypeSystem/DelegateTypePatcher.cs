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
		private HashSet<RuntimeType> alreadyPatched = new HashSet<RuntimeType>();
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
		private readonly string BeginInvokeMethodName = "BeginInvoke";
		/// <summary>
		/// 
		/// </summary>
		private readonly string EndInvokeMethodName = "EndInvoke";

		/// <summary>
		/// 
		/// </summary>
		private readonly string InstanceFieldName = "instance";
		/// <summary>
		/// 
		/// </summary>
		private readonly string MethodPtrFieldName = "methodPtr";
		/// <summary>
		/// 
		/// </summary>
		private ITypeSystem typeSystem = null;
		/// <summary>
		/// 
		/// </summary>
		private RuntimeType delegateStub = null;

		private string platform;

		/// <summary>
		/// Initializes a new instance of the <see cref="DelegateTypePatcher"/> class.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		public DelegateTypePatcher(ITypeSystem typeSystem, string platform)
		{
			this.typeSystem = typeSystem;
			this.platform = platform;
		}

		/// <summary>
		/// Patches the type.
		/// </summary>
		/// <param name="type">The type.</param>
		public void PatchType(RuntimeType type)
		{
			if (alreadyPatched.Contains(type))
				return;

			if (delegateStub == null)
				LoadDelegateStub();

			GenerateAndInsertFields(type);
			GenerateAndReplaceMethods(type);

			alreadyPatched.Add(type);
		}

		/// <summary>
		/// Loads the delegate stub.
		/// </summary>
		private void LoadDelegateStub()
		{

			foreach (var t in typeSystem.GetAllTypes())
			{
				// FIXME: This is not platform independent
				if (t.FullName == "Mosa.Platform." + platform + ".Intrinsic.DelegateStub")
				{
					delegateStub = t;
					return;
				}
			}
		}

		/// <summary>
		/// Generates and inserts fields.
		/// </summary>
		/// <param name="type">The type.</param>
		private void GenerateAndInsertFields(RuntimeType type)
		{
			GenerateAndInsertInstanceField(type);
			GenerateAndInsertMethodPtrField(type);
		}

		/// <summary>
		/// Generates and inserts the instance field.
		/// </summary>
		/// <param name="type">The type.</param>
		private void GenerateAndInsertInstanceField(RuntimeType type)
		{
			var stubObjectField = delegateStub.Fields[0];
			var objectField = new CilRuntimeField(type.Module, InstanceFieldName,
				stubObjectField.Signature, stubObjectField.Token, 0, 0, type, stubObjectField.Attributes);
			type.Fields.Add(objectField);
		}

		/// <summary>
		/// Generates and inserts the method pointer field.
		/// </summary>
		/// <param name="type">The type.</param>
		private void GenerateAndInsertMethodPtrField(RuntimeType type)
		{
			var stubObjectField = delegateStub.Fields[1];
			var objectField = new CilRuntimeField(type.Module, MethodPtrFieldName,
				stubObjectField.Signature, stubObjectField.Token, 0, 0, type, stubObjectField.Attributes);
			type.Fields.Add(objectField);
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

			GenerateAndReplaceBeginInvokeMethod(type);
			GenerateAndReplaceEndInvokeMethod(type);
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
		/// Generates the and replace begin invoke method.
		/// </summary>
		/// <param name="type">The type.</param>
		private void GenerateAndReplaceBeginInvokeMethod(RuntimeType type)
		{
			RuntimeParameter[] parameters = new RuntimeParameter[type.Methods[2].Parameters.Count];
			type.Methods[2].Parameters.CopyTo(parameters, 0);
			var stubMethod = delegateStub.Methods[2];

			var method = new CilRuntimeMethod(delegateStub.Module, BeginInvokeMethodName,
				type.Methods[2].Signature, stubMethod.Token, type, stubMethod.Attributes, stubMethod.ImplAttributes, stubMethod.Rva);

			foreach (var parameter in parameters)
				method.Parameters.Add(parameter);

			SearchAndReplaceMethod(type, BeginInvokeMethodName, method);
		}

		/// <summary>
		/// Generates the and replace end invoke method.
		/// </summary>
		/// <param name="type">The type.</param>
		private void GenerateAndReplaceEndInvokeMethod(RuntimeType type)
		{
			RuntimeParameter[] parameters = new RuntimeParameter[type.Methods[3].Parameters.Count];
			type.Methods[3].Parameters.CopyTo(parameters, 0);
			var stubMethod = delegateStub.Methods[3];

			var method = new CilRuntimeMethod(delegateStub.Module, EndInvokeMethodName,
				type.Methods[3].Signature, stubMethod.Token, type, stubMethod.Attributes, stubMethod.ImplAttributes, stubMethod.Rva);

			foreach (var parameter in parameters)
				method.Parameters.Add(parameter);

			SearchAndReplaceMethod(type, EndInvokeMethodName, method);
		}

		/// <summary>
		/// Searches and replaces the method.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="methodName">Name of the method.</param>
		/// <param name="methodToReplace">The method to replace.</param>
		private void SearchAndReplaceMethod(RuntimeType type, string methodName, RuntimeMethod methodToReplace)
		{
			for (int i = 0; i < type.Methods.Count; ++i)
				if (type.Methods[i].Name == methodName)
					type.Methods[i] = methodToReplace;
		}
	}
}
