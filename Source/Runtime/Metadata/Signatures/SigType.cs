/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Bruce Markham (illuminus) <illuminus86@gmail.com>
 */

using System;
using System.Diagnostics;

namespace Mosa.Runtime.Metadata.Signatures
{
	/// <summary>
	/// Base class of a type signature.
	/// </summary>
	public class SigType : IEquatable<SigType>
	{
		#region Data members

		/// <summary>
		/// Holds the CIL element type of the signature type.
		/// </summary>
		private CilElementType _type;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="SigType"/> class.
		/// </summary>
		/// <param name="type">The type.</param>
		public SigType(CilElementType type)
		{
			_type = type;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <value>The type.</value>
		public CilElementType Type { get { return _type; } }

		#endregion // Properties

		#region Object Overrides

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			return _type.ToString();
		}

		#endregion // Object Overrides

		#region Methods

		/// <summary>
		/// Matches the specified other.
		/// </summary>
		/// <param name="other">The other signature type.</param>
		/// <returns>True, if the signature type matches.</returns>
		public virtual bool Matches(SigType other)
		{
			if (object.ReferenceEquals(this, other))
				return true;
			// TODO: Check to make sure a SigType matches
			if (other.Type != this.Type)
				return false;

			switch (this.Type)
			{
				case CilElementType.Void:
				case CilElementType.Boolean:
				case CilElementType.Char:
				case CilElementType.I1:
				case CilElementType.U1:
				case CilElementType.I2:
				case CilElementType.U2:
				case CilElementType.I4:
				case CilElementType.U4:
				case CilElementType.I8:
				case CilElementType.U8:
				case CilElementType.R4:
				case CilElementType.R8:
				case CilElementType.String:
				case CilElementType.Type:
				case CilElementType.I:
				case CilElementType.U:
				case CilElementType.Object:
				case CilElementType.Class:
				case CilElementType.ValueType:
					return true;

				case CilElementType.SZArray:
					return this.Equals(other);

				case CilElementType.GenericInst:
					return true; // this.Equals(other);

				default:
					throw new NotImplementedException();
			}
		}

		#endregion // Methods

		#region Static methods

		/// <summary>
		/// Compares both arrays of signature types for equality.
		/// </summary>
		/// <param name="first">The first array to compare.</param>
		/// <param name="second">The second array to compare.</param>
		/// <returns>True, if the signature types in both arrays are equal.</returns>
		public static bool Equals(SigType[] first, SigType[] second)
		{
			if (null == first)
				throw new ArgumentNullException(@"first");
			if (null == second)
				throw new ArgumentNullException(@"second");

			if (first == second)
				return true;
			if (first.Length != second.Length)
				return false;

			bool result = true;
			for (int idx = 0; result == true && idx < first.Length; idx++)
			{
				result = (first[idx].Equals(second[idx]));
			}

			return result;
		}

		/// <summary>
		/// Parses the type signature.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		public static SigType ParseTypeSignature(ISignatureContext context, SignatureReader reader)
		{
			CilElementType type = (CilElementType)reader.ReadByte();
			switch (type)
			{
				case CilElementType.Void:
					return BuiltInSigType.Void;

				case CilElementType.Boolean:
					return BuiltInSigType.Boolean;

				case CilElementType.Char:
					return BuiltInSigType.Char;

				case CilElementType.I1:
					return BuiltInSigType.SByte;

				case CilElementType.U1:
					return BuiltInSigType.Byte;

				case CilElementType.I2:
					return BuiltInSigType.Int16;

				case CilElementType.U2:
					return BuiltInSigType.UInt16;

				case CilElementType.I4:
					return BuiltInSigType.Int32;

				case CilElementType.U4:
					return BuiltInSigType.UInt32;

				case CilElementType.I8:
					return BuiltInSigType.Int64;

				case CilElementType.U8:
					return BuiltInSigType.UInt64;

				case CilElementType.R4:
					return BuiltInSigType.Single;

				case CilElementType.R8:
					return BuiltInSigType.Double;

				case CilElementType.String:
					return BuiltInSigType.String;

				case CilElementType.Object:
					return BuiltInSigType.Object;

				case CilElementType.I:
					return BuiltInSigType.IntPtr;

				case CilElementType.U:
					return BuiltInSigType.UIntPtr;

				case CilElementType.TypedByRef:
					return BuiltInSigType.TypedByRef;

				case CilElementType.Array:
					return ParseArraySignature(context, reader);

				case CilElementType.Class:
					return ParseClassSignature(context, reader);

				case CilElementType.FunctionPtr:
					return ParseFunctionPointer(context, reader);

				case CilElementType.GenericInst:
					return ParseGenericInstance(context, reader);

				case CilElementType.MVar:
					return ParseMVar(context, reader);

				case CilElementType.Ptr:
					return ParsePointer(context, reader);

				case CilElementType.SZArray:
					return ParseSZArraySignature(context, reader);

				case CilElementType.ValueType:
					return ParseValueType(context, reader);

				case CilElementType.Var:
					return ParseVar(context, reader);

				case CilElementType.ByRef:
					return ParseReference(context, reader);

				default:
					throw new NotSupportedException(@"Unsupported CIL element type: " + type);
			}

		}

		/// <summary>
		/// Parses the var.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		private static SigType ParseVar(ISignatureContext context, SignatureReader reader)
		{
			int typeVariableIndex = reader.ReadCompressedInt32();
			return context.GetGenericTypeArgument(typeVariableIndex);
		}

		/// <summary>
		/// Parses the type of the value.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		private static TypeSigType ParseValueType(ISignatureContext context, SignatureReader reader)
		{
			TokenTypes token = reader.ReadEncodedTypeDefOrRef();
			return new ValueTypeSigType(token);
		}

		/// <summary>
		/// Parses the pointer.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		private static SigType ParsePointer(ISignatureContext context, SignatureReader reader)
		{
			CustomMod[] mods = CustomMod.ParseCustomMods(reader);
			SigType type = ParseTypeSignature(context, reader);
			return new PtrSigType(mods, type);
		}

		/// <summary>
		/// Parses the reference.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		private static SigType ParseReference(ISignatureContext context, SignatureReader reader)
		{
			SigType type = ParseTypeSignature(context, reader);
			return new RefSigType(type);
		}

		/// <summary>
		/// Parses the M var.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		private static SigType ParseMVar(ISignatureContext context, SignatureReader reader)
		{
			int methodVariableIndex = reader.ReadCompressedInt32();
			return context.GetGenericMethodArgument(methodVariableIndex);
		}

		/// <summary>
		/// Parses the generic instance.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		private static SigType ParseGenericInstance(ISignatureContext context, SignatureReader reader)
		{
			TypeSigType originalType;
			CilElementType type = (CilElementType)reader.ReadByte();
			switch (type)
			{
				case CilElementType.Class:
					originalType = ParseClassSignature(context, reader);
					break;

				case CilElementType.ValueType:
					originalType = ParseValueType(context, reader);
					break;

				default:
					throw new InvalidOperationException(@"Invalid signature type.");
			}

			int genArgCount = reader.ReadCompressedInt32();
			SigType[] genArgs = new SigType[genArgCount];
			for (int i = 0; i < genArgCount; i++)
			{
				genArgs[i] = ParseTypeSignature(context, reader);
			}

			return new GenericInstSigType(originalType, genArgs);
		}

		/// <summary>
		/// Parses the function pointer.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		private static SigType ParseFunctionPointer(ISignatureContext context, SignatureReader reader)
		{
			TokenTypes token = reader.ReadEncodedToken();
			return new FnptrSigType(token);
		}

		/// <summary>
		/// Parses the class signature.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		private static TypeSigType ParseClassSignature(ISignatureContext context, SignatureReader reader)
		{
			TokenTypes token = reader.ReadEncodedTypeDefOrRef();
			return new ClassSigType(token);
		}

		/// <summary>
		/// Parses the array signature.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		private static SigType ParseArraySignature(ISignatureContext context, SignatureReader reader)
		{
			SigType elementType = ParseTypeSignature(context, reader);
			int rank, count;
			int[] sizes, lowerBounds;

			rank = reader.ReadCompressedInt32();
			count = reader.ReadCompressedInt32();
			sizes = new int[count];
			for (int i = 0; i < count; i++)
				sizes[i] = reader.ReadCompressedInt32();

			count = reader.ReadCompressedInt32();
			lowerBounds = new int[count];
			for (int i = 0; i < count; i++)
				lowerBounds[i] = reader.ReadCompressedInt32();

			return new ArraySigType(elementType, rank, sizes, lowerBounds);
		}

		/// <summary>
		/// Parses the SZ array signature.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		private static SigType ParseSZArraySignature(ISignatureContext context, SignatureReader reader)
		{
			CustomMod[] customMods = CustomMod.ParseCustomMods(reader);
			SigType elementType = ParseTypeSignature(context, reader);
			return new SZArraySigType(customMods, elementType);
		}


		#endregion // Static methods

		#region IEquatable<SigType> Members

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		public virtual bool Equals(SigType other)
		{
			return (_type == other._type);
		}

		#endregion // IEquatable<SigType> Members

		/// <summary>
		/// Expresses the signature element information in a string form differentiating it from other signature
		/// elements expressed the same way
		/// </summary>
		/// <remarks>Sub-classes should override this method completely and not call it. If the subclass is in this same library, and there is a preliminary implementation in this
		/// base of this method, then replace that with an exception thrower.</remarks>
		public virtual string ToSymbolPart()
		{
			// If it's not a subclass of SigType and it is trivial to express, do it here. 
			// Otherwise, call the virtual method _ToSymbolPart() and hope that it is overriden so that it doesn't throw a NotImplementedException
			switch (this.Type)
			{
				case CilElementType.Boolean:
					return "bool";
				case CilElementType.Char:
					return "char";
				case CilElementType.I1:
					return "sbyte";
				case CilElementType.U1:
					return "byte";
				case CilElementType.I2:
					return "short";
				case CilElementType.U2:
					return "ushort";
				case CilElementType.I4:
					return "int";
				case CilElementType.U4:
					return "uint";
				case CilElementType.I8:
					return "long";
				case CilElementType.U8:
					return "ulong";
				case CilElementType.R4:
					return "single";
				case CilElementType.R8:
					return "double";
				case CilElementType.String:
					return "string";
				case CilElementType.ValueType:
					return "valuetype";	// FIXME: HACK?
				//throw new NotImplementedException();
				case CilElementType.Class:
					throw new NotImplementedException();
				case CilElementType.Var:
					throw new NotImplementedException();
				case CilElementType.Array:
					throw new NotImplementedException();
				case CilElementType.GenericInst:
					throw new NotImplementedException();
				case CilElementType.TypedByRef:
					throw new NotImplementedException();
				case CilElementType.I:
					return "IntPtr";
				case CilElementType.U:
					return "UIntPtr";
				case CilElementType.FunctionPtr:
					throw new NotImplementedException();
				case CilElementType.Object:
					return "object";	// FIXME: HACK?
				//throw new NotImplementedException();
				case CilElementType.SZArray:
					throw new NotImplementedException();
				case CilElementType.MVar:
					throw new NotImplementedException();
				case CilElementType.Required:
					throw new NotImplementedException();
				case CilElementType.Optional:
					throw new NotImplementedException();
				case CilElementType.Internal:
					throw new NotImplementedException();
				case CilElementType.Modifier:
					throw new NotImplementedException();
				case CilElementType.Sentinel:
					throw new NotImplementedException();
				case CilElementType.Pinned:
					throw new NotImplementedException();
				case CilElementType.Type:
					throw new NotImplementedException();
				case CilElementType.BoxedObject:
					throw new NotImplementedException();
				case CilElementType.Reserved:
					throw new NotImplementedException();
				case CilElementType.Field:
					throw new NotImplementedException();
				case CilElementType.Property:
					throw new NotImplementedException();
				case CilElementType.Enum:
					throw new NotImplementedException();
				default:
					throw new NotImplementedException();
			}
		}
	}
}
