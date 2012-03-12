/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// 
	/// </summary>
	public enum DeviceDriverStartStatus
	{
		/// <summary>
		/// Started
		/// </summary>
		Started,
		/// <summary>
		/// Not Found
		/// </summary>
		NotFound,
		/// <summary>
		/// Resource Conflict
		/// </summary>
		ResourceConflict,
		/// <summary>
		/// Failed
		/// </summary>
		Failed
	}

}
