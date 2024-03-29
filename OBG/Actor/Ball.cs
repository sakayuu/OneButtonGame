﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    public enum BallState
    {
        Start,
        Free,
        Link,
    }

    class Ball : Character, IGameMediator
    {
        private float speed = 10f; //スピード
        private float startspeed = 10f;
        private Vector2 distance; //目的地
        private Vector2 velocity; //移動量
        public Vector2 vector; //ベクトルの向き

        public bool moveFlag = false; //動くかどうか

        int LR; //右回転か左回転かを制御

        public bool LRflag; //ピンに対してプレイヤーが左にいるか右にいるか
        public bool UDflag; //ピンに対してプレイヤーが上にいるか下にいるか

        public float angle;
        public int rr;

        public Vector2 pPosition;

        float rad = 0;
        public float w = 0;
        public float h = 0;

        public static BallState ballState;

        IGameMediator mediator;

        public int nowPinNum;

        float radius;
        private int count = 0;
        public float ang;
        private bool hitflag = false;
        public bool freeFlag = false;
        private bool yflag = false;
        private bool effectfrag;
        private float effect;
        public bool Lhitflag;
        private Vector2 effectpos;
        private Sound sound;//サウンドオブジェクト
        Random rnd = new Random();
        int rndNum;

        public Vector2 nowVector;

        public Ball(string name, Vector2 position, IGameMediator mediator)
        {
            var gamedevice = GameDevice.Instance();
            sound = gamedevice.GetSound();
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
            nowPinNum = 0;
            nowVector = position;
            Lhitflag = false;
        }

        public override void Update(GameTime gameTime)
        {
            rndNum = rnd.Next(0, 2);
            if (rndNum == 0)
                mediator.AddActor(new RayLine("sikaku", position));
            else
                mediator.AddActor(new RayLine("sikaku2", position));


            if (LRflag)
                LR = 1;
            else
                LR = -1;
            Move();
            //Debug.WriteLine(rad);
            if (hitflag == true)
            {
                count++;
                if (count >= 5)
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
            if (ballState == BallState.Link)
            {
                if (other is Pin)
                {
                    isDeadFlag = true;
                }
            }

            if (ballState == BallState.Free)
            {
                if (other is Enemy)
                {
                    isDeadFlag = true;
                }
                if (other is Pin || other is Collider)
                {
                    sound.PlaySE("reflect");
                    if (Yflag == false && Xflag == true && hitflag == false)
                    {

                        hitflag = true;
                        rad *= -1;
                        Xflag = true;
                        Yflag = true;
                        effectpos = position;
                        effectfrag = true;
                    }
                    if (yflag == false && Yflag == true && Xflag == false && hitflag == false)
                    {
                        yflag = true;
                        hitflag = true;
                        rad *= -1;
                        Yflag = true;
                        Xflag = true;
                        effectpos = position;
                        effectfrag = true;
                    }
                    if (yflag == true && Yflag == true && Xflag == false && hitflag == false)
                    {
                        yflag = false;
                        hitflag = true;
                        rad *= -1;
                        Yflag = true;
                        Xflag = true;
                        effectpos = position;
                        effectfrag = true;
                    }
                }

            }

            //  hitflag = true;
        }

        public override void Move()
        {
            if (GamePlay.timeflag == true && GamePlay.startflag == true)
            {
                if (ballState == BallState.Start)
                {
                    position += new Vector2(0, -1) * (startspeed / 10);
                    nowVector = new Vector2(0, -1) * (startspeed / 10);
                }
                if(position.Y < 0 || position.Y + pixelSize >= Screen.Width)
                {
                    startspeed *= -1;
                }
                if (ballState == BallState.Free && !IsDead()) //移動可能なら
                {
                    if (freeFlag)

                    {
                        if(position.X < 0 || position.X + pixelSize >= Screen.Width ||
                            position.Y < 0 || position.Y + pixelSize >= Screen.Width)
                        {
                            effectpos = position;
                            rad *= -1;
                            effectfrag = true;
                            sound.PlaySE("reflect");
                        }
                        //まっすぐに移動
                        if (position.X + pixelSize /2 < 0 || position.X + pixelSize / 2 >= Screen.Width ||
                            position.Y + pixelSize / 2 < 0 || position.Y + pixelSize / 2 >= Screen.Width)
                        {
                            isDeadFlag = true;
                        }
                        if (position.X < 0)
                        {
                            position.X += pixelSize / 4;
                        }
                        if (position.X + pixelSize >= Screen.Width)
                        {
                            position.X -= pixelSize / 4;
                        }
                        if (position.Y < 0 && yflag == false && hitflag == false)
                        {
                            position.Y += pixelSize / 4;
                            hitflag = true;
                            yflag = true;
                        }
                        if(position.Y + pixelSize >= Screen.Height && yflag == false && hitflag == false)
                        {
                            position.Y -= pixelSize / 4;
                            hitflag = true;
                            yflag = true;
                        }
                        if (position.Y < 0 && yflag == true && hitflag == false)
                        {
                            position.Y += pixelSize / 4;
                            hitflag = true;
                            yflag = false;
                        }
                        if (position.Y + pixelSize >= Screen.Height && yflag == true && hitflag == false)
                        {
                            position.Y -= pixelSize / 4;
                            hitflag = true;
                            yflag = false;
                        }
                        if (yflag == true)
                        {
                            position.X += ((float)Math.Cos(rad + MathHelper.ToRadians(270)) * speed) * -LR;
                            position.Y += ((float)Math.Sin(rad + MathHelper.ToRadians(270)) * speed) * -LR;
                            nowVector = new Vector2(((float)Math.Cos(rad + MathHelper.ToRadians(270)) * speed) * -LR,
                                ((float)Math.Sin(rad + MathHelper.ToRadians(270)) * speed) * -LR);
                        }
                        if (yflag == false)
                        {
                            position.X += ((float)Math.Cos(rad + MathHelper.ToRadians(90)) * speed) * -LR;
                            position.Y += ((float)Math.Sin(rad + MathHelper.ToRadians(90)) * speed) * -LR;
                            nowVector = new Vector2(((float)Math.Cos(rad + MathHelper.ToRadians(90)) * speed) * -LR,
                                ((float)Math.Sin(rad + MathHelper.ToRadians(90)) * speed) * -LR);

                        }
                        ang = 0;
                    }
                }
                if (ballState == BallState.Link && !IsDead())
                {
                    yflag = false;
                    Xflag = true;
                    Yflag = true;
                    //AddActor(new RayLine("particleSmall", position, pPosition));
                    position = pPosition + new Vector2((float)Math.Cos(ang + MathHelper.ToRadians(angle)), (float)Math.Sin(ang + MathHelper.ToRadians(angle))) * radius;
                    //Debug.WriteLine(MathHelper.ToDegrees(ang));

                }

                if (Input.GetKeyRelease(Keys.Enter)) //キーが離されたら
                {

                    rad = 0; //角度初期化

                    rad = GetRadian(position, pPosition); //ベクトルの角度を取得

                    ballState = BallState.Free; //移動可能
                }
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

        public Vector2 GetVector()
        {
            nowVector.Normalize();
            return nowVector;
        }

        public override void Draw(Renderer renderer)
        {
            if (ballState == BallState.Link && Lhitflag == true)
            {
                renderer.DrawLine(new Vector2(position.X + 32, position.Y + 32), new Vector2(pPosition.X + 32, pPosition.Y + 32), Color.Red);
                if (Lhitflag && IsDead())
                {
                    renderer.DrawLine(new Vector2(position.X + 32, position.Y + 32), new Vector2(pPosition.X + 32, pPosition.Y + 32), Color.Red);
                }
            }
            if (!isDeadFlag)
                if (ballState == BallState.Link)
                {
                    renderer.DrawTexture(name, position, null, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0, 1);
                    renderer.DrawLine(new Vector2(position.X + 32, position.Y + 32), new Vector2(pPosition.X + 32, pPosition.Y + 32), Color.White);
                }

                else
                {
                    renderer.DrawTexture("Player2", position, null, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0, 1);
                }
            if (effect <= 0)
            {
                effect = 1;
                effectfrag = false;
            }
            else
            {
                effect -= 0.01f;
            }
            if (effectfrag == true)
            {
                renderer.DrawTexture("Playerwaku1", new Vector2(effectpos.X + 32 - (32 * (1.5f - effect)), effectpos.Y + 32 - (32 * (1.5f - effect))), null, Color.White * effect, 0.0f, new Vector2(64 / 1280f, 64 / 1280f),
        new Vector2((1 * (1.5f - effect)), (1 * (1.5f - effect))));
            }
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
            if (character is RayLine)
                mediator.AddActor((RayLine)character);
        }

        public void AddCollider(Collider collider, int pinNum)
        {
            mediator.AddCollider(collider, pinNum);
        }
    }
}
