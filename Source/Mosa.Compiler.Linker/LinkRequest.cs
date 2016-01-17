// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Linker
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
		public LinkerSymbol PatchSymbol { get; private set; }

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
		public LinkerSymbol ReferenceSymbol { get; private set; }

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
		public LinkRequest(LinkType linkType, PatchType patchType, LinkerSymbol patchSymbol, int patchOffset, int relativeBase, LinkerSymbol referenceSymbol, int referenceOffset)
		{
			Debug.Assert(patchSymbol != null);
			Debug.Assert(referenceSymbol != null);
			Debug.Assert(patchType != null);

			LinkType = linkType;
			PatchType = patchType;

			PatchSymbol = patchSymbol;
			PatchOffset = patchOffset;

			RelativeBase = relativeBase;
			ReferenceSymbol = referenceSymbol;
			ReferenceOffset = referenceOffset;
		}

		#endregion Construction

		public override string ToString()
		{
			return PatchSymbol.Name + " -> " + ReferenceSymbol.Name;
		}
	}
}
