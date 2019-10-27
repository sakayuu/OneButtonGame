using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OBG.Actor;
using OBG.Def;
using OBG.Device;
using OBG.Util;

namespace OBG.Scene
{
    enum Stage
    {
        stage1,
        stage2,
        stage3,
        stage4,
        stage5
    }
    class GamePlay : IScene, IGameMediator
    {
        private CharacterManager characterManager;
        private bool isEndFlag;
        public Stage stage = Stage.stage1;
        private float AllField, NowField;
        private float pasent;
        private float timecount;
        private int time = 3;
        public static bool timeflag = false;
        public bool clearflag = false;
        private float area;

        public Timer timer;

        Scene nextScene;
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
            renderer.DrawTexture("back2", Vector2.Zero, null, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 1, 1);

            if (Ball.ballState == BallState.Free)
            {
                renderer.DrawTexture("target", characterManager.pin.GetPosition(), null, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0, 1);
            }
            characterManager.Draw(renderer);//キャラクター管理者の描画
            renderer.DrawNumber("number1", new Vector2(600, 13), pasent);
            renderer.DrawNumber("number1", new Vector2(200, 13), area);
            if (timeflag == false && Transition.irisState == Transition.IrisState.None && isEndFlag == false)
            {
                renderer.DrawNumber("number1", new Vector2(400, 400), time);
            }
            if (timeflag == true && time == 0)
            {
                renderer.DrawTexture("Ggo2", new Vector2(200, 350));
            }

            renderer.End();
        }

        public void Initialize()
        {
            //シ－ン終了フラグを初期化
            isEndFlag = false;
            timer = new CountDownTimer(2);
            //キャラクターマネージャーの実体生成

            characterManager = new CharacterManager();
            Ball ball = new Ball("Player4", new Vector2(400, 800), this);
            characterManager.Add(ball);
            Ball.ballState = BallState.Start;
            if (stage == Stage.stage1)
            {
                characterManager.Add(new Pin("pinmusic2", new Vector2(400, 400), 0, this));
                Enemy enemy = new Enemy("Enemy1", new Vector2(9000, 9000), this);
                characterManager.Add(enemy);
            }
            if (stage == Stage.stage2)
            {
                Enemy enemy = new Enemy("Enemy1", new Vector2(9000, 9000), this);
                characterManager.Add(enemy);
                characterManager.Add(new Pin("pinmusic2", new Vector2(200, 200), 0, this)); //継承してるのでthisでmediatorを渡せる
                characterManager.Add(new Pin("pinmovie2", new Vector2(550, 550), 1, this));
            }
            if (stage == Stage.stage3)
            {
                Enemy enemy = new Enemy("Enemy1", new Vector2(9000, 9000), this);
                characterManager.Add(enemy);
                characterManager.Add(new Pin("pinmusic2", new Vector2(400, 300), 0, this)); //継承してるのでthisでmediatorを渡せる
                characterManager.Add(new Pin("pinmovie2", new Vector2(250, 500), 1, this));
                characterManager.Add(new Pin("pingame2", new Vector2(550, 500), 2, this));
            }
            if (stage == Stage.stage4)
            {
                Enemy enemy = new Enemy("Enemy1", new Vector2(400, 100), this);
                characterManager.Add(enemy);
                characterManager.Add(new Pin("pinmusic2", new Vector2(200, 200), 0, this)); //継承してるのでthisでmediatorを渡せる
                characterManager.Add(new Pin("pinmovie2", new Vector2(400, 400), 1, this));
                characterManager.Add(new Pin("pingame2", new Vector2(600, 600), 2, this));
            }
            if (stage == Stage.stage5)
            {
                Enemy enemy = new Enemy("Enemy1", new Vector2(400, 100), this);
                characterManager.Add(enemy);
                characterManager.Add(new Pin("pinmusic2", new Vector2(400, 400), 0, this)); //継承してるのでthisでmediatorを渡せる
                characterManager.Add(new Pin("pinmovie2", new Vector2(200, 200), 1, this));
                characterManager.Add(new Pin("pingame2", new Vector2(600, 600), 2, this));
                characterManager.Add(new Pin("pinmusic2", new Vector2(600, 200), 3, this)); //継承してるのでthisでmediatorを渡せる
                characterManager.Add(new Pin("pinmovie2", new Vector2(200, 600), 4, this));
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
            if (characterManager.GetBall().IsDead())
                nextScene = Scene.Ending;

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
                timer.Update(gameTime);
            if (timer.IsTime())
                isEndFlag = true;
            if (Transition.irisState == Transition.IrisState.None)
            {
                timecount++;
                if (timeflag == false)
                {
                    if (timecount >= 60)
                    {
                        time--;
                        timecount = 0;
                    }
                    
                }
                
                if (time <= 0)
                {
                    timeflag = true;
                    if (timecount >= 60)
                    {
                        time = 4;
                    }

                }
            }

            if (timeflag == true)
            {
                if (Input.GetKeyTrigger(Keys.Enter))
                {
                    characterManager.GetBall().freeFlag = false;
                    characterManager.pin = null;
                    characterManager.GetAngleForBallToPins();

                    Vector2 vec = characterManager.GetBall().GetVector();
                    Vector2 vec2 = characterManager.pin.GetPosition() - characterManager.GetBall().GetPosition();

                    Vector3 w = Vector3.Cross(new Vector3(vec.X, vec.Y, 0), new Vector3(vec2.X, vec2.Y, 0));

                    if (w.Z > 0)
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
                    foreach (var pin in characterManager.GetList())
                    {
                        pin.catchFlag = false;
                    }
                    characterManager.GetBall().freeFlag = true;
                }

            }
            NowField = 0;
            foreach (var a in characterManager.GetList())
            {
                NowField += a.SetField();
            }
            pasent = (NowField / AllField) * 100;
            Debug.WriteLine(clearflag);
            switch (stage)
            {
                case Stage.stage1:
                    
                    if (Input.GetKeyRelease(Keys.Enter))
                    {
                        if (pasent >= area && !characterManager.GetBall().IsDead())
                        {
                            
                            stage = Stage.stage2;
                            nextScene = Scene.Clear;
                            isEndFlag = true;
                            //Initialize();
                            NowField = 0;
                            timeflag = false;
                        }
                    }
                    
                    area = 50;
                    break;
                    
                case Stage.stage2:
                    
                    if (Input.GetKeyRelease(Keys.Enter))
                    {
                        if (pasent >= area && !characterManager.GetBall().IsDead())
                        {
                            
                            stage = Stage.stage3;
                            //Initialize();
                            nextScene = Scene.Clear;
                            isEndFlag = true;
                            NowField = 0;
                            timeflag = false;
                        }
                    }
                    area = 50;
                    break;
                    
                case Stage.stage3:
                    
                    if (Input.GetKeyRelease(Keys.Enter))
                    {
                        if (pasent >= area && !characterManager.GetBall().IsDead())
                        {
                            
                            NowField = 0;
                            stage = Stage.stage4;
                            //Initialize();
                            nextScene = Scene.Clear;
                            isEndFlag = true;
                            timeflag = false;
                        }
                    }
                   
                    area = 30;
                    break;
                case Stage.stage4:
                    
                    if (Input.GetKeyRelease(Keys.Enter))
                    {
                        if (pasent >= area && !characterManager.GetBall().IsDead())
                        {
                            
                            NowField = 0;
                            stage = Stage.stage5;
                            nextScene = Scene.Clear;
                            //Initialize();
                            isEndFlag = true;
                            timeflag = false;
                        }
                    }
                   
                    area = 10;
                    break;
                case Stage.stage5:
                    
                    if (Input.GetKeyRelease(Keys.Enter))
                    {
                        if (pasent >= area && !characterManager.GetBall().IsDead())
                        {
                            NowField = 0;
                            stage = Stage.stage1;
                            nextScene = Scene.Title;
                            isEndFlag = true;
                            timeflag = false;
                        }
                    }
                    area = 30;
                    break;
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
