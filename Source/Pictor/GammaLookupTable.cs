/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */


//#ifndef AGG_GAMMA_LUT_INCLUDED
//#define AGG_GAMMA_LUT_INCLUDED

//#include <math.h>
//#include "Basics.h"

using System;

namespace Pictor
{
    //template<class LoResT=int8u, class HiResT=int8u, unsigned GammaShift=8, unsigned HiResShift=8> 
    public class GammaLookupTable
    {
        private double m_gamma;
        private byte[] m_dir_gamma;
        private byte[] m_inv_gamma;

        enum GammaScale
        {
            Shift = 8,
            Size  = 1 << Shift,
            Mask  = Size - 1
        };

        public GammaLookupTable()
        {
            m_gamma = (1.0);
            m_dir_gamma = new byte[(int)GammaScale.Size];
            m_inv_gamma = new byte[(int)GammaScale.Size];
        }

        public GammaLookupTable(double g)
        {
            m_gamma = g;
            m_dir_gamma = new byte[(int)GammaScale.Size];
            m_inv_gamma = new byte[(int)GammaScale.Size];
            Gamma = m_gamma;
        }

        public double Gamma
        {
            get
            {
                return m_gamma;
            }
            set
            {
                m_gamma = value;

                for (uint i = 0; i < (uint)GammaScale.Size; i++)
                {
                    m_dir_gamma[i] = (byte)Basics.UnsignedRound(Math.Pow(i / (double)GammaScale.Mask, m_gamma) * (double)GammaScale.Mask);
                }

                double inv_g = 1.0 / value;
                for (uint i = 0; i < (uint)GammaScale.Size; i++)
                {
                    m_inv_gamma[i] = (byte)Basics.UnsignedRound(Math.Pow(i / (double)GammaScale.Mask, inv_g) * (double)GammaScale.Mask);
                }
            }
        }

        public byte Dir(byte v) 
        { 
            return m_dir_gamma[v]; 
        }

        public byte Inv(byte v) 
        { 
            return m_inv_gamma[v];
        }
    };
}

