using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CircusCharlie
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private Classes.Editor editor;

        public static int SCREENWIDTH = 1000;
        public static int SCREENHEIGHT = 650;

        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private static List<Classes.Quad> quads;
        private static LinkedList<Classes.Quad> quadsTrans;

        public Game1()
        {
            quads = new List<Classes.Quad>();
            quadsTrans = new LinkedList<Classes.Quad>();

            graphics = new GraphicsDeviceManager(this);

            // Change screen size.
            graphics.PreferredBackBufferWidth = SCREENWIDTH;
            graphics.PreferredBackBufferHeight = SCREENHEIGHT;
            graphics.ApplyChanges();

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
            // TODO: Add your initialization logic here

            base.Initialize();

            //rasterizerState.CullMode = CullMode.CullCounterClockwise;



        }

        Texture2D texture;


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            texture = Content.Load<Texture2D>("Sprites/spr_ball");


            //quadEffect.Texture = texture;


            /*vertexDeclaration = new VertexDeclaration(new VertexElement[]
                {
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                    new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
                }
            );*/



            // TODO: use this.Content to load your game content here





            editor = new Classes.Editor(spriteBatch, this.Content);


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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin(SpriteSortMode.Deferred,
                              BlendState.AlphaBlend,
                              SamplerState.AnisotropicClamp,
                              DepthStencilState.None,
                              RasterizerState.CullNone);

            spriteBatch.End();

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;   // Enable proper depth.
            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;       // Pixelated mode for models.
            //GraphicsDevice.RasterizerState = RasterizerState.CullClockwise; // Backface

            spriteBatch.Begin(SpriteSortMode.Deferred,
                              BlendState.AlphaBlend,
                              SamplerState.PointClamp,
                              DepthStencilState.None,
                              RasterizerState.CullNone);

            // Only do things when the game is active.
            editor.Draw(IsActive);

            /********/
            /*      */
            /*  3D  */
            /*      */
            /********/

            foreach (EffectPass pass in Classes.MainGame.quadEffect.CurrentTechnique.Passes)
            {
                // Draw solid quads.
                for (int i = 0; i < quads.Count; i++)
                {
                    Classes.MainGame.quadEffect.Texture = quads[i].Tex;
                    Classes.MainGame.quadEffect.Alpha = quads[i].Alpha;
                    pass.Apply();

                    GraphicsDevice.DrawUserIndexedPrimitives
                    <VertexPositionNormalTexture>(
                    PrimitiveType.TriangleList,
                    quads[i].Vertices, 0, 4,
                    quads[i].Indexes, 0, 2);
                }

                // Need to draw transparent quads afterwards.
                for (int i = 0; i < quadsTrans.Count; i++)
                {
                    //Console.WriteLine(quadsTrans.ElementAt(i).Order);

                    Classes.MainGame.quadEffect.Texture = quadsTrans.ElementAt(i).Tex;
                    Classes.MainGame.quadEffect.Alpha = quadsTrans.ElementAt(i).Alpha;
                    pass.Apply();

                    GraphicsDevice.DrawUserIndexedPrimitives
                    <VertexPositionNormalTexture>(
                    PrimitiveType.TriangleList,
                    quadsTrans.ElementAt(i).Vertices, 0, 4,
                    quadsTrans.ElementAt(i).Indexes, 0, 2);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);

            // Clear the Quads because I don't see a better way of doing this.
            quads.Clear();
            quadsTrans.Clear();
        }

        public static void AddQuad(ref Classes.Quad quad)
        {
            quads.Add(quad);
        }

        public static void AddQuadTrans(ref Classes.Quad quad)
        {
            // Need to add them in order.
            int z = quad.Order;

            if (quadsTrans.Count == 0)
            {
                quadsTrans.AddFirst(quad);
            }
            else
            {
                bool done = false;

                // Thiiiiis... doesn't seem to work.

                for (int i = 0; i < quadsTrans.Count; i++)
                {
                    if (z >= quadsTrans.ElementAt(i).Order)
                    {
                        LinkedListNode<Classes.Quad> current = quadsTrans.Find(quadsTrans.ElementAt(i));
                        quadsTrans.AddBefore(current, quad);

                        done = true;
                        break;
                    }
                }

                if (!done)
                {
                    quadsTrans.AddLast(quad);
                }
            }
        }
    }
}
