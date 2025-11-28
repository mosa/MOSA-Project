namespace System.Diagnostics;

public enum ThreadWaitReason
{
	Executive,
	FreePage,
	PageIn,
	SystemAllocation,
	ExecutionDelay,
	Suspended,
	UserRequest,
	EventPairHigh,
	EventPairLow,
	LpcReceive,
	LpcReply,
	VirtualMemory,
	PageOut,
	Unknown
}
