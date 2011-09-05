
namespace Pictor.UI
{
	public class MouseEventArgs// : EventArgs
	{
		private MouseButtons m_Button;
		private int m_Clicks;
		private double m_X;
		private double m_Y;
		private int m_WheelDelta;
		private bool m_Handled;

		//public MouseEventArgs(MouseButtons button, int clicks, int x, int y, int wheelDelta)
		//: this

		public MouseEventArgs(MouseEventArgs original, double newX, double newY)
			: this(original.Button, original.Clicks, newX, newY, original.WheelDelta)
		{
		}

		public MouseEventArgs(MouseButtons button, int clicks, double x, double y, int wheelDelta)
		{
			m_Button = button;
			m_Clicks = clicks;
			m_X = x;
			m_Y = y;
			m_WheelDelta = wheelDelta;
		}

		public MouseButtons Button { get { return m_Button; } }
		public int Clicks { get { return m_Clicks; } }
		public int WheelDelta { get { return m_WheelDelta; } }
		//public Point Location { get; }
		public double X { get { return m_X; } }
		public double Y { get { return m_Y; } }
		public bool Handled { get { return m_Handled; } set { m_Handled = value; } }
	}
}
