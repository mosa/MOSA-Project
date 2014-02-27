/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Emulator
{
	/// <summary>
	///
	/// </summary>
	public class EmulatorDemo : Pictor.UI.EmulatorPlatform.PlatformSupport
	{
		private double[] m_x = new double[2];
		private double[] m_y = new double[2];
		private double m_dx;
		private double m_dy;
		private int m_idx;
		private Pictor.UI.ButtonWidget button;
		private Pictor.UI.ButtonWidget white;
		private Pictor.UI.ButtonWidget blue;
		private Pictor.UI.ButtonWidget reset;
		private Pictor.RGBA_Bytes background;
		private Pictor.UI.CheckBoxWidget border;
		private Pictor.UI.SliderWidget alpha;
		private Pictor.UI.SliderWidget radius;

		private Pictor.UI.Dialogs.YesNoDialog dialog;

		public EmulatorDemo(PixelFormats format, Pictor.UI.PlatformSupportAbstract.ERenderOrigin RenderOrigin)
			: base(format, RenderOrigin)
		{
			m_idx = (-1);
			background = new Pictor.RGBA_Bytes(127, 127, 127);
			m_x[0] = 100; m_y[0] = 100;
			m_x[1] = 500; m_y[1] = 350;
			button = Pictor.UI.UIManager.CreateButton(8.0, 130.0, "Quit", 8, 1, 1, 3);
			button.ButtonClick += Quit;
			AddChild(button);

			white = Pictor.UI.UIManager.CreateButton(8.0, 110.0, "Gray", 8, 1, 1, 3);
			blue = Pictor.UI.UIManager.CreateButton(8.0, 90.0, "Blue", 8, 1, 1, 3);
			reset = Pictor.UI.UIManager.CreateButton(8.0, 70.0, "Reset", 8, 1, 1, 3);
			border = new Pictor.UI.CheckBoxWidget(8.0, 50.0, "Draw as outline");
			alpha = new Pictor.UI.SliderWidget(10, 10, 590, 19);
			radius = new Pictor.UI.SliderWidget(10, 30, 590, 39);
			alpha.Range(0, 255);
			alpha.Value(255);

			//alpha.BackgroundColor(new Pictor.RGBA_Doubles(255, 255, 255));

			radius.Label("Radius = {0:F3}");
			radius.Range(0.0, 50.0);
			radius.Value(25.0);

			border.Status(false);
			white.ButtonClick += White;
			blue.ButtonClick += Blue;
			reset.ButtonClick += Reset;

			AddChild(white);
			AddChild(blue);
			AddChild(reset);
			AddChild(border);
			AddChild(alpha);
			AddChild(radius);
		}

		public void Quit(Pictor.UI.ButtonWidget button)
		{
			dialog = new Pictor.UI.Dialogs.YesNoDialog(150, 200, "Close Application?");
			dialog.Show(this);
		}

		public void White(Pictor.UI.ButtonWidget button)
		{
			background = new Pictor.RGBA_Bytes(127, 127, 127);
		}

		public void Blue(Pictor.UI.ButtonWidget button)
		{
			background = new Pictor.RGBA_Bytes(117, 115, 217);
		}

		public void Reset(Pictor.UI.ButtonWidget button)
		{
			m_x[0] = 100; m_y[0] = 100;
			m_x[1] = 500; m_y[1] = 350;
		}

		public override void OnDraw()
		{
			background.A_Byte = (uint)alpha.Value();
			alpha.Label("Opacity: " + ((uint)((alpha.Value() / 255.0) * 100)).ToString() + "%");
			Pictor.GammaLookupTable gamma = new Pictor.GammaLookupTable(1.8);
			Pictor.PixelFormat.IBlender NormalBlender = new Pictor.PixelFormat.BlenderBGRA();
			Pictor.PixelFormat.IBlender GammaBlender = new Pictor.PixelFormat.BlenderGammaBGRA(gamma);
			Pictor.PixelFormat.FormatRGBA pixf = new Pictor.PixelFormat.FormatRGBA(RenderBufferWindow, NormalBlender);
			Pictor.PixelFormat.FormatClippingProxy clippingProxy = new Pictor.PixelFormat.FormatClippingProxy(pixf);

			clippingProxy.Clear(new Pictor.RGBA_Doubles(1, 1, 1));

			Pictor.AntiAliasedScanlineRasterizer ras = new Pictor.AntiAliasedScanlineRasterizer();
			Pictor.Scanline sl = new Pictor.Scanline();

			Pictor.VertexSource.Ellipse e = new Pictor.VertexSource.Ellipse();

			// TODO: If you drag the control circles below the bottom of the window we get an exception.  This does not happen in Pictor.
			// It needs to be debugged.  Turning on clipping fixes it.  But standard Pictor works without clipping.  Could be a bigger problem than this.
			//ras.clip_box(0, 0, Width(), Height());

			// Render two "control" circles
			e.Init(m_x[0], m_y[0], 3, 3, 16);
			ras.AddPath(e);
			Pictor.Renderer.RenderSolid(clippingProxy, ras, sl, new Pictor.RGBA_Bytes(127, 127, 127));
			e.Init(m_x[1], m_y[1], 3, 3, 16);
			ras.AddPath(e);
			Pictor.Renderer.RenderSolid(clippingProxy, ras, sl, new Pictor.RGBA_Bytes(127, 127, 127));

			double d = 0.0;

			// Creating a rounded rectangle
			Pictor.VertexSource.RoundedRect r = new Pictor.VertexSource.RoundedRect(m_x[0] + d, m_y[0] + d, m_x[1] + d, m_y[1] + d, radius.Value());
			r.NormalizeRadius();

			if (border.Status())
			{
				Pictor.VertexSource.StrokeConverter p = new Pictor.VertexSource.StrokeConverter(r);
				p.Width(1.0);
				ras.AddPath(p);
			}
			else
			{
				ras.AddPath(r);
			}

			pixf.Blender = GammaBlender;

			Pictor.Renderer.RenderSolid(clippingProxy, ras, sl, background);

			// this was in the original demo, but it does nothing because we changed the blender not the Gamma function.
			//ras.Gamma(new gamma_none());
			// so let's change the blender instead
			pixf.Blender = NormalBlender;

			if (null != dialog && !dialog.Running && dialog.Result)
				System.Windows.Forms.Application.Exit();

			// Render the controls
			//m_radius.Render(ras, sl, clippingProxy);
			//m_gamma.Render(ras, sl, clippingProxy);
			//m_offset.Render(ras, sl, clippingProxy);
			//m_white_on_black.Render(ras, sl, clippingProxy);
			//m_DrawAsOutlineCheckBox.Render(ras, sl, clippingProxy);
			base.OnDraw();
		}

		public override void OnMouseDown(Pictor.UI.MouseEventArgs mouseEvent)
		{
			if (mouseEvent.Button == Pictor.UI.MouseButtons.Left)
			{
				for (int i = 0; i < 2; i++)
				{
					double x = mouseEvent.X;
					double y = mouseEvent.Y;
					if (System.Math.Sqrt((x - m_x[i]) * (x - m_x[i]) + (y - m_y[i]) * (y - m_y[i])) < 5.0)
					{
						m_dx = x - m_x[i];
						m_dy = y - m_y[i];
						m_idx = i;
						break;
					}
				}
			}

			base.OnMouseDown(mouseEvent);
		}

		public override void OnMouseMove(Pictor.UI.MouseEventArgs mouseEvent)
		{
			if (mouseEvent.Button == Pictor.UI.MouseButtons.Left)
			{
				if (m_idx >= 0)
				{
					m_x[m_idx] = mouseEvent.X - m_dx;
					m_y[m_idx] = mouseEvent.Y - m_dy;
					ForceRedraw();
				}
			}

			base.OnMouseMove(mouseEvent);
		}

		override public void OnMouseUp(Pictor.UI.MouseEventArgs mouseEvent)
		{
			m_idx = -1;
			base.OnMouseUp(mouseEvent);
		}

		public static void StartDemo()
		{
			EmulatorDemo app = new EmulatorDemo(Pictor.UI.PlatformSupportAbstract.PixelFormats.Rgba32, Pictor.UI.PlatformSupportAbstract.ERenderOrigin.OriginBottomLeft);
			app.Caption = "MOSA :: Emulator :: Pictor Demonstration";

			if (app.Init(600, 400, (uint)Pictor.UI.PlatformSupportAbstract.EWindowFlags.Risizeable))
			{
				app.Run();
			}
		}
	}
}