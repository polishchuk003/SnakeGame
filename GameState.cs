using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class GameState
    {
        public int Rows { get; }
        public int Columns { get; }

        public GridValue[,] Grid { get; }
        public Direction Dir { get; private set; }
        public int Score { get; private set; }
        public bool GameOver { get; private set; }

        private readonly LinkedList<Position> _snakePosition = new LinkedList<Position>();
        private readonly Random _random = new Random();

        public GameState(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Grid = new GridValue[rows, columns];
            Dir = Direction.Right;

            AddSnake();
            AddFood();

        }

        private void AddSnake()
        {
            int r = Rows / 2;

            for (int i = 1; i <= 3; i++)
            {
                Grid[r, i] = GridValue.Snake;
                _snakePosition.AddFirst(new Position(r, i));
            }
        }

        private IEnumerable<Position> EmptyPositios()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (Grid[i, j] == GridValue.Empty)
                    {
                        yield return new Position(i, j);
                    }
                }
            }
        }

        private void AddFood()
        {
            List<Position> empty = new List<Position>(EmptyPositios());

            if (empty.Count == 0)
            {
                return;
            }

            Position pos = empty[_random.Next(empty.Count)];
            Grid[pos.Row, pos.Column] = GridValue.Food;
        }

        public Position HeadPosition()
        {
            return _snakePosition.First.Value;
        }

        public Position TailPosition()
        {
            return _snakePosition.Last.Value;
        }

        public IEnumerable<Position> SnakePositions()
        {
            return _snakePosition;
        }

        private void AddHead(Position pos)
        {
            _snakePosition.AddFirst(pos);
            Grid[pos.Row, pos.Column] = GridValue.Snake;
        }

        private void RemoveTail()
        {
            Position tail = _snakePosition.Last.Value;
            Grid[tail.Row, tail.Column] = GridValue.Empty;
            _snakePosition.RemoveLast();
        }

        public void ChangeDirection(Direction direction)
        {
            Dir = direction;
        }

        private bool OutsideGrid(Position pos)
        {
            return pos.Row < 0 || pos.Row >= Rows || pos.Column < 0 || pos.Column >= Columns;
        }

        private GridValue WillHit(Position newHeadPos)
        {
            if (OutsideGrid(newHeadPos))
            {
                return GridValue.Outside;
            }

            return Grid[newHeadPos.Row, newHeadPos.Column];
        }

    }
}
