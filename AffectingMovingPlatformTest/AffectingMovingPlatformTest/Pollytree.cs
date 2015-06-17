using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;


namespace AffectingMovingPlatformTest {
    class GameObjects {

        protected Texture2D texture;
        protected Vector2 pos;
        protected float width, height;
        protected internal Rectangle rec;
        protected Color btnColor;

        public GameObjects(Texture2D texture, Vector2 pos, float width, float height) {
            this.texture = texture;
            this.pos = pos;
            this.width = width;
            this.height = height;

            rec = new Rectangle((int)pos.X, (int)pos.Y, (int)width, (int)height);
        }
        public virtual void Update() { }
        public virtual void Draw(SpriteBatch spriteBatch) { }

        MouseState mState;
        public virtual bool OnHover() { //if mouse is on top of object
            mState = Mouse.GetState();
            if (mState.X > pos.X && mState.X < (pos.X + width)) {
                if (mState.Y > pos.Y && mState.Y < (pos.Y + height)) {
                    return true;
                }
            }
            return false;
        }
        bool objectIsClicked = false;
        public virtual bool IsClicked() { //checks if object is clicked
            mState = Mouse.GetState();
            if (mState.LeftButton == ButtonState.Pressed && !objectIsClicked) {
                objectIsClicked = true;
                if (mState.X > pos.X && mState.X < (pos.X + width)) {
                    if (mState.Y > pos.Y && mState.Y < (pos.Y + height)) {
                        return true; //object is clicked!
                    }
                }
            }
            else if (mState.LeftButton == ButtonState.Released) { objectIsClicked = false; }
            return false; // == not Clicked
        }

        //Properties
        public Texture2D Texture { set { texture = value; } }
        public Vector2 Pos { get { return pos; } set { pos = value; } }
        public float Width {
            get { return width; }
            set {
                if (value <= 1) { width = 1; }
                else { width = value; }
            }
        }
        public float Height {
            get { return height; }
            set {
                if (value <= 1) { height = 1; }
                else { height = value; }
            }
        }

    }//end GameObjects

    class PhysicalObjects : GameObjects {

        protected bool isAlive = true;
        protected Rectangle intersectRec;

        public PhysicalObjects(Texture2D texture, Vector2 pos, float width, float height) : base(texture, pos, width, height) {
            intersectRec = new Rectangle((int)pos.X, (int)(pos.Y + height * 0.95f), (int)width, (int)(height * 0.05f));
        }

        public virtual bool CollisionDetection(Rectangle otherRec) {
            //Updating Recs
            intersectRec.X = (int)pos.X;
            intersectRec.Y = (int)(pos.Y + height * 0.95f);

            return intersectRec.Intersects(otherRec);
        }

        //Properties
        public bool IsAlive { get { return isAlive; } set { isAlive = value; } }
        public Rectangle IntersectRec { get { return intersectRec; } set { intersectRec = value; } }
    }//end PhysicalObjects

    class MovingObjects : PhysicalObjects {

        protected Vector2 speed;
        protected bool isMoving;
        protected bool isFalling;

        public MovingObjects(Texture2D texture, Vector2 pos, float width, float height, Vector2 speed)
            : base(texture, pos, width, height) {
            this.speed = speed;
            this.isMoving = false;
            this.isFalling = false;
        }

        public virtual void Update(GameTime gameTime) { }

        //properties
        public bool IsMoving { get { return isMoving; } set { isMoving = value; } }
        public bool IsFalling { get { return isFalling; } set { isFalling = value; } }

    }//end MovingObjects

    class Buttons : GameObjects {

        //protected Color btnColor;

        public Buttons(Texture2D texture, Vector2 pos, float width, float height) : base(texture, pos, width, height) { }

        public virtual void Draw(SpriteBatch spriteBatch, Color onHoverColor) {
            if (OnHover()) { btnColor = onHoverColor; }
            else { btnColor = Color.White; }
            spriteBatch.Draw(texture, rec, btnColor);
        }

    }//end Buttons

    class Platforms : MovingObjects {
        
        public Platforms(Texture2D texture, Vector2 pos, float width, float height, Vector2 speed) : base(texture, pos, width, height, speed) { }

        public virtual void Draw(SpriteBatch spriteBatch, Color color) {
            spriteBatch.Draw(texture, rec, color);
        }

        public override bool CollisionDetection(Rectangle otherRec) {
            //Updating Recs
            intersectRec.X = (int)pos.X;
            intersectRec.Y = (int)(pos.Y);

            return intersectRec.Intersects(otherRec);
        }

    }//end Platforms

}
