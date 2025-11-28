using System.Collections;

namespace System.ComponentModel.Design;

public interface ITreeDesigner : IDesigner, IDisposable
{
	ICollection Children { get; }

	IDesigner? Parent { get; }
}
