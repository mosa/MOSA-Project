// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Drawing;

namespace Mosa.Demo.VBEWorld.x86.Components
{
	public class Button
	{
		public int X, Y, Width, Height;

		public string Text;

		public Color BackColor, ForeColor, HoverColor;

		private bool IsHovering;

		public Label Label;

		public Func<object> Action;

		public Button(string text, int x, int y, int height, Color backColor, Color foreColor, Color hoverColor, Func<object> action)
		{
			Text = text;

			X = x;
			Y = y;

			Width = Display.DefaultFont.Size / 2 * Text.Length;
			Height = height;

			BackColor = backColor;
			ForeColor = foreColor;
			HoverColor = hoverColor;

			Label = new Label(Text, Display.DefaultFont.Name, x, y, foreColor);

			if (action != null)
				Action = action;
		}

		public void Draw()
		{
			// Update width in case the text has changed
			Width = Display.DefaultFont.Size / 2 * Text.Length;

			Display.DrawRectangle(X, Y, Width, Height, BackColor, true);
			if (IsHovering)
			{
				Display.DrawRectangle(X, Y, Width, Height, HoverColor, false);
				IsHovering = false;
			}

			Label.X = X;
			Label.Y = Y;
			Label.Text = Text;

			Label.Draw();
		}

		public void Update()
		{
			IsHovering = IsHovered();

			if (Action != null && IsClicked())
				Action.Invoke();
		}

		private static bool IsPressedOneTime()
		{
			int state = Mouse.State;

			if (state == 0)
			{
				while (Mouse.State == state) ;
				return Mouse.State == byte.MaxValue;
			}

			return false;
		}

		public bool IsClicked()
		{
			return IsHovered() &&
				IsPressedOneTime();
		}

		public bool IsHovered()
		{
			return !WindowManager.IsWindowMoving &&
				Mouse.IsInBounds(X, Y, Width, Height);
		}
	}
}
