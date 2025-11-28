using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public sealed class DebuggerVisualizerAttribute : Attribute
{
	public string? Description
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Type? Target
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? TargetTypeName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
	public string? VisualizerObjectSourceTypeName
	{
		get
		{
			throw null;
		}
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
	public string VisualizerTypeName
	{
		get
		{
			throw null;
		}
	}

	public DebuggerVisualizerAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] string visualizerTypeName)
	{
	}

	public DebuggerVisualizerAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] string visualizerTypeName, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] string? visualizerObjectSourceTypeName)
	{
	}

	public DebuggerVisualizerAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] string visualizerTypeName, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type visualizerObjectSource)
	{
	}

	public DebuggerVisualizerAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type visualizer)
	{
	}

	public DebuggerVisualizerAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type visualizer, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] string? visualizerObjectSourceTypeName)
	{
	}

	public DebuggerVisualizerAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type visualizer, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type visualizerObjectSource)
	{
	}
}
