// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace Mosa.Tool.Bootstrap;

public partial class App : Application
{
	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			desktop.Startup += (_, args) => LauncherHunter.Hunt(desktop, args.Args);
		}

		base.OnFrameworkInitializationCompleted();
	}
}
