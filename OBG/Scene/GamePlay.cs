using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using OBG.Actor;
using OBG.Device;

namespace OBG.Scene
{
    class GamePlay : IScene
    {
        private CharacterManager characterManager;
        private bool isEndFlag;

        public GamePlay()
        {
            isEndFlag = false;
            var gameDevice = GameDevice.Instance();

        }
        public void Draw(Renderer renderer)
        {
            //描画開始
            renderer.Begin();
            //背景を描画
            renderer.DrawTexture("stage", Vector2.Zero);
            characterManager.Draw(renderer);//キャラクター管理者の描画

            renderer.End();
        }

        public void Initialize()
        {
            //シ－ン終了フラグを初期化
            isEndFlag = false;

            //キャラクターマネージャーの実体生成
            characterManager = new CharacterManager();
            Ball ball = new Ball("black", new Vector2(300, 300));
            characterManager.Add(ball);

            characterManager.Add(new Pin("pin", new Vector2(400, 100)));
            characterManager.Add(new Pin("pin", new Vector2(500, 300)));
            characterManager.Add(new Pin("pin", new Vector2(350, 400)));

        }

        public bool IsEnd()
        {
            return isEndFlag;
        }

        public Scene Next()
        {
            Scene nextScene = Scene.Ending;
            return nextScene;
        }

        public void Shutdown()
        {

        }

        public void Update(GameTime gameTime)
        {
            characterManager.Update(gameTime);

            if (characterManager.GetBall().IsDead())
                isEndFlag = true;

            if (Input.GetKeyTrigger(Keys.P))
            {
                characterManager.GetBall().gameStartFlag = true;
                characterManager.GetShortestCheck(characterManager.GetBall(), characterManager.GetList()).SetCatchPos(characterManager.GetBall().GetPosition());
                Vector2 bP = characterManager.GetBall().GetPosition();
                Vector2 pP = characterManager.GetShortestCheck(characterManager.GetBall(), characterManager.GetList()).GetPosition();

                characterManager.GetBall().radius
                    = (float)characterManager.CheckDistance(characterManager.GetBall().GetPosition()
                    , characterManager.GetShortestCheck(characterManager.GetBall(), characterManager.GetList()).GetPosition());

                if (bP.X < pP.X)
                    characterManager.GetBall().LRflag = true;
                else
                    characterManager.GetBall().LRflag = false;
                if (bP.Y < pP.Y)
                    characterManager.GetBall().UDflag = true;
                else
                    characterManager.GetBall().UDflag = false;
                characterManager.GetBall().GetPPos(characterManager.GetShortestCheck(characterManager.GetBall(), characterManager.GetList()).GetPosition());
                characterManager.GetBall().moveFlag = false;



            }

        }
    }
}
