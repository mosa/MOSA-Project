/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

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

		/// <summary>
		/// Holds the signature of the method.
		/// </summary>
		private MethodSignature signature;

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

		/// <summary>
		/// Retrieves the signature of the method.
		/// </summary>
		/// <value>The signature.</value>
		public MethodSignature Signature
		{
			get { return signature; }
			protected set { signature = value; }
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
						var sig = this.Signature;
						int i = 0;
						foreach (var p in this.Parameters)
						{
							sb.AppendFormat("{0} {1},", sig.Parameters[i++].Type, p.Name);
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

	}
}
