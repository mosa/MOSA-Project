namespace System.Threading.Channels;

public enum BoundedChannelFullMode
{
	Wait,
	DropNewest,
	DropOldest,
	DropWrite
}
