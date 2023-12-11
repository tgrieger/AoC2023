internal class Program
{
	private static void Main(string[] args)
	{
		Part1();
		Part2();
	}

	private static void Part1()
	{
		string[] lines = File.ReadAllLines("Input1.txt");
		bool[] emptyRows = Enumerable.Repeat(true, lines.Length).ToArray();
		bool[] emptyColumns = Enumerable.Repeat(true, lines[0].Length).ToArray();
		int sum = 0;

		List<Point> points = new List<Point>();
		for (int y = 0; y < lines.Length; y++)
		{
			for (int x = 0; x < lines[y].Length; x++)
			{
				if (lines[y][x] == '.')
				{
					continue;
				}

				emptyRows[y] = false;
				emptyColumns[x] = false;

				points.Add(new Point(x, y));
			}
		}

		int[] rowModifier = new int[lines.Length];
		int[] columnModifier = new int[lines[0].Length];
		for (int i = 0; i < emptyRows.Length; i++)
		{
			if (!emptyRows[i])
			{
				continue;
			}

			for (int j = i; j < rowModifier.Length; j++)
			{
				rowModifier[j]++;
			}
		}

		for (int i = 0; i < emptyColumns.Length; i++)
		{
			if (!emptyColumns[i])
			{
				continue;
			}

			for (int j = i; j < columnModifier.Length; j++)
			{
				columnModifier[j]++;
			}
		}

		for (int i = 0; i < points.Count - 1; i++)
		{
			for (int j = i + 1; j < points.Count; j++)
			{
				Point starting = points[i];
				Point target = points[j];

				sum += Math.Abs((starting.X + columnModifier[starting.X]) - (target.X + columnModifier[target.X]));
				sum += Math.Abs((starting.Y + rowModifier[starting.Y]) - (target.Y + rowModifier[target.Y]));
			}
		}

		Console.WriteLine($"Part 1: {sum}");
	}

	private static void Part2()
	{
		string[] lines = File.ReadAllLines("Input1.txt");
		bool[] emptyRows = Enumerable.Repeat(true, lines.Length).ToArray();
		bool[] emptyColumns = Enumerable.Repeat(true, lines[0].Length).ToArray();
		ulong sum = 0;

		List<Point> points = new List<Point>();
		for (int y = 0; y < lines.Length; y++)
		{
			for (int x = 0; x < lines[y].Length; x++)
			{
				if (lines[y][x] == '.')
				{
					continue;
				}

				emptyRows[y] = false;
				emptyColumns[x] = false;

				points.Add(new Point(x, y));
			}
		}

		int[] rowModifier = new int[lines.Length];
		int[] columnModifier = new int[lines[0].Length];
		for (int i = 0; i < emptyRows.Length; i++)
		{
			if (!emptyRows[i])
			{
				continue;
			}

			for (int j = i; j < rowModifier.Length; j++)
			{
				rowModifier[j] += 1000000 - 1;
			}
		}

		for (int i = 0; i < emptyColumns.Length; i++)
		{
			if (!emptyColumns[i])
			{
				continue;
			}

			for (int j = i; j < columnModifier.Length; j++)
			{
				columnModifier[j] += 1000000 - 1;
			}
		}

		for (int i = 0; i < points.Count - 1; i++)
		{
			for (int j = i + 1; j < points.Count; j++)
			{
				Point starting = points[i];
				Point target = points[j];

				sum += (ulong)Math.Abs((starting.X + columnModifier[starting.X]) - (target.X + columnModifier[target.X]));
				sum += (ulong)Math.Abs((starting.Y + rowModifier[starting.Y]) - (target.Y + rowModifier[target.Y]));
			}
		}

		Console.WriteLine($"Part 2: {sum}");
	}

	private class Point
	{
		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}

		public int X { get; set; }

		public int Y { get; set; }
	}
}