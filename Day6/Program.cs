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
		List<int> times = lines[0][5..].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToList();
		List<int> distances = lines[1][9..].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToList();

		int sum = 0;
		for (int i = 0; i < times.Count; i++)
		{
			int localSum = 0;
			int starting = times[i] / 2;
			for (int j = starting; j >= 0; j--)
			{
				if ((times[i] - j) * j > distances[i])
				{
					localSum++;
				}
			}

			localSum *= 2;

			localSum = times[i] % 2 == 0 ? localSum - 1 : localSum;

			if (sum == 0)
			{
				sum += localSum;
			}
			else
			{
				sum *= localSum;
			}
		}

		Console.WriteLine($"Part 1: {sum}");
	}

	private static void Part2()
	{
		string[] lines = File.ReadAllLines("Input1.txt");
		ulong time = ulong.Parse(lines[0][5..].Replace(" ", ""));
		ulong distance = ulong.Parse(lines[1][9..].Replace(" ", ""));

		ulong start = 0;
		ulong end = time / 2;
		while (end > start + 1)
		{
			ulong midPoint = (end + start) / 2;
			if ((time - midPoint) * midPoint >= distance)
			{
				end = midPoint;
			}
			else
			{
				start = midPoint;
			}
		}

		ulong mid = time / 2;
		ulong diff = mid - end;

		Console.WriteLine($"Part 2: {diff * 2 + 1}");
	}
}