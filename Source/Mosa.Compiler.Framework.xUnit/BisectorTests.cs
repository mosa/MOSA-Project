// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Compiler.Framework.xUnit;

public class BisectorTests
{
	[Fact]
	public void ConstructorRequiresAtLeastOneItem()
	{
		Assert.Throws<ArgumentException>(() => new Bisector<string>(Array.Empty<string>()));
	}

	[Fact]
	public void ConstructorRejectsNullItem()
	{
		Assert.Throws<ArgumentException>(() => new Bisector<string>(new[] { "A", null }));
	}

	[Fact]
	public void DuplicateItemsAreIgnored()
	{
		var session = new Bisector<string>(new[] { "A", "B", "A", "B", "C" });
		var status = session.GetStatus();

		Assert.Equal(3, status.TotalItemCount);
		Assert.Equal(3, status.SuspectItemCount);
	}

	[Fact]
	public void GetNextDisabledItemsRequiresPreviousResultToBeAccepted()
	{
		var session = new Bisector<string>(new[] { "A", "B" });

		session.GetNextDisabledItems();

		Assert.Throws<InvalidOperationException>(() => session.GetNextDisabledItems());
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

		var disabledItems = session.GetNextDisabledItems();
		Assert.Empty(disabledItems);

		session.AcceptResult(true);

		var status = session.GetStatus();
		Assert.True(session.IsComplete);
		Assert.Equal(Bisector<string>.BisectorPhase.Complete, status.Phase);
		Assert.Equal(0, status.SuspectItemCount);
		Assert.Empty(session.ConfirmedBadItems);
	}

	[Fact]
	public void GetNextDisabledItemsThrowsAfterCompletion()
	{
		var session = new Bisector<string>(new[] { "A", "B" });

		session.GetNextDisabledItems();
		session.AcceptResult(true);

		Assert.Throws<InvalidOperationException>(() => session.GetNextDisabledItems());
	}

	[Fact]
	public void SingleBadItemIsConfirmed()
	{
		var items = new HashSet<string> { "A", "B", "C", "D", "E", "F" };
		var session = new Bisector<string>(items);

		RunUntilComplete(session, disabledItems => !IsFailingSingle(disabledItems, "C"));

		Assert.Equal(new HashSet<string> { "C" }, session.ConfirmedBadItems);
		Assert.Empty(session.ConfirmedBadPairs);
		Assert.Empty(session.RemainingSuspectItems);
	}

	[Fact]
	public void ThresholdSizedSuspectSetGoesStraightToSingleItemChecks()
	{
		var items = new HashSet<string> { "A", "B", "C", "D" };
		var session = new Bisector<string>(items);

		session.GetNextDisabledItems();
		session.AcceptResult(false);

		var status = session.GetStatus();
		Assert.Equal(Bisector<string>.BisectorPhase.SingleItemChecks, status.Phase);
		Assert.Equal(Bisector<string>.BisectorLevel.Level1SingleItemSet, status.Level);

		RunUntilComplete(session, disabledItems => !IsFailingSingle(disabledItems, "D"));

		Assert.Equal(new HashSet<string> { "D" }, session.ConfirmedBadItems);
	}

	[Fact]
	public void MultipleBadItemsAreConfirmedAcrossCycles()
	{
		var items = new HashSet<string> { "A", "B", "C", "D", "E", "F" };
		var session = new Bisector<string>(items);

		RunUntilComplete(session, disabledItems =>
		{
			var enabledItems = GetEnabledItems(items, disabledItems);
			return !(enabledItems.Contains("B") || enabledItems.Contains("E"));
		});

		Assert.Equal(new HashSet<string> { "B", "E" }, session.ConfirmedBadItems);
		Assert.Empty(session.ConfirmedBadPairs);
		Assert.Empty(session.RemainingSuspectItems);
	}

	[Fact]
	public void CustomComparerControlsItemIdentity()
	{
		var items = new[] { "alpha", "ALPHA", "beta", "gamma" };
		var session = new Bisector<string>(items, StringComparer.OrdinalIgnoreCase);

		RunUntilComplete(session, disabledItems => disabledItems.Contains("ALPHA"));

		Assert.Single(session.ConfirmedBadItems);
		Assert.Contains("alpha", session.ConfirmedBadItems, StringComparer.OrdinalIgnoreCase);
		Assert.Empty(session.RemainingSuspectItems);
	}

	[Fact]
	public void Level2StartsAutomaticallyAfterSingleItemChecks()
	{
		var items = new HashSet<string> { "A", "B", "C", "D" };
		var session = new Bisector<string>(items);

		var disabledItems = session.GetNextDisabledItems();
		Assert.Empty(disabledItems);
		Assert.Equal(Bisector<string>.BisectorPhase.Baseline, session.GetStatus().Phase);

		session.AcceptResult(false);

		for (var i = 0; i < items.Count; i++)
		{
			disabledItems = session.GetNextDisabledItems();
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
		var items = new HashSet<string> { "A", "B", "C", "D" };
		var session = new Bisector<string>(items);

		RunUntilComplete(session, disabledItems =>
		{
			var enabledItems = GetEnabledItems(items, disabledItems);
			return !(enabledItems.Contains("B") && enabledItems.Contains("D"));
		});

		Assert.Empty(session.ConfirmedBadItems);
		Assert.Contains(new Bisector<string>.Pair("B", "D"), session.ConfirmedBadPairs);
		Assert.Equal(new HashSet<string>(items), session.RemainingSuspectItems);
	}

	[Fact]
	public void PairwiseStatusTracksCompletedAndRemainingTests()
	{
		var items = new HashSet<string> { "A", "B", "C", "D" };
		var session = new Bisector<string>(items);

		session.GetNextDisabledItems();
		session.AcceptResult(false);

		for (var i = 0; i < items.Count; i++)
		{
			session.GetNextDisabledItems();
			session.AcceptResult(true);
		}

		var disabledItems = session.GetNextDisabledItems();
		Assert.Equal(new HashSet<string> { "C", "D" }, disabledItems);

		session.AcceptResult(true);

		var status = session.GetStatus();
		Assert.Equal(1, status.PairwiseTestsCompleted);
		Assert.Equal(5, status.PairwiseTestsRemaining);
	}

	[Fact]
	public void StatusTracksIterationsAndOutstandingExperiments()
	{
		var items = new HashSet<string> { "A", "B", "C", "D", "E", "F" };
		var session = new Bisector<string>(items);

		var status = session.GetStatus();
		Assert.Equal(0, status.Iteration);
		Assert.False(status.HasOutstandingExperiment);
		Assert.Equal(items.Count, status.TotalItemCount);
		Assert.Equal(items.Count, status.SuspectItemCount);

		var disabledItems = session.GetNextDisabledItems();
		Assert.Empty(disabledItems);

		status = session.GetStatus();
		Assert.True(status.HasOutstandingExperiment);
		Assert.Equal(Bisector<string>.BisectorPhase.Baseline, status.Phase);

		session.AcceptResult(false);

		status = session.GetStatus();
		Assert.Equal(1, status.Iteration);
		Assert.False(status.HasOutstandingExperiment);
		Assert.Equal(Bisector<string>.BisectorPhase.Reduction, status.Phase);
	}

	[Fact]
	public void ObserveItemAddsNewCandidateDuringSingleItemChecks()
	{
		var session = new Bisector<string>(new[] { "A", "B", "C", "D" });

		session.GetNextDisabledItems();
		session.AcceptResult(false);

		session.ObserveItem("Z");

		var seenZ = false;
		while (!session.IsComplete)
		{
			var disabledItems = session.GetNextDisabledItems();
			if (!disabledItems.Contains("Z"))
				seenZ = true;

			session.AcceptResult(true);
		}

		Assert.True(seenZ);
		Assert.Contains("Z", session.RemainingSuspectItems);
	}

	private static void RunUntilComplete(Bisector<string> session, Func<IReadOnlySet<string>, bool> passEvaluator)
	{
		while (!session.IsComplete)
		{
			var disabledItems = session.GetNextDisabledItems();
			var passed = passEvaluator(disabledItems);
			session.AcceptResult(passed);
		}
	}

	private static bool IsFailingSingle(IReadOnlySet<string> disabledItems, string badItem)
	{
		return !disabledItems.Contains(badItem);
	}

	private static HashSet<string> GetEnabledItems(HashSet<string> allItems, IReadOnlySet<string> disabledItems)
	{
		var enabledItems = new HashSet<string>(allItems);
		enabledItems.ExceptWith(disabledItems);
		return enabledItems;
	}
}
