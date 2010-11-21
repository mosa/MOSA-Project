/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platform.X86.CPUx86
{
	/// <summary>
	/// 
	/// </summary>
	public class NopInstruction : BaseInstruction
	{

		#region Methods

		/// <summary>
		/// Emits the specified CTX.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			emitter.WriteByte(0x90);
		}

		#endregion // Methods

	}
}
