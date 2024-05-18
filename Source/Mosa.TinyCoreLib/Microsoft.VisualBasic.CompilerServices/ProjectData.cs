using System;
using System.ComponentModel;

namespace Microsoft.VisualBasic.CompilerServices;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class ProjectData
{
	internal ProjectData()
	{
	}

	public static void ClearProjectError()
	{
	}

	public static Exception CreateProjectError(int hr)
	{
		throw null;
	}

	public static void EndApp()
	{
	}

	public static void SetProjectError(Exception? ex)
	{
	}

	public static void SetProjectError(Exception? ex, int lErl)
	{
	}
}
