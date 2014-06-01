/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System.Runtime.CompilerServices;

namespace System.Threading
{
	public struct SpinLock
	{
		private bool bLock;

		public bool IsHeld
		{
			get { return bLock; }
		}
		
		public void Enter(ref bool lockTaken)
		{
			//while (!EnterLock(ref bLock)) ;

			lockTaken = true;
		}

		public void Exit()
		{
			//ExitLock(ref bLock);
		}
	}
}