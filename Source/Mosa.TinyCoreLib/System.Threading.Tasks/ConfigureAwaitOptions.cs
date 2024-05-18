namespace System.Threading.Tasks;

[Flags]
public enum ConfigureAwaitOptions
{
	None = 0,
	ContinueOnCapturedContext = 1,
	SuppressThrowing = 2,
	ForceYielding = 4
}
