/* Daniel Berezovski
 * June 1st, 2016
 * This class store info about the cherrybomb
 */
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Plant_VS.Vampires.Plants
{
    public class CherryBomb : Plant
    {
        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="bounds">Bounds</param>
        public CherryBomb(int row, int col, Rectangle bounds) : base(row, col, bounds)
        {
            //Set the health, price, and shoot delay
            _health = SharedVariables.CHERRY_BOMB_HEALTH;
            _price = SharedVariables.CHERRY_BOMB_PRICE;
            _delay = SharedVariables.CHERRY_BOMB_DELAY;
        }

        /// <summary>
        /// Shoots a projectile
        /// </summary>
        /// <param name="vampires">Vampires</param>
        /// <returns>The projectile</returns>
        internal override Projectile Shoot(List<Vampire> vampires)
        {
            //Checks if the plant should shoot
            if(ShouldShoot(null))
            {
                //Create a projectile
                Projectile projectile = new Projectile(Projectile.ProjectileType.BombProjectile, _row, _col, Bounds);

                //Return the projectile
                return projectile;
            }

            //Return null
            return null;
        }

        /// <summary>
        /// Gets the time until the cherrybomb blows up
        /// </summary>
        /// <returns>The time left</returns>
        internal int GetSecondsToDetonation()
        {
            //Get the time passed
            int passed = Environment.TickCount - _lastUpdate;

            //Return the time
            return (int)Math.Ceiling((_delay - passed) / 1000f);
        }
    }
}
