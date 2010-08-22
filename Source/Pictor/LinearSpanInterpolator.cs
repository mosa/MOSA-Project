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

	public interface ISpanInterpolator
	{
		void Begin(double x, double y, uint len);
		void Coordinates(out int x, out int y);
		void Next();

		Transform.ITransform Transformer
		{
			get;
			set;
		}
		void ReSynchronize(double xe, double ye, uint len);
		void LocalScale(out int x, out int y);
	};

	//================================================LinearSpanInterpolator
	public sealed class LinearSpanInterpolator : ISpanInterpolator
	{
		private Transform.ITransform m_trans;
		private Dda2LineInterpolator m_li_x;
		private Dda2LineInterpolator m_li_y;

		public enum ESubpixelScale
		{
			SubpixelShift = 8,
			subpixel_shift = SubpixelShift,
			Scale = 1 << subpixel_shift
		};

		//--------------------------------------------------------------------
		public LinearSpanInterpolator() { }
		public LinearSpanInterpolator(Transform.ITransform trans)
		{
			m_trans = trans;
		}

		public LinearSpanInterpolator(Transform.ITransform trans, double x, double y, uint len)
		{
			m_trans = trans;
			Begin(x, y, len);
		}

		//----------------------------------------------------------------
		public Transform.ITransform Transformer
		{
			get
			{
				return m_trans;
			}
			set
			{
				m_trans = value;
			}
		}

		public void LocalScale(out int x, out int y)
		{
			throw new System.NotImplementedException();
		}

		//----------------------------------------------------------------
		public void Begin(double x, double y, uint len)
		{
			double tx;
			double ty;

			tx = x;
			ty = y;
			m_trans.Transform(ref tx, ref ty);
			int x1 = Basics.Round(tx * (double)ESubpixelScale.Scale);
			int y1 = Basics.Round(ty * (double)ESubpixelScale.Scale);

			tx = x + len;
			ty = y;
			m_trans.Transform(ref tx, ref ty);
			int x2 = Basics.Round(tx * (double)ESubpixelScale.Scale);
			int y2 = Basics.Round(ty * (double)ESubpixelScale.Scale);

			m_li_x = new Dda2LineInterpolator(x1, x2, (int)len);
			m_li_y = new Dda2LineInterpolator(y1, y2, (int)len);
		}

		//----------------------------------------------------------------
		public void ReSynchronize(double xe, double ye, uint len)
		{
			m_trans.Transform(ref xe, ref ye);
			m_li_x = new Dda2LineInterpolator(m_li_x.y(), Basics.Round(xe * (double)ESubpixelScale.Scale), (int)len);
			m_li_y = new Dda2LineInterpolator(m_li_y.y(), Basics.Round(ye * (double)ESubpixelScale.Scale), (int)len);
		}

		//----------------------------------------------------------------
		//public void operator++()
		public void Next()
		{
			m_li_x.Next();
			m_li_y.Next();
		}

		//----------------------------------------------------------------
		public void Coordinates(out int x, out int y)
		{
			x = m_li_x.y();
			y = m_li_y.y();
		}
	};

	/*
		//=====================================span_interpolator_linear_subdiv
		template<class Transformer = ITransformer, uint SubpixelShift = 8> 
		class span_interpolator_linear_subdiv
		{
		public:
			typedef Transformer trans_type;

			enum ESubpixelScale
			{
				subpixel_shift = SubpixelShift,
				Scale = 1 << subpixel_shift
			};


			//----------------------------------------------------------------
			span_interpolator_linear_subdiv() :
				m_subdiv_shift(4),
				m_subdiv_size(1 << m_subdiv_shift),
				m_subdiv_mask(m_subdiv_size - 1) {}

			span_interpolator_linear_subdiv(const trans_type& trans, 
											uint SubdivisionShift = 4) : 
				m_subdiv_shift(SubdivisionShift),
				m_subdiv_size(1 << m_subdiv_shift),
				m_subdiv_mask(m_subdiv_size - 1),
				m_trans(&trans) {}

			span_interpolator_linear_subdiv(const trans_type& trans,
											double x, double y, uint len,
											uint SubdivisionShift = 4) :
				m_subdiv_shift(SubdivisionShift),
				m_subdiv_size(1 << m_subdiv_shift),
				m_subdiv_mask(m_subdiv_size - 1),
				m_trans(&trans)
			{
				Begin(x, y, len);
			}

			//----------------------------------------------------------------
			const trans_type& Transformer() const { return *m_trans; }
			void Transformer(const trans_type& trans) { m_trans = &trans; }

			//----------------------------------------------------------------
			uint SubdivisionShift() const { return m_subdiv_shift; }
			void SubdivisionShift(uint shift) 
			{
				m_subdiv_shift = shift;
				m_subdiv_size = 1 << m_subdiv_shift;
				m_subdiv_mask = m_subdiv_size - 1;
			}

			//----------------------------------------------------------------
			void Begin(double x, double y, uint len)
			{
				double tx;
				double ty;
				m_pos   = 1;
				m_src_x = Round(x * Scale) + Scale;
				m_src_y = y;
				m_len   = len;

				if(len > m_subdiv_size) len = m_subdiv_size;
				tx = x;
				ty = y;
				m_trans->Transform(&tx, &ty);
				int x1 = Round(tx * Scale);
				int y1 = Round(ty * Scale);

				tx = x + len;
				ty = y;
				m_trans->Transform(&tx, &ty);

				m_li_x = Dda2LineInterpolator(x1, Round(tx * Scale), len);
				m_li_y = Dda2LineInterpolator(y1, Round(ty * Scale), len);
			}

			//----------------------------------------------------------------
			void operator++()
			{
				++m_li_x;
				++m_li_y;
				if(m_pos >= m_subdiv_size)
				{
					uint len = m_len;
					if(len > m_subdiv_size) len = m_subdiv_size;
					double tx = double(m_src_x) / double(Scale) + len;
					double ty = m_src_y;
					m_trans->Transform(&tx, &ty);
					m_li_x = Dda2LineInterpolator(m_li_x.y(), Round(tx * Scale), len);
					m_li_y = Dda2LineInterpolator(m_li_y.y(), Round(ty * Scale), len);
					m_pos = 0;
				}
				m_src_x += Scale;
				++m_pos;
				--m_len;
			}

			//----------------------------------------------------------------
			void Coordinates(int* x, int* y) const
			{
				*x = m_li_x.y();
				*y = m_li_y.y();
			}

		private:
			uint m_subdiv_shift;
			uint m_subdiv_size;
			uint m_subdiv_mask;
			const trans_type* m_trans;
			Dda2LineInterpolator m_li_x;
			Dda2LineInterpolator m_li_y;
			int      m_src_x;
			double   m_src_y;
			uint m_pos;
			uint m_len;
		};

	 */
}