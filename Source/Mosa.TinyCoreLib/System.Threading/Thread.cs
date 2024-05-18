using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Versioning;
using System.Security.Principal;

namespace System.Threading;

public sealed class Thread : CriticalFinalizerObject
{
	[Obsolete("The ApartmentState property has been deprecated. Use GetApartmentState, SetApartmentState or TrySetApartmentState instead.")]
	public ApartmentState ApartmentState
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CultureInfo CurrentCulture
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static IPrincipal? CurrentPrincipal
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static Thread CurrentThread
	{
		get
		{
			throw null;
		}
	}

	public CultureInfo CurrentUICulture
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ExecutionContext? ExecutionContext
	{
		get
		{
			throw null;
		}
	}

	public bool IsAlive
	{
		get
		{
			throw null;
		}
	}

	public bool IsBackground
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsThreadPoolThread
	{
		get
		{
			throw null;
		}
	}

	public int ManagedThreadId
	{
		get
		{
			throw null;
		}
	}

	public string? Name
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ThreadPriority Priority
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ThreadState ThreadState
	{
		get
		{
			throw null;
		}
	}

	public Thread(ParameterizedThreadStart start)
	{
	}

	public Thread(ParameterizedThreadStart start, int maxStackSize)
	{
	}

	public Thread(ThreadStart start)
	{
	}

	public Thread(ThreadStart start, int maxStackSize)
	{
	}

	[Obsolete("Thread.Abort is not supported and throws PlatformNotSupportedException.", DiagnosticId = "SYSLIB0006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public void Abort()
	{
	}

	[Obsolete("Thread.Abort is not supported and throws PlatformNotSupportedException.", DiagnosticId = "SYSLIB0006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public void Abort(object? stateInfo)
	{
	}

	public static LocalDataStoreSlot AllocateDataSlot()
	{
		throw null;
	}

	public static LocalDataStoreSlot AllocateNamedDataSlot(string name)
	{
		throw null;
	}

	public static void BeginCriticalRegion()
	{
	}

	public static void BeginThreadAffinity()
	{
	}

	public void DisableComObjectEagerCleanup()
	{
	}

	public static void EndCriticalRegion()
	{
	}

	public static void EndThreadAffinity()
	{
	}

	~Thread()
	{
	}

	public static void FreeNamedDataSlot(string name)
	{
	}

	public ApartmentState GetApartmentState()
	{
		throw null;
	}

	[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public CompressedStack GetCompressedStack()
	{
		throw null;
	}

	public static int GetCurrentProcessorId()
	{
		throw null;
	}

	public static object? GetData(LocalDataStoreSlot slot)
	{
		throw null;
	}

	public static AppDomain GetDomain()
	{
		throw null;
	}

	public static int GetDomainID()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static LocalDataStoreSlot GetNamedDataSlot(string name)
	{
		throw null;
	}

	public void Interrupt()
	{
	}

	public void Join()
	{
	}

	public bool Join(int millisecondsTimeout)
	{
		throw null;
	}

	public bool Join(TimeSpan timeout)
	{
		throw null;
	}

	public static void MemoryBarrier()
	{
	}

	[Obsolete("Thread.ResetAbort is not supported and throws PlatformNotSupportedException.", DiagnosticId = "SYSLIB0006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static void ResetAbort()
	{
	}

	[Obsolete("Thread.Resume has been deprecated. Use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.")]
	public void Resume()
	{
	}

	[SupportedOSPlatform("windows")]
	public void SetApartmentState(ApartmentState state)
	{
	}

	[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public void SetCompressedStack(CompressedStack stack)
	{
	}

	public static void SetData(LocalDataStoreSlot slot, object? data)
	{
	}

	public static void Sleep(int millisecondsTimeout)
	{
	}

	public static void Sleep(TimeSpan timeout)
	{
	}

	public static void SpinWait(int iterations)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public void Start()
	{
	}

	[UnsupportedOSPlatform("browser")]
	public void Start(object? parameter)
	{
	}

	[Obsolete("Thread.Suspend has been deprecated. Use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.")]
	public void Suspend()
	{
	}

	public bool TrySetApartmentState(ApartmentState state)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public void UnsafeStart()
	{
	}

	[UnsupportedOSPlatform("browser")]
	public void UnsafeStart(object? parameter)
	{
	}

	public static byte VolatileRead(ref byte address)
	{
		throw null;
	}

	public static double VolatileRead(ref double address)
	{
		throw null;
	}

	public static short VolatileRead(ref short address)
	{
		throw null;
	}

	public static int VolatileRead(ref int address)
	{
		throw null;
	}

	public static long VolatileRead(ref long address)
	{
		throw null;
	}

	public static IntPtr VolatileRead(ref IntPtr address)
	{
		throw null;
	}

	[return: NotNullIfNotNull("address")]
	public static object? VolatileRead([NotNullIfNotNull("address")] ref object? address)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte VolatileRead(ref sbyte address)
	{
		throw null;
	}

	public static float VolatileRead(ref float address)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort VolatileRead(ref ushort address)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint VolatileRead(ref uint address)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong VolatileRead(ref ulong address)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static UIntPtr VolatileRead(ref UIntPtr address)
	{
		throw null;
	}

	public static void VolatileWrite(ref byte address, byte value)
	{
	}

	public static void VolatileWrite(ref double address, double value)
	{
	}

	public static void VolatileWrite(ref short address, short value)
	{
	}

	public static void VolatileWrite(ref int address, int value)
	{
	}

	public static void VolatileWrite(ref long address, long value)
	{
	}

	public static void VolatileWrite(ref IntPtr address, IntPtr value)
	{
	}

	public static void VolatileWrite([NotNullIfNotNull("value")] ref object? address, object? value)
	{
	}

	[CLSCompliant(false)]
	public static void VolatileWrite(ref sbyte address, sbyte value)
	{
	}

	public static void VolatileWrite(ref float address, float value)
	{
	}

	[CLSCompliant(false)]
	public static void VolatileWrite(ref ushort address, ushort value)
	{
	}

	[CLSCompliant(false)]
	public static void VolatileWrite(ref uint address, uint value)
	{
	}

	[CLSCompliant(false)]
	public static void VolatileWrite(ref ulong address, ulong value)
	{
	}

	[CLSCompliant(false)]
	public static void VolatileWrite(ref UIntPtr address, UIntPtr value)
	{
	}

	public static bool Yield()
	{
		throw null;
	}
}
