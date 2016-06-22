// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Utility.DebugEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Mosa.Tool.Debugger
{
	/// <summary>
	///
	/// </summary>
	public partial class DisplayView : DebuggerDockContent
	{
		public const uint StandardAddressBase = 0xB8000;

		public const uint StandardMemorySize = 0x10000 - 80;

		protected byte[] memory;

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

		/// <summary>
		/// Initializes a new instance of the <see cref="DisplayView" /> class.
		/// </summary>
		public DisplayView()
		{
			InitializeComponent();
			DoubleBuffered = true;

			width = 80;
			height = 25;
			memory = new byte[width * height * 2];

			font = new Font("Lucida Console", 9, FontStyle.Bold);

			fontWidth = (int)font.SizeInPoints;
			fontHeight = (int)font.SizeInPoints + 5;

			bitmap = new System.Drawing.Bitmap(fontWidth * width, fontHeight * height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			graphic = Graphics.FromImage(bitmap);
		}

		public override void Connect()
		{
			UpdateView();
		}

		private void DisplayForm_Paint(object sender, PaintEventArgs e)
		{
			lock (bitmap)
			{
				e.Graphics.DrawImage(bitmap, 0, 28, bitmap.Width, bitmap.Height);
			}
		}

		protected void UpdateBitMap()
		{
			lock (bitmap)
			{
				for (int index = 0; index < width * height * 2; index = index + 2)
				{
					int text = memory[index];
					int color = memory[index + 1];

					ushort y = (ushort)(index / ((uint)width * 2));
					ushort x2 = (ushort)(index - (y * (uint)width * 2));
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

		private void UpdateView()
		{
			Status = "Querying...";
			SendCommand(new DebugMessage(DebugCode.ReadMemory, new int[] { (int)StandardAddressBase, (int)width * height * 2 }, CallBack_DisplayMemory));
		}

		private void CallBack_DisplayMemory(DebugMessage message)
		{
			BeginInvoke((MethodInvoker)delegate { DisplayMemory(message); });
		}

		private void DisplayMemory(DebugMessage message)
		{
			Status = string.Empty;
			for (int i = 0; i < width * height * 2; i++)
			{
				memory[i] = message.ResponseData[i + 8];
			}
			UpdateBitMap();
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			UpdateView();
		}

		private void DisplayView_Load(object sender, EventArgs e)
		{
			UpdateView();
		}
	}
}
