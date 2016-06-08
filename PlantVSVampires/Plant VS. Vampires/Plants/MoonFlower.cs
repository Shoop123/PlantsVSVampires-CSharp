/* Daniel Berezovski
 * June 1st, 2016
 * This class store info about the moonflower
 */
using System.Collections.Generic;
using System.Drawing;

namespace Plant_VS.Vampires.Plants
{
    public class MoonFlower : Plant
    {
        //List of moons
        private List<Moon> _moons = new List<Moon>();

        //List of moons that should still be drawn
        private List<Moon> _moonsToDraw = new List<Moon>();

        //Size of moons
        private Size _moonSize;

        //Space between each moon when cascading
        private const int MOON_SPACE = 10;

        //Constant used for scaling
        private const int SIZE = 5;

        //Max amount of moons per moonflower
        private const int MAX_MOONS = 10;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="bounds">Bounds of the flower</param>
        public MoonFlower(int row, int col, Rectangle bounds) : base(row, col, bounds)
        {
            //Set the price, health, and shooting delay
            _price = SharedVariables.MOON_FLOWER_PRICE;
            _health = SharedVariables.MOON_FLOWER_HEALTH;
            _delay = SharedVariables.MOON_DELAY;

            //Set the size based on plant size
            _moonSize = new Size(Width / SIZE, Height / SIZE);
        }

        /// <summary>
        /// Makes a moon
        /// </summary>
        /// <param name="endDest">The final destination of the moon</param>
        internal void MakeMoon(Point endDest)
        {
            //Exit if the moons are full
            if (_moons.Count >= MAX_MOONS) return;

            //Check if its time to shoot (used for moons)
            if(ShouldShoot(null))
            {
                //Create the bounds for the moon
                Rectangle moonBounds = Bounds;

                //Set the moon size
                moonBounds.Size = _moonSize;

                //Create the moon
                Moon moon = new Moon(_row, _col, moonBounds, endDest);

                //Add the moon to the list
                _moons.Add(moon);
                _moonsToDraw.Add(moon);

                //Cascade all of the moons
                CascadeMoons();
            }
        }

        /// <summary>
        /// Gets the location of the next moon
        /// </summary>
        /// <returns>The location</returns>
        private Point GetMoonLocation()
        {
            //Create the location
            Point location = new Point();

            //Set the y location as its constant
            location.Y = Y;

            //Check if the moons can fit with cascading
            if (_moonSize.Width * (_moons.Count + 1) <= Width)
            {
                //Add the moons
                location.X = X + _moonSize.Width * _moons.Count;
            }
            else
            {
                //Get how much the moons stand out
                int standOut = GetStandOut();

                //Update the locations of all of the moons
                UpdateMoonPoints(standOut);

                //Get the x location of this moon
                location.X = _moons[_moons.Count - 1].X + standOut;
            }

            //Return the location
            return location;
        }

        /// <summary>
        /// Update the locations of all of the moons
        /// </summary>
        /// <param name="standOut">Amount the moons stand out</param>
        private void UpdateMoonPoints(int standOut)
        {
            //Loop through all of the moons
            for(int i = 1; i < _moons.Count; i++)
            {
                //Set their x coordinates
                _moons[i].SetX(_moons[i - 1].X + standOut);
            }
        }

        /// <summary>
        /// Cascades the moon so that they appear on top of each other
        /// </summary>
        internal void CascadeMoons()
        {
            //Check if there are moons
            if (_moons.Count == 0) return;

            //If the moons take up too much space
            if (_moonSize.Width * _moons.Count <= Width)
            {
                //Loop through the moons
                for (int i = 0; i < _moons.Count; i++)
                {
                    //Set the new x of the moon
                    _moons[i].SetX(X + _moonSize.Width * i);
                }
            }
            else
            {
                //Get how much the moon stands out
                int standOut = GetStandOut();

                //Set the x location
                _moons[0].SetX(X);

                //Loop through all of the moons
                for (int i = 1; i < _moons.Count; i++)
                {
                    //Set the x of the moon
                    _moons[i].SetX(_moons[i - 1].X + standOut);
                }
            }
        }

        /// <summary>
        /// Collect the moon
        /// </summary>
        /// <param name="moon">Moon to collect</param>
        internal void CollectMoon(Moon moon)
        {
            //If the moons exists
            if(_moons.Contains(moon))
            {
                //Remove it
                _moons.Remove(moon);
            }
        }

        /// <summary>
        /// Get how much the moons stand out
        /// </summary>
        /// <returns>The stand out</returns>
        private int GetStandOut()
        {
            //Calculate the stand out
            return (Width - _moonSize.Width) / (_moons.Count - 1);
        }

        /// <summary>
        /// Get the moons
        /// </summary>
        /// <returns>The moons</returns>
        internal List<Moon> GetMoons()
        {
            //Return the moons
            return _moons;
        }

        /// <summary>
        /// Get the moons that will be drawn
        /// </summary>
        /// <returns>The moons to draw</returns>
        internal List<Moon> GetMoonsToDraw()
        {
            //Return the moons
            return _moonsToDraw;
        }
    }
}
