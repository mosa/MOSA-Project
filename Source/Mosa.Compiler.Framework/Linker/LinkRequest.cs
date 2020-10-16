// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.Linker
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
		public LinkType LinkType { get; }

		/// <summary>
		/// Gets the patches.
		/// </summary>
		public PatchType PatchType { get; }

		/// <summary>
		/// The object that is being patched.
		/// </summary>
		public LinkerSymbol PatchSymbol { get; }

		/// <summary>
		/// Gets the patch offset.
		/// </summary>
		public int PatchOffset { get; }

		/// <summary>
		/// Gets the reference symbol.
		/// </summary>
		public LinkerSymbol ReferenceSymbol { get; }

		/// <summary>
		/// Gets the offset to apply to the reference target.
		/// </summary>
		public int ReferenceOffset { get; }

		#endregion Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of LinkRequest.
		/// </summary>
		/// <param name="linkType">Type of the link.</param>
		/// <param name="patchType">Type of the patch.</param>
		/// <param name="patchSymbol">The patch symbol.</param>
		/// <param name="patchOffset">The patch offset.</param>
		/// <param name="referenceSymbol">The reference symbol.</param>
		/// <param name="referenceOffset">The reference offset.</param>
		///
		public LinkRequest(LinkType linkType, PatchType patchType, LinkerSymbol patchSymbol, int patchOffset, LinkerSymbol referenceSymbol, int referenceOffset)
		{
			Debug.Assert(patchSymbol != null);
			Debug.Assert(referenceSymbol != null);

			LinkType = linkType;
			PatchType = patchType;

			PatchSymbol = patchSymbol;
			PatchOffset = patchOffset;

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
