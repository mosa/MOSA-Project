using System.Collections;

namespace System.ComponentModel.Design;

public interface IComponentDiscoveryService
{
	ICollection GetComponentTypes(IDesignerHost? designerHost, Type? baseType);
}
