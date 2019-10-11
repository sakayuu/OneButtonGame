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
        private float speed = 0.1f; //スピード
        private Vector2 distance; //目的地
        private Vector2 velocity; //移動量

        public bool moveFlag = false; //動くかどうか
        public Vector2 catchPos;

        public float radius;
        public float angle;

        public bool LRflag;

        int LR;

        public Vector2 pPosition;

        private float moveStartAngle;

        private int xx;

        float X, Y;

        float rad = 0;

        public Ball(string name, Vector2 position)
        {
            this.position = position;
            this.name = name;
        }

        public override void Initialize()
        {
            radius = 0;
            angle = 0;
            LR = 0;
            LRflag = false;
            X = 0;
            Y = 0;
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

        }

        public override void Move()
        {
            if (LRflag)
                LR = 1;
            else
                LR = -1;

            if (Input.GetKeyTrigger(Keys.I))
            {
                rad = 0;
                //var x = (distance - position) / 2;
                //position += x * speed;
                //position += 0.001f * new Vector2(catchPos.X + (float)Math.Cos(moveStartAngle), catchPos.Y + (float)Math.Sin(moveStartAngle));
                velocity = (pPosition - position);

                rad = GetRadian(position, pPosition);

                velocity.Normalize();


                //X = velocity.X + ((float)Math.Cos((90 / 360 * (Math.PI * 2)) * LR));
                //Y = velocity.Y + ((float)Math.Sin((90 / 360 * (Math.PI * 2)) * LR));
                moveFlag = true;
            }

            if (moveFlag)
            {
                position.X += ((float)Math.Cos(rad + MathHelper.ToRadians(90)) * 0.5f) * -LR;
                position.Y += ((float)Math.Sin(rad + MathHelper.ToRadians(90)) * 0.5f) * -LR;

            }
            else
            {
                position = catchPos;
                catchPos = new Vector2(pPosition.X + (float)Math.Cos(angle) * radius, pPosition.Y + (float)Math.Sin(angle) * radius);
                angle += 0.1f * LR;
                moveStartAngle = angle;
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

        public void GetPPos(Vector2 pos)
        {
            pPosition = pos;
        }

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
