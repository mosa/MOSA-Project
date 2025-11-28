namespace System.ComponentModel;

public interface ITypedList
{
	PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[]? listAccessors);

	string GetListName(PropertyDescriptor[]? listAccessors);
}
