/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceSystem
{

	/// <summary>
	/// 
	/// </summary>
	public class ColorPalette
	{
		/// <summary>
		/// 
		/// </summary>
		protected ushort entries;

		/// <summary>
		/// 
		/// </summary>
		protected Color[] colors;

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorPalette"/> class.
		/// </summary>
		/// <param name="entries">The entries.</param>
		public ColorPalette(ushort entries)
		{
			this.entries = entries;
			colors = new Color[entries];

			for (int i = 0; i < entries; i++)
				colors[i] = Color.Transparent;
		}

		/// <summary>
		/// Sets the color.
		/// </summary>
		/// <param name="colorIndex">Index of the color.</param>
		/// <param name="color">The color.</param>
		public void SetColor(byte colorIndex, Color color)
		{
			colors[colorIndex] = color;
		}

		/// <summary>
		/// Sets the color.
		/// </summary>
		/// <param name="colorIndex">Index of the color.</param>
		/// <param name="red">The red.</param>
		/// <param name="green">The green.</param>
		/// <param name="blue">The blue.</param>
		public void SetColor(byte colorIndex, byte red, byte green, byte blue)
		{
			colors[colorIndex] = new Color(red, green, blue);
		}

		/// <summary>
		/// Gets the color.
		/// </summary>
		/// <param name="colorIndex">Index of the color.</param>
		/// <returns></returns>
		public Color GetColor(byte colorIndex)
		{
			return colors[colorIndex];
		}

		/// <summary>
		/// Finds the closest color match.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns></returns>
		public byte FindClosestMatch(Color color)
		{
			byte bestIndex = 0;
			int bestDiff = 255 * 255 * 3 + 1;

			for (byte i = 0; i < entries; i++) {
				if (colors[i].IsEqual(color))
					return i;

				// very simple implementation
				int dist = (colors[i].Red * color.Red) + (colors[i].Green * color.Green) + (colors[i].Blue * color.Blue);

				if (dist < bestDiff) {
					bestIndex = i;
					bestDiff = dist;
				}
			}

			return bestIndex;
		}

		#region Standard Palette

		/// <summary>
		/// Gets the Standard 16 color palette.
		/// </summary>
		/// <returns></returns>
		static public ColorPalette CreateStandard16ColorPalette()
		{
			ColorPalette palette = new ColorPalette(16);

			palette.SetColor(0, 0, 0, 0);
			palette.SetColor(1, 0, 0, 170);
			palette.SetColor(2, 0, 170, 0);
			palette.SetColor(3, 0, 170, 170);
			palette.SetColor(4, 170, 0, 0);
			palette.SetColor(5, 170, 0, 170);
			palette.SetColor(6, 170, 85, 0);
			palette.SetColor(7, 170, 170, 170);
			palette.SetColor(8, 85, 85, 85);
			palette.SetColor(9, 85, 85, 255);
			palette.SetColor(10, 85, 255, 85);
			palette.SetColor(11, 85, 255, 255);
			palette.SetColor(12, 255, 85, 85);
			palette.SetColor(13, 255, 85, 255);
			palette.SetColor(14, 255, 255, 85);
			palette.SetColor(15, 255, 255, 255);

			return palette;
		}

		/// <summary>
		/// Gets the Netscape 256 color palette.
		/// </summary>
		/// <returns></returns>
		static public ColorPalette CreateNetscape256ColorPalette()
		{
			ColorPalette palette = new ColorPalette(256);

			palette.SetColor(0, 255, 255, 255);
			palette.SetColor(1, 255, 255, 204);
			palette.SetColor(2, 255, 255, 153);
			palette.SetColor(3, 255, 255, 102);
			palette.SetColor(4, 255, 255, 51);
			palette.SetColor(5, 255, 255, 0);
			palette.SetColor(6, 255, 204, 255);
			palette.SetColor(7, 255, 204, 204);
			palette.SetColor(8, 255, 204, 153);
			palette.SetColor(9, 255, 204, 102);
			palette.SetColor(10, 255, 204, 51);
			palette.SetColor(11, 255, 204, 0);
			palette.SetColor(12, 255, 153, 255);
			palette.SetColor(13, 255, 153, 204);
			palette.SetColor(14, 255, 153, 153);
			palette.SetColor(15, 255, 153, 102);
			palette.SetColor(16, 255, 153, 51);
			palette.SetColor(17, 255, 153, 0);
			palette.SetColor(18, 255, 102, 255);
			palette.SetColor(19, 255, 102, 204);
			palette.SetColor(20, 255, 102, 153);
			palette.SetColor(21, 255, 102, 102);
			palette.SetColor(22, 255, 102, 51);
			palette.SetColor(23, 255, 102, 0);
			palette.SetColor(24, 255, 51, 255);
			palette.SetColor(25, 255, 51, 204);
			palette.SetColor(26, 255, 51, 153);
			palette.SetColor(27, 255, 51, 102);
			palette.SetColor(28, 255, 51, 51);
			palette.SetColor(29, 255, 51, 0);
			palette.SetColor(30, 255, 0, 255);
			palette.SetColor(31, 255, 0, 204);
			palette.SetColor(32, 255, 0, 153);
			palette.SetColor(33, 255, 0, 102);
			palette.SetColor(34, 255, 0, 51);
			palette.SetColor(35, 255, 0, 0);
			palette.SetColor(36, 204, 255, 255);
			palette.SetColor(37, 204, 255, 204);
			palette.SetColor(38, 204, 255, 153);
			palette.SetColor(39, 204, 255, 102);
			palette.SetColor(40, 204, 255, 51);
			palette.SetColor(41, 204, 255, 0);
			palette.SetColor(42, 204, 204, 255);
			palette.SetColor(43, 204, 204, 204);
			palette.SetColor(44, 204, 204, 153);
			palette.SetColor(45, 204, 204, 102);
			palette.SetColor(46, 204, 204, 51);
			palette.SetColor(47, 204, 204, 0);
			palette.SetColor(48, 204, 153, 255);
			palette.SetColor(49, 204, 153, 204);
			palette.SetColor(50, 204, 153, 153);
			palette.SetColor(51, 204, 153, 102);
			palette.SetColor(52, 204, 153, 51);
			palette.SetColor(53, 204, 153, 0);
			palette.SetColor(54, 204, 102, 255);
			palette.SetColor(55, 204, 102, 204);
			palette.SetColor(56, 204, 102, 153);
			palette.SetColor(57, 204, 102, 102);
			palette.SetColor(58, 204, 102, 51);
			palette.SetColor(59, 204, 102, 0);
			palette.SetColor(60, 204, 51, 255);
			palette.SetColor(61, 204, 51, 204);
			palette.SetColor(62, 204, 51, 153);
			palette.SetColor(63, 204, 51, 102);
			palette.SetColor(64, 204, 51, 51);
			palette.SetColor(65, 204, 51, 0);
			palette.SetColor(66, 204, 0, 255);
			palette.SetColor(67, 204, 0, 204);
			palette.SetColor(68, 204, 0, 153);
			palette.SetColor(69, 204, 0, 102);
			palette.SetColor(70, 204, 0, 51);
			palette.SetColor(71, 204, 0, 0);
			palette.SetColor(72, 153, 255, 255);
			palette.SetColor(73, 153, 255, 204);
			palette.SetColor(74, 153, 255, 153);
			palette.SetColor(75, 153, 255, 102);
			palette.SetColor(76, 153, 255, 51);
			palette.SetColor(77, 153, 255, 0);
			palette.SetColor(78, 153, 204, 255);
			palette.SetColor(79, 153, 204, 204);
			palette.SetColor(80, 153, 204, 153);
			palette.SetColor(81, 153, 204, 102);
			palette.SetColor(82, 153, 204, 51);
			palette.SetColor(83, 153, 204, 0);
			palette.SetColor(84, 153, 153, 255);
			palette.SetColor(85, 153, 153, 204);
			palette.SetColor(86, 153, 153, 153);
			palette.SetColor(87, 153, 153, 102);
			palette.SetColor(88, 153, 153, 51);
			palette.SetColor(89, 153, 153, 0);
			palette.SetColor(90, 153, 102, 255);
			palette.SetColor(91, 153, 102, 204);
			palette.SetColor(92, 153, 102, 153);
			palette.SetColor(93, 153, 102, 102);
			palette.SetColor(94, 153, 102, 51);
			palette.SetColor(95, 153, 102, 0);
			palette.SetColor(96, 153, 51, 255);
			palette.SetColor(97, 153, 51, 204);
			palette.SetColor(98, 153, 51, 153);
			palette.SetColor(99, 153, 51, 102);
			palette.SetColor(100, 153, 51, 51);
			palette.SetColor(101, 153, 51, 0);
			palette.SetColor(102, 153, 0, 255);
			palette.SetColor(103, 153, 0, 204);
			palette.SetColor(104, 153, 0, 153);
			palette.SetColor(105, 153, 0, 102);
			palette.SetColor(106, 153, 0, 51);
			palette.SetColor(107, 153, 0, 0);
			palette.SetColor(108, 102, 255, 255);
			palette.SetColor(109, 102, 255, 204);
			palette.SetColor(110, 102, 255, 153);
			palette.SetColor(111, 102, 255, 102);
			palette.SetColor(112, 102, 255, 51);
			palette.SetColor(113, 102, 255, 0);
			palette.SetColor(114, 102, 204, 255);
			palette.SetColor(115, 102, 204, 204);
			palette.SetColor(116, 102, 204, 153);
			palette.SetColor(117, 102, 204, 102);
			palette.SetColor(118, 102, 204, 51);
			palette.SetColor(119, 102, 204, 0);
			palette.SetColor(120, 102, 153, 255);
			palette.SetColor(121, 102, 153, 204);
			palette.SetColor(122, 102, 153, 153);
			palette.SetColor(123, 102, 153, 102);
			palette.SetColor(124, 102, 153, 51);
			palette.SetColor(125, 102, 153, 0);
			palette.SetColor(126, 102, 102, 255);
			palette.SetColor(127, 102, 102, 204);
			palette.SetColor(128, 102, 102, 153);
			palette.SetColor(129, 102, 102, 102);
			palette.SetColor(130, 102, 102, 51);
			palette.SetColor(131, 102, 102, 0);
			palette.SetColor(132, 102, 51, 255);
			palette.SetColor(133, 102, 51, 204);
			palette.SetColor(134, 102, 51, 153);
			palette.SetColor(135, 102, 51, 102);
			palette.SetColor(136, 102, 51, 51);
			palette.SetColor(137, 102, 51, 0);
			palette.SetColor(138, 102, 0, 255);
			palette.SetColor(139, 102, 0, 204);
			palette.SetColor(140, 102, 0, 153);
			palette.SetColor(141, 102, 0, 102);
			palette.SetColor(142, 102, 0, 0);
			palette.SetColor(143, 102, 0, 0);
			palette.SetColor(144, 51, 255, 255);
			palette.SetColor(145, 51, 255, 204);
			palette.SetColor(146, 51, 255, 153);
			palette.SetColor(147, 51, 255, 102);
			palette.SetColor(148, 51, 255, 51);
			palette.SetColor(149, 51, 255, 0);
			palette.SetColor(150, 51, 204, 255);
			palette.SetColor(151, 51, 204, 204);
			palette.SetColor(152, 51, 204, 153);
			palette.SetColor(153, 51, 204, 102);
			palette.SetColor(154, 51, 204, 51);
			palette.SetColor(155, 51, 204, 0);
			palette.SetColor(156, 51, 153, 255);
			palette.SetColor(157, 51, 153, 204);
			palette.SetColor(158, 51, 153, 153);
			palette.SetColor(159, 51, 153, 102);
			palette.SetColor(160, 51, 153, 51);
			palette.SetColor(161, 51, 153, 0);
			palette.SetColor(162, 51, 102, 255);
			palette.SetColor(163, 51, 102, 204);
			palette.SetColor(164, 51, 102, 153);
			palette.SetColor(165, 51, 102, 102);
			palette.SetColor(166, 51, 102, 51);
			palette.SetColor(167, 51, 102, 0);
			palette.SetColor(168, 51, 51, 255);
			palette.SetColor(169, 51, 51, 204);
			palette.SetColor(170, 51, 51, 153);
			palette.SetColor(171, 51, 51, 102);
			palette.SetColor(172, 51, 51, 51);
			palette.SetColor(173, 51, 51, 0);
			palette.SetColor(174, 51, 0, 255);
			palette.SetColor(175, 51, 0, 204);
			palette.SetColor(176, 51, 0, 153);
			palette.SetColor(177, 51, 0, 102);
			palette.SetColor(178, 51, 0, 51);
			palette.SetColor(179, 51, 0, 0);
			palette.SetColor(180, 0, 255, 255);
			palette.SetColor(181, 0, 255, 204);
			palette.SetColor(182, 0, 255, 153);
			palette.SetColor(183, 0, 255, 102);
			palette.SetColor(184, 0, 255, 51);
			palette.SetColor(185, 0, 255, 0);
			palette.SetColor(186, 0, 204, 255);
			palette.SetColor(187, 0, 204, 204);
			palette.SetColor(188, 0, 204, 153);
			palette.SetColor(189, 0, 204, 102);
			palette.SetColor(190, 0, 204, 51);
			palette.SetColor(191, 0, 204, 0);
			palette.SetColor(192, 0, 153, 255);
			palette.SetColor(193, 0, 153, 204);
			palette.SetColor(194, 0, 153, 153);
			palette.SetColor(195, 0, 153, 102);
			palette.SetColor(196, 0, 153, 51);
			palette.SetColor(197, 0, 153, 0);
			palette.SetColor(198, 0, 102, 255);
			palette.SetColor(199, 0, 102, 204);
			palette.SetColor(200, 0, 102, 153);
			palette.SetColor(201, 0, 102, 102);
			palette.SetColor(202, 0, 102, 51);
			palette.SetColor(203, 0, 102, 0);
			palette.SetColor(204, 0, 51, 255);
			palette.SetColor(205, 0, 51, 204);
			palette.SetColor(206, 0, 51, 153);
			palette.SetColor(207, 0, 51, 102);
			palette.SetColor(208, 0, 51, 51);
			palette.SetColor(209, 0, 51, 0);
			palette.SetColor(210, 0, 0, 255);
			palette.SetColor(211, 0, 0, 204);
			palette.SetColor(212, 0, 0, 153);
			palette.SetColor(213, 0, 0, 102);
			palette.SetColor(214, 0, 0, 51);
			palette.SetColor(215, 238, 0, 0);
			palette.SetColor(216, 221, 0, 0);
			palette.SetColor(217, 187, 0, 0);
			palette.SetColor(218, 170, 0, 0);
			palette.SetColor(219, 136, 0, 0);
			palette.SetColor(220, 119, 0, 0);
			palette.SetColor(221, 85, 0, 0);
			palette.SetColor(222, 68, 0, 0);
			palette.SetColor(223, 34, 0, 0);
			palette.SetColor(224, 17, 0, 0);
			palette.SetColor(225, 0, 238, 0);
			palette.SetColor(226, 0, 221, 0);
			palette.SetColor(227, 0, 187, 0);
			palette.SetColor(228, 0, 170, 0);
			palette.SetColor(229, 0, 136, 0);
			palette.SetColor(230, 0, 119, 0);
			palette.SetColor(231, 0, 85, 0);
			palette.SetColor(232, 0, 68, 0);
			palette.SetColor(233, 0, 34, 0);
			palette.SetColor(234, 0, 17, 0);
			palette.SetColor(235, 0, 0, 238);
			palette.SetColor(236, 0, 0, 221);
			palette.SetColor(237, 0, 0, 187);
			palette.SetColor(238, 0, 0, 170);
			palette.SetColor(239, 0, 0, 136);
			palette.SetColor(240, 0, 0, 119);
			palette.SetColor(241, 0, 0, 85);
			palette.SetColor(242, 0, 0, 68);
			palette.SetColor(243, 0, 0, 34);
			palette.SetColor(244, 0, 0, 17);
			palette.SetColor(245, 238, 238, 238);
			palette.SetColor(246, 221, 221, 221);
			palette.SetColor(247, 187, 187, 187);
			palette.SetColor(248, 170, 170, 170);
			palette.SetColor(249, 136, 136, 136);
			palette.SetColor(250, 119, 119, 119);
			palette.SetColor(251, 85, 85, 85);
			palette.SetColor(252, 68, 68, 68);
			palette.SetColor(253, 34, 34, 34);
			palette.SetColor(254, 17, 17, 17);
			palette.SetColor(255, 0, 0, 0);

			return palette;
		}

		/// <summary>
		/// Gets the Standard 256 color palette.
		/// </summary>
		/// <returns></returns>
		static public ColorPalette CreateStandardIBM256ColorPalette()
		{
			ColorPalette palette = new ColorPalette(256);

			palette.SetColor(0, 0, 0, 0);
			palette.SetColor(1, 0, 0, 170);
			palette.SetColor(2, 0, 170, 0);
			palette.SetColor(3, 0, 170, 170);
			palette.SetColor(4, 170, 0, 0);
			palette.SetColor(5, 170, 0, 170);
			palette.SetColor(6, 170, 85, 0);
			palette.SetColor(7, 170, 170, 170);
			palette.SetColor(8, 85, 85, 85);
			palette.SetColor(9, 85, 85, 255);
			palette.SetColor(10, 85, 255, 85);
			palette.SetColor(11, 85, 255, 255);
			palette.SetColor(12, 255, 85, 85);
			palette.SetColor(13, 255, 85, 255);
			palette.SetColor(14, 255, 255, 85);
			palette.SetColor(15, 255, 255, 255);
			palette.SetColor(16, 0, 0, 0);
			palette.SetColor(17, 17, 17, 17);
			palette.SetColor(18, 34, 34, 34);
			palette.SetColor(19, 51, 51, 51);
			palette.SetColor(20, 68, 68, 68);
			palette.SetColor(21, 85, 85, 85);
			palette.SetColor(22, 102, 102, 102);
			palette.SetColor(23, 119, 119, 119);
			palette.SetColor(24, 136, 136, 136);
			palette.SetColor(25, 153, 153, 153);
			palette.SetColor(26, 170, 170, 170);
			palette.SetColor(27, 187, 187, 187);
			palette.SetColor(28, 204, 204, 204);
			palette.SetColor(29, 221, 221, 221);
			palette.SetColor(30, 238, 238, 238);
			palette.SetColor(31, 255, 255, 255);
			palette.SetColor(32, 0, 0, 0);
			palette.SetColor(33, 0, 0, 0);
			palette.SetColor(34, 0, 0, 0);
			palette.SetColor(35, 0, 0, 0);
			palette.SetColor(36, 0, 0, 0);
			palette.SetColor(37, 0, 0, 0);
			palette.SetColor(38, 0, 0, 0);
			palette.SetColor(39, 0, 0, 0);
			palette.SetColor(40, 0, 0, 0);
			palette.SetColor(41, 0, 0, 51);
			palette.SetColor(42, 0, 0, 102);
			palette.SetColor(43, 0, 0, 153);
			palette.SetColor(44, 0, 0, 204);
			palette.SetColor(45, 0, 0, 255);
			palette.SetColor(46, 0, 51, 0);
			palette.SetColor(47, 0, 51, 51);
			palette.SetColor(48, 0, 51, 102);
			palette.SetColor(49, 0, 51, 153);
			palette.SetColor(50, 0, 51, 204);
			palette.SetColor(51, 0, 51, 255);
			palette.SetColor(52, 0, 102, 0);
			palette.SetColor(53, 0, 102, 51);
			palette.SetColor(54, 0, 102, 102);
			palette.SetColor(55, 0, 102, 153);
			palette.SetColor(56, 0, 102, 204);
			palette.SetColor(57, 0, 102, 255);
			palette.SetColor(58, 0, 153, 0);
			palette.SetColor(59, 0, 153, 51);
			palette.SetColor(60, 0, 153, 102);
			palette.SetColor(61, 0, 153, 153);
			palette.SetColor(62, 0, 153, 204);
			palette.SetColor(63, 0, 153, 255);
			palette.SetColor(64, 0, 204, 0);
			palette.SetColor(65, 0, 204, 51);
			palette.SetColor(66, 0, 204, 102);
			palette.SetColor(67, 0, 204, 153);
			palette.SetColor(68, 0, 204, 204);
			palette.SetColor(69, 0, 204, 255);
			palette.SetColor(70, 0, 255, 0);
			palette.SetColor(71, 0, 255, 51);
			palette.SetColor(72, 0, 255, 102);
			palette.SetColor(73, 0, 255, 153);
			palette.SetColor(74, 0, 255, 204);
			palette.SetColor(75, 0, 255, 255);
			palette.SetColor(76, 51, 0, 0);
			palette.SetColor(77, 51, 0, 51);
			palette.SetColor(78, 51, 0, 102);
			palette.SetColor(79, 51, 0, 153);
			palette.SetColor(80, 51, 0, 204);
			palette.SetColor(81, 51, 0, 255);
			palette.SetColor(82, 51, 51, 0);
			palette.SetColor(83, 51, 51, 51);
			palette.SetColor(84, 51, 51, 102);
			palette.SetColor(85, 51, 51, 153);
			palette.SetColor(86, 51, 51, 204);
			palette.SetColor(87, 51, 51, 255);
			palette.SetColor(88, 51, 102, 0);
			palette.SetColor(89, 51, 102, 51);
			palette.SetColor(90, 51, 102, 102);
			palette.SetColor(91, 51, 102, 153);
			palette.SetColor(92, 51, 102, 204);
			palette.SetColor(93, 51, 102, 255);
			palette.SetColor(94, 51, 153, 0);
			palette.SetColor(95, 51, 153, 51);
			palette.SetColor(96, 51, 153, 102);
			palette.SetColor(97, 51, 153, 153);
			palette.SetColor(98, 51, 153, 204);
			palette.SetColor(99, 51, 153, 255);
			palette.SetColor(100, 51, 204, 0);
			palette.SetColor(101, 51, 204, 51);
			palette.SetColor(102, 51, 204, 102);
			palette.SetColor(103, 51, 204, 153);
			palette.SetColor(104, 51, 204, 204);
			palette.SetColor(105, 51, 204, 255);
			palette.SetColor(106, 51, 255, 0);
			palette.SetColor(107, 51, 255, 51);
			palette.SetColor(108, 51, 255, 102);
			palette.SetColor(109, 51, 255, 153);
			palette.SetColor(110, 51, 255, 204);
			palette.SetColor(111, 51, 255, 255);
			palette.SetColor(112, 102, 0, 0);
			palette.SetColor(113, 102, 0, 51);
			palette.SetColor(114, 102, 0, 102);
			palette.SetColor(115, 102, 0, 153);
			palette.SetColor(116, 102, 0, 204);
			palette.SetColor(117, 102, 0, 255);
			palette.SetColor(118, 102, 51, 0);
			palette.SetColor(119, 102, 51, 51);
			palette.SetColor(120, 102, 51, 102);
			palette.SetColor(121, 102, 51, 153);
			palette.SetColor(122, 102, 51, 204);
			palette.SetColor(123, 102, 51, 255);
			palette.SetColor(124, 102, 102, 0);
			palette.SetColor(125, 102, 102, 51);
			palette.SetColor(126, 102, 102, 102);
			palette.SetColor(127, 102, 102, 153);
			palette.SetColor(128, 102, 102, 204);
			palette.SetColor(129, 102, 102, 255);
			palette.SetColor(130, 102, 153, 0);
			palette.SetColor(131, 102, 153, 51);
			palette.SetColor(132, 102, 153, 102);
			palette.SetColor(133, 102, 153, 153);
			palette.SetColor(134, 102, 153, 204);
			palette.SetColor(135, 102, 153, 255);
			palette.SetColor(136, 102, 204, 0);
			palette.SetColor(137, 102, 204, 51);
			palette.SetColor(138, 102, 204, 102);
			palette.SetColor(139, 102, 204, 153);
			palette.SetColor(140, 102, 204, 204);
			palette.SetColor(141, 102, 204, 255);
			palette.SetColor(142, 102, 255, 0);
			palette.SetColor(143, 102, 255, 51);
			palette.SetColor(144, 102, 255, 102);
			palette.SetColor(145, 102, 255, 153);
			palette.SetColor(146, 102, 255, 204);
			palette.SetColor(147, 102, 255, 255);
			palette.SetColor(148, 153, 0, 0);
			palette.SetColor(149, 153, 0, 51);
			palette.SetColor(150, 153, 0, 102);
			palette.SetColor(151, 153, 0, 153);
			palette.SetColor(152, 153, 0, 204);
			palette.SetColor(153, 153, 0, 255);
			palette.SetColor(154, 153, 51, 0);
			palette.SetColor(155, 153, 51, 51);
			palette.SetColor(156, 153, 51, 102);
			palette.SetColor(157, 153, 51, 153);
			palette.SetColor(158, 153, 51, 204);
			palette.SetColor(159, 153, 51, 255);
			palette.SetColor(160, 153, 102, 0);
			palette.SetColor(161, 153, 102, 51);
			palette.SetColor(162, 153, 102, 102);
			palette.SetColor(163, 153, 102, 153);
			palette.SetColor(164, 153, 102, 204);
			palette.SetColor(165, 153, 102, 255);
			palette.SetColor(166, 153, 153, 0);
			palette.SetColor(167, 153, 153, 51);
			palette.SetColor(168, 153, 153, 102);
			palette.SetColor(169, 153, 153, 153);
			palette.SetColor(170, 153, 153, 204);
			palette.SetColor(171, 153, 153, 255);
			palette.SetColor(172, 153, 204, 0);
			palette.SetColor(173, 153, 204, 51);
			palette.SetColor(174, 153, 204, 102);
			palette.SetColor(175, 153, 204, 153);
			palette.SetColor(176, 153, 204, 204);
			palette.SetColor(177, 153, 204, 255);
			palette.SetColor(178, 153, 255, 0);
			palette.SetColor(179, 153, 255, 51);
			palette.SetColor(180, 153, 255, 102);
			palette.SetColor(181, 153, 255, 153);
			palette.SetColor(182, 153, 255, 204);
			palette.SetColor(183, 153, 255, 255);
			palette.SetColor(184, 204, 0, 0);
			palette.SetColor(185, 204, 0, 51);
			palette.SetColor(186, 204, 0, 102);
			palette.SetColor(187, 204, 0, 153);
			palette.SetColor(188, 204, 0, 204);
			palette.SetColor(189, 204, 0, 255);
			palette.SetColor(190, 204, 51, 0);
			palette.SetColor(191, 204, 51, 51);
			palette.SetColor(192, 204, 51, 102);
			palette.SetColor(193, 204, 51, 153);
			palette.SetColor(194, 204, 51, 204);
			palette.SetColor(195, 204, 51, 255);
			palette.SetColor(196, 204, 102, 0);
			palette.SetColor(197, 204, 102, 51);
			palette.SetColor(198, 204, 102, 102);
			palette.SetColor(199, 204, 102, 153);
			palette.SetColor(200, 204, 102, 204);
			palette.SetColor(201, 204, 102, 255);
			palette.SetColor(202, 204, 153, 0);
			palette.SetColor(203, 204, 153, 51);
			palette.SetColor(204, 204, 153, 102);
			palette.SetColor(205, 204, 153, 153);
			palette.SetColor(206, 204, 153, 204);
			palette.SetColor(207, 204, 153, 255);
			palette.SetColor(208, 204, 204, 0);
			palette.SetColor(209, 204, 204, 51);
			palette.SetColor(210, 204, 204, 102);
			palette.SetColor(211, 204, 204, 153);
			palette.SetColor(212, 204, 204, 204);
			palette.SetColor(213, 204, 204, 255);
			palette.SetColor(214, 204, 255, 0);
			palette.SetColor(215, 204, 255, 51);
			palette.SetColor(216, 204, 255, 102);
			palette.SetColor(217, 204, 255, 153);
			palette.SetColor(218, 204, 255, 204);
			palette.SetColor(219, 204, 255, 255);
			palette.SetColor(220, 255, 0, 0);
			palette.SetColor(221, 255, 0, 51);
			palette.SetColor(222, 255, 0, 102);
			palette.SetColor(223, 255, 0, 153);
			palette.SetColor(224, 255, 0, 204);
			palette.SetColor(225, 255, 0, 255);
			palette.SetColor(226, 255, 51, 0);
			palette.SetColor(227, 255, 51, 51);
			palette.SetColor(228, 255, 51, 102);
			palette.SetColor(229, 255, 51, 153);
			palette.SetColor(230, 255, 51, 204);
			palette.SetColor(231, 255, 51, 255);
			palette.SetColor(232, 255, 102, 0);
			palette.SetColor(233, 255, 102, 51);
			palette.SetColor(234, 255, 102, 102);
			palette.SetColor(235, 255, 102, 153);
			palette.SetColor(236, 255, 102, 204);
			palette.SetColor(237, 255, 102, 255);
			palette.SetColor(238, 255, 153, 0);
			palette.SetColor(239, 255, 153, 51);
			palette.SetColor(240, 255, 153, 102);
			palette.SetColor(241, 255, 153, 153);
			palette.SetColor(242, 255, 153, 204);
			palette.SetColor(243, 255, 153, 255);
			palette.SetColor(244, 255, 204, 0);
			palette.SetColor(245, 255, 204, 51);
			palette.SetColor(246, 255, 204, 102);
			palette.SetColor(247, 255, 204, 153);
			palette.SetColor(248, 255, 204, 204);
			palette.SetColor(249, 255, 204, 255);
			palette.SetColor(250, 255, 255, 0);
			palette.SetColor(251, 255, 255, 51);
			palette.SetColor(252, 255, 255, 102);
			palette.SetColor(253, 255, 255, 153);
			palette.SetColor(254, 255, 255, 204);
			palette.SetColor(255, 255, 255, 255);

			return palette;
		}

		#endregion

	}
}
