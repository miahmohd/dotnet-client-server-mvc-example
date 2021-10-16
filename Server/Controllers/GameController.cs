using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Text.Json;
using TrisGame.Model;
using TrisGame.Views;

namespace TrisGame.Controllers
{
    public class GameController
    {
        private GameModel _model;
        private VirtualView[] _views;
        private int _turn;

        public GameController(GameModel model)
        {
            _model = model;
            _views = new VirtualView[2];
            _turn = 0;
        }

        public void AddView(VirtualView view)
        {
            _views[_turn] = view;
            NextTurn();
        }

        public void Start()
        {
            foreach (var virtualView in _views)
                virtualView.SendMessage(new Message {Type = Type.START_GAME});

            StartTurn();
        }


        public void Move(VirtualView currentView, Message message)
        {
            if (currentView != _views[_turn])
                throw new InvalidOperationException();

            int[] move = JsonSerializer.Deserialize<int[]>(message.Body);

            _model.SetPlayer(move[0], move[1], (Player) _turn + 1);

            if (isWinner())
            {
                currentView.SendMessage(new Message {Type = Type.WIN});
                NextTurn();
                _views[_turn].SendMessage(new Message {Type = Type.LOSE});
                return;
            }

            NextTurn();
            StartTurn();
        }

        private bool isWinner()
        {
            bool isWinner = false;

            for (int i = 0; i < 3; i++)
                isWinner = isWinner || (Player) _turn + 1 == _model.GetPlayer(i, 0) &&
                    _model.GetPlayer(i, 0) == _model.GetPlayer(i, 1) &&
                    _model.GetPlayer(i, 0) == _model.GetPlayer(i, 2);

            for (int i = 0; i < 3; i++)
                isWinner = isWinner || (Player) _turn + 1 == _model.GetPlayer(0, i) &&
                    _model.GetPlayer(0, i) == _model.GetPlayer(1, i) &&
                    _model.GetPlayer(0, i) == _model.GetPlayer(2, i);

            isWinner = isWinner || (Player) _turn + 1 == _model.GetPlayer(0, 0) &&
                _model.GetPlayer(0, 0) == _model.GetPlayer(1, 1) &&
                _model.GetPlayer(0, 0) == _model.GetPlayer(2, 2);


            isWinner = isWinner || (Player) _turn + 1 == _model.GetPlayer(0, 2) &&
                _model.GetPlayer(0, 2) == _model.GetPlayer(1, 1) &&
                _model.GetPlayer(0, 2) == _model.GetPlayer(2, 0);

            return isWinner;
        }


        private void NextTurn()
        {
            _turn = (_turn + 1) % 2;
        }

        private void StartTurn()
        {
            _views[_turn].SendMessage(new Message {Type = Type.START_TURN});
        }
    }
}