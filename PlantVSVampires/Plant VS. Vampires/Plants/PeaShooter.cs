/* Daniel Berezovski
 * June 1st, 2016
 * This class store info about the peashooter
 */
using System.Collections.Generic;
using System.Drawing;

namespace Plant_VS.Vampires.Plants
{
    public class PeaShooter : Plant
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="bounds">Bounds</param>
        public PeaShooter(int row, int col, Rectangle bounds) : base(row, col, bounds)
        {
            //Set the price, delay, and health
            _price = SharedVariables.PEA_SHOOTER_PRICE;
            _delay = SharedVariables.PEA_SHOOTER_SHOOT_DELAY;
            _health = SharedVariables.PEA_SHOOTER_HEALTH;
        }

        /// <summary>
        /// Shoots the projectile
        /// </summary>
        /// <param name="vampires">The vampires on the board</param>
        /// <returns>The projectile</returns>
        internal override Projectile Shoot(List<Vampire> vampires)
        {
            //Check if it should shoot
            if(ShouldShoot(vampires))
            {
                //Create the projectile
                Projectile projectile = new Projectile(Projectile.ProjectileType.PeaShooterProjectile, _row, _col, GetProjectileBounds());

                //Return it
                return projectile;
            }

            //Return null
            return null;
        }
    }
}
