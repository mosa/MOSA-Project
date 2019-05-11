// Copyright (c) MOSA Project. Licensed under the New BSD License.

// Quote from 0xd4d, https://github.com/0xd4d/dnlib/issues/230:
// "I don't plan on making that type public ever again. If you need to use it,
// you can just copy the source code to your project, it's not a lot of code."

using System;

namespace dnlib.DotNet
{
	/// <summary>
	/// Recursion counter
	/// </summary>
	internal struct RecursionCounter
	{
		/// <summary>
		/// Max recursion count. If this is reached, we won't continue, and will use a default value.
		/// </summary>
		public const int MAX_RECURSION_COUNT = 100;

		private int counter;

		/// <summary>
		/// Gets the recursion counter
		/// </summary>
		public int Counter => counter;

		/// <summary>
		/// Increments <see cref="counter"/> if it's not too high. <c>ALL</c> instance methods
		/// that can be called recursively must call this method and <see cref="Decrement"/>
		/// (if this method returns <c>true</c>)
		/// </summary>
		/// <returns><c>true</c> if it was incremented and caller can continue, <c>false</c> if
		/// it was <c>not</c> incremented and the caller must return to its caller.</returns>
		public bool Increment()
		{
			if (counter >= MAX_RECURSION_COUNT)
				return false;
			counter++;
			return true;
		}

		/// <summary>
		/// Must be called before returning to caller if <see cref="Increment"/>
		/// returned <c>true</c>.
		/// </summary>
		public void Decrement()
		{
#if DEBUG
			if (counter <= 0)
				throw new InvalidOperationException("recursionCounter <= 0");
#endif
			counter--;
		}

		/// <inheritdoc/>
		public override string ToString() => counter.ToString();
	}
}
