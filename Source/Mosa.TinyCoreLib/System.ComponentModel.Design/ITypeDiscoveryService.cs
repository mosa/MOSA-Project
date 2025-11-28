using System.Collections;

namespace System.ComponentModel.Design;

public interface ITypeDiscoveryService
{
	ICollection GetTypes(Type? baseType, bool excludeGlobalTypes);
}
