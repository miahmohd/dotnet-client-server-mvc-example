using System;
using System.Text.Json;
using TrisGame;

namespace Client
{
    public class ConsoleView
    {
        private int[,] _board;

        public ConsoleView()
        {
            _board = new int[3, 3];
        }

        public void Start()
        {
            PrintBoard();
        }

        private void Clear()
        {
            for (int i = 0; i < 20; i++)
                Console.WriteLine();
        }

        private void PrintBoard()
        {
            Clear();
            string[] sym = new[] {" ", "X", "O"};
            Console.WriteLine("┌───┬───┬───┐");
            for (int i = 0; i < _board.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < _board.GetLength(1); j++)
                    Console.Write($"│ {sym[_board[i, j]]} ");

                Console.WriteLine("│");
                Console.WriteLine("├───┼───┼───┤");
            }

            for (int j = 0; j < _board.GetLength(1); j++)
                Console.Write($"│ {sym[_board[2, j]]} ");

            Console.WriteLine("│");
            Console.WriteLine("└───┴───┴───┘");
        }

        public int[] PromptMove()
        {
            Console.WriteLine("Insert move (row, col): ");
            var moves = Console.ReadLine().Split(',');
            return new int[] {Int32.Parse(moves[0]), Int32.Parse(moves[1])};
        }

        public void UpdateBoard(Message? message)
        {
            var update = JsonSerializer.Deserialize<MoveUpdate>(message.Body);
            _board[update.Move[0], update.Move[1]] = update.Player;
            Clear();
            PrintBoard();
        }

        public void Win()
        {
            Console.WriteLine("Congratulation you won!");
        }

        public void Lose()
        {
            Console.WriteLine("You lost!");
        }
    }
}