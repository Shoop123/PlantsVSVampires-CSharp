/* Daniel Berezovski
 * June 1st, 2016
 * This class store info about the moon
 */
using System;
using System.Drawing;

namespace Plant_VS.Vampires
{
    public class Moon : Entity
    {
        //If the moon is collected
        private bool _isCollected = false;

        //The final destination of the moon
        private Point _endDest;

        //The original x location
        private int _originalX;

        //Speed of the moon
        private const int SPEED = 100;

        //If the moon is expired
        private bool _isExpired = false;

        //Getter and setter for if the moon is collected
        public bool IsCollected
        {
            //Getter
            get { return _isCollected; }
            //Setter
            set
            {
                //Check if the moon is collected
                if (!_isCollected)
                {
                    //Set the value
                    _isCollected = value;
                }
            }
        }

        //Getter and setter for if the moon is expired
        public bool IsExpired
        {
            //Getter
            get { return _isExpired; }
            //Setter
            private set
            {
                //Checks if the moon is expired
                if(!_isExpired)
                    //Set the value
                    _isExpired = value;
            }
        }

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="bounds">Bounds</param>
        /// <param name="endDest">Final destination</param>
        public Moon(int row, int col, Rectangle bounds, Point endDest) : base(row, col, bounds)
        {
            //Make the end destination global
            _endDest = endDest;

            //Save the original x location
            _originalX = X;
        }

        /// <summary>
        /// Moves the moon
        /// </summary>
        public void Move()
        {
            //Check if the moon is at the final destination
            if(X == _endDest.X && Y == _endDest.Y && IsCollected)
            {
                //Expire the moon
                IsExpired = true;
            }
            //Check if the moon is collected and not expired
            else if(IsCollected && !IsExpired)
            {
                //X and y location differences from the final destination
                int xDiff = X - _endDest.X;
                int yDiff = Y - _endDest.Y;

                //Angle of travel
                double angle = Math.Atan2(xDiff, yDiff);

                //The speed of the moon
                int xSpeed = (int)(SPEED * Math.Sin(angle));
                int ySpeed = (int)(SPEED * Math.Cos(angle));

                //Check if the x is greater than the final destination
                if (X > _endDest.X)
                {
                    if (X - xSpeed < _endDest.X)
                        X = _endDest.X;
                    else
                        X -= xSpeed;
                }
                //Check if the x with the move is greater than the final destination
                else
                {
                    if (X + xSpeed > _endDest.X)
                        X = _endDest.X;
                    else
                        X += xSpeed;
                }

                //Check if the y is greater than the final destination
                if (Y > _endDest.Y)
                {
                    if (Y - ySpeed < _endDest.Y)
                        Y = _endDest.Y;
                    else
                        Y -= ySpeed;
                }
                //Otherwise
                else
                {
                    if (Y + ySpeed > _endDest.Y)
                        Y = _endDest.Y;
                    else
                        Y += ySpeed;
                }
            }
        }

        /// <summary>
        /// Set the x of the moon
        /// </summary>
        /// <param name="x">The x to update to</param>
        internal void SetX(int x)
        {
            //Check if the moon is not collected
            if(!IsCollected)
            {
                //Set the coordinate
                X = x;
            }
        }
    }
}
