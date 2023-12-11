internal class Program
{
	private static void Main(string[] args)
	{
		Part1();
		Part2();
	}

	private static void Part1()
	{
		int distance = 0;
		string[] lines = File.ReadAllLines("Input1.txt");
		int y, x = 0;
		for (y = 0; y < lines.Length; y++)
		{
			for (x = 0; x < lines[y].Length; x++)
			{
				if (lines[y][x] == 'S')
				{
					break;
				}
			}


			if (x < lines[y].Length && lines[y][x] == 'S')
			{
				break;
			}
		}

		Point startingPoint = new Point(x, y);

		Point? currentPoint1 = null;
		Point? currentPoint2 = null;
		if (y > 0 && lines[y - 1][x] is '|' or '7' or 'F')
		{
			currentPoint1 = new Point(x, y - 1);
		}

		if (x > 0 && lines[y][x - 1] is '-' or 'F' or 'L')
		{

			if (currentPoint1 is null)
			{
				currentPoint1 = new Point(x - 1, y);
			}
			else
			{
				currentPoint2 = new Point(x - 1, y);
			}
		}

		if (y < lines.Length - 1 && lines[y + 1][x] is '|' or 'L' or 'J')
		{
			if (currentPoint1 is null)
			{
				currentPoint1 = new Point(x, y + 1);
			}
			else
			{
				currentPoint2 = new Point(x, y + 1);
			}
		}

		if (x < lines[y].Length - 1 && lines[y][x + 1] is '-' or '7' or 'J')
		{
			currentPoint2 = new Point(x + 1, y);
		}

		if (currentPoint1 is null  || currentPoint2 is null)
		{
			throw new Exception("One of the points is null");
		}

		HashSet<Point> points = new HashSet<Point>();
		Vector vector1 = new(startingPoint, currentPoint1);
		Vector vector2 = new(startingPoint, currentPoint2);

		while (!points.Contains(vector1.End) && !points.Contains(vector2.End))
		{
			points.Add(vector1.End);
			points.Add(vector2.End);
			Point endPoint1 = GetNextPoint(vector1, lines[vector1.End.Y][vector1.End.X]) ?? throw new Exception("uh oh 1");
			Point endPoint2 = GetNextPoint(vector2, lines[vector2.End.Y][vector2.End.X]) ?? throw new Exception("uh oh 2");

			vector1 = new Vector(vector1.End, endPoint1);
			vector2 = new Vector(vector2.End, endPoint2);
			distance++;
		}

		Console.WriteLine($"Part 1: {distance}");
	}

	private static void Part2()
	{
		string[] lines = File.ReadAllLines("Input1.txt");
		char[][] mapping = new char[lines.Length][];

		Point startingPoint = null;
		for (int y = 0; y < lines.Length; y++)
		{
			mapping[y] = new char[lines[y].Length];
			for (int x = 0; x < lines[y].Length; x++)
			{
				if (lines[y][x] == 'S')
				{
					startingPoint = new Point(x, y);
				}
			}
		}

		Point? currentPoint1 = null;
		Point? currentPoint2 = null;
		if (startingPoint.Y > 0 && lines[startingPoint.Y - 1][startingPoint.X] is '|' or '7' or 'F')
		{
			currentPoint1 = new Point(startingPoint.X, startingPoint.Y - 1);
		}

		if (startingPoint.X > 0 && lines[startingPoint.Y][startingPoint.X - 1] is '-' or 'F' or 'L')
		{

			if (currentPoint1 is null)
			{
				currentPoint1 = new Point(startingPoint.X - 1, startingPoint.Y);
			}
			else
			{
				currentPoint2 = new Point(startingPoint.X - 1, startingPoint.Y);
			}
		}

		if (startingPoint.Y < lines.Length - 1 && lines[startingPoint.Y + 1][startingPoint.X] is '|' or 'L' or 'J')
		{
			if (currentPoint1 is null)
			{
				currentPoint1 = new Point(startingPoint.X, startingPoint.Y + 1);
			}
			else
			{
				currentPoint2 = new Point(startingPoint.X, startingPoint.Y + 1);
			}
		}

		if (startingPoint.X < lines[startingPoint.Y].Length - 1 && lines[startingPoint.Y][startingPoint.X + 1] is '-' or '7' or 'J')
		{
			currentPoint2 = new Point(startingPoint.X + 1, startingPoint.Y);
		}

		if (currentPoint1 is null || currentPoint2 is null)
		{
			throw new Exception("One of the points is null");
		}

		mapping[currentPoint1.Y][currentPoint1.X] = lines[currentPoint1.Y][currentPoint1.X];
		mapping[currentPoint2.Y][currentPoint2.X] = lines[currentPoint2.Y][currentPoint2.X];

		// If S is in between two -'s or |'s, set it to be that. Otherwise, leave it as S.
		mapping[startingPoint.Y][startingPoint.X] = mapping[currentPoint1.Y][currentPoint1.X] == mapping[currentPoint2.Y][currentPoint2.X] ? mapping[currentPoint1.Y][currentPoint1.X] : lines[startingPoint.Y][startingPoint.X];

		HashSet<Point> points = new HashSet<Point>();
		Vector vector1 = new(startingPoint, currentPoint1);
		Vector vector2 = new(startingPoint, currentPoint2);

		while (!points.Contains(vector1.End) && !points.Contains(vector2.End))
		{
			points.Add(vector1.End);
			points.Add(vector2.End);
			Point endPoint1 = GetNextPoint(vector1, lines[vector1.End.Y][vector1.End.X]) ?? throw new Exception("uh oh 1");
			Point endPoint2 = GetNextPoint(vector2, lines[vector2.End.Y][vector2.End.X]) ?? throw new Exception("uh oh 2");

			mapping[endPoint1.Y][endPoint1.X] = lines[endPoint1.Y][endPoint1.X];
			mapping[endPoint2.Y][endPoint2.X] = lines[endPoint2.Y][endPoint2.X];

			vector1 = new Vector(vector1.End, endPoint1);
			vector2 = new Vector(vector2.End, endPoint2);
		}

		int area = 0;
		for (int y = 0; y < lines.Length; y++)
		{
			bool inside = false;
			bool corner = false;
			char previousCorner = '\0';
			for (int x = 0; x < lines[y].Length; x++)
			{
				if (mapping[y][x] == '|')
				{
					inside = !inside;
					continue;
				}

				if (mapping[y][x] != '\0' && mapping[y][x] != '-')
				{
					if (!corner)
					{
						previousCorner = mapping[y][x];
						corner = true;
					}
					else
					{
						corner = false;

						if (previousCorner is '7' or 'L' && mapping[y][x] is '7' or 'L' ||
							previousCorner is 'J' or 'F' && mapping[y][x] is 'J' or 'F')
						{
							inside = !inside;
						}
					}

					continue;
				}

				if (inside && mapping[y][x] == '\0')
				{
					area++;
				}
			}
		}

		Console.WriteLine($"Part 2: {area}");
	}

	private static Point? GetNextPoint(Vector vector, char endChar)
	{
		int xDiff = vector.End.X - vector.Starting.X;
		int yDiff = vector.End.Y - vector.Starting.Y;

		switch (endChar)
		{
			case '-':
				return xDiff != 0 ? new Point(vector.End.X + xDiff, vector.End.Y) : null;

			case '|':
				return yDiff != 0 ? new Point(vector.End.X, vector.End.Y + yDiff) : null;

			case '7':
				if (xDiff == 1)
				{
					return new Point(vector.End.X, vector.End.Y + 1);
				}

				if (yDiff == -1)
				{
					return new Point(vector.End.X - 1, vector.End.Y);
				}

				return null;

			case 'J':
				if (xDiff == 1)
				{
					return new Point(vector.End.X, vector.End.Y - 1);
				}

				if (yDiff == 1)
				{
					return new Point(vector.End.X - 1, vector.End.Y);
				}

				return null;

			case 'L':
				if (xDiff == -1)
				{
					return new Point(vector.End.X, vector.End.Y - 1);
				}

				if (yDiff == 1)
				{
					return new Point(vector.End.X + 1, vector.End.Y);
				}

				return null;

			case 'F':
				if (xDiff == -1)
				{
					return new Point(vector.End.X, vector.End.Y + 1);
				}

				if (yDiff == -1)
				{
					return new Point(vector.End.X + 1, vector.End.Y);
				}

				return null;

			default:
				throw new Exception("Character not matched");
		}
	}

	private record Point(int X, int Y);

	private record Vector(Point Starting, Point End);
}