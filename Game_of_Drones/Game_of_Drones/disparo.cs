using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_of_Drones
{
	public class disparo
	{  // animation the represents the laser animation.
		public Animation LaserAnimation;

		// the speed the laser travels
		float laserMoveSpeed = 30f;

		// position of the laser
		public Vector2 Position;

		// The damage the laser deals.
		int Damage = 10;

		// set the laser to active
		public bool Active;

		// Laser beams range.
		int Range;

		// the width of the laser image.
		public int Width
		{
			get { return LaserAnimation.FrameWidth; }
		}

		// the height of the laser image.
		public int Height
		{
			get { return LaserAnimation.FrameHeight; }
		}
		public void Initialize(Animation LaserAnimation, Vector2 position)
		{
			LaserAnimation = LaserAnimation;
			Position = position;
			Active = true;
		}

		public void Update(GameTime gameTime)
		{
			Position.X += laserMoveSpeed;
			LaserAnimation.Position = Position;
			LaserAnimation.Update(gameTime);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			LaserAnimation.Draw(spriteBatch);
		}
	}
}

