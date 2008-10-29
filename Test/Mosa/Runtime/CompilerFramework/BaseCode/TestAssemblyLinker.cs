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
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

using Mosa.Runtime;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Vm;

namespace Test.Mosa.Runtime.CompilerFramework.BaseCode
{
    /// <summary>
    /// 
    /// </summary>
    public class TestAssemblyLinker : AssemblyLinkerStageBase
    {
        #region Data members

        /// <summary>
        /// Holds the linker sections of the assembly linker.
        /// </summary>
        private List<TestLinkerSection> _sections;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAssemblyLinker"/> class.
        /// </summary>
        public TestAssemblyLinker()
        {
            int maxSections = (int)SectionKind.Max;
            _sections = new List<TestLinkerSection>(maxSections);
            for (int i = 0; i < maxSections; i++)
                _sections.Add(new TestLinkerSection((SectionKind)i, String.Empty, IntPtr.Zero));
        }

        #endregion // Construction

        #region AssemblyLinkerStageBase Overrides

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public override string Name
        {
            get { return @"Test Linker"; }
        }

        /// <summary>
        /// Retrieves the collection of sections created during compilation.
        /// </summary>
        /// <value>The sections collection.</value>
        public override ICollection<LinkerSection> Sections
        {
            get { return (ICollection<LinkerSection>)_sections.AsReadOnly(); }
        }

        /// <summary>
        /// Allocates a symbol of the given name in the specified section.
        /// </summary>
        /// <param name="section">The executable section to allocate from.</param>
        /// <param name="size">The number of bytes to allocate. If zero, indicates an unknown amount of memory is required.</param>
        /// <param name="alignment">The alignment. A value of zero indicates the use of a default alignment for the section.</param>
        /// <returns>
        /// A stream, which can be used to populate the section.
        /// </returns>
        protected override Stream Allocate(SectionKind section, int size, int alignment)
        {
            TestLinkerSection tle = _sections[(int)section];
            return tle.Allocate(size, alignment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="linkType"></param>
        /// <param name="method"></param>
        /// <param name="methodOffset"></param>
        /// <param name="methodRelativeBase"></param>
        /// <param name="targetAddress"></param>
        protected unsafe override void ApplyPatch(LinkType linkType, RuntimeMethod method, long methodOffset, long methodRelativeBase, long targetAddress)
        {
            long value;
            switch (linkType & LinkType.KindMask)
            {
                case LinkType.RelativeOffset:
                    value = targetAddress - (method.Address.ToInt64() + methodRelativeBase);
                    break;
                case LinkType.AbsoluteAddress:
                    value = targetAddress;
                    break;
                default:
                    throw new NotSupportedException();
            }
            long address = method.Address.ToInt64() + methodOffset;
            // Position is a raw memory address, we're just storing value there
            Debug.Assert(0 != value && value == (int)value);
            int* pAddress = (int*)address;
            *pAddress = (int)value;
        }

        private Dictionary<RuntimeMethod, Delegate> resolvedInternals = new Dictionary<RuntimeMethod, Delegate>();

        /// <summary>
        /// Special resolution for internal calls.
        /// </summary>
        /// <param name="method">The internal call method to resolve.</param>
        /// <returns>The address</returns>
        protected unsafe override long ResolveInternalCall(RuntimeMethod method)
        {
            Delegate methodDelegate = null;
            if (false == this.resolvedInternals.TryGetValue(method, out methodDelegate))
            {
                ITypeSystem ts = RuntimeBase.Instance.TypeLoader;
                RuntimeMethod internalImpl = ts.GetImplementationForInternalCall(method);
                if (null != internalImpl)
                {
                    // Find the .NET counterpart for this method (we're not really compiling or using trampolines just yet.)
                    string typeName = String.Format(@"{0}.{1}, {2}",
                        internalImpl.DeclaringType.Namespace,
                        internalImpl.DeclaringType.Name,
                        internalImpl.Module.Name);
                    Type type = Type.GetType(typeName);
                    MethodInfo mi = type.GetMethod(internalImpl.Name);

                    methodDelegate = BuildDelegateForMethodInfo(mi);
                    this.resolvedInternals.Add(method, methodDelegate);
                }
            }

            if (null == methodDelegate)
                throw new NotImplementedException(@"InternalCall implementation not loaded.");

            return Marshal.GetFunctionPointerForDelegate(methodDelegate).ToInt64();
        }

        #endregion // AssemblyLinkerStageBase Overrides

        /// <summary>
        /// Builds the delegate for the given method info.
        /// </summary>
        /// <param name="mi">The method to build a delegate for.</param>
        /// <returns>The created delegate instance.</returns>
        private Delegate BuildDelegateForMethodInfo(MethodInfo mi)
        {
            AssemblyName assemblyName = new AssemblyName(mi.Name + "DynamicDelegate");
            AssemblyBuilder builder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = builder.DefineDynamicModule(assemblyName.Name);

            TypeBuilder typeBuilder = moduleBuilder.DefineType(mi.Name + "Delegate", TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.AnsiClass | TypeAttributes.AutoClass, typeof(MulticastDelegate));
            CustomAttributeBuilder cab = CreateUnmanagedCallingConvention();
            typeBuilder.SetCustomAttribute(cab);

            ConstructorBuilder ctorBuilder = typeBuilder.DefineConstructor(MethodAttributes.RTSpecialName | MethodAttributes.HideBySig | MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(object), typeof(IntPtr) });
            ctorBuilder.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

            // Create the invoke method
            ParameterInfo[] parameters = mi.GetParameters();
            Type[] paramTypes = new Type[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                paramTypes[i] = parameters[i].ParameterType;
            }

            // Define the Invoke method for the delegate
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("Invoke", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual, CallingConventions.Standard, mi.ReturnType, paramTypes);
            methodBuilder.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

            Type fullType = typeBuilder.CreateType();
            return Delegate.CreateDelegate(fullType, mi);
        }

        private CustomAttributeBuilder CreateUnmanagedCallingConvention()
        {
            Type ufpa = typeof(UnmanagedFunctionPointerAttribute);
            ConstructorInfo ci = ufpa.GetConstructor(new Type[] { typeof(System.Runtime.InteropServices.CallingConvention) });
            return new CustomAttributeBuilder(ci, new object[] { System.Runtime.InteropServices.CallingConvention.Cdecl });
        }
    }
}
