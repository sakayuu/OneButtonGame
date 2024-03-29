﻿using System;
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
        public int stagecount = 1;

        private Scene scene;
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
            if (stagecount < 6)
            {
                renderer.DrawTexture("stagecler1", new Vector2(-50, 0), null, Color.White * 1.0f, 0, Vector2.Zero, new Vector2(0.5f, 0.5f));
            }
            else
            {
                renderer.DrawTexture("stagecler1", new Vector2(-50, 0), null, Color.White * 1.0f, 0, Vector2.Zero, new Vector2(0.5f, 0.5f));
            }
            renderer.End();
        }

        public void Initialize()
        {
            isEndFlag = false;
            if (stagecount < 6)
            {
                stagecount++;
            }
            else
            {
                stagecount = 1;
            }
        }

        public bool IsEnd()
        {
            return isEndFlag;
        }

        public Scene Next()
        {
            return scene;
        }

        public void Shutdown()
        {
            sound.StopBGM();
        }

        public void Update(GameTime gameTime)
        {
            if (Input.GetKeyTrigger(Keys.Enter)&&!isEndFlag)
            {
                if (stagecount <= 5)
                {
                    switch (GamePlay.stage)
                    {
                        case Stage.stage1:
                            GamePlay.stage = Stage.stage2;
                            break;
                        case Stage.stage2:
                            GamePlay.stage = Stage.stage3;
                            break;
                        case Stage.stage3:
                            GamePlay.stage = Stage.stage4;
                            break;
                        case Stage.stage4:
                            GamePlay.stage = Stage.stage5;
                            break;
                        case Stage.stage5:
                            GamePlay.stage = Stage.stage1;
                            break;
                        default:
                            break;
                    }
                    scene = Scene.GamePlay;
                }
                else
                {
                    GamePlay.stage = Stage.stage1;
                    scene = Scene.Title;
                }
                isEndFlag = true;

                //sound.PlaySE("endingse");
            }
        }
    }
}
