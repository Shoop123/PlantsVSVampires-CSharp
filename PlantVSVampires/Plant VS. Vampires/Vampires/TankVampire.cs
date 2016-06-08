// Elliot Spall,
// June 6th 2016,
// Handles the creation of a tank vampire.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_VS.Vampires.Vampires
{
    public class TankVampire : Vampire
    {
        /// <summary>
        /// The Constructor for a TankVampire
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="bounds"></param>
        internal TankVampire(int row, int col, Rectangle bounds) : base(row, col, bounds)
        {
            // Sets the speed damage and health to the passed in values.
            this._speed = SharedVariables.TANK_VAMPIRE_SPEED;
            this._damage = SharedVariables.TANK_VAMPIRE_DAMAGE;
            this._health = SharedVariables.TANK_VAMPIRE_HEALTH;
        }
    }
}
