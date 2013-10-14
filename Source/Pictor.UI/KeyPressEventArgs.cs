namespace Pictor.UI
{
	public class KeyPressEventArgs
	{
		private char m_KeyChar;
		private bool m_Handled;

		public KeyPressEventArgs(char keyChar)
		{
			m_Handled = false;
			m_KeyChar = keyChar;
		}

		public bool Handled { get { return m_Handled; } set { m_Handled = value; } }

		public char KeyChar { get { return m_KeyChar; } set { m_KeyChar = value; } }
	}
}