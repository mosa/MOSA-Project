﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;
using Mosa.Kernel.BareMetal.Intel;
using Mosa.Runtime;
using Mosa.Runtime.x86;

namespace Mosa.Kernel.BareMetal.x86;

public static class Scheduler
{
	public static class IRQ
	{
		public const int SystemCall = 253;
		public const int Clock = 0x20;
		public const int ThreadTermination = 254;
	}

	public static void Start()
	{
		Native.Int(IRQ.Clock);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Yield()
	{
		Native.Hlt();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SignalTermination()
	{
		Native.Int(IRQ.ThreadTermination);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static object SignalSystemCall(object obj, uint senderID, uint receiverID)
	{
		Native.Int3(IRQ.SystemCall, Intrinsic.GetObjectAddress(obj).ToUInt32(), receiverID, senderID);

		return Native.GetEAX();
	}

	public static Pointer SetupThreadStack(Pointer stackTop, Pointer methodAddress, Pointer termAddress)
	{
		Debug.WriteLine("x86.Scheduler:SetupThreadStack()");

		// Setup stack state
		stackTop.Store32(-4, 0);          // Zero Sentinel
		stackTop.Store32(-8, termAddress.ToInt32());  // Address of method that will raise a interrupt signal to terminate thread

		stackTop.Store32(-12, 0x00000202);// EFLAG
		stackTop.Store32(-16, 0x08);      // CS
		stackTop.Store32(-20, methodAddress.ToInt32()); // EIP

		stackTop.Store32(-24, 0);     // ErrorCode - not used
		stackTop.Store32(-28, 0);     // Interrupt Number - not used

		stackTop.Store32(-32, 0);     // EAX
		stackTop.Store32(-36, 0);     // ECX
		stackTop.Store32(-40, 0);     // EDX
		stackTop.Store32(-44, 0);     // EBX
		stackTop.Store32(-48, 0);     // ESP (original) - not used
		stackTop.Store32(-52, (stackTop - 8).ToInt32()); // EBP
		stackTop.Store32(-56, 0);     // ESI
		stackTop.Store32(-60, 0);     // EDI

		Debug.WriteLine("x86.Scheduler:SetupThreadStack() [Exit]");

		return stackTop - 60;
	}

	public static void SwitchToThread(Thread thread)
	{
		PIC.SendEndOfInterrupt(IRQ.Clock);

		Native.InterruptReturn((uint)thread.StackStatePointer.ToInt32());
	}

	public static void SetReturnObject(Pointer stackTop, Pointer ob)
	{
		stackTop.Store32(-32, ob.ToInt32());    // EAX
	}
}
