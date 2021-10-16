using System;
using System.Text.Json;

namespace TrisGame.Model
{
    public enum Player
    {
        None,
        X,
        O
    }

    public class GameModel : Observable<Message>
    {
        private Player[,] _board;

        public GameModel()
        {
            _board = new Player[3, 3];
            for (int i = 0; i < _board.GetLength(0); i++)
            {
                for (int j = 0; j < _board.GetLength(1); j++)
                {
                    _board[i, j] = Player.None;
                }
            }
        }

        public void SetPlayer(int row, int col, Player player)
        {
            if (_board[row, col] != Player.None)
                throw new InvalidOperationException();
            if (
                row is < 0 or > 2 ||
                col is < 0 or > 2
            )
                throw new ArgumentException();
            _board[row, col] = player;
            Notify(new Message
            {
                Type = Type.MODEL_UPDATE,
                Body = JsonSerializer.Serialize(new MoveUpdate {Move = new int[] {row, col}, Player = (int) player})
            });
        }

        public Player GetPlayer(int row, int col)
        {
            if (
                row is < 0 or > 2 ||
                col is < 0 or > 2
            )
                throw new ArgumentException();

            return _board[row, col];
        }
    }
}