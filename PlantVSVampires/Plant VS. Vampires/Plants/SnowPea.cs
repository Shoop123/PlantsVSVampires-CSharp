/* Daniel Berezovski
 * June 1st, 2016
 * This class store info about the snowpea
 */
using System.Collections.Generic;
using System.Drawing;

namespace Plant_VS.Vampires.Plants
{
    public class SnowPea : Plant
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="bounds">Bounds</param>
        public SnowPea(int row, int col, Rectangle bounds) : base(row, col, bounds)
        {
            //Set the price, shoot delay, and health
            _price = SharedVariables.SNOW_PEA_PRICE;
            _delay = SharedVariables.SNOW_PEA_SHOOT_DELAY;
            _health = SharedVariables.SNOW_PEA_HEALTH;
        }

        /// <summary>
        /// Shoots a projectile
        /// </summary>
        /// <param name="vampires">Vampires on the board</param>
        /// <returns>The projectile</returns>
        internal override Projectile Shoot(List<Vampire> vampires)
        {
            //Check if it should shoot
            if(ShouldShoot(vampires))
            {
                //Create the bounds of the projectile
                Rectangle projectileBounds = GetProjectileBounds();
                
                //Set the y location
                projectileBounds.Y += Bounds.Height / 6;

                //Create the projectile
                Projectile projectile = new Projectile(Projectile.ProjectileType.SnowPeaProjectile, _row, _col, projectileBounds);

                //Return it
                return projectile;
            }

            //Return null if its not time to shoot
            return null;
        }
    }
}
