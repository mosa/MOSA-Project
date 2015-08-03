// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;

namespace Mosa.Compiler.MosaTypeSystem
{
	internal class SignatureName
	{
		public static string GetSignature(string name, MosaMethodSignature sig, bool shortSig)
		{
			StringBuilder result = new StringBuilder();
			if (shortSig)
			{
				result.Append(name);
				result.Append("(");
				for (int i = 0; i < sig.Parameters.Count; i++)
				{
					if (i != 0)
						result.Append(", ");
					result.Append(sig.Parameters[i].ParameterType.Name);
				}
				result.Append(") : ");
				result.Append(sig.ReturnType.Name);
				return result.ToString();
			}
			else
			{
				result.Append(sig.ReturnType.FullName);
				result.Append(" ");
				result.Append(name);
				result.Append("(");
				for (int i = 0; i < sig.Parameters.Count; i++)
				{
					if (i != 0)
						result.Append(", ");
					result.Append(sig.Parameters[i].ParameterType.FullName);
				}
				result.Append(")");
				return result.ToString();
			}
		}

		public static void UpdateType(MosaType type)
		{
			StringBuilder result = new StringBuilder();
			if (type.ElementType != null)
			{
				result.Append(type.ElementType.Signature);
			}
			if (type.GenericArguments.Count > 0)
			{
				result.Append("<");
				for (int i = 0; i < type.GenericArguments.Count; i++)
				{
					if (i != 0)
						result.Append(", ");
					result.Append(type.GenericArguments[i].FullName);
				}
				result.Append(">");
			}

			switch (type.TypeCode)
			{
				case MosaTypeCode.UnmanagedPointer:
					result.Append("*");
					break;

				case MosaTypeCode.ManagedPointer:
					result.Append("&");
					break;

				case MosaTypeCode.SZArray:
				case MosaTypeCode.Array:
					result.Append(type.ArrayInfo.ToString());
					break;

				case MosaTypeCode.FunctionPointer:
					result.Append(type.FunctionPtrSig.ToString());
					break;

				default:
					break;
			}

			if (type.Modifier != null)
			{
				result.Append(" mod(");
				result.Append(type.Modifier.Name);
				result.Append(")");
			}
			type.Signature = result.ToString();
		}
	}
}