using System.Drawing;
using System.Collections.Generic;
using System;

namespace Mosa.Demo.VBEWorld.x86.Components
{
	public class TaskbarButton : Button
	{
		public Window AttachedWindow;

		public TaskbarButton(Taskbar taskbar, string text, Color backColor, Color foreColor, Color hoverColor, Func<object> action, Window attachedWindow = null)
			: base(text, 0, 0, 0, backColor, foreColor, hoverColor, action)
		{
			Text = text;

			if (taskbar.Buttons.Count == 0)
				X = taskbar.DefaultPadding;
			else
			{
				int totalWidth = 0, count = taskbar.Buttons.Count;
				for (int i = 0; i < count; i++)
					totalWidth += taskbar.Buttons[i].Width;
				X = totalWidth + (taskbar.DefaultPadding * (count + 1));
			}

			Y = taskbar.Y;

			Height = taskbar.Height;

			ForeColor = foreColor;
			BackColor = backColor;

			if (attachedWindow != null)
				AttachedWindow = attachedWindow;
		}
	}

	public class Taskbar
	{
		public int X, Y, Width = Display.Width, Height = 30, DefaultPadding = 5;
		public Color Color = Color.White;

		public List<TaskbarButton> Buttons;

		public Taskbar()
		{
			Buttons = new List<TaskbarButton>();
			Y = Display.Height - Height;
		}

		public void Draw()
		{
			Display.DrawRectangle(0, Y, Width, Height, Color, true);
		}

		public void Update()
		{
			foreach (TaskbarButton b in Buttons)
			{
				b.Draw();
				b.Update();
			}

			// Check if taskbar buttons holding a window (each) are clicked
			for (int i = 0; i < Boot.Taskbar.Buttons.Count; i++)
			{
				TaskbarButton button = Boot.Taskbar.Buttons[i];

				if (button.AttachedWindow != null)
				{
					if (WindowManager.ActiveWindow != null)
					{
						if (button.AttachedWindow.Id == WindowManager.ActiveWindow.Id && button.AttachedWindow.Opened)
						{
							button.HoverColor = button.AttachedWindow.InactiveTitlebarColor;
							button.BackColor = button.AttachedWindow.ActiveTitlebarColor;
						}
						else
						{
							button.HoverColor = button.AttachedWindow.ActiveTitlebarColor;
							button.BackColor = button.AttachedWindow.InactiveTitlebarColor;
						}
					}

					if (button.IsClicked())
					{
						button.AttachedWindow.Opened = !button.AttachedWindow.Opened;

						if (button.AttachedWindow.Opened)
						{
							WindowManager.ActiveWindow = button.AttachedWindow;

							button.HoverColor = button.AttachedWindow.InactiveTitlebarColor;
							button.BackColor = button.AttachedWindow.ActiveTitlebarColor;
						}
						else
						{
							button.HoverColor = button.AttachedWindow.ActiveTitlebarColor;
							button.BackColor = button.AttachedWindow.InactiveTitlebarColor;
						}
					}
				}
			}
		}
	}
}
