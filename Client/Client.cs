using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using TrisGame;
using Type = TrisGame.Type;

namespace Client
{
    class Program
    {
        private static void Main(string[] args)
        {
            var view = new ConsoleView();

            var ipe = new IPEndPoint(IPAddress.Parse("192.168.1.52"), 8080);
            var socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(ipe);

            var reader = new StreamReader(new NetworkStream(socket));
            var writer = new StreamWriter(new NetworkStream(socket));
            writer.AutoFlush = true;

            while (true)
            {
                var message = JsonSerializer.Deserialize<Message>(reader.ReadLine());
                switch (message.Type)
                {
                    case Type.START_GAME:
                        view.Start();
                        break;
                    case Type.START_TURN:
                        int[] move = view.PromptMove();
                        writer.WriteLine(JsonSerializer.Serialize(new Message
                            {Type = Type.MOVE, Body = JsonSerializer.Serialize(move)}));
                        break;
                    case Type.MODEL_UPDATE:
                        view.UpdateBoard(message);
                        break;
                    case Type.WIN:
                        view.Win();
                        break;
                    case Type.LOSE:
                        view.Lose();
                        break;

                    default:
                        Console.WriteLine($"{message.Type} not supported");
                        break;
                }
            }
        }
    }
}