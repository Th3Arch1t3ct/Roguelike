using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RLNET;

namespace Roguelike.Systems
{
    public class MessageLog
    {
        private static readonly int _maxLines = 9;
        private readonly Queue<string> _lines;

        public MessageLog()
        {
            _lines = new Queue<string>();
        }

        public void Add(string msg)
        {
            _lines.Enqueue(msg);

            if(_lines.Count > _maxLines)
            {
                _lines.Dequeue();
            }
        }

        public void Draw(RLConsole console)
        {
            console.Clear();
            string[] lines = _lines.ToArray();
            for(int i =0; i < lines.Length; i++)
            {
                console.Print(1, 1 + i, lines[i], RLColor.White);
            }
        }
    }
}
