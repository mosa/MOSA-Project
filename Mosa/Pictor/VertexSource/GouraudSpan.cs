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

    //============================================================span_gouraud
    public class span_gouraud : IVertexSource
    {
        coord_type[] m_coord = new coord_type[3];
        double[] m_x = new double[8];
        double[] m_y = new double[8];
        uint[] m_cmd = new uint[8];
        uint m_vertex;

        public struct coord_type
        {
            public double x;
            public double y;
            public RGBA_Bytes color;
        };

        //--------------------------------------------------------------------
        public span_gouraud()
        {
            m_vertex = (0);
            m_cmd[0] = (uint)Path.EPathCommands.Stop;
        }

        //--------------------------------------------------------------------
        public span_gouraud(RGBA_Bytes c1,
                     RGBA_Bytes c2,
                     RGBA_Bytes c3,
                     double x1, double y1,
                     double x2, double y2,
                     double x3, double y3,
                     double d)
        {
            m_vertex=(0);
            colors(c1, c2, c3);
            triangle(x1, y1, x2, y2, x3, y3, d);
        }

        //--------------------------------------------------------------------
        public void colors(IColorType c1, IColorType c2, IColorType c3)
        {
            m_coord[0].color = c1.GetAsRGBA_Bytes();
            m_coord[1].color = c2.GetAsRGBA_Bytes();
            m_coord[2].color = c3.GetAsRGBA_Bytes();
        }

        //--------------------------------------------------------------------
        // Sets the triangle and dilates it if needed.
        // The trick here is to Calculate beveled joins in the vertices of the 
        // triangle and render it as a 6-Vertex polygon. 
        // It's necessary to achieve numerical stability. 
        // However, the Coordinates to interpolate Colors are calculated
        // as miter joins (CalculateIntersection).
        public void triangle(double x1, double y1, 
                      double x2, double y2,
                      double x3, double y3,
                      double d)
        {
            m_coord[0].x = m_x[0] = x1; 
            m_coord[0].y = m_y[0] = y1;
            m_coord[1].x = m_x[1] = x2; 
            m_coord[1].y = m_y[1] = y2;
            m_coord[2].x = m_x[2] = x3; 
            m_coord[2].y = m_y[2] = y3;
            m_cmd[0] = (uint)Path.EPathCommands.MoveTo;
            m_cmd[1] = (uint)Path.EPathCommands.LineTo;
            m_cmd[2] = (uint)Path.EPathCommands.LineTo;
            m_cmd[3] = (uint)Path.EPathCommands.Stop;

            if(d != 0.0)
            {   
                PictorMath.DilateTriangle(m_coord[0].x, m_coord[0].y,
                                m_coord[1].x, m_coord[1].y,
                                m_coord[2].x, m_coord[2].y,
                                m_x, m_y, d);

                PictorMath.CalculateIntersection(m_x[4], m_y[4], m_x[5], m_y[5],
                                  m_x[0], m_y[0], m_x[1], m_y[1],
                                  out m_coord[0].x, out m_coord[0].y);

                PictorMath.CalculateIntersection(m_x[0], m_y[0], m_x[1], m_y[1],
                                  m_x[2], m_y[2], m_x[3], m_y[3],
                                  out m_coord[1].x, out m_coord[1].y);

                PictorMath.CalculateIntersection(m_x[2], m_y[2], m_x[3], m_y[3],
                                  m_x[4], m_y[4], m_x[5], m_y[5],
                                  out m_coord[2].x, out m_coord[2].y);
                m_cmd[3] = (uint)Path.EPathCommands.LineTo;
                m_cmd[4] = (uint)Path.EPathCommands.LineTo;
                m_cmd[5] = (uint)Path.EPathCommands.LineTo;
                m_cmd[6] = (uint)Path.EPathCommands.Stop;
            }
        }

        //--------------------------------------------------------------------
        // Vertex Source Interface to feed the Coordinates to the rasterizer
        public void Rewind(uint idx)
        {
            m_vertex = 0;
        }

        //--------------------------------------------------------------------
        public uint Vertex(out double x, out double y)
        {
            x = m_x[m_vertex];
            y = m_y[m_vertex];
            return m_cmd[m_vertex++];
        }

        //--------------------------------------------------------------------
        protected void arrange_vertices(coord_type[] coord)
        {
            unsafe
            {
                coord[0] = m_coord[0];
                coord[1] = m_coord[1];
                coord[2] = m_coord[2];

                if (m_coord[0].y > m_coord[2].y)
                {
                    coord[0] = m_coord[2];
                    coord[2] = m_coord[0];
                }

                coord_type tmp;
                if (coord[0].y > coord[1].y)
                {
                    tmp = coord[1];
                    coord[1] = coord[0];
                    coord[0] = tmp;
                }

                if (coord[1].y > coord[2].y)
                {
                    tmp = coord[2];
                    coord[2] = coord[1];
                    coord[1] = tmp;
                }
            }
       }
    };
}
