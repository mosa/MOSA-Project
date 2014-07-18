/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

namespace System.Reflection
{
	/// <summary>
	/// Selects a member from a list of candidates, and performs type conversion from actual argument type to formal argument type.
	/// </summary>
	[Serializable]
	public struct ParameterModifier
	{
		private bool[] _byRef;

		/// <summary>
		/// Initializes a new instance of the ParameterModifier structure representing the specified number of parameters.
		/// </summary>
		/// <param name="parameterCount">The number of parameters.</param>
		public ParameterModifier(int parameterCount)
		{
			if (parameterCount < 0)
				throw new ArgumentException("parameterCount is negative.", "parameterCount");

			this._byRef = new bool[parameterCount];
		}

		/// <summary>
		/// Gets or sets a value that specifies whether the parameter at the specified index position is to be modified by the current ParameterModifier.
		/// </summary>
		/// <param name="index">The index position of the parameter whose modification status is being examined or set.</param>
		/// <returns>true if the parameter at this index position is to be modified by this ParameterModifier; otherwise, false.</returns>
		public bool this[int index]
		{
			get { return _byRef[index]; }
			set { _byRef[index] = value; }
		}
	}
}