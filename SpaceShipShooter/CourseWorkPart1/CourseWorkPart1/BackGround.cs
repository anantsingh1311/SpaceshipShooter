
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CourseWorkPart1
{
    class BackGround
    {
        private Texture2D texture;  // The background texture
        int bgHeight;
        int bgWidth;

        public void Initialize(ContentManager content, string texturePath, int screenwidth, int screenheight)
        {
            //set heigh and width of background
            bgHeight = screenheight;
            bgWidth = screenwidth;
            // Load the background texture
            texture = content.Load<Texture2D>(texturePath);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
            {
                Rectangle screenRectangle = new Rectangle(0, 0, bgWidth, bgHeight);
                spriteBatch.Draw(texture, screenRectangle, Color.White);
            }
        }

    }
}
