//----------------------------------------------------------------------------
// Anti-Grain Geometry - Version 2.4
// Copyright (C) 2002-2005 Maxim Shemanarev (http://www.antigrain.com)
//
// C# Port port by: Lars Brubaker
//                  larsbrubaker@gmail.com
// Copyright (C) 2007
//
// Permission to copy, use, modify, sell and distribute this software 
// is granted provided this copyright notice appears in all copies. 
// This software is provided "as is" without express or implied
// warranty, and with no claim as to its suitability for any purpose.
//
//----------------------------------------------------------------------------
// Contact: mcseem@antigrain.com
//          mcseemPictor@yahoo.com
//          http://www.antigrain.com
//----------------------------------------------------------------------------
//
// class platform_support
// 
//----------------------------------------------------------------------------
//#define USE_OPENGL

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Pictor.UI.EmulatorPlatform
{

    //------------------------------------------------------------------------
    public class PlatformSpecificWindow : Form
    {
        public PlatformSupportAbstract.PixelFormats m_format;
        public PlatformSupportAbstract.PixelFormats m_sys_format;
        private PlatformSupportAbstract.ERenderOrigin m_RenderOrigin;
        //private HWND          m_hwnd;
        internal PixelMap m_pmap_window;
        internal PixelMap[] m_pmap_img = new PixelMap[(int)PlatformSupportAbstract.max_images_e.max_images];
        private uint[] m_keymap = new uint[256];
        private uint m_last_translated_key;
        //private int           m_cur_x;
        //private int           m_cur_y;
        //private uint      m_input_flags;
        public bool m_WindowContentNeedsRedraw;
        public bool fullscreen;
        internal System.Diagnostics.Stopwatch m_StopWatch;
        PlatformSupportAbstract m_app;

        public PlatformSpecificWindow(PlatformSupportAbstract app, PlatformSupportAbstract.PixelFormats format, PlatformSupportAbstract.ERenderOrigin RenderOrigin)
        {
            m_app = app;
            m_format = format;
            m_sys_format = PlatformSupportAbstract.PixelFormats.Undefined;
            m_RenderOrigin = RenderOrigin;
            m_WindowContentNeedsRedraw = true;
            m_StopWatch = new Stopwatch();

            switch (m_format)
            {
                case PlatformSupportAbstract.PixelFormats.BlackWhite:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.BlackWhite;
                    break;

                case PlatformSupportAbstract.PixelFormats.Gray8:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Gray8;
                    break;

                case PlatformSupportAbstract.PixelFormats.Gray16:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Gray16;
                    break;

                case PlatformSupportAbstract.PixelFormats.Rgb565:
                case PlatformSupportAbstract.PixelFormats.Rgb555:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Rgb555;
                    break;

                case PlatformSupportAbstract.PixelFormats.RgbAAA:
                case PlatformSupportAbstract.PixelFormats.BgrAAA:
                case PlatformSupportAbstract.PixelFormats.RgbBBA:
                case PlatformSupportAbstract.PixelFormats.BgrABB:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Bgr24;
                    break;

                case PlatformSupportAbstract.PixelFormats.Rgb24:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Rgb24;
                    break;

                case PlatformSupportAbstract.PixelFormats.Bgr24:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Bgr24;
                    break;

                case PlatformSupportAbstract.PixelFormats.Rgb48:
                case PlatformSupportAbstract.PixelFormats.Bgr48:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Bgr24;
                    break;

                case PlatformSupportAbstract.PixelFormats.Bgra32:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Bgra32;
                    break;
                case PlatformSupportAbstract.PixelFormats.Abgr32:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Abgr32;
                    break;
                case PlatformSupportAbstract.PixelFormats.Argb32:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Argb32;
                    break;
                case PlatformSupportAbstract.PixelFormats.Rgba32:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Rgba32;
                    break;

                case PlatformSupportAbstract.PixelFormats.Bgra64:
                case PlatformSupportAbstract.PixelFormats.Abgr64:
                case PlatformSupportAbstract.PixelFormats.Argb64:
                case PlatformSupportAbstract.PixelFormats.Rgba64:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Bgra32;
                    break;
            }

            m_StopWatch.Reset();
            m_StopWatch.Start();
        }

        protected override void OnKeyUp(System.Windows.Forms.KeyEventArgs windowsKeyEvent)
        {
            Pictor.UI.KeyEventArgs PictorKeyEvent = new Pictor.UI.KeyEventArgs((Pictor.UI.Keys)windowsKeyEvent.KeyData);
            m_app.OnKeyUp(PictorKeyEvent);

            windowsKeyEvent.Handled = PictorKeyEvent.Handled;
            windowsKeyEvent.SuppressKeyPress = PictorKeyEvent.SuppressKeyPress;

            base.OnKeyUp(windowsKeyEvent);
        }

        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs windowsKeyEvent)
        {
            Pictor.UI.KeyEventArgs PictorKeyEvent = new Pictor.UI.KeyEventArgs((Pictor.UI.Keys)windowsKeyEvent.KeyData);
            m_app.OnKeyDown(PictorKeyEvent);

            if (PictorKeyEvent.Handled)
            {
                m_app.OnControlChanged();
                m_app.ForceRedraw();
            }

            windowsKeyEvent.Handled = PictorKeyEvent.Handled;
            windowsKeyEvent.SuppressKeyPress = PictorKeyEvent.SuppressKeyPress;

            base.OnKeyDown(windowsKeyEvent);
        }

        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs windowsKeyPressEvent)
        {
            Pictor.UI.KeyPressEventArgs PictorKeyPressEvent = new Pictor.UI.KeyPressEventArgs(windowsKeyPressEvent.KeyChar);
            m_app.OnKeyPress(PictorKeyPressEvent);
            windowsKeyPressEvent.Handled = PictorKeyPressEvent.Handled;

            base.OnKeyPress(windowsKeyPressEvent);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (m_WindowContentNeedsRedraw)
            {
                m_app.OnDraw();
                m_WindowContentNeedsRedraw = false;
            }

            display_pmap(e.Graphics, m_app.RenderBufferWindow);
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs mouseEvent)
        {
            base.OnMouseDown(mouseEvent);

            int Y = mouseEvent.Y;
            if (m_app.FlipY())
            {
                Y = (int)m_app.RenderBufferWindow.Height() - Y;
            }
            Pictor.UI.MouseEventArgs PictorMouseEvent = new Pictor.UI.MouseEventArgs((Pictor.UI.MouseButtons)mouseEvent.Button, mouseEvent.Clicks, mouseEvent.X, Y, mouseEvent.Delta);

            m_app.SetChildCurrent(PictorMouseEvent.X, PictorMouseEvent.Y);
            m_app.OnMouseDown(PictorMouseEvent);
            if (PictorMouseEvent.Handled)
            {
                m_app.OnControlChanged();
                m_app.ForceRedraw();
            }
            else
            {
                if (m_app.InRect(PictorMouseEvent.X, PictorMouseEvent.Y))
                {
                    if (m_app.SetChildCurrent(PictorMouseEvent.X, PictorMouseEvent.Y))
                    {
                        m_app.OnControlChanged();
                        m_app.ForceRedraw();
                    }
                }
                else
                {
                    m_app.OnMouseDown(PictorMouseEvent);
                }
            }
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs mouseEvent)
        {
            base.OnMouseMove(mouseEvent);

            int Y = mouseEvent.Y;
            if (m_app.FlipY())
            {
                Y = (int)m_app.RenderBufferWindow.Height() - Y;
            }
            Pictor.UI.MouseEventArgs PictorMouseEvent = new Pictor.UI.MouseEventArgs((Pictor.UI.MouseButtons)mouseEvent.Button, mouseEvent.Clicks, mouseEvent.X, Y, mouseEvent.Delta);

            m_app.OnMouseMove(PictorMouseEvent);
            if (PictorMouseEvent.Handled)
            {
                m_app.OnControlChanged();
                m_app.ForceRedraw();
            }
            else
            {
                if (!m_app.InRect(PictorMouseEvent.X, PictorMouseEvent.Y))
                {
                    m_app.OnMouseMove(PictorMouseEvent);
                }
            }
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs mouseEvent)
        {
            base.OnMouseUp(mouseEvent);

            int Y = mouseEvent.Y;
            if (m_app.FlipY())
            {
                Y = (int)m_app.RenderBufferWindow.Height() - Y;
            }
            Pictor.UI.MouseEventArgs PictorMouseEvent = new Pictor.UI.MouseEventArgs((Pictor.UI.MouseButtons)mouseEvent.Button, mouseEvent.Clicks, mouseEvent.X, Y, mouseEvent.Delta);

            m_app.OnMouseUp(PictorMouseEvent);
            if (PictorMouseEvent.Handled)
            {
                m_app.OnControlChanged();
                m_app.ForceRedraw();
            }
            m_app.OnMouseUp(PictorMouseEvent);
        }

        public unsafe void create_pmap(uint width, uint height, RasterBuffer wnd)
        {
            m_pmap_window = new PixelMap();
            m_pmap_window.Create(width, height, PlatformSupportAbstract.GetBitDepthForPixelFormat(m_format));
            wnd.Attach(m_pmap_window.buf(),
                m_pmap_window.Width(),
                m_pmap_window.Height(),
                m_RenderOrigin == PlatformSupportAbstract.ERenderOrigin.OriginBottomLeft ? -m_pmap_window.stride() : m_pmap_window.stride(),
                m_pmap_window.bpp());
        }

        static void convert_pmap(RasterBuffer dst,
                                 RasterBuffer src,
                                 PlatformSupportAbstract.PixelFormats format)
        {
            
        }

        public void display_pmap(Graphics displayGraphics, RasterBuffer src)
        {
            if (m_sys_format == m_format)
            {
                m_pmap_window.Draw(displayGraphics);
            }
            else
            {
                PixelMap pmap_tmp = new PixelMap();
                pmap_tmp.Create(m_pmap_window.Width(),
                    m_pmap_window.Height(), PlatformSupportAbstract.GetBitDepthForPixelFormat(m_sys_format));

                RasterBuffer rbuf_tmp = new RasterBuffer();
                unsafe
                {
                    rbuf_tmp.Attach(pmap_tmp.buf(),
                        pmap_tmp.Width(),
                        pmap_tmp.Height(),
                        m_app.FlipY() ?
                        pmap_tmp.stride() :
                        -pmap_tmp.stride(), pmap_tmp.bpp());
                }

                convert_pmap(rbuf_tmp, src, m_format);
                pmap_tmp.Draw(displayGraphics);

                throw new System.NotImplementedException();
            }
        }

        public bool load_pmap(string fn, uint idx, RasterBuffer dst)
        {
            PixelMap pmap_tmp = new PixelMap();
            if (!pmap_tmp.load_from_bmp(fn)) return false;

            RasterBuffer rbuf_tmp = new RasterBuffer();
            unsafe
            {
                rbuf_tmp.Attach(pmap_tmp.buf(),
                                pmap_tmp.Width(),
                                pmap_tmp.Height(),
                                m_app.FlipY() ?
                                  pmap_tmp.stride() :
                                 -pmap_tmp.stride(),
                                 pmap_tmp.bpp());

                m_pmap_img[idx] = new PixelMap();
                m_pmap_img[idx].Create(pmap_tmp.Width(), pmap_tmp.Height(),
                    PlatformSupportAbstract.GetBitDepthForPixelFormat(m_format),
                    0);

                dst.Attach(m_pmap_img[idx].buf(),
                    m_pmap_img[idx].Width(),
                    m_pmap_img[idx].Height(),
                    m_app.FlipY() ?
                    m_pmap_img[idx].stride() :
                    -m_pmap_img[idx].stride(), m_pmap_img[idx].bpp());

                switch (m_format)
                {
                   
                    case PlatformSupportAbstract.PixelFormats.Bgr24:
                        switch (pmap_tmp.bpp())
                        {
                            case 24:
                                unsafe
                                {
                                    for (uint y = 0; y < rbuf_tmp.Height(); y++)
                                    {
                                        byte* sourceBuffer = rbuf_tmp.GetPixelPointer((int)rbuf_tmp.Height() - (int)y - 1);
                                        byte* destBuffer = dst.GetPixelPointer((int)y);
                                        for (uint x = 0; x < rbuf_tmp.Width(); x++)
                                        {
                                            *destBuffer++ = sourceBuffer[0];
                                            *destBuffer++ = sourceBuffer[1];
                                            *destBuffer++ = sourceBuffer[2];
                                            sourceBuffer += 3;
                                        }
                                    }
                                }
                                break;

                            default:
                                throw new System.NotImplementedException();
                        }
                        break;

                    case PlatformSupportAbstract.PixelFormats.Rgb24:
                        switch (pmap_tmp.bpp())
                        {
                            //                 case 16: color_conv(dst, &rbuf_tmp, color_conv_rgb555_to_rgb24()); break;
                            case 24:
                                //color_conv(dst, &rbuf_tmp, color_conv_bgr24_to_rgba32()); 
                                unsafe
                                {
                                    for (uint y = 0; y < rbuf_tmp.Height(); y++)
                                    {
                                        byte* sourceBuffer = rbuf_tmp.GetPixelPointer((int)rbuf_tmp.Height() - (int)y - 1);
                                        byte* destBuffer = dst.GetPixelPointer((int)y);
                                        for (uint x = 0; x < rbuf_tmp.Width(); x++)
                                        {
                                            *destBuffer++ = sourceBuffer[2];
                                            *destBuffer++ = sourceBuffer[1];
                                            *destBuffer++ = sourceBuffer[0];
                                            sourceBuffer += 3;
                                        }
                                    }
                                }
                                break;
                            //                 case 32: color_conv(dst, &rbuf_tmp, color_conv_bgra32_to_rgb24()); break;
                            default:
                                throw new System.NotImplementedException();
                        }
                        break;
                    

                    case PlatformSupportAbstract.PixelFormats.Bgra32:
                        switch (pmap_tmp.bpp())
                        {
                            case 24:
                                unsafe
                                {
                                    for (uint y = 0; y < rbuf_tmp.Height(); y++)
                                    {
                                        byte* sourceBuffer = rbuf_tmp.GetPixelPointer((int)rbuf_tmp.Height() - (int)y - 1);
                                        byte* destBuffer = dst.GetPixelPointer((int)y);
                                        for (uint x = 0; x < rbuf_tmp.Width(); x++)
                                        {
                                            *destBuffer++ = sourceBuffer[0];
                                            *destBuffer++ = sourceBuffer[1];
                                            *destBuffer++ = sourceBuffer[2];
                                            *destBuffer++ = 255;
                                            sourceBuffer += 3;
                                        }
                                    }
                                }
                                break;
                        }
                        break;

                    case PlatformSupportAbstract.PixelFormats.Rgba32:
                        switch (pmap_tmp.bpp())
                        {
                            //                 case 16: color_conv(dst, &rbuf_tmp, color_conv_rgb555_to_rgba32()); break;
                            case 24:
                                //color_conv(dst, &rbuf_tmp, color_conv_bgr24_to_rgba32()); 
                                unsafe
                                {
                                    for (uint y = 0; y < rbuf_tmp.Height(); y++)
                                    {
                                        byte* sourceBuffer = rbuf_tmp.GetPixelPointer((int)rbuf_tmp.Height() - (int)y - 1);
                                        byte* destBuffer = dst.GetPixelPointer((int)y);
                                        for (uint x = 0; x < rbuf_tmp.Width(); x++)
                                        {
                                            *destBuffer++ = sourceBuffer[2];
                                            *destBuffer++ = sourceBuffer[1];
                                            *destBuffer++ = sourceBuffer[0];
                                            *destBuffer++ = 255;
                                            sourceBuffer += 3;
                                        }
                                    }
                                }
                                break;


                            //                 case 32: color_conv(dst, &rbuf_tmp, color_conv_bgra32_to_rgba32()); break;
                            default:
                                throw new System.NotImplementedException();
                        }
                        break;

                    

                    default:
                        throw new System.NotImplementedException();

                }
            }

            return true;
        }

        public bool save_pmap(string fn, uint idx, RasterBuffer src)
        {
            
            return true;
        }


        public uint translate(uint keycode)
        {
            return m_last_translated_key = (keycode > 255) ? 0 : m_keymap[keycode];
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (this.Visible)
            {
                if (m_app.RenderBufferWindow != null)
                {
                    create_pmap((uint)ClientSize.Width, (uint)ClientSize.Height, m_app.RenderBufferWindow);
                }

                m_app.TransAffineResizing(ClientSize.Width, ClientSize.Height);
                m_app.OnResize(ClientSize.Width, ClientSize.Height);
                m_app.ForceRedraw();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            // TODO: call on closing and check if we can close ("do you want to save might cancel." :).
            //OnClosing();
            m_app.OnClosed();

            base.OnClosed(e);
        }

        internal void ForceRedraw()
        {
            m_WindowContentNeedsRedraw = true;
            Invalidate();
        }
    };

}
