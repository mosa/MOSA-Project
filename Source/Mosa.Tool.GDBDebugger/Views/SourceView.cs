// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.GDBDebugger.DebugData;
using System.Drawing;
using System.IO;

namespace Mosa.Tool.GDBDebugger.Views
{
	public partial class SourceView : DebugDockContent
	{
		private SourceLocation lastSourceLocation;

		public SourceView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
		}

		public override void OnRunning()
		{
			// Clear
		}

		public override void OnPause()
		{
			toolStripStatusLabel1.Text = string.Empty;
			lbSourceFilename.Text = string.Empty;

			//rtbSource.DeselectAll();
			//rtbSource.SelectionBackColor = Color.LightBlue;
			//rtbSource.SelectionColor = Color.Gray;

			if (!IsConnected || !IsPaused)
				return;

			if (Platform == null)
				return;

			if (Platform.Registers == null)
				return;

			var sourceLocation = Source.GetSourceLocationByAddress(DebugSource, Platform.InstructionPointer.Value);

			if (sourceLocation == null || sourceLocation.SourceFilename == null)
			{
				lastSourceLocation = null;
				return;
			}

			if (!(lastSourceLocation != null && lastSourceLocation.SourceFilename == sourceLocation.SourceFilename))
			{
				rtbSource.Text = File.ReadAllText(sourceLocation.SourceFilename);
			}
			else
			{
				rtbSource.Text = rtbSource.Text;
			}

			lbSourceFilename.Text = Path.GetFileName(sourceLocation.SourceFilename);

			//var methodStartSourceLocation = Source.GetSourceLocationByMethodID(DebugSource, sourceLocation.MethodID);

			//var firstLineIndex = rtbSource.GetFirstCharIndexFromLine(methodStartSourceLocation.StartLine - 1) + methodStartSourceLocation.StartColumn - 1;
			var startIndex = rtbSource.GetFirstCharIndexFromLine(sourceLocation.StartLine - 1) + sourceLocation.StartColumn - 1;
			var endIndex = rtbSource.GetFirstCharIndexFromLine(sourceLocation.EndLine - 1) + sourceLocation.EndColumn - 1;

			//if (sourceLocation.StartLine - methodStartSourceLocation.StartLine <= 10)
			//{
			//	rtbSource.Select(firstLineIndex, 1);
			//}
			//else
			//{
			//	int before = sourceLocation.StartLine > 10 ? sourceLocation.StartLine - 10 : 0;
			//	var beforeLineIndex = rtbSource.GetFirstCharIndexFromLine(before);
			//	rtbSource.Select(beforeLineIndex, 1);
			//}

			//rtbSource.ScrollToCaret();

			rtbSource.SelectionBackColor = Color.Blue;
			rtbSource.SelectionColor = Color.White;

			rtbSource.Select(startIndex, endIndex - startIndex);
			rtbSource.ScrollToCaret();

			//rtbSource.GetCharIndexFromPosition
			//rtbSource.GetFirstCharIndexFromLine
			//rtbSource.GetFirstCharIndexOfCurrentLine
			//rtbSource.GetLineFromCharIndex

			toolStripStatusLabel1.Text = "Label: " + sourceLocation.Label + " / " + sourceLocation.SourceLabel + " - Lines " + sourceLocation.StartLine + "." + sourceLocation.StartColumn + "  to " + sourceLocation.EndLine + "." + sourceLocation.EndColumn;
			lastSourceLocation = sourceLocation;
		}
	}
}
