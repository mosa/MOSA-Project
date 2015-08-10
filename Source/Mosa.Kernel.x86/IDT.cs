// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;
using Mosa.Platform.Internal.x86;

namespace Mosa.Kernel.x86
{
	/// <summary>
	///
	/// </summary>
	public static class IDT
	{
		public delegate void InterruptHandler(uint irq, uint error);

		private static InterruptHandler interruptHandler;

		private static uint idtEntries = Address.IDTTable + 6;

		#region Data members

		internal struct Offset
		{
			internal const byte BaseLow = 0x00;
			internal const byte Select = 0x02;
			internal const byte Always0 = 0x04;
			internal const byte Flags = 0x05;
			internal const byte BaseHigh = 0x06;
			internal const byte TotalSize = 0x08;
		}

		#endregion Data members

		public static void Setup()
		{
			// Setup IDT table
			Memory.Clear(Address.IDTTable, 6);
			Native.Set16(Address.IDTTable, (Offset.TotalSize * 256) - 1);
			Native.Set32(Address.IDTTable + 2, idtEntries);

			SetTableEntries();

			Native.Lidt(Address.IDTTable);
			Native.Sti();
		}

		public static void SetInterruptHandler(InterruptHandler interruptHandler)
		{
			IDT.interruptHandler = interruptHandler;
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
			uint entry = GetEntryLocation(index);
			Native.Set16(entry + Offset.BaseLow, (ushort)(address & 0xFFFF));
			Native.Set16(entry + Offset.BaseHigh, (ushort)((address >> 16) & 0xFFFF));
			Native.Set16(entry + Offset.Select, select);
			Native.Set8(entry + Offset.Always0, 0);
			Native.Set8(entry + Offset.Flags, flags);
		}

		/// <summary>
		/// Gets the idt entry location.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		private static uint GetEntryLocation(uint index)
		{
			return idtEntries + (index * Offset.TotalSize);
		}

		/// <summary>
		/// Sets the IDT.
		/// </summary>
		private static void SetTableEntries()
		{
			// Clear out idt table
			Memory.Clear(idtEntries, Offset.TotalSize * 256);

			// Note: GetIDTJumpLocation parameter must be a constant and not a variable
			Set(0, Native.GetIDTJumpLocation(0), 0x08, 0x8E);
			Set(1, Native.GetIDTJumpLocation(1), 0x08, 0x8E);
			Set(2, Native.GetIDTJumpLocation(2), 0x08, 0x8E);
			Set(3, Native.GetIDTJumpLocation(3), 0x08, 0x8E);
			Set(4, Native.GetIDTJumpLocation(4), 0x08, 0x8E);
			Set(5, Native.GetIDTJumpLocation(5), 0x08, 0x8E);
			Set(6, Native.GetIDTJumpLocation(6), 0x08, 0x8E);
			Set(7, Native.GetIDTJumpLocation(7), 0x08, 0x8E);
			Set(8, Native.GetIDTJumpLocation(8), 0x08, 0x8E);
			Set(9, Native.GetIDTJumpLocation(9), 0x08, 0x8E);
			Set(10, Native.GetIDTJumpLocation(10), 0x08, 0x8E);
			Set(11, Native.GetIDTJumpLocation(11), 0x08, 0x8E);
			Set(12, Native.GetIDTJumpLocation(12), 0x08, 0x8E);
			Set(13, Native.GetIDTJumpLocation(13), 0x08, 0x8E);
			Set(14, Native.GetIDTJumpLocation(14), 0x08, 0x8E);
			Set(15, Native.GetIDTJumpLocation(15), 0x08, 0x8E);
			Set(16, Native.GetIDTJumpLocation(16), 0x08, 0x8E);
			Set(17, Native.GetIDTJumpLocation(17), 0x08, 0x8E);
			Set(18, Native.GetIDTJumpLocation(18), 0x08, 0x8E);
			Set(19, Native.GetIDTJumpLocation(19), 0x08, 0x8E);
			Set(20, Native.GetIDTJumpLocation(20), 0x08, 0x8E);
			Set(21, Native.GetIDTJumpLocation(21), 0x08, 0x8E);
			Set(22, Native.GetIDTJumpLocation(22), 0x08, 0x8E);
			Set(23, Native.GetIDTJumpLocation(23), 0x08, 0x8E);
			Set(24, Native.GetIDTJumpLocation(24), 0x08, 0x8E);
			Set(25, Native.GetIDTJumpLocation(25), 0x08, 0x8E);
			Set(26, Native.GetIDTJumpLocation(26), 0x08, 0x8E);
			Set(27, Native.GetIDTJumpLocation(27), 0x08, 0x8E);
			Set(28, Native.GetIDTJumpLocation(28), 0x08, 0x8E);
			Set(29, Native.GetIDTJumpLocation(29), 0x08, 0x8E);
			Set(30, Native.GetIDTJumpLocation(30), 0x08, 0x8E);
			Set(31, Native.GetIDTJumpLocation(31), 0x08, 0x8E);
			Set(32, Native.GetIDTJumpLocation(32), 0x08, 0x8E);
			Set(33, Native.GetIDTJumpLocation(33), 0x08, 0x8E);
			Set(34, Native.GetIDTJumpLocation(34), 0x08, 0x8E);
			Set(35, Native.GetIDTJumpLocation(35), 0x08, 0x8E);
			Set(36, Native.GetIDTJumpLocation(36), 0x08, 0x8E);
			Set(37, Native.GetIDTJumpLocation(37), 0x08, 0x8E);
			Set(38, Native.GetIDTJumpLocation(38), 0x08, 0x8E);
			Set(39, Native.GetIDTJumpLocation(39), 0x08, 0x8E);
			Set(40, Native.GetIDTJumpLocation(40), 0x08, 0x8E);
			Set(41, Native.GetIDTJumpLocation(41), 0x08, 0x8E);
			Set(42, Native.GetIDTJumpLocation(42), 0x08, 0x8E);
			Set(43, Native.GetIDTJumpLocation(43), 0x08, 0x8E);
			Set(44, Native.GetIDTJumpLocation(44), 0x08, 0x8E);
			Set(45, Native.GetIDTJumpLocation(45), 0x08, 0x8E);
			Set(46, Native.GetIDTJumpLocation(46), 0x08, 0x8E);
			Set(47, Native.GetIDTJumpLocation(47), 0x08, 0x8E);
			Set(48, Native.GetIDTJumpLocation(48), 0x08, 0x8E);
			Set(49, Native.GetIDTJumpLocation(49), 0x08, 0x8E);
			Set(50, Native.GetIDTJumpLocation(50), 0x08, 0x8E);
			Set(51, Native.GetIDTJumpLocation(51), 0x08, 0x8E);
			Set(52, Native.GetIDTJumpLocation(52), 0x08, 0x8E);
			Set(53, Native.GetIDTJumpLocation(53), 0x08, 0x8E);
			Set(54, Native.GetIDTJumpLocation(54), 0x08, 0x8E);
			Set(55, Native.GetIDTJumpLocation(55), 0x08, 0x8E);
			Set(56, Native.GetIDTJumpLocation(56), 0x08, 0x8E);
			Set(57, Native.GetIDTJumpLocation(57), 0x08, 0x8E);
			Set(58, Native.GetIDTJumpLocation(58), 0x08, 0x8E);
			Set(59, Native.GetIDTJumpLocation(59), 0x08, 0x8E);
			Set(60, Native.GetIDTJumpLocation(60), 0x08, 0x8E);
			Set(61, Native.GetIDTJumpLocation(61), 0x08, 0x8E);
			Set(62, Native.GetIDTJumpLocation(62), 0x08, 0x8E);
			Set(63, Native.GetIDTJumpLocation(63), 0x08, 0x8E);
			Set(64, Native.GetIDTJumpLocation(64), 0x08, 0x8E);
			Set(65, Native.GetIDTJumpLocation(65), 0x08, 0x8E);
			Set(66, Native.GetIDTJumpLocation(66), 0x08, 0x8E);
			Set(67, Native.GetIDTJumpLocation(67), 0x08, 0x8E);
			Set(68, Native.GetIDTJumpLocation(68), 0x08, 0x8E);
			Set(69, Native.GetIDTJumpLocation(69), 0x08, 0x8E);
			Set(70, Native.GetIDTJumpLocation(70), 0x08, 0x8E);
			Set(71, Native.GetIDTJumpLocation(71), 0x08, 0x8E);
			Set(72, Native.GetIDTJumpLocation(72), 0x08, 0x8E);
			Set(73, Native.GetIDTJumpLocation(73), 0x08, 0x8E);
			Set(74, Native.GetIDTJumpLocation(74), 0x08, 0x8E);
			Set(75, Native.GetIDTJumpLocation(75), 0x08, 0x8E);
			Set(76, Native.GetIDTJumpLocation(76), 0x08, 0x8E);
			Set(77, Native.GetIDTJumpLocation(77), 0x08, 0x8E);
			Set(78, Native.GetIDTJumpLocation(78), 0x08, 0x8E);
			Set(79, Native.GetIDTJumpLocation(79), 0x08, 0x8E);
			Set(80, Native.GetIDTJumpLocation(80), 0x08, 0x8E);
			Set(81, Native.GetIDTJumpLocation(81), 0x08, 0x8E);
			Set(82, Native.GetIDTJumpLocation(82), 0x08, 0x8E);
			Set(83, Native.GetIDTJumpLocation(83), 0x08, 0x8E);
			Set(84, Native.GetIDTJumpLocation(84), 0x08, 0x8E);
			Set(85, Native.GetIDTJumpLocation(85), 0x08, 0x8E);
			Set(86, Native.GetIDTJumpLocation(86), 0x08, 0x8E);
			Set(87, Native.GetIDTJumpLocation(87), 0x08, 0x8E);
			Set(88, Native.GetIDTJumpLocation(88), 0x08, 0x8E);
			Set(89, Native.GetIDTJumpLocation(89), 0x08, 0x8E);
			Set(90, Native.GetIDTJumpLocation(90), 0x08, 0x8E);
			Set(91, Native.GetIDTJumpLocation(91), 0x08, 0x8E);
			Set(92, Native.GetIDTJumpLocation(92), 0x08, 0x8E);
			Set(93, Native.GetIDTJumpLocation(93), 0x08, 0x8E);
			Set(94, Native.GetIDTJumpLocation(94), 0x08, 0x8E);
			Set(95, Native.GetIDTJumpLocation(95), 0x08, 0x8E);
			Set(96, Native.GetIDTJumpLocation(96), 0x08, 0x8E);
			Set(97, Native.GetIDTJumpLocation(97), 0x08, 0x8E);
			Set(98, Native.GetIDTJumpLocation(98), 0x08, 0x8E);
			Set(99, Native.GetIDTJumpLocation(99), 0x08, 0x8E);
			Set(100, Native.GetIDTJumpLocation(100), 0x08, 0x8E);
			Set(101, Native.GetIDTJumpLocation(101), 0x08, 0x8E);
			Set(102, Native.GetIDTJumpLocation(102), 0x08, 0x8E);
			Set(103, Native.GetIDTJumpLocation(103), 0x08, 0x8E);
			Set(104, Native.GetIDTJumpLocation(104), 0x08, 0x8E);
			Set(105, Native.GetIDTJumpLocation(105), 0x08, 0x8E);
			Set(106, Native.GetIDTJumpLocation(106), 0x08, 0x8E);
			Set(107, Native.GetIDTJumpLocation(107), 0x08, 0x8E);
			Set(108, Native.GetIDTJumpLocation(108), 0x08, 0x8E);
			Set(109, Native.GetIDTJumpLocation(109), 0x08, 0x8E);
			Set(110, Native.GetIDTJumpLocation(110), 0x08, 0x8E);
			Set(111, Native.GetIDTJumpLocation(111), 0x08, 0x8E);
			Set(112, Native.GetIDTJumpLocation(112), 0x08, 0x8E);
			Set(113, Native.GetIDTJumpLocation(113), 0x08, 0x8E);
			Set(114, Native.GetIDTJumpLocation(114), 0x08, 0x8E);
			Set(115, Native.GetIDTJumpLocation(115), 0x08, 0x8E);
			Set(116, Native.GetIDTJumpLocation(116), 0x08, 0x8E);
			Set(117, Native.GetIDTJumpLocation(117), 0x08, 0x8E);
			Set(118, Native.GetIDTJumpLocation(118), 0x08, 0x8E);
			Set(119, Native.GetIDTJumpLocation(119), 0x08, 0x8E);
			Set(120, Native.GetIDTJumpLocation(120), 0x08, 0x8E);
			Set(121, Native.GetIDTJumpLocation(121), 0x08, 0x8E);
			Set(122, Native.GetIDTJumpLocation(122), 0x08, 0x8E);
			Set(123, Native.GetIDTJumpLocation(123), 0x08, 0x8E);
			Set(124, Native.GetIDTJumpLocation(124), 0x08, 0x8E);
			Set(125, Native.GetIDTJumpLocation(125), 0x08, 0x8E);
			Set(126, Native.GetIDTJumpLocation(126), 0x08, 0x8E);
			Set(127, Native.GetIDTJumpLocation(127), 0x08, 0x8E);
			Set(128, Native.GetIDTJumpLocation(128), 0x08, 0x8E);
			Set(129, Native.GetIDTJumpLocation(129), 0x08, 0x8E);
			Set(130, Native.GetIDTJumpLocation(130), 0x08, 0x8E);
			Set(131, Native.GetIDTJumpLocation(131), 0x08, 0x8E);
			Set(132, Native.GetIDTJumpLocation(132), 0x08, 0x8E);
			Set(133, Native.GetIDTJumpLocation(133), 0x08, 0x8E);
			Set(134, Native.GetIDTJumpLocation(134), 0x08, 0x8E);
			Set(135, Native.GetIDTJumpLocation(135), 0x08, 0x8E);
			Set(136, Native.GetIDTJumpLocation(136), 0x08, 0x8E);
			Set(137, Native.GetIDTJumpLocation(137), 0x08, 0x8E);
			Set(138, Native.GetIDTJumpLocation(138), 0x08, 0x8E);
			Set(139, Native.GetIDTJumpLocation(139), 0x08, 0x8E);
			Set(140, Native.GetIDTJumpLocation(140), 0x08, 0x8E);
			Set(141, Native.GetIDTJumpLocation(141), 0x08, 0x8E);
			Set(142, Native.GetIDTJumpLocation(142), 0x08, 0x8E);
			Set(143, Native.GetIDTJumpLocation(143), 0x08, 0x8E);
			Set(144, Native.GetIDTJumpLocation(144), 0x08, 0x8E);
			Set(145, Native.GetIDTJumpLocation(145), 0x08, 0x8E);
			Set(146, Native.GetIDTJumpLocation(146), 0x08, 0x8E);
			Set(147, Native.GetIDTJumpLocation(147), 0x08, 0x8E);
			Set(148, Native.GetIDTJumpLocation(148), 0x08, 0x8E);
			Set(149, Native.GetIDTJumpLocation(149), 0x08, 0x8E);
			Set(150, Native.GetIDTJumpLocation(150), 0x08, 0x8E);
			Set(151, Native.GetIDTJumpLocation(151), 0x08, 0x8E);
			Set(152, Native.GetIDTJumpLocation(152), 0x08, 0x8E);
			Set(153, Native.GetIDTJumpLocation(153), 0x08, 0x8E);
			Set(154, Native.GetIDTJumpLocation(154), 0x08, 0x8E);
			Set(155, Native.GetIDTJumpLocation(155), 0x08, 0x8E);
			Set(156, Native.GetIDTJumpLocation(156), 0x08, 0x8E);
			Set(157, Native.GetIDTJumpLocation(157), 0x08, 0x8E);
			Set(158, Native.GetIDTJumpLocation(158), 0x08, 0x8E);
			Set(159, Native.GetIDTJumpLocation(159), 0x08, 0x8E);
			Set(160, Native.GetIDTJumpLocation(160), 0x08, 0x8E);
			Set(161, Native.GetIDTJumpLocation(161), 0x08, 0x8E);
			Set(162, Native.GetIDTJumpLocation(162), 0x08, 0x8E);
			Set(163, Native.GetIDTJumpLocation(163), 0x08, 0x8E);
			Set(164, Native.GetIDTJumpLocation(164), 0x08, 0x8E);
			Set(165, Native.GetIDTJumpLocation(165), 0x08, 0x8E);
			Set(166, Native.GetIDTJumpLocation(166), 0x08, 0x8E);
			Set(167, Native.GetIDTJumpLocation(167), 0x08, 0x8E);
			Set(168, Native.GetIDTJumpLocation(168), 0x08, 0x8E);
			Set(169, Native.GetIDTJumpLocation(169), 0x08, 0x8E);
			Set(170, Native.GetIDTJumpLocation(170), 0x08, 0x8E);
			Set(171, Native.GetIDTJumpLocation(171), 0x08, 0x8E);
			Set(172, Native.GetIDTJumpLocation(172), 0x08, 0x8E);
			Set(173, Native.GetIDTJumpLocation(173), 0x08, 0x8E);
			Set(174, Native.GetIDTJumpLocation(174), 0x08, 0x8E);
			Set(175, Native.GetIDTJumpLocation(175), 0x08, 0x8E);
			Set(176, Native.GetIDTJumpLocation(176), 0x08, 0x8E);
			Set(177, Native.GetIDTJumpLocation(177), 0x08, 0x8E);
			Set(178, Native.GetIDTJumpLocation(178), 0x08, 0x8E);
			Set(179, Native.GetIDTJumpLocation(179), 0x08, 0x8E);
			Set(180, Native.GetIDTJumpLocation(180), 0x08, 0x8E);
			Set(181, Native.GetIDTJumpLocation(181), 0x08, 0x8E);
			Set(182, Native.GetIDTJumpLocation(182), 0x08, 0x8E);
			Set(183, Native.GetIDTJumpLocation(183), 0x08, 0x8E);
			Set(184, Native.GetIDTJumpLocation(184), 0x08, 0x8E);
			Set(185, Native.GetIDTJumpLocation(185), 0x08, 0x8E);
			Set(186, Native.GetIDTJumpLocation(186), 0x08, 0x8E);
			Set(187, Native.GetIDTJumpLocation(187), 0x08, 0x8E);
			Set(188, Native.GetIDTJumpLocation(188), 0x08, 0x8E);
			Set(189, Native.GetIDTJumpLocation(189), 0x08, 0x8E);
			Set(190, Native.GetIDTJumpLocation(190), 0x08, 0x8E);
			Set(191, Native.GetIDTJumpLocation(191), 0x08, 0x8E);
			Set(192, Native.GetIDTJumpLocation(192), 0x08, 0x8E);
			Set(193, Native.GetIDTJumpLocation(193), 0x08, 0x8E);
			Set(194, Native.GetIDTJumpLocation(194), 0x08, 0x8E);
			Set(195, Native.GetIDTJumpLocation(195), 0x08, 0x8E);
			Set(196, Native.GetIDTJumpLocation(196), 0x08, 0x8E);
			Set(197, Native.GetIDTJumpLocation(197), 0x08, 0x8E);
			Set(198, Native.GetIDTJumpLocation(198), 0x08, 0x8E);
			Set(199, Native.GetIDTJumpLocation(199), 0x08, 0x8E);
			Set(200, Native.GetIDTJumpLocation(200), 0x08, 0x8E);
			Set(201, Native.GetIDTJumpLocation(201), 0x08, 0x8E);
			Set(202, Native.GetIDTJumpLocation(202), 0x08, 0x8E);
			Set(203, Native.GetIDTJumpLocation(203), 0x08, 0x8E);
			Set(204, Native.GetIDTJumpLocation(204), 0x08, 0x8E);
			Set(205, Native.GetIDTJumpLocation(205), 0x08, 0x8E);
			Set(206, Native.GetIDTJumpLocation(206), 0x08, 0x8E);
			Set(207, Native.GetIDTJumpLocation(207), 0x08, 0x8E);
			Set(208, Native.GetIDTJumpLocation(208), 0x08, 0x8E);
			Set(209, Native.GetIDTJumpLocation(209), 0x08, 0x8E);
			Set(210, Native.GetIDTJumpLocation(210), 0x08, 0x8E);
			Set(211, Native.GetIDTJumpLocation(211), 0x08, 0x8E);
			Set(212, Native.GetIDTJumpLocation(212), 0x08, 0x8E);
			Set(213, Native.GetIDTJumpLocation(213), 0x08, 0x8E);
			Set(214, Native.GetIDTJumpLocation(214), 0x08, 0x8E);
			Set(215, Native.GetIDTJumpLocation(215), 0x08, 0x8E);
			Set(216, Native.GetIDTJumpLocation(216), 0x08, 0x8E);
			Set(217, Native.GetIDTJumpLocation(217), 0x08, 0x8E);
			Set(218, Native.GetIDTJumpLocation(218), 0x08, 0x8E);
			Set(219, Native.GetIDTJumpLocation(219), 0x08, 0x8E);
			Set(220, Native.GetIDTJumpLocation(220), 0x08, 0x8E);
			Set(221, Native.GetIDTJumpLocation(221), 0x08, 0x8E);
			Set(222, Native.GetIDTJumpLocation(222), 0x08, 0x8E);
			Set(223, Native.GetIDTJumpLocation(223), 0x08, 0x8E);
			Set(224, Native.GetIDTJumpLocation(224), 0x08, 0x8E);
			Set(225, Native.GetIDTJumpLocation(225), 0x08, 0x8E);
			Set(226, Native.GetIDTJumpLocation(226), 0x08, 0x8E);
			Set(227, Native.GetIDTJumpLocation(227), 0x08, 0x8E);
			Set(228, Native.GetIDTJumpLocation(228), 0x08, 0x8E);
			Set(229, Native.GetIDTJumpLocation(229), 0x08, 0x8E);
			Set(230, Native.GetIDTJumpLocation(230), 0x08, 0x8E);
			Set(231, Native.GetIDTJumpLocation(231), 0x08, 0x8E);
			Set(232, Native.GetIDTJumpLocation(232), 0x08, 0x8E);
			Set(233, Native.GetIDTJumpLocation(233), 0x08, 0x8E);
			Set(234, Native.GetIDTJumpLocation(234), 0x08, 0x8E);
			Set(235, Native.GetIDTJumpLocation(235), 0x08, 0x8E);
			Set(236, Native.GetIDTJumpLocation(236), 0x08, 0x8E);
			Set(237, Native.GetIDTJumpLocation(237), 0x08, 0x8E);
			Set(238, Native.GetIDTJumpLocation(238), 0x08, 0x8E);
			Set(239, Native.GetIDTJumpLocation(239), 0x08, 0x8E);
			Set(240, Native.GetIDTJumpLocation(240), 0x08, 0x8E);
			Set(241, Native.GetIDTJumpLocation(241), 0x08, 0x8E);
			Set(242, Native.GetIDTJumpLocation(242), 0x08, 0x8E);
			Set(243, Native.GetIDTJumpLocation(243), 0x08, 0x8E);
			Set(244, Native.GetIDTJumpLocation(244), 0x08, 0x8E);
			Set(245, Native.GetIDTJumpLocation(245), 0x08, 0x8E);
			Set(246, Native.GetIDTJumpLocation(246), 0x08, 0x8E);
			Set(247, Native.GetIDTJumpLocation(247), 0x08, 0x8E);
			Set(248, Native.GetIDTJumpLocation(248), 0x08, 0x8E);
			Set(249, Native.GetIDTJumpLocation(249), 0x08, 0x8E);
			Set(250, Native.GetIDTJumpLocation(250), 0x08, 0x8E);
			Set(251, Native.GetIDTJumpLocation(251), 0x08, 0x8E);
			Set(252, Native.GetIDTJumpLocation(252), 0x08, 0x8E);
			Set(253, Native.GetIDTJumpLocation(253), 0x08, 0x8E);
			Set(254, Native.GetIDTJumpLocation(254), 0x08, 0x8E);
			Set(255, Native.GetIDTJumpLocation(255), 0x08, 0x8E);
		}

		/// <summary>
		/// Interrupts the handler.
		/// </summary>
		/// <param name="stack">The stack.</param>
		private unsafe static void ProcessInterrupt(IDTStack* stack)
		{
			DebugClient.Process();

			switch (stack->Interrupt)
			{
				case 0:
					Error(stack->EBP, stack->EIP, "Divide Error");
					break;

				case 4:
					Error(stack->EBP, stack->EIP, "Arithmetic Overflow Exception");
					break;

				case 5:
					Error(stack->EBP, stack->EIP, "Bound Check Error");
					break;

				case 6:
					Error(stack->EBP, stack->EIP, "Invalid Opcode");
					break;

				case 7:
					Error(stack->EBP, stack->EIP, "Coprocessor Not Available");
					break;

				case 8:

					//TODO: Analyze the double fault
					Error(stack->EBP, stack->EIP, "Double Fault");
					break;

				case 9:
					Error(stack->EBP, stack->EIP, "Coprocessor Segment Overrun");
					break;

				case 10:
					Error(stack->EBP, stack->EIP, "Invalid TSS");
					break;

				case 11:
					Error(stack->EBP, stack->EIP, "Segment Not Present");
					break;

				case 12:
					Error(stack->EBP, stack->EIP, "Stack Exception");
					break;

				case 13:
					Error(stack->EBP, stack->EIP, "General Protection Exception");
					break;

				case 14:

					// Page Fault!
					var cr2 = Native.GetCR2() >> 5;
					if (cr2 < 0x1000)
					{
						Error(stack->EBP, stack->EIP, "Null Pointer Exception");
						break;
					}

					//PageFaultHandler.Fault(errorCode);
					Panic.SetStackPointer(stack->EBP, stack->EIP);
					Panic.Error(cr2);
					break;

				case 16:
					Error(stack->EBP, stack->EIP, "Coprocessor Error");
					break;

				case 19:
					Error(stack->EBP, stack->EIP, "SIMD Floating-Point Exception");
					break;

				default:
					if (interruptHandler != null)
						interruptHandler(stack->Interrupt, stack->ErrorCode);
					break;
			}

			PIC.SendEndOfInterrupt(stack->Interrupt);
		}

		private static void Error(uint ebp, uint eip, string message)
		{
			Panic.SetStackPointer(ebp, eip);
			Panic.Error(message);
		}

		[StructLayout(LayoutKind.Explicit)]
		private struct IDTStack
		{
			[FieldOffset(0x00)]
			public uint EDI;

			[FieldOffset(0x04)]
			public uint ESI;

			[FieldOffset(0x08)]
			public uint EBP;

			[FieldOffset(0x0C)]
			public uint ESP;

			[FieldOffset(0x10)]
			public uint EBX;

			[FieldOffset(0x14)]
			public uint EDX;

			[FieldOffset(0x18)]
			public uint ECX;

			[FieldOffset(0x1C)]
			public uint EAX;

			[FieldOffset(0x20)]
			public uint Interrupt;

			[FieldOffset(0x24)]
			public uint ErrorCode;

			[FieldOffset(0x28)]
			public uint EIP;

			[FieldOffset(0x2C)]
			public uint CS;

			[FieldOffset(0x30)]
			public uint EFLAGS;
		}
	}
}
