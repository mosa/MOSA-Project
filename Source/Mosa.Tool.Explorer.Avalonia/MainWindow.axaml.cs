using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.CompilerStages;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.Framework.Stages.Diagnostic;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Compiler.MosaTypeSystem.CLR;
using Mosa.Tool.Explorer.Avalonia.CompilerStage;
using Mosa.Tool.Explorer.Avalonia.Stages;
using Mosa.Utility.Configuration;
using Timer = System.Timers.Timer;

namespace Mosa.Tool.Explorer.Avalonia;

public partial class MainWindow : Window
{
	private readonly object statusLock = new object();

	private readonly CompilerData compilerData = new CompilerData();
	private readonly MethodStore methodStore = new MethodStore();

	private readonly ObservableCollection<CounterEntry> counterCollection = new ObservableCollection<CounterEntry>();
	private readonly ObservableCollection<CounterEntry> compilerCounterCollection = new ObservableCollection<CounterEntry>();

	private readonly MosaSettings mosaSettings = new MosaSettings();

	private MosaCompiler compiler;
	private int completedMethods;
	private string currentLogSection = string.Empty;

	private MosaMethod currentMethod;
	private MethodData currentMethodData;

	private string status;
	private int totalMethods, transformStep;
	private TypeSystemTree typeSystemTree;

	private bool graphvizFound;

	private string lastOpenedFile;

	public MainWindow()
	{
		InitializeComponent();

		var timer = new Timer();
		timer.Elapsed += (_, _) => Dispatcher.UIThread.Post(() =>
		{
			UpdateProgressBar();
			RefreshCompilerSelectionDropDown();
			RefreshCompilerLog();
			RefreshStatus();
		});
		timer.Start();

		CountersGrid.ItemsSource = counterCollection;
		CompilerCountersGrid.ItemsSource = compilerCounterCollection;

		ClearAll();

		PlatformRegistry.Add(new Compiler.x86.Architecture());
		PlatformRegistry.Add(new Compiler.x64.Architecture());
		PlatformRegistry.Add(new Compiler.ARM32.Architecture());
		//PlatformRegistry.Add(new Compiler.ARM64.Architecture());
	}

	private void Control_OnLoaded(object _, RoutedEventArgs e)
	{
		SetStatus("Ready!");

		if (mosaSettings.SourceFiles == null || mosaSettings.SourceFiles.Count == 0)
			return;

		var fileName = Path.GetFullPath(mosaSettings.SourceFiles[0]);

		lastOpenedFile = fileName;
		UpdateSettings(fileName);
		LoadAssembly();

		if (mosaSettings.ExplorerStart)
			Compile_OnClick(null, null);
	}

	public void Initialize(string[] args)
	{
		mosaSettings.SetDefaultSettings();
		mosaSettings.LoadAppLocations();
		mosaSettings.LoadArguments(args);
		mosaSettings.NormalizeSettings();
		mosaSettings.ResolveDefaults();

		SetRequiredSettings();

		graphvizFound = File.Exists(mosaSettings.GraphvizApp);

		UpdateDisplay();
	}

	private void SetRequiredSettings()
	{
		mosaSettings.TraceLevel = 10;
		mosaSettings.EmulatorSerial = "none";
		mosaSettings.LauncherExit = false;
	}

	private void UpdateDisplay()
	{
		Inline.IsChecked = mosaSettings.InlineMethods;
		StaticSingleAssignment.IsChecked = mosaSettings.SSA;
		BasicOptimizations.IsChecked = mosaSettings.BasicOptimizations;
		ConditionalConstantPropagation.IsChecked = mosaSettings.SparseConditionalConstantPropagation;
		Devirtualization.IsChecked = mosaSettings.Devirtualization;
		InlineExplicit.IsChecked = mosaSettings.InlineExplicit;
		PlatformOptimizations.IsChecked = mosaSettings.PlatformOptimizations;
		LongExpansion.IsChecked = mosaSettings.LongExpansion;
		TwoOptimizationPasses.IsChecked = mosaSettings.TwoPassOptimization;
		LoopInvariantCodeMotion.IsChecked = mosaSettings.LoopInvariantCodeMotion;
		ValueNumbering.IsChecked = mosaSettings.ValueNumbering;
		BitTracker.IsChecked = mosaSettings.BitTracker;
		LoopRangeTracker.IsChecked = mosaSettings.LoopRangeTracker;
		BinaryCodeGeneration.IsChecked = mosaSettings.EmitBinary;
		MethodScanner.IsChecked = mosaSettings.MethodScanner;
		MultiThreading.IsChecked = mosaSettings.Multithreading;
		TreeFilter.Text = mosaSettings.ExplorerFilter;
		DebugDiagnostic.IsChecked = mosaSettings.DebugDiagnostic;
		CodeSizeReduction.IsChecked = mosaSettings.ReduceCodeSize;

		Platform.SelectedIndex = mosaSettings.Platform.ToLowerInvariant() switch
		{
			"x86" => 0,
			"x64" => 1,
			"arm32" => 2,
			//"arm64" => 3,
			_ => Platform.SelectedIndex
		};

		DisplayGraphviz.IsChecked = graphvizFound;
		DisplayGraphviz.IsEnabled = graphvizFound;
	}

	private async void Open_OnClick(object _, RoutedEventArgs e)
	{
		var topLevel = GetTopLevel(this);
		if (topLevel == null)
			return;

		var files = await topLevel.StorageProvider.OpenFilePickerAsync(Utils.LibraryOpenOptions);
		if (files.Count != 1)
			return;

		lastOpenedFile = files[0].Path.LocalPath;
		OpenFile();
	}

	private void Refresh_OnClick(object _, RoutedEventArgs e)
	{
		if (File.Exists(lastOpenedFile))
			OpenFile();
	}

	private void Compile_OnClick(object _, RoutedEventArgs e)
	{
		if (compiler == null)
			return;

		compilerData.Stopwatch.Restart();
		compiler.ScheduleAll();

		MainPanel.IsEnabled = false;

		ThreadPool.QueueUserWorkItem(_ =>
		{
			try
			{
				compiler.Compile();
			}
			finally
			{
				Dispatcher.UIThread.Post(CompileCompleted);
			}
		});
	}

	private void EnableAllOptimizations_OnClick(object _, RoutedEventArgs e) => ToggleOptimizations(true);

	private void DisableAllOptimizations_OnClick(object _, RoutedEventArgs e) => ToggleOptimizations(false);

	private void ToggleOptimizations(bool state)
	{
		StaticSingleAssignment.IsChecked = state;
		BasicOptimizations.IsChecked = state;
		ValueNumbering.IsChecked = state;
		ConditionalConstantPropagation.IsChecked = state;
		BinaryCodeGeneration.IsChecked = state;
		Inline.IsChecked = state;
		InlineExplicit.IsChecked = state;
		LongExpansion.IsChecked = state;
		TwoOptimizationPasses.IsChecked = state;
		BitTracker.IsChecked = state;
		LoopRangeTracker.IsChecked = state;
		LoopInvariantCodeMotion.IsChecked = state;
		PlatformOptimizations.IsChecked = state;
		Devirtualization.IsChecked = state;
		CodeSizeReduction.IsChecked = state;
	}

	private void CompileCompleted()
	{
		MainPanel.IsEnabled = true;

		SetStatus("Compiled!");

		compilerData.SortLog("Counters");

		if (typeSystemTree != null)
			typeSystemTree.Update();
		else
			CreateTree();
	}

	private void Quit_OnClick(object _, RoutedEventArgs e) => Close();

	private void DisplayClick(object _, RoutedEventArgs e)
	{
		UpdateInstructions();
		UpdateTransforms();
	}

	private void ShowSizes_OnClick(object _, RoutedEventArgs e)
	{
		CreateTree();
		UpdateInstructions();
		UpdateTransforms();
	}

	private async void DumpAllMethodStages_OnClick(object _, RoutedEventArgs e)
	{
		if (currentMethod == null)
			return;

		var topLevel = GetTopLevel(this);
		if (topLevel == null)
			return;

		var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(Utils.FolderOpenOptions);
		if (folders.Count != 1)
			return;

		var path = folders[0].Path.LocalPath;

		InstructionsStage.SelectedIndex = 0;

		for (; ; )
		{
			UpdateInstructionStageSelection();

			var stage = InstructionsStage.SelectionBoxItem?.ToString()?
				.Replace("\\", " - ")
				.Replace("/", " - ");

			await File.WriteAllTextAsync(Path.Combine(path, stage + "-stage.txt"), Instructions.Text);

			if (InstructionsStage.Items.Count == InstructionsStage.SelectedIndex + 1)
				break;

			InstructionsStage.SelectedIndex++;
		}

		DebugStage.SelectedIndex = 0;

		for (; ; )
		{
			UpdateDebugResults();

			var stage = DebugStage.SelectionBoxItem?.ToString()?
				.Replace("\\", " - ")
				.Replace("/", " - ");

			await File.WriteAllTextAsync(Path.Combine(path, stage + "-debug.txt"), Debug.Text);

			if (DebugStage.Items.Count == DebugStage.SelectedIndex + 1)
				break;

			DebugStage.SelectedIndex++;
		}
	}

	private void Platform_OnSelectionChanged(object _, SelectionChangedEventArgs e)
	{
		if (TreeView != null)
			ClearAll();
	}

	private void TreeFilter_OnTextChanged(object _, TextChangedEventArgs e) => CreateTree();

	private void TreeView_OnSelectionChanged(object _, SelectionChangedEventArgs e)
	{
		currentMethod = GetCurrentMethod();
		currentMethodData = GetCurrentMethodData();

		if (compiler == null)
			return;

		Instructions.Clear();

		if (currentMethod == null)
			return;

		compilerData.Stopwatch.Reset();
		ThreadPool.QueueUserWorkItem(_ => compiler.CompileSingleMethod(currentMethod));
	}

	private void TabControl_OnSelectionChanged(object _, SelectionChangedEventArgs e)
	{
		compilerData.DirtyLog = true;

		if (TabControl != null)
			RefreshCompilerLog();
	}

	private void InstructionsStage_OnSelectionChanged(object _, SelectionChangedEventArgs e) => UpdateInstructionStageSelection();

	private void UpdateInstructionStageSelection()
	{
		var previousItemLabel = InstructionsBlock.SelectionBoxItem;

		UpdateInstructionBlocks();

		if (previousItemLabel != null && InstructionsBlock.Items.Contains(previousItemLabel))
			InstructionsBlock.SelectedItem = previousItemLabel;
		else
			InstructionsBlock.SelectedIndex = 0;

		UpdateInstructions();
	}

	private void UpdateInstructionBlocks()
	{
		var records = GetCurrentInstructionLines();

		InstructionsBlock.Items.Clear();
		InstructionsBlock.Items.Add("All");

		var labels = ExtractLabels(records);
		if (labels == null)
			return;

		foreach (var label in labels)
			InstructionsBlock.Items.Add(label);
	}

	private static List<string> ExtractLabels(List<InstructionRecord> records)
	{
		if (records == null)
			return null;

		var labels = new List<string>();

		foreach (var record in records)
			if (record.IsStartBlock)
				labels.Add(record.BlockLabel);

		return labels;
	}

	private void InstructionsBlock_OnSelectionChanged(object _, SelectionChangedEventArgs e) => UpdateInstructions();

	private void SaveA_OnClick(object _, RoutedEventArgs e)
		=> File.WriteAllText(Path.Combine(mosaSettings.TemporaryFolder, "A.txt"), Instructions.Text);

	private void SaveB_OnClick(object _, RoutedEventArgs e)
		=> File.WriteAllText(Path.Combine(mosaSettings.TemporaryFolder, "B.txt"), Instructions.Text);

	private void DebugStage_OnSelectionChanged(object _, SelectionChangedEventArgs e) => UpdateDebugResults();

	private void DisplayGraphviz_OnIsCheckedChanged(object _, RoutedEventArgs e) => UpdateGraphviz();

	private void TransformsStage_OnSelectionChanged(object _, SelectionChangedEventArgs e)
	{
		var previousItemLabel = TransformsBlock.SelectionBoxItem;

		UpdateTransformLabels();

		if (previousItemLabel != null && TransformsBlock.Items.Contains(previousItemLabel))
			TransformsBlock.SelectedItem = previousItemLabel;
		else
			TransformsBlock.SelectedIndex = 0;

		SetTransformationStep(0);
		UpdateTransforms();
		PopulateTransformList();
	}

	private void TransformsBlock_OnSelectionChanged(object _, SelectionChangedEventArgs e) => UpdateTransforms();

	private void Save1_OnClick(object _, RoutedEventArgs e)
		=> File.WriteAllText(Path.Combine(mosaSettings.TemporaryFolder, "1.txt"), Transforms.Text);

	private void Save2_OnClick(object _, RoutedEventArgs e)
		=> File.WriteAllText(Path.Combine(mosaSettings.TemporaryFolder, "2.txt"), Transforms.Text);

	private void First_OnClick(object _, RoutedEventArgs e) => SetTransformationStep(0);

	private void Previous_OnClick(object _, RoutedEventArgs e) => SetTransformationStep(transformStep - 1);

	private void Next_OnClick(object _, RoutedEventArgs e) => SetTransformationStep(transformStep + 1);

	private void Last_OnClick(object _, RoutedEventArgs e) => SetTransformationStep(int.MaxValue);

	private void SetBlock_OnIsCheckedChanged(object _, RoutedEventArgs e)
		=> TransformsGrid_OnSelectionChanged(null, null);

	private void TransformsGrid_OnSelectionChanged(object _, SelectionChangedEventArgs e)
	{
		if (TransformsGrid.SelectedItem is not TransformEntry entry)
			return;

		if (SetBlock.IsChecked != null && SetBlock.IsChecked!.Value && !string.IsNullOrEmpty(entry.Block))
			TransformsBlock.SelectedItem = entry.Block;

		SetTransformationStep(entry.ID);
	}

	private void CountersGridFilter_OnTextChanged(object _, TextChangedEventArgs e) => UpdateCounters();

	private async void CopyCountersText_OnClick(object _, RoutedEventArgs e)
	{
		if (Clipboard == null)
		{
			SetStatus("Error: No Clipboard Detected.");
			return;
		}

		await Clipboard.SetTextAsync(CreateText(currentMethodData.Counters));
		SetStatus("Text Copied!");
	}

	private void CompilerLogsSection_OnSelectionChanged(object _, SelectionChangedEventArgs e)
	{
		var formatted = CompilerLogsSection.SelectionBoxItem?.ToString();
		if (formatted == null)
			return;

		currentLogSection = formatted[(formatted.IndexOf(' ') + 1)..];
		compilerData.DirtyLog = true;

		RefreshCompilerLog();
	}

	private void CompilerCountersGridFilter_OnTextChanged(object _, TextChangedEventArgs e) => UpdateCompilerCounters();

	private async void CopyCompilerCountersText_OnClick(object _, RoutedEventArgs e)
	{
		if (Clipboard == null)
		{
			SetStatus("Error: No Clipboard Detected.");
			return;
		}

		var lines = compilerData.GetLog("Counters");
		if (lines == null)
			return;

		await Clipboard.SetTextAsync(CreateText(lines));
		SetStatus("Text Copied!");
	}

	private void SetTransformationStep(int step)
	{
		if (step <= 0)
			step = 0;

		var max = GetTransformMaxSteps();

		if (step > max - 1)
			step = max - 1;

		transformStep = step;

		Current.Content = step;
		Total.Content = max - 1;
		TransformsGrid.SelectedIndex = step;

		UpdateTransforms();
	}

	private int GetTransformMaxSteps()
	{
		if (currentMethodData == null)
			return 0;

		var stage = TransformsStage.SelectionBoxItem?.ToString();
		if (stage == null)
			return 0;

		var logs = currentMethodData.TransformLogs[stage];
		return logs.Count;
	}

	private void PopulateTransformList()
	{
		if (currentMethodData == null)
			return;

		var stage = TransformsStage.SelectionBoxItem?.ToString();
		if (stage == null)
			return;

		if (!currentMethodData.DebugLogs.TryGetValue(stage, out var debug))
			return;

		if (debug.Contains("*** Pass"))
			return;

		var list = new List<TransformEntry> { new TransformEntry { ID = 0, Name = "***Start***" } };
		var pass = 0;
		TransformEntry entry = null;

		foreach (var line in debug)
		{
			if (string.IsNullOrEmpty(line))
				continue;

			if (line.StartsWith("*** Pass"))
			{
				pass = Convert.ToInt32(line[10..]);
				continue;
			}

			if (line.StartsWith("Merge Blocking: ") || line.StartsWith("Removed Unreachable Block:"))
				continue;

			var parts = line.Split('\t');
			if (parts.Length != 2)
				continue;

			var part1 = parts[1][1..].Trim();

			if (parts[0].StartsWith("L_") && entry != null)
			{
				entry.Block = parts[0].TrimEnd();
				entry.Before = part1;
				continue;
			}

			if (parts[0].StartsWith(' ') && entry != null)
			{
				entry.After = part1;
				continue;
			}

			entry = new TransformEntry
			{
				ID = Convert.ToInt32(parts[0].Trim()),
				Name = part1,
				Pass = pass
			};

			list.Add(entry);
		}

		TransformsGrid.ItemsSource = list;
	}

	private void UpdateTransformLabels()
	{
		var lines = GetCurrentTransformRecords();

		TransformsBlock.Items.Clear();
		TransformsBlock.Items.Add("All");

		var labels = ExtractLabels(lines);
		if (labels == null)
			return;

		foreach (var label in labels)
			TransformsBlock.Items.Add(label);
	}

	private void UpdateDebugResults()
	{
		Debug.Clear();

		var lines = GetCurrentDebugLines();
		if (lines == null)
			return;

		Debug.Text = CreateText(lines);

		UpdateGraphviz();
	}

	private List<string> GetCurrentDebugLines()
	{
		if (currentMethodData == null)
			return null;

		var stage = DebugStage.SelectionBoxItem?.ToString();
		return stage == null ? null : currentMethodData.DebugLogs[stage];
	}

	private void UpdateGraphviz() => DebugPanel.IsVisible = ShowGraphviz();

	private bool ShowGraphviz()
	{
		DebugPanel.Children.Clear();

		if (!graphvizFound)
			return false;

		if (DisplayGraphviz.IsChecked != null && !DisplayGraphviz.IsChecked.Value)
			return false;

		if (string.IsNullOrEmpty(Debug.Text) || !Debug.Text.StartsWith("digraph blocks"))
			return false;

		var dot = Path.GetRandomFileName();
		var img = Path.GetRandomFileName();

		try
		{
			File.WriteAllText(dot, Debug.Text);

			var startInfo = new ProcessStartInfo
			{
				FileName = mosaSettings.GraphvizApp,
				Arguments = $"-Tpng -o \"{img}\" \"{dot}\"",
				CreateNoWindow = true
			};

			var process = Process.Start(startInfo);
			process?.WaitForExit();

			using var stream = File.OpenRead(img);

			var bitmap = new Bitmap(stream);
			var image = new Image
			{
				Source = bitmap
			};

			DebugPanel.Children.Add(image);
		}
		finally
		{
			File.Delete(dot);
			File.Delete(img);
		}

		return true;
	}

	private void OpenFile()
	{
		UpdateSettings();
		UpdateSettings(lastOpenedFile);
		LoadAssembly();
	}

	private CompilerHooks CreateCompilerHooks() => new CompilerHooks
	{
		ExtendCompilerPipeline = ExtendCompilerPipeline,
		ExtendMethodCompilerPipeline = ExtendMethodCompilerPipeline,

		NotifyProgress = NotifyProgress,
		NotifyEvent = NotifyEvent,
		NotifyTraceLog = NotifyTraceLog,
		NotifyMethodCompiled = NotifyMethodCompiled,
		NotifyMethodInstructionTrace = NotifyMethodInstructionTrace,
		NotifyMethodTranformTrace = NotifyMethodTransformTrace,
		GetMethodTraceLevel = GetMethodTraceLevel
	};

	private static void ExtendCompilerPipeline(Pipeline<BaseCompilerStage> pipeline)
		=> pipeline.InsertAfterFirst<TypeInitializerStage>(new ExplorerMethodCompileTimeStage());

	private void ExtendMethodCompilerPipeline(Pipeline<BaseMethodCompilerStage> pipeline, MosaSettings mosaSettings)
	{
		pipeline.Add(new DisassemblyStage());
		pipeline.Add(new DebugInfoStage());

		pipeline.InsertAfterLast<FastBlockOrderingStage>(new LoopAnalysisStage());
		pipeline.InsertAfterLast<FastBlockOrderingStage>(new DominanceAnalysisStage());

		if (Dispatcher.UIThread.Invoke(() => DebugDiagnostic.IsChecked))
		{
			for (var i = 1; i < pipeline.Count; i += 2)
				pipeline.Insert(i, new ControlFlowGraphStage());
		}
		else
		{
			if (mosaSettings.InlineMethods || mosaSettings.InlineExplicit)
			{
				pipeline.InsertBefore<InlineStage>(new ControlFlowGraphStage());
				pipeline.InsertAfterLast<InlineStage>(new ControlFlowGraphStage());
			}

			if (mosaSettings.SSA)
			{
				pipeline.InsertBefore<EnterSSAStage>(new DominanceAnalysisStage());
				pipeline.InsertBefore<EnterSSAStage>(new ControlFlowGraphStage());
			}

			pipeline.InsertAfterLast<FastBlockOrderingStage>(new ControlFlowGraphStage());
			pipeline.Add(new ControlFlowGraphStage());
		}
	}

	private void NotifyProgress(int total, int completed)
	{
		totalMethods = total;
		completedMethods = completed;
	}

	private void NotifyEvent(CompilerEvent compilerEvent, string message, int threadID)
	{
		if (compilerEvent != CompilerEvent.Counter)
		{
			var newStatus = compilerEvent.ToText();
			if (!string.IsNullOrWhiteSpace(message))
				newStatus += $": {message}";

			lock (statusLock)
				status = newStatus;
		}

		compilerData.AddTraceEvent(compilerEvent, message, threadID);

		currentMethodData ??= GetCurrentMethodData();
	}

	private void NotifyTraceLog(TraceLog traceLog)
	{
		switch (traceLog.Type)
		{
			case TraceType.MethodDebug when traceLog.Lines.Count == 0: return;
			case TraceType.MethodDebug:
				{
					var stageSection = traceLog.Stage;
					if (traceLog.Section != null)
						stageSection = $"{stageSection}-{traceLog.Section}";

					methodStore.SetDebugStageInformation(traceLog.Method, stageSection, traceLog.Lines, traceLog.Version);
					break;
				}
			case TraceType.MethodCounters:
				{
					methodStore.SetMethodCounterInformation(traceLog.Method, traceLog.Lines, traceLog.Version);
					break;
				}
			case TraceType.MethodInstructions:
				{
					NotifyMethodInstructionTraceResponse(traceLog);
					break;
				}
			case TraceType.GlobalDebug:
				{
					compilerData.UpdateLog(traceLog.Section, traceLog.Lines, traceLog.Section == currentLogSection);
					break;
				}
		}
	}

	private void NotifyMethodCompiled(MosaMethod method)
	{
		if (method == currentMethod)
			Dispatcher.UIThread.Post(UpdateMethodInformation);
	}

	private CompilerHooks.NotifyTraceLogHandler NotifyMethodInstructionTrace(MosaMethod method)
	{
		if (method != currentMethod)
			return null;

		return NotifyMethodInstructionTraceResponse;
	}

	private CompilerHooks.NotifyTraceLogHandler NotifyMethodTransformTrace(MosaMethod method)
	{
		if (method != currentMethod)
			return null;

		return NotifyMethodTransformTraceResponse;
	}

	private int? GetMethodTraceLevel(MosaMethod method) => method == currentMethod ? 10 : -1;

	private void NotifyMethodTransformTraceResponse(TraceLog traceLog)
		=> methodStore.SetTransformTraceInformation(traceLog.Method, traceLog.Stage, traceLog.Lines, traceLog.Version, traceLog.Step);

	private void NotifyMethodInstructionTraceResponse(TraceLog traceLog)
		=> methodStore.SetInstructionTraceInformation(traceLog.Method, traceLog.Stage, traceLog.Lines, traceLog.Version);

	private void UpdateMethodInformation()
	{
		UpdateInstructionStages();
		UpdateDebugStages();
		UpdateCounters();
		UpdateTransformStages();
	}

	private void UpdateInstructions()
	{
		Instructions.Clear();

		if (currentMethod == null)
			return;

		var records = GetCurrentInstructionLines();
		var label = InstructionsBlock.SelectionBoxItem?.ToString();

		SetStatus(currentMethod.FullName);

		if (records == null)
			return;

		if (string.IsNullOrWhiteSpace(label) || label == "All")
			label = string.Empty;

		Instructions.Text = FormatInstructions.Format(records, label, !ShowOperandTypes.IsChecked,
			RemoveIrNop.IsChecked, LineBetweenBlocks.IsChecked);
	}

	private void UpdateTransforms()
	{
		Transforms.Clear();

		if (currentMethod == null)
			return;

		var records = GetCurrentTransformRecords();
		var label = TransformsBlock.SelectionBoxItem?.ToString();

		if (records == null)
			return;

		if (string.IsNullOrWhiteSpace(label) || label == "All")
			label = string.Empty;

		Transforms.Text = FormatInstructions.Format(records, label, !ShowOperandTypes.IsChecked,
			RemoveIrNop.IsChecked, LineBetweenBlocks.IsChecked);
	}

	private List<InstructionRecord> GetCurrentTransformRecords()
	{
		if (currentMethodData == null)
			return null;

		var stage = TransformsStage.SelectionBoxItem?.ToString();
		if (stage == null)
			return null;

		var logs = currentMethodData.TransformLogs[stage];
		logs.TryGetValue(transformStep, out var log);

		return log;
	}

	private List<InstructionRecord> GetCurrentInstructionLines()
	{
		if (currentMethodData == null)
			return null;

		var stage = InstructionsStage.SelectionBoxItem?.ToString();
		return stage == null ? null : currentMethodData.InstructionLogs[stage];
	}

	private void UpdateInstructionStages()
	{
		InstructionsStage.Items.Clear();

		if (currentMethodData == null)
			return;

		foreach (var stage in currentMethodData.OrderedStageNames)
			InstructionsStage.Items.Add(stage);

		InstructionsStage.SelectedIndex = InstructionsStage.Items.Count == 0 ? -1 : 0;
	}

	private void UpdateDebugStages()
	{
		DebugStage.Items.Clear();

		if (currentMethodData == null)
			return;

		foreach (var stage in currentMethodData.OrderedDebugStageNames)
			DebugStage.Items.Add(stage);

		if (DebugStage.Items.Count > 0)
			DebugStage.SelectedIndex = 0;
	}

	private void UpdateCounters()
	{
		if (currentMethodData == null)
			return;

		counterCollection.Clear();

		var filter = CountersGridFilter.Text ?? string.Empty;

		foreach (var line in currentMethodData.Counters)
		{
			if (!line.Contains(filter))
				continue;

			var entry = ExtractCounterData(line);
			counterCollection.Add(entry);
		}
	}

	private void UpdateTransformStages()
	{
		TransformsStage.Items.Clear();

		if (currentMethodData == null)
			return;

		foreach (var stage in currentMethodData.OrderedTransformStageNames)
			TransformsStage.Items.Add(stage);

		if (TransformsStage.Items.Count > 0)
			TransformsStage.SelectedIndex = 0;
	}

	private MosaMethod GetCurrentMethod() => ((TreeViewItem)TreeView.SelectedItem)?.Tag as MosaMethod;

	private MethodData GetCurrentMethodData() => currentMethod == null ? null : methodStore.GetMethodData(currentMethod, false);

	private void CreateTree()
	{
		if (compiler == null)
			return;

		if (compiler.TypeSystem == null || compiler.TypeLayout == null)
		{
			typeSystemTree = null;
			TreeView.Items.Clear();
			return;
		}

		var included = GetIncluded(TreeFilter.Text, out MosaUnit selected);

		typeSystemTree = new TypeSystemTree(TreeView, compiler.TypeSystem, compiler.TypeLayout, ShowSizes.IsChecked, included);

		Select(selected);
	}

	private HashSet<MosaUnit> GetIncluded(string value, out MosaUnit selected)
	{
		value = value.Trim();
		selected = null;

		if (string.IsNullOrWhiteSpace(value))
			return null;

		if (value.Length < 1)
			return null;

		var include = new HashSet<MosaUnit>();

		MosaUnit typeSelected = null;
		MosaUnit methodSelected = null;

		foreach (var type in compiler.TypeSystem.AllTypes)
		{
			if (string.IsNullOrEmpty(type.FullName))
				continue;

			var typeIncluded = false;
			var typeMatch = type.FullName.Contains(value);

			if (typeMatch)
			{
				include.Add(type);
				include.AddIfNew(type.Module);

				typeSelected ??= type;
			}

			foreach (var method in type.Methods)
			{
				if (string.IsNullOrEmpty(method.FullName))
					continue;

				var methodMatch = method.FullName.Contains(value);
				if (!typeMatch && !methodMatch)
					continue;

				include.Add(method);
				include.AddIfNew(type);
				include.AddIfNew(type.Module);

				if (methodMatch && methodSelected == null)
					methodSelected = method;
			}

			foreach (var property in type.Properties)
			{
				if (string.IsNullOrEmpty(property.FullName))
					continue;

				if (!typeIncluded && !property.FullName.Contains(value))
					continue;

				include.Add(property);
				include.AddIfNew(type);
				include.AddIfNew(type.Module);
			}
		}

		selected = methodSelected ?? typeSelected;

		return include;
	}

	private void Select(MosaUnit selected)
	{
		if (selected == null)
			return;

		foreach (TreeViewItem node in TreeView.Items)
			if (Select(node, selected))
				return;
	}

	private bool Select(TreeViewItem node, MosaUnit selected)
	{
		if (node == null)
			return false;

		if (node.Tag != null && node.Tag == selected)
		{
			TreeView.SelectedItem = node;
			return true;
		}

		foreach (TreeViewItem children in node.Items)
			if (Select(children, selected))
				return true;

		return false;
	}

	private void SetStatus(string newStatus)
		=> Status.Content = $"{compilerData.Stopwatch.Elapsed.TotalSeconds:00.00} | {newStatus}";

	private void UpdateSettings()
	{
		mosaSettings.MethodScanner = MethodScanner.IsChecked;
		mosaSettings.EmitBinary = BinaryCodeGeneration.IsChecked;
		mosaSettings.Platform = Platform.SelectionBoxItem?.ToString();
		mosaSettings.Multithreading = MultiThreading.IsChecked;
		mosaSettings.SSA = StaticSingleAssignment.IsChecked;
		mosaSettings.BasicOptimizations = BasicOptimizations.IsChecked;
		mosaSettings.ValueNumbering = ValueNumbering.IsChecked;
		mosaSettings.SparseConditionalConstantPropagation = ConditionalConstantPropagation.IsChecked;
		mosaSettings.Devirtualization = Devirtualization.IsChecked;
		mosaSettings.BitTracker = BitTracker.IsChecked;
		mosaSettings.LoopRangeTracker = LoopRangeTracker.IsChecked;
		mosaSettings.LoopInvariantCodeMotion = LoopInvariantCodeMotion.IsChecked;
		mosaSettings.LongExpansion = LongExpansion.IsChecked;
		mosaSettings.TwoPassOptimization = TwoOptimizationPasses.IsChecked;
		mosaSettings.PlatformOptimizations = PlatformOptimizations.IsChecked;
		mosaSettings.InlineMethods = Inline.IsChecked;
		mosaSettings.InlineExplicit = InlineExplicit.IsChecked;
		mosaSettings.ReduceCodeSize = CodeSizeReduction.IsChecked;

		mosaSettings.TraceLevel = 10;
		//mosaSettings.InlineMaximum = 12;
		//mosaSettings.InlineAggressiveMaximum = 24;
		mosaSettings.MultibootVersion = "v2";
	}

	private void UpdateSettings(string path)
	{
		// Source Files
		mosaSettings.ClearSourceFiles();
		mosaSettings.AddSourceFile(path);

		// Search Paths
		mosaSettings.ClearSearchPaths();
		mosaSettings.AddSearchPath(Path.GetDirectoryName(path));

		mosaSettings.ResolveFileAndPathSettings();
		mosaSettings.AddStandardPlugs();
		mosaSettings.ExpandSearchPaths();
	}

	private void LoadAssembly()
	{
		ClearAll();
		UpdateSettings();

		compiler = new MosaCompiler(mosaSettings, CreateCompilerHooks(), new ClrModuleLoader(), new ClrTypeResolver());

		ThreadPool.QueueUserWorkItem(_ =>
		{
			Dispatcher.UIThread.Post(() => SetStatus("Loading Types..."));
			compiler.Load();

			Dispatcher.UIThread.Post(() => SetStatus("Building Type System Tree..."));
			Dispatcher.UIThread.Post(CreateTree);

			Dispatcher.UIThread.Post(() => SetStatus("Assemblies Loaded!"));
		});
	}

	private void ClearAll()
	{
		compiler = null;
		typeSystemTree = null;

		TreeView.Items.Clear();
		Instructions.Clear();
		Debug.Clear();

		methodStore.Clear();
		counterCollection.Clear();
		compilerCounterCollection.Clear();

		ClearAllLogs();
	}

	private void ClearAllLogs()
	{
		compilerData.ClearAllLogs();
		ClearSectionDropDown();
		CompilerLogsSection.SelectedIndex = 0;
	}

	private void ClearSectionDropDown()
	{
		CompilerLogsSection.Items.Clear();

		compilerData.DirtyLogSections = true;
		compilerData.DirtyLog = true;

		RefreshCompilerSelectionDropDown();
		RefreshCompilerLog();
	}

	private void UpdateProgressBar()
	{
		ProgressBar.Maximum = totalMethods;
		ProgressBar.Value = completedMethods;
	}

	private void RefreshCompilerSelectionDropDown()
	{
		if (!compilerData.DirtyLogSections)
			return;

		compilerData.DirtyLogSections = false;

		lock (compilerData.Logs)
			for (var i = CompilerLogsSection.Items.Count; i < compilerData.LogSections.Count; i++)
				CompilerLogsSection.Items.Add($"[{i}] {compilerData.LogSections[i]}");
	}

	private void RefreshCompilerLog()
	{
		if (!compilerData.DirtyLog)
			return;

		switch (TabControl.SelectedIndex)
		{
			// Compiler Logs
			case 4:
				{
					CompilerLogs.Text = CreateText(compilerData.GetLog(currentLogSection));
					break;
				}
			// Compiler Counters
			case 5:
				{
					UpdateCompilerCounters();
					break;
				}
		}

		compilerData.DirtyLog = false;
	}

	private void RefreshStatus()
	{
		lock (statusLock)
		{
			if (status == null)
				return;

			SetStatus(status);
			status = null;
		}
	}

	private static string CreateText(List<string> list)
	{
		if (list == null)
			return string.Empty;

		var sb = new StringBuilder();

		lock (list)
			foreach (var l in list)
				sb.AppendLine(l);

		return sb.ToString();
	}

	private void UpdateCompilerCounters()
	{
		compilerCounterCollection.Clear();

		var lines = compilerData.GetLog("Counters");
		if (lines == null)
			return;

		var filter = CompilerCountersGridFilter.Text ?? string.Empty;
		foreach (var line in lines)
		{
			if (!line.Contains(filter))
				continue;

			var entry = ExtractCounterData(line);
			compilerCounterCollection.Add(entry);
		}
	}

	private static CounterEntry ExtractCounterData(string line)
	{
		var index = line.IndexOf(':');
		var name = line[..index].Trim();
		var value = int.Parse(line[(index + 1)..].Trim());
		var entry = new CounterEntry(name, value);

		return entry;
	}
}
