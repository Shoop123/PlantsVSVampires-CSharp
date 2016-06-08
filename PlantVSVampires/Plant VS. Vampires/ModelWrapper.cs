/* Daniel Berezovski, Elliot Spall, but mostly Daniel Berezovski
 * June 1st, 2016
 * This class wraps the two models into one
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plant_VS.Vampires.Plants;
using System.Drawing;

namespace Plant_VS.Vampires
{
    public class ModelWrapper
    {
        //Plants
        private Plant[] _plants;

        //Common variables
        private SharedVariables _vars;

        //Both models
        private ElliotModel _eModel;
        private DanielModel _dModel;

        //Screen width
        private int _screenWidth;

        //Y location of when the board starts
        private int _topOffset;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rows">Row</param>
        /// <param name="cols">Column</param>
        /// <param name="width">Width of tiles</param>
        /// <param name="height">Height of tiles</param>
        /// <param name="screenWidth">Width of screen</param>
        /// <param name="topOffset">Offset of top of board</param>
        /// <param name="edge">The left edge of the board</param>
        public ModelWrapper(int rows, int cols, int width, int height, int screenWidth, int topOffset, int edge, int timeDelay)
        {
            //Create the variables
            _vars = new SharedVariables(rows, cols, width, height);

            //Create the models
            _dModel = new DanielModel(_vars, screenWidth, this, timeDelay);
            _eModel = new ElliotModel(_vars, edge);

            //Initialize the plants
            _plants = new Plant[SharedVariables.PLANT_COUNT];
            
            //Set the screen width
            _screenWidth = screenWidth;

            //Set the top offset
            _topOffset = topOffset;

            //Set the plants
            SetPlants();
        }

        /// <summary>
        /// Gets the wave of the game
        /// </summary>
        /// <returns>Wave</returns>
        public int GetWave()
        {
            return _eModel.GetWave();
        }

        /// <summary>
        /// Checks if that spot is available
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="place">Location</param>
        /// <param name="type">Type of plant</param>
        /// <returns>If the location is free</returns>
        public bool CheckPlace(int row, int col, Rectangle place, Plant type)
        {
            return _eModel.CheckPlace(row, col, place, type);
        }

        /// <summary>
        /// Buys and places a plant
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="plant">Plant to buy</param>
        public void BuyAndPlace(int row, int col, Plant plant)
        {
            _dModel.BuyAndPlace(row, col, plant);
        }

        /// <summary>
        /// When to start the game
        /// </summary>
        /// <returns>If the game should start</returns>
        public bool StartGame()
        {
            return _dModel.StartGame();
        }

        /// <summary>
        /// Time until the game starta
        /// </summary>
        /// <returns>Seconds</returns>
        public int SecondsToStartGame()
        {
            return _dModel.SecondsToStartGame();
        }

        /// <summary>
        /// Updates game mechanics
        /// </summary>
        public void Update()
        {
            //Update the gamae
            _dModel.UpdateGame();

            //If the game has started
            if (StartGame())
            {
                //Makes vampires
                _eModel.MakeVampire(_screenWidth, _topOffset);

                //Moves vampires
                _eModel.MoveVampires();

                //Collision detection for projectiles
                _eModel.ProjectileHit();
            }
        }

        /// <summary>
        /// Makes the moons
        /// </summary>
        /// <param name="moonCountLocation">The location if the main moon</param>
        public void MakeMoons(Point moonCountLocation)
        {
            _eModel.MakeMoons(moonCountLocation);
        }

        /// <summary>
        /// Gets the board of plants
        /// </summary>
        /// <returns>The board</returns>
        public Plant[,] GetBoard()
        {
            return _vars.board;
        }

        /// <summary>
        /// Gets the amount of moons the player has
        /// </summary>
        /// <returns>The player's moon count</returns>
        public int GetPlayerMoons()
        {
            return _vars.GetMoons();
        }

        /// <summary>
        /// The health of the player
        /// </summary>
        /// <returns></returns>
        public int GetHealth()
        {
            return _vars.GetHealth();
        }

        /// <summary>
        /// Adds moons to the total
        /// </summary>
        /// <param name="amount">Amount to add</param>
        public void AddMoons(int amount)
        {
            _vars.AddMoons(amount);
        }

        /// <summary>
        /// Gets all types of plants
        /// </summary>
        /// <returns>The plant array</returns>
        public Plant[] GetPlants()
        {
            return _plants;
        }

        /// <summary>
        /// Gets the projectiles on the board
        /// </summary>
        /// <returns>The projectiles</returns>
        public List<Projectile> GetProjectiles()
        {
            return _vars.projectiles;
        }

        /// <summary>
        /// Gets the vampires
        /// </summary>
        /// <returns>The vampires</returns>
        public List<Vampire> GetVampires()
        {
            return _vars.vampires;
        }

        /// <summary>
        /// Sets one of each type of plant
        /// </summary>
        private void SetPlants()
        {
            _plants[0] = new Tree(-1, -1, new Rectangle());
            _plants[1] = new MoonFlower(-1, -1, new Rectangle());
            _plants[2] = new PeaShooter(-1, -1, new Rectangle());
            _plants[3] = new WallNut(-1, -1, new Rectangle());
            _plants[4] = new SnowPea(-1, -1, new Rectangle());
            _plants[5] = new PotatoMine(-1, -1, new Rectangle());
            _plants[6] = new CherryBomb(-1, -1, new Rectangle());
        }

        /// <summary>
        /// Gets the price of that plant
        /// </summary>
        /// <param name="plant">The plant in question</param>
        /// <returns>The price</returns>
        public int GetPlantPrice(Plant plant)
        {
            return plant.GetPrice();
        }

        /// <summary>
        /// Gets the health for a plant
        /// </summary>
        /// <param name="plant">The plant in question</param>
        /// <returns>The health</returns>
        public int GetPlantHealth(Plant plant)
        {
            return plant.GetHealth();
        }
        
        /// <summary>
        /// Gets the moons for a moonflower
        /// </summary>
        /// <param name="moonFlower">The moonflower in question</param>
        /// <returns>The moons on it</returns>
        public List<Moon> GetMoons(MoonFlower moonFlower)
        {
            return moonFlower.GetMoons();
        }

        /// <summary>
        /// Gets the moons to draw
        /// </summary>
        /// <param name="moonFlower">The moonflower in question</param>
        /// <returns>The moons to draw</returns>
        public List<Moon> GetMoonsToDraw(MoonFlower moonFlower)
        {
            return moonFlower.GetMoonsToDraw();
        }

        /// <summary>
        /// Collects moons
        /// </summary>
        /// <param name="moonFlower">The moon to collect from</param>
        /// <param name="moon">The moon to collect</param>
        public void CollectMoon(MoonFlower moonFlower, Moon moon)
        {
            //Collect it
            moonFlower.CollectMoon(moon);

            //Cascade the moons
            moonFlower.CascadeMoons();
        }

        /// <summary>
        /// Gets the seconds to detonation for a cherrybomb
        /// </summary>
        /// <param name="cherryBomb">The cherrybomb in question</param>
        /// <returns>The time</returns>
        public int GetSecondsToDetonation(CherryBomb cherryBomb)
        {
            return cherryBomb.GetSecondsToDetonation();
        }

        /// <summary>
        /// Gets the splash radius for a projectile
        /// </summary>
        /// <param name="projectile">The projectile</param>
        /// <returns>The splash radius</returns>
        public int GetProjectileSplashRadius(Projectile projectile)
        {
            return projectile.GetSplashRadius();
        }

        /// <summary>
        /// If the player lost
        /// </summary>
        /// <returns>Player's lost status</returns>
        public bool PlayerLost()
        {
            return _vars.GetHealth() <= 0;
        }

        /// <summary>
        /// A reason for what happens when a player buys a plant
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="plant">Plant to buy</param>
        /// <returns>Why something can be done</returns>
        public Reason GetReason(int row, int col, Plant plant)
        {
            //Check if there is already a plant there
            if (GetBoard()[row, col] != null)
                return Reason.Occupied;
            //Check if there is a tree nearby
            else if (!CheckPlace(row, col, plant.Bounds, plant))
                return Reason.NoTreesNearby;
            //Check if the player has enough moons
            else if (plant.GetPrice() > GetPlayerMoons())
                return Reason.InsufficientMoons;

            //Return that everything was fine
            return Reason.Accepted;
        }

        /// <summary>
        /// Possible reasons for a plant not being purchasable
        /// </summary>
        public enum Reason
        {
            //Everything is fine
            Accepted,

            //No enough moons
            InsufficientMoons,

            //There is already a plant there
            Occupied,

            //There aren't any tree nearby
            NoTreesNearby,
        }
    }
}
