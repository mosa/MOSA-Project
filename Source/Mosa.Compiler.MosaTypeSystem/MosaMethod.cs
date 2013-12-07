using System.Collections.Generic;
using System.Text;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaMethod
	{
		public string Name { get; internal set; }

		public string FullName { get; internal set; }

		public string MethodName { get; internal set; }

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

			Parameters = new List<MosaParameter>();
			GenericParameters = new List<MosaGenericParameter>();
			CustomAttributes = new List<MosaAttribute>();
		}

		public override string ToString()
		{
			return MethodName ?? FullName;
		}

		public void SetName()
		{
			FullName = DeclaringType.FullName + "." + Name;

			var sb = new StringBuilder();

			sb.Append(ReturnType.Name);
			sb.Append(' ');
			sb.Append(FullName);
			sb.Append('(');

			for (int i = 0; i < Parameters.Count; i++)
			{
				sb.AppendFormat("{0} {1}, ", Parameters[i].Type.FullName, Parameters[i].Name);
			}

			if (Parameters.Count != 0)
				sb.Length = sb.Length - 2;

			sb.Append(')');

			MethodName = sb.ToString();
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

		public MosaMethod Clone(MosaType declaringType)
		{
			var cloneMethod = Clone();
			cloneMethod.DeclaringType = declaringType;

			if (this.ReturnType.IsVarFlag || this.ReturnType.IsMVarFlag)
			{
				cloneMethod.ReturnType = declaringType.GenericTypes[this.ReturnType.VarOrMVarIndex];
			}

			for (int i = 0; i < Parameters.Count; i++)
			{
				var paramater = Parameters[i];

				if (paramater.Type.IsVarFlag || paramater.Type.IsMVarFlag)
				{
					var cloneParam = paramater.Clone();

					cloneParam.Type = declaringType.GenericTypes[paramater.Type.VarOrMVarIndex];

					cloneMethod.Parameters[i] = cloneParam;
				}
			}

			cloneMethod.SetName();

			return cloneMethod;
		}

		private MosaMethod Clone()
		{
			MosaMethod cloneMethod = new MosaMethod();
			cloneMethod.Name = this.Name;
			cloneMethod.MethodName = this.MethodName;
			cloneMethod.DeclaringType = this.DeclaringType;
			cloneMethod.IsAbstract = this.IsAbstract;
			cloneMethod.IsGeneric = this.IsGeneric;
			cloneMethod.IsStatic = this.IsStatic;
			cloneMethod.HasThis = this.HasThis;
			cloneMethod.HasExplicitThis = this.HasExplicitThis;
			cloneMethod.ReturnType = this.ReturnType;
			cloneMethod.IsInternal = this.IsInternal;
			cloneMethod.IsNoInlining = this.IsNoInlining;
			cloneMethod.IsSpecialName = this.IsSpecialName;
			cloneMethod.IsRTSpecialName = this.IsRTSpecialName;
			cloneMethod.IsVirtual = this.IsVirtual;
			cloneMethod.IsPInvokeImpl = this.IsPInvokeImpl;
			cloneMethod.IsNewSlot = this.IsNewSlot;
			cloneMethod.IsFinal = this.IsFinal;
			cloneMethod.Rva = this.Rva;

			foreach (var p in Parameters)
			{
				cloneMethod.Parameters.Add(p);
			}

			foreach (var a in CustomAttributes)
			{
				cloneMethod.CustomAttributes.Add(a);
			}

			return cloneMethod;
		}
	}
}