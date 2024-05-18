using System.Runtime.Versioning;

namespace System.Text;

public enum NormalizationForm
{
	FormC = 1,
	FormD = 2,
	[UnsupportedOSPlatform("browser")]
	FormKC = 5,
	[UnsupportedOSPlatform("browser")]
	FormKD = 6
}
