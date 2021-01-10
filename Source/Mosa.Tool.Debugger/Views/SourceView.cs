// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.Debugger.DebugData;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Mosa.Tool.Debugger.Views
{
	public partial class SourceView : DebugDockContent
	{
		private readonly SourceLocation lastSourceLocation;
		private string lastFileContent;
		private readonly List<int> lines = new List<int>();

		public SourceView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
			ClearDisplay();
		}

		public override void OnRunning()
		{
			// Clear
		}

		protected override void ClearDisplay()
		{
			rtbSource.Text = string.Empty;
			toolStripStatusLabel1.Text = string.Empty;
			lbSourceFilename.Text = string.Empty;
		}

		protected override void UpdateDisplay()
		{
			var sourceLocation = Source.Find(DebugSource, InstructionPointer);

			if (sourceLocation == null)
				return;

			if (sourceLocation.SourceFilename == null)
				return;

			string fileContent = string.Empty;

			if (lastSourceLocation != null && lastSourceLocation.SourceFilename == sourceLocation.SourceFilename)
			{
				fileContent = lastFileContent;
			}
			else
			{
				fileContent = File.ReadAllText(sourceLocation.SourceFilename);
				lastFileContent = fileContent;
			}

			lbSourceFilename.Text = Path.GetFileName(sourceLocation.SourceFilename);
			rtbSource.Text = fileContent;

			int length = fileContent.Length;
			int startPosition = -1;
			int endPosition = -1;

			int currentLineAtStart = 0;
			int currentLineAtEnd = 0;

			int at = 0;
			int currentLine = 1;
			int startLine = 0;

			lines.Clear();
			lines.Add(0);

			while (at < length)
			{
				var c = fileContent[at++];

				if (c == '\n')
				{
					currentLine++;
					lines.Add(at);
				}

				if (currentLine == sourceLocation.StartLine && startPosition < 0)
				{
					startLine = currentLine;
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

			// capture the number of visible lines
			int totallines = rtbSource.Height / rtbSource.Font.Height;

			// Visible text position at 25% of height of control (if possible)
			int targetLine = startLine - (totallines / 4);
			int target = lines[targetLine < 0 ? 0 : targetLine];
			rtbSource.Select(target, 1);
			rtbSource.ScrollToCaret();

			int start = startPosition - currentLineAtStart + 1;
			int len = endPosition - startPosition - (currentLineAtEnd - currentLineAtStart);
			rtbSource.Select(start, len);

			rtbSource.SelectionBackColor = Color.Blue;
			rtbSource.SelectionColor = Color.White;

			toolStripStatusLabel1.Text = $"Label: {sourceLocation.Label}:{sourceLocation.SourceLabel} - Lines: {sourceLocation.StartLine}:{sourceLocation.StartColumn}  to {sourceLocation.EndLine}:{sourceLocation.EndColumn}";
		}
	}
}
