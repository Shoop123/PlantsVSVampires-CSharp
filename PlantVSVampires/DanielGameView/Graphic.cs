/* Daniel Berezovski
 * June 1st, 2016
 * This class prepares all of the graphics
 */
using Plant_VS.Vampires;
using Plant_VS.Vampires.Plants;
using Plant_VS.Vampires.Vampires;
using System.Drawing;

namespace DanielView
{
    internal static class Graphic
    {
        //Graphics for the plants
        private static Image _moonFlower = Properties.Resources.moonflower;
        private static Image _cherryBomb = Properties.Resources.cherrybomb;
        private static Image _potatoMine = Properties.Resources.potatomine;
        private static Image _peaShooter = Properties.Resources.peashooter;
        private static Image _snowPea = Properties.Resources.snowpea;
        private static Image _tree = Properties.Resources.tree;
        private static Image _wallNut = Properties.Resources.wallnut;

        //Graphics for the vampires
        private static Image _fastVampire = Properties.Resources.fastvampire;
        private static Image _weakVampire = Properties.Resources.weakvampire;
        private static Image _killerVampire = Properties.Resources.killervampire;
        private static Image _tankVampire = Properties.Resources.tankvampire;
        
        //Graphics for the projectiles
        private static Image _explosionProjectile = Properties.Resources.explosionprojectile;
        private static Image _peaProjectile = Properties.Resources.peaprojectile;
        private static Image _snowPeaProjectile = Properties.Resources.snowpeaprojectile;

        //Moon graphic
        private static Image _moon = Properties.Resources.moon;

        //Unknown graphic
        private static Image _unknown = Properties.Resources.unknown;

        /// <summary>
        /// Gets the cooresponding image
        /// </summary>
        /// <param name="entity">Entity for </param>
        /// <returns>The image</returns>
        internal static Image GetImage(Entity entity)
        {
            //Checks what kind of entity it is, and returns the appropriate image
            if (entity is MoonFlower)
            {
                return _moonFlower;
            }
            else if (entity is CherryBomb)
            {
                return _cherryBomb;
            }
            else if (entity is PotatoMine)
            {
                return _potatoMine;
            }
            else if (entity is PeaShooter)
            {
                return _peaShooter;
            }
            else if (entity is SnowPea)
            {
                return _snowPea;
            }
            else if (entity is Tree)
            {
                return _tree;
            }
            else if (entity is WallNut)
            {
                return _wallNut;
            }
            else if (entity is FastVampire)
            {
                return _fastVampire;
            }
            else if (entity is WeakVampire)
            {
                return _weakVampire;
            }
            else if (entity is KillerVampire)
            {
                return _killerVampire;
            }
            else if (entity is TankVampire)
            {
                return _tankVampire;
            }
            else if (entity is Projectile)
            {
                Projectile projectile = entity as Projectile;

                if (projectile.Type == Projectile.ProjectileType.BombProjectile || projectile.Type == Projectile.ProjectileType.MineProjectile)
                {
                    return _explosionProjectile;
                }
                else if (projectile.Type == Projectile.ProjectileType.PeaShooterProjectile)
                {
                    return _peaProjectile;
                }
                else if (projectile.Type == Projectile.ProjectileType.SnowPeaProjectile)
                {
                    return _snowPeaProjectile;
                }
            }
            else if(entity is Moon)
            {
                return _moon;
            }

            return _unknown;
        }
    }
}
