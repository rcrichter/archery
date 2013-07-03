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
    class Enemy : MovingSprite
    {
        public int SpawnTime;
        public bool Spawned;
        public bool Die;
        public float alphaLevel;
        public int ScoreValue;

        public Enemy(Texture2D texture, Vector2 position, float speed, int time)
            : base(texture, position, speed)
        {
            Spawned = false;
            SpawnTime = time;
            alphaLevel = 1f;
        }

        public new virtual void Update(Game1 game)
        {
            if (Die)
            {
                if (alphaLevel <= 0.0f)
                {
                    active = false;
                    game.Score += ScoreValue;
                }
                alphaLevel -= .05f;
            }
            base.Update(game);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteTexture, spritePosition, new Color(255, 255, 255, alphaLevel));
            //base.Draw(spriteBatch);
        }
    }
}
