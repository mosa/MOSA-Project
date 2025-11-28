using System.ComponentModel;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace System.Runtime.InteropServices.JavaScript;

[StructLayout(LayoutKind.Sequential, Size = 1)]
[SupportedOSPlatform("browser")]
[CLSCompliant(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public struct JSMarshalerArgument
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public delegate void ArgumentToManagedCallback<T>(ref JSMarshalerArgument arg, out T value);

	[EditorBrowsable(EditorBrowsableState.Never)]
	public delegate void ArgumentToJSCallback<T>(ref JSMarshalerArgument arg, T value);

	public void Initialize()
	{
		throw null;
	}

	public void ToManaged(out bool value)
	{
		throw null;
	}

	public void ToJS(bool value)
	{
		throw null;
	}

	public void ToManaged(out bool? value)
	{
		throw null;
	}

	public void ToJS(bool? value)
	{
		throw null;
	}

	public void ToManaged(out byte value)
	{
		throw null;
	}

	public void ToJS(byte value)
	{
		throw null;
	}

	public void ToManaged(out byte? value)
	{
		throw null;
	}

	public void ToJS(byte? value)
	{
		throw null;
	}

	public void ToManaged(out byte[]? value)
	{
		throw null;
	}

	public void ToJS(byte[]? value)
	{
		throw null;
	}

	public void ToManaged(out char value)
	{
		throw null;
	}

	public void ToJS(char value)
	{
		throw null;
	}

	public void ToManaged(out char? value)
	{
		throw null;
	}

	public void ToJS(char? value)
	{
		throw null;
	}

	public void ToManaged(out short value)
	{
		throw null;
	}

	public void ToJS(short value)
	{
		throw null;
	}

	public void ToManaged(out short? value)
	{
		throw null;
	}

	public void ToJS(short? value)
	{
		throw null;
	}

	public void ToManaged(out int value)
	{
		throw null;
	}

	public void ToJS(int value)
	{
		throw null;
	}

	public void ToManaged(out int? value)
	{
		throw null;
	}

	public void ToJS(int? value)
	{
		throw null;
	}

	public void ToManaged(out int[]? value)
	{
		throw null;
	}

	public void ToJS(int[]? value)
	{
		throw null;
	}

	public void ToManaged(out long value)
	{
		throw null;
	}

	public void ToJS(long value)
	{
		throw null;
	}

	public void ToManaged(out long? value)
	{
		throw null;
	}

	public void ToJS(long? value)
	{
		throw null;
	}

	public void ToManagedBig(out long value)
	{
		throw null;
	}

	public void ToJSBig(long value)
	{
		throw null;
	}

	public void ToManagedBig(out long? value)
	{
		throw null;
	}

	public void ToJSBig(long? value)
	{
		throw null;
	}

	public void ToManaged(out float value)
	{
		throw null;
	}

	public void ToJS(float value)
	{
		throw null;
	}

	public void ToManaged(out float? value)
	{
		throw null;
	}

	public void ToJS(float? value)
	{
		throw null;
	}

	public void ToManaged(out double value)
	{
		throw null;
	}

	public void ToJS(double value)
	{
		throw null;
	}

	public void ToManaged(out double? value)
	{
		throw null;
	}

	public void ToJS(double? value)
	{
		throw null;
	}

	public void ToManaged(out double[]? value)
	{
		throw null;
	}

	public void ToJS(double[]? value)
	{
		throw null;
	}

	public void ToManaged(out IntPtr value)
	{
		throw null;
	}

	public void ToJS(IntPtr value)
	{
		throw null;
	}

	public void ToManaged(out IntPtr? value)
	{
		throw null;
	}

	public void ToJS(IntPtr? value)
	{
		throw null;
	}

	public void ToManaged(out DateTimeOffset value)
	{
		throw null;
	}

	public void ToJS(DateTimeOffset value)
	{
		throw null;
	}

	public void ToManaged(out DateTimeOffset? value)
	{
		throw null;
	}

	public void ToJS(DateTimeOffset? value)
	{
		throw null;
	}

	public void ToManaged(out DateTime value)
	{
		throw null;
	}

	public void ToJS(DateTime value)
	{
		throw null;
	}

	public void ToManaged(out DateTime? value)
	{
		throw null;
	}

	public void ToJS(DateTime? value)
	{
		throw null;
	}

	public void ToManaged(out string? value)
	{
		throw null;
	}

	public void ToJS(string? value)
	{
		throw null;
	}

	public void ToManaged(out string?[]? value)
	{
		throw null;
	}

	public void ToJS(string?[]? value)
	{
		throw null;
	}

	public void ToManaged(out Exception? value)
	{
		throw null;
	}

	public void ToJS(Exception? value)
	{
		throw null;
	}

	public void ToManaged(out object? value)
	{
		throw null;
	}

	public void ToJS(object? value)
	{
		throw null;
	}

	public void ToManaged(out object?[]? value)
	{
		throw null;
	}

	public void ToJS(object?[]? value)
	{
		throw null;
	}

	public void ToManaged(out JSObject? value)
	{
		throw null;
	}

	public void ToJS(JSObject? value)
	{
		throw null;
	}

	public void ToManaged(out JSObject?[]? value)
	{
		throw null;
	}

	public void ToJS(JSObject?[]? value)
	{
		throw null;
	}

	public void ToManaged(out Task? value)
	{
		throw null;
	}

	public void ToJS(Task? value)
	{
		throw null;
	}

	public void ToManaged<T>(out Task<T>? value, ArgumentToManagedCallback<T> marshaler)
	{
		throw null;
	}

	public void ToJS<T>(Task<T>? value, ArgumentToJSCallback<T> marshaler)
	{
		throw null;
	}

	public void ToManaged(out Action? value)
	{
		throw null;
	}

	public void ToJS(Action? value)
	{
		throw null;
	}

	public void ToManaged<T>(out Action<T>? value, ArgumentToJSCallback<T> arg1Marshaler)
	{
		throw null;
	}

	public void ToJS<T>(Action<T>? value, ArgumentToManagedCallback<T> arg1Marshaler)
	{
		throw null;
	}

	public void ToManaged<T1, T2>(out Action<T1, T2>? value, ArgumentToJSCallback<T1> arg1Marshaler, ArgumentToJSCallback<T2> arg2Marshaler)
	{
		throw null;
	}

	public void ToJS<T1, T2>(Action<T1, T2>? value, ArgumentToManagedCallback<T1> arg1Marshaler, ArgumentToManagedCallback<T2> arg2Marshaler)
	{
		throw null;
	}

	public void ToManaged<T1, T2, T3>(out Action<T1, T2, T3>? value, ArgumentToJSCallback<T1> arg1Marshaler, ArgumentToJSCallback<T2> arg2Marshaler, ArgumentToJSCallback<T3> arg3Marshaler)
	{
		throw null;
	}

	public void ToJS<T1, T2, T3>(Action<T1, T2, T3>? value, ArgumentToManagedCallback<T1> arg1Marshaler, ArgumentToManagedCallback<T2> arg2Marshaler, ArgumentToManagedCallback<T3> arg3Marshaler)
	{
		throw null;
	}

	public void ToManaged<TResult>(out Func<TResult>? value, ArgumentToManagedCallback<TResult> resMarshaler)
	{
		throw null;
	}

	public void ToJS<TResult>(Func<TResult>? value, ArgumentToJSCallback<TResult> resMarshaler)
	{
		throw null;
	}

	public void ToManaged<T, TResult>(out Func<T, TResult>? value, ArgumentToJSCallback<T> arg1Marshaler, ArgumentToManagedCallback<TResult> resMarshaler)
	{
		throw null;
	}

	public void ToJS<T, TResult>(Func<T, TResult>? value, ArgumentToManagedCallback<T> arg1Marshaler, ArgumentToJSCallback<TResult> resMarshaler)
	{
		throw null;
	}

	public void ToManaged<T1, T2, TResult>(out Func<T1, T2, TResult>? value, ArgumentToJSCallback<T1> arg1Marshaler, ArgumentToJSCallback<T2> arg2Marshaler, ArgumentToManagedCallback<TResult> resMarshaler)
	{
		throw null;
	}

	public void ToJS<T1, T2, TResult>(Func<T1, T2, TResult>? value, ArgumentToManagedCallback<T1> arg1Marshaler, ArgumentToManagedCallback<T2> arg2Marshaler, ArgumentToJSCallback<TResult> resMarshaler)
	{
		throw null;
	}

	public void ToManaged<T1, T2, T3, TResult>(out Func<T1, T2, T3, TResult>? value, ArgumentToJSCallback<T1> arg1Marshaler, ArgumentToJSCallback<T2> arg2Marshaler, ArgumentToJSCallback<T3> arg3Marshaler, ArgumentToManagedCallback<TResult> resMarshaler)
	{
		throw null;
	}

	public void ToJS<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult>? value, ArgumentToManagedCallback<T1> arg1Marshaler, ArgumentToManagedCallback<T2> arg2Marshaler, ArgumentToManagedCallback<T3> arg3Marshaler, ArgumentToJSCallback<TResult> resMarshaler)
	{
		throw null;
	}

	public unsafe void ToManaged(out void* value)
	{
		throw null;
	}

	public unsafe void ToJS(void* value)
	{
		throw null;
	}

	public void ToManaged(out Span<byte> value)
	{
		throw null;
	}

	public void ToJS(Span<byte> value)
	{
		throw null;
	}

	public void ToManaged(out ArraySegment<byte> value)
	{
		throw null;
	}

	public void ToJS(ArraySegment<byte> value)
	{
		throw null;
	}

	public void ToManaged(out Span<int> value)
	{
		throw null;
	}

	public void ToJS(Span<int> value)
	{
		throw null;
	}

	public void ToManaged(out Span<double> value)
	{
		throw null;
	}

	public void ToJS(Span<double> value)
	{
		throw null;
	}

	public void ToManaged(out ArraySegment<int> value)
	{
		throw null;
	}

	public void ToJS(ArraySegment<int> value)
	{
		throw null;
	}

	public void ToManaged(out ArraySegment<double> value)
	{
		throw null;
	}

	public void ToJS(ArraySegment<double> value)
	{
		throw null;
	}
}
