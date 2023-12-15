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
		string[] lines = File.ReadAllLines("Input.txt");

		foreach (string line in lines)
		{
			string[] parts = line.Split(',');
			foreach (string part in parts)
			{
				int localSum = 0;
				foreach (char ch in part)
				{
					localSum += ch;
					localSum *= 17;
					localSum = localSum % 256;
				}

				sum += localSum;
			}
		}

		Console.WriteLine($"Part 1: {sum}");
	}

	private static void Part2()
	{
		int sum = 0;
		string[] lines = File.ReadAllLines("Input.txt");
		List<Box> boxes = new();
		for (int i = 0; i < 256; i++)
		{
			boxes.Add(new Box());
		}

		foreach (string line in lines)
		{
			string[] parts = line.Split(',');
			foreach (string part in parts)
			{
				if (part[^1] == '-')
				{
					int localSum = 0;
					foreach (char ch in part[..^1])
					{
						localSum += ch;
						localSum *= 17;
						localSum %= 256;
					}

					boxes[localSum].RemoveLens(part[..^1]);
				}
				else
				{
					int localSum = 0;
					foreach (char ch in part[..^2])
					{
						localSum += ch;
						localSum *= 17;
						localSum %= 256;
					}

					boxes[localSum].AddLens(new Lens(part[..^2], int.Parse(part[^1..])));
				}
			}
		}

		for (int i = 0; i < 256; i++)
		{
			for (int j = 0; j < boxes[i].LensList.Count; j++)
			{
				sum += (i + 1) * (j + 1) * boxes[i].LensList[j].FocalLength;
			}
		}

		Console.WriteLine($"Part 2: {sum}");
	}

	private class Box
	{
		public List<Lens> LensList { get; } = new();

		public HashSet<string> LensSet { get; } = new();

		public void AddLens(Lens lens)
		{
			if (LensSet.Contains(lens.Label))
			{
				int index = LensList.FindIndex(0, l => l.Label == lens.Label);
				LensList[index] = lens;
			}
			else
			{
				LensList.Add(lens);
				LensSet.Add(lens.Label);
			}
		}

		public void RemoveLens(string label)
		{
			if (LensSet.Contains(label))
			{
				for (int i = 0; i < LensList.Count; i++)
				{
					if (LensList[i].Label == label)
					{
						LensList.RemoveAt(i);
						LensSet.Remove(label);
						break;
					}
				}
			}
		}
	}

	private class Lens
	{
		public Lens(string label, int focalLength)
		{
			Label = label;
			FocalLength = focalLength;
		}

		public string Label { get; }

		public int FocalLength { get; }
	}
}