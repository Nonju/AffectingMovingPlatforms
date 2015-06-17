using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;


namespace AffectingMovingPlatformTest.PlatformsFolder {

    class StaticPlatform : Platforms {

        public StaticPlatform(Texture2D texture, Vector2 pos, float width, float height, Vector2 speed) : base(texture, pos, width, height, speed) { }

    }
}
