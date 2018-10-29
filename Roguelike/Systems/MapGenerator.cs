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

        private readonly DungeonMap _map;

        public MapGenerator(int width, int height)
        {
            _width = width;
            _height = height;
            _map = new DungeonMap();
        }

        public DungeonMap CreateMap()
        {
            _map.Initialize(_width, _height);
            foreach (Cell cell in _map.GetAllCells())
            {
                _map.SetCellProperties(cell.X, cell.Y, true, true, true);
            }

            // Set the first and last rows in the map to not be transparent or walkable
            foreach (Cell c in _map.GetCellsInRows(0, _height- 1))
            {
                _map.SetCellProperties(c.X, c.Y, false, false, true);
            }

            foreach (Cell c in _map.GetCellsInColumns(0, _width-1))
            {
                _map.SetCellProperties(c.X, c.Y, false, false, true);
            }

            return _map;
        }
    }
}
