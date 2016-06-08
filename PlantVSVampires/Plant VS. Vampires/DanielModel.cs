/* Daniel Berezovski
 * June 1st, 2016
 * This class is the model that handles a lot of the tasks
 */
using Plant_VS.Vampires.Plants;
using System;
using System.Drawing;

namespace Plant_VS.Vampires
{
    internal class DanielModel
    {
        //Variables that will be common accross multiple classes
        private SharedVariables _vars;

        //Plays sound
        private WMPLib.WindowsMediaPlayer _soundPlayer = new WMPLib.WindowsMediaPlayer();

        //Width of the screen
        private int _screenWidth;

        //To access methods from EliiotModel
        private ModelWrapper _wrapper;

        //The time passed since the game started
        private int _pastTime = Environment.TickCount;     
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vars">The shared variables</param>
        /// <param name="screenWidth">The width of the screen</param>
        /// <param name="wrapper">The wrapper</param>
        internal DanielModel(SharedVariables vars, int screenWidth, ModelWrapper wrapper, int timeDelay)
        {
            //Set the variables
            _vars = vars;

            _vars.SetTimeDelay(timeDelay);

            //Set the screen width
            _screenWidth = screenWidth;

            //Set the wrapper
            _wrapper = wrapper;
        }

        /// <summary>
        /// Signals when the game should start
        /// </summary>
        /// <returns>If the game should start</returns>
        internal bool StartGame()
        {
            //Return if enough time has passed
            return Environment.TickCount - _pastTime > _vars.GetTimeDelay();
        }

        /// <summary>
        /// Time until the game starts
        /// </summary>
        /// <returns>Time</returns>
        internal int SecondsToStartGame()
        {
            //Return the time
            return (int)Math.Ceiling(((_vars.GetTimeDelay() - (Environment.TickCount - _pastTime)) / 1000f));
        }

        /// <summary>
        /// Move the prijectile
        /// </summary>
        /// <param name="i">Index of projectile to move</param>
        private void MoveProjectile(int i)
        {
            //Check if that projectile exists
            if (_vars.projectiles.Count > i)
            {
                //Move it
                _vars.projectiles[i].Move();

                //Check if the projectile is out of bounds
                if (_vars.projectiles[i].X > _screenWidth)
                {
                    //Remove it
                    _vars.projectiles.Remove(_vars.projectiles[i]);
                }
            }
        }

        /// <summary>
        /// Buys and places a plant
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="plant">Plant to place</param>
        internal void BuyAndPlace(int row, int col, Plant plant)
        {
            //Check if the row and column are valid
            if (_wrapper.CheckPlace(row, col, plant.Bounds, plant) && _vars.GetMoons() >= plant.GetPrice())
            {
                //Place the plant
                _vars.board[row, col] = plant;
                
                //Make the player lose moons
                _vars.AddMoons(-plant.GetPrice());
            }
        }

        /// <summary>
        /// Update the internals of the game
        /// </summary>
        internal void UpdateGame()
        {
            //Loop through the rows
            for (int row = 0; row < _vars.board.GetLength(0); row++)
            {
                //Loop through the columns
                for (int col = 0; col < _vars.board.GetLength(1); col++)
                {
                    //Get the plant
                    Plant plant = _vars.board[row, col];

                    //If the plant exists
                    if (plant != null)
                    {
                        //If the plant is dead
                        if (plant.IsDead())
                        {
                            //Remove the plant
                            RemovePlant(plant);
                        }
                        else
                        {
                            //Shoot
                            PlantShoot(row, col);
                        }

                        //Move all of the moons
                        MoveMoons(row, col);
                    }

                    //Make vampires attack
                    VampireAttack(row, col);
                }
            }

            //Update the projectiles
            UpdateProjectiles();
        }

        /// <summary>
        /// All vampires see if they can attack
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        private void VampireAttack(int row, int col)
        {
            //Plant at that spot
            Plant plant = _vars.board[row, col];

            //Loop through all of the vampires
            foreach (Vampire vampire in _vars.vampires)
            {
                //If the vampire is at the end of the board
                if (vampire.End)
                {
                    //Player gets damaged
                    _vars.GetHit(vampire);
                }

                //If the plant doesn't exist skip this iteration
                if (plant == null) continue;

                //Get the plant's bounds
                Rectangle halfBounds = plant.Bounds;

                //Half the width
                halfBounds.Width /= 2;

                //Push it to the front of the tile
                halfBounds.X += halfBounds.Width;

                //Check if the vampire intersects those bounds
                if (vampire.Bounds.IntersectsWith(halfBounds))
                {
                    //Attack the plant
                    vampire.Attack(plant);
                }
            }
        }

        /// <summary>
        /// Shoots the plants
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Columns</param>
        private void PlantShoot(int row, int col)
        {
            //Create the plant
            Plant plant = _vars.board[row, col];

            //Check if the plant exists
            if (plant != null)
            {
                //Get the projectile of the plant
                Projectile projectile = plant.Shoot(_vars.vampires);

                //If the projectile is real
                if (projectile != null)
                {
                    //If the plant is a potatomine
                    if (plant is PotatoMine)
                    {
                        //Loop through all of the vampires
                        foreach (Vampire vampire in _vars.vampires)
                        {
                            //Check if the vampire intersects the potatomine
                            if (vampire.Bounds.IntersectsWith(plant.Bounds))
                            {
                                //Shoot the potatomine
                                _vars.projectiles.Add(projectile);

                                //Remove the potatomine
                                RemovePlant(plant);

                                try
                                {
                                    //Play the explosion
                                    _soundPlayer.URL = Properties.Resources.cherrybombSound;
                                }
                                catch (Exception e) { }

                                //Exit the loop
                                break;
                            }
                        }
                    }
                    else
                    {
                        //Add the projectile to the list
                        _vars.projectiles.Add(projectile);

                        //If the plant is a peashooter
                        if (plant is PeaShooter)
                        {
                            try
                            {
                                //Play the peashooter sound
                                _soundPlayer.URL = Properties.Resources.peashooterSound;
                            }
                            catch (Exception e) { }
                        }
                        //If its a snowpea
                        else if (plant is SnowPea)
                        {
                            try
                            {
                                //Play the snowpea sound
                                _soundPlayer.URL = Properties.Resources.snowpeaSound;
                            }
                            catch (Exception e) { }
                        }
                        //If its a cherrybomb
                        else if (plant is CherryBomb)
                        {
                            //Remove the play
                            RemovePlant(plant);

                            try
                            {
                                //Play the explosion.
                                _soundPlayer.URL = Properties.Resources.cherrybombSound;
                            }
                            catch (Exception e) { }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Removes a plant from the board
        /// </summary>
        /// <param name="plant">Plant to remove</param>
        private void RemovePlant(Plant plant)
        {
            //Loop through the rows
            for (int row = 0; row < _vars.board.GetLength(0); row++)
            {
                //Loop through the columns
                for (int col = 0; col < _vars.board.GetLength(1); col++)
                {
                    //Check if this is the plant
                    if (_vars.board[row, col] == plant)
                    {
                        //Delete the plant
                        plant = null;

                        //Remove it
                        _vars.board[row, col] = null;
                    }
                }
            }
        }

        /// <summary>
        /// Update all of the projectiles
        /// </summary>
        private void UpdateProjectiles()
        {
            //Loop through all of the projectiles
            for (int i = 0; i < _vars.projectiles.Count; i++)
            {
                //Update the bombs
                UpdateBombProjectiles(i);

                //Move them
                MoveProjectile(i);
            }
        }

        /// <summary>
        /// Updates the bomb projectile
        /// </summary>
        /// <param name="i">Index to update</param>
        private void UpdateBombProjectiles(int i)
        {
            //Check if its a bomb projectile
            if (_vars.projectiles[i].Type == Projectile.ProjectileType.BombProjectile || _vars.projectiles[i].Type == Projectile.ProjectileType.MineProjectile)
            {
                //Checks if its time to remove it
                if(Environment.TickCount - _vars.projectiles[i].LastTime >= SharedVariables.SHOOT_DELAY)
                {
                    //Remove it
                    _vars.projectiles.Remove(_vars.projectiles[i]);
                }
            }
        }

        /// <summary>
        /// Moves all of the moons
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        private void MoveMoons(int row, int col)
        {
            //Check if its a moonflower
            if (_vars.board[row, col] is MoonFlower)
            {
                //Create the moonflower
                MoonFlower moonFlower = _vars.board[row, col] as MoonFlower;

                //Loop through all of the moons to draw
                foreach (Moon moon in moonFlower.GetMoonsToDraw())
                {
                    //Move it
                    moon.Move();
                }
            }
        }
    }
}
