// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class SignatureName
	{

		// For better GDB, ELF, DWARF and exported EntryPoints, we want customize the SymbolName for better usability.
		public delegate string GetSignatureDelegate(string name, MosaMethodSignature sig, bool shortSig, bool includeReturnType = true);
		public static GetSignatureDelegate GetSignatureOverride;

		public static string GetSignature(string name, MosaMethodSignature sig, bool shortSig, bool includeReturnType = true)
		{
			if (GetSignatureOverride != null)
			{
				var signature = GetSignatureOverride(name, sig, shortSig, includeReturnType);
				if (!string.IsNullOrEmpty(signature))
					return signature;
			}

			var result = new StringBuilder();
			if (shortSig)
			{
				result.Append(name);
				result.Append("(");
				for (int i = 0; i < sig.Parameters.Count; i++)
				{
					if (i != 0)
						result.Append(", ");
					result.Append(sig.Parameters[i].ParameterType.ShortName);
				}
				result.Append(")");

				if (includeReturnType)
				{
					result.Append(" : ");
					result.Append(sig.ReturnType.ShortName);
				}

				return result.ToString();
			}
			else
			{
				if (includeReturnType)
				{
					result.Append(sig.ReturnType.FullName);
					result.Append(" ");
				}

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

		internal static void UpdateType(MosaType type)
		{
			var result = new StringBuilder();

			if (type.GenericArguments?.Count > 0)
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
					result.Append(type.ElementType.Signature);
					result.Append("*");
					break;

				case MosaTypeCode.ManagedPointer:
					result.Append(type.ElementType.Signature);
					result.Append("&");
					break;

				case MosaTypeCode.SZArray:
				case MosaTypeCode.Array:
					result.Append(type.ElementType.Signature);
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
