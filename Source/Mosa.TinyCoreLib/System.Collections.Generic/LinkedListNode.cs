namespace System.Collections.Generic;

public sealed class LinkedListNode<T>(T value)
{
	public LinkedList<T>? List => list;

	public LinkedListNode<T>? Next => next;

	public LinkedListNode<T>? Previous => previous;

	public T Value
	{
		get => nodeValue;
		set => nodeValue = value;
	}

	public ref T ValueRef => ref nodeValue;

	internal LinkedList<T>? list;
	internal LinkedListNode<T>? next, previous;
	internal T nodeValue = value;
}
