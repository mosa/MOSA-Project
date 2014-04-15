/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.NewLinker
{
	/// <summary>
	/// Represents a linking request to the assembly linker.
	/// </summary>
	public sealed class LinkRequest
	{
		#region Properties

		/// <summary>
		/// The type of link required
		/// </summary>
		public LinkType LinkType { get; private set; }

		/// <summary>
		/// Gets the patches.
		/// </summary>
		public PatchType PatchType { get; private set; }

		/// <summary>
		/// The object that is being patched.
		/// </summary>
		public LinkerObject PatchObject { get; private set; }

		/// <summary>
		/// Gets the patch offset.
		/// </summary>
		/// <value>
		/// The symbol offset.
		/// </value>
		public int PatchOffset { get; private set; }

		/// <summary>
		/// Determines the relative base of the link request.
		/// </summary>
		public int RelativeBase { get; private set; }

		/// <summary>
		/// Gets the reference symbol.
		/// </summary>
		/// <value>
		/// The reference symbol.
		/// </value>
		public LinkerObject ReferenceObject { get; private set; }

		/// <summary>
		/// Gets the offset to apply to the reference target.
		/// </summary>
		/// <value>The offset.</value>
		public int ReferenceOffset { get; private set; }

		#endregion Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of LinkRequest.
		/// </summary>
		/// <param name="linkType">Type of the link.</param>
		/// <param name="patchType">Type of the patch.</param>
		/// <param name="patchSymbol">The patch symbol.</param>
		/// <param name="patchOffset">The patch offset.</param>
		/// <param name="relativeBase">The relative base.</param>
		/// <param name="referenceSymbol">The reference symbol.</param>
		/// <param name="referenceOffset">The reference offset.</param>
		public LinkRequest(LinkType linkType, PatchType patchType, LinkerObject patchSymbol, int patchOffset, int relativeBase, LinkerObject referenceSymbol, int referenceOffset)
		{
			this.LinkType = linkType;
			this.PatchType = patchType;

			this.PatchObject = patchSymbol;
			this.PatchOffset = patchOffset;

			this.RelativeBase = relativeBase;
			this.ReferenceObject = referenceSymbol;
			this.ReferenceOffset = referenceOffset;
		}

		#endregion Construction

	}
}