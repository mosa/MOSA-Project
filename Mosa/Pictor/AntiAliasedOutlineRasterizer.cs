/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;

namespace Pictor
{
    /*
    //-----------------------------------------------------------line_aa_vertex
    // Vertex (x, y) with the distance to the next one. The last Vertex has 
    // the distance between the last and the first points
    struct line_aa_vertex
    {
        int x;
        int y;
        int len;

        line_aa_vertex() {}
        line_aa_vertex(int x_, int y_)
        {
            x=(x_);
            y=(y_);
            len=(0);
        }

        bool operator () (const line_aa_vertex& val)
        {
            double dx = val.x - x;
            double dy = val.y - y;
            return (len = UnsignedRound(sqrt(dx * dx + dy * dy))) > 
                   (line_subpixel_scale + line_subpixel_scale / 2);
        }
    };

    //=======================================================rasterizer_outline_aa
    //template<class Renderer, class Coord=LineCoordinate> 
    public class rasterizer_outline_aa
    {
        Renderer*           m_ren;
        vertex_storage_type m_src_vertices;
        outline_aa_join_e   m_line_join;
        bool                m_round_cap;
        int                 m_start_x;
        int                 m_start_y;

        public enum outline_aa_join_e
        {
            outline_no_join,             //-----outline_no_join
            outline_miter_join,          //-----outline_miter_join
            outline_round_join,          //-----outline_round_join
            outline_miter_accurate_join  //-----outline_accurate_join
        };

        public bool cmp_dist_start(int d) { return d > 0;  }
        public bool cmp_dist_end(int d)   { return d <= 0; }

        //------------------------------------------------------------------------
        private struct draw_vars
        {
            unsigned idx;
            int x1, y1, x2, y2;
            LineParameters curr, next;
            int lcurr, lnext;
            int xb1, yb1, xb2, yb2;
            unsigned flags;
        };

        private void draw(draw_vars& dv, unsigned Start, unsigned End);

        //typedef line_aa_vertex                  vertex_type;
        //typedef vertex_sequence<vertex_type, 6> vertex_storage_type;

        public rasterizer_outline_aa(Renderer& ren) : 
            m_ren(&ren), 
            m_line_join(ren.accurate_join_only() ? 
                            outline_miter_accurate_join : 
                            outline_round_join),
            m_round_cap(false),
            m_start_x(0),
            m_start_y(0)
        {}
        
        public void Attach(Renderer& ren) { m_ren = &ren; }

        //------------------------------------------------------------------------
        public void ELineJoin(outline_aa_join_e join) 
        { 
            m_line_join = m_ren->accurate_join_only() ? 
                outline_miter_accurate_join : 
                join; 
        }
        public bool ELineJoin() const { return m_line_join; }

        //------------------------------------------------------------------------
        public void round_cap(bool v) { m_round_cap = v; }
        public bool round_cap() const { return m_round_cap; }

        //------------------------------------------------------------------------
        public void MoveTo(int x, int y)
        {
            m_src_vertices.modify_last(vertex_type(m_start_x = x, m_start_y = y));
        }

        //------------------------------------------------------------------------
        public void LineTo(int x, int y)
        {
            m_src_vertices.Add(vertex_type(x, y));
        }

        //------------------------------------------------------------------------
        public void MoveToD(double x, double y)
        {
            MoveTo(Coord::Convert(x), Coord::Convert(y));
        }

        //------------------------------------------------------------------------
        public void LineToD(double x, double y)
        {
            LineTo(Coord::Convert(x), Coord::Convert(y));
        }

        //------------------------------------------------------------------------
        public void render(bool ClosePolygon);

        //------------------------------------------------------------------------
        public void AddVertex(double x, double y, unsigned cmd)
        {
            if(IsMoveTo(cmd)) 
            {
                render(false);
                MoveToD(x, y);
            }
            else 
            {
                if(IsEndPoly(cmd))
                {
                    render(IsClosed(cmd));
                    if(IsClosed(cmd)) 
                    {
                        MoveTo(m_start_x, m_start_y);
                    }
                }
                else
                {
                    LineToD(x, y);
                }
            }
        }

        //------------------------------------------------------------------------
        //template<class VertexSource>
        public void AddPath(VertexSource& vs, unsigned path_id=0)
        {
            double x;
            double y;

            unsigned cmd;
            vs.Rewind(path_id);
            while(!IsStop(cmd = vs.Vertex(&x, &y)))
            {
                AddVertex(x, y, cmd);
            }
            render(false);
        }


        //------------------------------------------------------------------------
        //template<class VertexSource, class ColorStorage, class PathId>
        public void render_all_paths(VertexSource& vs, 
                              const ColorStorage& Colors, 
                              const PathId& path_id,
                              unsigned num_paths)
        {
            for(unsigned i = 0; i < num_paths; i++)
            {
                m_ren->Color(Colors[i]);
                AddPath(vs, path_id[i]);
            }
        }


        //------------------------------------------------------------------------
        //template<class Ctrl> 
        public void render_ctrl(Ctrl& c)
        {
            unsigned i;
            for(i = 0; i < c.num_paths(); i++)
            {
                m_ren->Color(c.Color(i));
                AddPath(c, i);
            }
        }
    };








    //----------------------------------------------------------------------------
    template<class Renderer, class Coord> 
    void rasterizer_outline_aa<Renderer, Coord>::draw(draw_vars& dv, 
                                                      unsigned Start, 
                                                      unsigned End)
    {
        unsigned i;
        const vertex_storage_type::value_type* v;

        for(i = Start; i < End; i++)
        {
            if(m_line_join == outline_round_join)
            {
                dv.xb1 = dv.curr.x1 + (dv.curr.y2 - dv.curr.y1); 
                dv.yb1 = dv.curr.y1 - (dv.curr.x2 - dv.curr.x1); 
                dv.xb2 = dv.curr.x2 + (dv.curr.y2 - dv.curr.y1); 
                dv.yb2 = dv.curr.y2 - (dv.curr.x2 - dv.curr.x1);
            }

            switch(dv.flags)
            {
            case 0: m_ren->line3(dv.curr, dv.xb1, dv.yb1, dv.xb2, dv.yb2); break;
            case 1: m_ren->line2(dv.curr, dv.xb2, dv.yb2); break;
            case 2: m_ren->line1(dv.curr, dv.xb1, dv.yb1); break;
            case 3: m_ren->line0(dv.curr); break;
            }

            if(m_line_join == outline_round_join && (dv.flags & 2) == 0)
            {
                m_ren->pie(dv.curr.x2, dv.curr.y2, 
                           dv.curr.x2 + (dv.curr.y2 - dv.curr.y1),
                           dv.curr.y2 - (dv.curr.x2 - dv.curr.x1),
                           dv.curr.x2 + (dv.next.y2 - dv.next.y1),
                           dv.curr.y2 - (dv.next.x2 - dv.next.x1));
            }

            dv.x1 = dv.x2;
            dv.y1 = dv.y2;
            dv.lcurr = dv.lnext;
            dv.lnext = m_src_vertices[dv.idx].len;

            ++dv.idx;
            if(dv.idx >= m_src_vertices.Size()) dv.idx = 0; 

            v = &m_src_vertices[dv.idx];
            dv.x2 = v->x;
            dv.y2 = v->y;

            dv.curr = dv.next;
            dv.next = LineParameters(dv.x1, dv.y1, dv.x2, dv.y2, dv.lnext);
            dv.xb1 = dv.xb2;
            dv.yb1 = dv.yb2;

            switch(m_line_join)
            {
            case outline_no_join:
                dv.flags = 3;
                break;

            case outline_miter_join:
                dv.flags >>= 1;
                dv.flags |= ((dv.curr.DiagonalQuadrant() == 
                              dv.next.DiagonalQuadrant()) << 1);
                if((dv.flags & 2) == 0)
                {
                    bisectrix(dv.curr, dv.next, &dv.xb2, &dv.yb2);
                }
                break;

            case outline_round_join:
                dv.flags >>= 1;
                dv.flags |= ((dv.curr.DiagonalQuadrant() == 
                              dv.next.DiagonalQuadrant()) << 1);
                break;

            case outline_miter_accurate_join:
                dv.flags = 0;
                bisectrix(dv.curr, dv.next, &dv.xb2, &dv.yb2);
                break;
            }
        }

    //----------------------------------------------------------------------------
    template<class Renderer, class Coord> 
    void rasterizer_outline_aa<Renderer, Coord>::render(bool ClosePolygon)
    {
        m_src_vertices.close(ClosePolygon);
        draw_vars dv;
        const vertex_storage_type::value_type* v;
        int x1;
        int y1;
        int x2;
        int y2;
        int lprev;

        if(ClosePolygon)
        {
            if(m_src_vertices.Size() >= 3)
            {
                dv.idx = 2;

                v     = &m_src_vertices[m_src_vertices.Size() - 1];
                x1    = v->x;
                y1    = v->y;
                lprev = v->len;

                v  = &m_src_vertices[0];
                x2 = v->x;
                y2 = v->y;
                dv.lcurr = v->len;
                LineParameters prev(x1, y1, x2, y2, lprev);

                v = &m_src_vertices[1];
                dv.x1    = v->x;
                dv.y1    = v->y;
                dv.lnext = v->len;
                dv.curr = LineParameters(x2, y2, dv.x1, dv.y1, dv.lcurr);

                v = &m_src_vertices[dv.idx];
                dv.x2 = v->x;
                dv.y2 = v->y;
                dv.next = LineParameters(dv.x1, dv.y1, dv.x2, dv.y2, dv.lnext);

                dv.xb1 = 0;
                dv.yb1 = 0;
                dv.xb2 = 0;
                dv.yb2 = 0;

                switch(m_line_join)
                {
                case outline_no_join:
                    dv.flags = 3;
                    break;

                case outline_miter_join:
                case outline_round_join:
                    dv.flags = 
                            (prev.DiagonalQuadrant() == dv.curr.DiagonalQuadrant()) |
                        ((dv.curr.DiagonalQuadrant() == dv.next.DiagonalQuadrant()) << 1);
                    break;

                case outline_miter_accurate_join:
                    dv.flags = 0;
                    break;
                }

                if((dv.flags & 1) == 0 && m_line_join != outline_round_join)
                {
                    bisectrix(prev, dv.curr, &dv.xb1, &dv.yb1);
                }

                if((dv.flags & 2) == 0 && m_line_join != outline_round_join)
                {
                    bisectrix(dv.curr, dv.next, &dv.xb2, &dv.yb2);
                }
                draw(dv, 0, m_src_vertices.Size());
            }
        }
        else
        {
            switch(m_src_vertices.Size())
            {
                case 0:
                case 1:
                    break;

                case 2:
                {
                    v     = &m_src_vertices[0];
                    x1    = v->x;
                    y1    = v->y;
                    lprev = v->len;
                    v     = &m_src_vertices[1];
                    x2    = v->x;
                    y2    = v->y;
                    LineParameters lp(x1, y1, x2, y2, lprev);
                    if(m_round_cap) 
                    {
                        m_ren->semidot(cmp_dist_start, x1, y1, x1 + (y2 - y1), y1 - (x2 - x1));
                    }
                    m_ren->line3(lp, 
                                 x1 + (y2 - y1), 
                                 y1 - (x2 - x1),
                                 x2 + (y2 - y1), 
                                 y2 - (x2 - x1));
                    if(m_round_cap) 
                    {
                        m_ren->semidot(cmp_dist_end, x2, y2, x2 + (y2 - y1), y2 - (x2 - x1));
                    }
                }
                break;

                case 3:
                {
                    int x3, y3;
                    int lnext;
                    v     = &m_src_vertices[0];
                    x1    = v->x;
                    y1    = v->y;
                    lprev = v->len;
                    v     = &m_src_vertices[1];
                    x2    = v->x;
                    y2    = v->y;
                    lnext = v->len;
                    v     = &m_src_vertices[2];
                    x3    = v->x;
                    y3    = v->y;
                    LineParameters lp1(x1, y1, x2, y2, lprev);
                    LineParameters lp2(x2, y2, x3, y3, lnext);

                    if(m_round_cap) 
                    {
                        m_ren->semidot(cmp_dist_start, x1, y1, x1 + (y2 - y1), y1 - (x2 - x1));
                    }

                    if(m_line_join == outline_round_join)
                    {
                        m_ren->line3(lp1, x1 + (y2 - y1), y1 - (x2 - x1), 
                                          x2 + (y2 - y1), y2 - (x2 - x1));

                        m_ren->pie(x2, y2, x2 + (y2 - y1), y2 - (x2 - x1),
                                           x2 + (y3 - y2), y2 - (x3 - x2));

                        m_ren->line3(lp2, x2 + (y3 - y2), y2 - (x3 - x2),
                                          x3 + (y3 - y2), y3 - (x3 - x2));
                    }
                    else
                    {
                        bisectrix(lp1, lp2, &dv.xb1, &dv.yb1);
                        m_ren->line3(lp1, x1 + (y2 - y1), y1 - (x2 - x1),
                                          dv.xb1,         dv.yb1);

                        m_ren->line3(lp2, dv.xb1,         dv.yb1,
                                          x3 + (y3 - y2), y3 - (x3 - x2));
                    }
                    if(m_round_cap) 
                    {
                        m_ren->semidot(cmp_dist_end, x3, y3, x3 + (y3 - y2), y3 - (x3 - x2));
                    }
                }
                break;

                default:
                {
                    dv.idx = 3;

                    v     = &m_src_vertices[0];
                    x1    = v->x;
                    y1    = v->y;
                    lprev = v->len;

                    v  = &m_src_vertices[1];
                    x2 = v->x;
                    y2 = v->y;
                    dv.lcurr = v->len;
                    LineParameters prev(x1, y1, x2, y2, lprev);

                    v = &m_src_vertices[2];
                    dv.x1    = v->x;
                    dv.y1    = v->y;
                    dv.lnext = v->len;
                    dv.curr = LineParameters(x2, y2, dv.x1, dv.y1, dv.lcurr);

                    v = &m_src_vertices[dv.idx];
                    dv.x2 = v->x;
                    dv.y2 = v->y;
                    dv.next = LineParameters(dv.x1, dv.y1, dv.x2, dv.y2, dv.lnext);

                    dv.xb1 = 0;
                    dv.yb1 = 0;
                    dv.xb2 = 0;
                    dv.yb2 = 0;

                    switch(m_line_join)
                    {
                    case outline_no_join:
                        dv.flags = 3;
                        break;

                    case outline_miter_join:
                    case outline_round_join:
                        dv.flags = 
                                (prev.DiagonalQuadrant() == dv.curr.DiagonalQuadrant()) |
                            ((dv.curr.DiagonalQuadrant() == dv.next.DiagonalQuadrant()) << 1);
                        break;

                    case outline_miter_accurate_join:
                        dv.flags = 0;
                        break;
                    }

                    if(m_round_cap) 
                    {
                        m_ren->semidot(cmp_dist_start, x1, y1, x1 + (y2 - y1), y1 - (x2 - x1));
                    }
                    if((dv.flags & 1) == 0)
                    {
                        if(m_line_join == outline_round_join)
                        {
                            m_ren->line3(prev, x1 + (y2 - y1), y1 - (x2 - x1),
                                               x2 + (y2 - y1), y2 - (x2 - x1));
                            m_ren->pie(prev.x2, prev.y2, 
                                       x2 + (y2 - y1), y2 - (x2 - x1),
                                       dv.curr.x1 + (dv.curr.y2 - dv.curr.y1), 
                                       dv.curr.y1 - (dv.curr.x2 - dv.curr.x1));
                        }
                        else
                        {
                            bisectrix(prev, dv.curr, &dv.xb1, &dv.yb1);
                            m_ren->line3(prev, x1 + (y2 - y1), y1 - (x2 - x1),
                                               dv.xb1,         dv.yb1);
                        }
                    }
                    else
                    {
                        m_ren->line1(prev, 
                                     x1 + (y2 - y1), 
                                     y1 - (x2 - x1));
                    }
                    if((dv.flags & 2) == 0 && m_line_join != outline_round_join)
                    {
                        bisectrix(dv.curr, dv.next, &dv.xb2, &dv.yb2);
                    }

                    draw(dv, 1, m_src_vertices.Size() - 2);

                    if((dv.flags & 1) == 0)
                    {
                        if(m_line_join == outline_round_join)
                        {
                            m_ren->line3(dv.curr, 
                                         dv.curr.x1 + (dv.curr.y2 - dv.curr.y1), 
                                         dv.curr.y1 - (dv.curr.x2 - dv.curr.x1),
                                         dv.curr.x2 + (dv.curr.y2 - dv.curr.y1), 
                                         dv.curr.y2 - (dv.curr.x2 - dv.curr.x1));
                        }
                        else
                        {
                            m_ren->line3(dv.curr, dv.xb1, dv.yb1,
                                         dv.curr.x2 + (dv.curr.y2 - dv.curr.y1), 
                                         dv.curr.y2 - (dv.curr.x2 - dv.curr.x1));
                        }
                    }
                    else
                    {
                        m_ren->line2(dv.curr, 
                                     dv.curr.x2 + (dv.curr.y2 - dv.curr.y1), 
                                     dv.curr.y2 - (dv.curr.x2 - dv.curr.x1));
                    }
                    if(m_round_cap) 
                    {
                        m_ren->semidot(cmp_dist_end, dv.curr.x2, dv.curr.y2,
                                       dv.curr.x2 + (dv.curr.y2 - dv.curr.y1),
                                       dv.curr.y2 - (dv.curr.x2 - dv.curr.x1));
                    }

                }
                break;
            }
        }
        m_src_vertices.RemoveAll();
    }

}
     */
}