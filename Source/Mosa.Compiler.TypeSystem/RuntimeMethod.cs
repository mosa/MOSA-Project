/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Loader;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.TypeSystem
{
	/// <summary>
	/// Base class for the runtime representation of methods.
	/// </summary>
	public abstract class RuntimeMethod : RuntimeMember
	{
		#region Data members

		/// <summary>
		/// The implementation attributes of the method.
		/// </summary>
		private MethodImplAttributes implFlags;

		/// <summary>
		/// Generic attributes of the method.
		/// </summary>
		private MethodAttributes attributes;

		private bool hasThis;

		private bool hasExplicitThis;

		private SigType returnType;

		private SigType[] sigParameters;

		/// <summary>
		/// Holds the list of parameters of the method.
		/// </summary>
		private IList<RuntimeParameter> parameters;

		/// <summary>
		/// Holds the rva of the MSIL of the method.
		/// </summary>
		private uint rva;

		/// <summary>
		/// 
		/// </summary>
		private readonly IList<GenericParameter> genericParameters;

		/// <summary>
		/// Holds the full method name
		/// </summary>
		private string fullName;

		/// <summary>
		/// Holds the method name
		/// </summary>
		private string methodName;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="RuntimeMethod"/> class.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="declaringType">The type, which declared this method.</param>
		public RuntimeMethod(ITypeModule module, Token token, RuntimeType declaringType) :
			base(module, token, declaringType)
		{
			this.genericParameters = new List<GenericParameter>();
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Retrieves the method attributes.
		/// </summary>
		/// <value>The attributes.</value>
		public MethodAttributes Attributes
		{
			get { return attributes; }
			protected set { attributes = value; }
		}

		/// <summary>
		/// Determines if the method is a generic method.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is generic; otherwise, <c>false</c>.
		/// </value>
		public bool IsGeneric
		{
			get { return this.genericParameters.Count != 0; }
		}

		/// <summary>
		/// Gets a value indicating whether this method is abstract.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is abstract; otherwise, <c>false</c>.
		/// </value>
		public bool IsAbstract
		{
			get { return (attributes & MethodAttributes.Abstract) == MethodAttributes.Abstract; }
		}

		/// <summary>
		/// Gets a value indicating whether this method is static.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is static; otherwise, <c>false</c>.
		/// </value>
		public bool IsStatic
		{
			get { return (attributes & MethodAttributes.Static) == MethodAttributes.Static; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance has code.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is native; otherwise, <c>false</c>.
		/// </value>
		public bool HasCode
		{
			get { return (Rva != 0); }
		}

		/// <summary>
		/// Gets the instruction stream.
		/// </summary>
		public InstructionStream InstructionStream
		{
			get
			{
				if (Rva == 0)
					return null;

				return new InstructionStream(Module.MetadataModule.GetInstructionStream(this.Rva), 0);
			}
		}

		/// <summary>
		/// Gets the method implementation attributes.
		/// </summary>
		/// <value>The impl attributes.</value>
		public MethodImplAttributes ImplAttributes
		{
			get { return implFlags; }
			protected set { implFlags = value; }
		}

		/// <summary>
		/// Returns the parameter definitions of this method.
		/// </summary>
		/// <value>The parameters.</value>
		public IList<RuntimeParameter> Parameters
		{
			get { return parameters; }
			protected set { parameters = value; }
		}

		public bool HasThis
		{
			get { return hasThis; }
			protected set { hasThis = value; }
		}

		public bool HasExplicitThis
		{
			get { return hasExplicitThis; }
			protected set { hasExplicitThis = value; }
		}

		public SigType ReturnType
		{
			get { return returnType; }
			protected set { returnType = value; }
		}

		public SigType[] SigParameters
		{
			get { return sigParameters; }
			protected set { sigParameters = value; }
		}

		/// <summary>
		/// Holds the RVA of the method in the binary.
		/// </summary>
		/// <value>The rva.</value>
		public uint Rva
		{
			get { return this.rva; }
			protected set { this.rva = value; }
		}

		/// <summary>
		/// Returns the interfaces implemented by this type.
		/// </summary>
		/// <value>A list of interfaces.</value>
		public IList<GenericParameter> GenericParameters
		{
			get { return genericParameters; }
		}

		/// <summary>
		/// Gets the full name.
		/// </summary>
		/// <value>The full name.</value>
		public string FullName
		{
			get
			{
				if (fullName == null)
				{
					fullName = DeclaringType.ToString() + '.' + MethodName;
				}

				return fullName;
			}
		}

		/// <summary>
		/// Gets the name of the method.
		/// </summary>
		/// <value>The name of the method.</value>
		public string MethodName
		{
			get
			{
				if (methodName == null)
				{
					var sb = new StringBuilder();

					sb.Append(Name);
					sb.Append('(');

					if (this.Parameters.Count != 0)
					{
						int i = 0;
						foreach (var p in this.Parameters)
						{
							sb.AppendFormat("{0} {1},", SigParameters[i++].Type, p.Name);
						}
						sb.Remove(sb.Length - 1, 1);
					}

					sb.Append(')');

					methodName = sb.ToString();
				}

				return methodName;
			}
		}

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
			return FullName;
		}

		#endregion // Object Overrides

		public bool Matches(RuntimeMethod other)
		{
			if (object.ReferenceEquals(this, other))
				return true;

			//if (other.GenericParameterCount != this.GenericParameterCount)
			//    return false;
			//if (other.MethodCallingConvention != this.MethodCallingConvention)
			//    return false;

			if (HasThis != other.HasThis)
				return false;
			if (HasExplicitThis != other.HasExplicitThis)
				return false;
			if (SigParameters.Length != other.SigParameters.Length)
				return false;
			if (!Matches(ReturnType, other.ReturnType))
				return false;

			return Matches(other.SigParameters);
		}

		public bool Matches(SigType[] sigTypes)
		{
			if (SigParameters.Length != sigTypes.Length)
				return false;

			for (int i = 0; i < SigParameters.Length; i++)
			{
				if (!Matches(SigParameters[i], sigTypes[i]))
					return false;
			}

			return true;
		}

		/// <summary>
		/// Matches the specified sigtypes.
		/// </summary>
		/// <param name="b">The other signature type.</param>
		/// <returns>True, if the signature type matches.</returns>
		private static bool Matches(SigType a, SigType b)
		{
			if (object.ReferenceEquals(a, b))
				return true;

			if (b.Type != a.Type)
				return false;

			switch (a.Type)
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
					return a.Equals(b);

				case CilElementType.GenericInst:
					return true; // this.Equals(other);

				default:
					throw new NotImplementedException();
			}
		}
	}
}
