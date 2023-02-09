// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Avalonia.Controls;

namespace Mosa.Tool.Bootstrap;

public partial class MainWindow : Window
{
	public MainWindow() => InitializeComponent();

	public void SetStatus(string status) => StatusLbl.Content = status;
}
