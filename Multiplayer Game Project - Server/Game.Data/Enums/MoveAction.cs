using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Data.Enums
{
    public enum MoveAction
    {
        None,
        Walk,
        Combat,
        Run,
        StrafeLeft,
        StrafeRight,
        /*RunStrafeLeft = Run | StrafeLeft,
        RunStrafeRight = Run | StrafeRight,
        CombatRun = Combat | Run,
        CombatRunStrafeLeft = CombatRun | StrafeLeft,
        CombatRunStrafeRight = CombatRun | StrafeRight,
        CombatWalk = Combat | Walk,
        CombatWalkStrafeLeft = CombatWalk | StrafeLeft,
        CombatWalkStrafeRight = CombatWalk | StrafeRight*/
    }
}
