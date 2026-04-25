// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Compiler.Framework.xUnit;

public class BisectorTests
{
	[Fact]
	public void ConstructorRequiresAtLeastOneRule()
	{
		Assert.Throws<ArgumentException>(() => new Bisector<string>(Array.Empty<string>()));
	}

	[Fact]
	public void ConstructorRejectsNullRule()
	{
		Assert.Throws<ArgumentException>(() => new Bisector<string>(new[] { "A", null }));
	}

	[Fact]
	public void DuplicateRulesAreIgnored()
	{
		var session = new Bisector<string>(new[] { "A", "B", "A", "B", "C" });
		var status = session.GetStatus();

		Assert.Equal(3, status.TotalRuleCount);
		Assert.Equal(3, status.SuspectRuleCount);
	}

	[Fact]
	public void GetNextDisabledRulesRequiresPreviousResultToBeAccepted()
	{
		var session = new Bisector<string>(new[] { "A", "B" });

		session.GetNextDisabledRules();

		Assert.Throws<InvalidOperationException>(() => session.GetNextDisabledRules());
	}

	[Fact]
	public void AcceptResultRequiresOutstandingExperiment()
	{
		var session = new Bisector<string>(new[] { "A", "B" });

		Assert.Throws<InvalidOperationException>(() => session.AcceptResult(true));
	}

	[Fact]
	public void BaselinePassCompletesSession()
	{
		var session = new Bisector<string>(new[] { "A", "B", "C" });

		var disabledRules = session.GetNextDisabledRules();
		Assert.Empty(disabledRules);

		session.AcceptResult(true);

		var status = session.GetStatus();
		Assert.True(session.IsComplete);
		Assert.Equal(Bisector<string>.BisectorPhase.Complete, status.Phase);
		Assert.Equal(0, status.SuspectRuleCount);
		Assert.Empty(session.ConfirmedBadRules);
	}

	[Fact]
	public void GetNextDisabledRulesThrowsAfterCompletion()
	{
		var session = new Bisector<string>(new[] { "A", "B" });

		session.GetNextDisabledRules();
		session.AcceptResult(true);

		Assert.Throws<InvalidOperationException>(() => session.GetNextDisabledRules());
	}

	[Fact]
	public void SingleBadRuleIsConfirmed()
	{
		var rules = new HashSet<string> { "A", "B", "C", "D", "E", "F" };
		var session = new Bisector<string>(rules);

		RunUntilComplete(session, disabledRules => !IsFailingSingle(disabledRules, "C"));

		Assert.Equal(new HashSet<string> { "C" }, session.ConfirmedBadRules);
		Assert.Empty(session.ConfirmedBadPairs);
		Assert.Empty(session.RemainingSuspectRules);
	}

	[Fact]
	public void ThresholdSizedSuspectSetGoesStraightToSingleRuleChecks()
	{
		var rules = new HashSet<string> { "A", "B", "C", "D" };
		var session = new Bisector<string>(rules);

		session.GetNextDisabledRules();
		session.AcceptResult(false);

		var status = session.GetStatus();
		Assert.Equal(Bisector<string>.BisectorPhase.SingleRuleChecks, status.Phase);
		Assert.Equal(Bisector<string>.BisectorLevel.Level1SingleRuleSet, status.Level);

		RunUntilComplete(session, disabledRules => !IsFailingSingle(disabledRules, "D"));

		Assert.Equal(new HashSet<string> { "D" }, session.ConfirmedBadRules);
	}

	[Fact]
	public void MultipleBadRulesAreConfirmedAcrossCycles()
	{
		var rules = new HashSet<string> { "A", "B", "C", "D", "E", "F" };
		var session = new Bisector<string>(rules);

		RunUntilComplete(session, disabledRules =>
		{
			var enabledRules = GetEnabledRules(rules, disabledRules);
			return !(enabledRules.Contains("B") || enabledRules.Contains("E"));
		});

		Assert.Equal(new HashSet<string> { "B", "E" }, session.ConfirmedBadRules);
		Assert.Empty(session.ConfirmedBadPairs);
		Assert.Empty(session.RemainingSuspectRules);
	}

	[Fact]
	public void CustomComparerControlsRuleIdentity()
	{
		var rules = new[] { "alpha", "ALPHA", "beta", "gamma" };
		var session = new Bisector<string>(rules, StringComparer.OrdinalIgnoreCase);

		RunUntilComplete(session, disabledRules => disabledRules.Contains("ALPHA"));

		Assert.Single(session.ConfirmedBadRules);
		Assert.Contains("alpha", session.ConfirmedBadRules, StringComparer.OrdinalIgnoreCase);
		Assert.Empty(session.RemainingSuspectRules);
	}

	[Fact]
	public void Level2StartsAutomaticallyAfterSingleRuleChecks()
	{
		var rules = new HashSet<string> { "A", "B", "C", "D" };
		var session = new Bisector<string>(rules);

		var disabledRules = session.GetNextDisabledRules();
		Assert.Empty(disabledRules);
		Assert.Equal(Bisector<string>.BisectorPhase.Baseline, session.GetStatus().Phase);

		session.AcceptResult(false);

		for (var i = 0; i < rules.Count; i++)
		{
			disabledRules = session.GetNextDisabledRules();
			session.AcceptResult(true);
		}

		var status = session.GetStatus();
		Assert.Equal(Bisector<string>.BisectorLevel.Level2Pairwise, status.Level);
		Assert.Equal(Bisector<string>.BisectorPhase.PairwiseChecks, status.Phase);
		Assert.Equal(0, status.PairwiseTestsCompleted);
		Assert.Equal(6, status.PairwiseTestsRemaining);
	}

	[Fact]
	public void PairwiseInteractionIsDetected()
	{
		var rules = new HashSet<string> { "A", "B", "C", "D" };
		var session = new Bisector<string>(rules);

		RunUntilComplete(session, disabledRules =>
		{
			var enabledRules = GetEnabledRules(rules, disabledRules);
			return !(enabledRules.Contains("B") && enabledRules.Contains("D"));
		});

		Assert.Empty(session.ConfirmedBadRules);
		Assert.Contains(new Bisector<string>.RulePair("B", "D"), session.ConfirmedBadPairs);
		Assert.Equal(new HashSet<string>(rules), session.RemainingSuspectRules);
	}

	[Fact]
	public void PairwiseStatusTracksCompletedAndRemainingTests()
	{
		var rules = new HashSet<string> { "A", "B", "C", "D" };
		var session = new Bisector<string>(rules);

		session.GetNextDisabledRules();
		session.AcceptResult(false);

		for (var i = 0; i < rules.Count; i++)
		{
			session.GetNextDisabledRules();
			session.AcceptResult(true);
		}

		var disabledRules = session.GetNextDisabledRules();
		Assert.Equal(new HashSet<string> { "C", "D" }, disabledRules);

		session.AcceptResult(true);

		var status = session.GetStatus();
		Assert.Equal(1, status.PairwiseTestsCompleted);
		Assert.Equal(5, status.PairwiseTestsRemaining);
	}

	[Fact]
	public void StatusTracksIterationsAndOutstandingExperiments()
	{
		var rules = new HashSet<string> { "A", "B", "C", "D", "E", "F" };
		var session = new Bisector<string>(rules);

		var status = session.GetStatus();
		Assert.Equal(0, status.Iteration);
		Assert.False(status.HasOutstandingExperiment);
		Assert.Equal(rules.Count, status.TotalRuleCount);
		Assert.Equal(rules.Count, status.SuspectRuleCount);

		var disabledRules = session.GetNextDisabledRules();
		Assert.Empty(disabledRules);

		status = session.GetStatus();
		Assert.True(status.HasOutstandingExperiment);
		Assert.Equal(Bisector<string>.BisectorPhase.Baseline, status.Phase);

		session.AcceptResult(false);

		status = session.GetStatus();
		Assert.Equal(1, status.Iteration);
		Assert.False(status.HasOutstandingExperiment);
		Assert.Equal(Bisector<string>.BisectorPhase.Reduction, status.Phase);
	}

	private static void RunUntilComplete(Bisector<string> session, Func<IReadOnlySet<string>, bool> passEvaluator)
	{
		while (!session.IsComplete)
		{
			var disabledRules = session.GetNextDisabledRules();
			var passed = passEvaluator(disabledRules);
			session.AcceptResult(passed);
		}
	}

	private static bool IsFailingSingle(IReadOnlySet<string> disabledRules, string badRule)
	{
		return !disabledRules.Contains(badRule);
	}

	private static HashSet<string> GetEnabledRules(HashSet<string> allRules, IReadOnlySet<string> disabledRules)
	{
		var enabledRules = new HashSet<string>(allRules);
		enabledRules.ExceptWith(disabledRules);
		return enabledRules;
	}
}
