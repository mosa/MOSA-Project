namespace Pictor.UI
{
	public class KeyEventArgs// : EventArgs
	{
		private Keys m_KeyData;
		private bool m_SuppressKeyPress;
		private bool m_Handled;

		public KeyEventArgs(Keys keyData)
		{
			m_KeyData = keyData;
		}

		public virtual bool Alt
		{
			get
			{
				return (m_KeyData & Keys.Alt) != 0;
			}
		}

		public bool Control
		{
			get
			{
				return (m_KeyData & Keys.Control) != 0;
			}
		}

		public bool Handled
		{
			get
			{
				return m_Handled;
			}
			set
			{
				m_Handled = value;
			}
		}

		public Keys KeyCode
		{
			get
			{
				return (m_KeyData & ~(Keys.Control | Keys.Shift | Keys.Alt));
			}
		}

		public Keys KeyData
		{
			get
			{
				return m_KeyData;
			}
		}

		public int KeyValue
		{
			get
			{
				return (int)m_KeyData;
			}
		}

		public Keys Modifiers
		{
			get
			{
				return m_KeyData & (Keys.Alt | Keys.Shift | Keys.Control);
			}
		}

		public virtual bool Shift
		{
			get
			{
				return (m_KeyData & Keys.Shift) != 0;
			}
		}

		public bool SuppressKeyPress
		{
			get
			{
				return m_SuppressKeyPress;
			}
			set
			{
				m_SuppressKeyPress = value;
			}
		}
	}
}