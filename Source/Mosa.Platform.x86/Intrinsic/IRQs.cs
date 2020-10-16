// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		private static readonly string DefaultInterruptMethodName = "Mosa.Kernel.x86.IDT::ProcessInterrupt";

		private static void InsertIRQ(int irq, Context context, MethodCompiler methodCompiler)
		{
			var interruptMethodName = methodCompiler.Compiler.CompilerSettings.Settings.GetValue("X86.InterruptMethodName", DefaultInterruptMethodName);

			if (string.IsNullOrEmpty(interruptMethodName))
			{
				interruptMethodName = DefaultInterruptMethodName;
			}

			var ar = interruptMethodName.Split(new string[] { "::" }, System.StringSplitOptions.None);

			if (ar.Length != 2)
				return;

			var typeFullname = ar[0];
			var methodName = ar[1];

			var type = methodCompiler.TypeSystem.GetTypeByName(typeFullname);

			if (type == null)
				return;

			var method = type.FindMethodByName(methodName);

			if (method == null)
				return;

			methodCompiler.MethodScanner.MethodInvoked(method, methodCompiler.Method);

			var interrupt = Operand.CreateSymbolFromMethod(method, methodCompiler.TypeSystem);

			var esp = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ESP);

			context.SetInstruction(X86.Cli);
			if (irq <= 7 || (irq >= 16 | irq == 9)) // For IRQ 8, 10, 11, 12, 13, 14 the cpu will automatically pushed the error code
			{
				context.AppendInstruction(X86.Push32, null, methodCompiler.CreateConstant(0));
			}
			context.AppendInstruction(X86.Push32, null, methodCompiler.CreateConstant(irq));
			context.AppendInstruction(X86.Pushad);
			context.AppendInstruction(X86.Push32, null, esp);
			context.AppendInstruction(X86.Call, null, interrupt);
			context.AppendInstruction(X86.Pop32, esp);
			context.AppendInstruction(X86.Popad);
			context.AppendInstruction(X86.Add32, esp, esp, methodCompiler.CreateConstant(8));
			context.AppendInstruction(X86.Sti);
			context.AppendInstruction(X86.IRetd);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ0")]
		private static void IRQ0(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(0, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ1")]
		private static void IRQ1(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(1, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ2")]
		private static void IRQ2(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(2, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ3")]
		private static void IRQ3(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(3, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ4")]
		private static void IRQ4(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(4, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ5")]
		private static void IRQ5(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(5, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ6")]
		private static void IRQ6(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(6, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ7")]
		private static void IRQ7(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(7, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ8")]
		private static void IRQ8(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(8, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ9")]
		private static void IRQ9(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(9, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ10")]
		private static void IRQ10(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(10, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ11")]
		private static void IRQ11(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(11, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ12")]
		private static void IRQ12(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(12, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ13")]
		private static void IRQ13(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(13, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ14")]
		private static void IRQ14(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(14, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ15")]
		private static void IRQ15(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(15, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ16")]
		private static void IRQ16(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(16, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ17")]
		private static void IRQ17(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(17, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ18")]
		private static void IRQ18(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(18, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ19")]
		private static void IRQ19(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(19, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ20")]
		private static void IRQ20(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(20, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ21")]
		private static void IRQ21(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(21, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ22")]
		private static void IRQ22(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(22, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ23")]
		private static void IRQ23(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(23, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ24")]
		private static void IRQ24(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(24, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ25")]
		private static void IRQ25(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(25, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ26")]
		private static void IRQ26(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(26, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ27")]
		private static void IRQ27(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(27, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ28")]
		private static void IRQ28(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(28, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ29")]
		private static void IRQ29(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(29, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ30")]
		private static void IRQ30(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(30, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ31")]
		private static void IRQ31(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(31, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ32")]
		private static void IRQ32(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(32, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ33")]
		private static void IRQ33(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(33, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ34")]
		private static void IRQ34(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(34, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ35")]
		private static void IRQ35(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(35, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ36")]
		private static void IRQ36(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(36, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ37")]
		private static void IRQ37(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(37, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ38")]
		private static void IRQ38(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(38, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ39")]
		private static void IRQ39(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(39, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ40")]
		private static void IRQ40(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(40, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ41")]
		private static void IRQ41(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(41, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ42")]
		private static void IRQ42(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(42, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ43")]
		private static void IRQ43(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(43, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ44")]
		private static void IRQ44(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(44, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ45")]
		private static void IRQ45(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(45, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ46")]
		private static void IRQ46(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(46, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ47")]
		private static void IRQ47(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(47, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ48")]
		private static void IRQ48(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(48, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ49")]
		private static void IRQ49(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(49, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ50")]
		private static void IRQ50(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(50, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ51")]
		private static void IRQ51(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(51, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ52")]
		private static void IRQ52(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(52, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ53")]
		private static void IRQ53(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(53, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ54")]
		private static void IRQ54(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(54, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ55")]
		private static void IRQ55(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(55, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ56")]
		private static void IRQ56(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(56, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ57")]
		private static void IRQ57(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(57, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ58")]
		private static void IRQ58(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(58, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ59")]
		private static void IRQ59(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(59, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ60")]
		private static void IRQ60(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(60, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ61")]
		private static void IRQ61(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(61, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ62")]
		private static void IRQ62(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(62, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ63")]
		private static void IRQ63(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(63, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ64")]
		private static void IRQ64(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(64, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ65")]
		private static void IRQ65(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(65, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ66")]
		private static void IRQ66(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(66, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ67")]
		private static void IRQ67(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(67, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ68")]
		private static void IRQ68(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(68, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ69")]
		private static void IRQ69(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(69, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ70")]
		private static void IRQ70(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(70, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ71")]
		private static void IRQ71(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(71, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ72")]
		private static void IRQ72(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(72, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ73")]
		private static void IRQ73(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(73, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ74")]
		private static void IRQ74(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(74, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ75")]
		private static void IRQ75(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(75, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ76")]
		private static void IRQ76(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(76, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ77")]
		private static void IRQ77(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(77, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ78")]
		private static void IRQ78(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(78, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ79")]
		private static void IRQ79(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(79, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ80")]
		private static void IRQ80(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(80, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ81")]
		private static void IRQ81(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(81, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ82")]
		private static void IRQ82(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(82, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ83")]
		private static void IRQ83(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(83, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ84")]
		private static void IRQ84(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(84, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ85")]
		private static void IRQ85(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(85, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ86")]
		private static void IRQ86(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(86, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ87")]
		private static void IRQ87(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(87, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ88")]
		private static void IRQ88(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(88, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ89")]
		private static void IRQ89(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(89, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ90")]
		private static void IRQ90(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(90, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ91")]
		private static void IRQ91(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(91, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ92")]
		private static void IRQ92(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(92, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ93")]
		private static void IRQ93(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(93, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ94")]
		private static void IRQ94(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(94, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ95")]
		private static void IRQ95(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(95, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ96")]
		private static void IRQ96(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(96, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ97")]
		private static void IRQ97(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(97, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ98")]
		private static void IRQ98(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(98, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ99")]
		private static void IRQ99(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(99, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ100")]
		private static void IRQ100(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(100, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ101")]
		private static void IRQ101(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(101, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ102")]
		private static void IRQ102(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(102, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ103")]
		private static void IRQ103(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(103, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ104")]
		private static void IRQ104(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(104, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ105")]
		private static void IRQ105(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(105, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ106")]
		private static void IRQ106(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(106, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ107")]
		private static void IRQ107(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(107, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ108")]
		private static void IRQ108(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(108, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ109")]
		private static void IRQ109(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(109, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ110")]
		private static void IRQ110(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(110, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ111")]
		private static void IRQ111(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(111, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ112")]
		private static void IRQ112(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(112, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ113")]
		private static void IRQ113(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(113, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ114")]
		private static void IRQ114(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(114, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ115")]
		private static void IRQ115(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(115, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ116")]
		private static void IRQ116(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(116, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ117")]
		private static void IRQ117(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(117, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ118")]
		private static void IRQ118(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(118, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ119")]
		private static void IRQ119(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(119, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ120")]
		private static void IRQ120(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(120, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ121")]
		private static void IRQ121(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(121, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ122")]
		private static void IRQ122(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(122, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ123")]
		private static void IRQ123(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(123, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ124")]
		private static void IRQ124(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(124, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ125")]
		private static void IRQ125(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(125, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ126")]
		private static void IRQ126(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(126, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ127")]
		private static void IRQ127(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(127, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ128")]
		private static void IRQ128(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(128, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ129")]
		private static void IRQ129(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(129, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ130")]
		private static void IRQ130(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(130, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ131")]
		private static void IRQ131(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(131, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ132")]
		private static void IRQ132(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(132, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ133")]
		private static void IRQ133(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(133, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ134")]
		private static void IRQ134(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(134, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ135")]
		private static void IRQ135(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(135, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ136")]
		private static void IRQ136(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(136, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ137")]
		private static void IRQ137(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(137, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ138")]
		private static void IRQ138(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(138, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ139")]
		private static void IRQ139(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(139, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ140")]
		private static void IRQ140(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(140, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ141")]
		private static void IRQ141(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(141, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ142")]
		private static void IRQ142(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(142, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ143")]
		private static void IRQ143(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(143, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ144")]
		private static void IRQ144(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(144, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ145")]
		private static void IRQ145(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(145, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ146")]
		private static void IRQ146(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(146, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ147")]
		private static void IRQ147(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(147, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ148")]
		private static void IRQ148(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(148, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ149")]
		private static void IRQ149(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(149, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ150")]
		private static void IRQ150(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(150, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ151")]
		private static void IRQ151(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(151, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ152")]
		private static void IRQ152(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(152, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ153")]
		private static void IRQ153(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(153, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ154")]
		private static void IRQ154(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(154, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ155")]
		private static void IRQ155(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(155, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ156")]
		private static void IRQ156(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(156, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ157")]
		private static void IRQ157(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(157, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ158")]
		private static void IRQ158(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(158, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ159")]
		private static void IRQ159(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(159, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ160")]
		private static void IRQ160(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(160, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ161")]
		private static void IRQ161(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(161, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ162")]
		private static void IRQ162(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(162, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ163")]
		private static void IRQ163(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(163, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ164")]
		private static void IRQ164(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(164, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ165")]
		private static void IRQ165(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(165, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ166")]
		private static void IRQ166(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(166, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ167")]
		private static void IRQ167(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(167, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ168")]
		private static void IRQ168(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(168, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ169")]
		private static void IRQ169(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(169, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ170")]
		private static void IRQ170(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(170, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ171")]
		private static void IRQ171(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(171, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ172")]
		private static void IRQ172(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(172, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ173")]
		private static void IRQ173(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(173, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ174")]
		private static void IRQ174(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(174, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ175")]
		private static void IRQ175(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(175, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ176")]
		private static void IRQ176(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(176, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ177")]
		private static void IRQ177(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(177, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ178")]
		private static void IRQ178(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(178, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ179")]
		private static void IRQ179(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(179, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ180")]
		private static void IRQ180(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(180, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ181")]
		private static void IRQ181(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(181, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ182")]
		private static void IRQ182(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(182, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ183")]
		private static void IRQ183(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(183, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ184")]
		private static void IRQ184(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(184, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ185")]
		private static void IRQ185(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(185, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ186")]
		private static void IRQ186(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(186, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ187")]
		private static void IRQ187(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(187, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ188")]
		private static void IRQ188(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(188, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ189")]
		private static void IRQ189(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(189, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ190")]
		private static void IRQ190(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(190, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ191")]
		private static void IRQ191(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(191, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ192")]
		private static void IRQ192(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(192, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ193")]
		private static void IRQ193(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(193, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ194")]
		private static void IRQ194(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(194, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ195")]
		private static void IRQ195(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(195, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ196")]
		private static void IRQ196(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(196, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ197")]
		private static void IRQ197(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(197, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ198")]
		private static void IRQ198(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(198, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ199")]
		private static void IRQ199(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(199, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ200")]
		private static void IRQ200(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(200, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ201")]
		private static void IRQ201(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(201, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ202")]
		private static void IRQ202(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(202, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ203")]
		private static void IRQ203(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(203, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ204")]
		private static void IRQ204(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(204, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ205")]
		private static void IRQ205(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(205, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ206")]
		private static void IRQ206(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(206, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ207")]
		private static void IRQ207(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(207, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ208")]
		private static void IRQ208(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(208, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ209")]
		private static void IRQ209(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(209, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ210")]
		private static void IRQ210(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(210, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ211")]
		private static void IRQ211(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(211, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ212")]
		private static void IRQ212(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(212, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ213")]
		private static void IRQ213(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(213, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ214")]
		private static void IRQ214(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(214, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ215")]
		private static void IRQ215(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(215, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ216")]
		private static void IRQ216(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(216, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ217")]
		private static void IRQ217(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(217, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ218")]
		private static void IRQ218(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(218, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ219")]
		private static void IRQ219(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(219, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ220")]
		private static void IRQ220(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(220, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ221")]
		private static void IRQ221(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(221, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ222")]
		private static void IRQ222(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(222, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ223")]
		private static void IRQ223(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(223, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ224")]
		private static void IRQ224(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(224, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ225")]
		private static void IRQ225(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(225, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ226")]
		private static void IRQ226(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(226, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ227")]
		private static void IRQ227(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(227, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ228")]
		private static void IRQ228(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(228, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ229")]
		private static void IRQ229(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(229, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ230")]
		private static void IRQ230(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(230, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ231")]
		private static void IRQ231(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(231, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ232")]
		private static void IRQ232(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(232, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ233")]
		private static void IRQ233(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(233, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ234")]
		private static void IRQ234(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(234, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ235")]
		private static void IRQ235(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(235, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ236")]
		private static void IRQ236(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(236, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ237")]
		private static void IRQ237(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(237, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ238")]
		private static void IRQ238(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(238, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ239")]
		private static void IRQ239(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(239, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ240")]
		private static void IRQ240(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(240, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ241")]
		private static void IRQ241(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(241, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ242")]
		private static void IRQ242(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(242, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ243")]
		private static void IRQ243(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(243, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ244")]
		private static void IRQ244(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(244, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ245")]
		private static void IRQ245(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(245, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ246")]
		private static void IRQ246(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(246, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ247")]
		private static void IRQ247(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(247, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ248")]
		private static void IRQ248(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(248, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ249")]
		private static void IRQ249(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(249, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ250")]
		private static void IRQ250(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(250, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ251")]
		private static void IRQ251(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(251, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ252")]
		private static void IRQ252(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(252, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ253")]
		private static void IRQ253(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(253, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ254")]
		private static void IRQ254(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(254, context, methodCompiler);
		}

		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::IRQ255")]
		private static void IRQ255(Context context, MethodCompiler methodCompiler)
		{
			InsertIRQ(255, context, methodCompiler);
		}
	}
}
