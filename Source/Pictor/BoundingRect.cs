/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Pictor.VertexSource;

namespace Pictor
{
	static public class BoundingRect
	{
		//-----------------------------------------------------------BoundingRect
		//template<class VertexSource, class GetId, class CoordT>
		public static bool GetBoundingRect(PathStorage vs, uint[] gi,
						   uint start, uint num,
						   out double x1, out double y1, out double x2, out double y2)
		{
			uint i;
			double x = 0;
			double y = 0;
			bool first = true;

			x1 = (double)(1);
			y1 = (double)(1);
			x2 = (double)(0);
			y2 = (double)(0);

			for (i = 0; i < num; i++)
			{
				vs.Rewind(gi[start + i]);
				uint PathAndFlags;
				while (!Path.IsStop(PathAndFlags = vs.Vertex(out x, out y)))
				{
					if (Path.IsVertex(PathAndFlags))
					{
						if (first)
						{
							x1 = (double)(x);
							y1 = (double)(y);
							x2 = (double)(x);
							y2 = (double)(y);
							first = false;
						}
						else
						{
							if ((double)(x) < x1) x1 = (double)(x);
							if ((double)(y) < y1) y1 = (double)(y);
							if ((double)(x) > x2) x2 = (double)(x);
							if ((double)(y) > y2) y2 = (double)(y);
						}
					}
				}
			}
			return x1 <= x2 && y1 <= y2;
		}

		public static bool BoundingRectSingle(IVertexSource vs, uint path_id, ref RectD rect)
		{
			double x1, y1, x2, y2;
			bool rValue = BoundingRectSingle(vs, path_id, out x1, out y1, out x2, out y2);
			rect.x1 = x1;
			rect.y1 = y1;
			rect.x2 = x2;
			rect.y2 = y2;
			return rValue;
		}

		//-----------------------------------------------------BoundingRectSingle
		//template<class VertexSource, class CoordT>
		public static bool BoundingRectSingle(IVertexSource vs, uint path_id,
								  out double x1, out double y1, out double x2, out double y2)
		{
			double x = 0;
			double y = 0;
			bool first = true;

			x1 = (double)(1);
			y1 = (double)(1);
			x2 = (double)(0);
			y2 = (double)(0);

			vs.Rewind(path_id);
			uint PathAndFlags;
			while (!Path.IsStop(PathAndFlags = vs.Vertex(out x, out y)))
			{
				if (Path.IsVertex(PathAndFlags))
				{
					if (first)
					{
						x1 = (double)(x);
						y1 = (double)(y);
						x2 = (double)(x);
						y2 = (double)(y);
						first = false;
					}
					else
					{
						if ((double)(x) < x1) x1 = (double)(x);
						if ((double)(y) < y1) y1 = (double)(y);
						if ((double)(x) > x2) x2 = (double)(x);
						if ((double)(y) > y2) y2 = (double)(y);
					}
				}
			}
			return x1 <= x2 && y1 <= y2;
		}
	};
}

//#endif