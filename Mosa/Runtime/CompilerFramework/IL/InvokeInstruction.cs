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
using System.Diagnostics;
using System.Text;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /* FIXME:
     * - Schedule compilation of invocation target
     * - Scheduling puts the target method on the jit or aot compilers work list
     * - This allows the jit to run async ahead of time in the same process
     * - This may not turn the jit into a full aot, but is used to prepare MethodCompilers and jit stubs
     *   for invoked methods.
     */

    /// <summary>
    /// Base class for instructions, which invoke other functions.
    /// </summary>
    public abstract class InvokeInstruction : ILInstruction
    {
        #region Types

        /// <summary>
        /// Specifies a set of flags used to control invocation target metadata decoding.
        /// </summary>
        [Flags]
        protected enum InvokeSupportFlags
        {
            MemberRef = 1,
            MethodDef = 2,
            MethodSpec = 4,
            CallSite = 8,
            All = MemberRef|MethodDef|MethodSpec|CallSite
        }

        #endregion // Types

        #region Data members

        /// <summary>
        /// Holds the function being called.
        /// </summary>
        protected RuntimeMethod _invokeTarget;

        /// <summary>
        /// Holds the parameter types of the function called.
        /// </summary>
        protected SigType[] _arguments;

        /// <summary>
        /// The target token.
        /// </summary>
        protected TokenTypes _token;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the invoke instruction.
        /// </summary>
        /// <param name="code">The opcode of the invoke instruction.</param>
        protected InvokeInstruction(OpCode code)
            : base(code)
        {
        }

        #endregion // Construction

        #region Properties

        public sealed override FlowControl FlowControl
        {
            get { return FlowControl.Call; }
        }

        /// <summary>
        /// Gets the supported immediate metadata tokens in the instruction.
        /// </summary>
        protected abstract InvokeSupportFlags InvokeSupport
        {
            get;
        }

        /// <summary>
        /// Retrieves the target method of the invocation.
        /// </summary>
        public RuntimeMethod InvokeTarget
        {
            get { return _invokeTarget; }
        }

        /// <summary>
        /// Retrieves the reference of the this pointer.
        /// </summary>
        public Operand ThisReference
        {
            get { throw new NotImplementedException(); }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Decodes the invocation target.
        /// </summary>
        /// <param name="decoder">The IL decoder, which provides decoding functionality.</param>
        /// <param name="flags">Flags, which control the </param>
        protected void DecodeInvocationTarget(IInstructionDecoder decoder, InvokeSupportFlags flags)
        {
            // Holds the token of the call target
            TokenTypes callTarget, targetType;
            // Signature of the call target
            MethodSignature signature;
            // Number of parameters required for the call
            int paramCount = 0;

            // Retrieve the immediate argument - it contains the token
            // of the methoddef, methodref, methodspec or callsite to call.
            callTarget = _token = decoder.DecodeToken();
            targetType = (TokenTypes.TableMask & callTarget);
            if (false == IsCallTargetSupported(targetType, flags))
                throw new InvalidOperationException(@"Invalid IL call target specification.");

            switch (targetType)
            {
                case TokenTypes.MethodDef:
                    // FIXME: _invokeTarget = DefaultTypeSystem.GetMethod(decoder.Method.Module, callTarget);
                    break;

                case TokenTypes.MemberRef:
                    throw new NotImplementedException();
                    break;

                case TokenTypes.MethodSpec:
                    throw new NotImplementedException();
                    break;

                default:
                    Debug.Assert(false, @"Should never reach this!");
                    break;
            }

            // Retrieve the target signature
            signature = _invokeTarget.Signature;

            // Fix the parameter list
            paramCount = signature.Parameters.Length;
            if (true == signature.HasThis && false == signature.HasExplicitThis)
                paramCount++;

            // Process parameters
            // FIXME: _operands = new Operand[paramCount];

            // Is the function returning void?
            if (signature.ReturnType.Type != CilElementType.Void)
            {
                SetResult(0, CreateResultOperand(decoder.Architecture, signature.ReturnType));
            }

/*
            TokenTypes token, tokenType;
            IMetadataProvider provider = decoder.Metadata;
            IMethodSignature method;

            // Figure out the token type
            switch (tokenType)
            {
                case TokenTypes.MemberRef:
                    method = new MetadataMemberReference(provider, token).Resolve() as IMethodSignature;
                    break;

                case TokenTypes.MethodSpec:
                    method = new MetadataMethodSpecification(provider, token);
                    break;
            }

            // FIXME: decoder.AssemblyCompiler.ScheduleMethodForCompilation(_invokeTarget);
 */ 
        }

        public override object Expand(MethodCompilerBase methodCompiler)
        {
            IArchitecture architecture = methodCompiler.Architecture;
            ICallingConvention cc = architecture.GetCallingConvention(_invokeTarget.Signature.CallingConvention);
            Debug.Assert(null != cc, @"Failed to retrieve the calling convention.");
            return cc.Expand(architecture, this);
        }

        private bool IsCallTargetSupported(TokenTypes targetType, InvokeSupportFlags flags)
        {
            bool result = false;

            if (TokenTypes.MethodDef == targetType && InvokeSupportFlags.MethodDef == (flags & InvokeSupportFlags.MethodDef))
                result = true;
            else if (TokenTypes.MemberRef == targetType && InvokeSupportFlags.MemberRef == (flags & InvokeSupportFlags.MemberRef))
                result = true;
            else if (TokenTypes.MethodSpec == targetType && InvokeSupportFlags.MethodSpec == (flags & InvokeSupportFlags.MethodSpec))
                result = true;

            return result;
        }

        private object FindInvokeOverload(IMetadataProvider metadata, SigType ownerType, TokenTypes nameIdx, TokenTypes signatureIdx)
        {
            throw new NotImplementedException();
/*
            MethodDefinition result = null;
            TypeDefinition elementTypeDef = ownerType.ElementType as TypeDefinition;
            string name;
            Debug.Assert(null != elementTypeDef, @"Cross assembly type resolution not supported yet.");
            if (null == elementTypeDef)
            {
                // FIXME: Resolve the reference using all referenced assemblies
                throw new InvalidOperationException(@"Cross assembly type resolution not supported yet.");
            }

            metadata.Read(nameIdx, out name);

            foreach (MethodDefinition methodDef in elementTypeDef.Methods)
            {
                if (true == methodDef.Name.Equals(name))
                {
                    // FIXME: Check the signatures...
                    if (true == IsSameSignature(metadata, methodDef.SignatureIdx, signatureIdx))
                    {
                        // We've found the method
                        //result = temp;
                        //result.OwnerType = ownerType;
                        result = methodDef;
                        break;
                    }
 
                }
            }
            
            return result;
 */
        }

        private bool IsSameSignature(IMetadataProvider metadata, TokenTypes sig1, TokenTypes sig2)
        {
            byte[] src, dst;
            metadata.Read(sig1, out src);
            metadata.Read(sig2, out dst);
            bool result = (src.Length == dst.Length);
            if (true == result)
            {
                for (int i = 0; true == result && i < src.Length; i++)
                    result = (src[i] == dst[i]);
            }
            return result;
        }

        /*
                private TypeReference FindOwnerTypeByMethod(IMetadataProvider provider, TokenTypes methodDefToken)
                {
                    TypeDefinition result = null, t1, t2;
                    TokenTypes max = (TokenTypes)provider.GetMaxTokenValue(TokenTypes.TypeDef);

                    // Read the first type definition
                    t1 = provider.GetRow<TypeDefinition>(provider, TokenTypes.TypeDef+1);
                    for (TokenTypes idx = TokenTypes.TypeDef+2; idx <= max; idx++)
                    {
                        t2 = provider.GetRow<TypeDefinition>(provider, idx);
                        if (t1.MethodIdx < methodDefToken && methodDefToken < t2.MethodIdx)
                        {
                            result = t1;
                            break;
                        }
                        else
                        {
                            // Swap the type definitions, and keep going
                            result = t1;
                            t1 = t2;
                            t2 = result;
                        }
                    }

                    Debug.Assert(null != result, @"Failed to find owner type of method.");
                    if (null == result)
                        throw new ExecutionEngineException(@"Failed to find owner type of method.");
                    return result;
                }

                /// <summary>
                /// Determines the owner type of the method to invoke.
                /// </summary>
                /// <param name="metadata">The metadata provider used for lookups.</param>
                /// <param name="token">The metadata token of the method to lookup.</param>
                /// <returns>A type reference of the owner type.</returns>
                private TypeReference FindOwnerType(IMetadataProvider metadata, TokenTypes token)
                {
                    Debug.Assert(TokenTypes.MethodDef == (TokenTypes.TableMask & (TokenTypes)token));
                    TypeDefinition typeDef;

                    TokenTypes methodBound, nextTypeRow;
                    TokenTypes typeRowCount = metadata.GetMaxTokenValue(TokenTypes.TypeDef);
                    for (TokenTypes typeRow = TokenTypes.TypeDef+1; typeRow < typeRowCount; typeRow++)
                    {
                        typeDef = metadata.GetRow<TypeDefinition>(metadata, typeRow);
                        nextTypeRow = typeRow + 1;
                        if (nextTypeRow <= typeRowCount)
                        {
                            TypeDefinition nextType = metadata.GetRow<TypeDefinition>(metadata, nextTypeRow);
                            methodBound = nextType.MethodIdx;
                        }
                        else
                        {
                            methodBound = metadata.GetMaxTokenValue(TokenTypes.MethodDef);
                        }

                        if (typeDef.MethodIdx <= token && token < methodBound)
                        {
                            return typeDef;
                        }
                    }

                    throw new InvalidOperationException();
                }
        */
        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode the jump target
            DecodeInvocationTarget(decoder, this.InvokeSupport);

            // Create an operand set for the call
            if (null != _arguments && 0 != _arguments.Length)
            {
                // FIXME: _operands = new Operand[_arguments.Length];
            }
        }

        /// <summary>
        /// Convert the call to a string.
        /// </summary>
        /// <returns>The string of the call expression.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            // Output the result...
            if (0 != this.Results.Length)
            {
                builder.AppendFormat("{0} = ", this.Results[0]);
            }
            else if (_code != OpCode.Jmp)
            {
                builder.Append("call ");
            }
            else
            {
                builder.Append("jump ");
            }

            builder.Append(_invokeTarget);
            return builder.ToString();
        }

        public override void Validate(MethodCompilerBase compiler)
        {
            // Validate the base class first.
            base.Validate(compiler);

            // Validate the operands...
            Operand[] ops = this.Operands;
            Debug.Assert(ops.Length == _arguments.Length, @"Operand count doesn't match parameter count.");
            for (int i = 0; i < ops.Length; i++)
            {
                if (null != ops[i])
                {
/* FIXME: Check implicit conversions
                    Debug.Assert(_operands[i].Type == _parameterTypes[i]);
                    if (_operands[i].Type != _parameterTypes[i])
                    {
                        // FIXME: Determine if we can do an implicit conversion
                        throw new ExecutionEngineException(@"Invalid operand types.");
                    }
 */
                }
            }
        }

        #endregion // Methods
    }
}
