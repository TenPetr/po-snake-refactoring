using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SnakeGame
{
    public class GameCore
    {
        private const int GameWidth = 32;
        private const int GameHeight = 16;
        private const char GamePixel = '■';
        private const int GameSpeed = 200;

        private Random random = new Random();

        private GamePixel snakeHead;
        private GamePixel fruit;
        private List<Point> snakeBody = new List<Point>();
        private MoveDirection currentDirection = MoveDirection.Left;

        private int score = 5;
        private bool isGameOver = false;

        public void StartGame()
        {
            SetupGame();
            RunGameLoop();
            DisplayGameOver();
        }

        private void SetupGame()
        {
            Console.WindowWidth = GameWidth;
            Console.WindowHeight = GameHeight;

            snakeHead = new GamePixel
            {
                Location = new Point { Row = GameHeight / 2, Column = GameWidth / 2 },
                PixelColor = ConsoleColor.Red
            };

            PlaceFruit();
        }

        private void RunGameLoop()
        {
            while (!isGameOver)
            {
                ProcessInput();
                UpdateGame();
                RenderScreen();
                Thread.Sleep(GameSpeed);
            }
        }

        private void DisplayGameOver()
        {
            Console.SetCursorPosition(GameWidth / 5, GameHeight / 2);
            Console.WriteLine($"Game Over! Score: {score}");
        }

        private void ProcessInput()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        currentDirection = currentDirection != MoveDirection.Down ? MoveDirection.Up : currentDirection;
                        break;
                    case ConsoleKey.DownArrow:
                        currentDirection = currentDirection != MoveDirection.Up ? MoveDirection.Down : currentDirection;
                        break;
                    case ConsoleKey.LeftArrow:
                        currentDirection = currentDirection != MoveDirection.Left ? MoveDirection.Right : currentDirection;
                        break;
                    case ConsoleKey.RightArrow:
                        currentDirection = currentDirection != MoveDirection.Right ? MoveDirection.Left : currentDirection;
                        break;
                }
            }
        }

        private void UpdateGame()
        {
            if (snakeHead.Location.Column == GameWidth - 1 || snakeHead.Location.Column == 0 ||
                snakeHead.Location.Row == GameHeight - 1 || snakeHead.Location.Row == 0)
            {
                isGameOver = true;
                return;
            }

            IncrementScoreIfNeeded();

            switch (currentDirection)
            {
                case MoveDirection.Up:
                    snakeHead.Location.Row--;
                    break;
                case MoveDirection.Down:
                    snakeHead.Location.Row++;
                    break;
                case MoveDirection.Right:
                    snakeHead.Location.Column--;
                    break;
                case MoveDirection.Left:
                    snakeHead.Location.Column++;
                    break;
            }

            if (snakeBody.Count > score)
            {
                snakeBody.RemoveAt(0);
            }

            CheckSnakeToBodyCollision();
        }

        private void IncrementScoreIfNeeded() 
        {
            if (snakeHead.Location.Column == fruit.Location.Column && snakeHead.Location.Row == fruit.Location.Row)
            {
                score++;
                PlaceFruit();
            }

            snakeBody.Add(new Point { Column = snakeHead.Location.Column, Row = snakeHead.Location.Row });
        }

        private void CheckSnakeToBodyCollision()
        {
            isGameOver = snakeBody.Any(part => part.Column == snakeHead.Location.Column && part.Row == snakeHead.Location.Row);
        }

        private void RenderScreen()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < GameWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write(GamePixel);
                Console.SetCursorPosition(i, GameHeight - 1);
                Console.Write(GamePixel);
            }

            for (int i = 0; i < GameHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(GamePixel);
                Console.SetCursorPosition(GameWidth - 1, i);
                Console.Write(GamePixel);
            }

            DrawGamePixel(fruit);
            DrawGamePixel(snakeHead);

            foreach (var part in snakeBody)
            {
                DrawGamePixel(new GamePixel { Location = part, PixelColor = ConsoleColor.Green });
            }
        }

        private void PlaceFruit()
        {
            fruit = new GamePixel
            {
                Location = new Point { Column = random.Next(1, GameWidth - 1), Row = random.Next(1, GameHeight - 1) },
                PixelColor = ConsoleColor.Blue
            };
        }

        private void DrawGamePixel(GamePixel pixel)
        {
            Console.SetCursorPosition(pixel.Location.Column, pixel.Location.Row);
            Console.ForegroundColor = pixel.PixelColor;
            Console.Write(GamePixel);
        }
    }
}