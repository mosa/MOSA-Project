// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.GDBDebugger.Debug;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger.Views
{
	public partial class SourceView : DebugDockContent
	{
		private SourceLocation currentSourceLocation;
		private string currentFileContent;

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
			Query();
		}

		private void Query()
		{
			if (!IsConnected || !IsPaused)
				return;

			if (Platform == null)
				return;

			if (Platform.Registers == null)
				return;

			var sourceLocation = Source.Find(DebugSource, Platform.InstructionPointer.Value);

			if (sourceLocation == null)
				return;

			if (sourceLocation.SourceFilename == null)
				return;

			//if (currentSourceLocation == null || currentSourceLocation.SourceFilename != sourceLocation.SourceFilename)
			//{
			currentSourceLocation = sourceLocation;

			//}

			currentFileContent = currentSourceLocation.SourceFilename != null ? File.ReadAllText(currentSourceLocation.SourceFilename) : string.Empty;

			lbSourceFilename.Text = currentSourceLocation.SourceFilename != null ? currentSourceLocation.SourceFilename : string.Empty;
			rtbSource.Text = currentFileContent;
			toolStripStatusLabel1.Text = string.Empty;

			if (currentSourceLocation.SourceFilename == null)
				return;

			int length = currentFileContent.Length;
			int startPosition = -1;
			int endPosition = -1;

			int currentLineAtStart = 0;
			int currentLineAtEnd = 0;

			int at = 0;
			int currentLine = 1;

			while (at < length)
			{
				char c = currentFileContent[at++];

				if (c == '\n')
					currentLine++;

				if (currentLine == currentSourceLocation.StartLine && startPosition < 0)
				{
					startPosition = at + currentSourceLocation.StartColumn - 1;
					currentLineAtStart = currentLine;
				}
				else if (currentLine == currentSourceLocation.EndLine && endPosition < 0)
				{
					endPosition = at + currentSourceLocation.EndColumn - 1;
					currentLineAtEnd = currentLine;
				}
			}

			if (startPosition < 0)
				return;

			if (endPosition < 0)
				return;

			if (endPosition > length)
				endPosition = length;

			rtbSource.Select(startPosition - currentLineAtStart + 1, endPosition - startPosition - (currentLineAtEnd - currentLineAtStart));
			rtbSource.SelectionBackColor = Color.Blue;
			rtbSource.SelectionColor = Color.White;
			lbSourceFilename.Text = Path.GetFileName(currentSourceLocation.SourceFilename);
			toolStripStatusLabel1.Text = "Label: " + currentSourceLocation.Label + " / " + currentSourceLocation.SourceLabel + " - Lines " + currentSourceLocation.StartLine + "." + currentSourceLocation.StartColumn + "  to " + currentSourceLocation.EndLine + "." + currentSourceLocation.EndColumn;

			rtbSource.ScrollToCaret();
			textBox1.Text = currentFileContent.Substring(startPosition, endPosition - startPosition);
		}
	}
}
