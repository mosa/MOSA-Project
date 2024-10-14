// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Drawing;

namespace Mosa.BareMetal.CoolWorld.Graphical.Components;

public class TaskbarWindowMenu
{
	public uint X, Y, Width, Height;

	public Color BackColor, ForeColor, HoverColor;

	public readonly TaskbarButton AttachedButton;

	// Controls
	private readonly Button CloseBtn, ShowHideBtn;

	private readonly uint Buttons = 2, Separation = 5, ButtonHeight = 20;

	public TaskbarWindowMenu(TaskbarButton btn, Color backColor, Color foreColor, Color hoverColor)
	{
		AttachedButton = btn;

		Width = btn.Width;
		Height = ButtonHeight * Buttons + Separation * (Buttons - 1);

		X = btn.X;
		Y = btn.Y - Separation - Height;

		BackColor = backColor;
		ForeColor = foreColor;
		HoverColor = hoverColor;

		ShowHideBtn = new Button(btn.AttachedWindow.Opened ? "Hide" : "Show", X, Y, ButtonHeight, BackColor, ForeColor, HoverColor, () =>
		{
			btn.AttachedWindow.Opened = !btn.AttachedWindow.Opened;

			for (var j = 0; j < btn.Taskbar.Menus.Count; j++)
			{
				var m = btn.Taskbar.Menus[j];

				if (m.AttachedButton != AttachedButton)
					continue;

				btn.Taskbar.Menus.RemoveAt(j);
				break;
			}

			return null;
		}, Width);

		CloseBtn = new Button("Close", X, Y + Separation * Separation, ButtonHeight, BackColor, ForeColor, HoverColor, () =>
		{
			WindowManager.Close(btn.AttachedWindow);

			for (var j = 0; j < btn.Taskbar.Menus.Count; j++)
			{
				var m = btn.Taskbar.Menus[j];

				if (m.AttachedButton != AttachedButton)
					continue;

				btn.Taskbar.Menus.RemoveAt(j);
				break;
			}

			return null;
		}, Width);
	}

	public void Draw()
	{
		Display.DrawRectangle(X, Y, Width, Height, BackColor, true);

		ShowHideBtn.Draw();
		CloseBtn.Draw();
	}

	public void Update()
	{
		ShowHideBtn.Update();
		CloseBtn.Update();
	}
}
