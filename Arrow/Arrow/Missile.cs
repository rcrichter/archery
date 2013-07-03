using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Archery
{
    class Missile : MovingSprite
    {
        public Missile(Texture2D texture, Vector2 position, float speed)
            :base(texture, position, speed)
        {
            
        }

        public override void Update(Game1 game)
        {
            spritePosition.X += spriteSpeed;
            if (spritePosition.X > Game1.SCREEN_WIDTH + 200)
            {
                active = false;
            }
            base.Update(game);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteTexture, spritePosition, Color.White);
            
        }
    }
}
