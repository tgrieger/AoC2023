using System.Diagnostics.CodeAnalysis;

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

		for (int i = 0; i < lines[0].Length; i++)
		{
			int value = lines.Length;
			for (int j = 0; j < lines.Length; j++)
			{
				if (lines[j][i] == 'O')
				{
					sum += value;
					value--;
				}
				else if (lines[j][i] == '#')
				{
					value = lines.Length - j - 1;
				}
			}
		}

		Console.WriteLine($"Part 1: {sum}");
	}

	private static void Part2()
	{
		string[] lines = File.ReadAllLines("Input.txt");
		Dish dish = new(lines);
		Dictionary<Dish, ulong> cache = new(new DishComparer());

		ulong i;
		ulong diff = 0;
		HashSet<int> hs = new();
		for (i = 0; i < 1000000000; i++)
		{
			dish.MoveNorth();
			dish.MoveWest();
			dish.MoveSouth();
			dish.MoveEast();

			if (!cache.TryAdd((Dish)dish.Clone(), i))
			{
				diff = i - cache[dish];
				break;
			}
		}

		ulong multiplier = (1000000000 - i) / diff;
		ulong newI = i + (multiplier * diff) + 1;
		for (i = newI; i < 1000000000; i++)
		{
			dish.MoveNorth();
			dish.MoveWest();
			dish.MoveSouth();
			dish.MoveEast();
		}

		Console.WriteLine($"Part 2: {dish.CalculateLoad()}");
	}

	private class DishComparer : IEqualityComparer<Dish>
	{
		public bool Equals(Dish? x, Dish? y)
		{
			for (int i = 0; i < x.Rows.Length; i++)
			{
				for (int j = 0; j < x.Rows[i].Length; j++)
				{
					if (x.Rows[i][j] != y.Rows[i][j])
					{
						return false;
					}
				}
			}

			return true;
		}

		public int GetHashCode([DisallowNull] Dish obj)
		{
			int hash = 0;
			for (int i = 0; i < obj.Rows.Length; i++)
			{
				hash += obj.Rows[i][^1].GetHashCode() * (i + 1);
			}

			return hash;
		}
	}

	private class Dish : ICloneable
	{
		public Dish(string[] lines)
		{
			Rows = new char[lines.Length][];
			for (int i = 0; i < lines.Length; i++)
			{
				Rows[i] = lines[i].ToCharArray();
			}
		}

		private Dish(char[][] dish)
		{
			Rows = new char[dish.Length][];
			for (int i = 0;i < dish.Length; i++)
			{
				Rows[i] = new char[dish[i].Length];
				for (int j = 0; j < dish[i].Length; j++)
				{
					Rows[i][j] = dish[i][j];
				}
			}
		}

		public char[][] Rows { get; }

		public int CalculateLoad()
		{
			int sum = 0;
			for (int i = 0; i < Rows.Length; i++)
			{
				for (int j = 0; j < Rows[i].Length; j++)
				{
					if (Rows[i][j] == 'O')
					{
						sum += Rows.Length - i;
					}
				}
			}

			return sum;
		}

		public void MoveNorth()
		{
			for (int i = 0; i < Rows[0].Length; i++)
			{
				int highestSpot = 0;
				for (int j = 0; j < Rows.Length; j++)
				{
					if (Rows[j][i] == 'O')
					{
						if (highestSpot != j)
						{
							Rows[highestSpot][i] = 'O';
							Rows[j][i] = '.';
						}

						highestSpot++;
					}
					else if (Rows[j][i] == '#')
					{
						highestSpot = j + 1;
					}
				}
			}
		}

		public void MoveWest()
		{
			for (int i = 0; i < Rows.Length; i++)
			{
				int highestSpot = 0;
				for (int j = 0; j < Rows[i].Length; j++)
				{
					char c = Rows[i][j];
					if (c == 'O')
					{
						if (highestSpot != j)
						{
							Rows[i][highestSpot] = 'O';
							Rows[i][j] = '.';
						}

						highestSpot++;
					}
					else if (c == '#')
					{
						highestSpot = j + 1;
					}
				}
			}
		}

		public void MoveSouth()
		{
			for (int i = Rows[0].Length - 1; i >= 0; i--)
			{
				int highestSpot = Rows[0].Length - 1;
				for (int j = Rows.Length - 1; j >= 0; j--)
				{
					if (Rows[j][i] == 'O')
					{
						if (highestSpot != j)
						{
							Rows[highestSpot][i] = 'O';
							Rows[j][i] = '.';
						}

						highestSpot--;
					}
					else if (Rows[j][i] == '#')
					{
						highestSpot = j - 1;
					}
				}
			}
		}

		public void MoveEast()
		{
			for (int i = Rows.Length - 1; i >= 0; i--)
			{
				int highestSpot = Rows.Length - 1;
				for (int j = Rows[i].Length - 1; j >= 0; j--)
				{
					char c = Rows[i][j];
					if (c == 'O')
					{
						if (highestSpot != j)
						{
							Rows[i][highestSpot] = 'O';
							Rows[i][j] = '.';
						}

						highestSpot--;
					}
					else if (c == '#')
					{
						highestSpot = j - 1;
					}
				}
			}
		}

		public object Clone()
		{
			return new Dish(Rows);
		}
	}
}