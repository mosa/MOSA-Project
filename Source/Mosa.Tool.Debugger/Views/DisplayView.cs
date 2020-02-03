// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Drawing;
using System.Windows.Forms;

namespace Mosa.Tool.Debugger.Views
{
	/// <summary>
	///
	/// </summary>
	public partial class DisplayView : DebugDockContent
	{
		public const uint StandardAddressBase = 0xB8000;

		protected byte height;
		protected byte width;

		private Bitmap bitmap;
		private Graphics graphic;

		private Font font;
		private int fontWidth;
		private int fontHeight;

		#region Definitions

		private static byte[,] palette = new byte[,]
		{
			{0, 0, 0, 0},
			{1, 0, 0, 170},
			{2, 0, 170, 0},
			{3, 0, 170, 170},
			{4, 170, 0, 0},
			{5, 170, 0, 170},
			{6, 170, 85, 0},
			{7, 170, 170, 170},
			{8, 85, 85, 85},
			{9, 85, 85, 255},
			{10, 85, 255, 85},
			{11, 85, 255, 255},
			{12, 255, 85, 85},
			{13, 255, 85, 255},
			{14, 255, 255, 85},
			{15, 255, 255, 255},
		};

		#endregion Definitions

		public DisplayView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
			DoubleBuffered = true;

			width = 80;
			height = 25;

			font = new Font("Lucida Console", 9, FontStyle.Bold);

			fontWidth = (int)font.SizeInPoints;
			fontHeight = (int)font.SizeInPoints + 5;

			bitmap = new System.Drawing.Bitmap(fontWidth * width, fontHeight * height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			graphic = Graphics.FromImage(bitmap);
		}

		public override void OnPause()
		{
			MemoryCache.ReadMemory(StandardAddressBase, (uint)(width * height * 2), OnMemoryRead);
		}

		private void OnMemoryRead(ulong address, byte[] bytes) => Invoke((MethodInvoker)(() => UpdateDisplay(address, bytes)));

		protected void UpdateDisplay(ulong address, byte[] memory)
		{
			//if (memory.Length < width * height)
			//	return;

			uint offset = (uint)(address - StandardAddressBase);

			lock (bitmap)
			{
				for (int index = 0; index < memory.Length; index += 2)
				{
					int text = memory[index];
					int color = memory[index + 1];

					ushort y = (ushort)((index + offset) / ((uint)width * 2));
					ushort x2 = (ushort)((index + offset) - (y * (uint)width * 2));
					ushort x = (ushort)(x2 >> 1);

					PutChar(x, y, (char)text, (byte)(color & 0x0F), (byte)(color >> 4));
				}
			}

			Refresh();
		}

		protected void PutChar(ushort x, ushort y, char c, byte colorindex, byte backgroundindex)
		{
			var color = new SolidBrush(Color.FromArgb(palette[colorindex, 1], palette[colorindex, 2], palette[colorindex, 3]));
			var background = new SolidBrush(Color.FromArgb(palette[backgroundindex, 1], palette[backgroundindex, 2], palette[backgroundindex, 3]));

			graphic.FillRectangle(background, new Rectangle(x * fontWidth, y * fontHeight, fontWidth + 1, fontHeight + 1));
			graphic.DrawString(c.ToString(), font, color, x * fontWidth, y * fontHeight);
		}

		private void DisplayForm_Paint(object sender, PaintEventArgs e)
		{
			lock (bitmap)
			{
				e.Graphics.DrawImage(bitmap, 0, 28, bitmap.Width, bitmap.Height);
			}
		}
	}
}
