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
using Mosa.Runtime.Vm;
using Mosa.Runtime.Metadata;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// This compilation stage links all external labels together, which
    /// were previously registered.
    /// </summary>
    public abstract class AssemblyLinkerStageBase : IAssemblyCompilerStage, IAssemblyLinker
    {
        #region Data members

        /// <summary>
        /// Holds all unresolved link requests.
        /// </summary>
        private Dictionary<RuntimeMember, List<LinkRequest>> _linkRequests;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="AssemblyLinkerStage"/>.
        /// </summary>
        protected AssemblyLinkerStageBase()
        {
            _linkRequests = new Dictionary<RuntimeMember, List<LinkRequest>>();
        }

        #endregion // Construction

        #region IAssemblyCompilerStage Members

        string IAssemblyCompilerStage.Name
        {
            get { return @"Linker"; }
        }

        void IAssemblyCompilerStage.Run(AssemblyCompiler compiler)
        {
            long address;

            // Check if we have unresolved requests and try to link them
            foreach (KeyValuePair<RuntimeMember, List<LinkRequest>> request in _linkRequests)
            {
                // Is the runtime member resolved?
                if (true == IsResolved(request.Key, out address))
                {
                    // Yes, patch up the method
                    PatchRequests(address, request.Value);
                }
            }
        }

        #endregion // IAssemblyCompilerStage Members

        #region Methods

        /// <summary>
        /// A request to patch already emitted code by storing the calculated address value.
        /// </summary>
        /// <param name="position">The position in code, where it should be patched.</param>
        /// <param name="value">The value to store at the position in code.</param>
        protected abstract void ApplyPatch(long position, long value);

        #endregion // Methods

        #region Internals

        /// <summary>
        /// Determines if the given runtime member can be resolved immediately.
        /// </summary>
        /// <param name="member">The runtime member to determine resolution of.</param>
        /// <param name="address">Receives the determined address of the runtime member.</param>
        /// <returns>The method returns true, when it was successfully resolved.</returns>
        protected virtual bool IsResolved(RuntimeMember member, out long address)
        {          
            // Init out params
            address = 0;

            // Is this a method?
            RuntimeMethod method = member as RuntimeMethod;
            if (null != method)
            {
                Debug.Assert(MethodAttributes.Abstract != (method.Attributes & MethodAttributes.Abstract), @"Can't link an abstract method.");
                address = method.Address.ToInt64();
            }
            else
            {
                RuntimeField field = member as RuntimeField;
                if (null != field)
                {
                    Debug.Assert(FieldAttributes.Static == (field.Attributes & FieldAttributes.Static));
                    address = field.Address.ToInt64();
                }
            }

            return (0 != address);
        }

        /// <summary>
        /// Checks that <paramref name="member"/> is a member, which can be linked.
        /// </summary>
        /// <param name="member">The member to check.</param>
        /// <returns>True, if the member is valid for linking.</returns>
        protected bool IsValid(RuntimeMember member)
        {
            return (member is RuntimeMethod || (member is RuntimeField && FieldAttributes.Static == (FieldAttributes.Static & ((RuntimeField)member).Attributes)));
        }

        /// <summary>
        /// Patches all requests in the given link request list.
        /// </summary>
        /// <param name="address">The address of the member.</param>
        /// <param name="requests">A list of requests to patch.</param>
        private void PatchRequests(long address, IEnumerable<LinkRequest> requests)
        {
            foreach (LinkRequest request in requests)
            {
                // Patch the code stream
                ApplyPatch(request.Position, address - request.RelativeBase);
            }
        }

        #endregion // Internals

        #region IAssemblyLinker Members

        public long Link(RuntimeMember member, long address, long relativeBase)
        {
            Debug.Assert(null != member, @"A RuntimeMember must be passed to IAssemblyLinker.Link");
            if (null == member)
                throw new ArgumentNullException(@"member");
            Debug.Assert(true == IsValid(member), @"Invalid RuntimeMember passed to IAssemblyLinker.Link");
            if (false == IsValid(member))
                throw new ArgumentException(@"RuntimeMember is not a static field or method.", @"member");


            long result = 0;
            if (true == IsResolved(member, out result))
            {
                List<LinkRequest> patchList;
                if (false == _linkRequests.TryGetValue(member, out patchList))
                {
                    patchList = new List<LinkRequest>(1);
                    patchList.Add(new LinkRequest(address, relativeBase));
                }

                PatchRequests(result, patchList);
            }
            else
            {
                // FIXME: Make this thread safe
                List<LinkRequest> list;
                if (false == _linkRequests.TryGetValue(member, out list))
                {
                    list = new List<LinkRequest>();
                    _linkRequests.Add(member, list);
                }

                list.Add(new LinkRequest(address, relativeBase));
            }

            return result;
        }

        #endregion // IAssemblyLinker Members
    }
}
