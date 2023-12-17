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
		List<string> pattern = new();
		foreach (string line in lines)
		{
			if (!string.IsNullOrWhiteSpace(line))
			{
				pattern.Add(line);
				continue;
			}

			int? localSum = IsMirroredHorizontally(pattern);
			localSum ??= IsMirroredVertically(pattern);

			sum += localSum!.Value;
			pattern.Clear();
		}

		Console.WriteLine($"Part 1: {sum}");
	}

	private static void Part2()
	{
		int sum = 0;
		string[] lines = File.ReadAllLines("Input.txt");
		List<string> pattern = new();
		foreach (string line in lines)
		{
			if (!string.IsNullOrWhiteSpace(line))
			{
				pattern.Add(line);
				continue;
			}

			int? localSum = IsOffByOneMirroredHorizontally(pattern);
			localSum ??= IsOffByOneMirroredVertically(pattern);

			sum += localSum!.Value;
			pattern.Clear();
		}

		Console.WriteLine($"Part 2: {sum}");
	}

	private static int? IsMirroredHorizontally(List<string> pattern)
	{
		int? value = null;
		for (int i = 0; i < pattern.Count - 1; i++)
		{
			if (pattern[i].Equals(pattern[i + 1]))
			{
				value = (i + 1) * 100;
				for (int j = 1; i + j + 1 < pattern.Count && i - j >= 0; j++)
				{
					if (!pattern[i - j].Equals(pattern[i + j + 1]))
					{
						value = null;
						break;
					}

					if (i + j + 1 == pattern.Count - 1 || i == j)
					{
						break;
					}
				}

				if (value is not null)
				{
					return value;
				}
			}
		}

		return null;
	}

	private static int? IsMirroredVertically(List<string> pattern)
	{
		int? value = null;
		for (int i = 0; i < pattern[0].Length - 1; i++)
		{
			if (AreColumnsEqual(i, i + 1, pattern))
			{
				value = i + 1;
				for (int j = 1; i + j + 1 < pattern[0].Length && i - j >= 0; j++)
				{
					if (!AreColumnsEqual(i - j, i + j + 1, pattern))
					{
						value = null;
						break;
					}

					if (i + j + 1 == pattern[0].Length - 1 || i == j)
					{
						break;
					}
				}

				if (value is not null)
				{
					return value;
				}
			}
		}

		return null;
	}

	private static int? IsOffByOneMirroredHorizontally(List<string> pattern)
	{
		for (int i = 0; i < pattern.Count - 1; i++)
		{
			int? value = null;
			int errors = 0;
			if (AreRowsEqualTrackErrors(i, i + 1, pattern, ref errors))
			{
				value = (i + 1) * 100;
				for (int j = 1; i + j + 1 < pattern.Count && i - j >= 0; j++)
				{
					if (!AreRowsEqualTrackErrors(i - j, i + j + 1, pattern, ref errors) || errors > 1)
					{
						value = null;
						break;
					}

					if (i + j + 1 == pattern.Count - 1 || i == j)
					{
						break;
					}
				}

				if (value is not null && errors == 1)
				{
					return value;
				}
			}
		}

		return null;
	}

	private static int? IsOffByOneMirroredVertically(List<string> pattern)
	{
		for (int i = 0; i < pattern[0].Length - 1; i++)
		{
			int? value = null;
			int errors = 0;
			if (AreColumnsEqualTrackErrors(i, i + 1, pattern, ref errors))
			{
				value = i + 1;
				for (int j = 1; i + j + 1 < pattern[0].Length && i - j >= 0; j++)
				{
					if (!AreColumnsEqualTrackErrors(i - j, i + j + 1, pattern, ref errors) || errors > 1)
					{
						value = null;
						break;
					}

					if (i + j + 1 == pattern[0].Length - 1 || i == j)
					{
						break;
					}
				}

				if (value is not null && errors == 1)
				{
					return value;
				}
			}
		}

		return null;
	}

	private static bool AreColumnsEqual(int column1, int column2, List<string> pattern)
	{
		for (int i = 0; i < pattern.Count; i++)
		{
			if (pattern[i][column1] != pattern[i][column2])
			{
				return false;
			}
		}

		return true;
	}

	private static bool AreRowsEqualTrackErrors(int row1, int row2, List<string> pattern, ref int errors)
	{
		for (int i = 0; i < pattern[0].Length; i++)
		{
			if (pattern[row1][i] != pattern[row2][i])
			{
				errors++;
				if (errors > 1)
				{
					return false;
				}
			}
		}

		return true;
	}

	private static bool AreColumnsEqualTrackErrors(int column1, int column2, List<string> pattern, ref int errors)
	{
		for (int i = 0; i < pattern.Count; i++)
		{
			if (pattern[i][column1] != pattern[i][column2])
			{
				errors++;
				if (errors > 1)
				{
					return false;
				}
			}
		}

		return true;
	}
}