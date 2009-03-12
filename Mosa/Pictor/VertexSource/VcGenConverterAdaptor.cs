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
    //------------------------------------------------------------NullMarkers
    public struct NullMarkers : IMarkers
    {
        public void RemoveAll() { }
        public void AddVertex(double x, double y, uint unknown) { }
        public void prepare_src() { }

        public void Rewind(uint unknown) { }
        public uint Vertex(ref double x, ref double y) { return (uint)Path.EPathCommands.Stop; }
    };


    //------------------------------------------------------conv_adaptor_vcgen
    public class conv_adaptor_vcgen
    {
        private enum EStatus
        {
            initial,
            accumulate,
            generate
        };

        public conv_adaptor_vcgen(IVertexSource source, IGenerator generator)
        {
            m_markers = new NullMarkers();
            m_source = source;
            m_generator = generator;
            m_status = EStatus.initial;
        }

        public conv_adaptor_vcgen(IVertexSource source, IGenerator generator, IMarkers markers)
            : this(source, generator)
        {
            m_markers = markers;
        }
        void Attach(IVertexSource source) { m_source = source; }

        protected IGenerator Generator() { return m_generator; }

        IMarkers markers() { return m_markers; }

        public void Rewind(uint path_id) 
        { 
            m_source.Rewind(path_id);
            m_status = EStatus.initial;
        }

        public uint Vertex(out double x, out double y)
        {
            x = 0;
            y = 0;
            uint cmd = (uint)Path.EPathCommands.Stop;
            bool done = false;
            while(!done)
            {
                switch(m_status)
                {
                    case EStatus.initial:
                    m_markers.RemoveAll();
                    m_last_cmd = m_source.Vertex(out m_start_x, out m_start_y);
                    m_status = EStatus.accumulate;
                    goto case EStatus.accumulate;

                case EStatus.accumulate:
                    if(Path.IsStop(m_last_cmd)) return (uint)Path.EPathCommands.Stop;

                    m_generator.RemoveAll();
                    m_generator.AddVertex(m_start_x, m_start_y, (uint)Path.EPathCommands.MoveTo);
                    m_markers.AddVertex(m_start_x, m_start_y, (uint)Path.EPathCommands.MoveTo);

                    for(;;)
                    {
                        cmd = m_source.Vertex(out x, out y);
                        //DebugFile.Print("x=" + x.ToString() + " y=" + y.ToString() + "\n");
                        if (Path.IsVertex(cmd))
                        {
                            m_last_cmd = cmd;
                            if(Path.IsMoveTo(cmd))
                            {
                                m_start_x = x;
                                m_start_y = y;
                                break;
                            }
                            m_generator.AddVertex(x, y, cmd);
                            m_markers.AddVertex(x, y, (uint)Path.EPathCommands.LineTo);
                        }
                        else
                        {
                            if(Path.IsStop(cmd))
                            {
                                m_last_cmd = (uint)Path.EPathCommands.Stop;
                                break;
                            }
                            if(Path.IsEndPoly(cmd))
                            {
                                m_generator.AddVertex(x, y, cmd);
                                break;
                            }
                        }
                    }
                    m_generator.Rewind(0);
                    m_status = EStatus.generate;
                    goto case EStatus.generate;

                case EStatus.generate:
                    cmd = m_generator.Vertex(ref x, ref y);
                    //DebugFile.Print("x=" + x.ToString() + " y=" + y.ToString() + "\n");
                    if (Path.IsStop(cmd))
                    {
                        m_status = EStatus.accumulate;
                        break;
                    }
                    done = true;
                    break;
                }
            }
            return cmd;
        }

        private IVertexSource  m_source;
        private IGenerator     m_generator;
        private IMarkers       m_markers;
        private EStatus        m_status;
        private uint m_last_cmd;
        private double        m_start_x;
        private double        m_start_y;
    };
}