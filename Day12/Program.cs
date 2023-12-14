using System.Diagnostics.CodeAnalysis;

internal class Program
{
	private static Dictionary<char[], Dictionary<int[], ulong>> _cache = new(new GenericArrayComparer<char>());

	private static void Main(string[] args)
	{
		Part1();
		Part2();
	}

	private static void Part1()
	{
		ulong sum = 0;
		string[] lines = File.ReadAllLines("Input.txt");
		foreach (string line in lines)
		{
			string[] parts = line.Split(' ');
			char[] springs = parts[0].ToCharArray();
			int[] brokenSpringCounts = parts[1].Split(',').Select(int.Parse).ToArray();
			sum += CheckArrangements(springs, brokenSpringCounts);
		}

		Console.WriteLine($"Part 1: {sum}");
	}

	private static void Part2()
	{
		ulong sum = 0;
		string[] lines = File.ReadAllLines("Input.txt");
		foreach (string line in lines)
		{
			string[] parts = line.Split(' ');
			char[] springs = $"{parts[0]}?{parts[0]}?{parts[0]}?{parts[0]}?{parts[0]}".ToCharArray();
			int[] brokenSpringCounts = $"{parts[1]},{parts[1]},{parts[1]},{parts[1]},{parts[1]}".Split(',').Select(int.Parse).ToArray();
			sum += CheckArrangements(springs, brokenSpringCounts);
			_cache.Clear();
		}

		Console.WriteLine($"Part 2: {sum}");
	}

	private static ulong CheckArrangements(char[] springs, int[] brokenSpringCounts)
	{
		if (_cache.TryGetValue(springs, out Dictionary<int[], ulong>? innerCache))
		{
			if (innerCache.TryGetValue(brokenSpringCounts, out ulong result))
			{
				return result;
			}
		}

		ulong localSum = 0;
		for (int i = 0; i <= springs.Length - (brokenSpringCounts.Sum() + brokenSpringCounts.Length - 1); i++)
		{
			if (i > 0 && springs[i - 1] is '#')
			{
				if (_cache.TryGetValue(springs, out innerCache))
				{
					innerCache.Add(brokenSpringCounts, localSum);
				}
				else
				{
					_cache.Add(springs, new Dictionary<int[], ulong>(new GenericArrayComparer<int>()) { { brokenSpringCounts, localSum } });
				}

				return localSum;
			}

			int count = 0;
			for (int j = 0; j < brokenSpringCounts[0]; j++)
			{
				int index = j + i;
				if (springs[index] is '.')
				{
					break;
				}
				else
				{
					count++;
				}

				if (count == brokenSpringCounts[0])
				{
					if (brokenSpringCounts.Length == 1)
					{
						if (index + 1 < springs.Length && (springs[(index + 1)..].Any(c => c == '#')))
						{
							break;
						}

						localSum++;
					}
					else if (index + 2 < springs.Length && springs[index + 1] is '.' or '?')
					{
						localSum += CheckArrangements(springs[(index + 2)..], brokenSpringCounts[1..]);
					}

					break;
				}
			}
		}

		if (_cache.TryGetValue(springs, out innerCache))
		{
			innerCache.Add(brokenSpringCounts, localSum);
		}
		else
		{
			_cache.Add(springs, new Dictionary<int[], ulong>(new GenericArrayComparer<int>()) { { brokenSpringCounts, localSum } });
		}

		return localSum;
	}

	private class GenericArrayComparer<T> : IEqualityComparer<T[]>
	{
		public bool Equals(T[]? x, T[]? y)
		{
			return x.SequenceEqual(y);
		}

		public int GetHashCode([DisallowNull] T[] obj)
		{
			return obj.Length.GetHashCode();
		}
	}
}