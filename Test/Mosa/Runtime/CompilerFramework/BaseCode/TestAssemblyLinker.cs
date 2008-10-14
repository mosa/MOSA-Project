using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.CompilerFramework;
using System.Diagnostics;
using Mosa.Runtime.Vm;
using System.Reflection;
using System.Runtime.InteropServices;
using Mosa.Runtime;
using System.Reflection.Emit;
using System.IO;

namespace Test.Mosa.Runtime.CompilerFramework.BaseCode
{
    /// <summary>
    /// 
    /// </summary>
    public class TestAssemblyLinker : AssemblyLinkerStageBase
    {
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
