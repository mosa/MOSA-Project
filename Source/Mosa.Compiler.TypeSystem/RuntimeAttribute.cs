/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Metadata;

namespace Mosa.Compiler.TypeSystem
{
	/// <summary>
	/// Represents an attribute in runtime type information.
	/// </summary>
	public class RuntimeAttribute
	{
		#region Data members

		/// <summary>
		/// Holds the module from which this object originated
		/// </summary>
		private readonly ITypeModule typeModule;

		/// <summary>
		/// Holds the ctor of the attribute type to invoke.
		/// </summary>
		private readonly Token ctor;

		/// <summary>
		/// Holds the ctor method of the attribute type.
		/// </summary>
		private readonly RuntimeMethod ctorMethod;

		/// <summary>
		/// Holds the blob index
		/// </summary>
		private readonly HeapIndexToken blobIndex;

		/// <summary>
		/// Holds the blob
		/// </summary>
		private byte[] blob;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="RuntimeAttribute"/> class.
		/// </summary>
		/// <param name="typeModule">The type module.</param>
		/// <param name="ctor">The ctor.</param>
		/// <param name="ctorMethod">The ctor method.</param>
		/// <param name="blobIndex">Index of the blob.</param>
		public RuntimeAttribute(ITypeModule typeModule, Token ctor, RuntimeMethod ctorMethod, HeapIndexToken blobIndex)
		{
			this.typeModule = typeModule;
			this.ctorMethod = ctorMethod;
			this.ctor = ctor;
			this.blobIndex = blobIndex;
		}

		#endregion Construction

		#region Methods

		#endregion Methods

		#region Properties

		/// <summary>
		/// Retrieves the module from which this object originated
		/// </summary>
		/// <value>The type module.</value>
		public ITypeModule TypeModule
		{
			get { return typeModule; }
		}

		/// <summary>
		/// Gets the runtime type.
		/// </summary>
		/// <value>The runtime type.</value>
		public RuntimeType Type { get { return ctorMethod.DeclaringType; } }

		/// <summary>
		/// Gets the ctor method.
		/// </summary>
		/// <value>The ctor method.</value>
		public RuntimeMethod CtorMethod { get { return ctorMethod; } }

		/// <summary>
		/// Gets the index of the blob.
		/// </summary>
		/// <value>The index of the blob.</value>
		public HeapIndexToken BlobIndex { get { return blobIndex; } }

		/// <summary>
		/// Gets the blob.
		/// </summary>
		/// <value>The blob.</value>
		public byte[] Blob
		{
			get
			{
				if (blob == null)
				{
					blob = TypeModule.MetadataModule.Metadata.ReadBlob(blobIndex);
				}

				return blob;
			}
		}

		#endregion Properties
	}
}