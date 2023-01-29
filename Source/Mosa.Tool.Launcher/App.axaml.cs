// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace Mosa.Tool.Launcher;

public partial class App : Application
{
	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			desktop.Startup += (_, args) =>
			{
				var win = new MainWindow();
				win.Initialize(args.Args);
				desktop.MainWindow = win;
			};

		base.OnFrameworkInitializationCompleted();
	}
}
