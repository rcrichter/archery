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
    class Player : MovingSprite
    {
        public List<Missile> arrows;
        Texture2D arrowTexture;
        Texture2D ammoTexture;
        public int Ammo;
        


        public Player(Texture2D texture, Vector2 position, float speed, Texture2D arrowTexture, Texture2D ammoTexture)
            :base(texture, position, speed)
        {
            arrows = new List<Missile>();
            this.arrowTexture = arrowTexture;
            this.ammoTexture = ammoTexture;
        }


        public void Update(GamePadState currentGamePadState, GamePadState previousGamePadState, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, Game1 game)
        {
            float stickY = currentGamePadState.ThumbSticks.Left.Y;
            spritePosition.Y -= stickY * spriteSpeed;

            if(currentKeyboardState.IsKeyDown(Keys.W))
            {
                spritePosition.Y -= spriteSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.S))
            {
                spritePosition.Y += spriteSpeed;
            }

            if (spritePosition.Y <= 0)
            {
                spritePosition.Y = 0;
            }
            else if (spritePosition.Y >= Game1.SCREEN_HEIGHT - spriteTexture.Height)
            {
                spritePosition.Y = Game1.SCREEN_HEIGHT - spriteTexture.Height;
            }

            if (currentGamePadState.IsButtonDown(Buttons.A) && previousGamePadState.IsButtonUp(Buttons.A))
            {
                if (Ammo > 0)
                {
                    arrows.Add(new Missile(arrowTexture, new Vector2(this.spritePosition.X, this.spritePosition.Y + 13), 8.0f));
                    Ammo--;
                }
            }

            List<Missile> newArrowList = new List<Missile>();

            foreach(Missile arrow in arrows)
            {
                arrow.Update(game);
                if (arrow.getActive())
                {
                    newArrowList.Add(arrow);
                }
            }

            arrows = newArrowList;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Missile arrow in arrows)
            {
                arrow.Draw(spriteBatch);
            }
            DrawAmmo(spriteBatch);

            base.Draw(spriteBatch);
        }

        public void ClearArrow()
        {
            arrows = new List<Missile>();
        }

        private void DrawAmmo(SpriteBatch spriteBatch)
        {
            int x = 700;

            for (int i = 0; i < Ammo; i++)
            {
                spriteBatch.Draw(ammoTexture, new Vector2(x, 20), Color.White);
                x -= 10;
            }
        }
    }
}
