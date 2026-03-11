using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static System.Net.Mime.MediaTypeNames;
namespace CourseWorkPart1
{
    class Collectible
    {
        // Animation representing the collectible  
        public Animation collectibleAnim;
        // The position of the collectible 
        public Vector2 Position;
        // The state of the collectible   
        public bool Active;
        // Get the width of collectible  
        public int Width { get { return collectibleAnim.FrameWidth; } }
        // Get the height collectible           
        public int Height { get { return collectibleAnim.FrameHeight; } }
        // The speed at which the enemy moves           
        float moveSpeed;

        //variable to keep a track of time:
        private float deltaTime;



        public void Initialize(Animation animation, Vector2 position)
        {
            // Load the enemy ship texture  
            collectibleAnim = animation;
            // Set the position of the enemy  
            Position = position;
            // We initialize the enemy to be active so it will be update in the game  
            Active = true;
            // Set how fast the enemy moves  
            moveSpeed = 180f;
            //intialize variable
            deltaTime = 0;
        }

        public void Update(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // The enemy always moves to the left so decrement it's x position  
            Position.X -= moveSpeed * deltaTime;
            // Update the position of the Animation  
            collectibleAnim.Position = Position;
            // Update Animation  
            collectibleAnim.Update(gameTime);
            // If the collectible is past the screen 
            if (Position.X < -Width)
            {
                // By setting the Active flag to false, the game will remove this object from the 
                // active game list  
                Active = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the animation  
            collectibleAnim.Draw(spriteBatch);
        }




    }
}
