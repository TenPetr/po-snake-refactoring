using System;

namespace SnakeGame
{
    public enum MoveDirection
    {
        Up,
        Down,
        Right,
        Left
    }

    public struct Point
    {
        public int Row;
        public int Column;
    }

    public struct GamePixel
    {
        public Point Location;
        public ConsoleColor PixelColor;
    }
}