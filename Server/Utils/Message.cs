using System;

namespace TrisGame
{
    public class Message
    {
        public Type Type { get; set; }
        public string Body { get; set; }
    }

    public enum Type
    {
        START_GAME,
        MODEL_UPDATE,

        START_TURN,
        MOVE,
        WIN,
        LOSE
    }
}