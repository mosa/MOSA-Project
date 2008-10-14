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
        /// Initializes a new instance of <see cref="AssemblyLinkerStageBase"/>.
        /// </summary>
        protected AssemblyLinkerStageBase()
        {
            _linkRequests = new Dictionary<RuntimeMember, List<LinkRequest>>();
        }

        #endregion // Construction

        #region IAssemblyCompilerStage Members

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value></value>
        string IAssemblyCompilerStage.Name
        {
            get { return @"Linker"; }
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        void IAssemblyCompilerStage.Run(AssemblyCompiler compiler)
        {
            long address;

            // Check if we have unresolved requests and try to link them
            List<RuntimeMember> members = new List<RuntimeMember>(_linkRequests.Keys);
            foreach (RuntimeMember member in members)
            {
                // Is the runtime member resolved?
                if (true == IsResolved(member, out address))
                {
                    // Yes, patch up the method
                    List<LinkRequest> link = _linkRequests[member];
                    PatchRequests(address, link);
                    _linkRequests.Remove(member);
                }
            }
            Debug.Assert(0 == _linkRequests.Count, @"AssemblyLinker has found unresolved symbols.");
            if (0 != _linkRequests.Count)
                throw new Exception(@"Unresolved symbols.");
        }

        #endregion // IAssemblyCompilerStage Members

        #region Methods

        /// <summary>
        /// A request to patch already emitted code by storing the calculated address value.
        /// </summary>
        /// <param name="linkType">Type of the link.</param>
        /// <param name="method">The method whose code is being patched.</param>
        /// <param name="methodOffset">The value to store at the position in code.</param>
        /// <param name="methodRelativeBase">The method relative base.</param>
        /// <param name="targetAddress">The position in code, where it should be patched.</param>
        protected abstract void ApplyPatch(LinkType linkType, RuntimeMethod method, long methodOffset, long methodRelativeBase, long targetAddress);

        #endregion // Methods

        #region Internals

        /// <summary>
        /// Determines if the given runtime member can be resolved immediately.
        /// </summary>
        /// <param name="member">The runtime member to determine resolution of.</param>
        /// <param name="address">Receives the determined address of the runtime member.</param>
        /// <returns>
        /// The method returns true, when it was successfully resolved.
        /// </returns>
        protected virtual bool IsResolved(RuntimeMember member, out long address)
        {
            // Init out params
            address = 0;

            // Is this a method?
            RuntimeMethod method = member as RuntimeMethod;
            if (null != method)
            {
                if (method.ImplAttributes == MethodImplAttributes.InternalCall)
                {
                    address = ResolveInternalCall(method);
                }
                else
                {
                    Debug.Assert(MethodAttributes.Abstract != (method.Attributes & MethodAttributes.Abstract), @"Can't link an abstract method.");
                    address = method.Address.ToInt64();
                }
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
        /// Special resolution for internal calls.
        /// </summary>
        /// <param name="method">The internal call method to resolve.</param>
        /// <returns>The address </returns>
        protected virtual long ResolveInternalCall(RuntimeMethod method)
        {
            long address = 0;
            ITypeSystem ts = RuntimeBase.Instance.TypeLoader;
            RuntimeMethod internalImpl = ts.GetImplementationForInternalCall(method);
            if (null != internalImpl)
                address = internalImpl.Address.ToInt64();
            return address;
        }

        /// <summary>
        /// Checks that <paramref name="member"/> is a member, which can be linked.
        /// </summary>
        /// <param name="member">The member to check.</param>
        /// <returns>
        /// True, if the member is valid for linking.
        /// </returns>
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
                ApplyPatch(
                    request.LinkType,
                    request.Method,
                    request.MethodOffset,
                    request.MethodRelativeBase,
                    address
                );
            }
        }

        #endregion // Internals

        #region IAssemblyLinker Members

        /// <summary>
        /// Issues a linker request for the given runtime method.
        /// </summary>
        /// <param name="linkType">The type of link required.</param>
        /// <param name="method">The method the patched code belongs to.</param>
        /// <param name="methodOffset">The offset inside the method where the patch is placed.</param>
        /// <param name="methodRelativeBase">The base address, if a relative link is required.</param>
        /// <param name="target">The method or static field to link against.</param>
        public long Link(LinkType linkType, RuntimeMethod method, int methodOffset, int methodRelativeBase, RuntimeMember target)
        {
            Debug.Assert(null != target, @"A RuntimeMember must be passed to IAssemblyLinker.Link");
            if (null == target)
                throw new ArgumentNullException(@"member");
            Debug.Assert(true == IsValid(target), @"Invalid RuntimeMember passed to IAssemblyLinker.Link");
            if (false == IsValid(target))
                throw new ArgumentException(@"RuntimeMember is not a static field or method.", @"member");


            long result = 0;
            if (true == IsResolved(target, out result))
            {
                List<LinkRequest> patchList;
                if (false == _linkRequests.TryGetValue(target, out patchList))
                {
                    patchList = new List<LinkRequest>(1);
                    patchList.Add(new LinkRequest(linkType, method, methodOffset, methodRelativeBase, target));
                }

                PatchRequests(result, patchList);
            }
            else
            {
                // FIXME: Make this thread safe
                List<LinkRequest> list;
                if (false == _linkRequests.TryGetValue(target, out list))
                {
                    list = new List<LinkRequest>();
                    _linkRequests.Add(target, list);
                }

                list.Add(new LinkRequest(linkType, method, methodOffset, methodRelativeBase, target));
            }

            return result;
        }

        #endregion // IAssemblyLinker Members
    }
}
