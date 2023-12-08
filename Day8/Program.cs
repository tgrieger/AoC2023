using System.Numerics;

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
		char[] sequence = lines[0].ToCharArray();
		Dictionary<string, Node> graph = new();
		foreach (string line in lines.Skip(2))
		{
			string key = line[..3];
			Node node = new(line[7..10], line[12..15]);
			graph.Add(key, node);
		}

		string currentKey = "AAA";
		while (currentKey != "ZZZ")
		{
			foreach (char direction in sequence)
			{
				sum++;
				currentKey = direction == 'L' ? graph[currentKey].Left : graph[currentKey].Right;

				if (currentKey == "ZZZ")
				{
					break;
				}
			}
		}

		Console.WriteLine($"Part 1: {sum}");
	}

	private static void Part2()
	{
		ulong sum = 0;
		string[] lines = File.ReadAllLines("Input1.txt");
		char[] sequence = lines[0].ToCharArray();
		Dictionary<string, Node> graph = new();
		List<string> currentKeys = new();
		foreach (string line in lines.Skip(2))
		{
			string key = line[..3];
			Node node = new(line[7..10], line[12..15]);
			graph.Add(key, node);

			if (key.EndsWith('A'))
			{
				currentKeys.Add(key);
			}
		}

		ulong[] firsts = new ulong[currentKeys.Count];
		while (firsts.Any(ul => ul == 0))
		{
			foreach (char direction in sequence)
			{
				sum++;

				for (int i = 0; i < currentKeys.Count; i++)
				{
					currentKeys[i] = direction == 'L' ? graph[currentKeys[i]].Left : graph[currentKeys[i]].Right;

					if (currentKeys[i].EndsWith('Z') && firsts[i] == 0)
					{
						firsts[i] = sum;
					}
				}

				if (!firsts.Any(ul => ul == 0))
				{
					break;
				}
			}
		}

		HashSet<ulong> primes = new();
		foreach (ulong first in firsts)
		{
			ulong working = first;
			for (ulong i = 2; working > 1; i++)
			{
				if (working % i == 0)
				{
					working /= i;
					primes.Add(i);
				}
			}
		}

		ulong final = 1;
		foreach (ulong prime in primes)
		{
			final *= prime;
		}

		Console.WriteLine($"Part 2: {final}");
	}

	private class Node
	{
		public Node(string left, string right)
		{
			Left = left;
			Right = right;
		}

		public string Left { get; }

		public string Right { get; }
	}
}