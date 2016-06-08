/* Daniel Berezovski
 * June 1st, 2016
 * This class stores info about the player
 */

namespace Plant_VS.Vampires
{
    internal class Player
    {
        //Amount of moons at the start
        private const int START_MOONS = 10;

        //Amount of health at the start
        private const int START_HEALTH = 10;

        //Initialize the moons
        private int _moons = START_MOONS;

        //Initialize the health
        private int _health = START_HEALTH;

        /// <summary>
        /// Constructor
        /// </summary>
        internal Player() { }

        /// <summary>
        /// Getter and setter for moons
        /// </summary>
        internal int Moons
        {
            //Get them
            get { return this._moons; }

            //Set them
            set { this._moons = value; }
        }

        /// <summary>
        /// Getter and setter for health
        /// </summary>
        internal int Health
        {
            //Get it
            get { return this._health; }

            //Set it
            private set
            {
                //Check if the health is less than 0
                if (value < 0)
                    //Set the health to 0
                    this._health = 0; 
                else
                    //Set the health
                    this._health = value;
            }
        }

        /// <summary>
        /// Player gets hit
        /// </summary>
        /// <param name="vampire">The attacker</param>
        internal void GetHit(Vampire vampire)
        {
            //Subtract the health
            Health -= vampire.GetDamage();
        }
    }
}
