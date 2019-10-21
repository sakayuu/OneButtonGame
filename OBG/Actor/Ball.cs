using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using OBG.Def;
using OBG.Device;
using OBG.Scene;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OBG.Actor
{
    enum BallState
    {
        Start,
        Free,
        Link,
    }

    class Ball : Character, IGameMediator
    {
        private float speed = 10f; //スピード
        private Vector2 distance; //目的地
        private Vector2 velocity; //移動量
        public Vector2 vector; //ベクトルの向き

        public bool moveFlag = false; //動くかどうか
        public Vector2 catchPos;

        int LR; //右回転か左回転かを制御

        public bool LRflag; //ピンに対してプレイヤーが左にいるか右にいるか
        public bool UDflag; //ピンに対してプレイヤーが上にいるか下にいるか

        public float angle;
        public int rr;

        public Vector2 pPosition;

        float rad = 0;
        public float w = 0;
        public float h = 0;

        public BallState ballState;

        IGameMediator mediator;

        float radius;
        private int count = 0;
        public float ang;
        private bool hitflag = false;
        public bool freeFlag = false;
        private bool yflag = false;
        public Ball(string name, Vector2 position, IGameMediator mediator)
        {
            this.position = position;
            this.name = name;
            this.mediator = mediator;
            pixelSize = 64;
        }

        public override void Initialize()
        {
            isDeadFlag = false;
            LR = 0;
            LRflag = false;
            ang = 0;
            rr = -1;
            ballState = BallState.Start;
        }

        public override void Update(GameTime gameTime)
        {
            if (LRflag)
                LR = 1;
            else
                LR = -1;
            Move();
            Debug.WriteLine(rad);
            if(hitflag == true)
            {
                count++;
                if(count>= 5)
                {
                    hitflag = false;
                }
            }
        }

        public override void Shutdown()
        {

        }

        public override void Hit(Character other)
        {
            if (other is Enemy )
            {
                isDeadFlag = true;
            }
            if(other is Pin && ballState == BallState.Link)
            {
                isDeadFlag = true;
            }
            if (other is Pin && yflag == false && hitflag == false && ballState == BallState.Free 
                || other is Collider && yflag == false && hitflag == false && ballState == BallState.Free)
            {
                hitflag = true;
                //rad *= -1;
                yflag = true;
            }
            if (other is Pin && yflag == true && hitflag == false && ballState == BallState.Free 
                || other is Collider && yflag == false && hitflag == false && ballState == BallState.Free)
            {
                hitflag = true;
                //rad *= -1;
                yflag = false;
            }

            //  hitflag = true;
        }

        public override void Move()
        {
            if (ballState == BallState.Start)
                position = position + new Vector2(0, -1) * (speed / 10);
            else if (ballState == BallState.Free) //移動可能なら
            {
                if (freeFlag)
                {
                    //まっすぐに移動
                    
                    if (position.X < 0 || position.X +pixelSize >= Screen.Width)
                    {
                        rad *= -1;
                    }
                    if (position.Y < 0 && yflag == false && hitflag == false
                        || position.Y + pixelSize >= Screen.Height && yflag == false && hitflag == false)
                    {
                        hitflag = true;
                        rad *= -1;
                        yflag = true;
                    }
                    if (position.Y < 0 && yflag == true && hitflag == false
                        || position.Y + pixelSize >= Screen.Height && yflag == true && hitflag == false)
                    {
                        hitflag = true;
                        rad *= -1;
                        yflag = false;
                    }
                    if (yflag==true)
                    {
                        position.X += ((float)Math.Cos(rad + MathHelper.ToRadians(270)) * speed) * -LR;
                        position.Y += ((float)Math.Sin(rad + MathHelper.ToRadians(270)) * speed) * -LR;
                    }
                    if (yflag == false)
                    {
                        position.X += ((float)Math.Cos(rad + MathHelper.ToRadians(90)) * speed) * -LR;
                        position.Y += ((float)Math.Sin(rad + MathHelper.ToRadians(90)) * speed) * -LR;
                    }
                    ang = 0;
                    Debug.WriteLine(yflag);
                    Debug.WriteLine(hitflag);
                }
            }
            else if (ballState == BallState.Link)
            {
                yflag = false;
                AddActor(new RayLine("particle", position, pPosition));

                position = pPosition + new Vector2((float)Math.Cos(ang + MathHelper.ToRadians(angle)), (float)Math.Sin(ang + MathHelper.ToRadians(angle))) * radius;
                Debug.WriteLine(MathHelper.ToDegrees(ang));
            }

            if (Input.GetKeyRelease(Keys.Enter)) //キーが離されたら
            {
                
                rad = 0; //角度初期化

                rad = GetRadian(position, pPosition); //ベクトルの角度を取得

                ballState = BallState.Free; //移動可能
            }

        }

        /// <summary>
        /// 自分の位置を渡す
        /// </summary>
        /// <returns></returns>
        public override Vector2 GetPosition()
        {
            return base.GetPosition();
        }


        public void SetBallPos(Vector2 pos)
        {
            position = pos;

        }

        public override void Draw(Renderer renderer)
        {
            if (ballState == BallState.Link)
                renderer.DrawLine(position, pPosition);
            base.Draw(renderer);
        }

        /// <summary>
        /// ピンの位置をもらう
        /// </summary>
        /// <param name="pos"></param>
        public void GetPPos(Vector2 pos)
        {
            pPosition = pos;
        }

        /// <summary>
        /// v1からv2までのベクトルの角度を求める
        /// </summary>
        /// <param name="v1">始めの位置</param>
        /// <param name="v2">終わりの位置</param>
        /// <returns></returns>
        public float GetRadian(Vector2 v1, Vector2 v2)
        {
             w = v2.X - v1.X;
             h = v2.Y - v1.Y;

            return (float)Math.Atan2(h, w);
        }


        public void SetRadius(float rd)
        {
            radius = rd;
        }

        public void AddActor(Character character)
        {
            if (character is Collider)
                mediator.AddActor((Collider)character);
            else if (character is RayLine)
                mediator.AddActor((RayLine)character);
        }

       
    }
}
