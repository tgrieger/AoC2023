using System.Runtime.InteropServices;

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
		char[][] chars = new char[lines.Length][];
		for (int i = 0; i < lines.Length; i++)
		{
			chars[i] = lines[i].ToCharArray();
		}

		int sum = 0;
		for (int i = 0; i < chars.Length; i++)
		{
			for (int j = 0; j < chars[i].Length; j++)
			{
				bool numberFound = false;
				int start = j;
				while (j < chars[i].Length && IsNumber(chars[i][j]))
				{
					j++;
					numberFound = true;
				}

				if (numberFound && IsSymbolAdjacent(chars, i, start, j - 1))
				{
					int localSum = 0;
					for (int n = 0; n < j - start; n++)
					{
						localSum *= 10;
						localSum += chars[i][n + start] - '0';
					}

					sum += localSum;
				}
			}
		}

		Console.WriteLine($"Part 1: {sum}");
	}

	private static void Part2()
	{
		string[] lines = File.ReadAllLines("Input1.txt");
		char[][] chars = new char[lines.Length][];
		for (int i = 0; i < lines.Length; i++)
		{
			chars[i] = lines[i].ToCharArray();
		}

		int sum = 0;
		for (int i = 0; i < chars.Length; i++)
		{
			for (int j = 0; j < chars[i].Length; j++)
			{
				if (chars[i][j] != '*')
				{
					continue;
				}

				List<int> adjacentNumbers = GetAdjacentNumbers(chars, i, j);
				if (adjacentNumbers.Count != 2)
				{
					continue;
				}

				sum += adjacentNumbers[0] * adjacentNumbers[1];
			}
		}

		Console.WriteLine($"Part 2: {sum}");
	}

	private static bool IsNumber(char c)
	{
		return c >= '0' && c <= '9';
	}

	private static bool IsSymbol(char c)
	{
		return !IsNumber(c) && c != '.';
	}

	private static bool IsSymbolAdjacent(char[][] chars, int column, int start, int end)
	{
		int symbolStart = Math.Max(start - 1, 0);
		int symbolEnd = Math.Min(end + 1, chars[column].Length - 1);
		if (column > 0)
		{
			for (int i = symbolStart; i <= symbolEnd; i++)
			{
				if (IsSymbol(chars[column - 1][i]))
				{
					return true;
				}
			}
		}

		if (column < chars.Length - 1)
		{
			for (int i = symbolStart; i <= symbolEnd; i++)
			{
				if (IsSymbol(chars[column + 1][i]))
				{
					return true;
				}
			}
		}

		return IsSymbol(chars[column][symbolStart]) || IsSymbol(chars[column][symbolEnd]);
	}

	private static List<int> GetAdjacentNumbers(char[][] chars, int i, int j)
	{
		List<int> result = new();
		int start = -1;
		int end = -1;

		// Top Left
		if (i > 0 && j > 0 && IsNumber(chars[i - 1][j - 1]))
		{
			result.Add(GetNumber(chars, i - 1, j - 1, out start, out end));
		}

		// Top Middle
		if (end < j && i > 0 && IsNumber(chars[i - 1][j]))
		{
			result.Add(GetNumber(chars, i - 1, j, out start, out end));
		}

		// Top Right
		if (end < j + 1 && i > 0 && j < chars[i].Length - 1 && IsNumber(chars[i - 1][j + 1]))
		{
			result.Add(GetNumber(chars, i - 1, j + 1, out _, out _));
		}

		// Left
		if (j > 0 && IsNumber(chars[i][j - 1]))
		{
			result.Add(GetNumber(chars, i, j - 1, out _, out _));
		}

		// Right
		if (j < chars[i].Length - 1 && IsNumber(chars[i][j + 1]))
		{
			result.Add(GetNumber(chars, i, j + 1, out _, out _));
		}

		start = -1;
		end = -1;

		// Bottom Left
		if (i < chars.Length - 1 && j > 0 && IsNumber(chars[i + 1][j - 1]))
		{
			result.Add(GetNumber(chars, i + 1, j - 1, out start, out end));
		}

		// Bottom Middle
		if (end < j && i < chars.Length - 1 && IsNumber(chars[i + 1][j]))
		{
			result.Add(GetNumber(chars, i + 1, j, out start, out end));
		}

		// Top Right
		if (end < j + 1 && i < chars.Length - 1 && j < chars[i].Length - 1 && IsNumber(chars[i + 1][j + 1]))
		{
			result.Add(GetNumber(chars, i + 1, j + 1, out _, out _));
		}

		return result;
	}

	private static int GetNumber(char[][] chars, int i, int j, out int start, out int end)
	{
		int cursor = j - 1;
		while (cursor >= 0 && IsNumber(chars[i][cursor]))
		{
			cursor--;
		}

		start = cursor + 1;
		cursor = j;
		while (cursor < chars[i].Length && IsNumber(chars[i][cursor]))
		{
			cursor++;
		}

		end = cursor;

		int localSum = 0;
		for (int n = 0; n < end - start; n++)
		{
			localSum *= 10;
			localSum += chars[i][n + start] - '0';
		}

		return localSum;
	}
}