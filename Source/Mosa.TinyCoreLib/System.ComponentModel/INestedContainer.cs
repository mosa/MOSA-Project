namespace System.ComponentModel;

public interface INestedContainer : IContainer, IDisposable
{
	IComponent Owner { get; }
}
