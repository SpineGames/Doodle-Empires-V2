using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Spine_Library;
using Spine_Library.SkeletalAnimation;

namespace Doodle_Empires_V2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class DoodleEmpires : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont debugFont;
        BasicEffect effect;
        Level level;
        FPSHandler fps = new FPSHandler();
        Slider horizontalSlider, verticalSlider;
        float zoom = 1.0F;

        Vector2 winPos = new Vector2(0,-240);

        Texture2D deskTex, paperTex, cursorTex;
        AdvancedDrawFuncs.Plane back, desk;
        int winWidth, winHeight;
        Vector2 cursor;

        public DoodleEmpires()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });

            effect = new BasicEffect(GraphicsDevice);
            effect.View = Matrix.CreateOrthographicOffCenter(0, 700, 125, -125, 0, 1);
            effect.VertexColorEnabled = true;
            
            horizontalSlider = new Slider(0F, spriteBatch, new Rectangle(20, 5, 760, 10), blank, blank);
            verticalSlider = new Slider(0F, spriteBatch, new Rectangle(780, 15, 10, 450), blank, blank, 1);

            cursor = new Vector2(400, 240);

            level = new Level(32768, blank);
            level.levelArea(0, 500);
            level.levelArea(level.getWidth() - 500, level.getWidth());

            level.addTeam(Color.Black, new Vector3(100, 100, 100));

            AnimatedSprite human = new AnimatedSprite(spriteBatch, Content.Load<Texture2D>("Units/TestUnit"), 64, 96, 12, 12, 2);

            level.addUnitType(human, new Vector3(0, 0, 0));
            level.spawnUnit(0, 0, 13);
            level.spawnUnit(0, 0, 26);
            level.spawnUnit(0, 0, 32);
            level.spawnUnit(0, 0, 43);
            level.spawnUnit(0, 0, 54);
            level.spawnUnit(0, 0, 62);
            level.spawnUnit(0, 0, 73);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Texture2D blank = Content.Load<Texture2D>(@"HUD/blank");
            Texture2D tree = Content.Load<Texture2D>(@"Terrain/Tree");

            paperTex = Content.Load<Texture2D>(@"Terrain/Paper");
            cursorTex = Content.Load<Texture2D>(@"HUD/cursor");
            deskTex = Content.Load<Texture2D>(@"HUD/desk");
            back = new AdvancedDrawFuncs.Plane(0, -level.getHeight(), level.getWidth(), level.getHeight(), paperTex, Color.FromNonPremultiplied(255, 255, 255, 255));
            desk = new AdvancedDrawFuncs.Plane(0, -level.getHeight(), level.getWidth(), level.getHeight(), deskTex, Color.FromNonPremultiplied(255,255,255,255));

            level.genVegetation(tree);
            
            debugFont = Content.Load<SpriteFont>("Fonts/debugFont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GamePadState cState = GamePad.GetState(PlayerIndex.One);
            // Allows the game to exit
            if (cState.Buttons.Back == ButtonState.Pressed)
                this.Exit();

            winWidth = GraphicsDevice.Viewport.Width;
            winHeight = GraphicsDevice.Viewport.Height;

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                winPos.Y += 4F;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                winPos.Y -= 4F;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                winPos.X += 4F;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                winPos.X -= 4F;

# if XBOX
            if (cState.ThumbSticks.Left.X > 0.1F)
                winPos.X += cState.ThumbSticks.Left.X * 4F;
            if (cState.ThumbSticks.Left.X < -0.1F)
                winPos.X -= cState.ThumbSticks.Left.X * 4F;

            if (cState.ThumbSticks.Right.X > 0.1F)
                cursor.X += cState.ThumbSticks.Left.X * 4F;
            if (cState.ThumbSticks.Right.X < -0.1F)
                cursor.X -= cState.ThumbSticks.Left.X * 4F;
            if (cState.ThumbSticks.Right.Y > 0.1F)
                cursor.Y += cState.ThumbSticks.Left.X * 4F;
            if (cState.ThumbSticks.Right.Y < -0.1F)
                cursor.Y -= cState.ThumbSticks.Left.X * 4F;
#endif

            level.tick(gameTime, winPos);

            winPos.X = MathHelper.Clamp(winPos.X, 1, level.getWidth() - winWidth);
            winPos.Y = MathHelper.Clamp(winPos.Y, -level.getHeight(), level.getHeight() - winHeight);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            fps.onDraw(gameTime);
            effect.View = Matrix.CreateOrthographicOffCenter(0 + winPos.X, (int)(winWidth * zoom) + winPos.X,
                (int)(winHeight * zoom) + winPos.Y, winPos.Y, 0, 1);

            GraphicsDevice.Clear(Color.Red);
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            
            desk.render(effect, winPos);
            back.render(effect, winPos);
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            level.render(effect, spriteBatch, winPos, gameTime);
            
            spriteBatch.Begin();
            spriteBatch.DrawString(debugFont, "FPS: " + fps.getFrameRate(), new Vector2(5, 5), Color.White);

            horizontalSlider.setValue(winPos.X / (level.getWidth() - winWidth));
            horizontalSlider.update(true);
            verticalSlider.setValue((float)extraMath.map(-level.getHeight(), level.getHeight(), 0.0, 1.0, (double)winPos.Y));
            verticalSlider.update(true);

            winPos.X = horizontalSlider.getValue() * (level.getWidth() - winWidth);
            winPos.Y = (float)extraMath.map(0.0, 1.0, -level.getHeight(), level.getHeight(), verticalSlider.getValue());
#if XBOX
            spriteBatch.Draw(cursorTex, cursor, null, Color.White, 0F, Vector2.Zero, 0.5F, SpriteEffects.None, 0F);
#else
            spriteBatch.Draw(cursorTex, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), null, Color.White, 0F, Vector2.Zero, 
                0.5F, SpriteEffects.None, 0F);
#endif

            spriteBatch.End();
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            base.Draw(gameTime);
        }

        /// <summary>
        /// Handles the level 
        /// </summary>
        class Level
        {
            float[] heightmap;
            float f = -(float)Math.PI;
            int levelHeight = 4000;

            Random rand = new Random();
            SimpleParticleSystem partSystem;
            BasicEffect effect;
            Vector2 winPos;

            List<Vegetation> vegetation = new List<Vegetation>();
            List<Team> teams = new List<Team>();
            List<Bullet> bullets = new List<Bullet>();
            VertexPositionColor[] landVerts;
            VertexPositionColor[] skyVerts;

            bool mousePrevDown;
            Vector2 pos1, pos2;

            /// <summary>
            /// Builds a new Level
            /// </summary>
            /// <param name="levelWidth">The width of the level, must be a power of 2</param>
            public Level(int levelWidth, Texture2D blank)
            {
                heightmap = extraMath.MidpointDisplacement(levelWidth / 64, 0, levelWidth);
                partSystem = new SimpleParticleSystem(blank, 0);
                rebuildGeometry();

                //skyVerts = new VertexPositionColor[heightmap.Length * 4];

                //Color brn = Color.FromNonPremultiplied(165, 42, 42, 128);
                //Color blk = Color.FromNonPremultiplied(0, 0, 0, 128);
                //Color lb = Color.FromNonPremultiplied(173, 216, 255, 128);

                //for (int xx = 0; xx < heightmap.Length; xx +=4)
                //{
                //    skyVerts[xx] = new VertexPositionColor(new Vector3(xx / 4, -levelHeight, 0), blk);
                //    skyVerts[xx + 1] = new VertexPositionColor(new Vector3(xx / 4, 0, 0), lb);

                //    skyVerts[xx + 2] = new VertexPositionColor(new Vector3(xx / 4, 0, 0), lb);
                //    skyVerts[xx + 3] = new VertexPositionColor(new Vector3(xx / 4, levelHeight, 0), brn);
                //}
            }

            /// <summary>
            /// Recontructs the world's geometry between the 2 points
            /// </summary>
            /// <param name="x1">The min x co-ord</param>
            /// <param name="x2">The max x co-ord</param>
            private void rebuildGeometry(int x1 = 0, int x2 = -1)
            {
                if (x2 == -1)
                {
                    landVerts = new VertexPositionColor[heightmap.Length];
                    x2 = heightmap.Length;
                }

                //clamp the range to values that fit in the heightmap
                x1 = (int)MathHelper.Clamp(x1, 0, getWidth());
                x2 = (int)MathHelper.Clamp(x2, 0, getWidth());

                for (int xx = x1; xx < x2; xx ++)
                {
                    //landVerts[xx] = new VertexPositionColor(new Vector3(xx / 4, -heightmap[xx / 4], 0), Color.FromNonPremultiplied(128,128,0, 200));
                    //landVerts[xx + 1] = new VertexPositionColor(new Vector3(xx / 4, -heightmap[xx / 4] + 100, 0), Color.FromNonPremultiplied(210, 180, 140, 200));

                    //landVerts[xx + 2] = new VertexPositionColor(new Vector3(xx / 4, -heightmap[xx / 4] + 100, 0), Color.FromNonPremultiplied(210, 180, 140, 200));
                    landVerts[xx] = new VertexPositionColor(new Vector3(xx, levelHeight, 0), Color.FromNonPremultiplied(0, 0, 0, 200));
                }
            }

            /// <summary>
            /// Renders this level
            /// </summary>
            /// <param name="effect">The BasicEffect to draw with</param>
            /// <param name="winPos">The position of the window</param>
            public void render(BasicEffect effect, SpriteBatch spriteBatch, Vector2 winPos, GameTime gameTime)
            {
                if (effect != null & winPos != null)
                {
                    int winWidth = effect.GraphicsDevice.Viewport.Width;
                    int winheight = effect.GraphicsDevice.Viewport.Height;

                    //tick the water offset value
                    f += 0.5F;

                    effect.CurrentTechnique.Passes[0].Apply();
                    //effect.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, skyVerts, (int)(winPos.X) * 4, 1600);

                    //loop through the terrain points on screen
                    //for (int i = (int)winPos.X; i < (int)winPos.X + winWidth; i++)
                    //{
                    //    //draw's the water
                    //    if (-10 > heightmap[i])
                    //        AdvancedDrawFuncs.DrawLine(effect, new Vector2(i, 10 + (float)Math.Sin((f + i) / 10)),
                    //            new Vector2(i, (-heightmap[i])), new Color(173, 216, 230, 200),
                    //            new Color(0, 0, 255, 200));
                    //}

                    effect.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, landVerts, (int)(winPos.X), 1600);

                    //Draws all the vegetation
                    foreach (Vegetation veg in vegetation)
                    {
                        veg.render(effect, winPos);
                    }

                    foreach (Bullet b in bullets)
                        b.render(effect);

                    spriteBatch.Begin();

                    foreach (Team team in teams)
                    {
                        team.render(effect, gameTime, new Point((int)winPos.X, (int)winPos.Y));
                    }

                    partSystem.tick(spriteBatch,winPos, this);
                    spriteBatch.End();
                }
            }

            /// <summary>
            /// Loads the BasicEffect and winpos into the level
            /// </summary>
            /// <param name="effect">The basicEffect to draw with</param>
            /// <param name="winPos">The position of the window</param>
            public void preRender(BasicEffect effect, Vector2 winPos)
            {
                this.effect = effect;
                this.winPos = winPos;
            }

            public void tick(GameTime gameTime, Vector2 winPos)
            {
                foreach (Team team in teams)
                    team.tick(this, gameTime);

                Bullet[] bs = bullets.ToArray();
                for (int i = 0; i< bs.Length; i++)
                    bs[i].tick(this);

                MouseState mouse = Mouse.GetState();
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    if (!mousePrevDown)
                        pos1 = new Vector2(mouse.X, mouse.Y);
                    mousePrevDown = true;
                }
                else
                {
                    if (mousePrevDown)
                    {
                        pos2 = new Vector2(mouse.X, mouse.Y);
                        bullets.Add(new Bullet(pos1 + winPos, Color.Yellow, 0, Vector2.Distance(pos1, pos2), extraMath.findAngle(pos1, pos2)));
                    }
                    mousePrevDown = false;
                }
            }

            /// <summary>
            /// Flattens the land between x and x2
            /// </summary>
            /// <param name="x">The leftmost x co-ord</param>
            /// <param name="x2">The rightmost co-ord</param>
            /// <param name="smooth">True is smoothing is to be used</param>
            public void levelArea(int x, int x2, bool smooth = true)
            {
                //clamp the range to values that fit in the heightmap
                x = (int)MathHelper.Clamp(x, 0, getWidth());
                x2 = (int)MathHelper.Clamp(x2, 0, getWidth());

                //gets the value to smooth to
                float val = heightmap[x];
                //loop through x co-ords
                for (int xx = x; xx < x2; xx++)
                {
                    //assign the value
                    heightmap[xx] = val;
                }
                //apply smoothing
                if (smooth)
                    this.smoothArea(x - 50, x2 + 50, 20);

                rebuildGeometry(x - 50, x2 + 50);
            }

            /// <summary>
            /// Smooths the area between x and x2
            /// </summary>
            /// <param name="x">The minimum x value</param>
            /// <param name="x2">the maximum x value</param>
            /// <param name="smoothFactor">The number of passes to make</param>
            public void smoothArea(int x, int x2, float smoothFactor = 1)
            {
                //limit min and max to heightmap's length
                x = (int)MathHelper.Clamp(x, 0, getWidth());
                x2 = (int)MathHelper.Clamp(x2, 0, getWidth());

                //loop through smooth factor
                for (int i = 0; i < smoothFactor; i++)
                {
                    //loop through area
                    for (int xx = x + 1; xx < x2 - 2; xx++)
                    {
                        //find the average of the left & right point and assign it
                        //to the middle point
                        float midVal = (heightmap[xx - 1] + heightmap[xx + 1]) / 2F;
                        heightmap[xx] = midVal;
                    }
                }
            }

            /// <summary>
            /// Returns the length of the level's heightmap
            /// </summary>
            /// <returns>heightmap.Length</returns>
            public int getWidth()
            {
                return heightmap.Length - 1;
            }

            /// <summary>
            /// Returns the height of the level
            /// </summary>
            /// <returns>height</returns>
            public int getHeight()
            {
                return levelHeight;
            }

            /// <summary>
            /// Gets the y at the specified position
            /// </summary>
            /// <param name="x">The x co-ordinate</param>
            /// <returns>The height as lerped between the min and max of x</returns>
            public float getY(float x)
            {
                //get min and max x values
                int x1 = (int)x, x2 = (int)x + 1;

                //linearly interpolate between the two values based off of the distance from x2
                return -(MathHelper.Lerp(heightmap[x2], heightmap[x1], (float)x2 - x));
            }

            /// <summary>
            /// Adds the vegetation to this level
            /// </summary>
            /// <param name="x">The x co-ordinate of the vegitation</param>
            /// <param name="tex">The texture to draw</param>
            /// <param name="rescources">The amount of recources dropped</param>
            public void addVegetation(float x, Texture2D tex = null, int rescources = 0)
            {
                vegetation.Add(new Vegetation(this, x, tex, rescources));
            }

            /// <summary>
            /// Generates vegetation along the map
            /// </summary>
            /// <param name="tex">The texture of the vegetation</param>
            /// <param name="rescources">The amount of recources dropped per vegetation</param>
            /// <param name="xStart">The leftmost x co-ord to start from</param>
            /// <param name="xEnd">The rightmost x co-ord to end at</param>
            /// <param name="sparsity">The sparsity, with 1 being one tree every 10 pix</param>
            public void genVegetation(Texture2D tex, int rescources = 0, int xStart = 0, int xEnd = -1, int sparsity = 10)
            {
                //set the end to be the length of the level if one is not specified
                if (xEnd == -1)
                    xEnd = getWidth();

                //loop through and add the veggies
                for (int xx = xStart; xx < xEnd; xx += rand.Next(sparsity * 10))
                {
                    if (getY(xx) - 4 < 10)
                        addVegetation(xx, tex, rescources);
                }
            }

            /// <summary>
            /// Adds a new team with the specified color
            /// </summary>
            /// <param name="teamColor">The team's color</param>
            /// <param name="startRec">The starting recources (M,W,F)</param>
            /// <returns></returns>
            public int addTeam(Color teamColor, Vector3 startRec)
            {
                teams.Add(new Team(teamColor, startRec, (byte)teams.Count));
                return teams.Count - 1;
            }

            /// <summary>
            /// Adds the new unit type to all teams
            /// </summary>
            /// <param name="skeleton">The unit's skeleton</param>
            /// <param name="cost">The cost (M,W,F)</param>
            public void addUnitType(AnimatedSprite sprite, Vector3 cost)
            {
                foreach (Team team in teams)
                    team.addUnitType(sprite, cost);
            }

            public void explodeLand(int x, int size = 1)
            {
                for (int xx = x - size; xx < x + size; xx++)
                {
                    heightmap[xx] -= size;
                }

                smoothArea(x - size - 10, x + size + 10, 10);
                rebuildGeometry(x - size - 11, x + size + 11);

                if (size > 1)
                {
                    Vegetation[] temp = vegetation.ToArray();
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].x > x - size - 10 & temp[i].x < x + size + 10)
                        {
                            vegetation.Remove(temp[i]);
                            particleBurst(new Vector2(temp[i].x, temp[i].y - 12), Color.Green);
                        }
                    }
                }
            }

            public void spawnUnit(byte teamId, int unitType, float x)
            {
                teams[teamId].addUnit(unitType, x);
            }

            private void disposeBullet(Bullet bullet)
            {
                bullets.Remove(bullet);
            }

            public void particleBurst(Vector2 position, Color color)
            {
                partSystem.burst(position, color, Color.Transparent, new Vector2(1,-3F), 100, 3F, 3F, 100);
            }

            /// <summary>
            /// Handles vegetation
            /// </summary>
            class Vegetation
            {
                public float x;
                public float y;
                public int resCount;
                Texture2D tex;

                /// <summary>
                /// Creates a new vegetation
                /// </summary>
                /// <param name="level">The level to map to</param>
                /// <param name="x">The x co-ordinate</param>
                /// <param name="tex">The texture to use</param>
                /// <param name="resourceCount">The amount of recources dropped</param>
                public Vegetation(Level level, float x, Texture2D tex = null, int resourceCount = 0)
                {
                    this.x = x;
                    this.y = level.getY(x);
                    if (tex != null)
                        this.tex = tex;
                    this.resCount = resourceCount;
                }

                /// <summary>
                /// Renders this Vegetation
                /// </summary>
                /// <param name="effect">The basicEffect to draw with</param>
                /// <param name="winPos">The position of the window</param>
                public void render(BasicEffect effect, Vector2 winPos)
                {
                    if (tex != null)
                        AdvancedDrawFuncs.drawTexture(effect, x - 16, y - 48, x + 16, y, tex, Color.Green, winPos);
                }
            }

            class Team
            {
                int minerals = 0, wood = 0, food = 0;
                byte teamIndex;
                Color teamColor;
                Random rand = new Random();
                List<Unit> units = new List<Unit>();
                List<Unit> unitTypes = new List<Unit>();
                List<Vector3> unitCosts = new List<Vector3>();

                public Team(Color teamColor, Vector3 startRec, byte teamIndex = 0)
                {
                    this.teamColor = teamColor;
                    this.teamIndex = teamIndex;
                    add(startRec);
                }

                public void tick(Level level, GameTime gameTime)
                {
                    foreach (Unit u in units)
                    {
                        u.tick(level);
                    }
                }

                public void render(BasicEffect effect, GameTime gameTime, Point winPos)
                {
                    foreach (Unit u in units)
                    {
                        u.render(effect, gameTime, winPos);
                    }
                }

                public void addUnit(int unitIndex, float x)
                {
                    units.Add(new Unit(teamIndex, unitTypes[unitIndex].getSprite().getCopy(), x));
                    buy(unitCosts.ElementAt(unitIndex));
                }

                public int addUnitType(AnimatedSprite sprite, Vector3 unitCost)
                {
                    unitTypes.Add(new Unit(teamIndex, sprite));
                    unitCosts.Add(unitCost);

                    return unitTypes.Count - 1;
                }

                /// <summary>
                /// Withdraws from the team's funds
                /// </summary>
                /// <param name="cost">The cost (M,W,F)</param>
                public void buy(Vector3 cost)
                {
                    minerals -= (int)cost.X;
                    wood -= (int)cost.Y;
                    food -= (int)cost.Z;
                }

                public void add(Vector3 amount)
                {
                    minerals += (int)amount.X;
                    wood += (int)amount.Y;
                    food += (int)amount.Z;
                }
            }

            class Unit
            {
                float x, y;
                byte teamIndex;
                byte stateID = 1;
                AnimatedSprite sprite;
                Random rand;

                public Unit(byte teamIndex = 0, AnimatedSprite sprite = null, float x = 0F)
                {
                    this.teamIndex = teamIndex;
                    this.sprite = sprite;
                    this.x = x;
                    rand = new Random((int)(x * teamIndex / x + x));
                }

                public void setX(float x) { this.x = x; }

                public void tick(Level level)
                {
                    x += (float)(rand.NextDouble() + rand.NextDouble());
                    this.y = level.getY(x);
                }

                public void render(BasicEffect effect, GameTime gameTime, Point winPos)
                {
                    sprite.tickFrames(new Vector2(x + winPos.X, y - winPos.Y), stateID, 0.5F);
                }

                public AnimatedSprite getSprite()
                {
                    return sprite;
                }
            }

            class Bullet
            {
                Vector2 pos;
                int teamID;
                double speed, angle;
                VertexPositionColor[] line = new VertexPositionColor[2];

                public Bullet(Vector2 pos, Color color, int teamID = 0, double speed = 1, double angle = 0)
                {
                    this.pos = pos;
                    this.teamID = teamID;
                    this.speed = speed;
                    this.angle = angle;

                    line[0] = new VertexPositionColor(new Vector3(0,0,0), color);
                    line[1] = new VertexPositionColor(new Vector3((float)speed,0,0), color);
                }

                public void tick(Level level)
                {
                    pos += extraMath.calculateVectorOffset(angle, speed);

                    if (pos.X <= 0 || pos.X >= level.getWidth() || pos.Y < -level.levelHeight || pos.Y > level.levelHeight)
                    {
                        level.disposeBullet(this);
                        return;
                    }

                    if (pos.Y > level.getY(pos.X))
                    {
                        level.disposeBullet(this);
                        level.particleBurst(new Vector2(pos.X, level.getY(pos.X) - 1F), Color.Brown);
                        level.explodeLand((int)pos.X, (int)(speed / 10));
                    }
                }

                public void render(BasicEffect effect)
                {
                    Matrix world = effect.World;
                    Matrix transform = Matrix.CreateRotationZ((float)angle) * Matrix.CreateTranslation(new Vector3(pos, 0));
                    effect.World = transform;
                    effect.CurrentTechnique.Passes[0].Apply();
                    effect.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, line, 0, 1);
                    effect.World = world;
                    //AdvancedDrawFuncs.DrawLine(effect, pos, extraMath.calculateVector(pos, angle, speed), Color.Red, Color.Red);
                }
            }

            public class SimpleParticleSystem
            {
                Texture2D texture;
                Texture2D[] goreTexs;
                particle[] particles;
                List<particle> particleList = new List<particle>();
                List<Gore> goreList = new List<Gore>();
                Emitter[] emitters = new Emitter[10];
                List<RectEmitter> rectEmitters = new List<RectEmitter>();
                public Stream[] streams = new Stream[10];
                public int partCount = 0;
                public byte index = 0;
                Random rand = new Random();

                public SimpleParticleSystem(Texture2D tex, byte index = 0, int maxSize = 1000)
                {
                    this.index = index;
                    this.texture = tex;
                    particles = new particle[maxSize];
                }

                public SimpleParticleSystem(Texture2D tex, byte index = 0)
                {
                    this.index = index;
                    this.texture = tex;
                    particles = new particle[10000];
                }

                public SimpleParticleSystem(Texture2D tex, Texture2D[] goreTexs, byte index = 0)
                {
                    this.index = index;
                    this.texture = tex;
                    particles = new particle[10000];
                    this.goreTexs = goreTexs;
                }

                /// <summary>
                /// Spawns a particle at pos with the specified paramenters
                /// </summary>
                /// <param name="pos">The position of the particle</param>
                /// <param name="size">The size of the particle</param>
                /// <param name="lifeSpan">The number of ticks until the partle gets disposed</param>
                /// <param name="hspeed">The horizontal speed</param>
                /// <param name="vspeed">The vertical speed</param>
                /// <param name="color">The initial color</param>
                /// <param name="endColor">The final color before fading out</param>
                public void addEffect(Vector2 pos, float size, int lifeSpan, float hspeed, float vspeed, Color color, Color endColor)
                {
                    hspeed *= (float)(rand.NextDouble() * 2) - 1F;
                    vspeed *= (float)(rand.NextDouble() * 2) - 1F;
                    particleList.Add(new particle(this, pos, color, endColor, size, lifeSpan + rand.Next(50), new Vector2(hspeed, vspeed)));
                }

                public bool addEffect(Vector2 pos, float size, int lifeSpan, Vector2 speed, Color color, Color endColor)
                {
                    speed.X *= (float)(rand.NextDouble() * 2) - 1F;
                    speed.Y *= (float)(rand.NextDouble() * 2) - 1F;
                    particleList.Add(new particle(this, pos, color, endColor, size, lifeSpan + rand.Next(50), speed));
                    return true;
                }

                public bool addGore(Vector2 pos, float size, int lifeSpan, int texID, Vector2 speed, Color color, Color endColor)
                {
                    speed.X *= (float)(rand.NextDouble() * 2) - 1F;
                    speed.Y *= (float)(rand.NextDouble() * 2) - 1F;
                    goreList.Add(new Gore(this, pos, color, endColor, size, lifeSpan + rand.Next(50), speed, texID));
                    return true;
                }

                private void destroyParticle(particle part)
                {
                    particleList.Remove(part);
                }

                private void destroyGore(Gore part)
                {
                    goreList.Remove(part);
                }

                public void changeStreamDirection(int streamId, float newAngle)
                {
                    try
                    {
                        streams[streamId].direction = newAngle;
                    }
                    catch (IndexOutOfRangeException)
                    {
                    }
                }

                public void changeStreamRate(int streamId, int newRate)
                {
                    streams[streamId].frameRate = newRate;
                }

                public void changeStreamCount(int streamId, int newCount)
                {
                    streams[streamId].partCount = newCount;
                }

                public void changeStreamVector(int streamId, Vector2 newVector)
                {
                    streams[streamId].pos = newVector;
                }

                public void changeEmitterVector(int streamId, Vector2 newVector)
                {
                    emitters[streamId].pos = newVector;
                }

                public void bounceFromRect(Rectangle rect)
                {
                    foreach (particle part in particleList)
                    {
                        if ((part.pos.Y + 2 > rect.Y) & (part.pos.Y + 2 < rect.Y + rect.Height) & (part.pos.X > rect.X) & (part.pos.X < rect.X + rect.Width))
                        {
                            part.pos.X -= (float)part.hSpeed;
                            part.pos.Y -= (float)part.vSpeed;
                            part.vSpeed = -(part.vSpeed * (0.25 + rand.NextDouble() / 5));
                            part.hSpeed /= (1.2F + rand.NextDouble() / 10);
                            part.life += 0.5F;
                            if (rand.Next(10) == 2)
                                part.hSpeed = -part.hSpeed;
                        }
                    }

                    foreach (Gore part in goreList)
                    {
                        if (new Rectangle((int)part.pos.X, (int)part.pos.Y, (int)(10 * 0.5F), (int)(10 * 0.5F)).Intersects(rect))
                        {
                            part.pos.X -= (float)part.hSpeed;
                            part.pos.Y -= (float)part.vSpeed;
                            part.vSpeed = -(part.vSpeed * (0.25 + rand.NextDouble() / 5));
                            part.hSpeed /= (1.4F + rand.NextDouble() / 10);
                            part.life += 0.5F;
                            part.rotSpeed /= 2;
                            if (rand.Next(10) == 2)
                                part.hSpeed = -part.hSpeed;
                        }
                    }
                }

                public void tick(SpriteBatch batch, Vector2 winPos, Level level)
                {
                    foreach (Stream stream in streams)
                    {
                        if (stream != null)
                        {
                            stream.tick();
                        }
                    }

                    particle[] temp = particleList.ToArray();
                    foreach (particle part in temp)
                    {
                        if (part != null)
                        {
                            part.tick(level);
                            part.draw(batch, texture, winPos);
                        }
                    }

                    Gore[] temp2 = goreList.ToArray();
                    foreach (Gore part in temp2)
                    {
                        if (part != null)
                        {
                            part.tick();
                            part.draw(batch, winPos);
                        }
                    }


                    foreach (RectEmitter r in rectEmitters)
                    {
                        r.timer++;
                        if (r.timer >= r.timing)
                        {
                            r.burst();
                            r.timing = 0;
                        }
                    }
                }

                public int addEmitter(Vector2 pos)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        if (emitters[i] == null)
                        {
                            emitters[i] = new Emitter(pos, this);
                            return i;
                        }
                    }
                    return -1;
                }

                public void removeEmitter(int id)
                {
                    try
                    {
                        emitters[id] = null;
                    }
                    catch (IndexOutOfRangeException)
                    {
                    }
                }

                public int addRectEmitter(Rectangle rect, int spacing, int timing, int lifeSpan, float size, Vector2 speed, Color start, Color end)
                {
                    rectEmitters.Add(new RectEmitter(rect, this, spacing, timing, lifeSpan, size, speed, start, end));
                    return rectEmitters.Count - 1;
                }

                public void removeRectEmitter(int id)
                {
                    try
                    {
                        rectEmitters[id] = null;
                    }
                    catch (IndexOutOfRangeException)
                    {
                    }
                }

                public int addStream(Vector2 pos, int partCount, float size, int lifespan, Vector2 speedScales, Color color, Color endColor, double direction, int frameRate)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        if (streams[i] == null)
                        {
                            streams[i] = new Stream(pos, this, partCount, size, lifespan, speedScales, color, endColor, direction, frameRate);
                            return i;
                        }
                    }
                    return -1;
                }

                public void burst(int index, int partCount, float radius, float size, int lifespan, float speed, Color color, Color endColor)
                {
                    if (index >= 0 & index <= 9)
                    {
                        emitters[index].burst(partCount, radius, size, lifespan, new Vector2(speed, speed), color, endColor);
                    }
                }

                public void burst(Vector2 pos, Color color, Color endColor, Vector2 speed, int partCount = 1, float radius = 1F, float size = 1F, int lifespan = 100)
                {
                    int c = 0;

                    while (c < partCount)
                    {
                        Vector2 testPos = new Vector2((pos.X - radius / 2) + (float)(rand.NextDouble() * (int)radius), (pos.Y - radius / 2) + (float)(rand.NextDouble() * (int)radius));
                        if (Math.Pow((testPos.X - pos.X), 2) + Math.Pow((testPos.Y - pos.Y), 2) < Math.Pow(radius, 2))
                        {
                            addEffect
                                (testPos,
                                size, lifespan,
                                (float)(rand.NextDouble() * speed.X),
                                (float)(rand.NextDouble() * speed.Y),
                                color, endColor);

                            c++;
                        }
                    }
                }

                //creates an rectangular emmiter that bursts out particles
                private class RectEmitter
                {
                    public Rectangle rect;
                    public int timing, timer = 0, lifeSpan;
                    public Color startColor, endColor;
                    public float partSize;
                    public Vector2 speeds;
                    int spacing = 1;
                    SimpleParticleSystem parent;

                    public RectEmitter(Rectangle rect, SimpleParticleSystem parent, int spacing, int timing, int lifeSpan, float partSize,
                        Vector2 speeds, Color startColor, Color endColor)
                    {
                        this.timing = timing;
                        this.lifeSpan = lifeSpan;
                        this.partSize = partSize;
                        this.speeds = speeds;
                        this.startColor = startColor;
                        this.endColor = endColor;
                        this.rect = rect;
                        this.parent = parent;
                        this.spacing = spacing;
                    }

                    public void burst()
                    {
                        for (int x = rect.X; x < rect.X + rect.Width; x += spacing)
                        {
                            for (int y = rect.Y; y < rect.Y + rect.Height; y += spacing)
                            {
                                parent.addEffect(new Vector2(x, y), partSize, lifeSpan, speeds, startColor, endColor);
                            }
                        }
                    }
                }

                //creates an emmiter that bursts out particles
                private class Emitter
                {
                    public Vector2 pos;
                    SimpleParticleSystem parent;

                    public Emitter(Vector2 position, SimpleParticleSystem parent)
                    {
                        pos = position;
                        this.parent = parent;
                    }

                    public void burst(int partCount, float radius, float size, int lifespan, Vector2 speedScales, Color color, Color endColor)
                    {
                        int c = 0;
                        Random rand = new Random();
                        while (c < partCount)
                        {
                            Vector2 testPos = new Vector2((pos.X - radius / 2) + (float)(rand.NextDouble() * (int)radius), (pos.Y - radius / 2) + (float)(rand.NextDouble() * (int)radius));
                            if (Math.Pow((testPos.X - pos.X), 2) + Math.Pow((testPos.Y - pos.Y), 2) < Math.Pow(radius, 2))
                            {
                                parent.addEffect
                                    (testPos,
                                    size, lifespan,
                                    (float)(rand.NextDouble() * (speedScales.X + (pos.X - testPos.X) / radius)) - speedScales.X / 2F,
                                    (float)(rand.NextDouble() * (speedScales.Y + (pos.Y - testPos.Y) / radius)) - speedScales.X / 2F,
                                    color, endColor);

                                c++;
                            }
                        }
                    }
                }

                //creates a stream of particles from a single point
                public class Stream
                {
                    public Vector2 pos;
                    Vector2 speedScales;
                    SimpleParticleSystem parent;
                    public int partCount;
                    public int lifespan;
                    public double direction;
                    public float size;
                    public Color color, endColor;
                    int counter = 0;
                    public int frameRate;

                    public Stream(Vector2 position, SimpleParticleSystem parent, int partCount, float size, int lifespan, Vector2 speedScales, Color color, Color endColor, double direction, int frameRate)
                    {
                        pos = position;
                        this.parent = parent;
                        this.partCount = partCount;
                        this.lifespan = lifespan;
                        this.size = size;
                        this.speedScales = speedScales;
                        this.color = color;
                        this.endColor = endColor;
                        this.direction = direction;
                        this.frameRate = frameRate;
                    }

                    public void tick()
                    {
                        if (frameRate != 501)
                        {
                            counter++;
                            if (counter >= frameRate)
                            {
                                Random rand = new Random();
                                for (int i = 0; i < partCount; i++)
                                {
                                    parent.addEffect
                                        (new Vector2((float)(pos.X - rand.NextDouble() * 10F), (float)(pos.Y - rand.NextDouble() * 10F)),
                                        size, lifespan,
                                        (float)(speedScales.X * rand.NextDouble()) - (float)((speedScales.X * rand.NextDouble()) * 2),
                                        (float)(speedScales.Y * rand.NextDouble()) - (float)((speedScales.Y * rand.NextDouble()) * 2),
                                        color, endColor);
                                }
                                counter = 0;
                            }
                        }
                    }
                }

                /// <summary>
                /// A particle as part of a SimpleParticleSystem
                /// </summary>
                private class particle
                {
                    public Vector2 pos;
                    SimpleParticleSystem parent;
                    float rChange = 0, gChange = 0, bChange = 0;
                    float rColor, gColor, bColor;
                    double alphaChange;
                    double alpha = 255;
                    public double hSpeed = 0, vSpeed = 0, vAcc = 0.1F;
                    public float life;
                    public Rectangle rect;

                    /// <summary>
                    /// Creates a new particle
                    /// </summary>
                    /// <param name="parent">The SimpleParticleSystem that this particle belongs to</param>
                    /// <param name="pos">The position of this particle</param>
                    /// <param name="startColor">The initial color of the particle</param>
                    /// <param name="endColor">The final color of the particle</param>
                    /// <param name="startSize">The size of the particle</param>
                    /// <param name="lifeSpan">How many ticks the particle lasts</param>
                    /// <param name="startSpeed">The initial horizontal/vertical speeds</param>
                    public particle(SimpleParticleSystem parent, Vector2 pos, Color startColor, Color endColor, float startSize, int lifeSpan, Vector2 startSpeed)
                    {
                        this.parent = parent;
                        this.pos = pos;
                        this.rect = new Rectangle(0, 0, (int)startSize, (int)startSize);
                        this.life = lifeSpan;
                        this.rChange = (endColor.R - startColor.R) / lifeSpan;
                        this.bChange = (endColor.B - startColor.B) / lifeSpan;
                        this.gChange = (endColor.G - startColor.G) / lifeSpan;
                        this.rColor = startColor.R;
                        this.gColor = startColor.G;
                        this.bColor = startColor.B;
                        this.alphaChange = ((startColor.A - endColor.A) / lifeSpan);
                        this.vSpeed = startSpeed.Y;
                        this.hSpeed = startSpeed.X;
                    }

                    /// <summary>
                    /// Advances the particle by one tick
                    /// </summary>
                    public void tick(Level level)
                    {
                        if (pos.Y > level.getY(pos.X))
                        {
                            vSpeed = -1F;
                            hSpeed /= 2F;
                        }

                        //handle color changes
                        rColor += rChange;
                        gColor += gChange;
                        bColor += bChange;
                        alpha += alphaChange;

                        //handle destroying this particle
                        life -= 1;
                        if (life <= 0)
                        {
                            parent.destroyParticle(this);
                        }

                        vAcc *= 1.001F;

                        hSpeed /= 1.001F;
                        vSpeed += vAcc;

                        pos.X += (float)hSpeed;
                        pos.Y += (float)vSpeed;
                    }

                    /// <summary>
                    /// Draws this particle using the SpriteBatch
                    /// </summary>
                    /// <param name="batch">The spriteBatch to draw to</param>
                    /// <param name="texture">The texture of this particle</param>
                    /// <param name="winPos">The position of the frame relative to (0,0)</param>
                    public void draw(SpriteBatch batch, Texture2D texture, Vector2 winPos)
                    {
                        batch.Draw(texture, pos - winPos, rect,
                            Color.FromNonPremultiplied((int)rColor, (int)gColor, (int)bColor, (int)alpha));
                    }
                }

                /// <summary>
                /// A peice of gore as part of a SimpleParticleSystem
                /// </summary>
                private class Gore
                {
                    public Vector2 pos;
                    SimpleParticleSystem parent;
                    double rChange = 0, gChange = 0, bChange = 0;
                    double rColor, gColor, bColor;
                    double alphaChange;
                    double alpha = 255;
                    public double hSpeed = 0, vSpeed = 0, vAcc = 0.1F;
                    public float life, angle, rotSpeed;
                    public int texID = 0;
                    public Rectangle rect;

                    /// <summary>
                    /// Creates a new particle
                    /// </summary>
                    /// <param name="parent">The SimpleParticleSystem that this particle belongs to</param>
                    /// <param name="pos">The position of this particle</param>
                    /// <param name="startColor">The initial color of the particle</param>
                    /// <param name="endColor">The final color of the particle</param>
                    /// <param name="startSize">The size of the particle</param>
                    /// <param name="lifeSpan">How many ticks the particle lasts</param>
                    /// <param name="startSpeed">The initial horizontal/vertical speeds</param>
                    public Gore(SimpleParticleSystem parent, Vector2 pos, Color startColor, Color endColor, float startSize, int lifeSpan, Vector2 startSpeed,
                        int texID)
                    {
                        this.parent = parent;
                        this.pos = pos;
                        this.rect = new Rectangle(0, 0, parent.goreTexs[texID].Width, parent.goreTexs[texID].Height);
                        this.life = lifeSpan;
                        this.rChange = (endColor.R - startColor.R) / (double)lifeSpan;
                        this.bChange = (endColor.B - startColor.B) / (double)lifeSpan;
                        this.gChange = (endColor.G - startColor.G) / (double)lifeSpan;
                        this.rColor = startColor.R;
                        this.gColor = startColor.G;
                        this.bColor = startColor.B;
                        this.alphaChange = (startColor.A / lifeSpan);
                        this.vSpeed = startSpeed.Y;
                        this.hSpeed = startSpeed.X;
                        this.texID = texID;
                        this.angle = (float)(parent.rand.NextDouble() * MathHelper.TwoPi);
                        rotSpeed = parent.rand.Next(-10, 10) / 10F;
                    }

                    /// <summary>
                    /// Advances the particle by one tick
                    /// </summary>
                    public void tick()
                    {
                        angle += rotSpeed;
                        if (pos.X < 0 | pos.Y < 0)
                            parent.destroyGore(this);

                        //handle color changes
                        rColor += rChange;
                        gColor += gChange;
                        bColor += bChange;

                        //handle destroying this particle
                        life -= 1;
                        if (life <= 0)
                        {
                            parent.destroyGore(this);
                        }

                        vAcc *= 1.001F;

                        hSpeed /= 1.001F;
                        vSpeed += vAcc;

                        pos.X += (float)hSpeed;
                        pos.Y += (float)vSpeed;
                    }

                    /// <summary>
                    /// Draws this particle using the SpriteBatch
                    /// </summary>
                    /// <param name="batch">The spriteBatch to draw to</param>
                    /// <param name="texture">The texture of this particle</param>
                    /// <param name="winPos">The position of the frame relative to (0,0)</param>
                    public void draw(SpriteBatch batch, Vector2 winPos)
                    {
                        batch.Draw(parent.goreTexs[texID], pos - winPos, rect,
                            Color.FromNonPremultiplied((int)rColor, (int)gColor, (int)bColor, (int)alpha), angle, new Vector2(5, 10),
                            0.75F, SpriteEffects.None, 0F);
                    }
                }
            }
        }
    }
}
