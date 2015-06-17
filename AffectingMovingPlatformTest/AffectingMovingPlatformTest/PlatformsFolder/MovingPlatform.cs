using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;


namespace AffectingMovingPlatformTest.PlatformsFolder {
    class MovingPlatform : Platforms {

        Vector2 lastPos, tempVector;
        List<Vector2> path;

        bool atDestination;
        int currentDestIndex;

        public MovingPlatform(Texture2D texture, Vector2 startPos, float width, float height, Vector2 speed, List<Vector2> path)
            : base(texture, startPos, width, height, speed) {
            this.path = path;

            //basevalues
            atDestination = false;
            currentDestIndex = 0;
            isMoving = true;
        }

        public void AffectOnTouch(List<MovingObjects> movingObjects) {
            tempVector = pos - lastPos; //sets tempVector to be the difference between pos and lastPos
            foreach (MovingObjects mO in movingObjects) {
                if (this.CollisionDetection(mO.IntersectRec) && this.IsMoving) {
                    //adds the platforms speed to whatever moving object touching it
                    //which makes them follow the platform and be able to travel on it.
                    mO.Pos += tempVector;
                }
            }
            lastPos = pos; //Updates lastPos
        }

        public void Movement() {

            if (atDestination) {
                atDestination = false;
                currentDestIndex++;
                if (currentDestIndex >= path.Count) { currentDestIndex = 0; }
                else if (currentDestIndex < 0) { currentDestIndex = path.Count - 1; }
            }
            else {
                atDestination = moveToPoint(path[currentDestIndex]);
            }
            
            //Update Recs
            //rec.X = (int)pos.X;
            //rec.Y = (int)pos.Y;
        }
        bool xReached, yReached;
        private bool moveToPoint(Vector2 destination) { //method that moves platform to destination
            //reset before running
            xReached = false;
            yReached = false;

            //X
            if (pos.X <= (destination.X - StandardMeasurements.StandardWidthUnit)) { //left of destination
                pos.X += speed.X;
            }
            else if (pos.X >= (destination.X + StandardMeasurements.StandardWidthUnit)) { //right of destination
                pos.X -= speed.X;
            }
            else { xReached = true; }

            //Y
            if (pos.Y <= (destination.Y - StandardMeasurements.StandardHeightUnit)) { //above destination
                pos.Y += speed.Y;
            }
            else if (pos.Y >= (destination.Y + StandardMeasurements.StandardHeightUnit)) { //below destination
                pos.Y -= speed.Y;
            }
            else { yReached = true; }

            //Update Recs
            rec.X = (int)pos.X;
            rec.Y = (int)pos.Y;

            //check if arrived
            if (xReached && yReached) { return true; }
            else { return false; }
        }

    }
}
