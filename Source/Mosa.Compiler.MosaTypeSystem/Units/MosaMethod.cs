// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Compiler.MosaTypeSystem
{
	public sealed class MosaMethod : MosaUnit, IEquatable<MosaMethod>
	{
		public MosaModule Module { get; private set; }

		public MosaType DeclaringType { get; private set; }

		public MosaMethodSignature Signature { get; private set; }

		public bool IsAbstract { get; private set; }

		public bool IsStatic { get; private set; }

		public bool HasThis { get; private set; }

		public bool HasExplicitThis { get; private set; }

		public bool IsInternal { get; private set; }

		public bool IsNoInlining { get; private set; }

		public bool IsAggressiveInlining { get; private set; }

		public bool IsSpecialName { get; private set; }

		public bool IsRTSpecialName { get; private set; }

		public bool IsVirtual { get; private set; }

		public bool IsNewSlot { get; private set; }

		public bool IsFinal { get; private set; }

		public bool HasOpenGenericParams { get; private set; }

		public bool HasImplementation { get { return Code.Count != 0; } }

		private List<MosaType> genericArguments;

		public IList<MosaType> GenericArguments { get; private set; }

		private List<MosaLocal> localVars;
		private List<MosaInstruction> instructions;
		private List<MosaExceptionHandler> exceptionHandlers;

		public MosaMethodAttributes MethodAttributes { get; private set; }

		public IList<MosaLocal> LocalVariables { get; private set; }

		public uint MaxStack { get; private set; }

		public IList<MosaInstruction> Code { get; private set; }

		public IList<MosaExceptionHandler> ExceptionHandlers { get; private set; }

		private List<MosaMethod> overrides;

		public IList<MosaMethod> Overrides { get; private set; }

		public bool IsExternal { get; private set; }

		public string ExternMethodName { get; private set; }

		public string ExternMethodModule { get; private set; }

		public bool IsConstructor { get { return IsSpecialName && IsRTSpecialName && Name == ".ctor"; } }

		public bool IsTypeConstructor { get { return IsSpecialName && IsRTSpecialName && IsStatic && Name == ".cctor"; } }

		internal MosaMethod()
		{
			GenericArguments = (genericArguments = new List<MosaType>()).AsReadOnly();

			LocalVariables = (localVars = new List<MosaLocal>()).AsReadOnly();
			Code = (instructions = new List<MosaInstruction>()).AsReadOnly();
			ExceptionHandlers = (exceptionHandlers = new List<MosaExceptionHandler>()).AsReadOnly();

			Overrides = (overrides = new List<MosaMethod>()).AsReadOnly();
		}

		internal MosaMethod Clone()
		{
			var result = (MosaMethod)base.MemberwiseClone();

			result.GenericArguments = (result.genericArguments = new List<MosaType>(genericArguments)).AsReadOnly();

			result.LocalVariables = (result.localVars = new List<MosaLocal>(localVars)).AsReadOnly();
			result.Code = (result.instructions = new List<MosaInstruction>(instructions)).AsReadOnly();
			result.ExceptionHandlers = (result.exceptionHandlers = new List<MosaExceptionHandler>(exceptionHandlers)).AsReadOnly();

			result.Overrides = (result.overrides = new List<MosaMethod>(overrides)).AsReadOnly();

			return result;
		}

		public bool Equals(MosaMethod other)
		{
			return SignatureComparer.Equals(Signature, other.Signature);

			//return SignatureEquals(other) && this.DeclaringType.FullName == other.DeclaringType.FullName && this.Name == other.Name;
		}

		public class Mutator : MosaUnit.MutatorBase
		{
			private readonly MosaMethod method;

			internal Mutator(MosaMethod method)
				: base(method)
			{
				this.method = method;
			}

			public MosaModule Module { set { method.Module = value; } }

			public MosaType DeclaringType { set { method.DeclaringType = value; } }

			public MosaMethodSignature Signature { set { method.Signature = value; } }

			public bool IsAbstract { set { method.IsAbstract = value; } }

			public bool IsStatic { set { method.IsStatic = value; } }

			public bool HasThis { set { method.HasThis = value; } }

			public bool HasExplicitThis { set { method.HasExplicitThis = value; } }

			public bool IsInternalCall { set { method.IsInternal = value; } }

			public bool IsNoInlining { set { method.IsNoInlining = value; } }

			public bool IsAggressiveInlining { set { method.IsAggressiveInlining = value; } }

			public bool IsSpecialName { set { method.IsSpecialName = value; } }

			public bool IsRTSpecialName { set { method.IsRTSpecialName = value; } }

			public bool IsVirtual { set { method.IsVirtual = value; } }

			public bool IsNewSlot { set { method.IsNewSlot = value; } }

			public bool IsFinal { set { method.IsFinal = value; } }

			public bool HasOpenGenericParams { set { method.HasOpenGenericParams = value; } }

			public IList<MosaType> GenericArguments { get { return method.genericArguments; } }

			public MosaMethodAttributes MethodAttributes { set { method.MethodAttributes = value; } }

			public IList<MosaLocal> LocalVariables { get { return method.localVars; } }

			public uint MaxStack { set { method.MaxStack = value; } }

			public List<MosaInstruction> Code { get { return method.instructions; } }

			public IList<MosaExceptionHandler> ExceptionBlocks { get { return method.exceptionHandlers; } }

			public IList<MosaMethod> Overrides { get { return method.overrides; } }

			public bool IsExternal { set { method.IsExternal = value; } }

			public string ExternMethodName { set { method.ExternMethodName = value; } }

			public string ExternMethodModule { set { method.ExternMethodModule = value; } }

			public override void Dispose()
			{
				if (method.Signature != null && method.DeclaringType != null)
				{
					var methodName = new StringBuilder();
					methodName.Append(method.Name);
					if (GenericArguments.Count > 0)
					{
						methodName.Append("<");
						for (int i = 0; i < GenericArguments.Count; i++)
						{
							if (i != 0)
								methodName.Append(", ");
							methodName.Append(GenericArguments[i].FullName);
						}
						methodName.Append(">");
					}

					method.ShortName = SignatureName.GetSignature(methodName.ToString(), method.Signature, true, true);
					method.FullName = SignatureName.GetSignature(method.DeclaringType.FullName + "::" + methodName, method.Signature, false, true);
				}
			}
		}
	}

	public class MosaMethodFullNameComparer : IEqualityComparer<MosaMethod>
	{
		public bool Equals(MosaMethod x, MosaMethod y)
		{
			return x.FullName.Equals(y.FullName);
		}

		public int GetHashCode(MosaMethod obj)
		{
			return obj.FullName.GetHashCode();
		}
	}
}
