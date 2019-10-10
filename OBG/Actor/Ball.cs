using Microsoft.Xna.Framework;
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

        public Ball(Vector2 position)
        {
            this.position = position;
        }



        public override void Move()
        {
            if (moveFlag)
            {
                var x = (distance - position) / 2;
                position += x * speed;
            }
            else
            {

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

        public override void Initialize()
        {
            throw new NotImplementedException();
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
    }
}
