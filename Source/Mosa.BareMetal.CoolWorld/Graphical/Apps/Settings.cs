// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Drawing;
using Mosa.BareMetal.CoolWorld.Graphical.Components;

namespace Mosa.BareMetal.CoolWorld.Graphical.Apps;

public class MouseColorBtn : Button
{
	public readonly uint BaseX, BaseY;

	public MouseColorBtn(Settings settings, Color color) : base(string.Empty, 0, 0, settings.ButtonWidthAndHeight, color,
		color, color, () => Mouse.Color = color, settings.ButtonWidthAndHeight)
	{
		BaseX = settings.X;
		BaseY = settings.Y;

		var count = 0U;
		foreach (var b in settings.Buttons)
			if (b is MouseColorBtn)
				count++;

		if (count == 0)
			X = BaseX + settings.DefaultPadding;
		else
		{
			uint totalWidth = 0;
			for (var i = 0; i < count; i++)
				totalWidth += settings.Buttons[i].Width;
			X = BaseX + totalWidth + settings.DefaultPadding * (count + 1);
		}

		Y = BaseY + settings.TitlebarHeight + settings.DefaultPadding + 20;
	}
}

public class BackColorBtn : Button
{
	public readonly uint BaseX, BaseY;

	public BackColorBtn(Settings settings, Color color) : base(string.Empty, 0, 0, settings.ButtonWidthAndHeight, color,
		color, color, () => Utils.BackColor = color, settings.ButtonWidthAndHeight)
	{
		BaseX = settings.X;
		BaseY = settings.Y;

		var count = 0U;
		foreach (var b in settings.Buttons)
			if (b is BackColorBtn)
				count++;

		if (count == 0)
			X = BaseX + settings.DefaultPadding;
		else
		{
			var totalWidth = 0U;
			for (var i = 0; i < count; i++)
				totalWidth += settings.Buttons[i].Width;
			X = BaseX + totalWidth + settings.DefaultPadding * (count + 1);
		}

		Y = BaseY + settings.TitlebarHeight + settings.DefaultPadding + 80;
	}
}

public class Settings : Window
{
	private readonly Label MouseColorLabel;
	private readonly Label BackColorLabel;
	private readonly Label FontLabel;

	public readonly List<Button> Buttons;

	public readonly uint DefaultPadding = 10, ButtonWidthAndHeight = 20;

	public Settings(uint x, uint y, uint width, uint height, Color inactiveTitlebarColor, Color activeTitlebarColor,
		Color bodyColor) : base("Settings", x, y, width, height, inactiveTitlebarColor, activeTitlebarColor,
		bodyColor)
	{
		MouseColorLabel = new Label("Mouse color:", Display.DefaultFont, x + DefaultPadding, y + TitlebarHeight
			+ DefaultPadding, Utils.Invert(bodyColor));
		BackColorLabel = new Label("Background color:", Display.DefaultFont, x + DefaultPadding, y + TitlebarHeight
			+ DefaultPadding + 60, Utils.Invert(bodyColor));
		FontLabel = new Label("Font:", Display.DefaultFont, x + DefaultPadding, y + TitlebarHeight
			+ DefaultPadding + 110, Utils.Invert(bodyColor));

		Buttons = new List<Button>();

		ListsUpdate();
	}

	public override void Draw()
	{
		base.Draw();

		if (!Opened)
			return;

		MouseColorLabel.Draw();
		BackColorLabel.Draw();
		FontLabel.Draw();

		foreach (var b in Buttons)
			b.Draw();
	}

	public override void Update()
	{
		base.Update();

		if (!Opened || WindowManager.ActiveWindow != this)
			return;

		MouseColorLabel.Font = Display.DefaultFont;
		MouseColorLabel.X = X + DefaultPadding;
		MouseColorLabel.Y = Y + TitlebarHeight + DefaultPadding;

		BackColorLabel.Font = Display.DefaultFont;
		BackColorLabel.X = X + DefaultPadding;
		BackColorLabel.Y = Y + TitlebarHeight + DefaultPadding + 60;

		FontLabel.Font = Display.DefaultFont;
		FontLabel.X = X + DefaultPadding;
		FontLabel.Y = Y + TitlebarHeight + DefaultPadding + 110;

		ListsUpdate();

		foreach (var b in Buttons)
			b.Update();
	}

	private void ListsUpdate()
	{
		Buttons.Clear();
		Buttons.Add(new MouseColorBtn(this, Color.Black));
		Buttons.Add(new MouseColorBtn(this, Color.Blue));
		Buttons.Add(new MouseColorBtn(this, Color.Red));
		Buttons.Add(new MouseColorBtn(this, Color.Green));
		Buttons.Add(new MouseColorBtn(this, Color.Yellow));
		Buttons.Add(new MouseColorBtn(this, Color.Brown));
		Buttons.Add(new MouseColorBtn(this, Color.Orange));
		Buttons.Add(new MouseColorBtn(this, Color.Indigo));
		Buttons.Add(new MouseColorBtn(this, Color.Pink));
		Buttons.Add(new BackColorBtn(this, Color.Black));
		Buttons.Add(new BackColorBtn(this, Color.Blue));
		Buttons.Add(new BackColorBtn(this, Color.Red));
		Buttons.Add(new BackColorBtn(this, Color.Green));
		Buttons.Add(new BackColorBtn(this, Color.Yellow));
		Buttons.Add(new BackColorBtn(this, Color.Brown));
		Buttons.Add(new BackColorBtn(this, Color.Orange));
		Buttons.Add(new BackColorBtn(this, Color.Indigo));
		Buttons.Add(new BackColorBtn(this, Color.Pink));
		var def = new Button(Utils.Fonts[0].Name, X + DefaultPadding, Y + TitlebarHeight + DefaultPadding + 130,
			20, Color.Crimson, Color.White, Color.Firebrick,
			() => Display.DefaultFont = Utils.Fonts[0]);
		Buttons.Add(def);
		Buttons.Add(new Button(Utils.Fonts[1].Name, X + DefaultPadding * 2 + def.Width,
			Y + TitlebarHeight + DefaultPadding + 130, 20, Color.Crimson, Color.White,
			Color.Firebrick, () => Display.DefaultFont = Utils.Fonts[1]));
	}
}
