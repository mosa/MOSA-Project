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
    ///<summary>
    ///</summary>
    public struct NullMarkers : IMarkers
    {
        ///<summary>
        ///</summary>
        public void RemoveAll() { }
        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="unknown"></param>
        public void AddVertex(double x, double y, uint unknown) { }
        ///<summary>
        ///</summary>
        public void PrepareSource() { }

        ///<summary>
        ///</summary>
        ///<param name="unknown"></param>
        public void Rewind(uint unknown) { }
        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<returns></returns>
        public uint Vertex(ref double x, ref double y) { return (uint)Path.EPathCommands.Stop; }
    };

    ///<summary>
    ///</summary>
    public class ConverterAdaptorVcgen
    {
        private enum EStatus
        {
            Initial,
            Accumulate,
            Generate
        };

        private readonly IGenerator _generator;
        private readonly IMarkers _markers;
        private IVertexSource _source;
        private EStatus _status;
        private uint _lastCommand;
        private double _startX;
        private double _startY;

        ///<summary>
        ///</summary>
        ///<param name="source"></param>
        ///<param name="generator"></param>
        public ConverterAdaptorVcgen(IVertexSource source, IGenerator generator)
        {
            _markers = new NullMarkers();
            _source = source;
            _generator = generator;
            _status = EStatus.Initial;
        }

        ///<summary>
        ///</summary>
        ///<param name="source"></param>
        ///<param name="generator"></param>
        ///<param name="markers"></param>
        public ConverterAdaptorVcgen(IVertexSource source, IGenerator generator, IMarkers markers)
            : this(source, generator)
        {
            _markers = markers;
        }
        void Attach(IVertexSource source) { _source = source; }

        protected IGenerator Generator
        {
            get { return _generator; }
        }

        private IMarkers Markers
        {
            get { return _markers; }
        }

        ///<summary>
        ///</summary>
        ///<param name="pathId"></param>
        public void Rewind(uint pathId) 
        { 
            _source.Rewind(pathId);
            _status = EStatus.Initial;
        }

        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<returns></returns>
        public uint Vertex(out double x, out double y)
        {
            x = 0;
            y = 0;
            uint cmd = (uint)Path.EPathCommands.Stop;
            bool done = false;
            while(!done)
            {
                switch(_status)
                {
                    case EStatus.Initial:
                        Initial();
                        goto case EStatus.Accumulate;

                case EStatus.Accumulate:
                        if (Path.IsStop(_lastCommand)) return (uint)Path.EPathCommands.Stop;
                        Accumulate(ref cmd, ref x, ref y);
                        goto case EStatus.Generate;

                case EStatus.Generate:
                        cmd = _generator.Vertex(ref x, ref y);
                        if (Path.IsStop(cmd))
                        {
                            _status = EStatus.Accumulate;
                            break;
                        }
                        done = true;
                    break;
                }
            }
            return cmd;
        }

        private void Initial()
        {
            _markers.RemoveAll();
            _lastCommand = _source.Vertex(out _startX, out _startY);
            _status = EStatus.Accumulate;
        }

        private void Accumulate(ref uint cmd, ref double x, ref double y)
        {
            _generator.RemoveAll();
            _generator.AddVertex(_startX, _startY, (uint)Path.EPathCommands.MoveTo);
            _markers.AddVertex(_startX, _startY, (uint)Path.EPathCommands.MoveTo);

            for (; ; )
            {
                cmd = _source.Vertex(out x, out y);
                if (Path.IsVertex(cmd))
                {
                    _lastCommand = cmd;
                    if (Path.IsMoveTo(cmd))
                    {
                        _startX = x;
                        _startY = y;
                        break;
                    }
                    _generator.AddVertex(x, y, cmd);
                    _markers.AddVertex(x, y, (uint)Path.EPathCommands.LineTo);
                }
                else
                {
                    if (Path.IsStop(cmd))
                    {
                        _lastCommand = (uint)Path.EPathCommands.Stop;
                        break;
                    }
                    if (Path.IsEndPoly(cmd))
                    {
                        _generator.AddVertex(x, y, cmd);
                        break;
                    }
                }
            }
            _generator.Rewind(0);
            _status = EStatus.Generate;
        }
    };
}