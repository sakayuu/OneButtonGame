using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using OBG.Device;
using Microsoft.Xna.Framework.Input;

namespace OBG.Scene
{
    class Ending : IScene
    {
        private bool isEndFlag;
        IScene backGroundScene;
        private Sound sound;//

        public Ending(IScene scene)
        {
            isEndFlag = false;
            backGroundScene = scene;
            var gameDevice = GameDevice.Instance();
            sound = gameDevice.GetSound();
        }
        public void Draw(Renderer renderer)
        {
            //
            //
            //
            backGroundScene.Draw(renderer);

            renderer.Begin();
            renderer.DrawTexture("gameover", new Vector2(0, 100), null, Color.White * 1.0f, 0, Vector2.Zero, new Vector2(0.5f, 0.5f));
            //renderer.DrawTexture("Player1", new Vector2(200));
            renderer.End();
        }

        public void Initialize()
        {
            isEndFlag = false;
        }

        public bool IsEnd()
        {
            return isEndFlag;
        }

        public Scene Next()
        {
            return Scene.GamePlay;
        }

        public void Shutdown()
        {
            sound.StopBGM();
        }

        public void Update(GameTime gameTime)
        {
            sound.PlayBGM("UNKNOWN_AREA");

            if (Input.GetKeyTrigger(Keys.Enter))
            {
                isEndFlag = true;
                sound.PlaySE("serect");
            }
        }
    }
}
