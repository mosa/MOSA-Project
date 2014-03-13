/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Ki (kiootic) <kiootic@gmail.com>
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaMethod : MosaUnit, IEquatable<MosaMethod>
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

		public bool IsSpecialName { get; private set; }

		public bool IsRTSpecialName { get; private set; }

		public bool IsVirtual { get; private set; }

		public bool IsNewSlot { get; private set; }

		public bool IsFinal { get; private set; }

		public bool HasOpenGenericParams { get; private set; }

		private List<MosaType> genericArguments;

		public IList<MosaType> GenericArguments { get; private set; }

		private List<MosaLocal> localVars;
		private List<MosaInstruction> instructions;
		private List<MosaExceptionHandler> exceptionHandlers;

		public IList<MosaLocal> LocalVariables { get; private set; }

		public uint MaxStack { get; private set; }

		public IList<MosaInstruction> Code { get; private set; }

		public IList<MosaExceptionHandler> ExceptionBlocks { get; private set; }

		private List<MosaMethod> overrides;

		public IList<MosaMethod> Overrides { get; private set; }

		public string ExternMethod { get; private set; }

		internal MosaMethod()
		{
			GenericArguments = (genericArguments = new List<MosaType>()).AsReadOnly();

			LocalVariables = (localVars = new List<MosaLocal>()).AsReadOnly();
			Code = (instructions = new List<MosaInstruction>()).AsReadOnly();
			ExceptionBlocks = (exceptionHandlers = new List<MosaExceptionHandler>()).AsReadOnly();

			Overrides = (overrides = new List<MosaMethod>()).AsReadOnly();
		}

		internal MosaMethod Clone()
		{
			MosaMethod result = (MosaMethod)base.MemberwiseClone();

			result.GenericArguments = (result.genericArguments = new List<MosaType>(this.genericArguments)).AsReadOnly();

			result.LocalVariables = (result.localVars = new List<MosaLocal>(this.localVars)).AsReadOnly();
			result.Code = (result.instructions = new List<MosaInstruction>(this.instructions)).AsReadOnly();
			result.ExceptionBlocks = (result.exceptionHandlers = new List<MosaExceptionHandler>(this.exceptionHandlers)).AsReadOnly();

			result.Overrides = (result.overrides = new List<MosaMethod>(this.overrides)).AsReadOnly();

			return result;
		}

		public bool Equals(MosaMethod other)
		{
			return SignatureComparer.Equals(this.Signature, other.Signature);
		}

		public class Mutator : MosaUnit.MutatorBase
		{
			private MosaMethod method;

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

			public bool IsSpecialName { set { method.IsSpecialName = value; } }

			public bool IsRTSpecialName { set { method.IsRTSpecialName = value; } }

			public bool IsVirtual { set { method.IsVirtual = value; } }

			public bool IsNewSlot { set { method.IsNewSlot = value; } }

			public bool IsFinal { set { method.IsFinal = value; } }

			public bool HasOpenGenericParams { set { method.HasOpenGenericParams = value; } }

			public IList<MosaType> GenericArguments { get { return method.genericArguments; } }

			public IList<MosaLocal> LocalVariables { get { return method.localVars; } }

			public uint MaxStack { set { method.MaxStack = value; } }

			public IList<MosaInstruction> Code { get { return method.instructions; } }

			public IList<MosaExceptionHandler> ExceptionBlocks { get { return method.exceptionHandlers; } }

			public IList<MosaMethod> Overrides { get { return method.overrides; } }

			public string ExternMethod { set { method.ExternMethod = value; } }

			public override void Dispose()
			{
				if (method.Signature != null && method.DeclaringType != null)
				{
					StringBuilder methodName = new StringBuilder();
					methodName.Append(method.Name);
					if (GenericArguments.Count > 0)
					{
						methodName.Append("<");
						for (int i = 0; i < GenericArguments.Count; i++)
						{
							if (i != 0)
								methodName.Append(", ");
							methodName.Append(GenericArguments[i].Name);
						}
						methodName.Append(">");
					}
					method.ShortName = SignatureName.GetSignature(methodName.ToString(), method.Signature, true);
					method.FullName = SignatureName.GetSignature(method.DeclaringType.FullName + "::" + methodName.ToString(), method.Signature, false);
				}
			}
		}
	}
}