namespace System.ComponentModel;

[Designer("System.ComponentModel.Design.ComponentDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[Designer("System.Windows.Forms.Design.ComponentDocumentDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.IRootDesigner, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
[TypeConverter("System.ComponentModel.ComponentConverter, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
public interface IComponent : IDisposable
{
	ISite? Site { get; set; }

	event EventHandler? Disposed;
}
