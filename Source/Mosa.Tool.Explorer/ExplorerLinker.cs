// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Linker;
using System.IO;

namespace Mosa.Tool.Explorer
{
	internal class ExplorerLinker : BaseLinker
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ExplorerLinker"/> class.
		/// </summary>
		public ExplorerLinker()
		{
			SectionAlignment = 1;
		}

		protected override void EmitImplementation(Stream stream)
		{
		}

		#endregion Construction
	}
}
