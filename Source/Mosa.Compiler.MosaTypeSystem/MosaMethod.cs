/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using System.Text;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaMethod
	{
		public string Name { get; internal set; }

		public string FullName { get; internal set; }

		public string MethodName { get; internal set; }

		public string ShortMethodName { get; internal set; }

		public MosaType DeclaringType { get; internal set; }

		public bool IsAbstract { get; internal set; }

		public bool IsGeneric { get; internal set; }

		public bool IsStatic { get; internal set; }

		public bool HasThis { get; internal set; }

		public bool HasExplicitThis { get; internal set; }

		public MosaType ReturnType { get; internal set; }

		public IList<MosaParameter> Parameters { get; internal set; }

		public IList<MosaAttribute> CustomAttributes { get; internal set; }

		public bool IsInternal { get; internal set; }

		public bool IsNoInlining { get; internal set; }

		public bool IsSpecialName { get; internal set; }

		public bool IsRTSpecialName { get; internal set; }

		public bool IsVirtual { get; internal set; }

		public bool IsPInvokeImpl { get; internal set; }

		public bool IsNewSlot { get; internal set; }

		public bool IsFinal { get; internal set; }

		public uint Rva { get; internal set; }

		public IList<MosaGenericParameter> GenericParameters { get; internal set; }

		public List<MosaType> GenericParameterTypes { get; internal set; }

		public List<MosaType> LocalVariables { get; internal set; }

		public byte[] Code { get; internal set; }

		public MosaAssembly CodeAssembly { get; internal set; }

		public List<ExceptionBlock> ExceptionBlocks { get; internal set; }

		public string ExternalReference { get; internal set; }

		public bool HasCode { get { return Rva != 0; } }

		public bool IsCILGenerated { get; internal set; }

		public bool IsOpenGeneric { get; internal set; }

		public MosaMethod()
		{
			IsAbstract = false;
			IsGeneric = false;
			IsStatic = false;
			HasThis = false;
			HasExplicitThis = false;
			IsInternal = false;
			IsNoInlining = false;
			IsSpecialName = false;
			IsRTSpecialName = false;
			IsVirtual = false;
			IsPInvokeImpl = false;
			IsNewSlot = false;
			IsFinal = false;
			IsCILGenerated = false;
			IsOpenGeneric = false;

			Parameters = new List<MosaParameter>();
			GenericParameters = new List<MosaGenericParameter>();
			CustomAttributes = new List<MosaAttribute>();
			LocalVariables = new List<MosaType>();
			GenericParameterTypes = new List<MosaType>();
			ExceptionBlocks = new List<ExceptionBlock>();
		}

		public override string ToString()
		{
			return MethodName ?? FullName;
		}

		internal string GetParameterNames()
		{
			var sb = new StringBuilder();

			sb.Append('(');

			for (int i = 0; i < Parameters.Count; i++)
			{
				sb.AppendFormat("{0} {1}, ", Parameters[i].Type.FullName, Parameters[i].Name);
			}

			if (Parameters.Count != 0)
				sb.Length = sb.Length - 2;

			sb.Append(')');

			return sb.ToString();
		}

		internal void SetName()
		{
			FullName = DeclaringType.FullName + "." + Name;

			string parameterNames = GetParameterNames();

			MethodName = FullName + parameterNames;

			ShortMethodName = ReturnType.Name + " " + Name + parameterNames;
		}

		private static bool IsOpenGenericType(MosaType type)
		{
			if (type.IsVarFlag || type.IsMVarFlag)
				return true;

			if (!type.HasElement)
				return false;

			return IsOpenGenericType(type.ElementType);
		}

		internal void SetOpenGeneric()
		{
			IsOpenGeneric = IsOpenGenericType(ReturnType);

			if (IsOpenGeneric)
				return;

			foreach (var param in Parameters)
			{
				IsOpenGeneric = IsOpenGenericType(param.Type);

				if (IsOpenGeneric)
					return;
			}

			IsOpenGeneric = false;
		}

		public bool Matches(MosaMethod method)
		{
			if (Parameters.Count != method.Parameters.Count)
				return false;

			for (int index = 0; index < Parameters.Count; index++)
			{
				if (!Parameters[index].Matches(method.Parameters[index]))
					return false;
			}

			if (ReturnType != method.ReturnType)
				return false;

			return true;
		}

		public bool Matches(List<MosaType> parameterTypes)
		{
			if (Parameters.Count != parameterTypes.Count)
				return false;

			for (int index = 0; index < Parameters.Count; index++)
			{
				if (!Parameters[index].Matches(parameterTypes[index]))
					return false;
			}

			return true;
		}
	}
}