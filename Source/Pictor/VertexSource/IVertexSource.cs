/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */
namespace Pictor.VertexSource
{
	///<summary>
	///</summary>
	public interface IVertexSource
	{
		///<summary>
		///</summary>
		///<param name="pathId"></param>
		void Rewind(uint pathId);
		///<summary>
		///</summary>
		///<param name="x"></param>
		///<param name="y"></param>
		///<returns></returns>
		uint Vertex(out double x, out double y);
	};
}
