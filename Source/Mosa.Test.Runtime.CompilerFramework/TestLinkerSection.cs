/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 */

using System;
using System.IO;

using Mosa.Runtime;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Memory;

namespace Mosa.Test.Runtime.CompilerFramework
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class TestLinkerSection : LinkerSection
	{
		#region Data members

		/// <summary>
		/// Holds the stream of this linker section.
		/// </summary>
		private Stream stream;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="TestLinkerSection"/> class.
		/// </summary>
		/// <param name="kind">The kind of the section.</param>
		/// <param name="name">The name.</param>
		/// <param name="address">The address.</param>
		public TestLinkerSection(SectionKind kind, string name, IntPtr address) :
			base(kind, name, address)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Allocates a stream of the specified size from the section.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="alignment">The alignment.</param>
		/// <returns></returns>
		public Stream Allocate(int size, int alignment)
		{
			Stream stream = this.stream;
			if (null == stream)
			{
				// Request 1Mb of memory
				VirtualMemoryStream vms = new VirtualMemoryStream(global::Mosa.Vm.Runtime.MemoryPageManager, 1024 * 1024);

				// Save the stream for further references
				this.stream = stream = vms;
				base.VirtualAddress = vms.Base;
			}

			if (size != 0 && size > (stream.Length - stream.Position))
				throw new OutOfMemoryException(@"Not enough space in section to allocate symbol.");

			return stream;
		}

		#endregion // Methods

		#region LinkerSection Overrides

		/// <summary>
		/// Gets the length of the section in bytes.
		/// </summary>
		/// <value>The length of the section in bytes.</value>
		public override long Length
		{
			get { return (this.stream != null ? this.stream.Length : 0); }
		}

		#endregion // LinkerSection Overrides
	}
}
