/* Daniel Berezovski
 * June 1st, 2016
 * This class has non specific methods
 */
namespace DanielView
{
    internal static class StaticMethods
    {
        /// <summary>
        /// Checks if the mouse intersect a certain boundary
        /// </summary>
        /// <param name="x">X of bounds</param>
        /// <param name="y">Y of bounds</param>
        /// <param name="width">Width of bounds</param>
        /// <param name="height">Height of bounds</param>
        /// <param name="mouseX">X of mouse</param>
        /// <param name="mouseY">Y of mouse</param>
        /// <returns></returns>
        internal static bool MouseIntersects(int x, int y, int width, int height, int mouseX, int mouseY)
        {
            //Check if the mouse location intersects the boundary given
            if (mouseX > x && mouseX < x + width && mouseY > y && mouseY < y + height)
            {
                //Return true if it does
                return true;
            }

            //Return false if it doesn't
            return false;
        }
    }
}
