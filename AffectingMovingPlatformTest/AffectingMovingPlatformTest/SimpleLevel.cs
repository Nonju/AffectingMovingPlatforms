using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;


namespace AffectingMovingPlatformTest {
    static class SimpleLevel {

        //level
        static List<MovingObjects> movingObjectsList;
        static List<PlatformsFolder.MovingPlatform> movingPlatformsList;
        static List<PlatformsFolder.StaticPlatform> staticPlatformsList;
        static PlatformsFolder.MovingPlatform mPlat1, mPlat2;
        static List<Vector2> mPlat1Paths, mPlat2Paths;
        static PlatformsFolder.StaticPlatform groundPlatform, sPlat1;

        static Texture2D platformTexture;
        static float mPlat1Width, mPlat2Width, sPlat1Width, platHeight;
        static Vector2 mPlat1Pos, mPlat2Pos, sPlat1Pos;
        static Vector2 platformSpeed;

        //Player
        static MovingObjects player;
        static Texture2D playerTexture;
        static Vector2 playerPos;
        static float playerWidth, playerHeight;
        static Vector2 playerSpeed;

        public static void Load(ContentManager content, GameWindow window) {
            //LEVEL
            movingObjectsList = new List<MovingObjects>();
            movingPlatformsList = new List<PlatformsFolder.MovingPlatform>();
            staticPlatformsList = new List<PlatformsFolder.StaticPlatform>();

            //general values
            platformTexture = content.Load<Texture2D>("Images/Platforms/BasicPlatformTexure");
            platHeight = window.ClientBounds.Height * 0.05f;
            platformSpeed = new Vector2(window.ClientBounds.Width * 0.001f, window.ClientBounds.Height * 0.003f);

            //groundPlatform
            groundPlatform = new PlatformsFolder.StaticPlatform(platformTexture, new Vector2(0, window.ClientBounds.Height * 0.9f), window.ClientBounds.Width, window.ClientBounds.Height * 0.1f, new Vector2(0));
            staticPlatformsList.Add(groundPlatform);

            //mPlat1
            mPlat1Width = window.ClientBounds.Width * 0.15f;
            mPlat1Pos = new Vector2(0, window.ClientBounds.Height * 0.6f);

            mPlat1Paths = new List<Vector2>();
            mPlat1Paths.Add(mPlat1Pos);
            mPlat1Paths.Add(new Vector2(mPlat1Pos.X, window.ClientBounds.Height * 0.3f));
            mPlat1 = new PlatformsFolder.MovingPlatform(platformTexture, mPlat1Pos, mPlat1Width, platHeight, platformSpeed, mPlat1Paths);
            movingPlatformsList.Add(mPlat1);

            //mPlat2
            mPlat2Width = window.ClientBounds.Width * 0.2f;
            mPlat2Pos = new Vector2(window.ClientBounds.Width * 0.4f, window.ClientBounds.Height * 0.45f);

            mPlat2Paths = new List<Vector2>();
            mPlat2Paths.Add(mPlat2Pos);
            mPlat2Paths.Add(new Vector2(window.ClientBounds.Width * 0.3f, window.ClientBounds.Height * 0.2f));
            mPlat2Paths.Add(new Vector2(window.ClientBounds.Width * 0.2f, window.ClientBounds.Height * 0.7f));
            mPlat2 = new PlatformsFolder.MovingPlatform(platformTexture, mPlat2Pos, mPlat2Width, platHeight, platformSpeed, mPlat2Paths);
            movingPlatformsList.Add(mPlat2);

            //sPlat1
            sPlat1Width = window.ClientBounds.Width * 0.4f;
            sPlat1Pos = new Vector2((window.ClientBounds.Width - sPlat1Width), window.ClientBounds.Height * 0.35f);
            sPlat1 = new PlatformsFolder.StaticPlatform(platformTexture, sPlat1Pos, sPlat1Width, platHeight, platformSpeed);
            staticPlatformsList.Add(sPlat1);

            //MOVING-OBJECTS
            //Player
            playerTexture = content.Load<Texture2D>("Images/Player/PlayerSpreadsheet");
            playerPos = new Vector2(window.ClientBounds.Width * 0.1f, window.ClientBounds.Height * 0.3f);
            playerWidth = window.ClientBounds.Width * 0.1f;
            playerHeight = window.ClientBounds.Height * 0.25f;
            playerSpeed = new Vector2(window.ClientBounds.Width * 0.005f, window.ClientBounds.Height * 0.01f);
            player = new Player(content, window, playerTexture, playerPos, playerWidth, playerHeight, playerSpeed);
            movingObjectsList.Add(player);

        }

        static bool intersectFound = false;
        public static void Update(GameTime gameTime) {
            //MovingObjects
            foreach (MovingObjects mO in movingObjectsList) {
                mO.Update(gameTime);
            }

            //on touch (movingPlatforms)
            foreach (PlatformsFolder.MovingPlatform mpf in movingPlatformsList) {
                mpf.AffectOnTouch(movingObjectsList);
                mpf.Movement();
            }

            //staticPlatforms
            foreach (PlatformsFolder.StaticPlatform spf in staticPlatformsList) { //NOT SURE IF NEEDED
                spf.Update();
            }

            //Collisiondetection for MovingObjects
            foreach (MovingObjects mO in movingObjectsList) {
                intersectFound = false;
                foreach (PlatformsFolder.MovingPlatform mpf in movingPlatformsList) {
                    if (mO.CollisionDetection(mpf.rec)) {
                        intersectFound = true;
                        mO.IsFalling = false;
                        break;
                    }
                    else { mO.IsFalling = true; }
                }
                if (intersectFound) { break; } //if intersect's found amongst moving platforms, quit searching

                foreach (PlatformsFolder.StaticPlatform spf in staticPlatformsList) {
                    if (mO.CollisionDetection(spf.rec)) {                   
                        mO.IsFalling = false;
                        break;
                    }
                    else { mO.IsFalling = true; }
                }
            }

        }

        public static void Draw(SpriteBatch spriteBatch) {
            //MovingObjectsList
            foreach (MovingObjects mO in movingObjectsList) {
                mO.Draw(spriteBatch);
            }

            //MovingPlatformsList
            foreach (PlatformsFolder.MovingPlatform mpf in movingPlatformsList) {
                mpf.Draw(spriteBatch, Color.Black);
            }

            //StaticPlatformList
            foreach (PlatformsFolder.StaticPlatform spf in staticPlatformsList) {
                spf.Draw(spriteBatch, Color.Gray);
            }

        }

    }
}
