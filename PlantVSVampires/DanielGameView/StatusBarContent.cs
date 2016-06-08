/* Daniel Berezovski
 * June 1st, 2016
 * This class draws the status bar
 */
using Plant_VS.Vampires;
using System;
using System.Drawing;

namespace DanielView
{
    internal class StatusBarContent
    {
        //Screen width
        private int _screenWidth;

        //The font of all of the text in the status bar
        private Font _statusBarFont = new Font(FontFamily.GenericSansSerif, 20);

        //Moon image
        private Bitmap _moon = Properties.Resources.moon;

        //Bounds of mooon
        private RectangleF _moonBounds;

        //Model Wrapper
        private ModelWrapper _wrapper;

        //Offset of the status bar
        private int _statusBarOffset;

        //Seconds and minutes strings
        private const string SECONDS = "s";
        private const string MINUTES = "m";

        //Health constant
        private const string HEALTH = "Health: ";

        //Wave constant
        private const string WAVE = "Wave: ";

        //Time that the game started
        private DateTime _startTime;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="wrapper">The model wrapper used</param>
        /// <param name="screenWidth">The width of the screen</param>
        /// <param name="statusBarOffset">The status bar offset</param>
        internal StatusBarContent(ModelWrapper wrapper, int screenWidth, int statusBarOffset)
        {
            //Make the screen width global
            _screenWidth = screenWidth;

            //Make the model wrapper global
            _wrapper = wrapper;

            //Make the status bar offset global
            _statusBarOffset = statusBarOffset;

            //Make the bounds of the moon
            _moonBounds = new RectangleF();
            _moonBounds.Size = new SizeF(_statusBarOffset / 1.3F, _statusBarOffset / 1.3F);

            //Make the location of the moon
            float xImage = 0;
            float yImage = (_statusBarOffset / 2F) - (_moonBounds.Height / 2F);

            //Store the location in a PointF
            PointF moonLocation = new PointF(xImage, yImage);

            //Update the location in the bounds
            _moonBounds.Location = moonLocation;

            //Set the start time to now
            _startTime = DateTime.Now;
        }

        /// <summary>
        /// Draws the status bar
        /// </summary>
        /// <param name="g">Graphics to draw on</param>
        internal void DrawStatusBar(Graphics g)
        {
            //Create the status bar bounds
            Rectangle statusBar = new Rectangle(0, 0, _screenWidth, _statusBarOffset);
            Image image = Properties.Resources.background;

            //Draw the status bar
            g.DrawImage(image, statusBar);

            //Draw the moons
            DrawPlayerMoons(g);

            //Draw the health
            DrawHealth(g);

            //Draw the wave number
            DrawWave(g);

            //Draw the game length
            DrawGameLength(g);
        }

        /// <summary>
        /// Draws the player's moons
        /// </summary>
        /// <param name="g">Graphics to draw on</param>
        private void DrawPlayerMoons(Graphics g)
        {
            //Moons string
            string moons = _wrapper.GetPlayerMoons().ToString();

            //Size of the moons string
            SizeF textSize = g.MeasureString(moons, _statusBarFont);

            //Location of the string
            float x = _moonBounds.Width + 2;
            float y = (_statusBarOffset / 2F) - (textSize.Height / 2F);
            PointF location = new PointF(x, y);

            //Draw the moon
            g.DrawImage(_moon, _moonBounds);

            //Draw the amount
            g.DrawString(moons, _statusBarFont, Brushes.White, location);
        }

        /// <summary>
        /// Draws the player's health
        /// </summary>
        /// <param name="g">Graphics to draw on</param>
        private void DrawHealth(Graphics g)
        {
            //Health string
            string health = HEALTH + _wrapper.GetHealth().ToString();

            //Size of the health string
            SizeF healthSize = g.MeasureString(health, _statusBarFont);

            //Location of the string
            float x = (_screenWidth / 3F) - (healthSize.Width / 2F);
            float y = (_statusBarOffset / 2F) - (healthSize.Height / 2F);

            PointF location = new PointF(x, y);

            //Draw the health
            g.DrawString(health, _statusBarFont, Brushes.White, location);
        }

        /// <summary>
        /// Draws the wave number
        /// </summary>
        /// <param name="g">Graphcis to draw on</param>
        private void DrawWave(Graphics g)
        {
            //Wave number string
            string wave = WAVE + _wrapper.GetWave().ToString();

            //Size of wave number string
            SizeF waveSize = g.MeasureString(wave, _statusBarFont);

            //Location of the wave number
            float x = (_screenWidth / 1.5F) - (waveSize.Width / 2F);
            float y = (_statusBarOffset / 2F) - (waveSize.Height / 2F);

            PointF location = new PointF(x, y);

            //Draw the wave number
            g.DrawString(wave, _statusBarFont, Brushes.White, location);
        }

        /// <summary>
        /// Draws the game length
        /// </summary>
        /// <param name="g">Graphics to draw on</param>
        private void DrawGameLength(Graphics g)
        {
            //Time game is running
            string time = GetTimeString();

            //Size of the time
            SizeF timeSize = g.MeasureString(time, _statusBarFont);

            //Location of the time
            float x = _screenWidth - timeSize.Width;
            float y = (_statusBarOffset / 2F) - (timeSize.Height / 2F);

            PointF location = new PointF(x, y);

            //Draw the time
            g.DrawString(time, _statusBarFont, Brushes.White, location);
        }

        /// <summary>
        /// Generates the time string in an organized manner
        /// </summary>
        /// <returns>The time string</returns>
        private string GetTimeString()
        {
            //Get the seconds and minutes
            int seconds = (DateTime.Now - _startTime).Seconds;
            int minutes = (DateTime.Now - _startTime).Minutes;

            //Get the string version
            string secondsString = seconds.ToString() + SECONDS;
            string minutesString = String.Empty;

            //Check if more than a minute passed
            if (minutes > 0)
            {
                //Set the minute string
                minutesString = minutes.ToString() + MINUTES + " : ";
            }

            //Return the two time strings
            return minutesString + secondsString;
        }

        /// <summary>
        /// Get the location of the main moon
        /// </summary>
        /// <returns>The moon's location</returns>
        internal PointF GetMoonLocation()
        {
            //Location of the moon
            return _moonBounds.Location;
        }
    }
}
