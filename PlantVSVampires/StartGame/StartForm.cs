/* Daniel Berezovski
 * June 1st, 2016
 * This class starts the program, and gives the player two options for each game
 */
using DanielView;
using ElliotView;
using System;
using System.Windows.Forms;

namespace StartGame
{
    public partial class StartForm : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StartForm()
        {
            //Initialize all components
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when user clicks  to play Elliot's game
        /// </summary>
        /// <param name="sender">Object that was clicked</param>
        /// <param name="e">Info about the event</param>
        private void btnElliot_Click(object sender, EventArgs e)
        {
            ElliotGameView view = new ElliotGameView(this);
            view.Show();
            this.Hide();
        }

        /// <summary>
        /// Occurs when user clicks  to play Daniel's game
        /// </summary>
        /// <param name="sender">Object that was clicked</param>
        /// <param name="e">Info about the event</param>
        private void btnDaniel_Click(object sender, EventArgs e)
        {
            DanielGameView view = new DanielGameView(this);
            view.Show();
            this.Hide();
        }
    }
}
