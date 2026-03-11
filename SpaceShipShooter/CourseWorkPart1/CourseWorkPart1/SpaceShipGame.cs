using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Reflection.Metadata.Ecma335;
using System.Resources;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.IO;


namespace CourseWorkPart1;

public class SpaceShipGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    //SpaceShip 
    SpaceShipUser spaceShip;

    // Image used to display the static background   
    //PBackground mainBackground;
    BackGround mainBackground;
    // Parallaxing Layers   
    PBackground bgLayer1;
    PBackground bgLayer2;
    PBackground bgLayer3;
    PBackground bgLayer4;

    // Enemies-1 
    Texture2D spaceMineTexture;  
    List<Enemy> spaceMines;
    //The rate at which the enemies appear  
    TimeSpan enemySpawnTime;
    TimeSpan previousSpawnTime;
    // A random number generator  
    Random random;

    //Collectibles:
    //Pink Collectible
    Texture2D collectibleTexPink;
    List<Collectible> collectibleListPink;
    TimeSpan colSpawnTime, colpreviousSpawnTime;
    //Orange Collectible
    Texture2D collectibleOrangeTex;
    List<Collectible> colOrangeList;
    TimeSpan colOrangeSpawnTime, colOrangePreviousSpawnTime;
    //Blue Collectible
    Texture2D collectibleBlueTex;
    List<Collectible> colBlueList;
    TimeSpan colBlueSpawnTime, colBluePreviousSpawnTime;







    //command manager 
    CommandManager cm;

    //Lasers
    List<LaserShooter> laserBeams;
    // texture to hold the laser.  
    Texture2D laserTexture;
    // govern how fast our laser can fire.  
    TimeSpan laserSpawnTime;
    TimeSpan previousLaserSpawnTime;

    // Collections of explosions  
    List<Explosions> explosions;
    //Texture to hold explosion animation.  
    Texture2D explosionTexture;

    //Our Laser Sound and Instance  
    private SoundEffect laserSound;
    private SoundEffectInstance laserSoundInstance;

    //Our Explosion Sound.  
    private SoundEffect explosionSound;
    private SoundEffectInstance explosionSoundInstance;

    // Game Music.  
    private Song gameMusic;

    //Score keeping
    private ScoreManager sm;
    private SpriteFont sf;

    //variable for currentGameTime to make sure the laser is fired through command manager
    private GameTime currGameTime;

    //health
    private SpriteFont health;

    //String Display Text
    private int displayScore;



    public SpaceShipGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
       
        // TODO: Add your initialization logic here

        spaceShip = new SpaceShipUser();

        //Background  
        bgLayer1 = new PBackground();
        bgLayer2 = new PBackground();
        bgLayer3 = new PBackground();
        bgLayer4 = new PBackground();
        mainBackground = new BackGround();

        
        // Initialize the enemies list
        spaceMines = new List<Enemy>();  
        // Set the time keepers to zero  
        previousSpawnTime = TimeSpan.Zero;
        // Used to determine how fast enemy respawns  
        enemySpawnTime = TimeSpan.FromSeconds(1.0f);
        // Initialize our random number generator  
        random = new Random();

        //Initializing the collectibles:
        collectibleListPink = new List<Collectible>();
        colpreviousSpawnTime = TimeSpan.Zero;
        colSpawnTime = TimeSpan.FromSeconds(1.0f);

        colOrangeList = new List<Collectible>();
         
        colBlueList = new List<Collectible>();

        //rarer
        colOrangeSpawnTime = TimeSpan.FromSeconds(15.0f);
        //rarest
        colBlueSpawnTime = TimeSpan.FromSeconds(25.0f);
        colOrangePreviousSpawnTime = TimeSpan.Zero;
        colBluePreviousSpawnTime = TimeSpan.Zero;





        cm = new CommandManager();

        InitializeBindings();

        //LaserBeams
        // init our laser
        laserBeams = new List<LaserShooter>();
        const float SECONDS_IN_MINUTE = 60f;
        const float RATE_OF_FIRE = 200f;
        laserSpawnTime = TimeSpan.FromSeconds(SECONDS_IN_MINUTE / RATE_OF_FIRE);
        previousLaserSpawnTime = TimeSpan.Zero;

        explosions = new List<Explosions>();

        //initializing our score manager
        sm = new ScoreManager();
        sm.ScoreChanged += OnScoreChanged;


        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "world", "world.txt");
        Console.WriteLine("Looking for: " + fullPath);
        Console.WriteLine("Exists: " + File.Exists(fullPath));


        base.Initialize();

    }


    protected override void UnloadContent()
    {
        laserSoundInstance.Dispose();
        explosionSoundInstance.Dispose();
        base.UnloadContent();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load the enemy animation  
        spaceMineTexture = Content.Load<Texture2D>("Enemy/mineAnimation"); 

        // Load the player resources   Bongseng. (n.d.) Parallax forest, desert, sky, moon. Available at: https://bongseng.itch.io/parallax-forest-desert-sky-moon 
        Animation playerAnimation = new Animation();
        Texture2D playerTexture = Content.Load<Texture2D>("Hero\\shipAnimation2");
        //playerAnimation.Initialize(playerTexture, Vector2.Zero, 115, 69, 8, 30, Color.Yellow, 1f, true);
        int fcount = 7; 
        int fWidth = playerTexture.Width / fcount; 
        int fheight = playerTexture.Height; 

        playerAnimation.Initialize(playerTexture, Vector2.Zero, fWidth, fheight, fcount, 30, Color.White, 0.25f, true);
        Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X,
        GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
        spaceShip.Initialize(playerAnimation, playerPosition);


        //Enjl. (n.d.) Background starry space. Available at: https://enjl.itch.io/background-starry-space (Accessed: 9 March 2025).

        bgLayer1.Initialize(Content, "Background/bg/background_1", GraphicsDevice.Viewport.Width,
        GraphicsDevice.Viewport.Height, -1);
        bgLayer2.Initialize(Content, "Background/bg/background_2", GraphicsDevice.Viewport.Width,
        GraphicsDevice.Viewport.Height, -1);
        bgLayer3.Initialize(Content, "Background/bg/background_2", GraphicsDevice.Viewport.Width,
        GraphicsDevice.Viewport.Height, -1);
        bgLayer4.Initialize(Content, "Background/bg/background_2", GraphicsDevice.Viewport.Width,
        GraphicsDevice.Viewport.Height, -1);
        mainBackground.Initialize(Content, "Background/bg/background", GraphicsDevice.Viewport.Width,
       GraphicsDevice.Viewport.Height);
        //mainBackground = Content.Load<Texture2D>("Background/bg/background");


        //loading the explosion texture: Ansimuz. (n.d.) Explosion animations pack. Available at: https://ansimuz.itch.io/explosion-animations-pack
        explosionTexture = Content.Load<Texture2D>("Explosion\\explosion1");

        // load the texture to serve as the laser
        laserTexture = Content.Load<Texture2D>("Shooter\\cropped_laser");

        //laserSound effect Instance  OpenGameArt. (n.d.) Space shooter sound effects. Available at: https://opengameart.org/content/space-shooter-sound-effects 
          
        laserSound = Content.Load<SoundEffect>("Sounds\\laser");
        laserSoundInstance = laserSound.CreateInstance();

        //explosion Sound: OpenGameArt. (n.d.) Space shooter sound effects. Available at: https://opengameart.org/content/space-shooter-sound-effects 
        explosionSound = Content.Load<SoundEffect>("Sounds\\explosion");
        explosionSoundInstance = explosionSound.CreateInstance();
        /*
         * Song: Adrift
Composer: Hayden Folker
Website: https://soundcloud.com/hayden-folker
License: Free To Use YouTube license youtube-free
Music powered by BreakingCopyright: https://breakingcopyright.com
         */
        // Load the game music  
        gameMusic = Content.Load<Song>("Sounds\\Adrift"); 
        // Start playing the music.  
        MediaPlayer.Play(gameMusic);

        //loading score fonts (Provided by instructor)
        sf = Content.Load<SpriteFont>("Fonts\\gameFont");

        health = Content.Load<SpriteFont>("Fonts\\gameFont");

        // Load the collectible animations  
        collectibleTexPink = Content.Load<Texture2D>("Collectible/gem1");
        collectibleBlueTex = Content.Load<Texture2D>("Collectible/gem3");
        collectibleOrangeTex = Content.Load<Texture2D>("Collectible/gem2");

        LoadWorldFromFile("world/world.txt");



    }


    //set up controls for the user 
    private void InitializeBindings()
    {
        cm.AddKeyboardBinding(Keys.Escape, StopGame);
        cm.AddKeyboardBinding(Keys.W, spaceShip.MoveShipUp);
        cm.AddKeyboardBinding(Keys.A, spaceShip.MoveShipLeft);
        cm.AddKeyboardBinding(Keys.S, spaceShip.MoveShipDown);
        cm.AddKeyboardBinding(Keys.D, spaceShip.MoveShipRight);
        cm.AddKeyboardBinding(Keys.Space, FireLaserCommand);
    }

    public void StopGame(eButtonState buttonState, Vector2 amount)
    {
        if (buttonState == eButtonState.DOWN)
        {
            Exit();
            //Stop playing the music  
            MediaPlayer.Stop();
        }
    }
    private void FireLaserCommand(eButtonState buttonState, Vector2 amount)
    {
        if (buttonState == eButtonState.DOWN)
        {
            Fire(currGameTime); // GameTime will be passed inside Update()
        }
    }


    private void checkPlayerOutOfBounds(GameTime gameTime)
    {

        float swidth = spaceShip.Width * 0.25f;
        float sheigh = spaceShip.Height * 0.25f;

        // Make sure that the player does not go out of bounds   
        spaceShip.Position.X = MathHelper.Clamp(spaceShip.Position.X, swidth / 2,
        GraphicsDevice.Viewport.Width - swidth / 2);

        spaceShip.Position.Y = MathHelper.Clamp(spaceShip.Position.Y, sheigh / 2,
        GraphicsDevice.Viewport.Height - sheigh / 2);
    }



    protected override void Update(GameTime gameTime)
    {
        //updating the command manager 
        cm.Update();

        // Update the collisions   
        UpdateCollision();

        // TODO: Add your update logic here
        spaceShip.Update(gameTime);

        checkPlayerOutOfBounds(gameTime);
        //player.Update(gameTime);

        // Update the enemies0
        UpdateEnemies(gameTime);

        //Update the collectibles
        UpdateCollectiblesPink(gameTime);
        UpdateCollectiblesOrange(gameTime);
        UpdateCollectiblesBlue(gameTime);


        // Update the parallaxing background    
        bgLayer1.Update(gameTime);
        bgLayer2.Update(gameTime);
        bgLayer3.Update(gameTime);
        bgLayer4.Update(gameTime);

        // Update explosions  
        UpdateExplosions(gameTime);

        // update laserbeams   
        UpdateLaserBeams(gameTime);

        //setting the cuurent game time to gametime

        currGameTime = gameTime;


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);




        // TODO: Add your drawing code here
        // Start drawing  
        _spriteBatch.Begin();
        //Draw the Main Background Texture  
        mainBackground.Draw(_spriteBatch);
        // Draw the moving background  
        bgLayer1.Draw(_spriteBatch);
        bgLayer2.Draw(_spriteBatch);
        bgLayer3.Draw(_spriteBatch);
        bgLayer4.Draw(_spriteBatch);
        // Draw the Player  
        spaceShip.Draw(_spriteBatch);
        // Draw the Enemies   
        for (int i = 0; i < spaceMines.Count; i++)
        {
            spaceMines[i].Draw(_spriteBatch);
        }
        // Draw the collectibles   
        for (int i = 0; i < collectibleListPink.Count; i++)
        {
            collectibleListPink[i].Draw(_spriteBatch);
        }
        for (int i = 0; i < colOrangeList.Count; i++)
        {
            colOrangeList[i].Draw(_spriteBatch);
        }
        for (int i = 0; i < colBlueList.Count; i++)
        {
            colBlueList[i].Draw(_spriteBatch);
        }
        // Draw the lasers.  
        foreach (var l in laserBeams)
        {
            l.Draw(_spriteBatch);
        }

        // draw explosions   
        foreach (var e in explosions)
        {
            e.Draw(_spriteBatch);
        }

        //Draw The score
        _spriteBatch.DrawString(sf, "Score: " + displayScore, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X,
GraphicsDevice.Viewport.TitleSafeArea.Y), Color.White);

        _spriteBatch.DrawString(sf, "Health: " + spaceShip.Health, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X+630,
GraphicsDevice.Viewport.TitleSafeArea.Y), Color.Red);

        // Stop drawing  
        _spriteBatch.End();
        base.Draw(gameTime);
    }



    ///Add enemy code
    private void AddEnemy()
    {
        // Create the animation object  
        Animation enemyAnimation = new Animation();
        // Initialize the animation with the correct animation information  
        enemyAnimation.Initialize(spaceMineTexture, Vector2.Zero, 47, 62, 8, 30, Color.GreenYellow, 1f, true);
        // Randomly generate the position of the enemy  
        Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + spaceMineTexture.Width / 2, random.Next(100,
        GraphicsDevice.Viewport.Height - 100));
        // Create an enemy  
        Enemy enemy = new Enemy();
        // Initialize the enemy  
        enemy.Initialize(enemyAnimation, position);
        // Add the enemy to the active enemies list 
        spaceMines.Add(enemy);
    }

    private void UpdateCollision()
    {
        // Use the Rectangle's built-in intersect function to   
        // determine if two objects are overlapping   
        Rectangle playerRectangle;
        Rectangle enemyRectangle;
        Rectangle laserRectangle;
        // Only create the rectangle once for the player  
        playerRectangle = new Rectangle((int)spaceShip.Position.X, (int)spaceShip.Position.Y, spaceShip.Width/5, spaceShip.Height/5);
        // Do the collision between the player and the enemies
        for (int i = 0; i < spaceMines.Count; i++)  
{
            enemyRectangle = new Rectangle((int)spaceMines[i].Position.X, (int)spaceMines[i].Position.Y, spaceMines[i].Width,spaceMines[i].Height);
            // Determine if the two objects collided with each  
            // other  
            if (playerRectangle.Intersects(enemyRectangle))
            {

              

                spaceShip.Health -= spaceMines[i].Damage;
                // Since the enemy collided with the player  
                // destroy it  
                spaceMines[i].Health = 0;
                // Show the explosion where the enemy was...  
                AddExplosion(spaceMines[i].Position);
                // If the player health is less than zero we died  
                if (spaceShip.Health <= 0)
                {
                    spaceShip.Active = false;
                    //Stop playing the music  
                    MediaPlayer.Stop();
                    Exit(); // game ends when the player dies
                }
            }

            // Laserbeam vs Enemy Collision  
            for (var l = 0; l < laserBeams.Count; l++)
            {
                // create a rectangle for this laserbeam  
                laserRectangle = new Rectangle((int)laserBeams[l].Position.X,
                (int)laserBeams[l].Position.Y, laserBeams[l].Width, laserBeams[l].Height);
                // test the bounds of the laser and enemy  
                if (laserRectangle.Intersects(enemyRectangle))
                {
                    // Show the explosion where the enemy was...  
                    AddExplosion(spaceMines[i].Position);
                    // kill off the enemy  
                    spaceMines[i].Health -= 10;
                    // kill off the laserbeam 
                    laserBeams[l].Active = false;
                    //add score when the enemy is killed
                    sm.AddEnemyKillPoints();
                }
            }



        }
    }


    //Update ENemies method
    private void UpdateEnemies(GameTime gameTime)
    {
        // Spawn a new enemy enemy every 1.5 seconds  
        if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
        {
            previousSpawnTime = gameTime.TotalGameTime;
            // Add an Enemy  
            AddEnemy();
        }
        // Update the Enemies  
        for (int i = spaceMines.Count - 1; i >= 0; i--)
        {
            spaceMines[i].Update(gameTime);
            if (spaceMines[i].Active == false)
            {
                spaceMines.RemoveAt(i);
            }
        }
    }


    // method to trigger the laser fire
    protected void Fire(GameTime gameTime)
    {
        // govern the rate of fire for our lasers  
        if (gameTime.TotalGameTime - previousLaserSpawnTime > laserSpawnTime)
        {
            previousLaserSpawnTime = gameTime.TotalGameTime;
            // Add the laer to our list.  
            AddLaser();
            // Play the laser sound!  
            laserSound.Play();
        }
    }   

    //method to add lasers to our collection of lasers
protected void AddLaser()
    {
        Animation laserAnimation = new Animation();
        // initlize the laser animation  
        laserAnimation.Initialize(laserTexture, spaceShip.Position, 35, 22, 1, 30, Color.White, 1f, true);
        LaserShooter laser = new LaserShooter();
        // Get the starting postion of the laser.   
        var laserPostion = spaceShip.Position;
        // Adjust the position slightly to match the muzzle of the cannon.  
        laserPostion.X += 30;
        // init the laser  
        laser.Initialize(laserAnimation, laserPostion); 
        laserBeams.Add(laser);
        /* todo: add code to create a laser. */
        // laserSoundInstance.Play();  
    }

    //Updating laser beams
    private void UpdateLaserBeams(GameTime gameTime)
    {
        // Update the Projectiles  
        for (int i = laserBeams.Count - 1; i >= 0; i--)
        {
            laserBeams[i].Update(gameTime);
            if (laserBeams[i].Active == false)
            {
                laserBeams.RemoveAt(i);
            }
        }
    }

    //Explosion Code to add explosion animation to the scene
    protected void AddExplosion(Vector2 enemyPosition)
    {
        Animation explosionAnimation = new Animation();
        explosionAnimation.Initialize(explosionTexture, enemyPosition, 31,34, 51, 30, Color.White, 1.0f, true);
        Explosions explosion = new Explosions();
        explosion.Initialize(explosionAnimation, enemyPosition);
        explosions.Add(explosion);
        /* play the explosion sound. */
        explosionSound.Play();
    }

    //Updating the explosions 
    private void UpdateExplosions(GameTime gameTime)
    {
        for (var e = explosions.Count - 1; e >= 0; e--)
        {
            explosions[e].Update(gameTime);
            if (!explosions[e].Active)
                explosions.Remove(explosions[e]);
        }
    }

    //Utilzing Input listener to trigger the score change 
    private void OnScoreChanged(int newScore)
    {
        displayScore = newScore;

    }

    private void AddCollectiblePink()
    {
        // Create the animation object  
        Animation collectibleAnimation = new Animation();
        // Initialize the animation with the correct animation information  
        collectibleAnimation.Initialize(collectibleTexPink, Vector2.Zero, 32, 32, 8, 30, Color.Pink, 1f, true);
        // Randomly generate the position of the enemy  
        Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + collectibleTexPink.Width, random.Next(100,
        GraphicsDevice.Viewport.Height - 100));
        // Create a collectible  
        Collectible collectible = new Collectible();
        // Initialize the collectible  
        collectible.Initialize(collectibleAnimation, position);
        // Add the collectible
        collectibleListPink.Add(collectible);

    }

    private void UpdateCollectiblesPink(GameTime gameTime)
    {
        // Spawn a new enemy enemy every 1.5 seconds  
        if (gameTime.TotalGameTime - colpreviousSpawnTime > colSpawnTime)
        {
            colpreviousSpawnTime = gameTime.TotalGameTime;
            // Add an Enemy  
            AddCollectiblePink();
        }
        // Update the Enemies  
        for (int i = collectibleListPink.Count - 1; i >= 0; i--)
        {
            collectibleListPink[i].Update(gameTime);
            if (collectibleListPink[i].Active == false)
            {
                collectibleListPink.RemoveAt(i);
            }
        }

    }


    private void AddCollectibleOrange()
    {
        Animation collectibleAnimation = new Animation();
        collectibleAnimation.Initialize(collectibleOrangeTex, Vector2.Zero, 32, 32, 8, 30, Color.Orange, 1f, true);
        Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + collectibleOrangeTex.Width,
                                     random.Next(100, GraphicsDevice.Viewport.Height - 100));
        Collectible collectible = new Collectible();
        collectible.Initialize(collectibleAnimation, position);
        colOrangeList.Add(collectible);  // Fixed: Now adding to correct list
    }


    private void UpdateCollectiblesOrange(GameTime gameTime)
    {
        if (gameTime.TotalGameTime - colOrangePreviousSpawnTime > colOrangeSpawnTime)
        {
            colOrangePreviousSpawnTime = gameTime.TotalGameTime;
            AddCollectibleOrange();
        }

        for (int i = colOrangeList.Count - 1; i >= 0; i--)
        {
            colOrangeList[i].Update(gameTime);
            if (!colOrangeList[i].Active)
                colOrangeList.RemoveAt(i);
        }
    }


    private void AddCollectibleBlue()
    {
        Animation collectibleAnimation = new Animation();
        collectibleAnimation.Initialize(collectibleBlueTex, Vector2.Zero, 32, 32, 8, 30, Color.Blue, 1f, true);
        Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + collectibleBlueTex.Width,
                                     random.Next(100, GraphicsDevice.Viewport.Height - 100));
        Collectible collectible = new Collectible();
        collectible.Initialize(collectibleAnimation, position);
        colBlueList.Add(collectible);  // Fixed: Now adding to correct list
    }

    private void UpdateCollectiblesBlue(GameTime gameTime)
    {
        if (gameTime.TotalGameTime - colBluePreviousSpawnTime > colBlueSpawnTime)
        {
            colBluePreviousSpawnTime = gameTime.TotalGameTime;
            AddCollectibleBlue();
        }

        for (int i = colBlueList.Count - 1; i >= 0; i--)
        {
            colBlueList[i].Update(gameTime);
            if (!colBlueList[i].Active)
                colBlueList.RemoveAt(i);
        }
    }


    //private void LoadWorldFromFile(string path)
    //{
    //    string worldData = TitleContainer.OpenStream(path);
    //    using (StreamReader reader = new StreamReader(worldData))
    //    {
    //        int row = 0;
    //        string line;
    //        while ((line = reader.ReadLine()) != null)
    //        {
    //            for (int col = 0; col < line.Length; col++)
    //            {
    //                char tile = line[col];
    //                Vector2 position = new Vector2(col * 64, row * 64); // 64 = tile size
    //                switch (tile)
    //                {
    //                    case 'P':
    //                        spaceShip.Position = position;
    //                        break;
    //                    case 'E':
    //                        AddEnemyAtPosition(position);
    //                        break;
    //                    case 'C':
    //                        AddCollectible1(position);
    //                        break;
    //                    case 'B':
    //                        AddCollectible2(position);
    //                        // Add more cases for other entities if needed
    //                        break;
    //                    case 'O':
    //                        AddCollectible3(position);
    //                        break;
    //                }
    //            }
    //            row++;
    //        }
    //    }
    //}

    private void LoadWorldFromFile(string path)
    {
        // Use TitleContainer to get a stream for the file
        using (Stream stream = TitleContainer.OpenStream(path))
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                int row = 0;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    for (int col = 0; col < line.Length; col++)
                    {
                        char tile = line[col];
                        Vector2 position = new Vector2(col * 64, row * 64);

                        // Debug output to verify reading
                        Console.WriteLine($"Read char '{tile}' at {position}");

                        switch (tile)
                        {
                            case 'P':
                                spaceShip.Position = position;
                                break;
                            case 'E':
                                AddEnemyAtPosition(position);
                                break;
                            case 'C':
                                AddCollectible1(position);
                                break;
                            case 'B':
                                AddCollectible2(position);
                                break;
                            case 'O':
                                AddCollectible3(position);
                                break;
                        }
                    }
                    row++;
                }
            }
        }
    }



    /// <summary>
    /// Functions to Load Collectibles and the enemy from a Txt file
    /// </summary>
    /// <param name="position"></param>
    private void AddEnemyAtPosition(Vector2 position)
    {
        Animation enemyAnimation = new Animation();
        enemyAnimation.Initialize(spaceMineTexture, Vector2.Zero, 47, 62, 8, 30, Color.GreenYellow, 1f, true);

        Enemy enemy = new Enemy();
        enemy.Initialize(enemyAnimation, position);
        spaceMines.Add(enemy);
    }

    private void AddCollectible1(Vector2 position)
    {
        Animation collectibleAnimation = new Animation();
        collectibleAnimation.Initialize(collectibleTexPink, Vector2.Zero, 32, 32, 8, 30, Color.Pink, 1f, true);

        Collectible collect = new Collectible();
        collect.Initialize(collectibleAnimation, position);
        collectibleListPink.Add(collect);
    }

    private void AddCollectible2(Vector2 position)
    {
        Animation collectibleAnimation = new Animation();
        collectibleAnimation.Initialize(collectibleOrangeTex, Vector2.Zero, 32, 32, 8, 30, Color.Orange, 1f, true);

        Collectible collect = new Collectible();
        collect.Initialize(collectibleAnimation, position);
        colBlueList.Add(collect);
    }

    private void AddCollectible3(Vector2 position)
    {
        Animation collectibleAnimation = new Animation();
        collectibleAnimation.Initialize(collectibleBlueTex, Vector2.Zero, 32, 32, 8, 30, Color.Blue, 1f, true);
        Collectible collect = new Collectible();
        collect.Initialize(collectibleAnimation, position);
        colOrangeList.Add(collect);
    }






}
