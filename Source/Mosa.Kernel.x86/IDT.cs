﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.x86;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// IDT
	/// </summary>
	public static class IDT
	{
		public delegate void InterruptHandler(uint irq, uint error);

		private static InterruptHandler Interrupt;

		#region Data Members

		internal struct Offset
		{
			internal const byte BaseLow = 0x00;
			internal const byte Select = 0x02;
			internal const byte Always0 = 0x04;
			internal const byte Flags = 0x05;
			internal const byte BaseHigh = 0x06;
			internal const byte TotalSize = 0x08;
		}

		#endregion Data Members

		public static void Setup()
		{
			// Setup IDT table
			Runtime.Internal.MemoryClear(new IntPtr(Address.IDTTable), 6);
			Intrinsic.Store16(new IntPtr(Address.IDTTable), (Offset.TotalSize * 256) - 1);
			Intrinsic.Store32(new IntPtr(Address.IDTTable), 2, Address.IDTTable + 6);

			SetTableEntries();

			_Lidt(Address.IDTTable);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void _Lidt(uint address)
		{
			Native.Lidt(address);
			Native.Sti();
		}

		public static void SetInterruptHandler(InterruptHandler interruptHandler)
		{
			Interrupt = interruptHandler;
		}

		/// <summary>
		/// Sets the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="address">The address.</param>
		/// <param name="select">The select.</param>
		/// <param name="flags">The flags.</param>
		private static void Set(uint index, uint address, ushort select, byte flags)
		{
			var entry = new IntPtr(Address.IDTTable + 6 + (index * Offset.TotalSize));
			Intrinsic.Store16(entry, Offset.BaseLow, (ushort)(address & 0xFFFF));
			Intrinsic.Store16(entry, Offset.BaseHigh, (ushort)((address >> 16) & 0xFFFF));
			Intrinsic.Store16(entry, Offset.Select, select);
			Intrinsic.Store8(entry, Offset.Always0, 0);
			Intrinsic.Store8(entry, Offset.Flags, flags);
		}

		private static void Set(uint index, Delegate method)
		{
			var p = Marshal.GetFunctionPointerForDelegate(method);

			Set(index, (uint)p.ToInt32(), 0x08, 0x8E);
		}

		/// <summary>
		/// Sets the IDT.
		/// </summary>
		private static void SetTableEntries()
		{
			// Clear out idt table
			Runtime.Internal.MemoryClear(new IntPtr(Address.IDTTable) + 6, Offset.TotalSize * 256);

			Set(0, new Action(IRQ0));
			Set(1, new Action(IRQ1));
			Set(2, new Action(IRQ2));
			Set(3, new Action(IRQ3));
			Set(4, new Action(IRQ4));
			Set(5, new Action(IRQ5));
			Set(6, new Action(IRQ6));
			Set(7, new Action(IRQ7));
			Set(8, new Action(IRQ8));
			Set(9, new Action(IRQ9));
			Set(10, new Action(IRQ10));
			Set(11, new Action(IRQ11));
			Set(12, new Action(IRQ12));
			Set(13, new Action(IRQ13));
			Set(14, new Action(IRQ14));
			Set(15, new Action(IRQ15));
			Set(16, new Action(IRQ16));
			Set(17, new Action(IRQ17));
			Set(18, new Action(IRQ18));
			Set(19, new Action(IRQ19));
			Set(20, new Action(IRQ20));
			Set(21, new Action(IRQ21));
			Set(22, new Action(IRQ22));
			Set(23, new Action(IRQ23));
			Set(24, new Action(IRQ24));
			Set(25, new Action(IRQ25));
			Set(26, new Action(IRQ26));
			Set(27, new Action(IRQ27));
			Set(28, new Action(IRQ28));
			Set(29, new Action(IRQ29));
			Set(30, new Action(IRQ30));
			Set(31, new Action(IRQ31));
			Set(32, new Action(IRQ32));
			Set(33, new Action(IRQ33));
			Set(34, new Action(IRQ34));
			Set(35, new Action(IRQ35));
			Set(36, new Action(IRQ36));
			Set(37, new Action(IRQ37));
			Set(38, new Action(IRQ38));
			Set(39, new Action(IRQ39));
			Set(40, new Action(IRQ40));
			Set(41, new Action(IRQ41));
			Set(42, new Action(IRQ42));
			Set(43, new Action(IRQ43));
			Set(44, new Action(IRQ44));
			Set(45, new Action(IRQ45));
			Set(46, new Action(IRQ46));
			Set(47, new Action(IRQ47));
			Set(48, new Action(IRQ48));
			Set(49, new Action(IRQ49));
			Set(50, new Action(IRQ50));
			Set(51, new Action(IRQ51));
			Set(52, new Action(IRQ52));
			Set(53, new Action(IRQ53));
			Set(54, new Action(IRQ54));
			Set(55, new Action(IRQ55));
			Set(56, new Action(IRQ56));
			Set(57, new Action(IRQ57));
			Set(58, new Action(IRQ58));
			Set(59, new Action(IRQ59));
			Set(60, new Action(IRQ60));
			Set(61, new Action(IRQ61));
			Set(62, new Action(IRQ62));
			Set(63, new Action(IRQ63));
			Set(64, new Action(IRQ64));
			Set(65, new Action(IRQ65));
			Set(66, new Action(IRQ66));
			Set(67, new Action(IRQ67));
			Set(68, new Action(IRQ68));
			Set(69, new Action(IRQ69));
			Set(70, new Action(IRQ70));
			Set(71, new Action(IRQ71));
			Set(72, new Action(IRQ72));
			Set(73, new Action(IRQ73));
			Set(74, new Action(IRQ74));
			Set(75, new Action(IRQ75));
			Set(76, new Action(IRQ76));
			Set(77, new Action(IRQ77));
			Set(78, new Action(IRQ78));
			Set(79, new Action(IRQ79));
			Set(80, new Action(IRQ80));
			Set(81, new Action(IRQ81));
			Set(82, new Action(IRQ82));
			Set(83, new Action(IRQ83));
			Set(84, new Action(IRQ84));
			Set(85, new Action(IRQ85));
			Set(86, new Action(IRQ86));
			Set(87, new Action(IRQ87));
			Set(88, new Action(IRQ88));
			Set(89, new Action(IRQ89));
			Set(90, new Action(IRQ90));
			Set(91, new Action(IRQ91));
			Set(92, new Action(IRQ92));
			Set(93, new Action(IRQ93));
			Set(94, new Action(IRQ94));
			Set(95, new Action(IRQ95));
			Set(96, new Action(IRQ96));
			Set(97, new Action(IRQ97));
			Set(98, new Action(IRQ98));
			Set(99, new Action(IRQ99));
			Set(100, new Action(IRQ100));
			Set(101, new Action(IRQ101));
			Set(102, new Action(IRQ102));
			Set(103, new Action(IRQ103));
			Set(104, new Action(IRQ104));
			Set(105, new Action(IRQ105));
			Set(106, new Action(IRQ106));
			Set(107, new Action(IRQ107));
			Set(108, new Action(IRQ108));
			Set(109, new Action(IRQ109));
			Set(110, new Action(IRQ110));
			Set(111, new Action(IRQ111));
			Set(112, new Action(IRQ112));
			Set(113, new Action(IRQ113));
			Set(114, new Action(IRQ114));
			Set(115, new Action(IRQ115));
			Set(116, new Action(IRQ116));
			Set(117, new Action(IRQ117));
			Set(118, new Action(IRQ118));
			Set(119, new Action(IRQ119));
			Set(120, new Action(IRQ120));
			Set(121, new Action(IRQ121));
			Set(122, new Action(IRQ122));
			Set(123, new Action(IRQ123));
			Set(124, new Action(IRQ124));
			Set(125, new Action(IRQ125));
			Set(126, new Action(IRQ126));
			Set(127, new Action(IRQ127));
			Set(128, new Action(IRQ128));
			Set(129, new Action(IRQ129));
			Set(130, new Action(IRQ130));
			Set(131, new Action(IRQ131));
			Set(132, new Action(IRQ132));
			Set(133, new Action(IRQ133));
			Set(134, new Action(IRQ134));
			Set(135, new Action(IRQ135));
			Set(136, new Action(IRQ136));
			Set(137, new Action(IRQ137));
			Set(138, new Action(IRQ138));
			Set(139, new Action(IRQ139));
			Set(140, new Action(IRQ140));
			Set(141, new Action(IRQ141));
			Set(142, new Action(IRQ142));
			Set(143, new Action(IRQ143));
			Set(144, new Action(IRQ144));
			Set(145, new Action(IRQ145));
			Set(146, new Action(IRQ146));
			Set(147, new Action(IRQ147));
			Set(148, new Action(IRQ148));
			Set(149, new Action(IRQ149));
			Set(150, new Action(IRQ150));
			Set(151, new Action(IRQ151));
			Set(152, new Action(IRQ152));
			Set(153, new Action(IRQ153));
			Set(154, new Action(IRQ154));
			Set(155, new Action(IRQ155));
			Set(156, new Action(IRQ156));
			Set(157, new Action(IRQ157));
			Set(158, new Action(IRQ158));
			Set(159, new Action(IRQ159));
			Set(160, new Action(IRQ160));
			Set(161, new Action(IRQ161));
			Set(162, new Action(IRQ162));
			Set(163, new Action(IRQ163));
			Set(164, new Action(IRQ164));
			Set(165, new Action(IRQ165));
			Set(166, new Action(IRQ166));
			Set(167, new Action(IRQ167));
			Set(168, new Action(IRQ168));
			Set(169, new Action(IRQ169));
			Set(170, new Action(IRQ170));
			Set(171, new Action(IRQ171));
			Set(172, new Action(IRQ172));
			Set(173, new Action(IRQ173));
			Set(174, new Action(IRQ174));
			Set(175, new Action(IRQ175));
			Set(176, new Action(IRQ176));
			Set(177, new Action(IRQ177));
			Set(178, new Action(IRQ178));
			Set(179, new Action(IRQ179));
			Set(180, new Action(IRQ180));
			Set(181, new Action(IRQ181));
			Set(182, new Action(IRQ182));
			Set(183, new Action(IRQ183));
			Set(184, new Action(IRQ184));
			Set(185, new Action(IRQ185));
			Set(186, new Action(IRQ186));
			Set(187, new Action(IRQ187));
			Set(188, new Action(IRQ188));
			Set(189, new Action(IRQ189));
			Set(190, new Action(IRQ190));
			Set(191, new Action(IRQ191));
			Set(192, new Action(IRQ192));
			Set(193, new Action(IRQ193));
			Set(194, new Action(IRQ194));
			Set(195, new Action(IRQ195));
			Set(196, new Action(IRQ196));
			Set(197, new Action(IRQ197));
			Set(198, new Action(IRQ198));
			Set(199, new Action(IRQ199));
			Set(200, new Action(IRQ200));
			Set(201, new Action(IRQ201));
			Set(202, new Action(IRQ202));
			Set(203, new Action(IRQ203));
			Set(204, new Action(IRQ204));
			Set(205, new Action(IRQ205));
			Set(206, new Action(IRQ206));
			Set(207, new Action(IRQ207));
			Set(208, new Action(IRQ208));
			Set(209, new Action(IRQ209));
			Set(210, new Action(IRQ210));
			Set(211, new Action(IRQ211));
			Set(212, new Action(IRQ212));
			Set(213, new Action(IRQ213));
			Set(214, new Action(IRQ214));
			Set(215, new Action(IRQ215));
			Set(216, new Action(IRQ216));
			Set(217, new Action(IRQ217));
			Set(218, new Action(IRQ218));
			Set(219, new Action(IRQ219));
			Set(220, new Action(IRQ220));
			Set(221, new Action(IRQ221));
			Set(222, new Action(IRQ222));
			Set(223, new Action(IRQ223));
			Set(224, new Action(IRQ224));
			Set(225, new Action(IRQ225));
			Set(226, new Action(IRQ226));
			Set(227, new Action(IRQ227));
			Set(228, new Action(IRQ228));
			Set(229, new Action(IRQ229));
			Set(230, new Action(IRQ230));
			Set(231, new Action(IRQ231));
			Set(232, new Action(IRQ232));
			Set(233, new Action(IRQ233));
			Set(234, new Action(IRQ234));
			Set(235, new Action(IRQ235));
			Set(236, new Action(IRQ236));
			Set(237, new Action(IRQ237));
			Set(238, new Action(IRQ238));
			Set(239, new Action(IRQ239));
			Set(240, new Action(IRQ240));
			Set(241, new Action(IRQ241));
			Set(242, new Action(IRQ242));
			Set(243, new Action(IRQ243));
			Set(244, new Action(IRQ244));
			Set(245, new Action(IRQ245));
			Set(246, new Action(IRQ246));
			Set(247, new Action(IRQ247));
			Set(248, new Action(IRQ248));
			Set(249, new Action(IRQ249));
			Set(250, new Action(IRQ250));
			Set(251, new Action(IRQ251));
			Set(252, new Action(IRQ252));
			Set(253, new Action(IRQ253));
			Set(254, new Action(IRQ254));
			Set(255, new Action(IRQ255));
		}

		#region IRQ Implementation Methods

		private static void IRQ0()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ0();
		}

		private static void IRQ1()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ1();
		}

		private static void IRQ2()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ2();
		}

		private static void IRQ3()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ3();
		}

		private static void IRQ4()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ4();
		}

		private static void IRQ5()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ5();
		}

		private static void IRQ6()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ6();
		}

		private static void IRQ7()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ7();
		}

		private static void IRQ8()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ8();
		}

		private static void IRQ9()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ9();
		}

		private static void IRQ10()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ10();
		}

		private static void IRQ11()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ11();
		}

		private static void IRQ12()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ12();
		}

		private static void IRQ13()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ13();
		}

		private static void IRQ14()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ14();
		}

		private static void IRQ15()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ15();
		}

		private static void IRQ16()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ16();
		}

		private static void IRQ17()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ17();
		}

		private static void IRQ18()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ18();
		}

		private static void IRQ19()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ19();
		}

		private static void IRQ20()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ20();
		}

		private static void IRQ21()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ21();
		}

		private static void IRQ22()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ22();
		}

		private static void IRQ23()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ23();
		}

		private static void IRQ24()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ24();
		}

		private static void IRQ25()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ25();
		}

		private static void IRQ26()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ26();
		}

		private static void IRQ27()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ27();
		}

		private static void IRQ28()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ28();
		}

		private static void IRQ29()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ29();
		}

		private static void IRQ30()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ30();
		}

		private static void IRQ31()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ31();
		}

		private static void IRQ32()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ32();
		}

		private static void IRQ33()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ33();
		}

		private static void IRQ34()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ34();
		}

		private static void IRQ35()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ35();
		}

		private static void IRQ36()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ36();
		}

		private static void IRQ37()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ37();
		}

		private static void IRQ38()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ38();
		}

		private static void IRQ39()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ39();
		}

		private static void IRQ40()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ40();
		}

		private static void IRQ41()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ41();
		}

		private static void IRQ42()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ42();
		}

		private static void IRQ43()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ43();
		}

		private static void IRQ44()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ44();
		}

		private static void IRQ45()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ45();
		}

		private static void IRQ46()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ46();
		}

		private static void IRQ47()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ47();
		}

		private static void IRQ48()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ48();
		}

		private static void IRQ49()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ49();
		}

		private static void IRQ50()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ50();
		}

		private static void IRQ51()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ51();
		}

		private static void IRQ52()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ52();
		}

		private static void IRQ53()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ53();
		}

		private static void IRQ54()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ54();
		}

		private static void IRQ55()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ55();
		}

		private static void IRQ56()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ56();
		}

		private static void IRQ57()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ57();
		}

		private static void IRQ58()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ58();
		}

		private static void IRQ59()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ59();
		}

		private static void IRQ60()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ60();
		}

		private static void IRQ61()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ61();
		}

		private static void IRQ62()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ62();
		}

		private static void IRQ63()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ63();
		}

		private static void IRQ64()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ64();
		}

		private static void IRQ65()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ65();
		}

		private static void IRQ66()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ66();
		}

		private static void IRQ67()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ67();
		}

		private static void IRQ68()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ68();
		}

		private static void IRQ69()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ69();
		}

		private static void IRQ70()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ70();
		}

		private static void IRQ71()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ71();
		}

		private static void IRQ72()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ72();
		}

		private static void IRQ73()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ73();
		}

		private static void IRQ74()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ74();
		}

		private static void IRQ75()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ75();
		}

		private static void IRQ76()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ76();
		}

		private static void IRQ77()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ77();
		}

		private static void IRQ78()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ78();
		}

		private static void IRQ79()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ79();
		}

		private static void IRQ80()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ80();
		}

		private static void IRQ81()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ81();
		}

		private static void IRQ82()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ82();
		}

		private static void IRQ83()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ83();
		}

		private static void IRQ84()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ84();
		}

		private static void IRQ85()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ85();
		}

		private static void IRQ86()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ86();
		}

		private static void IRQ87()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ87();
		}

		private static void IRQ88()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ88();
		}

		private static void IRQ89()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ89();
		}

		private static void IRQ90()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ90();
		}

		private static void IRQ91()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ91();
		}

		private static void IRQ92()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ92();
		}

		private static void IRQ93()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ93();
		}

		private static void IRQ94()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ94();
		}

		private static void IRQ95()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ95();
		}

		private static void IRQ96()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ96();
		}

		private static void IRQ97()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ97();
		}

		private static void IRQ98()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ98();
		}

		private static void IRQ99()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ99();
		}

		private static void IRQ100()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ100();
		}

		private static void IRQ101()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ101();
		}

		private static void IRQ102()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ102();
		}

		private static void IRQ103()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ103();
		}

		private static void IRQ104()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ104();
		}

		private static void IRQ105()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ105();
		}

		private static void IRQ106()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ106();
		}

		private static void IRQ107()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ107();
		}

		private static void IRQ108()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ108();
		}

		private static void IRQ109()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ109();
		}

		private static void IRQ110()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ110();
		}

		private static void IRQ111()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ111();
		}

		private static void IRQ112()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ112();
		}

		private static void IRQ113()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ113();
		}

		private static void IRQ114()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ114();
		}

		private static void IRQ115()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ115();
		}

		private static void IRQ116()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ116();
		}

		private static void IRQ117()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ117();
		}

		private static void IRQ118()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ118();
		}

		private static void IRQ119()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ119();
		}

		private static void IRQ120()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ120();
		}

		private static void IRQ121()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ121();
		}

		private static void IRQ122()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ122();
		}

		private static void IRQ123()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ123();
		}

		private static void IRQ124()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ124();
		}

		private static void IRQ125()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ125();
		}

		private static void IRQ126()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ126();
		}

		private static void IRQ127()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ127();
		}

		private static void IRQ128()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ128();
		}

		private static void IRQ129()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ129();
		}

		private static void IRQ130()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ130();
		}

		private static void IRQ131()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ131();
		}

		private static void IRQ132()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ132();
		}

		private static void IRQ133()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ133();
		}

		private static void IRQ134()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ134();
		}

		private static void IRQ135()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ135();
		}

		private static void IRQ136()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ136();
		}

		private static void IRQ137()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ137();
		}

		private static void IRQ138()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ138();
		}

		private static void IRQ139()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ139();
		}

		private static void IRQ140()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ140();
		}

		private static void IRQ141()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ141();
		}

		private static void IRQ142()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ142();
		}

		private static void IRQ143()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ143();
		}

		private static void IRQ144()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ144();
		}

		private static void IRQ145()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ145();
		}

		private static void IRQ146()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ146();
		}

		private static void IRQ147()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ147();
		}

		private static void IRQ148()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ148();
		}

		private static void IRQ149()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ149();
		}

		private static void IRQ150()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ150();
		}

		private static void IRQ151()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ151();
		}

		private static void IRQ152()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ152();
		}

		private static void IRQ153()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ153();
		}

		private static void IRQ154()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ154();
		}

		private static void IRQ155()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ155();
		}

		private static void IRQ156()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ156();
		}

		private static void IRQ157()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ157();
		}

		private static void IRQ158()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ158();
		}

		private static void IRQ159()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ159();
		}

		private static void IRQ160()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ160();
		}

		private static void IRQ161()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ161();
		}

		private static void IRQ162()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ162();
		}

		private static void IRQ163()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ163();
		}

		private static void IRQ164()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ164();
		}

		private static void IRQ165()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ165();
		}

		private static void IRQ166()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ166();
		}

		private static void IRQ167()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ167();
		}

		private static void IRQ168()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ168();
		}

		private static void IRQ169()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ169();
		}

		private static void IRQ170()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ170();
		}

		private static void IRQ171()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ171();
		}

		private static void IRQ172()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ172();
		}

		private static void IRQ173()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ173();
		}

		private static void IRQ174()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ174();
		}

		private static void IRQ175()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ175();
		}

		private static void IRQ176()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ176();
		}

		private static void IRQ177()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ177();
		}

		private static void IRQ178()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ178();
		}

		private static void IRQ179()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ179();
		}

		private static void IRQ180()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ180();
		}

		private static void IRQ181()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ181();
		}

		private static void IRQ182()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ182();
		}

		private static void IRQ183()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ183();
		}

		private static void IRQ184()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ184();
		}

		private static void IRQ185()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ185();
		}

		private static void IRQ186()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ186();
		}

		private static void IRQ187()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ187();
		}

		private static void IRQ188()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ188();
		}

		private static void IRQ189()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ189();
		}

		private static void IRQ190()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ190();
		}

		private static void IRQ191()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ191();
		}

		private static void IRQ192()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ192();
		}

		private static void IRQ193()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ193();
		}

		private static void IRQ194()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ194();
		}

		private static void IRQ195()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ195();
		}

		private static void IRQ196()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ196();
		}

		private static void IRQ197()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ197();
		}

		private static void IRQ198()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ198();
		}

		private static void IRQ199()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ199();
		}

		private static void IRQ200()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ200();
		}

		private static void IRQ201()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ201();
		}

		private static void IRQ202()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ202();
		}

		private static void IRQ203()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ203();
		}

		private static void IRQ204()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ204();
		}

		private static void IRQ205()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ205();
		}

		private static void IRQ206()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ206();
		}

		private static void IRQ207()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ207();
		}

		private static void IRQ208()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ208();
		}

		private static void IRQ209()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ209();
		}

		private static void IRQ210()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ210();
		}

		private static void IRQ211()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ211();
		}

		private static void IRQ212()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ212();
		}

		private static void IRQ213()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ213();
		}

		private static void IRQ214()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ214();
		}

		private static void IRQ215()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ215();
		}

		private static void IRQ216()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ216();
		}

		private static void IRQ217()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ217();
		}

		private static void IRQ218()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ218();
		}

		private static void IRQ219()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ219();
		}

		private static void IRQ220()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ220();
		}

		private static void IRQ221()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ221();
		}

		private static void IRQ222()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ222();
		}

		private static void IRQ223()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ223();
		}

		private static void IRQ224()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ224();
		}

		private static void IRQ225()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ225();
		}

		private static void IRQ226()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ226();
		}

		private static void IRQ227()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ227();
		}

		private static void IRQ228()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ228();
		}

		private static void IRQ229()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ229();
		}

		private static void IRQ230()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ230();
		}

		private static void IRQ231()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ231();
		}

		private static void IRQ232()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ232();
		}

		private static void IRQ233()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ233();
		}

		private static void IRQ234()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ234();
		}

		private static void IRQ235()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ235();
		}

		private static void IRQ236()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ236();
		}

		private static void IRQ237()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ237();
		}

		private static void IRQ238()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ238();
		}

		private static void IRQ239()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ239();
		}

		private static void IRQ240()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ240();
		}

		private static void IRQ241()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ241();
		}

		private static void IRQ242()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ242();
		}

		private static void IRQ243()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ243();
		}

		private static void IRQ244()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ244();
		}

		private static void IRQ245()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ245();
		}

		private static void IRQ246()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ246();
		}

		private static void IRQ247()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ247();
		}

		private static void IRQ248()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ248();
		}

		private static void IRQ249()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ249();
		}

		private static void IRQ250()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ250();
		}

		private static void IRQ251()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ251();
		}

		private static void IRQ252()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ252();
		}

		private static void IRQ253()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ253();
		}

		private static void IRQ254()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ254();
		}

		private static void IRQ255()
		{
			Intrinsic.SuppressStackFrame();
			Native.IRQ255();
		}

		#endregion IRQ Implementation Methods

		/// <summary>
		/// Interrupts the handler.
		/// </summary>
		/// <param name="stackStatePointer">The stack state pointer.</param>
		private unsafe static void ProcessInterrupt(uint stackStatePointer)
		{
			var stack = (IDTStack*)stackStatePointer;

			Debugger.Process(stack);

			switch (stack->Interrupt)
			{
				case 0:
					Error(stack, "Divide Error");
					break;

				case 4:
					Error(stack, "Arithmetic Overflow Exception");
					break;

				case 5:
					Error(stack, "Bound Check Error");
					break;

				case 6:
					Error(stack, "Invalid Opcode");
					break;

				case 7:
					Error(stack, "Co-processor Not Available");
					break;

				case 8:

					//TODO: Analyze the double fault
					Error(stack, "Double Fault");
					break;

				case 9:
					Error(stack, "Co-processor Segment Overrun");
					break;

				case 10:
					Error(stack, "Invalid TSS");
					break;

				case 11:
					Error(stack, "Segment Not Present");
					break;

				case 12:
					Error(stack, "Stack Exception");
					break;

				case 13:
					Error(stack, "General Protection Exception");
					break;

				case 14:

					// Check if Null Pointer Exception
					// Otherwise handle as Page Fault

					var cr2 = Native.GetCR2();

					if ((cr2 >> 5) < 0x1000)
					{
						Error(stack, "Null Pointer Exception");
					}

					if (cr2 >= 0xF0000000u)
					{
						Error(stack, "Invalid Access Above 0xF0000000");
						break;
					}

					var physicalpage = PageFrameAllocator.Allocate();

					if (physicalpage == IntPtr.Zero)
					{
						Error(stack, "Out of Memory");
						break;
					}

					PageTable.MapVirtualAddressToPhysical(cr2, (uint)physicalpage.ToInt32());

					break;

				case 16:
					Error(stack, "Co-processor Error");
					break;

				case 19:
					Error(stack, "SIMD Floating-Point Exception");
					break;

				case Scheduler.ClockIRQ:
					Interrupt?.Invoke(stack->Interrupt, stack->ErrorCode);
					Scheduler.ClockInterrupt(new IntPtr(stackStatePointer));
					break;

				case Scheduler.ThreadTerminationSignalIRQ:
					Scheduler.TerminateCurrentThread();
					break;

				default:
					{
						Interrupt?.Invoke(stack->Interrupt, stack->ErrorCode);
						break;
					}
			}

			PIC.SendEndOfInterrupt(stack->Interrupt);
		}

		private unsafe static void Error(IDTStack* stack, string message)
		{
			Panic.ESP = stack->ESP;
			Panic.EBP = stack->EBP;
			Panic.EIP = stack->EIP;
			Panic.EAX = stack->EAX;
			Panic.EBX = stack->EBX;
			Panic.ECX = stack->ECX;
			Panic.EDX = stack->EDX;
			Panic.EDI = stack->EDI;
			Panic.ESI = stack->ESI;
			Panic.CS = stack->CS;
			Panic.ErrorCode = stack->ErrorCode;
			Panic.EFLAGS = stack->EFLAGS;
			Panic.Interrupt = stack->Interrupt;
			Panic.CR2 = Native.GetCR2();
			Panic.FS = Native.GetFS();
			Panic.Error(message);
		}
	}
}
