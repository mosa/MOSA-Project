// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework;

public sealed class IsolationSession<TRule>
{
    private const int SingleRuleCheckThreshold = 4;

    public enum RuleIsolationLevel
    {
        Level1SingleRuleSet,
        Level2Pairwise,
    }

    public enum RuleIsolationPhase
    {
        Baseline,
        Reduction,
        SingleRuleChecks,
        PairwiseChecks,
        Complete,
    }

    public enum RuleIsolationResult
    {
        Pass,
        Fail,
    }

    public readonly record struct RulePair(TRule Rule1, TRule Rule2);

    public sealed class RuleIsolationStatus
    {
        internal RuleIsolationStatus(int iteration, RuleIsolationLevel level, RuleIsolationPhase phase, int totalRuleCount, int suspectRuleCount, int confirmedBadRuleCount, int confirmedBadPairCount, bool hasOutstandingExperiment, int pairwiseTestsCompleted, int pairwiseTestsRemaining)
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
        public RuleIsolationLevel Level { get; }
        public RuleIsolationPhase Phase { get; }
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

    private RuleIsolationLevel currentLevel = RuleIsolationLevel.Level1SingleRuleSet;
    private RuleIsolationPhase currentPhase = RuleIsolationPhase.Baseline;
    private OutstandingExperiment outstandingExperiment;
    private List<TRule> currentSuspects = new List<TRule>();
    private List<TRule> reductionCandidates = new List<TRule>();
    private Queue<TRule> singleRuleQueue = new Queue<TRule>();
    private List<RulePair> pairwiseQueue = new List<RulePair>();
    private int pairwiseIndex;
    private int pairwiseTestsCompleted;
    private bool foundBadRuleInSingleRuleChecks;
    private int iteration;

    public IsolationSession(IEnumerable<TRule> rules, IEqualityComparer<TRule> comparer = null)
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

    public bool IsComplete => currentPhase == RuleIsolationPhase.Complete;

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
                case RuleIsolationPhase.Baseline:
                    return CreateAndTrackExperiment(ExperimentKind.Baseline, CreateDisabledSet(), null, default, default);

                case RuleIsolationPhase.Reduction:
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

                case RuleIsolationPhase.SingleRuleChecks:
                    while (singleRuleQueue.Count > 0)
                    {
                        var rule = singleRuleQueue.Dequeue();
                        if (!unresolvedRules.Contains(rule))
                            continue;

                        return CreateAndTrackExperiment(ExperimentKind.SingleRuleCheck, CreateSingleRuleDisabledSet(rule), null, rule, default);
                    }

                    FinishSingleRuleChecks();
                    continue;

                case RuleIsolationPhase.PairwiseChecks:
                    while (pairwiseIndex < pairwiseQueue.Count)
                    {
                        var pair = pairwiseQueue[pairwiseIndex++];
                        if (!unresolvedRules.Contains(pair.Rule1) || !unresolvedRules.Contains(pair.Rule2))
                            continue;

                        return CreateAndTrackExperiment(ExperimentKind.PairwiseCheck, CreatePairwiseDisabledSet(pair), null, default, pair);
                    }

                    currentPhase = RuleIsolationPhase.Complete;
                    continue;

                case RuleIsolationPhase.Complete:
                    throw new InvalidOperationException("The isolation session is complete.");

                default:
                    throw new InvalidOperationException("The isolation session is in an invalid state.");
            }
        }
    }

    public void AcceptResult(bool passed)
    {
        AcceptResult(passed ? RuleIsolationResult.Pass : RuleIsolationResult.Fail);
    }

    public void AcceptResult(RuleIsolationResult result)
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

    public RuleIsolationStatus GetStatus()
    {
        var pairwiseTestsRemaining = pairwiseQueue.Count - pairwiseTestsCompleted;
        if (pairwiseTestsRemaining < 0)
            pairwiseTestsRemaining = 0;

        return new RuleIsolationStatus(iteration, currentLevel, currentPhase, allRules.Count, currentSuspects.Count, confirmedBadRules.Count, confirmedBadPairs.Count, outstandingExperiment != null, pairwiseTestsCompleted, pairwiseTestsRemaining);
    }

    private void ProcessBaselineResult(RuleIsolationResult result)
    {
        if (result == RuleIsolationResult.Pass)
        {
            currentSuspects.Clear();
            currentPhase = RuleIsolationPhase.Complete;
            return;
        }

        StartReductionCycle();
    }

    private void ProcessReductionResult(OutstandingExperiment experiment, RuleIsolationResult result)
    {
        if (result == RuleIsolationResult.Fail)
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

        currentPhase = RuleIsolationPhase.Reduction;
    }

    private void ProcessSingleRuleResult(OutstandingExperiment experiment, RuleIsolationResult result)
    {
        if (result == RuleIsolationResult.Fail)
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

    private void ProcessPairwiseResult(OutstandingExperiment experiment, RuleIsolationResult result)
    {
        pairwiseTestsCompleted++;

        if (result == RuleIsolationResult.Fail)
        {
            confirmedBadPairs.Add(experiment.Pair);
        }

        if (pairwiseTestsCompleted >= pairwiseQueue.Count)
        {
            currentPhase = RuleIsolationPhase.Complete;
        }
    }

    private void StartReductionCycle()
    {
        currentLevel = RuleIsolationLevel.Level1SingleRuleSet;
        reductionCandidates = GetOrderedActiveRules();
        currentSuspects = new List<TRule>(reductionCandidates);

        if (reductionCandidates.Count == 0)
        {
            currentSuspects.Clear();
            currentPhase = RuleIsolationPhase.Complete;
            return;
        }

        if (reductionCandidates.Count <= SingleRuleCheckThreshold)
        {
            BeginSingleRuleChecks(reductionCandidates);
            return;
        }

        currentPhase = RuleIsolationPhase.Reduction;
    }

    private void BeginSingleRuleChecks(IEnumerable<TRule> prioritizedRules)
    {
        currentLevel = RuleIsolationLevel.Level1SingleRuleSet;
        currentPhase = RuleIsolationPhase.SingleRuleChecks;
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
            currentPhase = RuleIsolationPhase.Baseline;
            return;
        }

        if (currentSuspects.Count < 2)
        {
            currentPhase = RuleIsolationPhase.Complete;
            return;
        }

        BeginPairwiseChecks();
    }

    private void BeginPairwiseChecks()
    {
        currentLevel = RuleIsolationLevel.Level2Pairwise;
        currentPhase = RuleIsolationPhase.PairwiseChecks;
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
            currentPhase = RuleIsolationPhase.Complete;
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
