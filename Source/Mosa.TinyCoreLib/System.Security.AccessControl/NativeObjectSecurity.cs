using System.Runtime.InteropServices;

namespace System.Security.AccessControl;

public abstract class NativeObjectSecurity : CommonObjectSecurity
{
	protected internal delegate Exception? ExceptionFromErrorCode(int errorCode, string? name, SafeHandle? handle, object? context);

	protected NativeObjectSecurity(bool isContainer, ResourceType resourceType)
		: base(isContainer: false)
	{
	}

	protected NativeObjectSecurity(bool isContainer, ResourceType resourceType, SafeHandle? handle, AccessControlSections includeSections)
		: base(isContainer: false)
	{
	}

	protected NativeObjectSecurity(bool isContainer, ResourceType resourceType, SafeHandle? handle, AccessControlSections includeSections, ExceptionFromErrorCode? exceptionFromErrorCode, object? exceptionContext)
		: base(isContainer: false)
	{
	}

	protected NativeObjectSecurity(bool isContainer, ResourceType resourceType, ExceptionFromErrorCode? exceptionFromErrorCode, object? exceptionContext)
		: base(isContainer: false)
	{
	}

	protected NativeObjectSecurity(bool isContainer, ResourceType resourceType, string? name, AccessControlSections includeSections)
		: base(isContainer: false)
	{
	}

	protected NativeObjectSecurity(bool isContainer, ResourceType resourceType, string? name, AccessControlSections includeSections, ExceptionFromErrorCode? exceptionFromErrorCode, object? exceptionContext)
		: base(isContainer: false)
	{
	}

	protected sealed override void Persist(SafeHandle handle, AccessControlSections includeSections)
	{
	}

	protected void Persist(SafeHandle handle, AccessControlSections includeSections, object? exceptionContext)
	{
	}

	protected sealed override void Persist(string name, AccessControlSections includeSections)
	{
	}

	protected void Persist(string name, AccessControlSections includeSections, object? exceptionContext)
	{
	}
}
