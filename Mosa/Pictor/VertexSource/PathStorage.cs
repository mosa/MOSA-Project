/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */
using System;

namespace Pictor.VertexSource
{
    //---------------------------------------------------------------path_base
    // A container to store vertices with their flags. 
    // A path consists of a number of contours separated with "MoveTo" 
    // commands. The path storage can keep and maintain more than one
    // path. 
    // To navigate to the beginning of a particular path, use Rewind(path_id);
    // Where path_id is what start_new_path() returns. So, when you call
    // start_new_path() you need to store its return Value somewhere else
    // to navigate to the path afterwards.
    //
    // See also: vertex_source concept
    //------------------------------------------------------------------------
    public class PathStorage : IVertexSource, IVertexDest
    {

        #region InternalVertexStarge
        private class VertexStorage
        {
            uint m_num_vertices;
            uint m_allocated_vertices;
            double[] m_coord_x;
            double[] m_coord_y;
            uint[] m_CommandAndFlags;

            public void free_all()
            {
                m_coord_x = null;
                m_coord_y = null;
                m_CommandAndFlags = null;

                m_num_vertices = 0;
            }

            public uint size()
            {
                return m_num_vertices;
            }

            public VertexStorage()
            {
            }

            public void remove_all()
            {
                m_num_vertices = 0;
            }

            public void add_vertex(double x, double y, uint CommandAndFlags)
            {
                allocate_if_required(m_num_vertices);
                m_coord_x[m_num_vertices] = x;
                m_coord_y[m_num_vertices] = y;
                m_CommandAndFlags[m_num_vertices] = CommandAndFlags;

                m_num_vertices++;
            }

            public void modify_vertex(uint idx, double x, double y)
            {
                m_coord_x[idx] = x;
                m_coord_y[idx] = y;
            }

            public void modify_vertex(uint idx, double x, double y, uint CommandAndFlags)
            {
                m_coord_x[idx] = x;
                m_coord_y[idx] = y;
                m_CommandAndFlags[idx] = CommandAndFlags;
            }

            public void modify_command(uint idx, uint CommandAndFlags)
            {
                m_CommandAndFlags[idx] = CommandAndFlags;
            }

            public void swap_vertices(uint v1, uint v2)
            {
                double val;

                val = m_coord_x[v1]; m_coord_x[v1] = m_coord_x[v2]; m_coord_x[v2] = val;
                val = m_coord_y[v1]; m_coord_y[v1] = m_coord_y[v2]; m_coord_y[v2] = val;
                uint cmd = m_CommandAndFlags[v1]; m_CommandAndFlags[v1] = m_CommandAndFlags[v2]; m_CommandAndFlags[v2] = cmd;
            }

            public uint last_command()
            {
                if (m_num_vertices != 0)
                {
                    return command(m_num_vertices - 1);
                }

                return (uint)Path.EPathCommands.Stop;
            }

            public uint last_vertex(out double x, out double y)
            {
                if (m_num_vertices != 0)
                {
                    return vertex((uint)(m_num_vertices - 1), out x, out y);
                }

                x = new double();
                y = new double();
                return (uint)Path.EPathCommands.Stop;
            }

            public uint prev_vertex(out double x, out double y)
            {
                if (m_num_vertices > 1)
                {
                    return vertex((uint)(m_num_vertices - 2), out x, out y);
                }

                x = new double();
                y = new double();
                return (uint)Path.EPathCommands.Stop;
            }

            public double last_x()
            {
                if (m_num_vertices > 1)
                {
                    uint idx = (uint)(m_num_vertices - 1);
                    return m_coord_x[idx];
                }

                return new double();
            }

            public double last_y()
            {
                if (m_num_vertices > 1)
                {
                    uint idx = (uint)(m_num_vertices - 1);
                    return m_coord_y[idx];
                }
                return new double();
            }

            public uint total_vertices()
            {
                return m_num_vertices;
            }

            public uint vertex(uint idx, out double x, out double y)
            {
                x = m_coord_x[idx];
                y = m_coord_y[idx];
                return m_CommandAndFlags[idx];
            }

            public uint command(uint idx)
            {
                return m_CommandAndFlags[idx];
            }

            private void allocate_if_required(uint indexToAdd)
            {
                if (indexToAdd < m_num_vertices)
                {
                    return;
                }

                while (indexToAdd >= m_allocated_vertices)
                {
                    uint newSize = m_allocated_vertices + 256;
                    double[] newX = new double[newSize];
                    double[] newY = new double[newSize];
                    uint[] newCmd = new uint[newSize];

                    if (m_coord_x != null)
                    {
                        for (uint i = 0; i < m_num_vertices; i++)
                        {
                            newX[i] = m_coord_x[i];
                            newY[i] = m_coord_y[i];
                            newCmd[i] = m_CommandAndFlags[i];
                        }
                    }

                    m_coord_x = newX;
                    m_coord_y = newY;
                    m_CommandAndFlags = newCmd;

                    m_allocated_vertices = newSize;
                }
            }
        };
        #endregion

        private VertexStorage m_vertices;
        private uint m_iterator;

        public PathStorage()
        {
            m_vertices = new VertexStorage();
        }

        public void Add(PointD vertex)
        {
            throw new System.NotImplementedException();
        }

        public uint Size()
        {
            return m_vertices.size();
        }

        public PointD this[int i]
        {
            get
            {
                throw new NotImplementedException("make this work");
            }
        }

        public void RemoveAll() { m_vertices.remove_all(); m_iterator = 0; }
        public void free_all()   { m_vertices.free_all();   m_iterator = 0; }

        // Make path functions
        //--------------------------------------------------------------------
        public uint start_new_path()
        {
            if(!Path.IsStop(m_vertices.last_command()))
            {
                m_vertices.add_vertex(0.0, 0.0, (uint)Path.EPathCommands.Stop);
            }
            return m_vertices.total_vertices();
        }


        public void rel_to_abs(ref double x, ref double y)
        {
            if(m_vertices.total_vertices() != 0)
            {
                double x2;
                double y2;
                if(Path.IsVertex(m_vertices.last_vertex(out x2, out y2)))
                {
                    x += x2;
                    y += y2;
                }
            }
        }

        public void move_to(double x, double y)
        {
            m_vertices.add_vertex(x, y, (uint)Path.EPathCommands.MoveTo);
        }

        public void move_rel(double dx, double dy)
        {
            rel_to_abs(ref dx, ref dy);
            m_vertices.add_vertex(dx, dy, (uint)Path.EPathCommands.MoveTo);
        }

        public void line_to(double x, double y)
        {
            m_vertices.add_vertex(x, y, (uint)Path.EPathCommands.LineTo);
        }

        public void line_rel(double dx, double dy)
        {
            rel_to_abs(ref dx, ref dy);
            m_vertices.add_vertex(dx, dy, (uint)Path.EPathCommands.LineTo);
        }

        public void hline_to(double x)
        {
            m_vertices.add_vertex(x, last_y(), (uint)Path.EPathCommands.LineTo);
        }

        public void hline_rel(double dx)
        {
            double dy = 0;
            rel_to_abs(ref dx, ref dy);
            m_vertices.add_vertex(dx, dy, (uint)Path.EPathCommands.LineTo);
        }

        public void vline_to(double y)
        {
            m_vertices.add_vertex(last_x(), y, (uint)Path.EPathCommands.LineTo);
        }

        public void vline_rel(double dy)
        {
            double dx = 0;
            rel_to_abs(ref dx, ref dy);
            m_vertices.add_vertex(dx, dy, (uint)Path.EPathCommands.LineTo);
        }

        /*
        public void arc_to(double rx, double ry,
                                   double angle,
                                   bool large_arc_flag,
                                   bool sweep_flag,
                                   double x, double y)
        {
            if(m_vertices.total_vertices() && IsVertex(m_vertices.last_command()))
            {
                double epsilon = 1e-30;
                double x0 = 0.0;
                double y0 = 0.0;
                m_vertices.last_vertex(&x0, &y0);

                rx = fabs(rx);
                ry = fabs(ry);

                // Ensure radii are valid
                //-------------------------
                if(rx < epsilon || ry < epsilon) 
                {
                    LineTo(x, y);
                    return;
                }

                if(CalculateDistance(x0, y0, x, y) < epsilon)
                {
                    // If the endpoints (x, y) and (x0, y0) are identical, then this
                    // is equivalent to omitting the elliptical Arc segment entirely.
                    return;
                }
                bezier_arc_svg a(x0, y0, rx, ry, angle, large_arc_flag, sweep_flag, x, y);
                if(a.radii_ok())
                {
                    join_path(a);
                }
                else
                {
                    LineTo(x, y);
                }
            }
            else
            {
                MoveTo(x, y);
            }
        }

        public void arc_rel(double rx, double ry,
                                    double angle,
                                    bool large_arc_flag,
                                    bool sweep_flag,
                                    double dx, double dy)
        {
            rel_to_abs(&dx, &dy);
            arc_to(rx, ry, angle, large_arc_flag, sweep_flag, dx, dy);
        }
         */

        public void curve3(double x_ctrl, double y_ctrl, 
                                   double x_to,   double y_to)
        {
            m_vertices.add_vertex(x_ctrl, y_ctrl, (uint)Path.EPathCommands.Curve3);
            m_vertices.add_vertex(x_to, y_to, (uint)Path.EPathCommands.Curve3);
        }

        public void curve3_rel(double dx_ctrl, double dy_ctrl, double dx_to, double dy_to)
        {
            rel_to_abs(ref dx_ctrl, ref dy_ctrl);
            rel_to_abs(ref dx_to,   ref dy_to);
            m_vertices.add_vertex(dx_ctrl, dy_ctrl, (uint)Path.EPathCommands.Curve3);
            m_vertices.add_vertex(dx_to, dy_to, (uint)Path.EPathCommands.Curve3);
        }

        public void curve3(double x_to, double y_to)
        {
            double x0;
            double y0;
            if(Path.IsVertex(m_vertices.last_vertex(out x0, out y0)))
            {
                double x_ctrl;
                double y_ctrl; 
                uint cmd = m_vertices.prev_vertex(out x_ctrl, out y_ctrl);
                if(Path.IsCurve(cmd))
                {
                    x_ctrl = x0 + x0 - x_ctrl;
                    y_ctrl = y0 + y0 - y_ctrl;
                }
                else
                {
                    x_ctrl = x0;
                    y_ctrl = y0;
                }
                curve3(x_ctrl, y_ctrl, x_to, y_to);
            }
        }

        public void curve3_rel(double dx_to, double dy_to)
        {
            rel_to_abs(ref dx_to, ref dy_to);
            curve3(dx_to, dy_to);
        }

        public void curve4(double x_ctrl1, double y_ctrl1, 
                                   double x_ctrl2, double y_ctrl2, 
                                   double x_to,    double y_to)
        {
            m_vertices.add_vertex(x_ctrl1, y_ctrl1, (uint)Path.EPathCommands.Curve4);
            m_vertices.add_vertex(x_ctrl2, y_ctrl2, (uint)Path.EPathCommands.Curve4);
            m_vertices.add_vertex(x_to, y_to, (uint)Path.EPathCommands.Curve4);
        }

        public void curve4_rel(double dx_ctrl1, double dy_ctrl1, 
                                       double dx_ctrl2, double dy_ctrl2, 
                                       double dx_to,    double dy_to)
        {
            rel_to_abs(ref dx_ctrl1, ref dy_ctrl1);
            rel_to_abs(ref dx_ctrl2, ref dy_ctrl2);
            rel_to_abs(ref dx_to, ref dy_to);
            m_vertices.add_vertex(dx_ctrl1, dy_ctrl1, (uint)Path.EPathCommands.Curve4);
            m_vertices.add_vertex(dx_ctrl2, dy_ctrl2, (uint)Path.EPathCommands.Curve4);
            m_vertices.add_vertex(dx_to, dy_to, (uint)Path.EPathCommands.Curve4);
        }

        public void curve4(double x_ctrl2, double y_ctrl2, 
                                   double x_to,    double y_to)
        {
            double x0;
            double y0;
            if(Path.IsVertex(last_vertex(out x0, out y0)))
            {
                double x_ctrl1;
                double y_ctrl1;
                uint cmd = prev_vertex(out x_ctrl1, out y_ctrl1);
                if(Path.IsCurve(cmd))
                {
                    x_ctrl1 = x0 + x0 - x_ctrl1;
                    y_ctrl1 = y0 + y0 - y_ctrl1;
                }
                else
                {
                    x_ctrl1 = x0;
                    y_ctrl1 = y0;
                }
                curve4(x_ctrl1, y_ctrl1, x_ctrl2, y_ctrl2, x_to, y_to);
            }
        }

        public void curve4_rel(double dx_ctrl2, double dy_ctrl2, 
                                       double dx_to,    double dy_to)
        {
            rel_to_abs(ref dx_ctrl2, ref dy_ctrl2);
            rel_to_abs(ref dx_to,    ref dy_to);
            curve4(dx_ctrl2, dy_ctrl2, dx_to, dy_to);
        }

        public uint total_vertices()
        {
            return m_vertices.total_vertices();
        }

        public uint last_vertex(out double x, out double y)
        {
            return m_vertices.last_vertex(out x, out y);
        }

        public uint prev_vertex(out double x, out double y)
        {
            return m_vertices.prev_vertex(out x, out y);
        }

        public double last_x()
        {
            return m_vertices.last_x();
        }

        public double last_y()
        {
            return m_vertices.last_y();
        }

        public uint Vertex(uint idx, out double x, out double y)
        {
            return m_vertices.vertex(idx, out x, out y);
        }
     
        public uint command(uint idx)
        {
            return m_vertices.command(idx);
        }

        public void modify_vertex(uint idx, double x, double y)
        {
            m_vertices.modify_vertex(idx, x, y);
        }

        public void modify_vertex(uint idx, double x, double y, uint PathAndFlags)
        {
            m_vertices.modify_vertex(idx, x, y, PathAndFlags);
        }

        public void modify_command(uint idx, uint PathAndFlags)
        {
            m_vertices.modify_command(idx, PathAndFlags);
        }

        public virtual void Rewind(uint path_id)
        {
            m_iterator = path_id;
        }

        public uint Vertex(out double x, out double y)
        {
            if (m_iterator >= m_vertices.total_vertices())
            {
                x = 0;
                y = 0;
                return (uint)Path.EPathCommands.Stop;
            }

            return m_vertices.vertex(m_iterator++, out x, out y);
        }

        // Arrange the orientation of a polygon, all polygons in a path, 
        // or in all paths. After calling arrange_orientations() or 
        // arrange_orientations_all_paths(), all the polygons will have 
        // the same orientation, i.e. Clockwise or CounterClockwise
        //--------------------------------------------------------------------
        public uint arrange_polygon_orientation(uint start, Path.EPathFlags orientation)
        {
            if(orientation == Path.EPathFlags.None) return start;
            
            // Skip all non-vertices At the beginning
            while(start < m_vertices.total_vertices() && 
                  !Path.IsVertex(m_vertices.command(start))) ++start;

            // Skip all insignificant MoveTo
            while(start+1 < m_vertices.total_vertices() && 
                  Path.IsMoveTo(m_vertices.command(start)) &&
                  Path.IsMoveTo(m_vertices.command(start+1))) ++start;

            // Find the last Vertex
            uint end = start + 1;
            while(end < m_vertices.total_vertices() && 
                  !Path.IsNextPoly(m_vertices.command(end))) ++end;

            if(end - start > 2)
            {
                if(perceive_polygon_orientation(start, end) != orientation)
                {
                    // Invert polygon, Set orientation flag, and skip all end_poly
                    invert_polygon(start, end);
                    uint PathAndFlags;
                    while(end < m_vertices.total_vertices() &&
                          Path.IsEndPoly(PathAndFlags = m_vertices.command(end)))
                    {
                        m_vertices.modify_command(end++, PathAndFlags | (uint)orientation);// Path.set_orientation(cmd, orientation));
                    }
                }
            }
            return end;
        }

        public uint arrange_orientations(uint start, Path.EPathFlags orientation)
        {
            if(orientation != Path.EPathFlags.None)
            {
                while(start < m_vertices.total_vertices())
                {
                    start = arrange_polygon_orientation(start, orientation);
                    if(Path.IsStop(m_vertices.command(start)))
                    {
                        ++start;
                        break;
                    }
                }
            }
            return start;
        }

        public void arrange_orientations_all_paths(Path.EPathFlags orientation)
        {
            if(orientation != Path.EPathFlags.None)
            {
                uint start = 0;
                while(start < m_vertices.total_vertices())
                {
                    start = arrange_orientations(start, orientation);
                }
            }
        }

        // Flip all vertices horizontally or vertically, 
        // between x1 and x2, or between y1 and y2 respectively
        //--------------------------------------------------------------------
        public void flip_x(double x1, double x2)
        {
            uint i;
            double x, y;
            for(i = 0; i < m_vertices.total_vertices(); i++)
            {
                uint PathAndFlags = m_vertices.vertex(i, out x, out y);
                if (Path.IsVertex(PathAndFlags))
                {
                    m_vertices.modify_vertex(i, x2 - x + x1, y);
                }
            }
        }

        public void flip_y(double y1, double y2)
        {
            uint i;
            double x, y;
            for(i = 0; i < m_vertices.total_vertices(); i++)
            {
                uint PathAndFlags = m_vertices.vertex(i, out x, out y);
                if (Path.IsVertex(PathAndFlags))
                {
                    m_vertices.modify_vertex(i, x, y2 - y + y1);
                }
            }
        }

        public void end_poly()
        {
            close_polygon((uint)Path.EPathFlags.Close);
        }

        public void end_poly(uint flags)
        {
            if (Path.IsVertex(m_vertices.last_command()))
            {
                m_vertices.add_vertex(0.0, 0.0, (uint)Path.EPathCommands.EndPoly | flags);
            }
        }


        public void close_polygon()
        {
            close_polygon((uint)Path.EPathFlags.None);
        }

        public void close_polygon(uint flags)
        {
            end_poly((uint)Path.EPathFlags.Close | flags);
        }

        // Concatenate path. The path is added as is.
        public void concat_path(IVertexSource vs)
        {
            concat_path(vs, 0);
        }

        public void concat_path(IVertexSource vs, uint path_id)
        {
            double x, y;
            uint PathAndFlags;
            vs.Rewind(path_id);
            while (!Path.IsStop(PathAndFlags = vs.Vertex(out x, out y)))
            {
                m_vertices.add_vertex(x, y, PathAndFlags);
            }
        }

        //--------------------------------------------------------------------
        // Join path. The path is joined with the existing one, that is, 
        // it behaves as if the pen of a plotter was always down (drawing)
        //template<class VertexSource> 
        public void join_path(PathStorage vs)
        {
            join_path(vs, 0);

        }

        public void join_path(PathStorage vs, uint path_id)
        {
            double x, y;
            vs.Rewind(path_id);
            uint PathAndFlags = vs.Vertex(out x, out y);
            if (!Path.IsStop(PathAndFlags))
            {
                if (Path.IsVertex(PathAndFlags))
                {
                    double x0, y0;
                    uint PathAndFlags0 = last_vertex(out x0, out y0);
                    if (Path.IsVertex(PathAndFlags0))
                    {
                        if(PictorMath.CalculateDistance(x, y, x0, y0) > PictorMath.vertex_dist_epsilon)
                        {
                            if (Path.IsMoveTo(PathAndFlags)) PathAndFlags = (uint)Path.EPathCommands.LineTo;
                            m_vertices.add_vertex(x, y, PathAndFlags);
                        }
                    }
                    else
                    {
                        if (Path.IsStop(PathAndFlags0))
                        {
                            PathAndFlags = (uint)Path.EPathCommands.MoveTo;
                        }
                        else
                        {
                            if (Path.IsMoveTo(PathAndFlags)) PathAndFlags = (uint)Path.EPathCommands.LineTo;
                        }
                        m_vertices.add_vertex(x, y, PathAndFlags);
                    }
                }
                while (!Path.IsStop(PathAndFlags = vs.Vertex(out x, out y)))
                {
                    m_vertices.add_vertex(x, y, Path.IsMoveTo(PathAndFlags) ? 
                                                    (uint)Path.EPathCommands.LineTo :
                                                    PathAndFlags);
                }
            }
        }

        /*
        // Concatenate polygon/polyline. 
        //--------------------------------------------------------------------
        void concat_poly(T* Data, uint num_points, bool closed)
        {
            poly_plain_adaptor<T> poly(Data, num_points, closed);
            concat_path(poly);
        }

        // Join polygon/polyline continuously.
        //--------------------------------------------------------------------
        void join_poly(T* Data, uint num_points, bool closed)
        {
            poly_plain_adaptor<T> poly(Data, num_points, closed);
            join_path(poly);
        }
         */

        //--------------------------------------------------------------------
        public void translate(double dx, double dy)
        {
            translate(dx, dy, 0);
        }

        public void translate(double dx, double dy, uint path_id)
        {
            uint num_ver = m_vertices.total_vertices();
            for(; path_id < num_ver; path_id++)
            {
                double x, y;
                uint PathAndFlags = m_vertices.vertex(path_id, out x, out y);
                if (Path.IsStop(PathAndFlags)) break;
                if (Path.IsVertex(PathAndFlags))
                {
                    x += dx;
                    y += dy;
                    m_vertices.modify_vertex(path_id, x, y);
                }
            }
        }

        public void translate_all_paths(double dx, double dy)
        {
            uint idx;
            uint num_ver = m_vertices.total_vertices();
            for(idx = 0; idx < num_ver; idx++)
            {
                double x, y;
                if(Path.IsVertex(m_vertices.vertex(idx, out x, out y)))
                {
                    x += dx;
                    y += dy;
                    m_vertices.modify_vertex(idx, x, y);
                }
            }
        }

        //--------------------------------------------------------------------
        public void transform(Transform.Affine trans)
        {
            transform(trans, 0);
        }

        public void transform(Transform.Affine trans, uint path_id)
        {
            uint num_ver = m_vertices.total_vertices();
            for(; path_id < num_ver; path_id++)
            {
                double x, y;
                uint PathAndFlags = m_vertices.vertex(path_id, out x, out y);
                if (Path.IsStop(PathAndFlags)) break;
                if (Path.IsVertex(PathAndFlags))
                {
                    trans.Transform(ref x, ref y);
                    m_vertices.modify_vertex(path_id, x, y);
                }
            }
        }

        //--------------------------------------------------------------------
        public void transform_all_paths(Transform.Affine trans)
        {
            uint idx;
            uint num_ver = m_vertices.total_vertices();
            for(idx = 0; idx < num_ver; idx++)
            {
                double x, y;
                if(Path.IsVertex(m_vertices.vertex(idx, out x, out y)))
                {
                    trans.Transform(ref x, ref y);
                    m_vertices.modify_vertex(idx, x, y);
                }
            }
        }

        public void invert_polygon(uint start)
        {
            // Skip all non-vertices At the beginning
            while (start < m_vertices.total_vertices() &&
                  !Path.IsVertex(m_vertices.command(start))) ++start;

            // Skip all insignificant MoveTo
            while (start + 1 < m_vertices.total_vertices() &&
                  Path.IsMoveTo(m_vertices.command(start)) &&
                  Path.IsMoveTo(m_vertices.command(start + 1))) ++start;

            // Find the last Vertex
            uint end = start + 1;
            while (end < m_vertices.total_vertices() &&
                  !Path.IsNextPoly(m_vertices.command(end))) ++end;

            invert_polygon(start, end);
        }

        private Path.EPathFlags perceive_polygon_orientation(uint start, uint end)
        {
            // Calculate signed area (double area to be exact)
            //---------------------
            uint np = end - start;
            double area = 0.0;
            uint i;
            for (i = 0; i < np; i++)
            {
                double x1, y1, x2, y2;
                m_vertices.vertex(start + i, out x1, out y1);
                m_vertices.vertex(start + (i + 1) % np, out x2, out y2);
                area += x1 * y2 - y1 * x2;
            }
            return (area < 0.0) ? Path.EPathFlags.Clockwise : Path.EPathFlags.CounterClockwise;
        }

        private void invert_polygon(uint start, uint end)
        {
            uint i;
            uint tmp_PathAndFlags = m_vertices.command(start);

            --end; // Make "End" inclusive

            // Shift all commands to one position
            for (i = start; i < end; i++)
            {
                m_vertices.modify_command(i, m_vertices.command(i + 1));
            }

            // Assign starting command to the ending command
            m_vertices.modify_command(end, tmp_PathAndFlags);

            // Reverse the polygon
            while (end > start)
            {
                m_vertices.swap_vertices(start++, end--);
            }
        }
    };
}