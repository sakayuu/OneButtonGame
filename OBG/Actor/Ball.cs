using Microsoft.Xna.Framework;
using OBG.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            if (moveFlag)
            {
                //var x = (distance - position) / 2;
                //position += x * speed;
                position.Y -= 1;

            }
            else
            {
                position = catchPos;
                catchPos = new Vector2(pPosition.X + (float)Math.Cos(angle) * radius, pPosition.Y + (float)Math.Sin(angle) * radius);
                angle += 0.1f * LR;
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
    }
}
