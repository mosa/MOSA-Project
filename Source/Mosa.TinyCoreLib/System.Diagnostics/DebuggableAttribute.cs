namespace System.Diagnostics;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module, AllowMultiple = false)]
public sealed class DebuggableAttribute : Attribute
{
	[Flags]
	public enum DebuggingModes
	{
		None = 0,
		Default = 1,
		IgnoreSymbolStoreSequencePoints = 2,
		EnableEditAndContinue = 4,
		DisableOptimizations = 0x100
	}

	public DebuggingModes DebuggingFlags
	{
		get
		{
			throw null;
		}
	}

	public bool IsJITOptimizerDisabled
	{
		get
		{
			throw null;
		}
	}

	public bool IsJITTrackingEnabled
	{
		get
		{
			throw null;
		}
	}

	public DebuggableAttribute(bool isJITTrackingEnabled, bool isJITOptimizerDisabled)
	{
	}

	public DebuggableAttribute(DebuggingModes modes)
	{
	}
}
