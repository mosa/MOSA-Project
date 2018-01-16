// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class SignatureComparer : IEqualityComparer<MosaType>, IEqualityComparer<MosaMethodSignature>
	{
		public static bool Equals(MosaType x, MosaType y)
		{
			if (x.TypeCode != y.TypeCode)
				return false;

			if (x.GenericArguments.Count != y.GenericArguments.Count)
				return false;

			for (int i = 0; i < x.GenericArguments.Count; i++)
			{
				if (!Equals(x.GenericArguments[i], y.GenericArguments[i]))
					return false;
			}

			switch (x.TypeCode)
			{
				case MosaTypeCode.UnmanagedPointer:
				case MosaTypeCode.ManagedPointer:
				case MosaTypeCode.SZArray:
					return Equals(x.ElementType, y.ElementType);

				case MosaTypeCode.Array:
					return Equals(x.ElementType, y.ElementType)
						   && Equals(x.ArrayInfo, y.ArrayInfo);

				case MosaTypeCode.FunctionPointer:
					return Equals(x.FunctionPtrSig, y.FunctionPtrSig);

				default:
					return x.Namespace == y.Namespace && x.Name == y.Name;
			}
		}

		bool IEqualityComparer<MosaType>.Equals(MosaType x, MosaType y)
		{
			return Equals(x, y);
		}

		public static int GetHashCode(MosaType type)
		{
			int result = (int)type.TypeCode * 7;

			foreach (var genericArg in type.GenericArguments)
			{
				result += (result * 7) + GetHashCode(genericArg);
			}

			switch (type.TypeCode)
			{
				case MosaTypeCode.UnmanagedPointer:
				case MosaTypeCode.ManagedPointer:
				case MosaTypeCode.SZArray:

				// Hash code DOES not need to be unique, so to save time ArrayInfo is not hashed
				case MosaTypeCode.Array:
					result += (result * 7) + GetHashCode(type.ElementType);
					break;

				case MosaTypeCode.FunctionPointer:
					result += GetHashCode(type.FunctionPtrSig);
					break;

				default:
					result += (result * 7) + type.Name.GetHashCode() + type.Namespace.GetHashCode();
					break;
			}
			return result;
		}

		int IEqualityComparer<MosaType>.GetHashCode(MosaType obj)
		{
			return GetHashCode(obj);
		}

		public static bool Equals(MosaMethodSignature x, MosaMethodSignature y)
		{
			return Equals(x.ReturnType, y.ReturnType)
				   && x.Parameters.SequenceEquals(y.Parameters);
		}

		bool IEqualityComparer<MosaMethodSignature>.Equals(MosaMethodSignature x, MosaMethodSignature y)
		{
			return Equals(x, y);
		}

		public static int GetHashCode(MosaMethodSignature method)
		{
			int result = GetHashCode(method.ReturnType);

			foreach (var param in method.Parameters)
			{
				result += (result * 7) + GetHashCode(param.ParameterType);
			}
			return result;
		}

		int IEqualityComparer<MosaMethodSignature>.GetHashCode(MosaMethodSignature obj)
		{
			return GetHashCode(obj);
		}
	}
}
