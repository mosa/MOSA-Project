/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// 
	/// </summary>
	public class BreakInstruction : BaseInstruction
	{

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context ctx, MachineCodeEmitter emitter)
		{
			emitter.WriteByte(0xCC);
		}

		#endregion // Methods

	}
}
