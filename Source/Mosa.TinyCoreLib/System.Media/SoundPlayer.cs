using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;

namespace System.Media;

[ToolboxItem(false)]
public class SoundPlayer : Component, ISerializable
{
	public bool IsLoadCompleted
	{
		get
		{
			throw null;
		}
	}

	public int LoadTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string SoundLocation
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Stream? Stream
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public object? Tag
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public event AsyncCompletedEventHandler? LoadCompleted
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler? SoundLocationChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler? StreamChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public SoundPlayer()
	{
	}

	public SoundPlayer(Stream? stream)
	{
	}

	protected SoundPlayer(SerializationInfo serializationInfo, StreamingContext context)
	{
	}

	public SoundPlayer(string soundLocation)
	{
	}

	public void Load()
	{
	}

	public void LoadAsync()
	{
	}

	protected virtual void OnLoadCompleted(AsyncCompletedEventArgs e)
	{
	}

	protected virtual void OnSoundLocationChanged(EventArgs e)
	{
	}

	protected virtual void OnStreamChanged(EventArgs e)
	{
	}

	public void Play()
	{
	}

	public void PlayLooping()
	{
	}

	public void PlaySync()
	{
	}

	public void Stop()
	{
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
