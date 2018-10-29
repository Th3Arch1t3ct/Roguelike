using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Roguelike.Core;
using RogueSharp;
using RogueSharp.DiceNotation;
using Roguelike.Monsters;

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

                
            }

            for(int r = 1; r < _map.Rooms.Count; r++)
            {
                int previousRoomCenterX = _map.Rooms[r-1].Center.X;
                int previousRoomCenterY = _map.Rooms[r-1].Center.Y;
                int currentRoomCenterX = _map.Rooms[r].Center.X;
                int currentRoomCenterY = _map.Rooms[r].Center.Y;

                if(Game.Random.Next(1,2) == 1)
                {
                    CreateHorizontalTunnel(previousRoomCenterX, currentRoomCenterX, previousRoomCenterY);
                    CreateVerticalTunnel(previousRoomCenterY, currentRoomCenterY, previousRoomCenterX);
                }
                 else
                {
                    CreateVerticalTunnel(previousRoomCenterY, currentRoomCenterY, previousRoomCenterX);
                    CreateHorizontalTunnel(previousRoomCenterX, currentRoomCenterX, previousRoomCenterY);
                }
            }

            foreach (Rectangle room in _map.Rooms)
            {
                CreateRoom(room);
            }

            PlacePlayer();
            PlaceMonsters();

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

        private void PlacePlayer()
        {
            Player player = Game.Player;
            if (player == null)
            {
                player = new Player();
            }

            player.X = _map.Rooms[0].Center.X;
            player.Y = _map.Rooms[0].Center.Y;

            _map.AddPlayer(player);
        }

        private void CreateHorizontalTunnel(int xStart, int xEnd, int yPos)
        {
            for (int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++)
            {
                _map.SetCellProperties(x, yPos, true,true);
            }
        }

        private void CreateVerticalTunnel(int yStart, int yEnd, int xPos)
        {
            for(int y =Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++)
            {
                _map.SetCellProperties(xPos, y, true, true);
            }
        }

        private void PlaceMonsters()
        {
            // TODO: fix faultyPoint. Probably insecure and should be a null value
            Point faultyPoint = new Point(-10000,-10000);
            foreach (var room in _map.Rooms)
            {
                if (Dice.Roll("1D10") < 7)
                {
                    var numberOfMonsters = Dice.Roll("1D4");
                    for(int i =0; i < numberOfMonsters; i++)
                    {
                        Point randomRoomLocation = _map.GetRandomWalkableLocationInRoom(room);
                        
                        if (randomRoomLocation != faultyPoint)
                        {
                            var monster = Kobold.Create(1);
                            monster.X = randomRoomLocation.X;
                            monster.Y = randomRoomLocation.Y;
                            _map.AddMonster(monster);
                        }
                    }
                }
            }
        }
    }
}
