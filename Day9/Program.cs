internal class Program
{
	private static void Main(string[] args)
	{
		Part1();
		Part2();
	}

	private static void Part1()
	{
		int sum = 0;
		foreach (string line in File.ReadLines("Input1.txt"))
		{
			int[] series = line.Split(' ').Select(int.Parse).ToArray();
			sum += Method1(series);
		}

		Console.WriteLine($"Part 1: {sum}");
	}

	private static int Method1(int[] series)
	{
		int[] newSeries = new int[series.Length - 1];
		bool allSame = true;
		int? firstNum = null;
		for (int i = 1; i < series.Length; i++)
		{
			int num = series[i] - series[i - 1];
			firstNum ??= num;
			if (allSame)
			{
				allSame = firstNum == num;
			}

			newSeries[i - 1] = num;
		}

		if (allSame)
		{
			return firstNum!.Value + series[^1];
		}

		int newNumber = Method1(newSeries);

		return newNumber + series[^1];
	}

	private static void Part2()
	{
		int sum = 0;
		foreach (string line in File.ReadLines("Input1.txt"))
		{
			int[] series = line.Split(' ').Select(int.Parse).ToArray();
			sum += Method2(series);
		}

		Console.WriteLine($"Part 2: {sum}");
	}

	private static int Method2(int[] series)
	{
		int[] newSeries = new int[series.Length - 1];
		bool allSame = true;
		int? firstNum = null;
		for (int i = 1; i < series.Length; i++)
		{
			int num = series[i] - series[i - 1];
			firstNum ??= num;
			if (allSame)
			{
				allSame = firstNum == num;
			}

			newSeries[i - 1] = num;
		}

		if (allSame)
		{
			return series[0] - firstNum!.Value;
		}

		int newNumber = Method2(newSeries);

		return series[0] - newNumber;
	}
}