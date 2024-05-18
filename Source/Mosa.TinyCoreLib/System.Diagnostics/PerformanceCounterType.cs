namespace System.Diagnostics;

public enum PerformanceCounterType
{
	NumberOfItemsHEX32 = 0,
	NumberOfItemsHEX64 = 256,
	NumberOfItems32 = 65536,
	NumberOfItems64 = 65792,
	CounterDelta32 = 4195328,
	CounterDelta64 = 4195584,
	SampleCounter = 4260864,
	CountPerTimeInterval32 = 4523008,
	CountPerTimeInterval64 = 4523264,
	RateOfCountsPerSecond32 = 272696320,
	RateOfCountsPerSecond64 = 272696576,
	RawFraction = 537003008,
	CounterTimer = 541132032,
	Timer100Ns = 542180608,
	SampleFraction = 549585920,
	CounterTimerInverse = 557909248,
	Timer100NsInverse = 558957824,
	CounterMultiTimer = 574686464,
	CounterMultiTimer100Ns = 575735040,
	CounterMultiTimerInverse = 591463680,
	CounterMultiTimer100NsInverse = 592512256,
	AverageTimer32 = 805438464,
	ElapsedTime = 807666944,
	AverageCount64 = 1073874176,
	SampleBase = 1073939457,
	AverageBase = 1073939458,
	RawBase = 1073939459,
	CounterMultiBase = 1107494144
}
