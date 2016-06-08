/* Daniel Berezovski
 * June 1st, 2016
 * This class store info about the potatomine
 */
using System.Collections.Generic;
using System.Drawing;

namespace Plant_VS.Vampires.Plants
{
    public class PotatoMine : Plant
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="bounds">Bounds</param>
        public PotatoMine(int row, int col, Rectangle bounds) : base(row, col, bounds)
        {
            //Set the price and health
            _price = SharedVariables.POTATO_MINE_PRICE;
            _health = SharedVariables.POTATO_MINE_HEALTH;
        }

        /// <summary>
        /// Shoots a projectile
        /// </summary>
        /// <param name="vampires">The vampires</param>
        /// <returns></returns>
        internal override Projectile Shoot(List<Vampire> vampires)
        {
            //Create the projectile
            Projectile projectile = new Projectile(Projectile.ProjectileType.MineProjectile, _row, _col, Bounds);
            
            //Return it
            return projectile;
        }
    }
}
