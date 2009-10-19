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
    public interface IGenerator
    {
        void RemoveAll();
        void AddVertex(double x, double y, uint unknown);
        void Rewind(uint path_id);
        uint Vertex(ref double x, ref double y);

        MathStroke.ELineCap LineCap();
        MathStroke.ELineJoin LineJoin();
        MathStroke.EInnerJoin InnerJoin();

        void LineCap(MathStroke.ELineCap lc);
        void LineJoin(MathStroke.ELineJoin lj);
        void InnerJoin(MathStroke.EInnerJoin ij);

        void Width(double w);
        void MiterLimit(double ml);
        void MiterLimitTheta(double t);
        void InnerMiterLimit(double ml);
        void ApproximationScale(double approxScale);

        double Width();
        double MiterLimit();
        double InnerMiterLimit();
        double ApproximationScale();

        void Shorten(double s);
        double Shorten();
    };
}
