// Elliot Spall,
// June 4th 2016,
// Handles the creation of an ElliotModel.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Plant_VS.Vampires.Plants;
using Plant_VS.Vampires.Vampires;

namespace Plant_VS.Vampires
{
    internal class ElliotModel
    {
        // Creates a variable to store the shared variables.
        private SharedVariables _sharedVariables;

        // Creates a list of rectangles to store the tiles around the trees as a one dimensional list.
        private List<Rectangle> _tilesAroundTreeSingle = new List<Rectangle>();

        // Creates a variable to store the time the last vampire was made.
        private int _lastVampireMade = Environment.TickCount;

        // Creates a variable to store the wave number.
        private int _wave = 0;

        // Creates a variable to store the amount of vampires spawned in the current wave.
        private int _vampiresSpawnedInCurrentWave = 0;

        // Creates a variable to store the total amount of vampires that have spawned.
        private int _vampiresSpawned = 0;

        // Creates a random to generate a random number later on.
        private Random _randomValue = new Random();

        // Creates a variable to store the location of the edge of the board.
        private int _edge;

        // Creates a 2D array to store the imaginary rectangles for the splash radius of a cherry bomb.
        private Rectangle[,] _imaginaryRectagles = new Rectangle[SharedVariables.CHERRY_BOMB_SPLASH_RADIUS, SharedVariables.CHERRY_BOMB_SPLASH_RADIUS];
 
        /// <summary>
        /// The Constructor of ElliotModel
        /// </summary>
        /// <param name="_sharedVariables"></param>
        /// <param name="edge"></param>
        internal ElliotModel(SharedVariables _sharedVariables, int edge)
        {
            // Sets the sharedvariables to the passed in shared variables.
            this._sharedVariables = _sharedVariables;

            // Sets the edge to the passed in edge.
            _edge = edge;
        }

        /// <summary>
        /// Handles getting the wave number
        /// </summary>
        /// <returns></returns>
        internal int GetWave()
        {
            // Returns the wave number.
            return _wave;
        }    

        /// <summary>
        /// Handles the projectile hitting a vampire
        /// </summary>
        internal void ProjectileHit()
        {
            // Loops from 0 to the amount of projectiles on the board.
            for (int i = 0; i < _sharedVariables.projectiles.Count; i++)
            {
                // Creates a projectile.
                Projectile projectile = _sharedVariables.projectiles[i];

                // Loops from 0 to the amount of vampires on the board.
                for (int j = 0; j < _sharedVariables.vampires.Count; j++)
                {
                    // Creates a vampire.
                    Vampire vampire = _sharedVariables.vampires[j];

                    // If the projectile is a bomb projectile.
                    if (projectile.Type == Projectile.ProjectileType.BombProjectile)
                    {
                        // Creates 3 imaginary rectangles one row above the projectile.
                        _imaginaryRectagles[0, 0] = new Rectangle((projectile.X - _sharedVariables.Width), (projectile.Y + _sharedVariables.Height), _sharedVariables.Width, _sharedVariables.Height);
                        _imaginaryRectagles[0, 1] = new Rectangle((projectile.X - _sharedVariables.Width), projectile.Y, _sharedVariables.Width, _sharedVariables.Height);
                        _imaginaryRectagles[0, 2] = new Rectangle((projectile.X + _sharedVariables.Width), (projectile.Y + _sharedVariables.Height), _sharedVariables.Width, _sharedVariables.Height);
                       
                        // Creates 3 imaginary rectangles on the same row as the projectile.
                        _imaginaryRectagles[1, 0] = new Rectangle((projectile.X - _sharedVariables.Width), projectile.Y, _sharedVariables.Width, _sharedVariables.Height);
                        _imaginaryRectagles[1, 1] = new Rectangle(projectile.X, projectile.Y, _sharedVariables.Width, _sharedVariables.Height);
                        _imaginaryRectagles[1, 2] = new Rectangle((projectile.X + _sharedVariables.Width), projectile.Y, _sharedVariables.Width, _sharedVariables.Height);

                        // Creates 3 imaginary rectangles one row below the projectile.
                        _imaginaryRectagles[2, 0] = new Rectangle((projectile.X - _sharedVariables.Width),  (projectile.Y - _sharedVariables.Height), _sharedVariables.Width, _sharedVariables.Height);
                        _imaginaryRectagles[2, 1] = new Rectangle(projectile.X, (projectile.Y - _sharedVariables.Height), _sharedVariables.Width, _sharedVariables.Height);
                        _imaginaryRectagles[2, 2] = new Rectangle((projectile.X + _sharedVariables.Width), (projectile.Y - _sharedVariables.Height), _sharedVariables.Width, _sharedVariables.Height);

                        // Loops from 0 to the constant cherry bomb splash radius.
                        for (int x = 0; x < SharedVariables.CHERRY_BOMB_SPLASH_RADIUS; x++)
                        {
                            // Loops from 0 to the constant cherry bomb splash radius.
                            for (int y = 0; y < SharedVariables.CHERRY_BOMB_SPLASH_RADIUS; y++)
                            {
                                // If the vampire intersects with the specific imaginary rectangle.
                                if (vampire.Bounds.IntersectsWith(_imaginaryRectagles[x, y]))
                                {
                                    // Calls the vampire to get hit.
                                    vampire.GetHit(projectile);
                                }
                            }
                        }
                    }

                    // If the projectile is a mine projectile.
                    else if (projectile.Type == Projectile.ProjectileType.MineProjectile)
                    {
                        // Checks if the vampire intersects with the projectile.
                        if (vampire.Bounds.IntersectsWith(projectile.Bounds))
                        {
                            // Calls the vampire to get hit.
                            vampire.GetHit(projectile);
                        }
                    }

                    // Otherwise
                    else
                    {
                        // Creates a rectangle to store the bounds of the vampire.
                        Rectangle customBounds = vampire.Bounds;

                        // If the vampire is a weak vampire.
                        if (vampire is WeakVampire)
                        {
                            // Increase the x coordinate of the vampire bounds rectangle by the amount of the constant side offset (for the transparent image).
                            customBounds.X += WeakVampire.SIDE_OFFSET;
                        }

                        // If the bounds of the vampire intersect with the projectile's bounds.
                        if (customBounds.IntersectsWith(projectile.Bounds))
                        {
                            // Calls the vampire to get hit.
                            vampire.GetHit(projectile);

                            // Calls the projectile to be removed from the board.
                            _sharedVariables.projectiles.Remove(projectile);
                        }
                    }

                    // If the vampire's health is less than or equal to 0.
                    if (vampire.GetHealth() <= 0)
                    {
                        // Calls the vampire to be removed from the board.
                        _sharedVariables.vampires.Remove(vampire);
                    }
                }
            }
        }

        /// <summary>
        /// Handles making vampires
        /// </summary>
        /// <param name="screenWidth"></param>
        /// <param name="topOffset"></param>
        internal void MakeVampire(int screenWidth, int topOffset)
        {
            // Creates a variable to store the time passed between the current time and the last vampire spawn time.
            int passed = Environment.TickCount - _lastVampireMade;

            // Creates a variable to store the right-most column on the board.
            int rightCol = _sharedVariables.board.GetLength(1);

            // Creates a variable to store a random spawn time between 0.5 and 3.3 seconds.
            int rndTime = _randomValue.Next(500, 3333);

            // Creates a variable to store a random row.
            int randomRow = _randomValue.Next(0, _sharedVariables.board.GetLength(0));

            // Creates a rectangle to store the bounds of the vampire to be made.
            Rectangle bounds = new Rectangle(screenWidth, topOffset + (randomRow * _sharedVariables.Height), _sharedVariables.Width, _sharedVariables.Height);

            // If the passed time is greater than or equal to the random time.
            if (passed >= rndTime)
            {
                // Updates the time of the last vampire made.
                _lastVampireMade = Environment.TickCount;
                
                // Creates a variable to store a random value between 1 and 4.
                int value = _randomValue.Next(1, 5);

                // If the vampires spawned in the current wave is less than the amount to be spawned in that wave.
                if (_vampiresSpawnedInCurrentWave < ((_wave * 5) + 1))
                {
                    // If less than 10 vampires have been spawned.
                    if (_vampiresSpawned > -1 && _vampiresSpawned < 11)
                    {
                        // Adds a new weak vampire to the list of vampires.
                        _sharedVariables.vampires.Add(new WeakVampire(randomRow, rightCol, bounds));

                        // Increases the amount of vampires spawned in the current wave.
                        _vampiresSpawnedInCurrentWave++;
                    }

                    // If more than 11 but less than 21 vampires have been spawned.
                    else if (_vampiresSpawned > 11 && _vampiresSpawned < 21)
                    {
                        // Sorts through the possible values.
                        switch (value)
                        {
                            case 1:
                            case 2:
                                // Adds a new weakvapire.
                                _sharedVariables.vampires.Add(new WeakVampire(randomRow, rightCol, bounds));

                                // Increases the amount of vampires spawned in the current wave.
                                _vampiresSpawnedInCurrentWave++;
                                break;
                            case 3:
                            case 4:
                                // Adds a new fast vampire.
                                _sharedVariables.vampires.Add(new FastVampire(randomRow, rightCol, bounds));

                                // Increases the amount of vampires spawned in the current wave.
                                _vampiresSpawnedInCurrentWave++;
                                break;
                        }
                    }

                    // If more than 21 but less than 31 vampires have been spawned.
                    else if (_vampiresSpawned > 21 && _vampiresSpawned > 31)
                    {
                        // Sorts through the possible values.
                        switch (value)
                        {
                            case 1:
                            case 2:
                                // Adds a new weakvapire.
                                _sharedVariables.vampires.Add(new WeakVampire(randomRow, rightCol, bounds));

                                // Increases the amount of vampires spawned in the current wave.
                                _vampiresSpawnedInCurrentWave++;
                                break;
                            case 3:
                                // Adds a new fast vampire.
                                _sharedVariables.vampires.Add(new FastVampire(randomRow, rightCol, bounds));

                                // Increases the amount of vampires spawned in the current wave.
                                _vampiresSpawnedInCurrentWave++;
                                break;
                            case 4:
                                // Adds a new tank vampire.
                                _sharedVariables.vampires.Add(new TankVampire(randomRow, rightCol, bounds));

                                // Increases the amount of vampires spawned in the current wave.
                                _vampiresSpawnedInCurrentWave++;
                                break;
                        }
                    }

                    // Otherwise
                    else
                    {
                        // Sorts through all the possible values.
                        switch (value)
                        {
                            case 1:
                                // Adds a new weakvapire.
                                _sharedVariables.vampires.Add(new WeakVampire(randomRow, rightCol, bounds));

                                // Increases the amount of vampires spawned in the current wave.
                                _vampiresSpawnedInCurrentWave++;
                                break;
                            case 2:
                                // Adds a new fast vampire.
                                _sharedVariables.vampires.Add(new FastVampire(randomRow, rightCol, bounds));

                                // Increases the amount of vampires spawned in the current wave.
                                _vampiresSpawnedInCurrentWave++;
                                break;
                            case 3:
                                // Adds a new tank vampire.
                                _sharedVariables.vampires.Add(new TankVampire(randomRow, rightCol, bounds));

                                // Increases the amount of vampires spawned in the current wave.
                                _vampiresSpawnedInCurrentWave++;
                                break;
                            case 4:
                                // Adds a new killer vampire.
                                _sharedVariables.vampires.Add(new KillerVampire(randomRow, rightCol, bounds));

                                // Increases the amount of vampires spawned in the current wave.
                                _vampiresSpawnedInCurrentWave++;
                                break;
                        }
                    }
                }
                
                // If the vampires spawned in the current wave - 1 is equal the total amount to be spawned in that wave, and the total amount of vampires in the game is 0.
                else if (_vampiresSpawnedInCurrentWave - 1 == (_wave * 5) && _sharedVariables.vampires.Count == 0)
                {
                    // Increase the wave by 1.
                    _wave++;

                    // Increass the total vampires spawned by the amount spawned in the last wave.
                    _vampiresSpawned += _vampiresSpawnedInCurrentWave;

                    // Sets the vampires spawned in the current wave to 0.
                    _vampiresSpawnedInCurrentWave = 0;
                }
            }
        }

        /// <summary>
        /// Handles the moving of the vampires
        /// </summary>
        internal void MoveVampires()
        {
            // Loops through each vampire on the board.
            foreach (Vampire vampire in _sharedVariables.vampires)
            {
                // Calls the vampire to move towards to passed in edge.
                vampire.Move(_edge);
            }
        }

        /// <summary>
        /// Handles creating tiles around the trees
        /// </summary>
        /// <returns>List of tiles around trees</returns>
        internal List<Rectangle> CreateTilesAroundTrees()
        {
            // Creates a jagged array to store the tiles around the trees.
            Rectangle[][] tilesAroundTree = new Rectangle[(2 * SharedVariables.TREE_RADIUS) + 1][];

            // Creates a list to store all of the trees on the board.
            List<Tree> treesOnBoard = new List<Tree>();

            // Creates a list to store all of the tiles around the trees in one dimension.
            List<Rectangle> _tilesAroundTreeSingle = new List<Rectangle>();

            // Creates a 2D to store the board.
            Plant[,] board = _sharedVariables.board;

            // Loops from 0 to the length of the board's first dimension.
            for (int x = 0; x < board.GetLength(0); x++)
            {
                // Loops from 0 to the length of the board's second dimension.
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    // If the board at the specified position is a tree.
                    if (board[x, y] is Tree)
                    {
                        // Creates a variable to store the plant at the board position as a tree.
                        var tree = board[x, y] as Tree;

                        // Adds the tree to the list of trees.
                        treesOnBoard.Add(tree);
                    }
                }
            }

            // Creates a variable to count u
            int upCounter = 1;

            // Creates a variable to count down.
            int downCounter = ((2 * SharedVariables.TREE_RADIUS) - 1);

            // Loops from 0 to the specified amount.
            for (int row = 0; row < ((2 * SharedVariables.TREE_RADIUS) + 1); row++)
            {
                // If the row is less than the constant tree radius.
                if (row < SharedVariables.TREE_RADIUS)
                {
                    // Creates a new array of rectangles at the row.
                    tilesAroundTree[row] = new Rectangle[upCounter];

                    // Increases the counter by 2.
                    upCounter += 2;
                }

                // If the row is equal to the constant tree radius.
                else if (row == SharedVariables.TREE_RADIUS)
                {
                    // Creates an array of rectangles at the row.
                    tilesAroundTree[row] = new Rectangle[(2 * SharedVariables.TREE_RADIUS) + 1];
                }

                // If the row is greater than the constant tree radius.
                else if (row > SharedVariables.TREE_RADIUS)
                {
                    // Creates an array of rectangles at the row.
                    tilesAroundTree[row] = new Rectangle[downCounter];

                    // Decreaes the counter by 2.
                    downCounter -= 2;
                }
            }

            // Loops through each tree in the list of trees on the board.
            foreach (Tree tree in treesOnBoard)
            {
                // Loops from 0 to the specified value.
                for (int row = 0; row < ((2 * SharedVariables.TREE_RADIUS) + 1); row++)
                {
                    // Loops from 0 to the specified value.
                    for (int col = 0; col < tilesAroundTree[row].Length; col++)
                    {
                        // If the row is less than the tree radius.
                        if (row < SharedVariables.TREE_RADIUS)
                        {
                            /// Creates rectangles above the center.
                            if (col < ((tilesAroundTree[row].Length + 1) / 2))
                            {
                                _tilesAroundTreeSingle.Add(new Rectangle(((tree.Bounds.X + ((col - ((tilesAroundTree[row].Length + 1) / 2)) * tree.Bounds.Width)) + _sharedVariables.Width), (tree.Bounds.Y - (Math.Abs(row - SharedVariables.TREE_RADIUS) * tree.Bounds.Height)), tree.Bounds.Width, tree.Bounds.Height));
                            }
                            else if (col == ((tilesAroundTree[row].Length + 1) / 2))
                            {
                                _tilesAroundTreeSingle.Add(new Rectangle((tree.Bounds.X + _sharedVariables.Width), (tree.Bounds.Y - (Math.Abs(row - SharedVariables.TREE_RADIUS) * tree.Bounds.Height)), tree.Bounds.Width, tree.Bounds.Height));
                            }
                            else if (col > ((tilesAroundTree[row].Length + 1) / 2))
                            {
                                _tilesAroundTreeSingle.Add(new Rectangle(((tree.Bounds.X + ((col - ((tilesAroundTree[row].Length + 1) / 2)) * tree.Bounds.Width)) + _sharedVariables.Width), (tree.Bounds.Y - (Math.Abs(row - SharedVariables.TREE_RADIUS) * tree.Bounds.Height)), tree.Bounds.Width, tree.Bounds.Height));
                            }///
                        }

                        // If the row is less than the tree radius.
                        else if (row > SharedVariables.TREE_RADIUS)
                        {
                            /// Creates rectangles below the center.
                            if (col < ((tilesAroundTree[row].Length + 1) / 2))
                            {
                                _tilesAroundTreeSingle.Add(new Rectangle(((tree.Bounds.X + ((col - ((tilesAroundTree[row].Length + 1) / 2)) * tree.Bounds.Width)) + _sharedVariables.Width), (tree.Bounds.Y + (Math.Abs(row - SharedVariables.TREE_RADIUS) * tree.Bounds.Height)), tree.Bounds.Width, tree.Bounds.Height));
                            }
                            else if (col == ((tilesAroundTree[row].Length + 1) / 2))
                            {
                                _tilesAroundTreeSingle.Add(new Rectangle((tree.Bounds.X + _sharedVariables.Width), (tree.Bounds.Y + (Math.Abs(row - SharedVariables.TREE_RADIUS) * tree.Bounds.Height)), tree.Bounds.Width, tree.Bounds.Height));
                            }
                            else if (col > ((tilesAroundTree[row].Length + 1) / 2))
                            {
                                _tilesAroundTreeSingle.Add(new Rectangle(((tree.Bounds.X + ((col - ((tilesAroundTree[row].Length + 1) / 2)) * tree.Bounds.Width)) + _sharedVariables.Width), (tree.Bounds.Y + (Math.Abs(row - SharedVariables.TREE_RADIUS) * tree.Bounds.Height)), tree.Bounds.Width, tree.Bounds.Height));
                            }///
                        }

                        // If the row is equal to the tree radius.
                        else if (row == SharedVariables.TREE_RADIUS)
                        {
                            /// Creates rectangles at the center.
                            if (col < (((tilesAroundTree[row].Length + 1) / 2) - 1))
                            {
                                _tilesAroundTreeSingle.Add(new Rectangle(((tree.Bounds.X + ((col - ((tilesAroundTree[row].Length + 1) / 2)) * tree.Bounds.Width)) + _sharedVariables.Width), tree.Bounds.Y, tree.Bounds.Width, tree.Bounds.Height));
                            }
                            else if (col == ((tilesAroundTree[row].Length + 1) / 2))
                            {
                                _tilesAroundTreeSingle.Add(new Rectangle((tree.Bounds.X + _sharedVariables.Width), tree.Bounds.Y, tree.Bounds.Width, tree.Bounds.Height));
                            }
                            else if (col > ((tilesAroundTree[row].Length + 1) / 2))
                            {
                                _tilesAroundTreeSingle.Add(new Rectangle(((tree.Bounds.X + ((col - ((tilesAroundTree[row].Length + 1) / 2)) * tree.Bounds.Width)) + _sharedVariables.Width), tree.Bounds.Y, tree.Bounds.Width, tree.Bounds.Height));
                            }///
                        }
                    }
                }
            }

            // Returns the list of retangles around the trees.
            return _tilesAroundTreeSingle;
        }

        /// <summary>
        /// Handles checking whether a plant can be placed at a specific location
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="place"></param>
        /// <param name="plantToPlace"></param>
        /// <returns>Whether a plant can be placed at the specific location</returns>
        internal bool CheckPlace(int row, int col, Rectangle place, Plant plantToPlace)
        {
            // Creates a 2D array to store the board.
            Plant[,] board = _sharedVariables.board;

            // If the board location is null and the plant to be placed is not a tree.
            if (board[row, col] == null && !(plantToPlace is Tree))
            {
                // Loops through each tile around tree in the list of tiles around all the trees.
                foreach (Rectangle tileAroundTree in CreateTilesAroundTrees())
                {
                    // If the rectangle where the plant is to be placed at is equal to the tile around the tree.
                    if (place.Equals(tileAroundTree))
                    {
                        // Return true.
                        return true;
                    }

                    // Otherwise
                    else
                    {
                        // Do nothing.
                    }
                }

                // Return false.
                return false;
            }

            // If the plant to be placed is a tree.
            else if (plantToPlace is Tree)
            {
                // Return true.
                return true;
            }

            // Otherwise
            else
            {
                // Return false;
                return false;
            }
        }

        /// <summary>
        /// Handles making moons
        /// </summary>
        /// <param name="endDest"></param>
        internal void MakeMoons(Point endDest)
        {
            // Creates a 2D array to store the board.
            Plant[,] board = _sharedVariables.board;

            // Loops from 0 to the length of the board's first dimension.
            for (int x = 0; x < board.GetLength(0); x++)
            {
                // Loops from 0 to the length of the board's second dimension.
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    // If the specific board location is a moon flower.
                    if (board[x, y] is MoonFlower)
                    {
                        // Creates a moon flower to store the board location as a moon flower.
                        MoonFlower moonFlower = board[x, y] as MoonFlower;

                        // Calls said moon flower to make a moon passing in the end destination of the moon.
                        moonFlower.MakeMoon(endDest);
                    }
                }
            }
        }
    }
}
