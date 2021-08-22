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

		private struct LineInfo
		{
			public string Line { get; set; }
			public int LineNbr { get; set; }
			public int Index { get; set; }
		}

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
			// capture the number of visible lines
			int totalLines = rtbSource.Height / rtbSource.Font.Height;

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
				lastSourceLocation = sourceLocation;
			}

			lbSourceFilename.Text = Path.GetFileName(sourceLocation.SourceFilename);
			rtbSource.Text = fileContent;

			// Visible text position at 25% of height of control (if possible)
			int caretLine = sourceLocation.StartLine - (totalLines / 4);

			if (caretLine < 0)
				caretLine = 0;

			int startPosition = 0;
			int endPosition = 0;
			int caretPosition = 0;

			int currentLine = 1;

			int length = fileContent.Length;

			for (int at = 0; at < length; at++)
			{
				var c = fileContent[at];

				if (c == '\n')
				{
					currentLine++;

					if (currentLine == caretLine && caretPosition == 0)
					{
						caretPosition = at;
					}

					if (currentLine == sourceLocation.StartLine && startPosition == 0)
					{
						startPosition = at;
					}

					if (currentLine == sourceLocation.EndLine && endPosition == 0)
					{
						endPosition = at;
					}
				}
			}

			if (startPosition == 0 || endPosition == 0)
				return;

			if (endPosition > length)
				endPosition = length;

			var caretIndex = caretPosition - caretLine;

			if (caretIndex < 0)
				caretIndex = 0;

			rtbSource.Select(caretIndex, 1);
			rtbSource.ScrollToCaret();

			var selectPosition = startPosition + sourceLocation.StartColumn + 1;
			var selectLength = endPosition + sourceLocation.EndColumn - startPosition - sourceLocation.StartColumn;
			var selectIndex = selectPosition - sourceLocation.StartLine;

			rtbSource.Select(selectIndex, selectLength);

			rtbSource.SelectionBackColor = Color.Blue;
			rtbSource.SelectionColor = Color.White;

			toolStripStatusLabel1.Text = $"Label: {sourceLocation.Label}:{sourceLocation.SourceLabel} - Lines: {sourceLocation.StartLine}:{sourceLocation.StartColumn} to {sourceLocation.EndLine}:{sourceLocation.EndColumn} - SelectLine/Select/CaretLine/Caret: {sourceLocation.StartLine},{selectPosition},{caretLine},{caretPosition}";
		}
	}
}
