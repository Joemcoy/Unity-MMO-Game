using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Data.Enums
{
    public enum MoveDirection
    {
        None,
        North = 1,
        South = 2,
        East = 4,
        West = 8,
        Northeast = North | East,
        Northwest = North | West,
        Southeast = South | East,
        Southwest = South | West,
    }
}
