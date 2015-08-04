// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.IO;

namespace Mosa.Compiler.Pdb
{
	internal class CvGlobalSymbolEnumerator : CvSymbolEnumerator
	{
		public CvGlobalSymbolEnumerator(PdbStream stream) :
			base(stream)
		{
		}

		protected override bool IsComplete(object state)
		{
			if (state == null)
				throw new ArgumentNullException(@"state");

			Stream stream = (Stream)state;
			return (stream.Position >= stream.Length);
		}

		protected override object Prepare(BinaryReader reader)
		{
			if (reader == null)
				throw new ArgumentNullException(@"reader");

			return reader.BaseStream;
		}
	}
}
