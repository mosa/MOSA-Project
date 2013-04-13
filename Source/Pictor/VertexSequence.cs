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
	//----------------------------------------------------------vertex_sequence
	// Modified Pictor::pod_vector. The Data is interpreted as a sequence
	// of vertices. It means that the type T must expose:
	//
	// bool T::operator() (const T& val)
	//
	// that is called every time a new Vertex is being added. The main purpose
	// of this operator is the possibility to Calculate some values during
	// adding and to return true if the Vertex fits some criteria or false if
	// it doesn't. In the last case the new Vertex is not added.
	//
	// The simple example is filtering coinciding vertices with calculation
	// of the distance between the current and previous ones:
	//
	//    struct VertexDistance
	//    {
	//        double   x;
	//        double   y;
	//        double   dist;
	//
	//        VertexDistance() {}
	//        VertexDistance(double x_, double y_) :
	//            x(x_),
	//            y(y_),
	//            dist(0.0)
	//        {
	//        }
	//
	//        bool operator () (const VertexDistance& val)
	//        {
	//            return (dist = CalculateDistance(x, y, val.x, val.y)) > EPSILON;
	//        }
	//    };
	//
	// Function close() calls this operator and removes the last Vertex if
	// necessary.
	//------------------------------------------------------------------------
	//template<class T, unsigned S=6>
	public class vertex_sequence : VectorPOD<VertexDistance>

	//public class vertex_sequence<TypeOfVertex> : pod_vector<TypeOfVertex> where TypeOfVertex :
	{
		//typedef pod_vector<T, S> base_type;

		public override void Add(VertexDistance val)
		{
			if (base.Size() > 1)
			{
				if (!Array[base.Size() - 2].IsEqual(Array[base.Size() - 1]))
				{
					base.RemoveLast();
				}
			}
			base.Add(val);
		}

		public void modify_last(VertexDistance val)
		{
			base.RemoveLast();
			Add(val);
		}

		public void close(bool closed)
		{
			while (base.Size() > 1)
			{
				if (Array[base.Size() - 2].IsEqual(Array[base.Size() - 1])) break;
				VertexDistance t = this[base.Size() - 1];
				base.RemoveLast();
				modify_last(t);
			}

			if (closed)
			{
				while (base.Size() > 1)
				{
					if (Array[base.Size() - 1].IsEqual(Array[0])) break;
					base.RemoveLast();
				}
			}
		}

		internal VertexDistance prev(uint idx)
		{
			return this[(idx + m_size - 1) % m_size];
		}

		internal VertexDistance curr(uint idx)
		{
			return this[idx];
		}

		internal VertexDistance next(uint idx)
		{
			return this[(idx + 1) % m_size];
		}
	};

	//-------------------------------------------------------------VertexDistance
	// Vertex (x, y) with the distance to the next one. The last Vertex has
	// distance between the last and the first points if the polygon is closed
	// and 0.0 if it's a polyline.
	public struct VertexDistance
	{
		public double x;
		public double y;
		public double dist;

		public VertexDistance(double x_, double y_)
		{
			x = x_;
			y = y_;
			dist = 0.0;
		}

		public bool IsEqual(VertexDistance val)
		{
			bool ret = (dist = PictorMath.CalculateDistance(x, y, val.x, val.y)) > PictorMath.vertex_dist_epsilon;
			if (!ret) dist = 1.0 / PictorMath.vertex_dist_epsilon;
			return ret;
		}
	};

	/*

	//--------------------------------------------------------vertex_dist_cmd
	// Save as the above but with additional "command" Value
	struct vertex_dist_cmd : VertexDistance
	{
		unsigned cmd;

		vertex_dist_cmd() {}
		vertex_dist_cmd(double x_, double y_, unsigned cmd_) :
			base (x_, y_)

		{
			cmd = cmd;
		}
	};
	 */
}

//#endif