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

	//===================================================================Gray8
	public struct Gray8
	{
		const int base_shift = 8;
		const uint base_scale = (uint)(1 << base_shift);
		const uint base_mask = base_scale - 1;

		byte v;
		byte a;

		//--------------------------------------------------------------------
		public Gray8(uint v_)
			: this(v_, (uint)base_mask)
		{

		}

		public Gray8(uint v_, uint a_)
		{
			v = (byte)(v_);
			a = (byte)(a_);
		}

		//--------------------------------------------------------------------
		Gray8(Gray8 c, uint a_)
		{
			v = (c.v);
			a = (byte)(a_);
		}

		//--------------------------------------------------------------------
		public Gray8(RGBA_Doubles c)
		{
			v = ((byte)Basics.UnsignedRound((0.299 * c.R_Byte + 0.587 * c.G_Byte + 0.114 * c.B_Byte) * (double)(base_mask)));
			a = ((byte)Basics.UnsignedRound(c.A_Byte * (double)(base_mask)));
		}


		//--------------------------------------------------------------------
		public Gray8(RGBA_Doubles c, double a_)
		{
			v = ((byte)Basics.UnsignedRound((0.299 * c.R_Byte + 0.587 * c.G_Byte + 0.114 * c.B_Byte) * (double)(base_mask)));
			a = ((byte)Basics.UnsignedRound(a_ * (double)(base_mask)));
		}

		//--------------------------------------------------------------------
		public Gray8(RGBA_Bytes c)
		{
			v = (byte)((c.R_Byte * 77 + c.G_Byte * 150 + c.B_Byte * 29) >> 8);
			a = (byte)(c.A_Byte);
		}

		//--------------------------------------------------------------------
		public Gray8(RGBA_Bytes c, uint a_)
		{
			v = (byte)((c.R_Byte * 77 + c.G_Byte * 150 + c.B_Byte * 29) >> 8);
			a = (byte)(a_);
		}

		//--------------------------------------------------------------------
		public void Clear()
		{
			v = a = 0;
		}

		//--------------------------------------------------------------------
		public Gray8 Transparent()
		{
			a = 0;
			return this;
		}
		//--------------------------------------------------------------------
		public double Opacity
		{
			get { return (double)(a) / (double)(base_mask); }
			set
			{
				if (value < 0.0) value = 0.0;
				if (value > 1.0) value = 1.0;
				a = (byte)Basics.UnsignedRound(value * (double)(base_mask));
			}
		}


		//--------------------------------------------------------------------
		public Gray8 PreMultiply()
		{
			if (a == (byte)base_mask) return this;
			if (a == 0)
			{
				v = 0;
				return this;
			}
			v = (byte)(((uint)(v) * a) >> base_shift);
			return this;
		}

		//--------------------------------------------------------------------
		public Gray8 PreMultiply(uint a_)
		{
			if (a == (int)base_mask && a_ >= (int)base_mask) return this;
			if (a == 0 || a_ == 0)
			{
				v = a = 0;
				return this;
			}
			uint v_ = ((uint)(v) * a_) / a;
			v = (byte)((v_ > a_) ? a_ : v_);
			a = (byte)(a_);
			return this;
		}

		//--------------------------------------------------------------------
		public Gray8 DeMultiply()
		{
			if (a == (int)base_mask) return this;
			if (a == 0)
			{
				v = 0;
				return this;
			}
			uint v_ = ((uint)(v) * (int)base_mask) / a;
			v = (byte)((v_ > (int)base_mask) ? (byte)base_mask : v_);
			return this;
		}

		//--------------------------------------------------------------------
		public Gray8 Gradient(Gray8 c, double k)
		{
			Gray8 ret;
			uint ik = Basics.UnsignedRound(k * (int)base_scale);
			ret.v = (byte)((uint)(v) + ((((uint)(c.v) - v) * ik) >> base_shift));
			ret.a = (byte)((uint)(a) + ((((uint)(c.a) - a) * ik) >> base_shift));
			return ret;
		}

		/*
		//--------------------------------------------------------------------
		void Add(Gray8 c, uint cover)
		{
			uint cv, ca;
			if(cover == cover_mask)
			{
				if (c.a == BaseMask) 
				{
					*this = c;
				}
				else
				{
					cv = v + c.v; v = (cv > (uint)(BaseMask)) ? (uint)(BaseMask) : cv;
					ca = a + c.a; a = (ca > (uint)(BaseMask)) ? (uint)(BaseMask) : ca;
				}
			}
			else
			{
				cv = v + ((c.v * cover + cover_mask/2) >> CoverShift);
				ca = a + ((c.a * cover + cover_mask/2) >> CoverShift);
				v = (cv > (uint)(BaseMask)) ? (uint)(BaseMask) : cv;
				a = (ca > (uint)(BaseMask)) ? (uint)(BaseMask) : ca;
			}
		}
		 */

		//--------------------------------------------------------------------
		//static Gray8 no_color() { return Gray8(0,0); }

		/*
		static Gray8 gray8_pre(uint v, uint a = Gray8.BaseMask)
		{
			return Gray8(v,a).PreMultiply();
		}

		static Gray8 gray8_pre(Gray8 c, uint a)
		{
			return Gray8(c,a).PreMultiply();
		}

		static Gray8 gray8_pre(rgba& c)
		{
			return Gray8(c).PreMultiply();
		}

		static Gray8 gray8_pre(rgba& c, double a)
		{
			return Gray8(c,a).PreMultiply();
		}

		static Gray8 gray8_pre(rgba8& c)
		{
			return Gray8(c).PreMultiply();
		}

		static Gray8 gray8_pre(rgba8& c, uint a)
		{
			return Gray8(c,a).PreMultiply();
		}
		 */
	};
}
