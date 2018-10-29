using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Roguelike.Core;
using RogueSharp;

namespace Roguelike.Systems
{
    class MapGenerator
    {
        private readonly int _width;
        private readonly int _height;
        private readonly int _maxRooms;
        private readonly int _maxRoomSize;
        private readonly int _minRoomSize;

        private readonly DungeonMap _map;

        public MapGenerator(int width, int height, int maxRooms, int maxRoomSize, int minRoomSize)
        {
            _width = width;
            _height = height;
            _maxRooms = maxRooms;
            _maxRoomSize = maxRoomSize;
            _minRoomSize = minRoomSize;
            _map = new DungeonMap();
        }

        public DungeonMap CreateMap()
        {
            _map.Initialize(_width, _height);
            for (int r = _maxRooms; r > 0; r--)
            {
                int roomWidth = Game.Random.Next(_minRoomSize, _maxRoomSize);
                int roomHeight = Game.Random.Next(_minRoomSize, _maxRoomSize);
                int roomXPos = Game.Random.Next(0, _width - roomWidth - 1);
                int roomYPos = Game.Random.Next(0, _height - roomHeight - 1);

                var newRoom = new Rectangle(roomXPos, roomYPos, roomWidth, roomHeight);

                bool doesRoomIntersect = _map.Rooms.Any(room => newRoom.Intersects(room));

                if (!doesRoomIntersect)
                {
                    _map.Rooms.Add(newRoom);
                }

                foreach(Rectangle room in _map.Rooms)
                {
                    CreateRoom(room);
                }
            }

            return _map;
        }

        private void CreateRoom(Rectangle room)
        {
            for (int x = room.Left+1;x < room.Right; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom; y++)
                {
                    _map.SetCellProperties(x, y, true, true, true);
                }
            }
        }
    }
}
