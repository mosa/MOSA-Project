// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework;

public sealed class Bisector<TItem>
{
	private const int SingleItemCheckThreshold = 4;

	public enum BisectorLevel
	{
		Level1SingleItemSet,
		Level2Pairwise,
	}

	public enum BisectorPhase
	{
		Baseline,
		Reduction,
		SingleItemChecks,
		PairwiseChecks,
		Complete,
	}

	public enum BisectorResult
	{
		Pass,
		Fail,
	}

	public readonly record struct Pair(TItem Item1, TItem Item2);

	public sealed class BisectorStatus
	{
		internal BisectorStatus(int iteration, BisectorLevel level, BisectorPhase phase, int totalItemCount, int suspectItemCount, int confirmedBadItemCount, int confirmedBadPairCount, bool hasOutstandingExperiment, int pairwiseTestsCompleted, int pairwiseTestsRemaining)
		{
			Iteration = iteration;
			Level = level;
			Phase = phase;
			TotalItemCount = totalItemCount;
			SuspectItemCount = suspectItemCount;
			ConfirmedBadItemCount = confirmedBadItemCount;
			ConfirmedBadPairCount = confirmedBadPairCount;
			HasOutstandingExperiment = hasOutstandingExperiment;
			PairwiseTestsCompleted = pairwiseTestsCompleted;
			PairwiseTestsRemaining = pairwiseTestsRemaining;
		}

		public int Iteration { get; }
		public BisectorLevel Level { get; }
		public BisectorPhase Phase { get; }
		public int TotalItemCount { get; }
		public int SuspectItemCount { get; }
		public int ConfirmedBadItemCount { get; }
		public int ConfirmedBadPairCount { get; }
		public bool HasOutstandingExperiment { get; }
		public int PairwiseTestsCompleted { get; }
		public int PairwiseTestsRemaining { get; }
	}

	private enum ExperimentKind
	{
		Baseline,
		Reduction,
		SingleItemCheck,
		PairwiseCheck,
	}

	private sealed class OutstandingExperiment
	{
		public OutstandingExperiment(ExperimentKind kind, HashSet<TItem> disabledItems, List<TItem> subset, TItem item, Pair pair)
		{
			Kind = kind;
			DisabledItems = disabledItems;
			Subset = subset;
			Item = item;
			Pair = pair;
		}

		public ExperimentKind Kind { get; }
		public HashSet<TItem> DisabledItems { get; }
		public List<TItem> Subset { get; }
		public TItem Item { get; }
		public Pair Pair { get; }
	}

	private readonly IEqualityComparer<TItem> comparer;
	private readonly List<TItem> allItems = new();
	private readonly HashSet<TItem> confirmedBadItems;
	private readonly HashSet<TItem> unresolvedItems;
	private readonly List<Pair> confirmedBadPairs = new();

	private BisectorLevel currentLevel = BisectorLevel.Level1SingleItemSet;
	private BisectorPhase currentPhase = BisectorPhase.Baseline;
	private OutstandingExperiment outstandingExperiment;
	private List<TItem> currentSuspects = new();
	private List<TItem> reductionCandidates = new();
	private Queue<TItem> singleItemQueue = new();
	private List<Pair> pairwiseQueue = new();
	private int pairwiseIndex;
	private int pairwiseTestsCompleted;
	private bool foundBadItemInSingleItemChecks;
	private int iteration;

	public Bisector(IEnumerable<TItem> items, IEqualityComparer<TItem> comparer = null)
	{
		ArgumentNullException.ThrowIfNull(items);

		this.comparer = comparer ?? EqualityComparer<TItem>.Default;
		confirmedBadItems = new HashSet<TItem>(this.comparer);
		unresolvedItems = new HashSet<TItem>(this.comparer);

		foreach (var item in items)
		{
			if (item is null)
				throw new ArgumentException("Items cannot contain null entries.", nameof(items));

			if (!unresolvedItems.Add(item))
				continue;

			allItems.Add(item);
		}

		if (allItems.Count == 0)
			throw new ArgumentException("At least one item is required.", nameof(items));

		currentSuspects = new List<TItem>(allItems);
	}

	public bool IsComplete => currentPhase == BisectorPhase.Complete;

	public IReadOnlySet<TItem> ConfirmedBadItems => new HashSet<TItem>(confirmedBadItems, comparer);

	public IReadOnlyCollection<Pair> ConfirmedBadPairs => confirmedBadPairs.AsReadOnly();

	public IReadOnlySet<TItem> RemainingSuspectItems => new HashSet<TItem>(currentSuspects, comparer);

	public IReadOnlySet<TItem> GetNextDisabledItems()
	{
		if (outstandingExperiment != null)
			throw new InvalidOperationException("The current experiment result must be reported before requesting the next experiment.");

		if (IsComplete)
			throw new InvalidOperationException("The bisector session is complete.");

		while (true)
		{
			switch (currentPhase)
			{
				case BisectorPhase.Baseline:
					return CreateAndTrackExperiment(ExperimentKind.Baseline, CreateDisabledSet(), null, default, default);

				case BisectorPhase.Reduction:
					if (reductionCandidates.Count <= SingleItemCheckThreshold)
					{
						BeginSingleItemChecks(reductionCandidates);
						continue;
					}

					var midpoint = reductionCandidates.Count / 2;
					if (midpoint == 0)
						midpoint = 1;

					var enabledSubset = reductionCandidates.GetRange(0, midpoint);
					return CreateAndTrackExperiment(ExperimentKind.Reduction, CreateReductionDisabledSet(enabledSubset), enabledSubset, default, default);

				case BisectorPhase.SingleItemChecks:
					while (singleItemQueue.Count > 0)
					{
						var item = singleItemQueue.Dequeue();
						if (!unresolvedItems.Contains(item))
							continue;

						return CreateAndTrackExperiment(ExperimentKind.SingleItemCheck, CreateSingleItemDisabledSet(item), null, item, default);
					}

					FinishSingleItemChecks();
					continue;

				case BisectorPhase.PairwiseChecks:
					while (pairwiseIndex < pairwiseQueue.Count)
					{
						var pair = pairwiseQueue[pairwiseIndex++];
						if (!unresolvedItems.Contains(pair.Item1) || !unresolvedItems.Contains(pair.Item2))
							continue;

						return CreateAndTrackExperiment(ExperimentKind.PairwiseCheck, CreatePairwiseDisabledSet(pair), null, default, pair);
					}

					currentPhase = BisectorPhase.Complete;
					continue;

				case BisectorPhase.Complete:
					throw new InvalidOperationException("The bisector session is complete.");

				default:
					throw new InvalidOperationException("The bisector session is in an invalid state.");
			}
		}
	}

	public void AcceptResult(bool passed)
	{
		AcceptResult(passed ? BisectorResult.Pass : BisectorResult.Fail);
	}

	public void AcceptResult(BisectorResult result)
	{
		if (outstandingExperiment == null)
			throw new InvalidOperationException("There is no outstanding experiment result to report.");

		iteration++;

		var experiment = outstandingExperiment;
		outstandingExperiment = null;

		switch (experiment.Kind)
		{
			case ExperimentKind.Baseline:
				ProcessBaselineResult(result);
				return;

			case ExperimentKind.Reduction:
				ProcessReductionResult(experiment, result);
				return;

			case ExperimentKind.SingleItemCheck:
				ProcessSingleItemResult(experiment, result);
				return;

			case ExperimentKind.PairwiseCheck:
				ProcessPairwiseResult(experiment, result);
				return;

			default:
				throw new InvalidOperationException("The experiment is in an invalid state.");
		}
	}

	public BisectorStatus GetStatus()
	{
		var pairwiseTestsRemaining = pairwiseQueue.Count - pairwiseTestsCompleted;
		if (pairwiseTestsRemaining < 0)
			pairwiseTestsRemaining = 0;

		return new BisectorStatus(iteration, currentLevel, currentPhase, allItems.Count, currentSuspects.Count, confirmedBadItems.Count, confirmedBadPairs.Count, outstandingExperiment != null, pairwiseTestsCompleted, pairwiseTestsRemaining);
	}

	public void ObserveItem(TItem item)
	{
		if (item is null)
			throw new ArgumentNullException(nameof(item));

		if (ContainsItem(allItems, item))
			return;

		allItems.Add(item);
		unresolvedItems.Add(item);

		if (currentPhase == BisectorPhase.Complete)
			return;

		if (!ContainsItem(currentSuspects, item))
			currentSuspects.Add(item);

		switch (currentPhase)
		{
			case BisectorPhase.Reduction:
				if (!ContainsItem(reductionCandidates, item))
					reductionCandidates.Add(item);
				break;

			case BisectorPhase.SingleItemChecks:
				singleItemQueue.Enqueue(item);
				break;

			case BisectorPhase.PairwiseChecks:
				for (var i = 0; i < currentSuspects.Count - 1; i++)
				{
					pairwiseQueue.Add(new Pair(currentSuspects[i], item));
				}
				break;
		}
	}

	private void ProcessBaselineResult(BisectorResult result)
	{
		if (result == BisectorResult.Pass)
		{
			currentSuspects.Clear();
			currentPhase = BisectorPhase.Complete;
			return;
		}

		StartReductionCycle();
	}

	private void ProcessReductionResult(OutstandingExperiment experiment, BisectorResult result)
	{
		if (result == BisectorResult.Fail)
		{
			reductionCandidates = new List<TItem>(experiment.Subset);
		}
		else
		{
			var subset = new HashSet<TItem>(experiment.Subset, comparer);
			var remaining = new List<TItem>();

			foreach (var item in reductionCandidates)
			{
				if (!subset.Contains(item))
					remaining.Add(item);
			}

			reductionCandidates = remaining;
		}

		currentSuspects = new List<TItem>(reductionCandidates);

		if (reductionCandidates.Count <= SingleItemCheckThreshold)
		{
			BeginSingleItemChecks(reductionCandidates);
			return;
		}

		currentPhase = BisectorPhase.Reduction;
	}

	private void ProcessSingleItemResult(OutstandingExperiment experiment, BisectorResult result)
	{
		if (result == BisectorResult.Fail)
		{
			if (confirmedBadItems.Add(experiment.Item))
			{
				unresolvedItems.Remove(experiment.Item);
				foundBadItemInSingleItemChecks = true;
			}
		}

		if (singleItemQueue.Count == 0)
		{
			FinishSingleItemChecks();
		}
	}

	private void ProcessPairwiseResult(OutstandingExperiment experiment, BisectorResult result)
	{
		pairwiseTestsCompleted++;

		if (result == BisectorResult.Fail)
		{
			confirmedBadPairs.Add(experiment.Pair);
		}

		if (pairwiseTestsCompleted >= pairwiseQueue.Count)
		{
			currentPhase = BisectorPhase.Complete;
		}
	}

	private void StartReductionCycle()
	{
		currentLevel = BisectorLevel.Level1SingleItemSet;
		reductionCandidates = GetOrderedActiveItems();
		currentSuspects = new List<TItem>(reductionCandidates);

		if (reductionCandidates.Count == 0)
		{
			currentSuspects.Clear();
			currentPhase = BisectorPhase.Complete;
			return;
		}

		if (reductionCandidates.Count <= SingleItemCheckThreshold)
		{
			BeginSingleItemChecks(reductionCandidates);
			return;
		}

		currentPhase = BisectorPhase.Reduction;
	}

	private void BeginSingleItemChecks(IEnumerable<TItem> prioritizedItems)
	{
		currentLevel = BisectorLevel.Level1SingleItemSet;
		currentPhase = BisectorPhase.SingleItemChecks;
		foundBadItemInSingleItemChecks = false;
		singleItemQueue = new Queue<TItem>();
		currentSuspects = new List<TItem>();

		var queued = new HashSet<TItem>(comparer);

		foreach (var item in prioritizedItems)
		{
			if (!unresolvedItems.Contains(item))
				continue;

			if (queued.Add(item))
			{
				currentSuspects.Add(item);
				singleItemQueue.Enqueue(item);
			}
		}
	}

	private void FinishSingleItemChecks()
	{
		if (foundBadItemInSingleItemChecks)
		{
			currentSuspects = GetOrderedActiveItems();
			currentPhase = BisectorPhase.Baseline;
			return;
		}

		if (currentSuspects.Count < 2)
		{
			currentPhase = BisectorPhase.Complete;
			return;
		}

		BeginPairwiseChecks();
	}

	private void BeginPairwiseChecks()
	{
		currentLevel = BisectorLevel.Level2Pairwise;
		currentPhase = BisectorPhase.PairwiseChecks;
		currentSuspects = currentSuspects.Where(unresolvedItems.Contains).ToList();
		RebuildPairwiseQueueFromCurrentSuspects();

		if (pairwiseQueue.Count == 0)
			currentPhase = BisectorPhase.Complete;
	}

	private void RebuildPairwiseQueueFromCurrentSuspects()
	{
		pairwiseQueue = new List<Pair>();
		pairwiseIndex = 0;
		pairwiseTestsCompleted = 0;

		for (var i = 0; i < currentSuspects.Count; i++)
		{
			for (var j = i + 1; j < currentSuspects.Count; j++)
			{
				pairwiseQueue.Add(new Pair(currentSuspects[i], currentSuspects[j]));
			}
		}
	}

	private HashSet<TItem> CreateAndTrackExperiment(ExperimentKind kind, HashSet<TItem> disabledItems, List<TItem> subset, TItem item, Pair pair)
	{
		outstandingExperiment = new OutstandingExperiment(kind, disabledItems, subset, item, pair);
		return new HashSet<TItem>(disabledItems, comparer);
	}

	private HashSet<TItem> CreateDisabledSet()
	{
		return new HashSet<TItem>(confirmedBadItems, comparer);
	}

	private HashSet<TItem> CreateReductionDisabledSet(IEnumerable<TItem> enabledSubset)
	{
		var disabledItems = CreateDisabledSet();
		var enabled = new HashSet<TItem>(enabledSubset, comparer);

		foreach (var item in reductionCandidates)
		{
			if (!enabled.Contains(item))
				disabledItems.Add(item);
		}

		return disabledItems;
	}

	private HashSet<TItem> CreateSingleItemDisabledSet(TItem itemToKeepEnabled)
	{
		var disabledItems = CreateDisabledSet();

		foreach (var item in allItems)
		{
			if (!unresolvedItems.Contains(item))
				continue;

			if (comparer.Equals(item, itemToKeepEnabled))
				continue;

			disabledItems.Add(item);
		}

		return disabledItems;
	}

	private HashSet<TItem> CreatePairwiseDisabledSet(Pair pair)
	{
		var disabledItems = CreateDisabledSet();

		foreach (var item in allItems)
		{
			if (!unresolvedItems.Contains(item))
				continue;

			if (comparer.Equals(item, pair.Item1) || comparer.Equals(item, pair.Item2))
				continue;

			disabledItems.Add(item);
		}

		return disabledItems;
	}

	private List<TItem> GetOrderedActiveItems()
	{
		var orderedItems = new List<TItem>(unresolvedItems.Count);

		foreach (var item in allItems)
		{
			if (unresolvedItems.Contains(item))
				orderedItems.Add(item);
		}

		return orderedItems;
	}

	private bool ContainsItem(IEnumerable<TItem> items, TItem item)
	{
		foreach (var existing in items)
		{
			if (comparer.Equals(existing, item))
				return true;
		}

		return false;
	}
}
