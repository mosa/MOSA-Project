/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;

using Mosa.Runtime;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Vm;

namespace Mosa.Jit.SimpleJit
{
	/// <summary>
	/// Provides a basic jit compiler that runs without many optimizations.
	/// </summary>
	public sealed class SimpleJitService : IJitService
	{
		#region Data members

		/// <summary>
		/// The current trampoline pool ptr.
		/// </summary>
		private IntPtr _trampolineStorage;

		/// <summary>
		/// Number of bytes remaining in the trampoline pool.
		/// </summary>
		private Stream _stream;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// 
		/// </summary>
		public SimpleJitService()
		{
			_trampolineStorage = IntPtr.Zero;
			_stream = RawMemoryStream.Null;
		}

		#endregion // Construction

		#region IJitService Members

		void IJitService.SetupJit(RuntimeMethod method)
		{
			// Check preconditions
			if (null == method)
				throw new ArgumentNullException(@"method");
			Debug.Assert(MethodImplAttributes.IL == (method.ImplAttributes & MethodImplAttributes.IL), @"Non-IL method passed to IJitService.SetupJit");
			if (MethodImplAttributes.IL != (method.ImplAttributes & MethodImplAttributes.IL))
				throw new ArgumentException(@"Non-IL method passed to IJitService.SetupJit.", @"method");

			// Code the appropriate trampoline
			/*
						if (method.DeclaringType.IsGeneric) {
							FIXME: method.DeclaringType is always null right now
			 *              the loader doesn't initialize these properly.
						}
			 */
			if (method.IsGeneric)
			{
				// Emit a generic call trampoline
				method.Address = EmitGenericMethodTrampoline();
			}
			else
			{
				// A normal call trampoline
				method.Address = EmitStandardTrampoline(method);
			}
		}

		#endregion // IJitService Members

		#region Internals

		private IntPtr EmitGenericMethodTrampoline()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// This function emits a standard trampoline.
		/// </summary>
		/// <returns>A pointer to the standard trampoline emitted.</returns>
		private IntPtr EmitStandardTrampoline(RuntimeMethod method)
		{
			return IntPtr.Zero;
		}

		#endregion // Internals
	}
}
