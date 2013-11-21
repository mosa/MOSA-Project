using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaType
	{
		public string Name { get; internal set; }

		public string FullName { get; internal set; }

		public string Namespace { get; internal set; }

		public MosaType MosaBaseType { get; internal set; }

		public bool IsValueType { get; internal set; }

		public bool IsDelegate { get; internal set; }

		public bool IsEnum { get; internal set; }

		public bool IsInterface { get; internal set; }

		public bool IsExplicitLayoutRequestedByType { get; internal set; }

		public IList<MosaMethod> Methods;
		public IList<MosaField> Fields;
		public IList<MosaType> Interfaces;

		// enclosing type, if the current type is a nested type; or the generic type definition
		public MosaType DeclaringType { get; internal set; }

		public bool IsUnsignedByte { get; internal set; }

		public bool IsSignedByte { get; internal set; }

		public bool IsUnsignedShort { get; internal set; }

		public bool IsSignedShort { get; internal set; }

		public bool IsUnsignedInt { get; internal set; }

		public bool IsSignedInt { get; internal set; }

		public bool IsUnsignedLong { get; internal set; }

		public bool IsSignedLong { get; internal set; }

		public bool IsByte { get { return IsUnsignedByte || IsSignedByte; } }

		public bool IsShort { get { return IsUnsignedShort || IsSignedShort; } }

		public bool IsChar { get; internal set; }

		public bool IsInt { get { return IsUnsignedInt || IsSignedInt; } }

		public bool IsLong { get { return IsUnsignedLong || IsSignedLong; } }

		public bool IsBoolean { get; internal set; }

		public bool IsPointer { get; internal set; }

		public bool IsObject { get; internal set; }

		public bool IsFloatingPoint { get { return IsDouble || IsSingle; } }

		public bool IsDouble { get; internal set; }

		public bool IsSingle { get; internal set; }

		public bool IsInteger { get; internal set; }

		public bool IsSigned { get; internal set; }

		public bool IsUnsigned { get; internal set; }
	}
}