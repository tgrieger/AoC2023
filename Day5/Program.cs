using System.Collections.Generic;
using System.Diagnostics;

internal class Program
{
	private static void Main(string[] args)
	{
		Part1();
		Part2();
	}

	private static void Part1()
	{
		long minLocationValue = long.MaxValue;
		List<KeyValuePair<LongRange, LongRange>> seedToSoil = new();
		List<KeyValuePair<LongRange, LongRange>> soilToFertilizer = new();
		List<KeyValuePair<LongRange, LongRange>> fertilizerToWater = new();
		List<KeyValuePair<LongRange, LongRange>> waterToLight = new();
		List<KeyValuePair<LongRange, LongRange>> lightToTemp = new();
		List<KeyValuePair<LongRange, LongRange>> tempToHumidity = new();
		List<KeyValuePair<LongRange, LongRange>> humidityToLocation = new();

		List<List<KeyValuePair<LongRange, LongRange>>> lists = [seedToSoil, soilToFertilizer, fertilizerToWater, waterToLight, lightToTemp, tempToHumidity, humidityToLocation];

		string[] lines = File.ReadAllLines("Input1.txt");
		int listNumber = 0;
		List<long> seeds = lines[0].Substring(6).Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
		foreach (string line in lines.Skip(2))
		{
			if (string.IsNullOrWhiteSpace(line))
			{
				listNumber++;
				continue;
			}

			if (line.EndsWith(':'))
			{
				continue;
			}

			List<KeyValuePair<LongRange, LongRange>> list = lists[listNumber];
			List<long> nums = line.Split(' ').Select(long.Parse).ToList();
			list.Add(new KeyValuePair<LongRange, LongRange>(new LongRange(nums[1], nums[1] + nums[2]), new LongRange(nums[0], nums[0] + nums[2])));
		}

		foreach (long seed in seeds)
		{
			long key = seed;
			foreach (List<KeyValuePair<LongRange, LongRange>> list in lists)
			{
				foreach ((LongRange source, LongRange target) in list)
				{
					if (source.IsInRange(key))
					{
						key = (key - source.Start) + target.Start;
						break;
					}
				}
			}

			minLocationValue = Math.Min(minLocationValue, key);
		}

		Console.WriteLine($"Part 1: {minLocationValue}");
	}

	private static void Part2()
	{
		List<KeyValuePair<LongRange, LongRange>> seedToSoil = new();
		List<KeyValuePair<LongRange, LongRange>> soilToFertilizer = new();
		List<KeyValuePair<LongRange, LongRange>> fertilizerToWater = new();
		List<KeyValuePair<LongRange, LongRange>> waterToLight = new();
		List<KeyValuePair<LongRange, LongRange>> lightToTemp = new();
		List<KeyValuePair<LongRange, LongRange>> tempToHumidity = new();
		List<KeyValuePair<LongRange, LongRange>> humidityToLocation = new();

		List<List<KeyValuePair<LongRange, LongRange>>> lists = [seedToSoil, soilToFertilizer, fertilizerToWater, waterToLight, lightToTemp, tempToHumidity, humidityToLocation];

		string[] lines = File.ReadAllLines("Input1.txt");
		int listNumber = 0;
		List<long> inputs = lines[0].Substring(6).Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

		foreach (string line in lines.Skip(2))
		{
			if (string.IsNullOrWhiteSpace(line))
			{
				listNumber++;
				continue;
			}

			if (line.EndsWith(':'))
			{
				continue;
			}

			List<KeyValuePair<LongRange, LongRange>> list = lists[listNumber];
			List<long> nums = line.Split(' ').Select(long.Parse).ToList();
			list.Add(new KeyValuePair<LongRange, LongRange>(new LongRange(nums[1], nums[1] + nums[2]), new LongRange(nums[0], nums[0] + nums[2])));
		}

		List<LongRange> seedRanges = new();
		for (int i = 0; i < inputs.Count; i += 2)
		{
			seedRanges.Add(new LongRange(inputs[i], inputs[i] + inputs[i + 1]));
		}

		List<LongRange> longRanges = GetLongRangeLists(seedRanges, 0);

		Console.WriteLine($"Part 2: {longRanges[0].Start}");

		List<LongRange> GetLongRangeLists(List<LongRange> inputRanges, int listDepth)
		{
			if (listDepth == lists.Count)
			{
				return inputRanges;
			}

			List<KeyValuePair<LongRange, LongRange>> list = lists[listDepth];
			SortedList<long, LongRange> sortedRanges = new();

			foreach (LongRange inputRange in inputRanges)
			{
				LongRange tempLongRange = new(inputRange.Start, inputRange.End);
				foreach ((LongRange source, LongRange target) in list)
				{
					LongRange? intersection = tempLongRange.GetIntersectionPoints(source);
					if (intersection is not null)
					{
						if (source.IsInRange(tempLongRange.Start))
						{
							tempLongRange = new LongRange(source.End, tempLongRange.End);
						}

						if (source.IsInRange(tempLongRange.End))
						{
							tempLongRange = new LongRange(tempLongRange.Start, source.Start);
						}

						long diff = target.Start - source.Start;
						AddToSortedList(sortedRanges, new LongRange(diff + intersection.Value.Start, diff + intersection.Value.End));
					}
				}

				// Only add to the list if it's a valid range.
				if (tempLongRange.Start < tempLongRange.End)
				{
					AddToSortedList(sortedRanges, tempLongRange);
				}
			}

			// Combine
			List<LongRange> newInputRanges = new();
			for (int i = 0; i < sortedRanges.Count; i++)
			{
				LongRange first = sortedRanges.GetValueAtIndex(i);
				for (int j = i + 1; j < sortedRanges.Count; j++)
				{
					LongRange second = sortedRanges.GetValueAtIndex(j);
					if (first.IsInRange(second.Start) && i < sortedRanges.Count - 1)
					{
						first = new LongRange(first.Start, second.End);
						second = sortedRanges.GetValueAtIndex(i + 1);
						i = j;
					}
					else
					{
						break;
					}
				}

				newInputRanges.Add(first);
			}

			return GetLongRangeLists(newInputRanges, listDepth + 1);
		}
	}

	private static void AddToSortedList(SortedList<long, LongRange> sortedList, LongRange longRange)
	{
		if (!sortedList.TryGetValue(longRange.Start, out LongRange value))
		{
			sortedList.Add(longRange.Start, longRange);
		}
		else
		{
			if (value.End != longRange.End)
			{
				sortedList[longRange.Start] = new LongRange(value.Start, Math.Max(value.End, longRange.End));
			}
		}
	}
}

[DebuggerDisplay("Start = {Start}, End = {End}")]
internal struct LongRange
{
	public long Start { get; }

	public long End { get; }

	public LongRange(long start, long end)
	{
		Start = start;
		End = end;
	}

	public bool IsInRange(long num)
	{
		return Start <= num && End >= num;
	}

	public LongRange? GetIntersectionPoints(LongRange otherRange)
	{
		if (Start > otherRange.End || End < otherRange.Start)
		{
			return null;
		}

		return new LongRange(Math.Max(Start, otherRange.Start), Math.Min(End, otherRange.End));
	}
}