using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Class)]
public class RunInstallerAttribute : Attribute
{
	public static readonly RunInstallerAttribute Default;

	public static readonly RunInstallerAttribute No;

	public static readonly RunInstallerAttribute Yes;

	public bool RunInstaller
	{
		get
		{
			throw null;
		}
	}

	public RunInstallerAttribute(bool runInstaller)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override bool IsDefaultAttribute()
	{
		throw null;
	}
}
