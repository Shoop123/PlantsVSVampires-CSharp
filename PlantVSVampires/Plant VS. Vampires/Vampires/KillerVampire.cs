// Elliot Spall,
// June 6th 2016,
// Handles the creation of a killer vampire.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_VS.Vampires.Vampires
{
    public class KillerVampire : Vampire
    {
        /// <summary>
        /// The Constructor for a KillerVampire
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="bounds"></param>
        internal KillerVampire(int row, int col, Rectangle bounds) : base(row, col, bounds) 
        {
            // Sets the speed damage and health to the passed in values.
            this._speed = SharedVariables.KILLER_VAMPIRE_SPEED;
            this._damage = SharedVariables.KILLER_VAMPIRE_DAMAGE;
            this._health = SharedVariables.KILLER_VAMPIRE_HEALTH;
        }
    }
}
