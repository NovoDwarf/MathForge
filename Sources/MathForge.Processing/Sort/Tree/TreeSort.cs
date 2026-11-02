namespace MathForge.Processing.Sort.Merge;

public sealed class TreeSort<T> : Sorting<T> where T : IComparable<T>
{
	public override void Sort(T[] array)
	{
		if (array.Length < 2)
			return;

		Node? root = null;

		foreach (var t in array)
			root = Insert(root, t);

		var index = 0;
		
		Traverse(root, array, ref index);
	}

	private Node Insert(Node? node, T value)
	{
		if (node is null)
			return new Node(value);

		if (value.CompareTo(node.Value) < 0)
			node.Left = Insert(node.Left, value);
		else
			node.Right = Insert(node.Right, value);

		return node;
	}

	private void Traverse(Node? node, T[] array, ref int index)
	{
		if (node is null)
			return;

		Traverse(node.Left, array, ref index);

		array[index] = node.Value;

		OnStep?.Invoke(new SortingStep<T>
		{
			Array = (T[])array.Clone(),
			Left = index,
			Right = index
		});

		index++;

		Traverse(node.Right, array, ref index);
	}

	private sealed class Node(T value)
	{
		public T Value { get; } = value;

		public Node? Left { get; set; }

		public Node? Right { get; set; }
	}
}