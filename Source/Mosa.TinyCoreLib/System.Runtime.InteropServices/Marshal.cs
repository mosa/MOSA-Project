using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Versioning;
using System.Security;
using Internal;

namespace System.Runtime.InteropServices;

public static class Marshal
{
	public static readonly int SystemDefaultCharSize;

	public static readonly int SystemMaxDBCSCharSize;

	public static int AddRef(IntPtr pUnk) => throw new NotImplementedException();

	public static IntPtr AllocCoTaskMem(int cb) => throw new NotImplementedException();

	public static IntPtr AllocHGlobal(int cb) => throw new NotImplementedException();

	public static IntPtr AllocHGlobal(IntPtr cb) => throw new NotImplementedException();

	public static bool AreComObjectsAvailableForCleanup() => throw new NotImplementedException();

	[RequiresUnreferencedCode("Built-in COM support is not trim compatible", Url = "https://aka.ms/dotnet-illink/com")]
	[SupportedOSPlatform("windows")]
	public static object BindToMoniker(string monikerName) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	public static void ChangeWrapperHandleStrength(object otp, bool fIsWeak) => throw new NotImplementedException();

	public static void CleanupUnusedObjectsInCurrentContext() => throw new NotImplementedException();

	public static void Copy(byte[] source, int startIndex, IntPtr destination, int length) => throw new NotImplementedException();

	public static void Copy(char[] source, int startIndex, IntPtr destination, int length) => throw new NotImplementedException();

	public static void Copy(double[] source, int startIndex, IntPtr destination, int length) => throw new NotImplementedException();

	public static void Copy(short[] source, int startIndex, IntPtr destination, int length) => throw new NotImplementedException();

	public static void Copy(int[] source, int startIndex, IntPtr destination, int length) => throw new NotImplementedException();

	public static void Copy(long[] source, int startIndex, IntPtr destination, int length) => throw new NotImplementedException();

	public static void Copy(IntPtr source, byte[] destination, int startIndex, int length) => throw new NotImplementedException();

	public static void Copy(IntPtr source, char[] destination, int startIndex, int length) => throw new NotImplementedException();

	public static void Copy(IntPtr source, double[] destination, int startIndex, int length) => throw new NotImplementedException();

	public static void Copy(IntPtr source, short[] destination, int startIndex, int length) => throw new NotImplementedException();

	public static void Copy(IntPtr source, int[] destination, int startIndex, int length) => throw new NotImplementedException();

	public static void Copy(IntPtr source, long[] destination, int startIndex, int length) => throw new NotImplementedException();

	public static void Copy(IntPtr source, IntPtr[] destination, int startIndex, int length) => throw new NotImplementedException();

	public static void Copy(IntPtr source, float[] destination, int startIndex, int length) => throw new NotImplementedException();

	public static void Copy(IntPtr[] source, int startIndex, IntPtr destination, int length) => throw new NotImplementedException();

	public static void Copy(float[] source, int startIndex, IntPtr destination, int length) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static IntPtr CreateAggregatedObject(IntPtr pOuter, object o) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	public static IntPtr CreateAggregatedObject<T>(IntPtr pOuter, T o) where T : notnull => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[return: NotNullIfNotNull("o")]
	public static object? CreateWrapperOfType(object? o, Type t) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	public static TWrapper CreateWrapperOfType<T, TWrapper>(T? o) => throw new NotImplementedException();

	[RequiresDynamicCode("Marshalling code for the object might not be available. Use the DestroyStructure<T> overload instead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static void DestroyStructure(IntPtr ptr, Type structuretype) => throw new NotImplementedException();

	public static void DestroyStructure<T>(IntPtr ptr) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	public static int FinalReleaseComObject(object o) => throw new NotImplementedException();

	public static void FreeBSTR(IntPtr ptr) => throw new NotImplementedException();

	public static void FreeCoTaskMem(IntPtr ptr) => throw new NotImplementedException();

	public static void FreeHGlobal(IntPtr hglobal) => throw new NotImplementedException();

	public static Guid GenerateGuidForType(Type type) => throw new NotImplementedException();

	public static string? GenerateProgIdForType(Type type) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static IntPtr GetComInterfaceForObject(object o, Type T) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static IntPtr GetComInterfaceForObject(object o, Type T, CustomQueryInterfaceMode mode) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	public static IntPtr GetComInterfaceForObject<T, TInterface>([DisallowNull] T o) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	public static object? GetComObjectData(object obj, object key) => throw new NotImplementedException();

	[RequiresDynamicCode("Marshalling code for the delegate might not be available. Use the GetDelegateForFunctionPointer<TDelegate> overload instead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Delegate GetDelegateForFunctionPointer(IntPtr ptr, Type t) => throw new NotImplementedException();

	public static TDelegate GetDelegateForFunctionPointer<TDelegate>(IntPtr ptr) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	public static int GetEndComSlot(Type t) => throw new NotImplementedException();

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("GetExceptionCode() may be unavailable in future releases.")]
	public static int GetExceptionCode() => throw new NotImplementedException();

	public static Exception? GetExceptionForHR(int errorCode) => throw new NotImplementedException();

	public static Exception? GetExceptionForHR(int errorCode, IntPtr errorInfo) => throw new NotImplementedException();

	public static IntPtr GetExceptionPointers() => throw new NotImplementedException();

	[RequiresDynamicCode("Marshalling code for the delegate might not be available. Use the GetFunctionPointerForDelegate<TDelegate> overload instead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static IntPtr GetFunctionPointerForDelegate(Delegate d)
	{
		ArgumentNullException.ThrowIfNull(d, nameof(d));

		return Impl.Marshal.GetFunctionPointerForDelegateInternal(d);
	}

	public static IntPtr GetFunctionPointerForDelegate<TDelegate>(TDelegate d) where TDelegate : notnull
	{
		ArgumentNullException.ThrowIfNull(d, nameof(d));

		return Impl.Marshal.GetFunctionPointerForDelegateInternal((Delegate)(object)d);
	}

	[RequiresAssemblyFiles("Windows only assigns HINSTANCE to assemblies loaded from disk. This API will return -1 for modules without a file on disk.")]
	public static IntPtr GetHINSTANCE(Module m) => throw new NotImplementedException();

	public static int GetHRForException(Exception? e) => throw new NotImplementedException();

	public static int GetHRForLastWin32Error() => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	public static IntPtr GetIDispatchForObject(object o) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	public static IntPtr GetIUnknownForObject(object o) => throw new NotImplementedException();

	public static int GetLastPInvokeError() => throw new NotImplementedException();

	public static int GetLastSystemError() => throw new NotImplementedException();

	public static int GetLastWin32Error() => throw new NotImplementedException();

	public static string GetLastPInvokeErrorMessage() => throw new NotImplementedException();

	public static string GetPInvokeErrorMessage(int error) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static void GetNativeVariantForObject(object? obj, IntPtr pDstNativeVariant) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static void GetNativeVariantForObject<T>(T? obj, IntPtr pDstNativeVariant) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	public static object GetObjectForIUnknown(IntPtr pUnk) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static object? GetObjectForNativeVariant(IntPtr pSrcNativeVariant) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static T? GetObjectForNativeVariant<T>(IntPtr pSrcNativeVariant) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static object?[] GetObjectsForNativeVariants(IntPtr aSrcNativeVariant, int cVars) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static T[] GetObjectsForNativeVariants<T>(IntPtr aSrcNativeVariant, int cVars) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	public static int GetStartComSlot(Type t) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	public static object GetTypedObjectForIUnknown(IntPtr pUnk, Type t) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	public static Type? GetTypeFromCLSID(Guid clsid) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	public static string GetTypeInfoName(ITypeInfo typeInfo) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	public static object GetUniqueObjectForIUnknown(IntPtr unknown) => throw new NotImplementedException();

	public static void InitHandle(SafeHandle safeHandle, IntPtr handle) => throw new NotImplementedException();

	public static bool IsComObject(object o) => throw new NotImplementedException();

	public static bool IsTypeVisibleFromCom(Type t) => throw new NotImplementedException();

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static IntPtr OffsetOf(Type t, string fieldName) => throw new NotImplementedException();

	public static IntPtr OffsetOf<T>(string fieldName) => throw new NotImplementedException();

	public static void Prelink(MethodInfo m) => throw new NotImplementedException();

	public static void PrelinkAll(Type c) => throw new NotImplementedException();

	public static string? PtrToStringAnsi(IntPtr ptr) => throw new NotImplementedException();

	public static string PtrToStringAnsi(IntPtr ptr, int len) => throw new NotImplementedException();

	public static string? PtrToStringAuto(IntPtr ptr) => throw new NotImplementedException();

	public static string? PtrToStringAuto(IntPtr ptr, int len) => throw new NotImplementedException();

	public static string PtrToStringBSTR(IntPtr ptr) => throw new NotImplementedException();

	public static string? PtrToStringUni(IntPtr ptr) => throw new NotImplementedException();

	public static string PtrToStringUni(IntPtr ptr, int len) => throw new NotImplementedException();

	public static string? PtrToStringUTF8(IntPtr ptr) => throw new NotImplementedException();

	public static string PtrToStringUTF8(IntPtr ptr, int byteLen) => throw new NotImplementedException();

	[RequiresDynamicCode("Marshalling code for the object might not be available")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static void PtrToStructure(IntPtr ptr, object structure) => throw new NotImplementedException();

	[RequiresDynamicCode("Marshalling code for the object might not be available")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static object? PtrToStructure(IntPtr ptr, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] Type structureType) => throw new NotImplementedException();

	public static T? PtrToStructure<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>(IntPtr ptr) => throw new NotImplementedException();

	public static void PtrToStructure<T>(IntPtr ptr, [DisallowNull] T structure) => throw new NotImplementedException();

	public static int QueryInterface(IntPtr pUnk, [In] ref Guid iid, out IntPtr ppv) => throw new NotImplementedException();

	public static byte ReadByte(IntPtr ptr) => throw new NotImplementedException();

	public static byte ReadByte(IntPtr ptr, int ofs) => throw new NotImplementedException();

	[RequiresDynamicCode("Marshalling code for the object might not be available")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("ReadByte(Object, Int32) may be unavailable in future releases.")]
	public static byte ReadByte(object ptr, int ofs) => throw new NotImplementedException();

	public static short ReadInt16(IntPtr ptr) => throw new NotImplementedException();

	public static short ReadInt16(IntPtr ptr, int ofs) => throw new NotImplementedException();

	[RequiresDynamicCode("Marshalling code for the object might not be available")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("ReadInt16(Object, Int32) may be unavailable in future releases.")]
	public static short ReadInt16(object ptr, int ofs) => throw new NotImplementedException();

	public static int ReadInt32(IntPtr ptr) => throw new NotImplementedException();

	public static int ReadInt32(IntPtr ptr, int ofs) => throw new NotImplementedException();

	[RequiresDynamicCode("Marshalling code for the object might not be available")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("ReadInt32(Object, Int32) may be unavailable in future releases.")]
	public static int ReadInt32(object ptr, int ofs) => throw new NotImplementedException();

	public static long ReadInt64(IntPtr ptr) => throw new NotImplementedException();

	public static long ReadInt64(IntPtr ptr, int ofs) => throw new NotImplementedException();

	[RequiresDynamicCode("Marshalling code for the object might not be available")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("ReadInt64(Object, Int32) may be unavailable in future releases.")]
	public static long ReadInt64(object ptr, int ofs) => throw new NotImplementedException();

	public static IntPtr ReadIntPtr(IntPtr ptr) => throw new NotImplementedException();

	public static IntPtr ReadIntPtr(IntPtr ptr, int ofs) => throw new NotImplementedException();

	[RequiresDynamicCode("Marshalling code for the object might not be available")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("ReadIntPtr(Object, Int32) may be unavailable in future releases.")]
	public static IntPtr ReadIntPtr(object ptr, int ofs) => throw new NotImplementedException();

	public static IntPtr ReAllocCoTaskMem(IntPtr pv, int cb) => throw new NotImplementedException();

	public static IntPtr ReAllocHGlobal(IntPtr pv, IntPtr cb) => throw new NotImplementedException();

	public static int Release(IntPtr pUnk) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	public static int ReleaseComObject(object o) => throw new NotImplementedException();

	public static IntPtr SecureStringToBSTR(SecureString s) => throw new NotImplementedException();

	public static IntPtr SecureStringToCoTaskMemAnsi(SecureString s) => throw new NotImplementedException();

	public static IntPtr SecureStringToCoTaskMemUnicode(SecureString s) => throw new NotImplementedException();

	public static IntPtr SecureStringToGlobalAllocAnsi(SecureString s) => throw new NotImplementedException();

	public static IntPtr SecureStringToGlobalAllocUnicode(SecureString s) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	public static bool SetComObjectData(object obj, object key, object? data) => throw new NotImplementedException();

	public static void SetLastPInvokeError(int error) => throw new NotImplementedException();

	public static void SetLastSystemError(int error) => throw new NotImplementedException();

	[RequiresDynamicCode("Marshalling code for the object might not be available. Use the SizeOf<T> overload instead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static int SizeOf(object structure) => throw new NotImplementedException();

	[RequiresDynamicCode("Marshalling code for the object might not be available. Use the SizeOf<T> overload instead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static int SizeOf(Type t) => throw new NotImplementedException();

	public static int SizeOf<T>() => throw new NotImplementedException();

	public static int SizeOf<T>(T structure) => throw new NotImplementedException();

	public static IntPtr StringToBSTR(string? s) => throw new NotImplementedException();

	public static IntPtr StringToCoTaskMemAnsi(string? s) => throw new NotImplementedException();

	public static IntPtr StringToCoTaskMemAuto(string? s) => throw new NotImplementedException();

	public static IntPtr StringToCoTaskMemUni(string? s) => throw new NotImplementedException();

	public static IntPtr StringToCoTaskMemUTF8(string? s) => throw new NotImplementedException();

	public static IntPtr StringToHGlobalAnsi(string? s) => throw new NotImplementedException();

	public static IntPtr StringToHGlobalAuto(string? s) => throw new NotImplementedException();

	public static IntPtr StringToHGlobalUni(string? s) => throw new NotImplementedException();

	[RequiresDynamicCode("Marshalling code for the object might not be available. Use the StructureToPtr<T> overload instead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static void StructureToPtr(object structure, IntPtr ptr, bool fDeleteOld) => throw new NotImplementedException();

	public static void StructureToPtr<T>([DisallowNull] T structure, IntPtr ptr, bool fDeleteOld) => throw new NotImplementedException();

	public static void ThrowExceptionForHR(int errorCode) => throw new NotImplementedException();

	public static void ThrowExceptionForHR(int errorCode, IntPtr errorInfo) => throw new NotImplementedException();

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static IntPtr UnsafeAddrOfPinnedArrayElement(Array arr, int index) => throw new NotImplementedException();

	public static IntPtr UnsafeAddrOfPinnedArrayElement<T>(T[] arr, int index) => throw new NotImplementedException();

	public static void WriteByte(IntPtr ptr, byte val) => throw new NotImplementedException();

	public static void WriteByte(IntPtr ptr, int ofs, byte val) => throw new NotImplementedException();

	[RequiresDynamicCode("Marshalling code for the object might not be available")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("WriteByte(Object, Int32, Byte) may be unavailable in future releases.")]
	public static void WriteByte(object ptr, int ofs, byte val) => throw new NotImplementedException();

	public static void WriteInt16(IntPtr ptr, char val) => throw new NotImplementedException();

	public static void WriteInt16(IntPtr ptr, short val) => throw new NotImplementedException();

	public static void WriteInt16(IntPtr ptr, int ofs, char val) => throw new NotImplementedException();

	public static void WriteInt16(IntPtr ptr, int ofs, short val) => throw new NotImplementedException();

	[RequiresDynamicCode("Marshalling code for the object might not be available")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("WriteInt16(Object, Int32, Char) may be unavailable in future releases.")]
	public static void WriteInt16(object ptr, int ofs, char val) => throw new NotImplementedException();

	[RequiresDynamicCode("Marshalling code for the object might not be available")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("WriteInt16(Object, Int32, Int16) may be unavailable in future releases.")]
	public static void WriteInt16(object ptr, int ofs, short val) => throw new NotImplementedException();

	public static void WriteInt32(IntPtr ptr, int val) => throw new NotImplementedException();

	public static void WriteInt32(IntPtr ptr, int ofs, int val) => throw new NotImplementedException();

	[RequiresDynamicCode("Marshalling code for the object might not be available")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("WriteInt32(Object, Int32, Int32) may be unavailable in future releases.")]
	public static void WriteInt32(object ptr, int ofs, int val) => throw new NotImplementedException();

	public static void WriteInt64(IntPtr ptr, int ofs, long val) => throw new NotImplementedException();

	public static void WriteInt64(IntPtr ptr, long val) => throw new NotImplementedException();

	[RequiresDynamicCode("Marshalling code for the object might not be available")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("WriteInt64(Object, Int32, Int64) may be unavailable in future releases.")]
	public static void WriteInt64(object ptr, int ofs, long val) => throw new NotImplementedException();

	public static void WriteIntPtr(IntPtr ptr, int ofs, IntPtr val) => throw new NotImplementedException();

	public static void WriteIntPtr(IntPtr ptr, IntPtr val) => throw new NotImplementedException();

	[RequiresDynamicCode("Marshalling code for the object might not be available")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("WriteIntPtr(Object, Int32, IntPtr) may be unavailable in future releases.")]
	public static void WriteIntPtr(object ptr, int ofs, IntPtr val) => throw new NotImplementedException();

	public static void ZeroFreeBSTR(IntPtr s) => throw new NotImplementedException();

	public static void ZeroFreeCoTaskMemAnsi(IntPtr s) => throw new NotImplementedException();

	public static void ZeroFreeCoTaskMemUnicode(IntPtr s) => throw new NotImplementedException();

	public static void ZeroFreeCoTaskMemUTF8(IntPtr s) => throw new NotImplementedException();

	public static void ZeroFreeGlobalAllocAnsi(IntPtr s) => throw new NotImplementedException();

	public static void ZeroFreeGlobalAllocUnicode(IntPtr s) => throw new NotImplementedException();
}
