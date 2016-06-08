/* Daniel Berezovski
 * June 1st, 2016
 * This class store info about the plants
 */
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Plant_VS.Vampires
{
    public abstract class Plant : Entity
    {
        //Health and price
        protected int _health;
        protected int _price;
        
        //Last time updated
        protected int _lastUpdate;

        //The shoot delay
        protected int _delay = SharedVariables.SHOOT_DELAY;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Colmumn</param>
        /// <param name="bounds">Bounds</param>
        protected Plant(int row, int col, Rectangle bounds) : base(row, col, bounds)
        {
            //Set the current time
            _lastUpdate = Environment.TickCount;
        }

        /// <summary>
        /// Shoot (Doesn't shoot)
        /// </summary>
        /// <param name="vampires">Vampires</param>
        /// <returns>The projectile</returns>
        internal virtual Projectile Shoot(List<Vampire> vampires)
        {
            //Null
            return null;
        }

        /// <summary>
        /// Check if it should shoot
        /// </summary>
        /// <param name="vampires">Vampires</param>
        /// <returns>If it should shoot</returns>
        protected virtual bool ShouldShoot(List<Vampire> vampires)
        {
            //Check if there are vampires
            if(vampires == null)
            {
                //Retunr the if its time
                return CheckTime();
            }

            //Loop through all of the vampires
            foreach(Vampire vampire in vampires)
            {
                //Check if there is a vampire in this row
                if (vampire.GetRow() == this._row && vampire.X + vampire.Width > this.X - (this.Width / 2))
                {
                    //Return if it should shoot
                    return CheckTime();
                }
            }

            //Return that it shouldn't shoot
            return false;
        }

        /// <summary>
        /// If the plant is dead
        /// </summary>
        /// <returns>Wheather or not the plant is dead</returns>
        internal bool IsDead()
        {
            //Return if the health is less that 0
            return _health <= 0;
        }

        /// <summary>
        /// Check the time for each shot
        /// </summary>
        /// <returns></returns>
        private bool CheckTime()
        {
            //Time that passed since last shot
            int timePassed = Environment.TickCount - _lastUpdate;

            //Check if the time passed is greater than or equal tothe delay
            if (timePassed >= _delay)
            {
                //Update the last time
                _lastUpdate = Environment.TickCount;

                //Return that its time to shoot
                return true;
            }

            //Return its not time to shoot
            return false;
        }

        /// <summary>
        /// Get the bounds for the projectile
        /// </summary>
        /// <returns>The bounds</returns>
        protected Rectangle GetProjectileBounds()
        {
            //Create the bounds rectangle
            Rectangle projectileBounds = Bounds;

            //Set the size
            projectileBounds.Size = new Size(Bounds.Width / 5, Bounds.Height / 5);

            //Get coordinates
            int x = X + Bounds.Width - projectileBounds.Width;
            int y = Y + (Bounds.Height / 10);

            //Set the location
            projectileBounds.Location = new Point(x, y);

            //Return the bounds
            return projectileBounds;
        }

        /// <summary>
        /// The plant gets attacked by a vampire
        /// </summary>
        /// <param name="vampire">The attacker</param>
        internal void GetHit(Vampire vampire)
        {
            //Decrease the health based on vampire damage
            _health -= vampire.GetDamage();
        }

        /// <summary>
        /// Gets the health
        /// </summary>
        /// <returns>The plant's health</returns>
        internal int GetHealth()
        {
            return _health;
        }

        /// <summary>
        /// Gets the price
        /// </summary>
        /// <returns>The plant's price</returns>
        internal int GetPrice()
        {
            return _price;
        }
    }
}
