using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using OBG.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OBG.Actor
{
    class Ball : Character
    {
        private float speed = 10f; //スピード
        private Vector2 distance; //目的地
        private Vector2 velocity; //移動量

        public bool moveFlag = false; //動くかどうか
        public Vector2 catchPos;

        public float radius; //プレイヤーとピンの間の距離（プレイヤーがピンを回る円における半径）
        public float angle; //回転

        public bool LRflag; //ピンに対してプレイヤーが左にいるか右にいるか
        public bool UDflag; //ピンに対してプレイヤーが上にいるか下にいるか

        int LR; //右回転か左回転かを制御

        public Vector2 pPosition;

        float rad = 0;

        int x = 1;

        public bool gameStartFlag;

        public Ball(string name, Vector2 position)
        {
            this.position = position;
            this.name = name;
            pixelSize = 64;
        }

        public override void Initialize()
        {
            isDeadFlag = false;
            radius = 0;
            angle = 0;
            LR = 0;
            LRflag = false;
            x = 1;
            gameStartFlag = false;
        }

        public override void Update(GameTime gameTime)
        {
            Move();
        }

        public override void Shutdown()
        {

        }

        public override void Hit(Character other)
        {
            if (other is Pin)
                isDeadFlag = true;
        }

        public override void Move()
        {
            if (LRflag)
                LR = 1;
            else
                LR = -1;

            if (moveFlag) //移動可能なら
            {
                //まっすぐに移動
                position.X += ((float)Math.Cos(rad + MathHelper.ToRadians(90)) * speed) * -LR;
                position.Y += ((float)Math.Sin(rad + MathHelper.ToRadians(90)) * speed) * -LR;
            }
            else if (gameStartFlag)
            {
                position = catchPos; //自身の位置をピンの周りを回転する位置に
                catchPos = new Vector2(pPosition.X + (float)Math.Cos(angle) * radius, pPosition.Y + (float)Math.Sin(angle) * radius);
                angle = angle + (speed / 100) * LR;
                if (Math.Abs(angle / MathHelper.ToRadians(360)) >= x)
                {
                    radius = radius + radius / 2;
                    x++;
                }
            }

            if (Input.GetKeyRelease(Keys.P)) //キーが離されたら
            {
                angle = 0;
                rad = 0; //角度初期化
                velocity = (pPosition - position); //ベクトル取得
                rad = GetRadian(position, pPosition); //ベクトルの角度を取得
                velocity.Normalize(); //ベクトルの正規化

                moveFlag = true; //移動可能
            }

        }

        /// <summary>
        /// 目的地を入れる
        /// </summary>
        /// <param name="hisPos"></param>
        /// <returns></returns>
        public Vector2 GetMoveDirection(Vector2 hisPos)
        {
            distance = hisPos;
            return distance;
        }

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
            float w = v2.X - v1.X;
            float h = v2.Y - v1.Y;
            //if (w != 0)
            //{
            //    float t = h / w;
            //    if (v1.X < v2.X)
            //    {
            //        return (float)Math.Atan(t);
            //    }
            //    return (float)Math.Atan(t) + (float)Math.PI;
            //}

            //if (v1.Y < v2.Y)
            //{
            //    return (float)Math.PI / 2;
            //}
            //return -(float)Math.PI / 2;
            return (float)Math.Atan2(h, w);
        }
    }
}
