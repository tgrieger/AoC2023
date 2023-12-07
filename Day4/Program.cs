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
		string[] lines = File.ReadAllLines("Input1.txt");
		foreach (string line in lines)
		{
			int colonIndex = line.IndexOf(':');
			int barIndex = line.IndexOf('|');
			HashSet<int> winningNumbers = line[(colonIndex + 1)..barIndex].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToHashSet();
			IEnumerable<int> numbers = line[(barIndex + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse);

			int localSum = 0;
			foreach (int number in numbers)
			{
				if (!winningNumbers.Contains(number))
				{
					continue;
				}

				localSum = localSum == 0 ? 1 : localSum * 2;
			}

			sum += localSum;
		}

		Console.WriteLine($"Part 1: {sum}");
	}

	private static void Part2()
	{
		int sum = 0;
		string[] lines = File.ReadAllLines("Input1.txt");
		int[] extraCards = new int[lines.Length];
		for (int i = 0; i < lines.Length; i++)
		{
			int colonIndex = lines[i].IndexOf(':');
			int barIndex = lines[i].IndexOf('|');
			HashSet<int> winningNumbers = lines[i][(colonIndex + 1)..barIndex].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToHashSet();
			IEnumerable<int> numbers = lines[i][(barIndex + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse);

			int extraCardIndex = i;
			extraCards[i] += 1;
			foreach (int number in numbers)
			{
				if (!winningNumbers.Contains(number))
				{
					continue;
				}

				extraCardIndex++;
				if (extraCardIndex < extraCards.Length)
				{
					extraCards[extraCardIndex] += extraCards[i];
				}
			}

			sum += extraCards[i];
		}

		Console.WriteLine($"Part 2: {sum}");
	}
}