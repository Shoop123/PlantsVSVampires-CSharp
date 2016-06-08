/* Daniel Berezovski
 * June 1st, 2016
 * This class store info about the wallnut
 */
using System.Drawing;

namespace Plant_VS.Vampires.Plants
{
    public class WallNut : Plant
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="bounds">Bounds</param>
        public WallNut(int row, int col, Rectangle bounds) : base(row, col, bounds)
        {
            //Set the price and health of the plant
            _price = SharedVariables.WALL_NUT_PRICE;
            _health = SharedVariables.WALL_NUT_HEALTH;
        }
    }
}
