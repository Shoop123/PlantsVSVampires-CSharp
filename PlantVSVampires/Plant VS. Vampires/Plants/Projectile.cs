/* Daniel Berezovski
 * June 1st, 2016
 * This class store info about the projectile
 */
using System;
using System.Drawing;

namespace Plant_VS.Vampires
{
    public class Projectile : Entity
    {
        //Create the speed, splash radius, and damage
        private int _speed;
        private int _splashRadius;
        private int _damage;

        //Store the type of projectile
        private ProjectileType _type;

        //Save the time it was created
        private int _lastTime;

        //How much this projectile slows down the enemy
        private int _slowEffect = 0;

        /// <summary>
        /// Getter and setter for the slow effect
        /// </summary>
        internal int SlowEffect
        {
            //Get it
            get { return _slowEffect; }

            //Set it
            private set { _slowEffect = value; }
        }

        //Getter and setter for type
        public ProjectileType Type
        {
            //Get it
            get { return _type; }

            //Set it
            private set { _type = value; }
        }

        /// <summary>
        /// Getter and setter for last time
        /// </summary>
        internal int LastTime
        {
            //Get it
            get { return _lastTime; }

            //Set it
            private set { _lastTime = value; }
        }

        //Getter and setter for the row
        public int Row
        {
            //Get it
            get { return _row; }

            //Set it
            private set { _row = value; }
        }

        //Getter and setter for the column
        public int Col
        {
            //Get it
            get { return _col; }

            //Set it
            private set { _col = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="bounds">Bounds</param>
        internal Projectile(ProjectileType type, int row, int col, Rectangle bounds) : base(row, col, bounds)
        {
            //Set the type
            Type = type;

            //Set the row
            Row = row;

            //Set the column
            Col = col;

            //Set the projectile statistics
            GetProjectileStats(type);
        }

        /// <summary>
        /// Sets all of the statistics
        /// </summary>
        /// <param name="type">Type</param>
        private void GetProjectileStats(ProjectileType type)
        {
            //If its a bomb projectile
            if (type == ProjectileType.BombProjectile)
            {
                //Set the damage, splash radius, and speed
                _damage = SharedVariables.HIGH_PLANT_DAMAGE;
                _splashRadius = SharedVariables.HIGH_PLANT_SPLASH_RADIUS;
                _speed = SharedVariables.NO_PROJECTILE_SPEED;

                //Set when it was created
                LastTime = Environment.TickCount;
            }
            //If its a mine projectile
            else if (type == ProjectileType.MineProjectile)
            {
                //Set the damage, splash radius, and speed
                _damage = SharedVariables.HIGH_PLANT_DAMAGE;
                _splashRadius = SharedVariables.LOW_PLANT_SPLASH_RADIUS;
                _speed = SharedVariables.NO_PROJECTILE_SPEED;

                //Set the time it was created
                LastTime = Environment.TickCount;
            }
            //If its a peashooter projectile
            else if (type == ProjectileType.PeaShooterProjectile)
            {
                //Set the damage, splash radius, and speed
                _damage = SharedVariables.MEDIUM_PLANT_DAMAGE;
                _splashRadius = SharedVariables.LOW_PLANT_SPLASH_RADIUS;
                _speed = SharedVariables.LOW_PROJECTILE_SPEED;
            }
            //If its a snowpea projectile
            else if (type == ProjectileType.SnowPeaProjectile)
            {
                //Set the damage, splash radius, and speed
                _damage = SharedVariables.MEDIUM_PLANT_DAMAGE;
                _splashRadius = SharedVariables.LOW_PLANT_SPLASH_RADIUS;
                _speed = SharedVariables.HIGH_PROJECTILE_SPEED;

                //Set how much it slows down a vampire
                SlowEffect = SharedVariables.SLOW_EFFECT;
            }
        }

        /// <summary>
        /// Move the projectile
        /// </summary>
        internal void Move()
        {
            //Increase the x
            X += _speed;
        }

        /// <summary>
        /// Get the splash radius
        /// </summary>
        /// <returns>The splash radius</returns>
        internal int GetSplashRadius()
        {
            //Return the splash radius
            return _splashRadius;
        }

        /// <summary>
        /// Gets the damge
        /// </summary>
        /// <returns>The damage</returns>
        internal int GetDamage()
        {
            return _damage;
        }

        /// <summary>
        /// Options for different types of projectiles
        /// </summary>
        public enum ProjectileType
        {
            //Peashooter one
            PeaShooterProjectile,

            //Snowpea one
            SnowPeaProjectile,

            //Mine one
            MineProjectile,

            //Bomb one
            BombProjectile,
        }
    }
}
