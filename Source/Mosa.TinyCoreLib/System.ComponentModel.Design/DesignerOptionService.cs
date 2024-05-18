using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.Design;

public abstract class DesignerOptionService : IDesignerOptionService
{
	[TypeConverter(typeof(DesignerOptionConverter))]
	[Editor("", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public sealed class DesignerOptionCollection : ICollection, IEnumerable, IList
	{
		public int Count
		{
			get
			{
				throw null;
			}
		}

		public DesignerOptionCollection? this[int index]
		{
			get
			{
				throw null;
			}
		}

		public DesignerOptionCollection? this[string name]
		{
			get
			{
				throw null;
			}
		}

		public string Name
		{
			get
			{
				throw null;
			}
		}

		public DesignerOptionCollection? Parent
		{
			get
			{
				throw null;
			}
		}

		public PropertyDescriptorCollection Properties
		{
			[RequiresUnreferencedCode("The Type of DesignerOptionCollection's value cannot be statically discovered.")]
			get
			{
				throw null;
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				throw null;
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				throw null;
			}
		}

		bool IList.IsFixedSize
		{
			get
			{
				throw null;
			}
		}

		bool IList.IsReadOnly
		{
			get
			{
				throw null;
			}
		}

		object? IList.this[int index]
		{
			get
			{
				throw null;
			}
			set
			{
			}
		}

		internal DesignerOptionCollection()
		{
		}

		public void CopyTo(Array array, int index)
		{
		}

		public IEnumerator GetEnumerator()
		{
			throw null;
		}

		public int IndexOf(DesignerOptionCollection value)
		{
			throw null;
		}

		public bool ShowDialog()
		{
			throw null;
		}

		int IList.Add(object? value)
		{
			throw null;
		}

		void IList.Clear()
		{
		}

		bool IList.Contains(object? value)
		{
			throw null;
		}

		int IList.IndexOf(object? value)
		{
			throw null;
		}

		void IList.Insert(int index, object? value)
		{
		}

		void IList.Remove(object? value)
		{
		}

		void IList.RemoveAt(int index)
		{
		}
	}

	internal sealed class DesignerOptionConverter
	{
	}

	public DesignerOptionCollection Options
	{
		get
		{
			throw null;
		}
	}

	protected DesignerOptionCollection CreateOptionCollection(DesignerOptionCollection parent, string name, object value)
	{
		throw null;
	}

	protected virtual void PopulateOptionCollection(DesignerOptionCollection options)
	{
	}

	protected virtual bool ShowDialog(DesignerOptionCollection options, object optionObject)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The option value's Type cannot be statically discovered.")]
	object IDesignerOptionService.GetOptionValue(string pageName, string valueName)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The option value's Type cannot be statically discovered.")]
	void IDesignerOptionService.SetOptionValue(string pageName, string valueName, object value)
	{
	}
}
