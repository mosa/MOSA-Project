// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.Debugger.DebugData;
using System.Drawing;
using System.IO;

namespace Mosa.Tool.Debugger.Views
{
	public partial class SourceView : DebugDockContent
	{
		private SourceLocation lastSourceLocation;
		private string lastFileContent;

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

			var sourceLocation = Source.Find(DebugSource, InstructionPointer);

			if (sourceLocation == null)
				return;

			if (sourceLocation.SourceFilename == null)
				return;

			string fileContent = string.Empty;

			if (sourceLocation.SourceFilename != null)
			{
				if (lastSourceLocation != null && lastSourceLocation.SourceFilename == sourceLocation.SourceFilename)
				{
					fileContent = lastFileContent;
				}
				else
				{
					fileContent = File.ReadAllText(sourceLocation.SourceFilename);
					lastFileContent = fileContent;
				}
			}

			lbSourceFilename.Text = sourceLocation.SourceFilename != null ? sourceLocation.SourceFilename : string.Empty;
			rtbSource.Text = fileContent;
			toolStripStatusLabel1.Text = string.Empty;

			if (sourceLocation.SourceFilename == null)
				return;

			int length = fileContent.Length;
			int startPosition = -1;
			int endPosition = -1;

			int currentLineAtStart = 0;
			int currentLineAtEnd = 0;

			int at = 0;
			int currentLine = 1;

			while (at < length)
			{
				char c = fileContent[at++];

				if (c == '\n')
					currentLine++;

				if (currentLine == sourceLocation.StartLine && startPosition < 0)
				{
					startPosition = at + sourceLocation.StartColumn - 1;
					currentLineAtStart = currentLine;
				}
				else if (currentLine == sourceLocation.EndLine && endPosition < 0)
				{
					endPosition = at + sourceLocation.EndColumn - 1;
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
			lbSourceFilename.Text = Path.GetFileName(sourceLocation.SourceFilename);
			toolStripStatusLabel1.Text = "Label: " + sourceLocation.Label + " / " + sourceLocation.SourceLabel + " - Lines " + sourceLocation.StartLine + "." + sourceLocation.StartColumn + "  to " + sourceLocation.EndLine + "." + sourceLocation.EndColumn;

			rtbSource.ScrollToCaret();

			lastSourceLocation = sourceLocation;
		}
	}
}
