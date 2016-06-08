/* Daniel Berezovski, Elliot Spall
 * June 1st, 2016
 * This class stores general info about the game
 */
using System.Collections.Generic;

namespace Plant_VS.Vampires
{
    internal class SharedVariables
    {
        //Damages
        internal const int LOW_PLANT_DAMAGE = 1;
        internal const int MEDIUM_PLANT_DAMAGE = 2;
        internal const int HIGH_PLANT_DAMAGE = 100;

        //Radius'
        internal const int HIGH_PLANT_SPLASH_RADIUS = 1;
        internal const int LOW_PLANT_SPLASH_RADIUS = 0;

        //Speeds
        internal const int NO_PROJECTILE_SPEED = 0;
        internal const int LOW_PROJECTILE_SPEED = 10;
        internal const int HIGH_PROJECTILE_SPEED = 100;

        //Healths of plants
        internal const int PEA_SHOOTER_HEALTH = 6;
        internal const int SNOW_PEA_HEALTH = 6;
        internal const int CHERRY_BOMB_HEALTH = 4;
        internal const int WALL_NUT_HEALTH = 72;
        internal const int POTATO_MINE_HEALTH = 2;
        internal const int TREE_HEALTH = 7;
        internal const int MOON_FLOWER_HEALTH = 5;

        //Healths of vampires
        internal const int KILLER_VAMPIRE_HEALTH = 15;
        internal const int WEAK_VAMPIRE_HEALTH = 10;
        internal const int TANK_VAMPIRE_HEALTH = 34;
        internal const int FAST_VAMPIRE_HEALTH = 10;

        //Damages of vampires
        internal const int KILLER_VAMPIRE_DAMAGE = 5;
        internal const int WEAK_VAMPIRE_DAMAGE = 2;
        internal const int TANK_VAMPIRE_DAMAGE = 2;
        internal const int FAST_VAMPIRE_DAMAGE = 2;

        //Speeds of vampires
        internal const int KILLER_VAMPIRE_SPEED = 2;
        internal const int WEAK_VAMPIRE_SPEED = 2;
        internal const int TANK_VAMPIRE_SPEED = 1;
        internal const int FAST_VAMPIRE_SPEED = 3;

        //Prices
        internal const int MOON_FLOWER_PRICE = 2;
        internal const int PEA_SHOOTER_PRICE = 4;
        internal const int WALL_NUT_PRICE = 2;
        internal const int SNOW_PEA_PRICE = 8;
        internal const int POTATO_MINE_PRICE = 1;
        internal const int CHERRY_BOMB_PRICE = 6;
        internal const int TREE_PRICE = 1;

        // The cherry bomb splash radius.
        internal const int CHERRY_BOMB_SPLASH_RADIUS = 3;

        //The tree radius
        internal const int TREE_RADIUS = 3;

        //How long to wait before every shot
        internal const int SHOOT_DELAY = 2000;
        internal const int MOON_DELAY = 5000;
        internal const int CHERRY_BOMB_DELAY = 2000;
        internal const int PEA_SHOOTER_SHOOT_DELAY = 1500;
        internal const int SNOW_PEA_SHOOT_DELAY = 1500;

        //Amount of plants
        internal const int PLANT_COUNT = 7;

        //How much vampires get slowed down
        internal const int SLOW_EFFECT = 1;

        //Tile size
        private int _width;
        private int _height;

        //The Time Delay
        private int _timeDelay;

        /// <summary>
        /// Getter and setter for tile width
        /// </summary>
        internal int Width
        {
            //Get it
            get { return _width; }

            //Set it
            private set { _width = value; }
        }

        /// <summary>
        /// Getter and setter for height
        /// </summary>
        internal int Height
        {
            //Get it
            get { return _height; }

            //Set it
            private set { _height = value; }
        }

        //List of projectiles
        internal List<Projectile> projectiles = new List<Projectile>();
        //List of vampires
        internal List<Vampire> vampires = new List<Vampire>();
        //Plant board
        internal Plant[,] board;

        //Player info
        private Player player = new Player();

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="rows">Rows of plants</param>
        /// <param name="cols">Columns of plants</param>
        internal SharedVariables(int rows, int cols, int width, int height)
        {
            //Make the board
            board = new Plant[rows, cols];

            //Set the width and height
            this._width = width;
            this._height = height;
        }

        /// <summary>
        /// Handles setting the time delay
        /// </summary>
        /// <param name="timeDelay"></param>
        internal void SetTimeDelay(int timeDelay)
        {
            // Sets the time delay to the passed in time delay.
            _timeDelay = timeDelay;
        }

        /// <summary>
        /// Handles getting the time delay.
        /// </summary>
        /// <returns></returns>
        internal int GetTimeDelay()
        {
            // Returns the time delay.
            return _timeDelay;
        }

        /// <summary>
        /// The player gets hit
        /// </summary>
        /// <param name="vampire">The attacker</param>
        internal void GetHit(Vampire vampire)
        {
            vampire.Attack(player);
        }

        /// <summary>
        /// Gets the moons of the player
        /// </summary>
        /// <returns>The moons</returns>
        internal int GetMoons()
        {
            return player.Moons;
        }

        /// <summary>
        /// Get the health of the player
        /// </summary>
        /// <returns>The health</returns>
        internal int GetHealth()
        {
            return player.Health;
        }

        /// <summary>
        /// Add moons to the player
        /// </summary>
        /// <param name="amount">Amount to add</param>
        internal void AddMoons(int amount)
        {
            player.Moons += amount;
        }
    }
}
