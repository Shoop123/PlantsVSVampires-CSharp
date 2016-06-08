/* Daniel Berezovski
 * June 1st, 2016
 * This class draws everythings
 */
using Plant_VS.Vampires;
using Plant_VS.Vampires.Plants;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace DanielView
{
    public partial class DanielGameView : Form
    {
        //Determines if the game is running
        private bool _running = false;

        //Sets the frame rate
        private const int FPS = 1000 / 32;

        //Current frames achieved
        private int _currentFPS = FPS;

        //Rows and columns of the grid
        private const int ROWS = 5;
        private const int COLS = 9;

        //Margin of each tile in the grid
        private const int TILE_HEIGHT_MARGIN = 30;
        private const int TILE_WIDTH_MARGIN = 30;

        //The amount of space around each of the four sides of the grid
        private const int TOTAL_HEIGHT_MARGIN = TILE_HEIGHT_MARGIN * ROWS;
        private const int TOTAL_WIDTH_MARGIN = TILE_WIDTH_MARGIN * COLS;

        //Height of the status bar
        private const int STATUS_BAR_OFFSET = TOTAL_HEIGHT_MARGIN / 3;

        //Font for drawing the plant descriptions
        private Font _plantFont = new Font(FontFamily.GenericSerif, 10);

        //The font for drawing the time passed
        private Font _timeFont = new Font(FontFamily.GenericSerif, 100);

        //The font for drawing the time passed
        private Font _noteFont = new Font(FontFamily.GenericSerif, 40);

        //ModelWrapper object for interacting with the model
        private ModelWrapper _wrapper;

        //Mouse location
        private int _mouseX = 0;
        private int _mouseY = 0;

        //Tile size
        private int _tileHeight;
        private int _tileWidth;

        //If the mouse is being pressed
        private bool _mouseDown = false;

        //Whether of not a plant is grabbed
        private bool[] _plantGrabbed;

        //The index in the aboce array of the grabbed plant
        private int _plantGrabbedIndex = 0;

        //Draws the status bar
        private StatusBarContent _statusBarContent;

        //Moon locations
        private Point _moonLocation;

        //Moons string
        private const string MOONS_STR = " Moons";

        //Bitmap that will be drawn to in the separate thread, and then updated from in the paint method
        private Bitmap _buffer;

        //Grid
        private Rectangle[,] _grid;

        //If the player has lost
        private LostGame _lostGame;

        private Form _startForm;

        private string _note = String.Empty;

        private string _backUpNote = String.Empty;

        private int _noteShowTime = 0;

        private const int MAX_NOTE_SHOW_TIME = 3000;

        private const string OCCUPIED = "Oops! There's already a plant there!";
        private const string NO_TREES_NEARBY = "Looks like you need to plant a tree first...";
        private const string INSUFFICIENT_MOONS = "Not enough moons :(";

        /// <summary>
        /// Constructor
        /// </summary>
        public DanielGameView(Form startForm)
        {
            //Initializes components
            InitializeComponent();

            _startForm = startForm;
        }

        /// <summary>
        /// Starts the main loop thread
        /// </summary>
        private void StartGame()
        {
            //Creates a thread that will call the main loop method
            Thread t = new Thread(MainLoop);

            //Starts the thread
            t.Start();
        }

        /// <summary>
        /// Main loop method that will draw everything to a buffer bitmap
        /// </summary>
        private void MainLoop()
        {
            //Placeholder bitmap for creating it in the loop
            Bitmap buffer;

            //Game is running now
            _running = true;

            //Last time the board has been refreshed
            int lastUpdate = Environment.TickCount;

            //Main loop, loops while the is running
            while (_running)
            {
                //Current time
                int now = Environment.TickCount;

                //The amount of time passed after the last update
                int passed = now - lastUpdate;

                //If enough time has passed
                if (passed >= FPS)
                {
                    //Create a new bitmap with the size of the screen
                    buffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
                    
                    //Use graphics from the bitmap
                    using (Graphics g = Graphics.FromImage(buffer))
                    {
                        //Set the render hint to anti alis the drawing
                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                        //Set the smoothing mode to anit alis
                        g.SmoothingMode = SmoothingMode.AntiAlias;

                        //If the game has not faded away
                        if (!_lostGame.IsFaded())
                        {
                            //Draw the status bar
                            _statusBarContent.DrawStatusBar(g);

                            //Draw the plant menu
                            DrawPlantMenu(g);

                            //Loop through each tile in the grid
                            BoardLoop(g);

                            //Draw the vampires
                            DrawVampires(g);

                            //Draw the time until vampires start coming
                            DrawSecondsToStartGame(g);

                            //Draw the note to the player
                            DrawNote(g);
                        }
                    }

                    //If the player has lost
                    if(_wrapper.PlayerLost())
                        //Set the bitmap to the lost game screen
                        buffer = _lostGame.GameLost(buffer);

                    //If the game is not faded
                    if (!_lostGame.IsFaded())
                        //Update the game
                        UpdateGame();

                    //Set the local buffer to the global buffer
                    _buffer = buffer;

                    //Update the current frame rate
                    _currentFPS = 1000 / passed;

                    //Try
                    try
                    {
                        //Invoke a main thread operation
                        this.Invoke(new Action(() =>
                        {
                            //Update frame rate display
                            this.Text = _currentFPS.ToString();
                            //Refresh the graphics
                            Refresh();
                        }));
                    }
                    //Do nothing for catch (only occurs when user exits)
                    catch (ObjectDisposedException ode) { }
                    catch(InvalidOperationException ioe) { }

                    //Set the last update to just now
                    lastUpdate = now;

                    //Dispose of the bitmap resources
                    buffer.Dispose();
                }
            }
        }

        /// <summary>
        /// Make the grid
        /// </summary>
        private void MakeGrid()
        {
            //Set tile size
            _tileHeight = this.ClientSize.Height / ROWS - TILE_HEIGHT_MARGIN;
            _tileWidth = this.ClientSize.Width / COLS - TILE_WIDTH_MARGIN;

            //Create the 2D array size
            _grid = new Rectangle[ROWS, COLS];

            //Loop through rows
            for (int row = 0; row < ROWS; row++)
            {
                //Loop through cols
                for (int col = 0; col < COLS; col++)
                {
                    //Calculate coordinates of tile
                    int x = col * _tileWidth + TOTAL_WIDTH_MARGIN;
                    int y = row * _tileHeight + TOTAL_HEIGHT_MARGIN / 2;

                    //Make tile
                    Rectangle tile = new Rectangle(x, y, _tileWidth, _tileHeight);

                    //Set tile
                    _grid[row, col] = tile;
                }
            }
        }

        /// <summary>
        /// Paints to the form
        /// </summary>
        /// <param name="e">Proveides data for the paint event</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //Call parent paint event
            base.OnPaint(e);

            //If the buffer is not null
            if (_buffer != null)
            {
                //Try
                try
                {
                    //Draw the bitmap
                    e.Graphics.DrawImage(_buffer, Point.Empty);
                }
                //Do nothing for catch (only occurs when user exits) 
                catch (ArgumentException ae) { }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //When user exits form, stop running the game
            _running = false;

            _startForm.Show();
        }

        /// <summary>
        /// Occurs when the form is shown
        /// </summary>
        /// <param name="sender">Object the fired the event</param>
        /// <param name="e">Event data</param>
        private void Form1_Shown(object sender, EventArgs e)
        {
            //Make the grid
            MakeGrid();

            //Create the ModelWrapper
            _wrapper = new ModelWrapper(ROWS, COLS, _tileWidth, _tileHeight, this.ClientSize.Width, (TOTAL_HEIGHT_MARGIN / 2), _grid[0, 0].X, 5000);

            //Get all types of plants
            _plantGrabbed = new bool[_wrapper.GetPlants().Length];

            //Initiate status bar
             _statusBarContent = new StatusBarContent(_wrapper, this.ClientSize.Width, STATUS_BAR_OFFSET);
            
            //Calculate moon loocation
             _moonLocation = new Point((int)_statusBarContent.GetMoonLocation().X, (int)_statusBarContent.GetMoonLocation().Y);
            
            //Prepare for when the player loses
             _lostGame = new LostGame(this, _startForm);

            //Start the game
            StartGame();
        }

        /// <summary>
        /// Draws the note to communicate with the player
        /// </summary>
        /// <param name="g">The graphics to draw on</param>
        private void DrawNote(Graphics g)
        {
            //Check if the note has been set
            if ((_note != String.Empty && _noteShowTime == 0) || (_note != String.Empty && _note != _backUpNote))
            {
                //Save the time its been set
                _noteShowTime = Environment.TickCount;

                //Backup the note
                _backUpNote = _note;
            }
            //Otherwise reset the tiem
            else if (_note == String.Empty)
                //Set the time to 0
                _noteShowTime = 0;

            //Check how long the note note has been visible
            if (Environment.TickCount - _noteShowTime >= MAX_NOTE_SHOW_TIME)
            {
                //Clear the note
                _note = String.Empty;

                //Clear the backup
                _backUpNote = _note;
            }

            //Size of note
            SizeF noteSize = g.MeasureString(_note, _noteFont);

            //Bottom of the game grid
            int gridBottom = _tileHeight * ROWS + TOTAL_HEIGHT_MARGIN / 2;

            //X coordinate of the note
            float x = this.ClientSize.Width / 2F - noteSize.Width / 2F;

            //Y coordinate of the note
            float y = gridBottom + TOTAL_HEIGHT_MARGIN / 4F - noteSize.Height / 2F;

            //Draw the note
            g.DrawString(_note, _noteFont, Brushes.Red, x, y);
        }

        //Loops through all of the tiles
        private void BoardLoop(Graphics g)
        {
            //Loops through rows
            for (int row = 0; row < ROWS; row++)
            {
                //Loop through columns
                for (int col = 0; col < COLS; col++)
                {
                    //Draw the game board
                    DrawBoard(g, row, col);
                    
                    //Draw the projectiles
                    DrawProjectiles(g, row, col);
                }
            }
        }

        /// <summary>
        /// Draws the game tiles
        /// </summary>
        /// <param name="g">Graphics to draw on</param>
        /// <param name="row">Row to draw</param>
        /// <param name="col">Column to draw</param>
        private void DrawBoard(Graphics g, int row, int col)
        {
            //Create a pen
            Pen pen = new Pen(Brushes.Black);

            //Store default pen width
            const int NORMAL_PEN_WIDTH = 3;

            //Store the thick pen width
            const int THICK_PEN_WIDTH = 5;

            //Set the pen width
            pen.Width = NORMAL_PEN_WIDTH;

            //Check if you can place a plant on this tile
            bool? canPlace = CanPlace(row, col, _grid[row, col]);

            //If the mouse is over this tile
            if (StaticMethods.MouseIntersects(_grid[row, col].X, _grid[row, col].Y, _tileWidth, _tileHeight, _mouseX, _mouseY))
            {
                //Buy the plant and place it
                BuyAndPlace(row, col, _grid[row, col]);

                //Check if the mouse is pressed and a plant is grabbed
                if (_mouseDown && _plantGrabbed[_plantGrabbedIndex])
                {
                    //Make the pen width thick
                    pen.Width = THICK_PEN_WIDTH;
                }
            }

            //If you can place a plant here
            if (canPlace == true)
            {
                //Make a green linear gradient brush and draw it
                LinearGradientBrush brush = new LinearGradientBrush(_grid[row, col].Location, new PointF(_grid[row, col].X + _tileWidth, _grid[row, col].Y + _tileHeight), Color.Green, Color.Transparent);
                g.FillRectangle(brush, _grid[row, col]);
            }
            //If you cant place a plant here
            else if(canPlace == null)
            {
                //Make a red gradient brush and draw it
                LinearGradientBrush brush = new LinearGradientBrush(_grid[row, col].Location, new PointF(_grid[row, col].X + _tileWidth, _grid[row, col].Y + _tileHeight), Color.Red, Color.Transparent);
                g.FillRectangle(brush, _grid[row, col]);
            }

            //If there's no plant on this tile
            if (_wrapper.GetBoard()[row, col] != null)
            {
                //Draw the plant
                g.DrawImage(Graphic.GetImage(_wrapper.GetBoard()[row, col]), _grid[row, col]);

                //Draw the moons
                DrawGameMoons(g, row, col);

                //Draw the cherry bomb timer
                DrawCherryBombTimer(g, _wrapper.GetBoard()[row, col]);

                //Draw the plant health
                DrawPlantHealth(g, _wrapper.GetBoard()[row, col]);
            }

            //Draw the grid
            g.DrawRectangle(pen, _grid[row, col]);
        }

        /// <summary>
        /// Draws the seconds until the game starts
        /// </summary>
        /// <param name="g">Graphics to draw on</param>
        private void DrawSecondsToStartGame(Graphics g)
        {
            //Check if the game already started, if it did, exit
            if (_wrapper.StartGame()) return;

            //The seconds until the start
            string text = _wrapper.SecondsToStartGame().ToString();

            //Size of time
            SizeF sizeOfText = g.MeasureString(text, _timeFont);

            //Location of the time
            PointF center = new PointF();
            
            //X location
            center.X = (this.ClientSize.Width / 2f) - (sizeOfText.Width / 2f);

            //Y location
            center.Y = (this.ClientSize.Height / 2f) - (sizeOfText.Height / 2f);

            //Draw the time
            g.DrawString(text, _timeFont, Brushes.Green, center);
        }

        /// <summary>
        /// Draw the timer for each cherrybomb
        /// </summary>
        /// <param name="g">Graphics to draw one</param>
        /// <param name="plant">The plant that may be a cherrybomb</param>
        private void DrawCherryBombTimer(Graphics g, Plant plant)
        {
            //Check id the plant is a cherrybomb
            if (plant is CherryBomb)
            {
                //Create the cherrybomb object
                CherryBomb cherryBomb = plant as CherryBomb;

                //Time that there is until explosion
                string time = _wrapper.GetSecondsToDetonation(cherryBomb).ToString();

                //Draw the time
                g.DrawString(time, _plantFont, Brushes.Black, plant.X, plant.Y);
            }
        }

        /// <summary>
        /// Draw the health of the plants
        /// </summary>
        /// <param name="g">Graphics to draw on</param>
        /// <param name="plant">The plant that will be draw on</param>
        private void DrawPlantHealth(Graphics g, Plant plant)
        {
            //Check if the plant isnt a cherrybomb or potatomine
            if (!(plant is CherryBomb) && !(plant is PotatoMine))
            {
                //Draw the health
                g.DrawString(_wrapper.GetPlantHealth(plant).ToString(), _plantFont, Brushes.Black, new Point(plant.X, plant.Y));       
            }
        }

        /// <summary>
        /// Buy and place a plant
        /// </summary>
        /// <param name="row">The row to place</param>
        /// <param name="col">The column to place</param>
        /// <param name="bounds">The bounds of the tile</param>
        private void BuyAndPlace(int row, int col, Rectangle bounds)
        {
            //Check if the mouse is pressed and a plant is grabbed
            if (!_mouseDown && _plantGrabbed[_plantGrabbedIndex])
            {
                //Get the grabbed plant
                Plant grabbedPlant = _wrapper.GetPlants()[_plantGrabbedIndex];

                //Create a plant
                Plant plant = null;

                //Check if the grabbed plant is a tree
                if (grabbedPlant is Tree)
                {
                    //Create the new plant
                    plant = new Tree(row, col, bounds);
                }
                ////Check if the grabbed plant is a moonflower
                else if (grabbedPlant is MoonFlower)
                {
                    //Create the new plant
                    plant = new MoonFlower(row, col, bounds);
                }
                //Check if the grabbed plant is a cherrybomb
                else if (grabbedPlant is CherryBomb)
                {
                    //Create the new plant
                    plant = new CherryBomb(row, col, bounds);
                }
                //Check if the grabbed plant is a potatomine
                else if (grabbedPlant is PotatoMine)
                {
                    //Create the new plant
                    plant = new PotatoMine(row, col, bounds);
                }
                //Check if the grabbed plant is a peashooter
                else if (grabbedPlant is PeaShooter)
                {
                    //Create the new plant
                    plant = new PeaShooter(row, col, bounds);
                }
                //Check if the grabbed plant is a snowpea
                else if (grabbedPlant is SnowPea)
                {
                    //Create the new plant
                    plant = new SnowPea(row, col, bounds);
                }
                //Check if the grabbed plant is a wallnut
                else if (grabbedPlant is WallNut)
                {
                    //Create the new plant
                    plant = new WallNut(row, col, bounds);
                }

                //Store the reason that a player can either place or not place a plant
                ModelWrapper.Reason reason = _wrapper.GetReason(row, col, plant);

                //Check if the tile is occupied by a plant
                if (reason == ModelWrapper.Reason.Occupied)
                    //Set the note accordingly
                    _note = OCCUPIED;
                //Check if there is no trees in bounds
                else if (reason == ModelWrapper.Reason.NoTreesNearby)
                    //Set the note accordingly
                    _note = NO_TREES_NEARBY;
                //Check if the player doesnt have enough moons
                else if (reason == ModelWrapper.Reason.InsufficientMoons)
                    //Set the note accordingly
                    _note = INSUFFICIENT_MOONS;

                //Buy and place the plant
                _wrapper.BuyAndPlace(row, col, plant);

                //Release the plant
                _plantGrabbed[_plantGrabbedIndex] = false;
            }
        }

        /// <summary>
        /// Draw all of the plant projectiles
        /// </summary>
        /// <param name="g">Graphics to draw on</param>
        /// <param name="row">Row of the plant that shot</param>
        /// <param name="col">Column of the plant that shot</param>
        private void DrawProjectiles(Graphics g, int row, int col)
        {
            //Loop through all of the projectiles
            foreach(Projectile projectile in _wrapper.GetProjectiles())
            {
                //If the projectile is a bomb projectile
                if (projectile.Type == Projectile.ProjectileType.BombProjectile)
                {
                    //The splash radius of the bomb
                    int splashRadius = _wrapper.GetProjectileSplashRadius(projectile);

                    //Check the projectile's range
                    if(projectile.Row - splashRadius == row || projectile.Row + splashRadius == row || projectile.Row == row)
                    {
                        //Check the projectile's range
                        if (projectile.Col - splashRadius == col || projectile.Col + splashRadius == col || projectile.Col == col)
                        {
                            //If there is no plant in this tile
                            if(_wrapper.GetBoard()[row, col] == null)
                            {
                                //Draw the explosion
                                g.DrawImage(Graphic.GetImage(projectile), _grid[row, col].X, _grid[row, col].Y, projectile.Width, projectile.Height);
                            }
                        }
                    }
                }
                //If the projectile is a mine
                else if (projectile.Type == Projectile.ProjectileType.MineProjectile)
                {
                    //Draw the explosion
                    g.DrawImage(Graphic.GetImage(projectile), projectile.X, projectile.Y, projectile.Width, projectile.Height);
                }
                //Otherwise if its any other projectile
                else
                {
                    //Draw the projectile
                    g.DrawImage(Graphic.GetImage(projectile), projectile.Bounds);
                }
            }
        }

        /// <summary>
        /// Draws the plant selection menu on the left
        /// </summary>
        /// <param name="g">Graphics to draw on</param>
        private void DrawPlantMenu(Graphics g)
        {
            //Height offset of the plant menu
            const int HEIGHT_OFFSET = 10;
            
            //Spacing between the grid and the menu
            const int MENU_GAME_SPACING = 5;

            //Pen width when the mouse is not over any items
            const int PEN_WIDTH = 1;

            //Pen width when the mouse is over an item
            const int MOUSE_OVER_PEN_WIDTH = 5;

            //Possible set of plants
            Plant[] plants = _wrapper.GetPlants();

            //Tile sizes of each item
            int tileWidth = (int)(TOTAL_WIDTH_MARGIN / 1.6F);
            int tileHeight = (this.ClientSize.Height - STATUS_BAR_OFFSET) / plants.Length - HEIGHT_OFFSET;

            //Pen to draw each grid
            Pen pen = new Pen(Brushes.Black);

            //Loop throught all the plants
            for(int i = 0; i < plants.Length; i++)
            {
                //Get the x and y of each plant
                int x = TOTAL_WIDTH_MARGIN - tileWidth - MENU_GAME_SPACING;
                int y = i * tileHeight + STATUS_BAR_OFFSET + HEIGHT_OFFSET;

                //The bounds of each grid
                Rectangle menuTile = new Rectangle(x, y, tileWidth, tileHeight);

                //Price of the plant
                string price = _wrapper.GetPlantPrice(plants[i]).ToString() + MOONS_STR;

                //Name of the plant
                string name = plants[i].GetType().Name;

                //Size of the price
                SizeF priceSize = g.MeasureString(price, _plantFont);

                //Size of the name
                SizeF nameSize = g.MeasureString(name, _plantFont);

                //Location of the price
                float xPrice = (x / 2) - (priceSize.Width / 2);
                float yPrice = (y + tileHeight / 2) - priceSize.Height;

                //Location of the name
                float xName = (x / 2) - (nameSize.Width / 2);
                float yName = (y + tileHeight / 2) + nameSize.Height;

                //Draw the price and name
                g.DrawString(price, _plantFont, Brushes.Black, xPrice, yPrice);
                g.DrawString(name, _plantFont, Brushes.Black, xName, yName);

                //Get the image of the plant
                Image plantImage = Graphic.GetImage(plants[i]);

                //Check if the player is dragging the plant
                CheckMouseDrag(i, g, x, y, tileWidth, tileHeight, plantImage, menuTile);

                //Draw the plant
                g.DrawImage(plantImage, menuTile);

                //If the mouse is over the item
                if (StaticMethods.MouseIntersects(x, y, tileWidth, tileHeight, _mouseX, _mouseY))
                {
                    //Increse the pen width
                    pen.Width = MOUSE_OVER_PEN_WIDTH;
                }
                else
                {
                    //Keep the line width normal
                    pen.Width = PEN_WIDTH;
                }

                //Draw the tile
                g.DrawRectangle(pen, menuTile);
            }
        }
        
        /// <summary>
        /// Check if the mouse is dragging an item
        /// </summary>
        /// <param name="index">The item that is being dragged</param>
        /// <param name="g">Graphics to draw on</param>
        /// <param name="x">X location</param>
        /// <param name="y">Y location</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="plantImage">Plant image</param>
        /// <param name="rect">Rectangle</param>
        private void CheckMouseDrag(int index, Graphics g, int x, int y, int width, int height, Image plantImage, Rectangle rect)
        {
            //Check if the mouse is pressed of an item
            if (StaticMethods.MouseIntersects(x, y, width, height, _mouseX, _mouseY) && _mouseDown && !_plantGrabbed[_plantGrabbedIndex])
            {
                //Set that the player has grabbed a plant
                _plantGrabbed[index] = true;

                //Set the plant that was grabbed
                _plantGrabbedIndex = index;
            }

            //If a plant is grabbed
            if (_plantGrabbed[index])
            {
                //Get the mouse location to draw the plant on
                Point mousePoint = new Point(_mouseX - width / 2, _mouseY - height / 2);

                //Reset the bounds location
                rect.Location = mousePoint;

                //Draw the dragged item
                g.DrawImage(plantImage, rect);
            }
        }

        /// <summary>
        /// Draws the vampires
        /// </summary>
        /// <param name="g">Graphics to draw on</param>
        private void DrawVampires(Graphics g)
        {
            //Loop through all of the vampires
            foreach (Vampire vampire in _wrapper.GetVampires())
            {
                //Draw all of the vampires
                g.DrawImage(Graphic.GetImage(vampire), vampire.Bounds);
            }
        }

        /// <summary>
        /// Draw the moons
        /// </summary>
        /// <param name="g">Graphics to draw on</param>
        /// <param name="row">The row of the plant</param>
        /// <param name="col">The column of the plant</param>
        private void DrawGameMoons(Graphics g, int row, int col)
        {
            //If the plant is a moonflower
            if (_wrapper.GetBoard()[row, col] is MoonFlower)
            {
                //Create the moonflower
                MoonFlower moonFlower = _wrapper.GetBoard()[row, col] as MoonFlower;

                //Store all moons that should be removed
                List<Moon> toRemove = new List<Moon>();

                //Loop through all of the moons for the moonflower
                foreach(Moon moon in _wrapper.GetMoonsToDraw(moonFlower))
                {
                    //Draw the moon
                    g.DrawImage(Properties.Resources.moon, moon.Bounds);

                    //Check if the moon is already collected
                    if(moon.IsExpired)
                    {
                        //Add it to the remove list
                        toRemove.Add(moon);
                    }
                }

                //Add the amount of moons that have been collected
                _wrapper.AddMoons(toRemove.Count);

                //Loop through all of the moons that should be removed
                foreach(Moon moon in toRemove)
                {
                    //Remove the moon
                    _wrapper.GetMoonsToDraw(moonFlower).Remove(moon);
                }
            }
        }

        /// <summary>
        /// Checks if the player can place a plant there
        /// </summary>
        /// <param name="row">Row of the place</param>
        /// <param name="col">Column of the place</param>
        /// <param name="place">Bounds of the place</param>
        /// <returns>Return if the tile is available</returns>
        private bool? CanPlace(int row, int col, Rectangle place)
        {
            //If the player has his mouse pressed and a plant is grabbed
            if (_mouseDown && _plantGrabbed[_plantGrabbedIndex])
            {
                //Get the type of plant thats grabbed
                Plant type = _wrapper.GetPlants()[_plantGrabbedIndex];

                //Check if you can place a plant here
                if (_wrapper.CheckPlace(row, col, place, type))
                {
                    //Return true if yes
                    return true;
                }
                else
                {
                    //Return null of no
                    return null;
                }
            }

            //Return false otherwise
            return false;
        }
        
        /// <summary>
        /// Update all components of the game
        /// </summary>
        private void UpdateGame()
        {
            //Update model components
            _wrapper.Update();

            //Make the moons
            _wrapper.MakeMoons(_moonLocation);

            //Check if the player has lost
            if (_wrapper.PlayerLost())
            {
                //Start the losing animation
                _lostGame.InitLose();
            }
        }

        /// <summary>
        /// Check if the player picked up any moons
        /// </summary>
        private void CheckMoonPickUp()
        {
            //Loop through all of the rows
            for (int rows = 0; rows < ROWS; rows++)
            {
                //Loop through all of  the columns
                for (int cols = 0; cols < COLS; cols++)
                {
                    //Check if the plant is a moonflower
                    if (_wrapper.GetBoard()[rows, cols] is MoonFlower)
                    {
                        //Create the moonflower
                        MoonFlower moonFlower = _wrapper.GetBoard()[rows, cols] as MoonFlower;

                        //Create the moon to remove
                        Moon toRemove = null;

                        //Loop through all of the moons
                        foreach (Moon moon in _wrapper.GetMoons(moonFlower))
                        {
                            //If the mouse is over the moon
                            if (StaticMethods.MouseIntersects(moon.X, moon.Y, moon.Width, moon.Height, _mouseX, _mouseY))
                            {
                                //Collect the moon
                                moon.IsCollected = true;

                                //Update the moon to remove
                                toRemove = moon;

                                //Break out of the loop
                                break;
                            }
                        }

                        //Check if there is a moon to remove
                        if(toRemove != null)
                        {
                            //Collect the moon
                            _wrapper.CollectMoon(moonFlower, toRemove);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if a plant is grabbed
        /// </summary>
        /// <returns>Returns if a plant is grabbed</returns>
        private bool PlantsGrabbed()
        {
            //Loop through all of the plants
            foreach(bool grabbed in _plantGrabbed)
            {
                //If the plant is grabbed
                if (grabbed)
                    //Return true
                    return true;
            }

            //Return false
            return false;
        }

        /// <summary>
        /// Runs when the user moves thier mouse
        /// </summary>
        /// <param name="sender">The object that called the event</param>
        /// <param name="e">Info abou the event</param>
        private void DanielGameView_MouseMove(object sender, MouseEventArgs e)
        {
            //Update the global mouse location
            _mouseX = e.X;
            _mouseY = e.Y;

            //Update the mouse location in the other class
            _lostGame.UpdateMouseMove(_mouseX, _mouseY);
        }


        /// <summary>
        /// Runs when the user clicks thier mouse
        /// </summary>
        /// <param name="sender">The object that called the event</param>
        /// <param name="e">Info abou the event</param>
        private void DanielGameView_MouseDown(object sender, MouseEventArgs e)
        {
            //Check if the player has lost
            if (_wrapper.PlayerLost())
                //Update mouse state
                _mouseDown = false;
            else
                //Update mouse state
                _mouseDown = true;

            //Update the mouse state in the other class
            _lostGame.UpdateMouseDown(true);
        }

        /// <summary>
        /// Runs when the user un-clicks thier mouse
        /// </summary>
        /// <param name="sender">The object that called the event</param>
        /// <param name="e">Info abou the event</param>
        private void DanielGameView_MouseUp(object sender, MouseEventArgs e)
        {
            //Update the mouse state in the other calss
            _lostGame.UpdateMouseDown(false);

            //If the player has lost return
            if (_wrapper.PlayerLost()) return;

            //Update the mouse state
            _mouseDown = false;

            //Get board bounds
            int x = TOTAL_WIDTH_MARGIN;
            int y = TOTAL_HEIGHT_MARGIN / 2;
            int width = _tileWidth * COLS;
            int height = _tileHeight * ROWS;

            //Check if the mouse intersects the board
            if (!StaticMethods.MouseIntersects(x, y, width, height, _mouseX, _mouseY))
            {
                //Reset the grabbed plant
                _plantGrabbed[_plantGrabbedIndex] = false;
            }
            //If no plants are grabbed
            else if(!PlantsGrabbed())
            {
                //Check if the player picked up a moon
                CheckMoonPickUp();
            }
        }
    }
}
