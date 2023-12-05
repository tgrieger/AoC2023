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
		char[] chars = new char[2];
		int sum = 0;
		foreach (string line in lines)
		{
			if (string.IsNullOrEmpty(line)) continue;

			chars[0] = line.First(c => c >= '0' && c <= '9');
			chars[1] = line.Last(c => c >= '0' && c <= '9');

			sum += int.Parse(chars);
		}

		Console.Write("Part 1: ");
		Console.WriteLine(sum);
	}

	private static void Part2()
	{
		string[] lines = File.ReadAllLines("Input1.txt");
		Dictionary<string, int> numberWords = new() { { "one", 1 }, { "two", 2 }, { "three", 3 }, { "four", 4 }, { "five", 5 }, { "six", 6 }, { "seven", 7 }, { "eight", 8 }, { "nine", 9 } };
		int sum = 0;
		foreach (string line in lines)
		{
			if (string.IsNullOrEmpty(line)) continue;

			int n;
			int first = -1;
			int last = -1;
			for (n = 0; n < line.Length; n++)
			{
				if (line[n] >= '0' && line[n] <= '9')
				{
					first = line[n] - '0';
					break;
				}
			}

			for (int i = 0; i < Math.Min(n - 2, line.Length - 3); i++)
			{
				bool found = false;
				for (int j = i + 3; j <= n && (j - i) <= 5; j++)
				{
					string substring = line[i..j];
					found = numberWords.ContainsKey(substring);
					if (found)
					{
						first = numberWords[substring];
						break;
					}
				}

				if (found)
				{
					break;
				}
			}

			if (first < 1)
			{
				throw new Exception("First was not set");
			}

			first *= 10;

			for (n = line.Length - 1; n >= 0; n--)
			{
				if (line[n] >= '0' && line[n] <= '9')
				{
					last = line[n] - '0';
					break;
				}
			}

			for (int i = line.Length - 3; i > n; i--)
			{
				bool found = false;
				for (int j = Math.Min(line.Length, i + 5); (j - i) >= 3; j--)
				{
					string substring = line[i..j];
					found = numberWords.ContainsKey(substring);
					if (found)
					{
						last = numberWords[substring];
						break;
					}
				}

				if (found)
				{
					break;
				}
			}

			if (last < 1)
			{
				throw new Exception("Last was not set");
			}

			sum = sum + first + last;
		}

		Console.Write("Part 2: ");
		Console.WriteLine(sum);
	}
}