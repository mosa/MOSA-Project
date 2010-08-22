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
	//-------------------------------------------------------------StrokeConverter
	public sealed class StrokeConverter : ConverterAdaptorVcgen, IVertexSource
	{
		public StrokeConverter(IVertexSource vs)
			: base(vs, new VcGenStroke())
		{
		}

		public void LineCap(MathStroke.ELineCap lc) { base.Generator.LineCap(lc); }
		public void LineJoin(MathStroke.ELineJoin lj) { base.Generator.LineJoin(lj); }
		public void InnerJoin(MathStroke.EInnerJoin ij) { base.Generator.InnerJoin(ij); }

		public MathStroke.ELineCap LineCap() { return base.Generator.LineCap(); }
		public MathStroke.ELineJoin LineJoin() { return base.Generator.LineJoin(); }
		public MathStroke.EInnerJoin InnerJoin() { return base.Generator.InnerJoin(); }

		public void Width(double w) { base.Generator.Width(w); }
		public void MiterLimit(double ml) { base.Generator.MiterLimit(ml); }
		public void MiterLimitTheta(double t) { base.Generator.MiterLimitTheta(t); }
		public void InnerMiterLimit(double ml) { base.Generator.InnerMiterLimit(ml); }
		public void ApproximationScale(double approxScale) { base.Generator.ApproximationScale(approxScale); }

		public double Width() { return base.Generator.Width(); }
		public double MiterLimit() { return base.Generator.MiterLimit(); }
		public double InnerMiterLimit() { return base.Generator.InnerMiterLimit(); }
		public double ApproximationScale() { return base.Generator.ApproximationScale(); }

		public void Shorten(double s) { base.Generator.Shorten(s); }
		public double Shorten() { return base.Generator.Shorten(); }
	};
}