// ================================================================================================
// Copyright (c) MOSA Project. Licensed under the New BSD License.
// ================================================================================================
// AUTHOR       : TAYLAN INAN
// E-MAIL       : taylaninan@yahoo.com
// GITHUB       : www.github.com/taylaninan/
// ================================================================================================

using System;

namespace Mosa.Collections.Generic
{
    ///////////////////////////////////////////////////////////////////////////
    //
    // BITFLAGS
    //
    ///////////////////////////////////////////////////////////////////////////
    #region BITFLAGS...
    public class BitFlags
    {
        private const byte MaxFlags = 64;
        private byte NrOfFlags = 0;
        public ulong Flags = 0;

        public BitFlags(byte NrOfFlags)
        {
            if (NrOfFlags > MaxFlags)
            {
                this.NrOfFlags = 0;

                throw new CollectionsDataOverflowException("BitFlags.cs", "BitFlags", "Constructor", "NrOfFlags", "Number of flags can't be greater than " + MaxFlags.ToString());
            }
            else
            {
                this.NrOfFlags = NrOfFlags;
            }
        }

        public BitFlags(byte NrOfFlags, ulong InitialFlags)
        {
            if (NrOfFlags > MaxFlags)
            {
                this.NrOfFlags = 0;

                throw new CollectionsDataOverflowException("BitFlags.cs", "BitFlags", "Constructor", "NrOfFlags", "Number of flags can't be greater than " + MaxFlags.ToString());
            }
            else
            {
                this.NrOfFlags = NrOfFlags;
                this.Flags = InitialFlags;
            }
        }

        ~BitFlags()
        {
            this.NrOfFlags = 0;
            this.Flags = 0;
        }

        public bool Test(byte FlagNumber)
        {
            if (FlagNumber < this.NrOfFlags)
            {
                return (((this.Flags >> FlagNumber) & 1) > 0);
            }
            else
            {
                throw new CollectionsDataOverflowException("BitFlags.cs", "BitFlags", "Test", "FlagNumber", "FlagNumber can't be greater than " + (this.NrOfFlags - 1).ToString());
            }
        }

        public void Set(byte FlagNumber)
        {
            if (FlagNumber < this.NrOfFlags)
            {
                this.Flags = this.Flags | ((ulong)1 << FlagNumber);
            }
            else
            {
                throw new CollectionsDataOverflowException("BitFlags.cs", "BitFlags", "Set", "FlagNumber", "FlagNumber can't be greater than " + (this.NrOfFlags - 1).ToString());
            }
        }

        public void Reset(byte FlagNumber)
        {
            if (FlagNumber < this.NrOfFlags)
            {
                this.Flags = this.Flags & ~((ulong)1 << FlagNumber);
            }
            else
            {
                throw new CollectionsDataOverflowException("BitFlags.cs", "BitFlags", "Reset", "FlagNumber", "FlagNumber can't be greater than " + (this.NrOfFlags - 1).ToString());
            }
        }

        public void Flip(byte FlagNumber)
        {
            if (FlagNumber < this.NrOfFlags)
            {
                this.Flags = this.Flags ^ ((ulong)1 << FlagNumber);
            }
            else
            {
                throw new CollectionsDataOverflowException("BitFlags.cs", "BitFlags", "Reset", "FlagNumber", "FlagNumber can't be greater than " + (this.NrOfFlags - 1).ToString());
            }
        }

        public ulong GetFlags()
        {
            return this.Flags;
        }

        public void SetFlags(ulong Flags)
        {
            this.Flags = Flags;
        }
    }
    #endregion
}
