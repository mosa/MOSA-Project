namespace System.Runtime.InteropServices.Marshalling;

[CLSCompliant(false)]
public interface IIUnknownInterfaceDetailsStrategy
{
	IComExposedDetails? GetComExposedTypeDetails(RuntimeTypeHandle type);

	IIUnknownDerivedDetails? GetIUnknownDerivedDetails(RuntimeTypeHandle type);
}
