using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.TypeSystem.Cil;

namespace Mosa.Runtime.TypeSystem
{
	/// <summary>
	/// 
	/// </summary>
	public class DelegateTypePatcher
	{
		/// <summary>
		/// 
		/// </summary>
		private static readonly HashSet<RuntimeType> alreadyPatched = new HashSet<RuntimeType>();
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

		/// <summary>
		/// Initializes a new instance of the <see cref="DelegateTypePatcher"/> class.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		public DelegateTypePatcher(ITypeSystem typeSystem)
		{
			this.typeSystem = typeSystem;
		}

		/// <summary>
		/// Patches the type.
		/// </summary>
		/// <param name="type">The type.</param>
		public void PatchType(RuntimeType type)
		{
			if (alreadyPatched.Contains(type))
				return;

			if (this.delegateStub == null)
				this.LoadDelegateStub();

			this.GenerateAndInsertFields(type);
			this.GenerateAndReplaceMethods(type);
		}

		/// <summary>
		/// Loads the delegate stub.
		/// </summary>
		private void LoadDelegateStub()
		{
			var types = this.typeSystem.GetAllTypes();

			foreach (var t in types)
			{
				if (t.FullName.Contains("DelegateStub"))
				{
					this.delegateStub = t;
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
			this.GenerateAndInsertInstanceField(type);
			this.GenerateAndInsertMethodPtrField(type);
		}

		/// <summary>
		/// Generates and inserts the instance field.
		/// </summary>
		/// <param name="type">The type.</param>
		private void GenerateAndInsertInstanceField(RuntimeType type)
		{
			var stubObjectField = this.delegateStub.Fields[0];
			var objectField = new CilRuntimeField(this.typeSystem.InternalTypeModule, this.InstanceFieldName,
				stubObjectField.Signature, stubObjectField.Token, 0, stubObjectField.RVA, type, stubObjectField.Attributes);
			this.InsertField(type, objectField);
		}

		/// <summary>
		/// Generates and inserts the method pointer field.
		/// </summary>
		/// <param name="type">The type.</param>
		private void GenerateAndInsertMethodPtrField(RuntimeType type)
		{
			var stubObjectField = this.delegateStub.Fields[1];
			var objectField = new CilRuntimeField(this.typeSystem.InternalTypeModule, this.MethodPtrFieldName,
				stubObjectField.Signature, stubObjectField.Token, 0, stubObjectField.RVA, type, stubObjectField.Attributes);
			this.InsertField(type, objectField);
		}

		/// <summary>
		/// Inserts the field into the type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="field">The field.</param>
		private void InsertField(RuntimeType type, RuntimeField field)
		{
			type.Fields.Add(field);
		}

		/// <summary>
		/// Generates and replace methods.
		/// </summary>
		/// <param name="type">The type.</param>
		private void GenerateAndReplaceMethods(RuntimeType type)
		{
			this.GenerateAndReplaceConstructor(type);
			this.GenerateAndReplaceInvokeMethod(type);
		}

		/// <summary>
		/// Generates and replace constructor.
		/// </summary>
		/// <param name="type">The type.</param>
		private void GenerateAndReplaceConstructor(RuntimeType type)
		{
			RuntimeParameter[] parameters = new RuntimeParameter[type.Methods[0].Parameters.Count];
			type.Methods[0].Parameters.CopyTo(parameters, 0);
			var stubConstructor = this.delegateStub.Methods[0];

			var constructor = new CilRuntimeMethod(type.Module, this.ConstructorName,
				type.Methods[0].Signature, stubConstructor.Token, type, stubConstructor.Attributes, stubConstructor.ImplAttributes, stubConstructor.Rva);

			foreach (var parameter in parameters)
				constructor.Parameters.Add(parameter);

			this.SearchAndReplaceMethod(type, this.ConstructorName, constructor);
		}

		/// <summary>
		/// Generates and replace invoke method.
		/// </summary>
		/// <param name="type">The type.</param>
		private void GenerateAndReplaceInvokeMethod(RuntimeType type)
		{
			RuntimeParameter[] parameters = new RuntimeParameter[type.Methods[1].Parameters.Count];
			type.Methods[1].Parameters.CopyTo(parameters, 0);
			var stubInvoke = this.delegateStub.Methods[1];

			var invokeMethod = new CilRuntimeMethod(type.Module, this.InvokeMethodName,
				type.Methods[1].Signature, stubInvoke.Token, type, stubInvoke.Attributes, stubInvoke.ImplAttributes, stubInvoke.Rva);
			foreach (var parameter in parameters)
				invokeMethod.Parameters.Add(parameter);

			this.SearchAndReplaceMethod(type, this.InvokeMethodName, invokeMethod);
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
