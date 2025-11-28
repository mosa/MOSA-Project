using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.DataAnnotations;

public sealed class ValidationContext : IServiceProvider
{
	public string DisplayName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IDictionary<object, object?> Items
	{
		get
		{
			throw null;
		}
	}

	public string? MemberName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public object ObjectInstance
	{
		get
		{
			throw null;
		}
	}

	public Type ObjectType
	{
		get
		{
			throw null;
		}
	}

	[RequiresUnreferencedCode("The Type of instance cannot be statically discovered and the Type's properties can be trimmed.")]
	public ValidationContext(object instance)
	{
	}

	[RequiresUnreferencedCode("The Type of instance cannot be statically discovered and the Type's properties can be trimmed.")]
	public ValidationContext(object instance, IDictionary<object, object?>? items)
	{
	}

	[RequiresUnreferencedCode("The Type of instance cannot be statically discovered and the Type's properties can be trimmed.")]
	public ValidationContext(object instance, IServiceProvider? serviceProvider, IDictionary<object, object?>? items)
	{
	}

	public object? GetService(Type serviceType)
	{
		throw null;
	}

	public void InitializeServiceProvider(Func<Type, object?> serviceProvider)
	{
	}
}
