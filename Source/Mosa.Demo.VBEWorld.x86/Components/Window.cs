﻿using System.Drawing;

namespace Mosa.Demo.VBEWorld.x86.Components
{
	public class Window
	{
		public string Title;

		public int Id, OffsetX, OffsetY, X, Y, Width, Height, TitlebarHeight = 20;

		public Color InactiveTitlebarColor, ActiveTitlebarColor, BodyColor;

		public bool Held = false, Opened = false;

		public Button CloseBtn;

		public Label Label;

		public Window(string title, int x, int y, int width, int height, Color inactiveTitlebarColor, Color activeTitlebarColor, Color bodyColor)
		{
			Title = title;
			Id = Boot.Random.Next(1, int.MaxValue);

			X = x;
			Y = y;

			Width = width;
			Height = height;

			InactiveTitlebarColor = inactiveTitlebarColor;
			ActiveTitlebarColor = activeTitlebarColor;
			BodyColor = bodyColor;

			CloseBtn = new Button(" X ", x + width, y, TitlebarHeight, Color.Red, Color.White, Color.DarkRed,
				() => { WindowManager.Close(this); return null; });
			X -= CloseBtn.Width;

			Label = new Label(title, Display.DefaultFont.Name, x + 5, y, bodyColor);
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
				Display.DrawRectangle(X, Y + TitlebarHeight, Width, Height, BodyColor, true);

				// Borders
				Display.DrawRectangle(X, Y, Width, Height + TitlebarHeight, WindowManager.ActiveWindow == this ? InactiveTitlebarColor : ActiveTitlebarColor, false);
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

				if (WindowManager.IsWindowMoving && WindowManager.ActiveWindow != null && WindowManager.ActiveWindow != this)
					return;

				if (!Held && !WindowManager.IsWindowMoving && Mouse.State == 0 && IsInBounds())
				{
					// Prevent inactive window from getting active if active window is overlapping that window
					if (WindowManager.ActiveWindow != null && WindowManager.ActiveWindow != this && IsTitlebarColliding())
						return;

					Held = true;
					WindowManager.IsWindowMoving = true;

					OffsetX = Mouse.X - X;
					OffsetY = Mouse.Y - Y;

					WindowManager.ActiveWindow = this;
				}

				// Performance optimization
				/*Opened = WindowManager.ActiveWindow != null &&
					WindowManager.ActiveWindow != this &&
					WindowManager.ActiveWindow.X == X && WindowManager.ActiveWindow.Y == Y &&
					WindowManager.ActiveWindow.Width >= Width && WindowManager.ActiveWindow.Height >= Height &&
					WindowManager.ActiveWindow.TitlebarHeight >= TitlebarHeight;*/

				if (Held)
				{
					X = Mouse.X - OffsetX;
					Y = Mouse.Y - OffsetY;

					Held = Mouse.State == 0;
					WindowManager.IsWindowMoving = Held;
				}
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
}
