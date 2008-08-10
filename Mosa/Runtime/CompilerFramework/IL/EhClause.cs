/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mosa.Runtime.CompilerFramework.IL
{
    public enum EhClauseType : byte
    {
        /// <summary>
        /// A typed exception handler clause.
        /// </summary>
        Exception = 0,

        /// <summary>
        /// An exception filter and handler clause.
        /// </summary>
        Filter = 1,

        /// <summary>
        /// A finally clause.
        /// </summary>
        Finally = 2,

        /// <summary>
        /// A fault clause. This is similar to finally, except its only executed if an exception is/was processed.
        /// </summary>
        Fault = 4
    }

    public struct EhClause
    {
        public EhClauseType Kind;
        public int TryOffset;
        public int TryLength;
        public int HandlerOffset;
        public int HandlerLength;
        public int ClassToken;
        public int FilterOffset;

        public void Read(BinaryReader reader, bool isFat)
        {
            if (false == isFat)
            {
                this.Kind = (EhClauseType)reader.ReadInt16();
                this.TryOffset = reader.ReadInt16();
                this.TryLength = reader.ReadByte();
                this.HandlerOffset = reader.ReadInt16();
                this.HandlerLength = reader.ReadByte();
            }
            else
            {
                this.Kind = (EhClauseType)reader.ReadInt32();
                this.TryOffset = reader.ReadInt32();
                this.TryLength = reader.ReadInt32();
                this.HandlerOffset = reader.ReadInt32();
                this.HandlerLength = reader.ReadInt32();
            }

            if (EhClauseType.Exception == this.Kind)
            {
                this.ClassToken = reader.ReadInt32();
            }
            else if (EhClauseType.Filter == this.Kind)
            {
                this.FilterOffset = reader.ReadInt32();
            }
        }
    }
}
