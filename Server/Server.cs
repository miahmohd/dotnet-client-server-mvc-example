using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using TrisGame.Controllers;
using TrisGame.Model;
using TrisGame.Views;

namespace TrisGame
{
    class Program
    {
        private static void Main(string[] args)
        {
            var model = new GameModel();
            var controller = new GameController(model);

            var ipe = new IPEndPoint(IPAddress.Parse("172.17.2.23"), 8080);
            var listener = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(ipe);
            listener.Listen(10);
            Console.WriteLine("Server listening on port 8080");

            var tasks = new Task[2];

            for (int i = 0; i < 2; i++)
            {
                var socket = listener.Accept();
                Console.WriteLine($"Player {i} connected");
                
                var view = new VirtualView(socket, controller);
                model.AddView(view);
                controller.AddView(view);
                tasks[i] = Task.Run(view.Run);
            }
            
            controller.Start();

            Task.WaitAll(tasks);
        }
    }
}