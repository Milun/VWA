using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace CircusCharlie.Classes
{
    class MainGame
    {
        public static Texture2D texBG;
        Texture2D texMonitor;
        public static Sprite sprBall;
        Sprite sprBg;

        SpriteBatch spriteBatch;
        public static Ball ball;
        public static Room room;

        public static bool HD = true;

        public static Model       modelCube;
        public static Matrix      matrixView = Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), Vector3.Up);
        public static Matrix      matrixProj = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), (float)Game1.SCREENWIDTH/(float)Game1.SCREENHEIGHT, 0.1f, 150f);
        public static BasicEffect quadEffect;

        public static ContentManager content;

        private Quad bg;
        private Quad trees;

        // Constructor
        public MainGame(SpriteBatch _spriteBatch, ContentManager _content)
        {
            content = _content;

            
            // Initialise the effect used for Quads //
            quadEffect = new BasicEffect(Game1.graphics.GraphicsDevice);
            quadEffect.World = Matrix.Identity;
            
            quadEffect.TextureEnabled = true;

            quadEffect.EnableDefaultLighting();
            //quadEffect.LightingEnabled = true; // turn on the lighting subsystem.
            //quadEffect.AmbientLightColor = new Vector3(0.3f, 0.14f, 0.0f);
            //quadEffect.EmissiveColor = new Vector3(0.5f, 0.5f, 0.5f);

            //quadEffect.DiffuseColor = new Vector3(2.0f, 2.0f, 2.0f);

            //quadEffect.DirectionalLight0.DiffuseColor = new Vector3(2f, 2f, 2f); // a red light
            quadEffect.DirectionalLight0.Direction = new Vector3(0f, 0f, -3f);  // coming along the x-axis
            quadEffect.DirectionalLight0.SpecularColor = new Vector3(0f, 0f, 0f); // with green highlights

            //quadEffect.DirectionalLight1.Direction = new Vector3(3f, 0f, 0f);  // coming along the x-axis
            //quadEffect.DirectionalLight1.SpecularColor = new Vector3(0f, 0f, 0f); // with green highlights


            //////////////////////////////////////////

            modelCube = content.Load<Model>("Sprites/cube");

            spriteBatch = _spriteBatch;
            sprBall = new Sprite(content.Load<Texture2D>("Sprites/spr_ball"), ref spriteBatch);

            ball = new Ball(sprBall);

            texBG = content.Load<Texture2D>("Sprites/spr_bg");
            texMonitor = content.Load<Texture2D>("Sprites/spr_monitor");

            sprBg = new Sprite(texBG, ref spriteBatch, 3380, 2560);


            bg = new Quad(new Vector3(15f,-10f,100),
                          Vector3.Backward,
                          Vector3.Down,
                          170.4f,
                          204.8f,
                          Vector2.Zero,
                          Vector2.One,
                          texBG);

            trees = new Quad(new Vector3(15f, -20f, 30),
                          Vector3.Backward,
                          Vector3.Down,
                          100f,
                          120f,
                          Vector2.Zero,
                          Vector2.One,
                          content.Load<Texture2D>("Sprites/spr_trees"));


            Global.viewSize = new Vector2(Game1.SCREENWIDTH / 4f, Game1.SCREENHEIGHT / 4f);
        }

        public void InitGame(ref Room _room, IntVector2D startPos)
        {
            room = _room;
            room.InitRoom();
            ball.SetPos(startPos);
            Global.viewZoom = 2f;
        }

        public static void StopGame()
        {
            if (room == null) return;
            room.StopRoom();
            room = null;
            Global.viewZoom = 1f;
            //Global.viewCenter = Vector2.Zero;
            Editor.showDebug = false;
        }

        private Vector2 GetBallPercent()
        {
            if (room == null) return Vector2.Zero;

            return new Vector2(ball.GetPos().X / room.GetRoomSize().X,
                               ball.GetPos().Y / room.GetRoomSize().Y);
        }

        private void DrawLvl1()
        {
            // Concider just making the BG 2D. Would save a lot of processing to have the cameras near/far planes smaller.
            Game1.AddQuad(ref bg);
            Game1.AddQuad(ref trees);
        }

        public void Draw()
        {
            if (room == null) return;


            DrawLvl1();
            room.Draw3D(new IntVector2D(0,0));
            

            ball.Draw();

            Global.SetViewCenter(ball.GetPos());

            ////////
            // 3D //
            ////////
            quadEffect.View         = matrixView;
            quadEffect.Projection   = matrixProj;

            float viewX = ball.GetPos().X;
            float viewY = ball.GetPos().Y;

            if (viewX < 11f) viewX = 11f;
            if (viewY < 7f) viewY = 7f;

            if (viewX > 18.3f) viewX = 18.3f;
            if (viewY > 14.8f) viewY = 14.8f;

            matrixView = Matrix.CreateLookAt(new Vector3(viewX, viewY, -17),
                                             new Vector3(viewX, viewY, 0), Vector3.Down);
        }
    }
}
