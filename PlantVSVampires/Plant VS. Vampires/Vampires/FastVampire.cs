// Elliot Spall,
// June 6th 2016,
// Handles the creation of a fast vampire.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_VS.Vampires.Vampires
{
    public class FastVampire : Vampire
    {
        /// <summary>
        /// The Constructor for FastVampire
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="bounds"></param>
        internal FastVampire(int row, int col, Rectangle bounds) : base(row, col, bounds) 
        {
            // Sets the speed damage and health to the passed in values.
            this._speed = SharedVariables.FAST_VAMPIRE_SPEED;
            this._damage = SharedVariables.FAST_VAMPIRE_DAMAGE;
            this._health = SharedVariables.FAST_VAMPIRE_HEALTH;
        }
    }
}
