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
        private Ball ball;

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
            Ball ball = new Ball("black", new Vector2(300, 300), this);
            characterManager.Add(ball);

            characterManager.Add(new Pin("pin", new Vector2(800, 100), this)); //継承してるのでthisでmediatorを渡せる
            characterManager.Add(new Pin("pin", new Vector2(400, 300), this));
            characterManager.Add(new Pin("pin", new Vector2(750, 400), this));

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

            //float degree = 0.0f;
            if (Input.GetKeyTrigger(Keys.Enter))
            {
                ball = characterManager.GetBall();
                pin = characterManager.GetShortestCheck(characterManager.GetBall(), characterManager.GetList());

                pin.radius = (float)characterManager.CheckDistance(ball.GetPosition(),
                    pin.GetPosition());

                ball.SetRadius((float)characterManager.CheckDistance(ball.GetPosition(),
                    pin.GetPosition()));

                ball.GetPPos(pin.GetPosition());

                ball.vector = ball.GetPosition() - pin.GetPosition();
                ball.vector.Normalize();

                var rad = Math.Atan2(ball.vector.Y, ball.vector.X);
                var degree = MathHelper.ToDegrees((float)rad);
                if (degree < 0)
                {
                    degree = (360 + degree);
                }
                ball.ang = MathHelper.ToRadians(degree);

                //position.X = pPosition.X + (float)-Math.Cos(MathHelper.ToRadians(MathHelper.ToRadians(degree))) * pin.radius;
                //position.Y = pPosition.Y + (float)-Math.Sin(MathHelper.ToRadians(MathHelper.ToRadians(degree))) * pin.radius;


                if (characterManager.GetBall().GetPosition().X < pin.GetPosition().X)
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
                ball.angle = pin.SetAngle();
                //pin.bPos = characterManager.GetBall().GetPosition();
                //characterManager.GetBall().SetBallPos(pin.catchPos);
            }
            else if (characterManager.GetBall().ballState == BallState.Free)
            {
                pin.catchFlag = false;
            }

        }

        public void AddActor(Character character)
        {
            if (character is Collider)
                characterManager.Add((Collider)character);
            else if (character is RayLine)
                characterManager.Add((RayLine)character);
        }
    }
}
