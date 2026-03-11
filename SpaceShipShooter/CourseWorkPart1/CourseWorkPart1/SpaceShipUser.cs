using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using CourseWorkPart1;

namespace CourseWorkPart1
{
    class SpaceShipUser
    {


        // public Texture2D PlayerTexture;  
        public Animation SpaceShipAnimation;

        // public Texture2D PlayerTexture;  
        //public Animation PlayerAnimation;
        // Animation representing the player  
        //public Texture2D PlayerTexture;
        // Position of the Player relative to the screen  
        public Vector2 Position;
        // State of the player  
        public bool Active;
        // Amount of hit points that player has  
        public int Health;
        // Get the width of the player ship  
        public float movSpeed;

        //to keep a measure of time per frame
        private float deltaTime;


        // Get the width of the player ship  
        public int Width
        { get { return SpaceShipAnimation.FrameWidth; } }
        // Get the height of the player ship  
        public int Height
        { get { return SpaceShipAnimation.FrameHeight; } }
        public void Initialize(Animation animation, Vector2 position)
        {
            SpaceShipAnimation = animation;
            // Set the starting position of the player around the middle of the screen and to the back   
            Position = position;
            // Set the player to be active   
            Active = true;
            // Set the player health   
            Health = 100;

            movSpeed = 200.0f;
            deltaTime = 0;

        }

        public void Update(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            SpaceShipAnimation.Position = Position;
            SpaceShipAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpaceShipAnimation.Draw(spriteBatch);
        }


        //Applying delta time to create frame rate independent movement
        //Move the ship left
        public void MoveShipLeft(eButtonState bs, Vector2 amt)
        {
            if (bs == eButtonState.DOWN)
            {
                //PlayerAnimation.Position.X = - movSpeed;
                Position.X -= movSpeed*deltaTime;
            }
            
        }

        //move the ship down
        public void MoveShipDown(eButtonState bs, Vector2 amt)
        {
            if (bs == eButtonState.DOWN)
            {
                //PlayerAnimation.Position.Y = movSpeed;
                Position.Y += movSpeed*deltaTime;
            }
        }

        //move the ship right
        public void MoveShipRight(eButtonState bs, Vector2 amt)
        {
            if (bs == eButtonState.DOWN)
            {
                //PlayerAnimation.Position.X = movSpeed;
                Position.X += movSpeed * deltaTime;
            }

        }

        //move the ship up
        public void MoveShipUp(eButtonState bs, Vector2 amt)
        {
            if (bs == eButtonState.DOWN)
            {
                //PlayerAnimation.Position.Y = -movSpeed;
                Position.Y -= movSpeed * deltaTime;
            }

        }



    }
}
