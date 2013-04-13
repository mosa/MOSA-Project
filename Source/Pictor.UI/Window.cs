//----------------------------------------------------------------------------
// Anti-Grain Geometry - Version 2.4
// Copyright (C) 2007 Lars Brubaker
//                  larsbrubaker@gmail.com
//
// Permission to copy, use, modify, sell and distribute this software
// is granted provided this copyright notice appears in all copies.
// This software is provided "as is" without express or implied
// warranty, and with no claim as to its suitability for any purpose.
//
// classes ButtonWidget
//
//----------------------------------------------------------------------------
using System;
using Pictor.Transform;
using Pictor.VertexSource;

namespace Pictor.UI
{
	//----------------------------------------------------------cbox_ctrl_impl
	public class Window : UIWidget
	{
		private TextWidget caption;
		private ButtonWidget closeButton;
		private double m_X;
		private double m_Y;
		private double m_BorderWidth;
		private double m_TextPadding;
		private double m_BorderRadius;
		private double m_width;
		private double m_height;

		private double mouseX;
		private double mouseY;

		public double BorderWidth { get { return m_BorderWidth; } set { m_BorderWidth = value; } }

		public double TextPadding { get { return m_TextPadding; } set { m_TextPadding = value; } }

		public double BorderRadius { get { return m_BorderRadius; } set { m_BorderRadius = value; } }

		public Window(double x, double y, double width, double height, string lable)
		{
			m_X = x;
			m_Y = y;
			m_width = width;
			m_height = height;
			Affine transform = GetTransform();
			transform.Translate(x, y);
			SetTransform(transform);
			caption = new TextWidget(lable, 0, height - 15, 7);
			caption.TextColor = new RGBA_Bytes(255, 255, 255);
			closeButton = new ButtonWidget(width - 15, height - 15, "X", 7, 1, 1, 5);
			closeButton.ButtonClick += CloseEvent;
			AddChild(caption);
			AddChild(closeButton);

			TextPadding = 1;
			BorderWidth = 2;
			BorderRadius = 5;

			double totalExtra = BorderWidth + TextPadding;
			m_Bounds.Left = x - totalExtra;
			m_Bounds.Bottom = y - totalExtra;
			m_Bounds.Right = x + width + totalExtra;
			m_Bounds.Top = y + height + totalExtra;
		}

		public override void OnDraw()
		{
			AntiAliasedScanlineRasterizer ras = new Pictor.AntiAliasedScanlineRasterizer();
			Scanline sl = new Pictor.Scanline();

			RoundedRect rectBorder = new RoundedRect(m_Bounds, m_BorderRadius);
			GetRenderer().Render(rectBorder, new RGBA_Bytes(0, 0, 0));
			RectD insideBounds = Bounds;
			insideBounds.Inflate(-BorderWidth);
			RoundedRect rectInside = new RoundedRect(insideBounds, Math.Max(m_BorderRadius - BorderWidth, 0));
			RGBA_Bytes insideColor = new RGBA_Bytes(222, 222, 222);

			GetRenderer().Render(rectInside, insideColor);

			RoundedRect titleBar = new RoundedRect(new RectD(m_Bounds.Left + BorderWidth, m_Bounds.Top - BorderWidth - 20, m_Bounds.Right - BorderWidth, m_Bounds.Top - BorderWidth), m_BorderRadius);
			GetRenderer().Render(titleBar, new RGBA_Bytes(0, 66, 128));

			base.OnDraw();
		}

		override public void OnMouseMove(MouseEventArgs mouseEvent)
		{
			double x = mouseEvent.X;
			double y = mouseEvent.Y;

			if (mouseEvent.Button == MouseButtons.Left && IsOverTitleBar(x, y))
			{
				double dx = mouseX - x;
				double dy = mouseY - y;
				double totalExtra = BorderWidth + TextPadding;
				Affine transform = GetTransform();
				transform.Translate(-dx, -dy);
				SetTransform(transform);
				m_Bounds.Left -= dx;
				m_Bounds.Right -= dx;
				m_Bounds.Top -= dy;
				m_Bounds.Bottom -= dy;
				mouseEvent.Handled = true;
			}
			mouseX = x;
			mouseY = y;
			Invalidate();
		}

		private bool IsOverTitleBar(double x, double y)
		{
			PointToClient(ref x, ref y);

			return (y >= m_Bounds.Top - 20 && m_Bounds.HitTest(x, y));
		}

		private void CloseEvent(ButtonWidget button)
		{
			Parent.RemoveChild(this);
		}
	};
}