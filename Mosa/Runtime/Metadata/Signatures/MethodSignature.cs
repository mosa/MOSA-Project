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
using System.Diagnostics;

namespace Mosa.Runtime.Metadata.Signatures
{
    public class MethodSignature : Signature
    {
		private CallingConvention _callingConvention;
		
		private int _genericParameterCount;
		
		private bool _hasExplicitThis;
		
		private bool _hasThis;
				
		private SigType[] _parameters;
		
		private SigType _returnType;
		
		public CallingConvention CallingConvention
		{
			get { return _callingConvention; }
		}
		
		public int GenericParameterCount
		{
			get { return _genericParameterCount; }
		}
		
		public bool HasExplicitThis
		{
			get { return _hasExplicitThis; }
		}
		
		public bool HasThis
		{
			get { return _hasThis; }
		}
		
		public SigType[] Parameters
		{
			get { return _parameters; }
		}
		
		public SigType ReturnType
		{
			get { return _returnType; }
		}

        public static MethodSignature Parse(IMetadataProvider provider, TokenTypes token)
        {
            byte[] buffer;
            int index = 0;
            provider.Read(token, out buffer);
            MethodSignature msig = new MethodSignature();
            msig.ParseSignature(buffer, ref index);
            Debug.Assert(index == buffer.Length, @"Signature parser didn't complete.");
            return msig;
        }
	
		protected sealed override void ParseSignature(byte[] buffer, ref int index)
		{
            // Check for instance signature
            if (HAS_THIS == (buffer[index] & HAS_THIS))
            {
                _hasThis = true;
            }
            if (HAS_EXPLICIT_THIS == (buffer[index] & HAS_EXPLICIT_THIS))
                _hasExplicitThis = true;
            if (GENERIC == (buffer[index] & GENERIC))
            {
                _callingConvention = CallingConvention.Generic;
                _genericParameterCount = Utilities.ReadCompressedInt32(buffer, ref index);
            }
            else if (VARARG == (buffer[index] & VARARG))
            {
                _callingConvention = CallingConvention.Vararg;
            }
            else if (0x00 != (buffer[index] & 0x1F))
            {
                throw new InvalidOperationException(@"Invalid method definition signature.");
            }
			
			index++;

            // Number of parameters
            int paramCount = Utilities.ReadCompressedInt32(buffer, ref index);
            _parameters = new SigType[paramCount];

            // Read the return type
            _returnType = SigType.ParseTypeSignature(buffer, ref index);

            // Read all parameters
            for (int i = 0; i < paramCount; i++)
                _parameters[i] = SigType.ParseTypeSignature(buffer, ref index);
		}

        private const byte DEFAULT = 0x00;
        private const byte VARARG = 0x05;
        private const byte GENERIC = 0x10;
        private const byte HAS_THIS = 0x20;
        private const byte HAS_EXPLICIT_THIS = 0x40;

        public bool Matches(MethodSignature other)
        {
            if (object.ReferenceEquals(this, other))
                return true;
            // TODO: Check this to make sure it is correct
            if (other.GenericParameterCount != this.GenericParameterCount)
                return false;
            if (other.CallingConvention != this.CallingConvention)
                return false;
            if (other.HasThis != this.HasThis)
                return false;
            if (other.HasExplicitThis != this.HasExplicitThis)
                return false;
            if (this.Parameters.Length != other.Parameters.Length)
                return false;
            if (!this.ReturnType.Matches(other.ReturnType))
                return false;
            for (int i = 0; i < this.Parameters.Length; i++)
            {
                if (!this.Parameters[i].Matches(other.Parameters[i]))
                    return false;
            }
            return true;
        }
    }
}
