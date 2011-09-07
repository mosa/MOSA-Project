/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Metadata;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public class MethodHeader
	{
		/// <summary>
		/// Header flags 
		/// </summary>
		public MethodFlags Flags;
		/// <summary>
		/// Maximum stack size 
		/// </summary>
		public ushort MaxStack;
		/// <summary>
		/// Size of the code in bytes 
		/// </summary>
		public uint CodeSize;
		/// <summary>
		/// Local variable signature token 
		/// </summary>
		public Token LocalsSignature;
	}
}
