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
using System.Text;

namespace Mosa.Runtime.Vm
{
	/// <summary>
	/// Used to mark a method with the <see cref="VmCall"/> to invoke.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public sealed class VmCallAttribute : Attribute
	{
		#region Data members

		/// <summary>
		/// Holds the runtime call represented by this attribute.
		/// </summary>
		private VmCall vmCall;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="VmCallAttribute"/> class.
		/// </summary>
		/// <param name="vmCall">The runtime call.</param>
		public VmCallAttribute(VmCall vmCall)
		{
			this.vmCall = vmCall;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the runtime call represented by this attribute.
		/// </summary>
		/// <value>The runtime call of this attribute.</value>
		public VmCall VmCall
		{
			get
			{
				return this.vmCall;
			}
		}

		#endregion // Properties
	}
}
