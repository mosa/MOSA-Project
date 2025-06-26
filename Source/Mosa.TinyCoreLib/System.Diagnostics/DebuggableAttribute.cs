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

	public DebuggingModes DebuggingFlags => throw new NotImplementedException();

	public bool IsJITOptimizerDisabled => throw new NotImplementedException();

	public bool IsJITTrackingEnabled => throw new NotImplementedException();

	public DebuggableAttribute(bool isJITTrackingEnabled, bool isJITOptimizerDisabled)
		=> throw new NotImplementedException();

	public DebuggableAttribute(DebuggingModes modes) {}
}
