// Elliot Spall,
// June 6th 2016,
// Handles the creation of a weak vampire.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_VS.Vampires.Vampires
{
    public class WeakVampire : Vampire
    {
        // Creates a constant to store the side offset.
        internal const int SIDE_OFFSET = 40;

        /// <summary>
        /// The Constructor for the WeakVampire
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="bounds"></param>
        internal WeakVampire(int row, int col, Rectangle bounds) : base(row, col, bounds)
        {
            // Sets the speed damage and health to the passed in values.
            this._speed = SharedVariables.WEAK_VAMPIRE_SPEED;
            this._damage = SharedVariables.WEAK_VAMPIRE_DAMAGE;
            this._health = SharedVariables.WEAK_VAMPIRE_HEALTH;
        }
    }
}
