using System.ComponentModel;
using System.Text;

namespace System.IO.Ports;

public class SerialPort : Component
{
	public const int InfiniteTimeout = -1;

	public Stream BaseStream
	{
		get
		{
			throw null;
		}
	}

	public int BaudRate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool BreakState
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int BytesToRead
	{
		get
		{
			throw null;
		}
	}

	public int BytesToWrite
	{
		get
		{
			throw null;
		}
	}

	public bool CDHolding
	{
		get
		{
			throw null;
		}
	}

	public bool CtsHolding
	{
		get
		{
			throw null;
		}
	}

	public int DataBits
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool DiscardNull
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool DsrHolding
	{
		get
		{
			throw null;
		}
	}

	public bool DtrEnable
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Encoding Encoding
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Handshake Handshake
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsOpen
	{
		get
		{
			throw null;
		}
	}

	public string NewLine
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Parity Parity
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public byte ParityReplace
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string PortName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int ReadBufferSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int ReadTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int ReceivedBytesThreshold
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool RtsEnable
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public StopBits StopBits
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int WriteBufferSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int WriteTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public event SerialDataReceivedEventHandler DataReceived
	{
		add
		{
		}
		remove
		{
		}
	}

	public event SerialErrorReceivedEventHandler ErrorReceived
	{
		add
		{
		}
		remove
		{
		}
	}

	public event SerialPinChangedEventHandler PinChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public SerialPort()
	{
	}

	public SerialPort(IContainer container)
	{
	}

	public SerialPort(string portName)
	{
	}

	public SerialPort(string portName, int baudRate)
	{
	}

	public SerialPort(string portName, int baudRate, Parity parity)
	{
	}

	public SerialPort(string portName, int baudRate, Parity parity, int dataBits)
	{
	}

	public SerialPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
	{
	}

	public void Close()
	{
	}

	public void DiscardInBuffer()
	{
	}

	public void DiscardOutBuffer()
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public static string[] GetPortNames()
	{
		throw null;
	}

	public void Open()
	{
	}

	public int Read(byte[] buffer, int offset, int count)
	{
		throw null;
	}

	public int Read(char[] buffer, int offset, int count)
	{
		throw null;
	}

	public int ReadByte()
	{
		throw null;
	}

	public int ReadChar()
	{
		throw null;
	}

	public string ReadExisting()
	{
		throw null;
	}

	public string ReadLine()
	{
		throw null;
	}

	public string ReadTo(string value)
	{
		throw null;
	}

	public void Write(byte[] buffer, int offset, int count)
	{
	}

	public void Write(char[] buffer, int offset, int count)
	{
	}

	public void Write(string text)
	{
	}

	public void WriteLine(string text)
	{
	}
}
