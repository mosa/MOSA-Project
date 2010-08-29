/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

namespace Pictor
{
	public interface IMarkers
	{
		void RemoveAll();
		void AddVertex(double x, double y, uint unknown);
	};
}
