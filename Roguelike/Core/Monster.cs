﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RLNET;
using Roguelike.Behaviors;
using Roguelike.Systems;

namespace Roguelike.Core
{
    public class Monster : Actor
    {
        public int? TurnsAlerted { get; set; }

        public virtual void PerformAction(CommandSystem commandSystem)
        {
            var behavior = new StandardMoveAndAttack();
            behavior.Act(this, commandSystem);
        }
        
        public void DrawStats (RLConsole statConsole, int position)
        {
            int yPos = 13 + (position * 2);
            statConsole.Print(1, yPos, Symbol.ToString(), Color);

            int width = Convert.ToInt32(((double)Health / (double)MaxHealth) * 16.0);
            int remainingWidth = 16 - width;

            statConsole.SetBackColor(3, yPos, width, 1, Swatch.Primary);
            statConsole.SetBackColor(3 + width, yPos, remainingWidth, 1, Swatch.PrimaryDarkest);

            statConsole.Print(2, yPos, $": {Name}", Swatch.DbLight);
        }
    }
}
