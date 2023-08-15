using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Snake
{
    public enum Direction
    {
        Down,
        Up,
        Right,
        Left
    }

    public class Snake
    {
        private readonly Queue<Piece> pieces = new Queue<Piece>();
        private readonly Brush color;

        public Snake(Brush color)
        {
            this.color = color;
            Reset();
        }

        public int HeadX => pieces.Last().X;
        public int HeadY => pieces.Last().Y;
        public int ScoreLength => pieces.Count - 2;
        public Direction Direction { get; set; }

        public void Draw(Graphics g) => pieces.ForEach(piece => piece.Draw(g));

        public bool CanEat(int a, int b, Piece food) => food.X == HeadX + a && food.Y == HeadY + b;

        public bool EatsItself() => pieces.SkipLast(1).Any(piece => HeadY == piece.Y && HeadX == piece.X);

        public bool Contains(int a, int b) => pieces.Any(piece => piece.X == a && piece.Y == b);

        public void Eat(Piece food) => pieces.Enqueue(new Piece(food.X, food.Y, color));

        public void Clear() => Reset();

        public void MoveTo(int a, int b)
        {
            pieces.Enqueue(new Piece(HeadX + a, HeadY + b, color));
            pieces.Dequeue();
        }

        private void Reset()
        {
            pieces.Clear();
            pieces.Enqueue(new Piece(0, 0, color));
            pieces.Enqueue(new Piece(0, 1, color));
            Direction = Direction.Down;
        }
    }
}

