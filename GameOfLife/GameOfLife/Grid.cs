using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameOfLife
{
    class Grid
    {

        private int SizeX;
        private int SizeY;
        private Cell[,] cells;
        private Cell[,] nextGenerationCells;
        private static Random rnd;
        private Canvas drawCanvas;
        private Ellipse[,] cellsVisuals;

        public Grid(Canvas c)
        {
            drawCanvas = c;
            rnd = new Random();
            SizeX = (int) (c.Width / 5);
            SizeY = (int)(c.Height / 5);
            cells = new Cell[SizeX, SizeY];
            nextGenerationCells = new Cell[SizeX, SizeY];
            cellsVisuals = new Ellipse[SizeX, SizeY];
 
            for (var i = 0; i < SizeX; i++)
                for (var j = 0; j < SizeY; j++)
                {
                    cells[i, j] = new Cell(i, j, 0, false);
                    nextGenerationCells[i, j] = new Cell(i, j, 0, false);
                }

            InitCellsVisuals();
        }

        public void Clear()
        {
            for (var i = 0; i < SizeX; i++)
                for (var j = 0; j < SizeY; j++)
                {
                    cells[i, j].Age = 0;
                    cells[i, j].IsAlive = false;

                    nextGenerationCells[i, j].Age = 0;
                    nextGenerationCells[i, j].IsAlive = false;
                    cellsVisuals[i, j].Fill = Brushes.Gray;
                }
        }


        void MouseMove(object sender, MouseEventArgs e)
        {
            var cellVisual = sender as Ellipse;
            
            var i = (int) cellVisual.Margin.Left / 5;
            var j = (int) cellVisual.Margin.Top / 5;
            

            if (e.LeftButton == MouseButtonState.Pressed && !cells[i, j].IsAlive)
            {
                cells[i, j].IsAlive = true;
                cells[i, j].Age = 0;
                cellVisual.Fill = Brushes.White;
            }
        }

        public void UpdateGraphics()
        {
            for (var i = 0; i < SizeX; i++)
                for (var j = 0; j < SizeY; j++)
                    cellsVisuals[i, j].Fill = cells[i, j].IsAlive
                                                  ? (cells[i, j].Age < 2 ? Brushes.White : Brushes.DarkGray)
                                                  : Brushes.Gray;
        }

        public void InitCellsVisuals()
        {
            for (var i = 0; i < SizeX; i++)
                for (var j = 0; j < SizeY; j++)
                {
                    cells[i, j].IsAlive = GetRandomBoolean();
                    var left = cells[i, j].PositionX;
                    var top = cells[i, j].PositionY;
                    cellsVisuals[i, j] = new Ellipse()
                    {
                        Width = 5,
                        Height = 5,
                        Margin = new Thickness(left, top, 0, 0),
                        Fill = Brushes.Gray
                    };
                    cellsVisuals[i, j].MouseMove += MouseMove;
                    cellsVisuals[i, j].MouseLeftButtonDown += MouseMove;

                    drawCanvas.Children.Add(cellsVisuals[i, j]);
                }

            UpdateGraphics();
        }
        

        public static bool GetRandomBoolean()
        {
            return rnd.NextDouble() > 0.8;
        }
        
        public void UpdateToNextGeneration()
        {
            for (var i = 0; i < SizeX; i++)
                for (var j = 0; j < SizeY; j++)
                {
                    cells[i, j].IsAlive = nextGenerationCells[i, j].IsAlive;
                    cells[i, j].Age = nextGenerationCells[i, j].Age;
                }

            UpdateGraphics();
        }
        

        public void Update()
        {
            var alive = false;
            var age = 0;

            for (var i = 0; i < SizeX; i++)
            {
                for (var j = 0; j < SizeY; j++)
                {
                    CalculateNextGeneration(i, j, ref alive, ref age);   // OPTIMIZED
                    nextGenerationCells[i, j].IsAlive = alive;  // OPTIMIZED
                    nextGenerationCells[i, j].Age = age;  // OPTIMIZED
                }
            }
            UpdateToNextGeneration();
        }

        public void CalculateNextGeneration(int row, int column, ref bool isAlive, ref int age)     // OPTIMIZED
        {
            isAlive = cells[row, column].IsAlive;
            age = cells[row, column].Age;

            var count = CountNeighbors(row, column);

            if (isAlive && count < 2)
            {
                isAlive = false;
                age = 0;
                return;
            }

            if (isAlive && (count == 2 || count == 3))
            {
                cells[row, column].Age++;
                isAlive = true;
                age = cells[row, column].Age;
                return;
            }

            if (isAlive && count > 3)
            {
                isAlive = false;
                age = 0;
                return;
            }

            if (!isAlive && count == 3)
            {
                isAlive = true;
                age = 0;
            }
        }

        public int CountNeighbors(int i, int j)
        {
            var count = 0;

            if (i != SizeX - 1 && cells[i + 1, j].IsAlive) count++;
            if (i != SizeX - 1 && j != SizeY - 1 && cells[i + 1, j + 1].IsAlive) count++;
            if (j != SizeY - 1 && cells[i, j + 1].IsAlive) count++;
            if (i != 0 && j != SizeY - 1 && cells[i - 1, j + 1].IsAlive) count++;
            if (i != 0 && cells[i - 1, j].IsAlive) count++;
            if (i != 0 && j != 0 && cells[i - 1, j - 1].IsAlive) count++;
            if (j != 0 && cells[i, j - 1].IsAlive) count++;
            if (i != SizeX - 1 && j != 0 && cells[i + 1, j - 1].IsAlive) count++;

            return count;
        }
    }
}