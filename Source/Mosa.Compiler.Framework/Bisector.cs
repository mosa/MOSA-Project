// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework;

public sealed class Bisector<TRule>
{
    private const int SingleRuleCheckThreshold = 4;

    public enum BisectorLevel
    {
        Level1SingleRuleSet,
        Level2Pairwise,
    }

    public enum BisectorPhase
    {
        Baseline,
        Reduction,
        SingleRuleChecks,
        PairwiseChecks,
        Complete,
    }

    public enum BisectorResult
    {
        Pass,
        Fail,
    }

    public readonly record struct RulePair(TRule Rule1, TRule Rule2);

    public sealed class BisectorStatus
    {
        internal BisectorStatus(int iteration, BisectorLevel level, BisectorPhase phase, int totalRuleCount, int suspectRuleCount, int confirmedBadRuleCount, int confirmedBadPairCount, bool hasOutstandingExperiment, int pairwiseTestsCompleted, int pairwiseTestsRemaining)
        {
            Iteration = iteration;
            Level = level;
            Phase = phase;
            TotalRuleCount = totalRuleCount;
            SuspectRuleCount = suspectRuleCount;
            ConfirmedBadRuleCount = confirmedBadRuleCount;
            ConfirmedBadPairCount = confirmedBadPairCount;
            HasOutstandingExperiment = hasOutstandingExperiment;
            PairwiseTestsCompleted = pairwiseTestsCompleted;
            PairwiseTestsRemaining = pairwiseTestsRemaining;
        }

        public int Iteration { get; }
        public BisectorLevel Level { get; }
        public BisectorPhase Phase { get; }
        public int TotalRuleCount { get; }
        public int SuspectRuleCount { get; }
        public int ConfirmedBadRuleCount { get; }
        public int ConfirmedBadPairCount { get; }
        public bool HasOutstandingExperiment { get; }
        public int PairwiseTestsCompleted { get; }
        public int PairwiseTestsRemaining { get; }
    }

    private enum ExperimentKind
    {
        Baseline,
        Reduction,
        SingleRuleCheck,
        PairwiseCheck,
    }

    private sealed class OutstandingExperiment
    {
        public OutstandingExperiment(ExperimentKind kind, HashSet<TRule> disabledRules, List<TRule> subset, TRule rule, RulePair pair)
        {
            Kind = kind;
            DisabledRules = disabledRules;
            Subset = subset;
            Rule = rule;
            Pair = pair;
        }

        public ExperimentKind Kind { get; }
        public HashSet<TRule> DisabledRules { get; }
        public List<TRule> Subset { get; }
        public TRule Rule { get; }
        public RulePair Pair { get; }
    }

    private readonly IEqualityComparer<TRule> comparer;
    private readonly List<TRule> allRules = new List<TRule>();
    private readonly HashSet<TRule> confirmedBadRules;
    private readonly HashSet<TRule> unresolvedRules;
    private readonly List<RulePair> confirmedBadPairs = new List<RulePair>();

    private BisectorLevel currentLevel = BisectorLevel.Level1SingleRuleSet;
    private BisectorPhase currentPhase = BisectorPhase.Baseline;
    private OutstandingExperiment outstandingExperiment;
    private List<TRule> currentSuspects = new List<TRule>();
    private List<TRule> reductionCandidates = new List<TRule>();
    private Queue<TRule> singleRuleQueue = new Queue<TRule>();
    private List<RulePair> pairwiseQueue = new List<RulePair>();
    private int pairwiseIndex;
    private int pairwiseTestsCompleted;
    private bool foundBadRuleInSingleRuleChecks;
    private int iteration;

    public Bisector(IEnumerable<TRule> rules, IEqualityComparer<TRule> comparer = null)
    {
        ArgumentNullException.ThrowIfNull(rules);

        this.comparer = comparer ?? EqualityComparer<TRule>.Default;
        confirmedBadRules = new HashSet<TRule>(this.comparer);
        unresolvedRules = new HashSet<TRule>(this.comparer);

        foreach (var rule in rules)
        {
            if (rule is null)
                throw new ArgumentException("Rules cannot contain null entries.", nameof(rules));

            if (!unresolvedRules.Add(rule))
                continue;

            allRules.Add(rule);
        }

        if (allRules.Count == 0)
            throw new ArgumentException("At least one rule is required.", nameof(rules));

        currentSuspects = new List<TRule>(allRules);
    }

    public bool IsComplete => currentPhase == BisectorPhase.Complete;

    public IReadOnlySet<TRule> ConfirmedBadRules => new HashSet<TRule>(confirmedBadRules, comparer);

    public IReadOnlyCollection<RulePair> ConfirmedBadPairs => confirmedBadPairs.AsReadOnly();

    public IReadOnlySet<TRule> RemainingSuspectRules => new HashSet<TRule>(currentSuspects, comparer);

    public IReadOnlySet<TRule> GetNextDisabledRules()
    {
        if (outstandingExperiment != null)
            throw new InvalidOperationException("The current experiment result must be reported before requesting the next experiment.");

        if (IsComplete)
            throw new InvalidOperationException("The isolation session is complete.");

        while (true)
        {
            switch (currentPhase)
            {
                case BisectorPhase.Baseline:
                    return CreateAndTrackExperiment(ExperimentKind.Baseline, CreateDisabledSet(), null, default, default);

                case BisectorPhase.Reduction:
                    if (reductionCandidates.Count <= SingleRuleCheckThreshold)
                    {
                        BeginSingleRuleChecks(reductionCandidates);
                        continue;
                    }

                    var midpoint = reductionCandidates.Count / 2;
                    if (midpoint == 0)
                        midpoint = 1;

                    var enabledSubset = reductionCandidates.GetRange(0, midpoint);
                    return CreateAndTrackExperiment(ExperimentKind.Reduction, CreateReductionDisabledSet(enabledSubset), enabledSubset, default, default);

                case BisectorPhase.SingleRuleChecks:
                    while (singleRuleQueue.Count > 0)
                    {
                        var rule = singleRuleQueue.Dequeue();
                        if (!unresolvedRules.Contains(rule))
                            continue;

                        return CreateAndTrackExperiment(ExperimentKind.SingleRuleCheck, CreateSingleRuleDisabledSet(rule), null, rule, default);
                    }

                    FinishSingleRuleChecks();
                    continue;

                case BisectorPhase.PairwiseChecks:
                    while (pairwiseIndex < pairwiseQueue.Count)
                    {
                        var pair = pairwiseQueue[pairwiseIndex++];
                        if (!unresolvedRules.Contains(pair.Rule1) || !unresolvedRules.Contains(pair.Rule2))
                            continue;

                        return CreateAndTrackExperiment(ExperimentKind.PairwiseCheck, CreatePairwiseDisabledSet(pair), null, default, pair);
                    }

                    currentPhase = BisectorPhase.Complete;
                    continue;

                case BisectorPhase.Complete:
                    throw new InvalidOperationException("The isolation session is complete.");

                default:
                    throw new InvalidOperationException("The isolation session is in an invalid state.");
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

            case ExperimentKind.SingleRuleCheck:
                ProcessSingleRuleResult(experiment, result);
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

        return new BisectorStatus(iteration, currentLevel, currentPhase, allRules.Count, currentSuspects.Count, confirmedBadRules.Count, confirmedBadPairs.Count, outstandingExperiment != null, pairwiseTestsCompleted, pairwiseTestsRemaining);
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
            reductionCandidates = new List<TRule>(experiment.Subset);
        }
        else
        {
            var subset = new HashSet<TRule>(experiment.Subset, comparer);
            var remaining = new List<TRule>();

            foreach (var rule in reductionCandidates)
            {
                if (!subset.Contains(rule))
                    remaining.Add(rule);
            }

            reductionCandidates = remaining;
        }

        currentSuspects = new List<TRule>(reductionCandidates);

        if (reductionCandidates.Count <= SingleRuleCheckThreshold)
        {
            BeginSingleRuleChecks(reductionCandidates);
            return;
        }

        currentPhase = BisectorPhase.Reduction;
    }

    private void ProcessSingleRuleResult(OutstandingExperiment experiment, BisectorResult result)
    {
        if (result == BisectorResult.Fail)
        {
            if (confirmedBadRules.Add(experiment.Rule))
            {
                unresolvedRules.Remove(experiment.Rule);
                foundBadRuleInSingleRuleChecks = true;
            }
        }

        if (singleRuleQueue.Count == 0)
        {
            FinishSingleRuleChecks();
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
        currentLevel = BisectorLevel.Level1SingleRuleSet;
        reductionCandidates = GetOrderedActiveRules();
        currentSuspects = new List<TRule>(reductionCandidates);

        if (reductionCandidates.Count == 0)
        {
            currentSuspects.Clear();
            currentPhase = BisectorPhase.Complete;
            return;
        }

        if (reductionCandidates.Count <= SingleRuleCheckThreshold)
        {
            BeginSingleRuleChecks(reductionCandidates);
            return;
        }

        currentPhase = BisectorPhase.Reduction;
    }

    private void BeginSingleRuleChecks(IEnumerable<TRule> prioritizedRules)
    {
        currentLevel = BisectorLevel.Level1SingleRuleSet;
        currentPhase = BisectorPhase.SingleRuleChecks;
        foundBadRuleInSingleRuleChecks = false;
        singleRuleQueue = new Queue<TRule>();
        currentSuspects = new List<TRule>();

        var queued = new HashSet<TRule>(comparer);

        foreach (var rule in prioritizedRules)
        {
            if (!unresolvedRules.Contains(rule))
                continue;

            if (queued.Add(rule))
            {
                currentSuspects.Add(rule);
                singleRuleQueue.Enqueue(rule);
            }
        }
    }

    private void FinishSingleRuleChecks()
    {
        if (foundBadRuleInSingleRuleChecks)
        {
            currentSuspects = GetOrderedActiveRules();
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
        pairwiseQueue = new List<RulePair>();
        pairwiseIndex = 0;
        pairwiseTestsCompleted = 0;
        currentSuspects = currentSuspects.Where(unresolvedRules.Contains).ToList();

        for (var i = 0; i < currentSuspects.Count; i++)
        {
            for (var j = i + 1; j < currentSuspects.Count; j++)
            {
                pairwiseQueue.Add(new RulePair(currentSuspects[i], currentSuspects[j]));
            }
        }

        if (pairwiseQueue.Count == 0)
            currentPhase = BisectorPhase.Complete;
    }

    private HashSet<TRule> CreateAndTrackExperiment(ExperimentKind kind, HashSet<TRule> disabledRules, List<TRule> subset, TRule rule, RulePair pair)
    {
        outstandingExperiment = new OutstandingExperiment(kind, disabledRules, subset, rule, pair);
        return new HashSet<TRule>(disabledRules, comparer);
    }

    private HashSet<TRule> CreateDisabledSet()
    {
        return new HashSet<TRule>(confirmedBadRules, comparer);
    }

    private HashSet<TRule> CreateDisabledSet(IEnumerable<TRule> additionalRules)
    {
        var disabledRules = CreateDisabledSet();

        foreach (var rule in additionalRules)
            disabledRules.Add(rule);

        return disabledRules;
    }

    private HashSet<TRule> CreateReductionDisabledSet(IEnumerable<TRule> enabledSubset)
    {
        var disabledRules = CreateDisabledSet();
        var enabled = new HashSet<TRule>(enabledSubset, comparer);

        foreach (var rule in reductionCandidates)
        {
            if (!enabled.Contains(rule))
                disabledRules.Add(rule);
        }

        return disabledRules;
    }

    private HashSet<TRule> CreateSingleRuleDisabledSet(TRule ruleToKeepEnabled)
    {
        var disabledRules = CreateDisabledSet();

        foreach (var rule in allRules)
        {
            if (!unresolvedRules.Contains(rule))
                continue;

            if (comparer.Equals(rule, ruleToKeepEnabled))
                continue;

            disabledRules.Add(rule);
        }

        return disabledRules;
    }

    private HashSet<TRule> CreatePairwiseDisabledSet(RulePair pair)
    {
        var disabledRules = CreateDisabledSet();

        foreach (var rule in allRules)
        {
            if (!unresolvedRules.Contains(rule))
                continue;

            if (comparer.Equals(rule, pair.Rule1) || comparer.Equals(rule, pair.Rule2))
                continue;

            disabledRules.Add(rule);
        }

        return disabledRules;
    }

    private List<TRule> GetOrderedActiveRules()
    {
        var orderedRules = new List<TRule>(unresolvedRules.Count);

        foreach (var rule in allRules)
        {
            if (unresolvedRules.Contains(rule))
                orderedRules.Add(rule);
        }

        return orderedRules;
    }
}
