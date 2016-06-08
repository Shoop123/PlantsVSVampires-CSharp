/* Daniel Berezovski
 * June 1st, 2016
 * This class store info about the tree
 */
using System.Drawing;

namespace Plant_VS.Vampires.Plants
{
    public class Tree : Plant
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="bounds">Bounds</param>
        public Tree(int row, int col, Rectangle bounds) : base(row, col, bounds) 
        {
            //Set the price and health of the tree
            _price = SharedVariables.TREE_PRICE;
            _health = SharedVariables.TREE_HEALTH;
        }
    }
}
