internal class Program
{
	private static void Main(string[] args)
	{
		Part1();
		Part2();
	}

	private static void Part1()
	{
		Dictionary<string, int> limits = new() { { "red", 12 }, { "green" , 13 }, { "blue", 14 } };

		string[] lines = File.ReadAllLines("Input1.txt");

		int sum = 0;
		foreach (string line in lines)
		{
			bool goodGame = true;
			int colonIndex = line.IndexOf(':');
			int id = int.Parse(line.Substring(5, colonIndex - 5));
			string[] pulls = line.Substring(colonIndex + 1).Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
			foreach (string pull in pulls)
			{
				string[] colors = pull.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
				foreach (string color in colors)
				{
					string[] parts = color.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
					if (limits[parts[1]] < int.Parse(parts[0]))
					{
						goodGame = false;
						break;
					}
				}

				if (!goodGame)
				{
					break;
				}
			}

			if (goodGame)
			{
				sum += id;
			}
		}

		Console.WriteLine($"Part 1: {sum}");
	}

	private static void Part2()
	{
		string[] lines = File.ReadAllLines("Input1.txt");

		int sum = 0;
		foreach (string line in lines)
		{
			Dictionary<string, int> maxs = new() { { "red", 0 }, { "green", 0 }, { "blue", 0 } };
			string[] pulls = line.Substring(line.IndexOf(':') + 1).Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
			foreach (string pull in pulls)
			{
				string[] colors = pull.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
				foreach (string color in colors)
				{
					string[] parts = color.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
					int value = int.Parse(parts[0]);
					if (maxs[parts[1]] < value)
					{
						maxs[parts[1]] = value;
					}
				}
			}

			sum += maxs["red"] * maxs["green"] * maxs["blue"];
		}

		Console.WriteLine($"Part 2: {sum}");
	}
}