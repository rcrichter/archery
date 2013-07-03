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
    class MovingSprite : Sprite
    {
        protected float spriteSpeed;
        protected bool active;

        public MovingSprite(Texture2D texture, Vector2 position, float speed)
            : base(texture, position)
        {
            spriteSpeed = speed;
            active = true;
        }

        public override void Update(Game1 game)
        {
            base.Update(game);
        }

        public bool getActive()
        {
            return active;
        }

        public void setActive(bool value)
        {
            active = value;
        }


    }
}
