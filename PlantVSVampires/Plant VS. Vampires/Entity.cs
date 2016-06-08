/* Daniel Berezovski
 * June 1st, 2016
 * This class store info about the vampires and plants and projectiles
 */
using System.Drawing;

namespace Plant_VS.Vampires
{
    public class Entity
    {
        //Set up the location of the entity
        private int _x;
        private int _y;

        //Bounds of the entity
        private Rectangle _bounds;

        //Size of the entity
        private int _width;
        private int _height;

        //Row and column
        protected int _row, _col;

        /// <summary>
        /// Getter and setter for the bounds
        /// </summary>
        public Rectangle Bounds
        {
            //Get it 
            get { return _bounds; }

            //Set it
            protected set 
            {
                _bounds = value;

                //Set all of the values
                Y = _bounds.Y;
                X = _bounds.X;
                Width = _bounds.Width;
                Height = _bounds.Height;
            }
        }

        /// <summary>
        /// Getter and setter
        /// </summary>
        public int Y
        {
            //Get it
            get { return _y; }

            //Set it
            protected set
            {
                _y = value;
                
                //Set the bounds
                _bounds.Y = _y;
            }
        }

        /// <summary>
        /// Getter and setter
        /// </summary>
        public int X
        {
            //Get it
            get { return _x; }

            //Set it
            protected set 
            {
                _x = value;

                //Set the bounds
                _bounds.X = _x;
            }
        }

        /// <summary>
        /// Getter and setter
        /// </summary>
        public int Width
        {
            //Get it
            get { return _width; }

            //Set it
            protected set 
            {
                _width = value;

                //Set the bounds
                _bounds.Width = _width;
            }
        }

        /// <summary>
        /// Getter and setter
        /// </summary>
        public int Height
        {
            //Get it
            get { return _height; }

            //Set it
            protected set 
            {
                _height = value;

                //Set the bounds
                _bounds.Height = _height;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="row">Row on board</param>
        /// <param name="col">Column on the board</param>
        /// <param name="bounds">Bounds</param>
        internal Entity(int row, int col, Rectangle bounds) 
        {
            //Set the row and column
            this._row = row;
            this._col = col;

            //Set the bounds
            Bounds = bounds;
        }
    }
}
