using System;
using System.Text;

namespace Minesweeper
{
  class Minesweeper
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Hello! Welcome to  Dup's Minesweeper");
      Console.WriteLine("Please introduce number of columns:");

      try
      {
        int x;
        if (!Int32.TryParse(Console.ReadLine(), out x))
        {
          throw new Exception();
        }

        Console.WriteLine("Please introduce number of lines:");
        int y;
        if (!Int32.TryParse(Console.ReadLine(), out y))
        {
          throw new Exception();
        }

        Console.WriteLine("Please introduce percentil of mines (from 0 to 100):");
        int mines;
        if (!Int32.TryParse(Console.ReadLine(), out mines))
        {
          throw new Exception();
        }
        Board board = new Board(x, y, mines);

        do
        {
          Console.WriteLine("This is your board:");
          Console.Write(board.showBoard());

          Console.WriteLine("Please introduce cell to check: Format x,y");
          string[] cell = Console.ReadLine().Split(',');
          x = Convert.ToInt32(cell[0]);
          y = Convert.ToInt32(cell[1]);
        }
        while (board.checkMine(x, y));

        Console.WriteLine("Game ended. This is your final board:");
        Console.Write(board.showBoard());
      }
      catch (Exception ex)
      {
        Console.Error.WriteLine("You must introduce a number.");
      }
    }
  }


  class Cell
  {
    private readonly int x;
    private readonly int y;

    public Cell(int x, int y)
    {
      this.x = x;
      this.y = y;
    }

    public bool Discovered { get; set; }
    public int NTouchingMines { get; set; }
    public bool IsMine { get; set; }
  }

  class Board
  {
    int x;
    int y;
    int mines;
    Cell[,] cells;

    public Board(int x, int y, int percentOfMines)
    {
      this.x = x;
      this.y = y;
      double mines = x * y * percentOfMines / 100;
      this.mines = Convert.ToInt32(Math.Round(mines));

      cells = new Cell[x, y];

      for (int i = 0; i < x; i++)
        for (int j = 0; j < y; j++)
          cells[i, j] = new Cell(i, j);


      int plantedMines = 0;
      Random random = new Random();

      while (plantedMines <= mines)
      {
        int i = random.Next(0, x);
        int j = random.Next(0, y);
        if (!cells[i, j].IsMine)
        {
          cells[i, j].IsMine = true;
          plantedMines++;
          if (i == 0)
            cells[i + 1, j].NTouchingMines++;
          else if (i == x - 1)
            cells[i - 1, j].NTouchingMines++;
          else
          {
            cells[i + 1, j].NTouchingMines++;
            cells[i - 1, j].NTouchingMines++;
          }

          if (j == 0)
            cells[i, j + 1].NTouchingMines++;
          else if (j == y - 1)
            cells[i, j - 1].NTouchingMines++;
          else
          {
            cells[i, j + 1].NTouchingMines++;
            cells[i, j - 1].NTouchingMines++;
          }
        }
      }
    }

    public bool checkMine(int x, int y)
    {
      bool result; //Returns true if the game continue or false if it ends.
      if (cells[x, y].Discovered)
        result = true;
      else
      {
        if (cells[x, y].IsMine)
        {
          cells[x, y].Discovered = true;
          result = false;
        }
        else
        {
          result = true;
          if (cells[x, y].NTouchingMines == 0)
            DiscoverEmptyCells(x, y);
          else
            cells[x, y].Discovered = true;
        }
      }
      return result;
    }

    private void DiscoverEmptyCells(int x, int y)
    {
      cells[x, y].Discovered = !cells[x, y].IsMine;
      if (x == 0)
      {
        if (cells[x + 1, y].NTouchingMines == 0 && !cells[x + 1, y].IsMine && !cells[x + 1, y].Discovered)
          DiscoverEmptyCells(x + 1, y);
        cells[x + 1, y].Discovered = !cells[x + 1, y].IsMine;
      }
      else if (x == this.x - 1)
      {
        if (cells[x - 1, y].NTouchingMines == 0 && !cells[x - 1, y].IsMine && !cells[x - 1, y].Discovered)
          DiscoverEmptyCells(x - 1, y);
        cells[x - 1, y].Discovered = !cells[x - 1, y].IsMine;
      }
      else
      {
        if (cells[x - 1, y].NTouchingMines == 0 && !cells[x - 1, y].IsMine && !cells[x - 1, y].Discovered)
          DiscoverEmptyCells(x - 1, y);
        cells[x - 1, y].Discovered = !cells[x - 1, y].IsMine;

        if (cells[x + 1, y].NTouchingMines == 0 && !cells[x + 1, y].IsMine && !cells[x + 1, y].Discovered)
          DiscoverEmptyCells(x + 1, y);
        cells[x + 1, y].Discovered = !cells[x + 1, y].IsMine;
      }

      if (y == 0)
      {
        if (cells[x, y + 1].NTouchingMines == 0 && !cells[x, y + 1].IsMine && !cells[x, y + 1].Discovered)
          DiscoverEmptyCells(x, y + 1);
        cells[x, y + 1].Discovered = !cells[x, y + 1].IsMine;
      }
      else if (y == this.y - 1)
      {
        if (cells[x, y - 1].NTouchingMines == 0 && !cells[x, y - 1].IsMine && !cells[x, y - 1].Discovered)
          DiscoverEmptyCells(x, y - 1);
        cells[x, y - 1].Discovered = !cells[x, y - 1].IsMine;
      }
      else
      {
        if (cells[x, y - 1].NTouchingMines == 0 && !cells[x, y - 1].IsMine && !cells[x, y - 1].Discovered)
          DiscoverEmptyCells(x, y - 1);
        cells[x, y - 1].Discovered = !cells[x, y - 1].IsMine;

        if (cells[x, y + 1].NTouchingMines == 0 && !cells[x, y + 1].IsMine && !cells[x, y + 1].Discovered)
          DiscoverEmptyCells(x, y + 1);
        cells[x, y + 1].Discovered = !cells[x, y + 1].IsMine;
      }
    }

    public string showBoard()
    {
      StringBuilder sb = new StringBuilder();

      for (int i = 0; i < x; i++)
      {
        string line = "";
        for (int j = 0; j < y; j++)
        {
          if (!cells[i, j].Discovered)
            line += " _ |";
          else
          {
            if (cells[i, j].IsMine)
              line += " X |";
            else
              line += $" {cells[i, j].NTouchingMines} |";
          }
        }
        sb.AppendLine(line);
      }

      return sb.ToString();
    }
  }
}
