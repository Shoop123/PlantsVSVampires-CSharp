// Elliot Spall, 
// June 4th 2016,
// Handles the creation of the ElliotGameView form.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Plant_VS.Vampires.Plants;
using Plant_VS.Vampires;

namespace ElliotView
{
    public partial class ElliotGameView : Form
    {
        // Creates a private variable to store the start form.
        private Form _startForm;

        // Creates a constant to store the number of rows.
        private const int ROWS = 5;

        // Creates a constant to store the number of columns.
        private const int COLS = 10;

        // Creates a constant to store the offset from the top of the screen to the board.
        private const int TOP_OFFSET = 100;

        // Creates a constant to store the offset from the left side of the screen to the board.
        private const int SELECTION_OFFSET = 217;

        // Creates a constant to store the height of each board tile.
        private const int TILE_HEIGHT = 170;

        // Creates a constant to store the width of each board tile.
        private const int TILE_WIDTH = 170;

        // Creates a pen with a semi-transparent color.
        private Pen _pen = new Pen(Color.FromArgb(85, Color.Black), 5);

        // Creates a bool to store whether or not the game is running.
        private bool? _isGameRunning;

        // Creates a constant to store the target FPS.
        private const int FPS = 1000 / 60;

        // Creates a constant to store the message for insufficient moons.
        private const string INSUFFICIENT_MOONS = "You don't have enough moons to buy that plant!";

        // Creates a constant to store the message for an occupied tile.
        private const string TILE_OCCUPIED = "The tile you are trying to place a plant in is already occupied!";

        // Creates a constant to store the message for no trees around a tile.
        private const string NO_TREE = "You cannot place a plant outside the radius of a tree!";   

        // Creates a 2D array of rectangles to store the board tiles.
        private Rectangle[,] _board = new Rectangle[ROWS, COLS];       

        // Creates a list of rectangles to store the plant selection tiles.
        private List<Rectangle> _plantSelection = new List<Rectangle>();

        // Creates an instance of the modelwrapper.
        private ModelWrapper _wrapper = new ModelWrapper(ROWS, COLS, TILE_WIDTH, TILE_HEIGHT, Screen.PrimaryScreen.Bounds.Width, TOP_OFFSET, SELECTION_OFFSET, 0);

        // Creates a variable to store the location of the mouse cursor.
        private Point _mouseLocation;

        // Creates a bool to store whether the user is holding down mouse 1.
        private bool _isMouseDown = false;

        // Creates a bool to store whether the user is holding a plant.
        private bool _isGrabbingPlant = false;

        // Creates a variable to store the index of the plant the user is holding.
        private int _grabbedPlantIndex;

        // Creates a 2D array of progressbars to store the healthbars of the plants on the board.
        private ProgressBar[,] _plantHealthBars = new ProgressBar[ROWS, COLS];

        // Creates a semi-transparent brush for the color red.
        private SolidBrush _redTransparentBrush = new SolidBrush(Color.FromArgb(75, Color.Red));

        // Creates a semi-transparent brush for the color green.
        private SolidBrush _greenTransparentBrush = new SolidBrush(Color.FromArgb(75, Color.Green));

        // Creates a brush to be used for drawing strings.
        private SolidBrush _stringBrush = new SolidBrush(Color.FromArgb(0, Color.Black));

        // Creates a string variable to store the error message.
        private string _errorMessage;

        /// <summary>
        /// The Constructor for ElliotGameView
        /// </summary>
        /// <param name="startForm"></param>
        public ElliotGameView(Form startForm)
        {
            // Initializes the form.
            InitializeComponent();            

            // Sets the startform to the passed in startform.
            _startForm = startForm;
        }

        /// <summary>
        /// When ElliotGameView is first shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ElliotGameView_Shown(object sender, EventArgs e)
        {
            // Starts the main game looop.
            GameLoop();
        }

        /// <summary>
        /// Handles constantly updating the game at specific intervals
        /// </summary>
        public void GameLoop()
        {
            // Makes the game run.
            _isGameRunning = true;

            // Creates a variable to store the last time the game was updated.
            int lastUpdate = Environment.TickCount;

            // Runs
            do
            {
                // Allows the application to keep taking user input;
                Application.DoEvents();

                // Creates a variable to store the current time;
                int now = Environment.TickCount;

                // Creates a variable to store the time between the current time and the last update.
                int passed = now - lastUpdate;

                // If the time passed is greater than or equal to the constant FPS value.
                if (passed >= FPS)
                {
                    // Calls the method to place a grabbed plant.
                    PlaceGrabbedPlant();

                    // Updates the label texts.
                    lblMoons.Text = "Moons: " + _wrapper.GetPlayerMoons().ToString();
                    lblHealth.Text = "Health: " + _wrapper.GetHealth().ToString();
                    lblWave.Text = "Wave: " + (_wrapper.GetWave() + 1).ToString();

                    // Calls the wrapper to make moons and passes in the location of the moon amount label.
                    _wrapper.MakeMoons(lblMoons.Location);

                    // Causes the form to re-draw.
                    Refresh();
                }

                // If the player has lost (Health is less than or equal to 0).
                if (_wrapper.PlayerLost())
                {
                    // Stops the game.
                    _isGameRunning = false;
                }
            }
            // While the game is running.
            while (_isGameRunning == true);

            // Makes all the labels invisible.
            lblHealth.Visible = false;
            lblMoons.Visible = false;
            lblWave.Visible = false;

            // Loops through all of the controls on the form.
            foreach (Control control in Controls)
            {
                // Removes each control.
                Controls.Remove(control);
            }

            // Runs
            do
            {
                // Allows the application to keep taking user input;
                Application.DoEvents();

                // Creates a variable to store the current time;
                int now = Environment.TickCount;

                // Creates a variable to store the time between the current time and the last update.
                int passed = now - lastUpdate;

                // If the time passed is greater than or equal to the constant FPS value.
                if (passed >= FPS)
                {
                    // Causes the form to re-draw.
                    Refresh();
                }
            }
            // While the game is not running.
            while (_isGameRunning == false);
        }

        /// <summary>
        /// Handles drawing the board
        /// </summary>
        /// <param name="g"></param>
        public void DrawGameBoard(Graphics g)
        {
            // Creates a variable to store the x-coordinate of a tile.
            int x;

            // Creates a variable to store the y-coordinate of a tile and gives it the constant top offset.
            int y = TOP_OFFSET;

            // Loops through the rows from 0 until the constant row value.
            for (int row = 0; row < ROWS; row++)
            {
                // Sets the x value to the constant selection offset.
                x = SELECTION_OFFSET;

                // Loops through the cols from 0 until the constant col value.
                for (int col = 0; col < COLS; col++)
                {
                    // Creates a new rectangle at the specific position in the board array.
                    _board[row, col] = new Rectangle(x, y, TILE_WIDTH, TILE_HEIGHT);                 

                    // Increases the x coordinate by the constant tile width.
                    x += TILE_WIDTH;
                }

                // Increases the y coorindate by the constant tile height.
                y += TILE_HEIGHT;
            }


            // Loops through the rows from 0 until the constant row value.
            for (int row = 0; row < ROWS; row++)
            {               
                // Loops through the cols from 0 until the constant col value.
                for (int col = 0; col < COLS; col++)
                {
                    // Draws the specific board rectangle.
                    g.DrawRectangle(_pen, _board[row, col]);

                    // Fills the specific board rectangle.
                    g.FillRectangle(new SolidBrush(_pen.Color), _board[row, col]);

                    // If the board rectangle contains the mouse location.
                    if (_board[row, col].Contains(_mouseLocation))
                    {
                        // Fills the specific board rectangle with a darker shade of black.
                        g.FillRectangle(new SolidBrush(Color.FromArgb(_pen.Color.A + 30, Color.Black)), _board[row, col]);
                    }

                    // If the player can place a plant at the specified location, the mouse is down, and the user is holding a plant.
                    if (CanPlacePlantAtLocation(row, col, _board[row, col]) && _isMouseDown && _isGrabbingPlant)
                    {
                        // Fills the specific board rectangle with a green shade.
                        g.FillRectangle(_greenTransparentBrush, _board[row, col]);
                    }
                    // If the player cannot place a plant at the specified location, the mouse is down, and the user is holding a plant.
                    else if (!CanPlacePlantAtLocation(row, col, _board[row, col]) && _isMouseDown && _isGrabbingPlant)
                    {
                        // Fills the specific board rectangle with a red shade.
                        g.FillRectangle(_redTransparentBrush, _board[row, col]);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the drawing of the plant selection menu
        /// </summary>
        /// <param name="g"></param>
        public void DrawPlantSelection(Graphics g)
        {
            // Creates a variable to store the y-coordinate of the menu tile and gives it the constant top offset.
            int y = TOP_OFFSET;

            // Creates a variable to store the menu plant tile height.
            int plantTileHeight = (Screen.PrimaryScreen.Bounds.Height - 2 * TOP_OFFSET - SystemInformation.CaptionHeight) / _wrapper.GetPlants().Length;

            // Loops from 0 until the amount of plants there are.
            for (int i = 0; i < _wrapper.GetPlants().Length; i++)
            {
                // Adds a new rectangle to the plant tile selection list.
                _plantSelection.Add(new Rectangle(0, y, TILE_WIDTH - 10, plantTileHeight));

                // Fills the plant selection tile with a shade of black that gets darker each tim the loop iterates.
                g.FillRectangle(new SolidBrush(Color.FromArgb(_pen.Color.A + i * 15, Color.Black)), _plantSelection[i]);

                // Draws the plant image at the specified rectangle.
                g.DrawImage(Graphic.GetImage(_wrapper.GetPlants()[i]), _plantSelection[i]);

                // Increases the y coordinate by the height of the plant menu tile.
                y += plantTileHeight;
            }
        }

        /// <summary>
        /// Handles the drawing of vampires
        /// </summary>
        /// <param name="g"></param>
        public void DrawVampires(Graphics g)
        {
            // Calls the wrapper to update.
            _wrapper.Update();

            // Loops through each vampire in the wrapper's list of vampires.
            foreach (Vampire vampire in _wrapper.GetVampires())
            {
                // Draws the vampire image at its bounds.
                g.DrawImage(Graphic.GetImage(vampire), vampire.Bounds);
            }
        }

        /// <summary>
        /// Handles the drawing of the plants on the board
        /// </summary>
        /// <param name="g"></param>
        public void DrawPlantsOnBoard(Graphics g)
        {
            // Loops through the rows from 0 until the constant row value.
            for (int row = 0; row < ROWS; row++)
            {
                // Loops through the cols from 0 until the constant col value.
                for (int col = 0; col < COLS; col++)
                {
                    // Creates a variable to store the plant that is at the specified board location.
                    Plant plant = _wrapper.GetBoard()[row, col];

                    // If the plant is not null.
                    if (plant != null)
                    {
                        // Draws the plant's image at the specified board location.
                        g.DrawImage(Graphic.GetImage(plant), _board[row, col]);

                        // Calls the method to check the plant's health.
                        CheckPlantHealth(row, col);
                    }

                    // If the plant is null.
                    if (plant == null)
                    {
                        // Calls the method to remove the healthbar of the specific plant.
                        RemoveHealthBars(row, col);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the drawing of the plant the user is holding
        /// </summary>
        /// <param name="g"></param>
        public void DrawGrabbedPlant(Graphics g)
        {
            // If the mouse is down and the user is holding a plant.
            if (_isMouseDown && _isGrabbingPlant)
            {
                // Creates a rectangle to store the bounds for the held plant.
                Rectangle grabbedPlantBounds = new Rectangle((_mouseLocation.X - (TILE_WIDTH / 2)), (_mouseLocation.Y - (TILE_HEIGHT / 2)), TILE_WIDTH, TILE_HEIGHT);

                // Draws the plant's image at the aforementioned bounds.
                g.DrawImage(Graphic.GetImage(_wrapper.GetPlants()[_grabbedPlantIndex]), grabbedPlantBounds);
            }
        }

        /// <summary>
        /// Handles the drawing of the projectiles
        /// </summary>
        /// <param name="g"></param>
        public void DrawProjectiles(Graphics g)
        {
            // Loops through each projectile in the wrapper's list of projectiles.
            foreach (Projectile projectile in _wrapper.GetProjectiles())
            {
                // Creates a variable to store the image of said projectile.
                Image image = Graphic.GetImage(projectile);

                // Creates a variable to store the row of said projectile.
                int row = projectile.Row;

                // Creates a variable to store the column of said projectile.
                int col = projectile.Col;

                // if the projectile is a bomb projectile.
                if (projectile.Type == Projectile.ProjectileType.BombProjectile)
                {                    
                    try
                    {
                        // Tries drawing the image at the specified board location.
                        g.DrawImage(image, _board[row - 1, col - 1]);
                    }
                    // Catches any exceptions.
                    catch { }

                    try
                    {
                        // Tries drawing the image at the specified board location.
                        g.DrawImage(image, _board[row - 1, col]);
                    }
                    // Catches any exceptions.
                    catch { }

                    try
                    {
                        // Tries drawing the image at the specified board location.
                        g.DrawImage(image, _board[row - 1, col + 1]);
                    }
                    // Catches any exceptions.
                    catch { }

                    try
                    {
                        // Tries drawing the image at the specified board location.
                        g.DrawImage(image, _board[row, col - 1]);
                    }
                    // Catches any exceptions.
                    catch { }

                    try
                    {
                        // Tries drawing the image at the specified board location.
                        g.DrawImage(image, _board[row, col]);
                    }
                    // Catches any exceptions.
                    catch { }

                    try
                    {
                        // Tries drawing the image at the specified board location.
                        g.DrawImage(image, _board[row, col + 1]);
                    }
                    // Catches any exceptions.
                    catch { }

                    try
                    {
                        // Tries drawing the image at the specified board location.
                        g.DrawImage(image, _board[row + 1, col - 1]);
                    }
                    // Catches any exceptions.
                    catch { }

                    try
                    {
                        // Tries drawing the image at the specified board location.
                        g.DrawImage(image, _board[row + 1, col]);
                    }
                    // Catches any exceptions.
                    catch { }

                    try
                    {
                        // Tries drawing the image at the specified board location.
                        g.DrawImage(image, _board[row + 1, col + 1]);
                    }
                    // Catches any exceptions.
                    catch { }
                }

                // If the projectile is a mine projectile. 
                else if (projectile.Type == Projectile.ProjectileType.MineProjectile)
                {
                    // Draws the image at the specified board location.
                    g.DrawImage(image, _board[row, col]);
                }

                // If the projectile is anything else.
                else
                {
                    // Draws the image in its bounds.
                    g.DrawImage(image, projectile.Bounds);
                }
            }
        }

        /// <summary>
        /// Handles the drawing of moons on the board
        /// </summary>
        /// <param name="g"></param>
        public void DrawMoons(Graphics g)
        {
            // Loops through the rows from 0 until the constant row value.
            for (int row = 0; row < ROWS; row++)
            {
                // Loops through the cols from 0 until the constant col value.
                for (int col = 0; col < COLS; col++)
                {
                    // If the plant at the specific board location is a moon flower.
                    if (_wrapper.GetBoard()[row, col] is MoonFlower)
                    {
                        // Creates a moonflower to store the specific plant at the board location as a moonflower.
                        MoonFlower moonflower = _wrapper.GetBoard()[row, col] as MoonFlower;

                        // Creates a list of moons which are to be removed.
                        List<Moon> moonsToBeRemoved = new List<Moon>();

                        // Loops through each moons in the wrapper's list of moons to be drawn.
                        foreach (Moon moon in _wrapper.GetMoonsToDraw(moonflower))
                        {
                            // Draws the moon at its bounds.
                            g.DrawImage(Graphic.GetImage(moon), moon.Bounds);

                            // Calls the method to handle the user's grabbing of a moon.
                            GrabMoon(moon, moonflower);

                            // Checks if the moon is expired and ready to be collected.
                            if (moon.IsExpired)
                            {
                                // Adds that specific moon to the list of moons to be removed.
                                moonsToBeRemoved.Add(moon);
                            }
                        }

                        // Adds the amount of moons that need to be removed to the total amount of moons.
                        _wrapper.AddMoons(moonsToBeRemoved.Count);

                        // Loops through each moon in the list of moons to be removed.
                        foreach (Moon moonToBeRemoved in moonsToBeRemoved)
                        {
                            // Removes the specific moon from the specific moonflower from the wrapper's list of moons to be drawn.
                            _wrapper.GetMoonsToDraw(moonflower).Remove(moonToBeRemoved);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the drawing of the lost message
        /// </summary>
        /// <param name="g"></param>
        public void DrawLostMessage(Graphics g)
        {
            // Draws "You Lose!" in the bounds of the lost label.
            g.DrawString("You Lose!", new Font("Champagne & Limousines", 120, FontStyle.Bold), _stringBrush, lblLost.Bounds);

            // If the alpha value of the string brush color + 2 is greater than 255.
            if (_stringBrush.Color.A + 2 > 255)
            {
                // Creates a new solid brush using the old brush's color.
                _stringBrush = new SolidBrush(_stringBrush.Color);
            }  
            
            // Otherwise          
            else
            {
                // Creates a new solid brush using the old brush's color, and increases it's alpha value by 2.
                _stringBrush = new SolidBrush(Color.FromArgb(_stringBrush.Color.A + 2, Color.Black));
            }
        }

        /// <summary>
        /// Handles the drawing of the error message
        /// </summary>
        /// <param name="g"></param>
        public void DrawErrorMessage(Graphics g)
        {
            // Draws the error message in a specific bounds.
            g.DrawString(_errorMessage, new Font("Champagne & Limousines", 20, FontStyle.Bold), Brushes.Black, new Point(SELECTION_OFFSET, Bottom - TOP_OFFSET));
        }

        /// <summary>
        /// Handles the user grabbing a moon
        /// </summary>
        /// <param name="moon"></param>
        /// <param name="moonflower"></param>
        public void GrabMoon(Moon moon, MoonFlower moonflower)
        {
            // If the mouse is down and the mouse location is within the bounds of the passed in moon.
            if (_isMouseDown && moon.Bounds.Contains(_mouseLocation))
            {
                // Sets the moon's isCollected property to true.
                moon.IsCollected = true;

                // Calls the wrapper to collect the specified moon at the specified moonflower.
                _wrapper.CollectMoon(moonflower, moon);
            }
        }

        /// <summary>
        /// Handles the placing of a grabbed plant
        /// </summary>
        public void PlaceGrabbedPlant()
        {
            // If the mouse isn't down and the user is holding a plant.
            if (!_isMouseDown && _isGrabbingPlant)
            {
                // Loops through the rows from 0 until the constant row value.
                for (int row = 0; row < ROWS; row++)
                {
                    // Loops through the cols from 0 until the constant col value.
                    for (int col = 0; col < COLS; col++)
                    {
                        // If the mouse location is within the specific board location.
                        if (_board[row, col].Contains(_mouseLocation))
                        {
                            // Creates an instance of the specific plant type given from the specific board location.
                            Plant plantToPlace = (Plant)Activator.CreateInstance(_wrapper.GetPlants()[_grabbedPlantIndex].GetType(), row, col, _board[row, col]);

                            // If the plant could be placed.
                            if (_wrapper.GetReason(row, col, plantToPlace) == ModelWrapper.Reason.Accepted)
                            {
                                // Sets the error message to a blank string.
                                _errorMessage = "";
                            }

                            // If the reason why a plant couldn't be placed is because of insufficient moons.
                            else if (_wrapper.GetReason(row, col, plantToPlace) == ModelWrapper.Reason.InsufficientMoons)
                            {
                                // Sets the error message to sthe constant string for insufficient moons.
                                _errorMessage = INSUFFICIENT_MOONS;
                            }

                            // If the reason why a plant couldn't be placed is because of no trees around it.
                            else if (_wrapper.GetReason(row, col, plantToPlace) == ModelWrapper.Reason.NoTreesNearby)
                            {
                                // Sets the error message to the constant string for no trees around the plant.
                                _errorMessage = NO_TREE;
                            }

                            // If the reason why a plant couldn't be place is because the tile already contains a plant.
                            else if (_wrapper.GetReason(row, col, plantToPlace) == ModelWrapper.Reason.Occupied)
                            {
                                // Sets the error message to the constant string for the tile is occupied.
                                _errorMessage = TILE_OCCUPIED;
                            }

                            // Calls the wrapper to buy and place the specific plant at the specific row and column.
                            _wrapper.BuyAndPlace(row, col, plantToPlace);                          

                            // Creates a new progress bar.
                            ProgressBar newPlantHealthBar = new ProgressBar();

                            // Sets the bounds of said progress bar.
                            newPlantHealthBar.SetBounds(plantToPlace.Bounds.X + 3, plantToPlace.Bounds.Bottom - 15, plantToPlace.Bounds.Width - 7, 12);

                            // Sets the maximum value of the progress bar the current health of the specific plant.
                            newPlantHealthBar.Maximum = _wrapper.GetPlantHealth(plantToPlace);

                            // Sets the value of the progress var to be its maximum value.
                            newPlantHealthBar.Value = newPlantHealthBar.Maximum;                            

                            // Adds the said progress bar to the 2D array of progress bars.
                            _plantHealthBars[row, col] = newPlantHealthBar;

                            // Adds said progress bar to the control collection of the form.
                            Controls.Add(newPlantHealthBar);

                            // Stops the plant from being held.
                            _isGrabbingPlant = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the checking of a plant's health
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void CheckPlantHealth(int row, int col)
        {
            // If the plant at the specific wrapper board location is not null.
            if (_wrapper.GetBoard()[row, col] != null)
            {
                // Creates a variable to store the health of the plant at said location.
                int health = _wrapper.GetPlantHealth(_wrapper.GetBoard()[row, col]);

                // If the health is greater than zero.
                if (health > 0)
                {
                    // Updates the progress bar's value to the current health amount of the plant.
                    _plantHealthBars[row, col].Value = _wrapper.GetPlantHealth(_wrapper.GetBoard()[row, col]);
                }

                // If the health is less than 0.
                else if (health < 0)
                {
                    // Sets the progress bar's value to 0.
                    _plantHealthBars[row, col].Value = 0;
                }
            }
        }

        /// <summary>
        ///  Handles the removal of a plant heatlh bar.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void RemoveHealthBars(int row, int col)
        {
            // If the specific board location no longer contains a plant.
            if (_wrapper.GetBoard()[row, col] == null)
            {
                // Loops through each control in the form's control collection.
                foreach (Control control in Controls)
                {
                    // If the health bar of the specific plant is not null.
                    if (_plantHealthBars[row, col] != null)
                    {
                        // If the health bar of the specific plant equals the certain control.
                        if (_plantHealthBars[row, col].Equals(control))
                        {
                            // Removes the health bar from the form's control collection.
                            Controls.Remove(control);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles checking whether a plant can be placed at a specific location
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="boardRectangle"></param>
        /// <returns></returns>
        public bool CanPlacePlantAtLocation(int row, int col, Rectangle boardRectangle)
        {
            // If the user is grabbing a plant.
            if (_isGrabbingPlant)
            {
                // If a plant is able to placed at the specified location.
                if (_wrapper.CheckPlace(row, col, boardRectangle, _wrapper.GetPlants()[_grabbedPlantIndex]))
                {
                    // Returns true.
                    return true;
                }
                
                // Otherwise
                else
                {
                    // Returns false.
                    return false;
                }
            }

            // Returns false;
            return false;
        }

        /// <summary>
        /// Handles the painting of ElliotGameView
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Calls the base on paint method.
            base.OnPaint(e);

            // If the game is running.
            if (_isGameRunning == true)
            {
                // Calls the method to draw the board.
                DrawGameBoard(e.Graphics);

                // Calls the method to draw the vampires.
                DrawVampires(e.Graphics);

                // Calls the method to draw the plant selection menu.
                DrawPlantSelection(e.Graphics);

                // Calls the method to draw the plant which the user is holding.
                DrawGrabbedPlant(e.Graphics);

                // Calls the method to draw all the plants on the board.
                DrawPlantsOnBoard(e.Graphics);

                // Calls the method to draw all the projectiles in the game.
                DrawProjectiles(e.Graphics);

                // Calls the method to draw all the moons in the game.
                DrawMoons(e.Graphics);

                // Calls the method to draw the error message.
                DrawErrorMessage(e.Graphics);
            }

            // If the game is not running.
            else if (_isGameRunning == false)
            {
                // Calls the method to draw the game lost message.
                DrawLostMessage(e.Graphics);
            }
        }

        /// <summary>
        /// Handles the mouse down event of ElliotGameView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ElliotGameView_MouseDown(object sender, MouseEventArgs e)
        {
            // The mouse is now down.
            _isMouseDown = true;

            // Loops through each tile in the plant selection tile list.
            foreach (Rectangle tile in _plantSelection)
            {
                // If the mouse location is within said tile.
                if (tile.Contains(e.Location))
                {
                    // Sets the grabbed plant index to the index of the specific tile.
                    _grabbedPlantIndex = _plantSelection.IndexOf(tile);

                    // The user is now grabbing a plant.
                    _isGrabbingPlant = true;
                }
            }
        }

        /// <summary>
        /// Handles the mouse up event of ElliotGameView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ElliotGameView_MouseUp(object sender, MouseEventArgs e)
        {
            // The mouse is no longer down.
            _isMouseDown = false;
        }

        /// <summary>
        /// Handles the mouse move event of ElliotGameView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ElliotGameView_MouseMove(object sender, MouseEventArgs e)
        {
            // Sets the mouse location variable to the location of the mouse cursor.
            _mouseLocation = e.Location;
        }

        /// <summary>
        /// Handles closing ElliotGameView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ElliotGameView_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stops the game from running.
            _isGameRunning = null;

            // Shows the start game form.
            _startForm.Show();
        }
    }
}
