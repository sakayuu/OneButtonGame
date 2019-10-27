using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using OBG.Device;

namespace OBG.Scene
{
    class Clear : IScene
    {
        private bool isEndFlag;
        IScene backGroundScene;
        private Sound sound;//

        public Clear(IScene scene)
        {
            isEndFlag = false;
            backGroundScene = scene;
            var gameDevice = GameDevice.Instance();
            sound = gameDevice.GetSound();
        }

        public void Draw(Renderer renderer)
        {
            backGroundScene.Draw(renderer);

            renderer.Begin();
            renderer.DrawTexture("stagecler1" ,new Vector2(-50, 0), null, Color.White * 1.0f, 0, Vector2.Zero, new Vector2(0.5f, 0.5f));
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
            if (Input.GetKeyTrigger(Keys.Enter))
            {
                isEndFlag = true;
                
                //sound.PlaySE("endingse");
            }
        }
    }
}
