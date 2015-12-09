using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Game_of_Drones
{
	public class FireLaser
	{
			protected void fLaser(GameTime gameTime)
			{
				// govern the rate of fire for our lasers
				if (gameTime.TotalGameTime - previousLaserSpawnTime > laserSpawnTime)
				{
					previousLaserSpawnTime = gameTime.TotalGameTime;
					// Add the laer to our list.
					AddLaser();
				}
			}
	
	}
}

