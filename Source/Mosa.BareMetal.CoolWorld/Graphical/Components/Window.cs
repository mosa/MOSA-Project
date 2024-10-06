// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Drawing;
using Mosa.DeviceSystem.Mouse;

namespace Mosa.BareMetal.CoolWorld.Graphical.Components;

public class Window
{
	public string Title;

	public int Id;
	public uint OffsetX, OffsetY, X, Y, Width, Height, TitlebarHeight = 20;

	public Color InactiveTitlebarColor, ActiveTitlebarColor, BodyColor;

	public bool Held, Opened = false;

	public readonly Button CloseBtn;

	public readonly Label Label;

	public Window(string title, uint x, uint y, uint width, uint height, Color inactiveTitlebarColor, Color activeTitlebarColor, Color bodyColor)
	{
		Title = title;
		Id = Desktop.Random.Next(1, int.MaxValue);

		X = x;
		Y = y;

		Width = width;
		Height = height;

		InactiveTitlebarColor = inactiveTitlebarColor;
		ActiveTitlebarColor = activeTitlebarColor;
		BodyColor = bodyColor;

		var closeBtnWidth = Display.DefaultFont.CalculateWidth(" X ");

		CloseBtn = new Button(" X ", x + width - closeBtnWidth, y, TitlebarHeight, Color.Red, Color.White, Color.DarkRed,
			() => { WindowManager.Close(this); return null; }, closeBtnWidth);

		Label = new Label(title, Display.DefaultFont, x, y, bodyColor);
	}

	public virtual void Draw()
	{
		if (Opened)
		{
			// Title bar
			Display.DrawRectangle(X, Y, Width, TitlebarHeight, WindowManager.ActiveWindow == this ? ActiveTitlebarColor : InactiveTitlebarColor, true);

			// Title text
			Label.X = X + 5;
			Label.Y = Y;
			Label.Text = Title;

			Label.Draw();

			// Body
			Display.DrawRectangle(X, Y + TitlebarHeight - 1, Width, Height, BodyColor, true);

			// Borders
			// + 2 on width and height to compensate the 1 on X and Y
			Display.DrawRectangle(X - 1, Y - 1, Width + 2, Height + 2 + TitlebarHeight, WindowManager.ActiveWindow == this ? InactiveTitlebarColor : ActiveTitlebarColor, false);
		}
	}

	public virtual void Update()
	{
		if (Opened)
		{
			CloseBtn.X = X + Width - CloseBtn.Width;
			CloseBtn.Y = Y;

			CloseBtn.Draw();
			CloseBtn.Update();

			if (WindowManager.IsWindowMoving && WindowManager.ActiveWindow != this)
				return;

			if (!Held && Mouse.State == MouseState.Left && IsInBounds())
			{
				// Prevent inactive window from getting active if active window is overlapping that window
				if (WindowManager.ActiveWindow != this && IsTitlebarColliding())
					return;

				Held = true;
				WindowManager.IsWindowMoving = true;

				OffsetX = Mouse.X - X;
				OffsetY = Mouse.Y - Y;

				WindowManager.ActiveWindow = this;
			}

			if (!Held)
				return;

			try { X = checked(Mouse.X - OffsetX); } catch { X = 1; }
			try { Y = checked(Mouse.Y - OffsetY); } catch { Y = 1; }

			Held = Mouse.State == MouseState.Left;
			WindowManager.IsWindowMoving = Held;
		}
	}

	public bool IsTitlebarColliding()
	{
		return WindowManager.ActiveWindow.X < X + Width &&
			   WindowManager.ActiveWindow.X + WindowManager.ActiveWindow.Width > X &&
			   WindowManager.ActiveWindow.Y < Y + TitlebarHeight &&
			   WindowManager.ActiveWindow.TitlebarHeight + WindowManager.ActiveWindow.Y > Y;
	}

	public bool IsInBounds()
	{
		return Mouse.IsInBounds(X, Y, Width, TitlebarHeight);
	}
}
