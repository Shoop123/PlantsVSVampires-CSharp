/* Daniel Berezovski
 * June 1st, 2016
 * This class draws everythings when the player lost
 */
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace DanielView
{
    internal class LostGame
    {
        //Stores if the player lost
        private bool _lost = false;

        //Color matrix for image opacity
        private ColorMatrix _matrix = new ColorMatrix();

        //Create image attributes
        private ImageAttributes _attributes = new ImageAttributes();

        //Speed of dissapearing effect
        private float OPACITY_SPEED = 0.1f;

        //Current opacity of screen
        private float _opacity = 1;

        //Lost string
        private const string LOST = "You Lose :'(";

        //Font of the lost text
        private Font _lostFont = new Font(FontFamily.GenericSerif, 100);

        ///Continue string
        private const string CONT = "Continue";

        //Continue font
        private Font _contFont = new Font(FontFamily.GenericSerif, 26);

        //Mouse location
        private int _mouseX;
        private int _mouseY;

        //Stores if the mouse is pressed
        private bool _mouseDown;

        //The form to return to
        private Form _backForm;

        //The current form
        private Form _currentForm;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="currentForm">Current form</param>
        /// <param name="backForm">Form to return to</param>
        internal LostGame(Form currentForm, Form backForm)
        {
            //Make the current form global
            _currentForm = currentForm;

            //Make the back form global
            _backForm = backForm;
        }

        /// <summary>
        /// Getter and setter for opacity
        /// </summary>
        private float Opacity
        {
            //Get the opacity
            get { return _opacity; }
            //Set it
            set 
            {
                //Check if the value is greater than or equal to 0
                if (value >= 0)
                    //Set the opacity
                    _opacity = value;
                //Otherwise
                else
                    //Set it to 0
                    _opacity = 0;
            }
        }

        /// <summary>
        /// Start the losing process
        /// </summary>
        internal void InitLose()
        {
            _lost = true;
        }

        /// <summary>
        /// Handles the losing process
        /// </summary>
        /// <param name="bmp">The image to draw</param>
        /// <returns></returns>
        internal Bitmap GameLost(Bitmap bmp)
        {
            //Check if the game is lost
            if (_lost)
            {
                //Decrease the opacity
                Opacity -= OPACITY_SPEED;

                //Check if there still is opacity to decrease
                if(Opacity > 0)
                    //Return the more opaque image
                    return SetImageOpacity(bmp);

                //Return the lose screen
                return ShowLoseScreen(bmp);
            }

            //Return null
            return null;
        }

        /// <summary>
        /// Shows the screen when the player loses
        /// </summary>
        /// <param name="bmp">Changes the image</param>
        /// <returns></returns>
        private Bitmap ShowLoseScreen(Bitmap bmp)
        {
            //Get the graphics from the image
            using (Graphics g = Graphics.FromImage(bmp))
            {
                //Size of the lost text
                SizeF sizeOfText = g.MeasureString(LOST, _lostFont);

                //Location of the text
                PointF center = new PointF();
                center.X = (bmp.Width / 2f) - sizeOfText.Width / 2f;
                center.Y = (bmp.Height / 2f) - sizeOfText.Height;

                //Clear the image
                g.Clear(Color.Transparent);

                //Draw the string
                g.DrawString(LOST, _lostFont, Brushes.Black, center);

                //Draw the continue button
                DrawContinueButton(g, bmp.Width, bmp.Height);
            }

            //Return the image
            return bmp;
        }

        /// <summary>
        /// Sets the image opacity
        /// </summary>
        /// <param name="bmp">Image to make opaque</param>
        /// <returns>The more opaque image</returns>
        private Bitmap SetImageOpacity(Bitmap bmp)
        {
            //Create a different image
            Bitmap bitmap = new Bitmap(bmp.Width, bmp.Height);

            //create a graphics object from the image  
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                //set the opacity
                _matrix.Matrix33 = Opacity;

                //set the color(opacity) of the image  
                _attributes.SetColorMatrix(_matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                //now draw the image  
                g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, _attributes);
            }

            //Return the new image
            return bitmap;
        }

        /// <summary>
        /// Draws the continue button
        /// </summary>
        /// <param name="g">Graphics to draw on</param>
        /// <param name="width">Width of the screen</param>
        /// <param name="height">Height of the screen</param>
        private void DrawContinueButton(Graphics g, int width, int height)
        {
            //Bounds of the button
            Rectangle buttonBounds = new Rectangle();
            buttonBounds.Size = new Size(150, 50);
            buttonBounds.Location = new Point((width / 2) - (buttonBounds.Width / 2), height - height / 3);

            //Draw the rectangle
            g.DrawRectangle(Pens.Black, buttonBounds);

            //Check if the mouse intersects with the button
            if(StaticMethods.MouseIntersects(buttonBounds.X, buttonBounds.Y, buttonBounds.Width, buttonBounds.Height, _mouseX, _mouseY))
            {
                //Draw the continue string in green
                g.DrawString(CONT, _contFont, Brushes.Green, GetCenterTextRectangle(buttonBounds, g.MeasureString(CONT, _contFont)));

                //Check if the mouse button has pressed
                if (_mouseDown)
                {
                    //Invoke the back method on the main thread
                    _currentForm.Invoke(new Action(BackToMainScreen));
                }
            }
            //Otherwise...
            else
                //Draw the continue string in black
                g.DrawString(CONT, _contFont, Brushes.Black, GetCenterTextRectangle(buttonBounds, g.MeasureString(CONT, _contFont)));
        }

        /// <summary>
        /// Goes back to the main screen
        /// </summary>
        private void BackToMainScreen()
        {
            //Show the back form, close this form
            _backForm.Show();
            _currentForm.Close();
        }

        /// <summary>
        /// Gets the center for text in a rectangle
        /// </summary>
        /// <param name="bounds">Rectangle</param>
        /// <param name="textSize">The size of the text</param>
        /// <returns></returns>
        private Rectangle GetCenterTextRectangle(Rectangle bounds, SizeF textSize)
        {
            //Recreate the text bounds
            Rectangle textBounds = bounds;

            //Get the center location
            int x = bounds.X + (textBounds.Width / 2) - ((int)textSize.Width / 2);
            int y = bounds.Y + (textBounds.Height / 2) - ((int)textSize.Height / 2);

            //Set the text location
            textBounds.X = x;
            textBounds.Y = y;

            //Return the bounds
            return textBounds;
        }

        /// <summary>
        /// Updates the location of the mouse
        /// </summary>
        /// <param name="mouseX">X location of the mouse</param>
        /// <param name="mouseY">Y location of the mouse</param>
        internal void UpdateMouseMove(int mouseX, int mouseY)
        {
            //Update the mouse coordinates
            _mouseX = mouseX;
            _mouseY = mouseY;
        }

        /// <summary>
        /// Updates the mouse pressed state
        /// </summary>
        /// <param name="mouseDown">Mouse down</param>
        internal void UpdateMouseDown(bool mouseDown)
        {
            //Set the mouse state
            _mouseDown = mouseDown;
        }

        /// <summary>
        /// Gets if the screen is faded
        /// </summary>
        /// <returns>If the screen is faded</returns>
        internal bool IsFaded()
        {
            //Return if the opacity is less than 0
            return Opacity <= 0;
        }
    }
}
