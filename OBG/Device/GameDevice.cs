using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBG.Device
{


    sealed class GameDevice
    {
        //
        private static GameDevice instance;

        //
        private Renderer renderer;
        private Sound sound;
        private static Random random;
        private ContentManager content;
        private GraphicsDevice graphics;
        private GameTime gameTime;

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name=""></param>
        private GameDevice(ContentManager content, GraphicsDevice graphics)
        {
            renderer = new Renderer(content, graphics);
            sound = new Sound(content);
            random = new Random();
            this.content = content;
            this.graphics = graphics;
        }


        public static GameDevice Instance(ContentManager content,
            GraphicsDevice graphics)
        {
            //
            if (instance == null)
            {
                instance = new GameDevice(content, graphics);
            }
            return instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static GameDevice Instance()
        {
            //
            Debug.Assert(instance != null,
                "Game1クラスのInitializeメソッド内で引数付きInstanceメソッドをよんでください");
            return instance;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">ゲーム時間</param>
        public void Update(GameTime gameTime)
        {
            //デバイスで絶対に1回のみ更新が必要なモノ
            Input.Update();
            this.gameTime = gameTime;
        }


        public Renderer GetRenderer()
        {
            return renderer;
        }


        public Sound GetSound()
        {
            return sound;
        }


        public Random GetRandom()
        {
            return random;
        }


        public GraphicsDevice GetGraphicsDevice()
        {
            return graphics;
        }

        /// <summary>
        /// ゲーム時間の取得
        /// </summary>
        /// <returns></returns>
        public GameTime GetGameTime()
        {
            return gameTime;
        }
    }
}
