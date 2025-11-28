namespace System.Reflection;

[Flags]
public enum MethodSemanticsAttributes
{
	Setter = 1,
	Getter = 2,
	Other = 4,
	Adder = 8,
	Remover = 0x10,
	Raiser = 0x20
}
