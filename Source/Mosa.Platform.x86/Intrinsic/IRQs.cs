// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	//private static readonly string DefaultInterruptMethodName = "Mosa.Kernel.x86.IDT::ProcessInterrupt";
	private static readonly string DefaultInterruptMethodName = "Mosa.Runtime.Interrupt::Process";

	private static readonly string[] seperator = new string[] { "::" };

	private static void InsertIRQ(int irq, Context context, TransformContext transformContext)
	{
		var interruptMethodName = transformContext.Compiler.MosaSettings.Settings.GetValue("X86.InterruptMethodName", DefaultInterruptMethodName);

		if (string.IsNullOrEmpty(interruptMethodName))
		{
			interruptMethodName = DefaultInterruptMethodName;
		}

		var ar = interruptMethodName.Split(seperator, System.StringSplitOptions.None);

		if (ar.Length != 2)
			return;

		var typeFullname = ar[0];
		var methodName = ar[1];

		var type = transformContext.TypeSystem.GetTypeByName(typeFullname);

		if (type == null)
			return;

		var method = type.FindMethodByName(methodName);

		if (method == null)
			return;

		var plugMethod = transformContext.Compiler.PlugSystem.GetReplacement(method);

		if (plugMethod != null)
		{
			method = plugMethod;
		}

		transformContext.MethodScanner.MethodInvoked(method, transformContext.Method);

		var interrupt = Operand.CreateLabel(method, transformContext.Is32BitPlatform);

		var esp = Operand.CreateCPURegister32(CPURegister.ESP);

		context.SetInstruction(X86.Cli);
		if (irq <= 7 || irq >= 16 | irq == 9) // For IRQ 8, 10, 11, 12, 13, 14 the cpu will automatically pushed the error code
		{
			context.AppendInstruction(X86.Push32, null, Operand.Constant32_0);
		}
		context.AppendInstruction(X86.Push32, null, Operand.CreateConstant32(irq));
		context.AppendInstruction(X86.Pushad);
		context.AppendInstruction(X86.Push32, null, esp);
		context.AppendInstruction(X86.Call, null, interrupt);
		context.AppendInstruction(X86.Pop32, esp);
		context.AppendInstruction(X86.Popad);
		context.AppendInstruction(X86.Add32, esp, esp, Operand.Constant32_8);
		context.AppendInstruction(X86.Sti);
		context.AppendInstruction(X86.IRetd);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ0")]
	private static void IRQ0(Context context, TransformContext transformContext)
	{
		InsertIRQ(0, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ1")]
	private static void IRQ1(Context context, TransformContext transformContext)
	{
		InsertIRQ(1, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ2")]
	private static void IRQ2(Context context, TransformContext transformContext)
	{
		InsertIRQ(2, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ3")]
	private static void IRQ3(Context context, TransformContext transformContext)
	{
		InsertIRQ(3, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ4")]
	private static void IRQ4(Context context, TransformContext transformContext)
	{
		InsertIRQ(4, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ5")]
	private static void IRQ5(Context context, TransformContext transformContext)
	{
		InsertIRQ(5, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ6")]
	private static void IRQ6(Context context, TransformContext transformContext)
	{
		InsertIRQ(6, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ7")]
	private static void IRQ7(Context context, TransformContext transformContext)
	{
		InsertIRQ(7, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ8")]
	private static void IRQ8(Context context, TransformContext transformContext)
	{
		InsertIRQ(8, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ9")]
	private static void IRQ9(Context context, TransformContext transformContext)
	{
		InsertIRQ(9, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ10")]
	private static void IRQ10(Context context, TransformContext transformContext)
	{
		InsertIRQ(10, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ11")]
	private static void IRQ11(Context context, TransformContext transformContext)
	{
		InsertIRQ(11, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ12")]
	private static void IRQ12(Context context, TransformContext transformContext)
	{
		InsertIRQ(12, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ13")]
	private static void IRQ13(Context context, TransformContext transformContext)
	{
		InsertIRQ(13, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ14")]
	private static void IRQ14(Context context, TransformContext transformContext)
	{
		InsertIRQ(14, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ15")]
	private static void IRQ15(Context context, TransformContext transformContext)
	{
		InsertIRQ(15, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ16")]
	private static void IRQ16(Context context, TransformContext transformContext)
	{
		InsertIRQ(16, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ17")]
	private static void IRQ17(Context context, TransformContext transformContext)
	{
		InsertIRQ(17, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ18")]
	private static void IRQ18(Context context, TransformContext transformContext)
	{
		InsertIRQ(18, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ19")]
	private static void IRQ19(Context context, TransformContext transformContext)
	{
		InsertIRQ(19, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ20")]
	private static void IRQ20(Context context, TransformContext transformContext)
	{
		InsertIRQ(20, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ21")]
	private static void IRQ21(Context context, TransformContext transformContext)
	{
		InsertIRQ(21, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ22")]
	private static void IRQ22(Context context, TransformContext transformContext)
	{
		InsertIRQ(22, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ23")]
	private static void IRQ23(Context context, TransformContext transformContext)
	{
		InsertIRQ(23, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ24")]
	private static void IRQ24(Context context, TransformContext transformContext)
	{
		InsertIRQ(24, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ25")]
	private static void IRQ25(Context context, TransformContext transformContext)
	{
		InsertIRQ(25, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ26")]
	private static void IRQ26(Context context, TransformContext transformContext)
	{
		InsertIRQ(26, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ27")]
	private static void IRQ27(Context context, TransformContext transformContext)
	{
		InsertIRQ(27, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ28")]
	private static void IRQ28(Context context, TransformContext transformContext)
	{
		InsertIRQ(28, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ29")]
	private static void IRQ29(Context context, TransformContext transformContext)
	{
		InsertIRQ(29, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ30")]
	private static void IRQ30(Context context, TransformContext transformContext)
	{
		InsertIRQ(30, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ31")]
	private static void IRQ31(Context context, TransformContext transformContext)
	{
		InsertIRQ(31, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ32")]
	private static void IRQ32(Context context, TransformContext transformContext)
	{
		InsertIRQ(32, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ33")]
	private static void IRQ33(Context context, TransformContext transformContext)
	{
		InsertIRQ(33, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ34")]
	private static void IRQ34(Context context, TransformContext transformContext)
	{
		InsertIRQ(34, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ35")]
	private static void IRQ35(Context context, TransformContext transformContext)
	{
		InsertIRQ(35, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ36")]
	private static void IRQ36(Context context, TransformContext transformContext)
	{
		InsertIRQ(36, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ37")]
	private static void IRQ37(Context context, TransformContext transformContext)
	{
		InsertIRQ(37, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ38")]
	private static void IRQ38(Context context, TransformContext transformContext)
	{
		InsertIRQ(38, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ39")]
	private static void IRQ39(Context context, TransformContext transformContext)
	{
		InsertIRQ(39, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ40")]
	private static void IRQ40(Context context, TransformContext transformContext)
	{
		InsertIRQ(40, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ41")]
	private static void IRQ41(Context context, TransformContext transformContext)
	{
		InsertIRQ(41, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ42")]
	private static void IRQ42(Context context, TransformContext transformContext)
	{
		InsertIRQ(42, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ43")]
	private static void IRQ43(Context context, TransformContext transformContext)
	{
		InsertIRQ(43, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ44")]
	private static void IRQ44(Context context, TransformContext transformContext)
	{
		InsertIRQ(44, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ45")]
	private static void IRQ45(Context context, TransformContext transformContext)
	{
		InsertIRQ(45, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ46")]
	private static void IRQ46(Context context, TransformContext transformContext)
	{
		InsertIRQ(46, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ47")]
	private static void IRQ47(Context context, TransformContext transformContext)
	{
		InsertIRQ(47, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ48")]
	private static void IRQ48(Context context, TransformContext transformContext)
	{
		InsertIRQ(48, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ49")]
	private static void IRQ49(Context context, TransformContext transformContext)
	{
		InsertIRQ(49, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ50")]
	private static void IRQ50(Context context, TransformContext transformContext)
	{
		InsertIRQ(50, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ51")]
	private static void IRQ51(Context context, TransformContext transformContext)
	{
		InsertIRQ(51, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ52")]
	private static void IRQ52(Context context, TransformContext transformContext)
	{
		InsertIRQ(52, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ53")]
	private static void IRQ53(Context context, TransformContext transformContext)
	{
		InsertIRQ(53, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ54")]
	private static void IRQ54(Context context, TransformContext transformContext)
	{
		InsertIRQ(54, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ55")]
	private static void IRQ55(Context context, TransformContext transformContext)
	{
		InsertIRQ(55, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ56")]
	private static void IRQ56(Context context, TransformContext transformContext)
	{
		InsertIRQ(56, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ57")]
	private static void IRQ57(Context context, TransformContext transformContext)
	{
		InsertIRQ(57, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ58")]
	private static void IRQ58(Context context, TransformContext transformContext)
	{
		InsertIRQ(58, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ59")]
	private static void IRQ59(Context context, TransformContext transformContext)
	{
		InsertIRQ(59, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ60")]
	private static void IRQ60(Context context, TransformContext transformContext)
	{
		InsertIRQ(60, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ61")]
	private static void IRQ61(Context context, TransformContext transformContext)
	{
		InsertIRQ(61, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ62")]
	private static void IRQ62(Context context, TransformContext transformContext)
	{
		InsertIRQ(62, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ63")]
	private static void IRQ63(Context context, TransformContext transformContext)
	{
		InsertIRQ(63, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ64")]
	private static void IRQ64(Context context, TransformContext transformContext)
	{
		InsertIRQ(64, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ65")]
	private static void IRQ65(Context context, TransformContext transformContext)
	{
		InsertIRQ(65, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ66")]
	private static void IRQ66(Context context, TransformContext transformContext)
	{
		InsertIRQ(66, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ67")]
	private static void IRQ67(Context context, TransformContext transformContext)
	{
		InsertIRQ(67, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ68")]
	private static void IRQ68(Context context, TransformContext transformContext)
	{
		InsertIRQ(68, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ69")]
	private static void IRQ69(Context context, TransformContext transformContext)
	{
		InsertIRQ(69, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ70")]
	private static void IRQ70(Context context, TransformContext transformContext)
	{
		InsertIRQ(70, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ71")]
	private static void IRQ71(Context context, TransformContext transformContext)
	{
		InsertIRQ(71, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ72")]
	private static void IRQ72(Context context, TransformContext transformContext)
	{
		InsertIRQ(72, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ73")]
	private static void IRQ73(Context context, TransformContext transformContext)
	{
		InsertIRQ(73, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ74")]
	private static void IRQ74(Context context, TransformContext transformContext)
	{
		InsertIRQ(74, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ75")]
	private static void IRQ75(Context context, TransformContext transformContext)
	{
		InsertIRQ(75, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ76")]
	private static void IRQ76(Context context, TransformContext transformContext)
	{
		InsertIRQ(76, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ77")]
	private static void IRQ77(Context context, TransformContext transformContext)
	{
		InsertIRQ(77, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ78")]
	private static void IRQ78(Context context, TransformContext transformContext)
	{
		InsertIRQ(78, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ79")]
	private static void IRQ79(Context context, TransformContext transformContext)
	{
		InsertIRQ(79, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ80")]
	private static void IRQ80(Context context, TransformContext transformContext)
	{
		InsertIRQ(80, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ81")]
	private static void IRQ81(Context context, TransformContext transformContext)
	{
		InsertIRQ(81, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ82")]
	private static void IRQ82(Context context, TransformContext transformContext)
	{
		InsertIRQ(82, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ83")]
	private static void IRQ83(Context context, TransformContext transformContext)
	{
		InsertIRQ(83, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ84")]
	private static void IRQ84(Context context, TransformContext transformContext)
	{
		InsertIRQ(84, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ85")]
	private static void IRQ85(Context context, TransformContext transformContext)
	{
		InsertIRQ(85, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ86")]
	private static void IRQ86(Context context, TransformContext transformContext)
	{
		InsertIRQ(86, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ87")]
	private static void IRQ87(Context context, TransformContext transformContext)
	{
		InsertIRQ(87, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ88")]
	private static void IRQ88(Context context, TransformContext transformContext)
	{
		InsertIRQ(88, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ89")]
	private static void IRQ89(Context context, TransformContext transformContext)
	{
		InsertIRQ(89, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ90")]
	private static void IRQ90(Context context, TransformContext transformContext)
	{
		InsertIRQ(90, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ91")]
	private static void IRQ91(Context context, TransformContext transformContext)
	{
		InsertIRQ(91, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ92")]
	private static void IRQ92(Context context, TransformContext transformContext)
	{
		InsertIRQ(92, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ93")]
	private static void IRQ93(Context context, TransformContext transformContext)
	{
		InsertIRQ(93, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ94")]
	private static void IRQ94(Context context, TransformContext transformContext)
	{
		InsertIRQ(94, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ95")]
	private static void IRQ95(Context context, TransformContext transformContext)
	{
		InsertIRQ(95, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ96")]
	private static void IRQ96(Context context, TransformContext transformContext)
	{
		InsertIRQ(96, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ97")]
	private static void IRQ97(Context context, TransformContext transformContext)
	{
		InsertIRQ(97, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ98")]
	private static void IRQ98(Context context, TransformContext transformContext)
	{
		InsertIRQ(98, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ99")]
	private static void IRQ99(Context context, TransformContext transformContext)
	{
		InsertIRQ(99, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ100")]
	private static void IRQ100(Context context, TransformContext transformContext)
	{
		InsertIRQ(100, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ101")]
	private static void IRQ101(Context context, TransformContext transformContext)
	{
		InsertIRQ(101, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ102")]
	private static void IRQ102(Context context, TransformContext transformContext)
	{
		InsertIRQ(102, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ103")]
	private static void IRQ103(Context context, TransformContext transformContext)
	{
		InsertIRQ(103, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ104")]
	private static void IRQ104(Context context, TransformContext transformContext)
	{
		InsertIRQ(104, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ105")]
	private static void IRQ105(Context context, TransformContext transformContext)
	{
		InsertIRQ(105, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ106")]
	private static void IRQ106(Context context, TransformContext transformContext)
	{
		InsertIRQ(106, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ107")]
	private static void IRQ107(Context context, TransformContext transformContext)
	{
		InsertIRQ(107, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ108")]
	private static void IRQ108(Context context, TransformContext transformContext)
	{
		InsertIRQ(108, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ109")]
	private static void IRQ109(Context context, TransformContext transformContext)
	{
		InsertIRQ(109, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ110")]
	private static void IRQ110(Context context, TransformContext transformContext)
	{
		InsertIRQ(110, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ111")]
	private static void IRQ111(Context context, TransformContext transformContext)
	{
		InsertIRQ(111, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ112")]
	private static void IRQ112(Context context, TransformContext transformContext)
	{
		InsertIRQ(112, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ113")]
	private static void IRQ113(Context context, TransformContext transformContext)
	{
		InsertIRQ(113, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ114")]
	private static void IRQ114(Context context, TransformContext transformContext)
	{
		InsertIRQ(114, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ115")]
	private static void IRQ115(Context context, TransformContext transformContext)
	{
		InsertIRQ(115, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ116")]
	private static void IRQ116(Context context, TransformContext transformContext)
	{
		InsertIRQ(116, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ117")]
	private static void IRQ117(Context context, TransformContext transformContext)
	{
		InsertIRQ(117, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ118")]
	private static void IRQ118(Context context, TransformContext transformContext)
	{
		InsertIRQ(118, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ119")]
	private static void IRQ119(Context context, TransformContext transformContext)
	{
		InsertIRQ(119, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ120")]
	private static void IRQ120(Context context, TransformContext transformContext)
	{
		InsertIRQ(120, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ121")]
	private static void IRQ121(Context context, TransformContext transformContext)
	{
		InsertIRQ(121, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ122")]
	private static void IRQ122(Context context, TransformContext transformContext)
	{
		InsertIRQ(122, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ123")]
	private static void IRQ123(Context context, TransformContext transformContext)
	{
		InsertIRQ(123, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ124")]
	private static void IRQ124(Context context, TransformContext transformContext)
	{
		InsertIRQ(124, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ125")]
	private static void IRQ125(Context context, TransformContext transformContext)
	{
		InsertIRQ(125, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ126")]
	private static void IRQ126(Context context, TransformContext transformContext)
	{
		InsertIRQ(126, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ127")]
	private static void IRQ127(Context context, TransformContext transformContext)
	{
		InsertIRQ(127, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ128")]
	private static void IRQ128(Context context, TransformContext transformContext)
	{
		InsertIRQ(128, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ129")]
	private static void IRQ129(Context context, TransformContext transformContext)
	{
		InsertIRQ(129, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ130")]
	private static void IRQ130(Context context, TransformContext transformContext)
	{
		InsertIRQ(130, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ131")]
	private static void IRQ131(Context context, TransformContext transformContext)
	{
		InsertIRQ(131, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ132")]
	private static void IRQ132(Context context, TransformContext transformContext)
	{
		InsertIRQ(132, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ133")]
	private static void IRQ133(Context context, TransformContext transformContext)
	{
		InsertIRQ(133, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ134")]
	private static void IRQ134(Context context, TransformContext transformContext)
	{
		InsertIRQ(134, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ135")]
	private static void IRQ135(Context context, TransformContext transformContext)
	{
		InsertIRQ(135, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ136")]
	private static void IRQ136(Context context, TransformContext transformContext)
	{
		InsertIRQ(136, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ137")]
	private static void IRQ137(Context context, TransformContext transformContext)
	{
		InsertIRQ(137, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ138")]
	private static void IRQ138(Context context, TransformContext transformContext)
	{
		InsertIRQ(138, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ139")]
	private static void IRQ139(Context context, TransformContext transformContext)
	{
		InsertIRQ(139, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ140")]
	private static void IRQ140(Context context, TransformContext transformContext)
	{
		InsertIRQ(140, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ141")]
	private static void IRQ141(Context context, TransformContext transformContext)
	{
		InsertIRQ(141, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ142")]
	private static void IRQ142(Context context, TransformContext transformContext)
	{
		InsertIRQ(142, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ143")]
	private static void IRQ143(Context context, TransformContext transformContext)
	{
		InsertIRQ(143, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ144")]
	private static void IRQ144(Context context, TransformContext transformContext)
	{
		InsertIRQ(144, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ145")]
	private static void IRQ145(Context context, TransformContext transformContext)
	{
		InsertIRQ(145, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ146")]
	private static void IRQ146(Context context, TransformContext transformContext)
	{
		InsertIRQ(146, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ147")]
	private static void IRQ147(Context context, TransformContext transformContext)
	{
		InsertIRQ(147, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ148")]
	private static void IRQ148(Context context, TransformContext transformContext)
	{
		InsertIRQ(148, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ149")]
	private static void IRQ149(Context context, TransformContext transformContext)
	{
		InsertIRQ(149, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ150")]
	private static void IRQ150(Context context, TransformContext transformContext)
	{
		InsertIRQ(150, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ151")]
	private static void IRQ151(Context context, TransformContext transformContext)
	{
		InsertIRQ(151, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ152")]
	private static void IRQ152(Context context, TransformContext transformContext)
	{
		InsertIRQ(152, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ153")]
	private static void IRQ153(Context context, TransformContext transformContext)
	{
		InsertIRQ(153, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ154")]
	private static void IRQ154(Context context, TransformContext transformContext)
	{
		InsertIRQ(154, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ155")]
	private static void IRQ155(Context context, TransformContext transformContext)
	{
		InsertIRQ(155, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ156")]
	private static void IRQ156(Context context, TransformContext transformContext)
	{
		InsertIRQ(156, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ157")]
	private static void IRQ157(Context context, TransformContext transformContext)
	{
		InsertIRQ(157, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ158")]
	private static void IRQ158(Context context, TransformContext transformContext)
	{
		InsertIRQ(158, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ159")]
	private static void IRQ159(Context context, TransformContext transformContext)
	{
		InsertIRQ(159, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ160")]
	private static void IRQ160(Context context, TransformContext transformContext)
	{
		InsertIRQ(160, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ161")]
	private static void IRQ161(Context context, TransformContext transformContext)
	{
		InsertIRQ(161, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ162")]
	private static void IRQ162(Context context, TransformContext transformContext)
	{
		InsertIRQ(162, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ163")]
	private static void IRQ163(Context context, TransformContext transformContext)
	{
		InsertIRQ(163, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ164")]
	private static void IRQ164(Context context, TransformContext transformContext)
	{
		InsertIRQ(164, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ165")]
	private static void IRQ165(Context context, TransformContext transformContext)
	{
		InsertIRQ(165, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ166")]
	private static void IRQ166(Context context, TransformContext transformContext)
	{
		InsertIRQ(166, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ167")]
	private static void IRQ167(Context context, TransformContext transformContext)
	{
		InsertIRQ(167, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ168")]
	private static void IRQ168(Context context, TransformContext transformContext)
	{
		InsertIRQ(168, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ169")]
	private static void IRQ169(Context context, TransformContext transformContext)
	{
		InsertIRQ(169, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ170")]
	private static void IRQ170(Context context, TransformContext transformContext)
	{
		InsertIRQ(170, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ171")]
	private static void IRQ171(Context context, TransformContext transformContext)
	{
		InsertIRQ(171, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ172")]
	private static void IRQ172(Context context, TransformContext transformContext)
	{
		InsertIRQ(172, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ173")]
	private static void IRQ173(Context context, TransformContext transformContext)
	{
		InsertIRQ(173, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ174")]
	private static void IRQ174(Context context, TransformContext transformContext)
	{
		InsertIRQ(174, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ175")]
	private static void IRQ175(Context context, TransformContext transformContext)
	{
		InsertIRQ(175, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ176")]
	private static void IRQ176(Context context, TransformContext transformContext)
	{
		InsertIRQ(176, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ177")]
	private static void IRQ177(Context context, TransformContext transformContext)
	{
		InsertIRQ(177, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ178")]
	private static void IRQ178(Context context, TransformContext transformContext)
	{
		InsertIRQ(178, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ179")]
	private static void IRQ179(Context context, TransformContext transformContext)
	{
		InsertIRQ(179, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ180")]
	private static void IRQ180(Context context, TransformContext transformContext)
	{
		InsertIRQ(180, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ181")]
	private static void IRQ181(Context context, TransformContext transformContext)
	{
		InsertIRQ(181, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ182")]
	private static void IRQ182(Context context, TransformContext transformContext)
	{
		InsertIRQ(182, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ183")]
	private static void IRQ183(Context context, TransformContext transformContext)
	{
		InsertIRQ(183, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ184")]
	private static void IRQ184(Context context, TransformContext transformContext)
	{
		InsertIRQ(184, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ185")]
	private static void IRQ185(Context context, TransformContext transformContext)
	{
		InsertIRQ(185, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ186")]
	private static void IRQ186(Context context, TransformContext transformContext)
	{
		InsertIRQ(186, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ187")]
	private static void IRQ187(Context context, TransformContext transformContext)
	{
		InsertIRQ(187, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ188")]
	private static void IRQ188(Context context, TransformContext transformContext)
	{
		InsertIRQ(188, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ189")]
	private static void IRQ189(Context context, TransformContext transformContext)
	{
		InsertIRQ(189, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ190")]
	private static void IRQ190(Context context, TransformContext transformContext)
	{
		InsertIRQ(190, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ191")]
	private static void IRQ191(Context context, TransformContext transformContext)
	{
		InsertIRQ(191, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ192")]
	private static void IRQ192(Context context, TransformContext transformContext)
	{
		InsertIRQ(192, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ193")]
	private static void IRQ193(Context context, TransformContext transformContext)
	{
		InsertIRQ(193, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ194")]
	private static void IRQ194(Context context, TransformContext transformContext)
	{
		InsertIRQ(194, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ195")]
	private static void IRQ195(Context context, TransformContext transformContext)
	{
		InsertIRQ(195, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ196")]
	private static void IRQ196(Context context, TransformContext transformContext)
	{
		InsertIRQ(196, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ197")]
	private static void IRQ197(Context context, TransformContext transformContext)
	{
		InsertIRQ(197, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ198")]
	private static void IRQ198(Context context, TransformContext transformContext)
	{
		InsertIRQ(198, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ199")]
	private static void IRQ199(Context context, TransformContext transformContext)
	{
		InsertIRQ(199, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ200")]
	private static void IRQ200(Context context, TransformContext transformContext)
	{
		InsertIRQ(200, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ201")]
	private static void IRQ201(Context context, TransformContext transformContext)
	{
		InsertIRQ(201, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ202")]
	private static void IRQ202(Context context, TransformContext transformContext)
	{
		InsertIRQ(202, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ203")]
	private static void IRQ203(Context context, TransformContext transformContext)
	{
		InsertIRQ(203, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ204")]
	private static void IRQ204(Context context, TransformContext transformContext)
	{
		InsertIRQ(204, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ205")]
	private static void IRQ205(Context context, TransformContext transformContext)
	{
		InsertIRQ(205, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ206")]
	private static void IRQ206(Context context, TransformContext transformContext)
	{
		InsertIRQ(206, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ207")]
	private static void IRQ207(Context context, TransformContext transformContext)
	{
		InsertIRQ(207, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ208")]
	private static void IRQ208(Context context, TransformContext transformContext)
	{
		InsertIRQ(208, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ209")]
	private static void IRQ209(Context context, TransformContext transformContext)
	{
		InsertIRQ(209, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ210")]
	private static void IRQ210(Context context, TransformContext transformContext)
	{
		InsertIRQ(210, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ211")]
	private static void IRQ211(Context context, TransformContext transformContext)
	{
		InsertIRQ(211, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ212")]
	private static void IRQ212(Context context, TransformContext transformContext)
	{
		InsertIRQ(212, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ213")]
	private static void IRQ213(Context context, TransformContext transformContext)
	{
		InsertIRQ(213, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ214")]
	private static void IRQ214(Context context, TransformContext transformContext)
	{
		InsertIRQ(214, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ215")]
	private static void IRQ215(Context context, TransformContext transformContext)
	{
		InsertIRQ(215, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ216")]
	private static void IRQ216(Context context, TransformContext transformContext)
	{
		InsertIRQ(216, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ217")]
	private static void IRQ217(Context context, TransformContext transformContext)
	{
		InsertIRQ(217, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ218")]
	private static void IRQ218(Context context, TransformContext transformContext)
	{
		InsertIRQ(218, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ219")]
	private static void IRQ219(Context context, TransformContext transformContext)
	{
		InsertIRQ(219, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ220")]
	private static void IRQ220(Context context, TransformContext transformContext)
	{
		InsertIRQ(220, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ221")]
	private static void IRQ221(Context context, TransformContext transformContext)
	{
		InsertIRQ(221, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ222")]
	private static void IRQ222(Context context, TransformContext transformContext)
	{
		InsertIRQ(222, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ223")]
	private static void IRQ223(Context context, TransformContext transformContext)
	{
		InsertIRQ(223, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ224")]
	private static void IRQ224(Context context, TransformContext transformContext)
	{
		InsertIRQ(224, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ225")]
	private static void IRQ225(Context context, TransformContext transformContext)
	{
		InsertIRQ(225, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ226")]
	private static void IRQ226(Context context, TransformContext transformContext)
	{
		InsertIRQ(226, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ227")]
	private static void IRQ227(Context context, TransformContext transformContext)
	{
		InsertIRQ(227, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ228")]
	private static void IRQ228(Context context, TransformContext transformContext)
	{
		InsertIRQ(228, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ229")]
	private static void IRQ229(Context context, TransformContext transformContext)
	{
		InsertIRQ(229, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ230")]
	private static void IRQ230(Context context, TransformContext transformContext)
	{
		InsertIRQ(230, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ231")]
	private static void IRQ231(Context context, TransformContext transformContext)
	{
		InsertIRQ(231, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ232")]
	private static void IRQ232(Context context, TransformContext transformContext)
	{
		InsertIRQ(232, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ233")]
	private static void IRQ233(Context context, TransformContext transformContext)
	{
		InsertIRQ(233, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ234")]
	private static void IRQ234(Context context, TransformContext transformContext)
	{
		InsertIRQ(234, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ235")]
	private static void IRQ235(Context context, TransformContext transformContext)
	{
		InsertIRQ(235, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ236")]
	private static void IRQ236(Context context, TransformContext transformContext)
	{
		InsertIRQ(236, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ237")]
	private static void IRQ237(Context context, TransformContext transformContext)
	{
		InsertIRQ(237, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ238")]
	private static void IRQ238(Context context, TransformContext transformContext)
	{
		InsertIRQ(238, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ239")]
	private static void IRQ239(Context context, TransformContext transformContext)
	{
		InsertIRQ(239, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ240")]
	private static void IRQ240(Context context, TransformContext transformContext)
	{
		InsertIRQ(240, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ241")]
	private static void IRQ241(Context context, TransformContext transformContext)
	{
		InsertIRQ(241, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ242")]
	private static void IRQ242(Context context, TransformContext transformContext)
	{
		InsertIRQ(242, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ243")]
	private static void IRQ243(Context context, TransformContext transformContext)
	{
		InsertIRQ(243, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ244")]
	private static void IRQ244(Context context, TransformContext transformContext)
	{
		InsertIRQ(244, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ245")]
	private static void IRQ245(Context context, TransformContext transformContext)
	{
		InsertIRQ(245, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ246")]
	private static void IRQ246(Context context, TransformContext transformContext)
	{
		InsertIRQ(246, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ247")]
	private static void IRQ247(Context context, TransformContext transformContext)
	{
		InsertIRQ(247, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ248")]
	private static void IRQ248(Context context, TransformContext transformContext)
	{
		InsertIRQ(248, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ249")]
	private static void IRQ249(Context context, TransformContext transformContext)
	{
		InsertIRQ(249, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ250")]
	private static void IRQ250(Context context, TransformContext transformContext)
	{
		InsertIRQ(250, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ251")]
	private static void IRQ251(Context context, TransformContext transformContext)
	{
		InsertIRQ(251, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ252")]
	private static void IRQ252(Context context, TransformContext transformContext)
	{
		InsertIRQ(252, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ253")]
	private static void IRQ253(Context context, TransformContext transformContext)
	{
		InsertIRQ(253, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ254")]
	private static void IRQ254(Context context, TransformContext transformContext)
	{
		InsertIRQ(254, context, transformContext);
	}

	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ255")]
	private static void IRQ255(Context context, TransformContext transformContext)
	{
		InsertIRQ(255, context, transformContext);
	}
}
