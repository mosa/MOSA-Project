﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Avalonia.Platform.Storage;

namespace Mosa.Tool.Explorer.Avalonia;

public static class Utils
{
	public static readonly FilePickerFileType LibraryFileType = new FilePickerFileType("Library")
	{
		Patterns = new List<string> { "*.dll" }
	};

	public static readonly FilePickerOpenOptions LibraryOpenOptions = new FilePickerOpenOptions
	{
		FileTypeFilter = new List<FilePickerFileType> { LibraryFileType }
	};

	public static readonly FolderPickerOpenOptions FolderOpenOptions = new FolderPickerOpenOptions();
}
