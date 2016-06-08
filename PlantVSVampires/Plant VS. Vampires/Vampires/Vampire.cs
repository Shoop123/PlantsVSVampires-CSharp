// Elliot Spall, Daniel Berezovski
// June 6th 2016,
// Handles the creation of vampire.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_VS.Vampires
{
    public class Vampire : Entity
    {
        // Creates variables to store the health damage and speed of a vampire.
        protected int _health, _damage, _speed = 0;

        // Creates a bool to store whether or not the vampire is attacking.
        private bool _attacking = false;

        // Creates a variable to store the time of the last attack the vampire made.
        private int _lastAttack = Environment.TickCount;

        // Creates a constant to store the attack time.
        private const int ATTACK_TIME = 1500;

        // Creates a constant to store the slow down attack time.
        private const int SLOW_DOWN_ATTACK_TIME = 500;

        // Creates a constant to store the slow down time.
        private const int SLOW_DOWN_TIME = 1000;

        // Creates a bool to store whether or not the vampire has been slowed.
        private bool _slowed = false;

        // Creates a variable to store the start time of the slowing.
        private int _startTimeSlow = 0;

        // Creates public bool to store whether or not the vampire has been slowed.
        private bool Slowed
        {
            // Returns slowed.
            get { return _slowed; }
            set 
            {
                // If the value to be passed in is true.
                if (value)
                {
                    // Sets the start time for the slowing to the current time.
                    _startTimeSlow = Environment.TickCount;
                }

                // Sets slowed to be the passed in value.
                _slowed = value; 
            }
        }

        // Creates a private bool to store whether or not the vampire is attacking.
        private bool Attacking
        {
            // Returns _attacking.
            get { return _attacking; }

            // Sets the value to the passed in value.
            set { _attacking = value; }
        }

        // Creates a bool for the end and sets it to false;
        private bool _end = false;

        // Creates an internal bool to store wehether has gotten to the end.
        internal bool End
        {
            // Returns _end
            get { return _end; }
            private set 
            {
                // If the vampire is not at the end.
                if(!_end)
                    // Sets the end to the passed in value.
                    _end = value; 
            }
        }

        /// <summary>
        /// The Constructor for the Vampire
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="bounds"></param>
        protected Vampire(int row, int col, Rectangle bounds) : base(row, col, bounds) { }

        /// <summary>
        /// Handles a vampire getting hit
        /// </summary>
        /// <param name="projectile"></param>
        internal void GetHit(Projectile projectile)
        {
            // If the projectile is a snowpea.
            if (projectile.Type == Projectile.ProjectileType.SnowPeaProjectile)
            {
                // If the vampire isn't already slowed.
                if(!Slowed)
                    // Sets the speed to the slow effect speed.
                    this._speed -= projectile.SlowEffect;
                // Sets the slowed to true.
                Slowed = true;
            }


            // Decreases the health of the vampire by the projectile damage.
            this._health -= projectile.GetDamage();
        }

        /// <summary>
        /// Handles a vampire attacking a player
        /// </summary>
        /// <param name="player"></param>
        internal void Attack(Player player)
        {
            // Sets attacking to true.
            Attacking = true;

            // If the time between the current time and the last attack is greater than or equal to the constant attack time.
            if (Environment.TickCount - _lastAttack >= ATTACK_TIME)
            {
                // Calls the player to get hit.
                player.GetHit(this);

                // Sets the last attack time to the current time.
                _lastAttack = Environment.TickCount;
            }
        }

        /// <summary>
        /// Handles a vampire attacking a plant
        /// </summary>
        /// <param name="plant"></param>
        internal void Attack(Plant plant)
        {
            // Sets attacking to true.
            Attacking = true;

            // Creates a variable to store the constant attack time.
            int attackTime = ATTACK_TIME;

            // If the vampire is slowed.
            if (Slowed)
                // Slows down the attack time.
                attackTime += SLOW_DOWN_ATTACK_TIME;

            // If the time between the current time and the last attack is greater than or equal to the attack time.
            if (Environment.TickCount - _lastAttack >= attackTime)
            {
                // Call the plant to get hit.
                plant.GetHit(this);

                // If the plant health is less than or equal to 0.
                if (plant.GetHealth() <= 0)
                {
                    // Set attacking to false;
                    Attacking = false;
                }

                // Sets the last attack time to the current time.
                _lastAttack = Environment.TickCount;
            }
        }

        /// <summary>
        /// Handles a vampire moving
        /// </summary>
        /// <param name="edge"></param>
        internal void Move(int edge)
        {
            // If the vampire is slowed.
            if (Slowed)
            {
                // If the time between the current time and the start of the slow down time is greater than the constant slow down time.
                if (Environment.TickCount - _startTimeSlow >= SLOW_DOWN_TIME)
                {
                    // Sets slowed to false.
                    Slowed = false;

                    // Incrases the vampire speed by the slow effect.
                    this._speed += SharedVariables.SLOW_EFFECT;
                }
            }

            // If the vampire is not attacking.
            if (!Attacking)
            {
                // If the vampire's x coordinate is less than or equal to the edge.
                if (this.X <= edge)
                {
                    // Set the end to true.
                    End = true;
                }

                // Otherwise
                else
                    // Increase the vampire's x coordinate by the vampire's speed.
                    this.X -= _speed;
            }
        }

        /// <summary>
        /// Gets the vampire's health
        /// </summary>
        /// <returns></returns>
        internal int GetHealth()
        {
            // Returns _health.
            return _health;
        }

        /// <summary>
        /// Gets the vampire's damage
        /// </summary>
        /// <returns></returns>
        internal int GetDamage()
        {
            // Returns _damage.
            return _damage;
        }

        /// <summary>
        /// Gets the vampire's speed
        /// </summary>
        /// <returns></returns>
        internal int GetSpeed()
        {
            // Returns _speed.
            return _speed;
        }

        /// <summary>
        /// Gets the vampire's row
        /// </summary>
        /// <returns></returns>
        internal int GetRow()
        {
            // Returns _row.
            return this._row;
        }
    }
}
