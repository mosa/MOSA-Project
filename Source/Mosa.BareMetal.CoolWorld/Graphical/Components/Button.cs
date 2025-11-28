// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Drawing;
using Mosa.DeviceSystem.Fonts;
using Mosa.DeviceSystem.Mouse;

namespace Mosa.BareMetal.CoolWorld.Graphical.Components;

public class Button
{
	public uint X, Y, Width, Height;

	public string Text;

	private string LastText;

	public Color BackColor, ForeColor, HoverColor;

	private bool IsHovering;
	private readonly bool HasCustomWidth;

	private readonly Label Label;

	private readonly Func<object> Action;

	private ISimpleFont LastFont;

	public Button(string text, uint x, uint y, uint height, Color backColor, Color foreColor, Color hoverColor, Func<Object> action, uint width = 0)
	{
		Text = text;

		LastText = Text;
		LastFont = Display.DefaultFont;

		X = x;
		Y = y;

		HasCustomWidth = width != 0;
		Width = !HasCustomWidth ? Display.DefaultFont.CalculateWidth(Text) : width;

		Height = height;

		BackColor = backColor;
		ForeColor = foreColor;
		HoverColor = hoverColor;

		Label = new Label(Text, Display.DefaultFont, x, y, ForeColor);

		if (action != null)
			Action = action;
	}

	public void Draw()
	{
		// Update width in case the text has changed, and in case we're not using a custom width
		if (!HasCustomWidth && (Text != LastText || Display.DefaultFont != LastFont))
		{
			Width = Display.DefaultFont.CalculateWidth(Text);

			LastText = Text;
			LastFont = Display.DefaultFont;
		}

		Display.DrawRectangle(X, Y, Width, Height, BackColor, true);
		if (IsHovering)
		{
			Display.DrawRectangle(X, Y, Width, Height, HoverColor, false);
			IsHovering = false;
		}

		Label.Font = Display.DefaultFont;
		Label.X = X;
		Label.Y = Y;
		Label.Text = Text;

		Label.Draw();
	}

	public void Update()
	{
		IsHovering = IsHovered();

		if (Action != null && IsClicked(MouseState.Left))
			Action.Invoke();
	}

	private static bool IsPressedOneTime(MouseState mouseState)
	{
		var state = Mouse.State;

		if (state != mouseState)
			return false;

		while (Mouse.State == state) ;
		return Mouse.State == MouseState.None;
	}

	public bool IsClicked(MouseState state)
	{
		return IsHovered() &&
			   IsPressedOneTime(state);
	}

	public bool IsHovered()
	{
		return !WindowManager.IsWindowMoving &&
			   Mouse.IsInBounds(X, Y, Width, Height);
	}
}
