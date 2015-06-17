using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;


namespace AffectingMovingPlatformTest {
    class Player : MovingObjects {


        //Animation
        Animator playerAnimator;
        int frameWidth, frameheight;
        int staticFrame, startFrameX, startFrameY;
        int xRow, yRow;
        int timeToNextFrame;
        Rectangle aniRec;

        GameWindow window;

        //remove
        Texture2D tempTexture;

        public Player(ContentManager content, GameWindow window, Texture2D texture, Vector2 pos, float width, float height, Vector2 speed) : base(texture, pos, width, height, speed) {
            this.window = window;

            //Animation
            frameWidth = 200;
            frameheight = 200;
            startFrameX = 1;
            staticFrame = 0;
            xRow = 5;
            yRow = 2;
            timeToNextFrame = 120;

            playerAnimator = new Animator(texture, width, height, frameWidth, frameheight);

            //REMOVE
            tempTexture = content.Load<Texture2D>("Images/Platforms/BasicPlatformTexure");

        }
        KeyboardState kState;
        public override void Update(GameTime gameTime) {
            kState = Keyboard.GetState();

            if (kState.IsKeyDown(Keys.D)) { //Right
                IsMoving = true;
                startFrameY = 0; //what spreadSheetframe to use
                if (pos.X <= (window.ClientBounds.Width - (rec.Width / 2))) { pos.X += speed.X; }
                else { pos.X -= speed.X; }
            }
            else if (kState.IsKeyDown(Keys.A)) { //Left
                IsMoving = true;
                startFrameY = frameheight; //what spreadSheetframe to use
                if (pos.X >= (0 - (rec.Width / 2))) { pos.X -= speed.X; }
                else { pos.X += speed.X; }
            }
            else { IsMoving = false; }
            //TESTKEYS!
            if (kState.IsKeyDown(Keys.S)) { pos.Y += 5; }
            else if (kState.IsKeyDown(Keys.W)) { pos.Y -= 5; }

            if (isFalling) { pos.Y += speed.Y / 2; }

            //Update Recs
            aniRec = playerAnimator.Update(gameTime, staticFrame, startFrameX, startFrameY, isMoving, xRow, yRow, timeToNextFrame);
            rec.X = (int)pos.X;
            rec.Y = (int)pos.Y;
            
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, rec, aniRec, Color.HotPink);//player
            spriteBatch.Draw(tempTexture, intersectRec, Color.Pink);
        }

    }
}
