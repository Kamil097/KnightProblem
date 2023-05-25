using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

Console.WriteLine("Podaj rozmiar szachownicy.");
string input = Console.ReadLine();
int n = validateInput(input);
if (n < 5)
{
    Console.WriteLine("Szachownica musi miec rozmiar przynajmniej 5 x 5");
    Environment.Exit(0);
}

Console.WriteLine("Podaj numer rzedu:");
input = Console.ReadLine();
int x = validateInput(input) - 1;


Console.WriteLine("Podaj numer kolumny:");
input = Console.ReadLine();
int y = validateInput(input) - 1;


List<(int, int)> knightPath = KnightPathFinder.FindKnightPath(n, x, y);

if (knightPath.Count == n * n)
{
    Console.WriteLine($"Sciezka skoczka dla szachownicy: {n}x{n}:");
    KnightPathFinder.showMatrix(n, knightPath);
    if (KnightPathFinder.openOrClose(x, y, knightPath.Last().Item1, knightPath.Last().Item2))
        Console.WriteLine("Sciezka zamknieta."); // np 6x6 3 / 3
    else
        Console.WriteLine("Sciezka otwarta.");

    Console.WriteLine("Czy chcesz wyswietlic pelna sciezke?: T / N");

    input = Console.ReadLine();

    if (input.ToUpper() == "T")
    {
        Console.WriteLine("Pelna sciezka:");
        foreach (var sequence in knightPath)
            Console.WriteLine($"[{sequence.Item1 + 1}][{sequence.Item2 + 1}]");
    }
}
else
{
    Console.Clear();
    Console.WriteLine($"Nie znaleziono sciezki dla szachownicy: {n}x{n}");
    Console.WriteLine($"Oraz poczatkowym indeksie: [{x + 1}][{y + 1}]");
}
int validateInput(string x)
{
    int output = 0;
    try
    {
        output = int.Parse(x);
    }
    catch (FormatException)
    {
        Console.WriteLine("Zostala wprowadzona zla wartosc.");
        Environment.Exit(0);
    }
    return output;
}


class KnightPathFinder
{
    private static readonly int[] RowOffsets = { -2, -1, 1, 2, 2, 1, -1, -2 };
    private static readonly int[] ColOffsets = { 1, 2, 2, 1, -1, -2, -2, -1 };

    public static List<(int, int)> FindKnightPath(int n, int row, int col)
    {
        if (!(row >= 0 && row < n && col >= 0 && col < n))
        {
            Console.WriteLine("Bledne dane!");
            Environment.Exit(0);
        }

        int[][] chessboard = new int[n][];
        for (int i = 0; i < n; i++)
        {
            chessboard[i] = new int[n];
        }
        int moveNumber = 1;

        List<(int, int)> path = new List<(int, int)>();

        FindKnightPathRecursive(chessboard, row, col, moveNumber, path);

        return path;
    }

    private static bool FindKnightPathRecursive(int[][] chessboard, int row, int col, int moveNumber, List<(int, int)> path)
    {
        int n = chessboard.Length;

        chessboard[row][col] = moveNumber;
        path.Add((row, col));

        if (moveNumber == n * n)
        {
            return true;
        }

        for (int i = 0; i < 8; i++)
        {
            int nextRow = row + RowOffsets[i];
            int nextCol = col + ColOffsets[i];

            if (IsValidMove(chessboard, nextRow, nextCol))
            {
                if (FindKnightPathRecursive(chessboard, nextRow, nextCol, moveNumber + 1, path))
                {
                    return true;
                }
            }
        }
        chessboard[row][col] = 0;
        path.RemoveAt(path.Count - 1);

        return false;
    }

    private static bool IsValidMove(int[][] chessboard, int row, int col)
    {
        int n = chessboard.Length;
        return row >= 0 && row < n && col >= 0 && col < n && chessboard[row][col] == 0;
    }

    public static bool openOrClose(int lastx, int lasty, int startx, int starty)
    {
        for (int i = 0; i < 8; i++)
        {
            int nextRow = lastx + RowOffsets[i];
            int nextCol = lasty + ColOffsets[i];
            if (nextRow == startx && nextCol == starty)
                return true;
        }
        return false;
    }

    public static void showMatrix(int n, List<(int, int)> knightPath)
    {
        char[][] matrix = new char[n][];
        for (int i = 0; i < n; i++)
        {
            matrix[i] = Enumerable.Repeat('0', n).ToArray();
        }

        foreach (var sequence in knightPath)
        {
            Console.Clear();
            matrix[sequence.Item1][sequence.Item2] = '#';

            Console.Write("    ");
            for (int i = 0; i < n; i++)
                Console.Write(i + 1 + "   ");
            Console.Write('\n');

            for (int i = 0; i < n; i++)
            {
                Console.Write(i + 1 + " |");
                for (int j = 0; j < n; j++)
                {
                    if (i == knightPath.First().Item1 && j == knightPath.First().Item2)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(" " + matrix[i][j]);
                        Console.ResetColor();
                        Console.Write(" |");
                    }
                    else if (i == sequence.Item1 && j == sequence.Item2)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" " + matrix[i][j]);
                        Console.ResetColor();
                        Console.Write(" |");
                    }
                    else if (matrix[i][j] == '#')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" " + matrix[i][j]);
                        Console.ResetColor();
                        Console.Write(" |");
                    }
                    else
                        Console.Write(" " + matrix[i][j] + " |");
                }
                Console.Write("\n\n");
            }

            Console.Write("\n");
            Console.WriteLine($"Aktualne pole: [{sequence.Item1 + 1}][{sequence.Item2 + 1}]");
            Thread.Sleep(1000);
        }
    }
}



