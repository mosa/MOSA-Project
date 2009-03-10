/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

namespace Pictor.VertexSource
{
    //---------------------------------------------------------------CurveConverter
    // Curve converter class. Any path storage can have Bezier curves defined 
    // by their control points. There're two types of curves supported: Curve3 
    // and Curve4. Curve3 is a conic Bezier curve with 2 endpoints and 1 control
    // point. Curve4 has 2 control points (4 points in total) and can be used
    // to interpolate more complicated curves. Curve4, unlike Curve3 can be used 
    // to approximate arcs, both circular and elliptical. Curves are approximated 
    // with straight lines and one of the approaches is just to store the whole 
    // sequence of vertices that approximate our curve. It takes additional 
    // memory, and At the same time the consecutive vertices can be calculated 
    // on demand. 
    //
    // Initially, path storages are not suppose to keep all the vertices of the
    // curves (although, nothing prevents us from doing so). Instead, path_storage
    // keeps only vertices, needed to Calculate a curve on demand. Those vertices
    // are marked with special commands. So, if the path_storage contains curves 
    // (which are not real curves yet), and we render this storage directly, 
    // all we will see is only 2 or 3 straight Line segments (for Curve3 and 
    // Curve4 respectively). If we need to see real curves drawn we need to 
    // include this class into the conversion pipeline. 
    //
    // Class CurveConverter recognizes commands Curve3 and Curve4 
    // and converts these vertices into a MoveTo/LineTo sequence. 
    //-----------------------------------------------------------------------
    public class CurveConverter : IVertexSource
    {
        IVertexSource m_source;
        double        m_last_x;
        double        m_last_y;
        Curve3 m_curve3;
        Curve4 m_curve4;

        public CurveConverter(IVertexSource source)
        {
            m_curve3 = new Curve3();
            m_curve4 = new Curve4();
            m_source=(source);
            m_last_x=(0.0);
            m_last_y=(0.0);
        }

        public void Attach(IVertexSource source) { m_source = source; }

        public Curves.ECurveApproximationMethod ApproximationMethod
        {
            get
            {
                return m_curve4.ApproximationMethod();
            }
            set
            {
                m_curve3.ApproximationMethod(value);
                m_curve4.ApproximationMethod(value);
            }
        }

        public double ApproximationScale
        {
            get { return m_curve4.ApproximationScale; }
            set
            {
                m_curve3.ApproximationScale = value;
                m_curve4.ApproximationScale = value;
            }
        }

        public double AngleTolerance
        {
            get
            {
                return m_curve4.AngleTolerance();
            }
            set
            {
                m_curve3.AngleTolerance = value;
                m_curve4.AngleTolerance(value);
            }
        }

        public double CuspLimit
        {
            get
            {
                return m_curve4.CuspLimit;
            }
            set
            {
                m_curve3.CuspLimit = value;
                m_curve4.CuspLimit = value;
            }
        }

        public void Rewind(uint path_id)
        {
            m_source.Rewind(path_id);
            m_last_x = 0.0;
            m_last_y = 0.0;
            m_curve3.reset();
            m_curve4.Reset();
        }

        public uint Vertex(out double x, out double y)
        {
            if(!Path.IsStop(m_curve3.Vertex(out x, out y)))
            {
                m_last_x = x;
                m_last_y = y;
                return (uint)Path.EPathCommands.LineTo;
            }

            if(!Path.IsStop(m_curve4.Vertex(out x, out y)))
            {
                m_last_x = x;
                m_last_y = y;
                return (uint)Path.EPathCommands.LineTo;
            }

            double ct2_x;
            double ct2_y;
            double end_x;
            double end_y;

            uint cmd = m_source.Vertex(out x, out y);
            switch(cmd)
            {
            case (uint)Path.EPathCommands.Curve3:
                m_source.Vertex(out end_x, out end_y);

                m_curve3.init(m_last_x, m_last_y, x, y, end_x, end_y);

                m_curve3.Vertex(out x, out y);    // First call returns MoveTo
                m_curve3.Vertex(out x, out y);    // This is the first Vertex of the curve
                cmd = (uint)Path.EPathCommands.LineTo;
                break;

            case (uint)Path.EPathCommands.Curve4:
                m_source.Vertex(out ct2_x, out ct2_y);
                m_source.Vertex(out end_x, out end_y);

                m_curve4.Init(m_last_x, m_last_y, x, y, ct2_x, ct2_y, end_x, end_y);

                m_curve4.Vertex(out x, out y);    // First call returns MoveTo
                m_curve4.Vertex(out x, out y);    // This is the first Vertex of the curve
                cmd = (uint)Path.EPathCommands.LineTo;
                break;
            }
            m_last_x = x;
            m_last_y = y;
            return cmd;
        }
    };
}