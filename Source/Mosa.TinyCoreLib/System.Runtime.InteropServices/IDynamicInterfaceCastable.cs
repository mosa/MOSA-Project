namespace System.Runtime.InteropServices;

public interface IDynamicInterfaceCastable
{
	bool IsInterfaceImplemented(RuntimeTypeHandle interfaceType, bool throwIfNotImplemented);

	RuntimeTypeHandle GetInterfaceImplementation(RuntimeTypeHandle interfaceType);
}
