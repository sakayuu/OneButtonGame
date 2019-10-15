using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using OBG.Actor;
using OBG.Device;

namespace OBG.Scene
{
    class GamePlay : IScene, IGameMediator
    {
        private CharacterManager characterManager;
        private bool isEndFlag;

        private Pin pin;

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
            //renderer.DrawTexture("stage", Vector2.Zero);
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

            characterManager.Add(new Pin("pin", new Vector2(400, 100), this)); //継承してるのでthisでmediatorを渡せる
            characterManager.Add(new Pin("pin", new Vector2(500, 300), this));
            characterManager.Add(new Pin("pin", new Vector2(350, 400), this));

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
            characterManager.Update(gameTime); //キャラ一括更新

            if (characterManager.GetBall().IsDead()) //プレイヤー死んだらゲームオーバー
                isEndFlag = true;

            if (Input.GetKeyTrigger(Keys.Enter))
            {
                pin = null;
                Vector2 bP = characterManager.GetBall().GetPosition();
                pin = characterManager.GetShortestCheck(characterManager.GetBall(), characterManager.GetList());

                pin.radius = (float)characterManager.CheckDistance(bP, pin.GetPosition());
                characterManager.GetBall().SetRadius(pin.radius);

                characterManager.GetBall().GetPPos(pin.GetPosition());

                characterManager.GetBall().vector = characterManager.GetBall().GetPosition() - pin.GetPosition();
                characterManager.GetBall().vector.Normalize();

                var rad = Math.Atan2(characterManager.GetBall().vector.Y, characterManager.GetBall().vector.X);

                Debug.WriteLine(rad);
                characterManager.GetBall().ang = (float)-Math.Cos(rad);
                characterManager.GetBall().ang2 = (float)-Math.Sin(rad);

                if (bP.X < pin.GetPosition().X)
                {
                    characterManager.GetBall().LRflag = true;

                    pin.LRflag = true;
                }
                else
                {
                    characterManager.GetBall().LRflag = false;
                    pin.LRflag = false;
                }


                characterManager.GetBall().ballState = BallState.Link;
                pin.catchFlag = true;
            }

            if (characterManager.GetBall().ballState == BallState.Link)
            {
                pin.bPos = characterManager.GetBall().GetPosition();
                //characterManager.GetBall().SetBallPos(pin.catchPos);
                characterManager.GetBall().angle = pin.SetAngle();
            }
            else if (characterManager.GetBall().ballState == BallState.Free)
            {
                pin.catchFlag = false;
            }

        }

        public void AddActor(Collider collider)
        {
            characterManager.Add(collider);
        }
    }
}
