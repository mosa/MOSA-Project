using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Mosa.Tools.Compiler.Symbols.Pdb
{
    class CvGlobalSymbolEnumerator : CvSymbolEnumerator
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
