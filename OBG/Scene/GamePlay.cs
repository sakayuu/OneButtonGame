using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using OBG.Actor;
using OBG.Def;
using OBG.Device;

namespace OBG.Scene
{
    enum Stage
    {
        stage1,
        stage2,
        stage3,
    }
    class GamePlay : IScene, IGameMediator
    {
        private CharacterManager characterManager;
        private bool isEndFlag;
        public Stage stage=Stage.stage1;
        private float AllField,NowField;
        private float pasent;
        private float timecount ;
        private int time = 3;
        public static bool timeflag = false;
        private float area;
        public GamePlay()
        {
            isEndFlag = false;
            var gameDevice = GameDevice.Instance();
            stage = Stage.stage1;
            AllField = Screen.Width * Screen.Width;
            area = 40;
            
        }
        public void Draw(Renderer renderer)
        {
            //描画開始
            renderer.Begin();
            //背景を描画
            //renderer.DrawTexture("stage", Vector2.Zero);
            renderer.DrawTexture("back2", Vector2.Zero);
            if(Ball.ballState == BallState.Free)
            {
                renderer.DrawTexture("target", characterManager.pin.GetPosition());
            }
            
            renderer.DrawNumber("number", new Vector2(200, 13), pasent);
            renderer.DrawNumber("number", new Vector2(400, 13), area);
            characterManager.Draw(renderer);//キャラクター管理者の描画
            if(timeflag == false)
            {
                renderer.DrawNumber("number", new Vector2(400, 400), time);
            }
            
            renderer.End();
        }

        public void Initialize()
        {
            //シ－ン終了フラグを初期化
            isEndFlag = false;
            
            //キャラクターマネージャーの実体生成

            characterManager = new CharacterManager();
            Ball ball = new Ball("Player4", new Vector2(300, 300), this);
            characterManager.Add(ball);
            Ball.ballState = BallState.Start;
            if (stage==Stage.stage1)
            {
                Enemy enemy = new Enemy("white", new Vector2(100, 700), this);
                characterManager.Add(enemy);
                characterManager.Add(new Pin("pinmusic2", new Vector2(800, 100), 0, this));
                characterManager.Add(new Pin("pinmusic2", new Vector2(100, 100),1, this)); //継承してるのでthisでmediatorを渡せる
                characterManager.Add(new Pin("pinmusic2", new Vector2(400, 400),2, this));
                characterManager.Add(new Pin("pinmovie2", new Vector2(800, 800), 3, this)); //継承してるのでthisでmediatorを渡せる
                characterManager.Add(new Pin("pinmovie2", new Vector2(100, 800), 4, this));  
            }
            if (stage == Stage.stage2)
            {
                Enemy enemy = new Enemy("white", new Vector2(100, 700), this);
                characterManager.Add(enemy);
                characterManager.Add(new Pin("pinmusic2", new Vector2(800, 100),0, this)); //継承してるのでthisでmediatorを渡せる
                characterManager.Add(new Pin("pinmovie2", new Vector2(400, 300),1, this));
                characterManager.Add(new Pin("pin", new Vector2(750, 400),2, this));
            }
            if (stage == Stage.stage3)
            {
                Enemy enemy = new Enemy("white", new Vector2(100, 700), this);
                characterManager.Add(enemy);
                characterManager.Add(new Pin("pinmusic2", new Vector2(800, 100),0, this)); //継承してるのでthisでmediatorを渡せる
                characterManager.Add(new Pin("pin", new Vector2(750, 400),1, this));
            }
            foreach (var a in characterManager.GetList())
            {
                a.GetField(0);
            }

        }

        public bool IsEnd()
        {
            return isEndFlag;
        }

        public Scene Next()
        {
            Scene nextScene = Scene.Title;
            return nextScene;
        }

        public void Shutdown()
        {

        }

        public void Update(GameTime gameTime)
        {
            characterManager.Update(gameTime); //キャラ一括更新
            if (Ball.ballState == BallState.Free)
            {
                characterManager.pin = null;
                characterManager.GetAngleForBallToPins();
            }
            
            if (characterManager.GetBall().IsDead()) //プレイヤー死んだらゲームオーバー
                isEndFlag = true;
            
            
            if (timecount >= 60 && timeflag == false)
            {
                time--;
                timecount = 0;
            }
            if(timeflag == false)
            {
                timecount++;
            }
            if(time <= 0)
            {
                timeflag = true;
            }


            if (timeflag == true)
            {
                if (Input.GetKeyTrigger(Keys.Enter))
                {
                    characterManager.GetBall().freeFlag = false;
                    characterManager.pin = null;
                    characterManager.GetAngleForBallToPins();

                    if (characterManager.GetBall().GetPosition().X < characterManager.pin.GetPosition().X)
                    {
                        characterManager.GetBall().LRflag = true;

                        characterManager.pin.LRflag = true;
                    }
                    else
                    {
                        characterManager.GetBall().LRflag = false;
                        characterManager.pin.LRflag = false;
                    }


                    Ball.ballState = BallState.Link;
                    characterManager.pin.catchFlag = true;
                }

                if (Ball.ballState == BallState.Link)
                {
                    characterManager.GetBall().angle = characterManager.pin.SetAngle();
                    characterManager.GetBall().GetPPos(characterManager.pin.GetPosition());
                    characterManager.GetBall().SetRadius(characterManager.pin.radius);
                    //pin.bPos = characterManager.GetBall().GetPosition();
                    //characterManager.GetBall().SetBallPos(pin.catchPos);
                }
                else if (Ball.ballState == BallState.Free)
                {
                    characterManager.pin.catchFlag = false;
                    characterManager.GetBall().freeFlag = true;
                }
            
            }
            NowField = 0;
            foreach (var a in characterManager.GetList())
            {
                NowField += a.SetField();
            }
            pasent = (NowField / AllField) * 100;
            Debug.WriteLine(pasent);
            if (pasent>=area)
            {
                switch (stage)
                {
                    case Stage.stage1:
                        if(Input.GetKeyRelease (Keys.Enter))
                        {
                            stage = Stage.stage2;
                            Initialize();
                            NowField = 0;
                        }

                        break;
                    case Stage.stage2:
                        if (Input.GetKeyRelease(Keys.Enter))
                        {
                            stage = Stage.stage3;
                            Initialize();
                            NowField = 0;
                        }
                        break;
                    case Stage.stage3:
                        if(Input.GetKeyRelease(Keys.Enter))
                        {
                            NowField = 0;
                            stage = Stage.stage1;
                            isEndFlag = true;
                        }
                        break;
                }
            }
            
        }

        /// <summary>
        /// キャラクター追加（if増やして）
        /// </summary>
        /// <param name="character">オブジェクトの型（継承元）</param>
        public void AddActor(Character character)
        {
            if (character is Collider)
                characterManager.Add((Collider)character);
            else if (character is RayLine)
                characterManager.Add((RayLine)character);
            else if (character is Ball)
                characterManager.Add((Ball)character);
        }

        public void AddCollider(Collider collider, int pinNum)
        {
            characterManager.AddCollider(collider, pinNum);
        }
    }
}
