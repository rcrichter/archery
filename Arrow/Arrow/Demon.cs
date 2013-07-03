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
    class Demon : Enemy
    {
        public Demon(Texture2D texture, Vector2 position, float speed, int spawnTime)
            : base(texture, position, speed, spawnTime)
        {
            ScoreValue = 10;
        }

        public override void Update(Game1 game)
        {

            if (!Die)
            {
                spritePosition.Y -= spriteSpeed;
                if (spritePosition.Y < (0 - spriteTexture.Height))
                {
                    spritePosition.Y = Game1.SCREEN_HEIGHT;
                }
            }
            base.Update(game);
        }

    }
}
