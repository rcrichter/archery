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
    class Sprite
    {
        protected Vector2 spritePosition;
        protected Texture2D spriteTexture;

        public Sprite(Texture2D texture, Vector2 position)
        {
            spritePosition = position;
            spriteTexture = texture;
        }
               
        public virtual void Update(Game1 game)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteTexture, spritePosition, Color.White);
        }

        public Vector2 getPosition()
        {
            return spritePosition;
        }

        public Texture2D getTexture()
        {
            return spriteTexture;
        }
    }
}
