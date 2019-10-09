using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBG
{
    class Ball
    {
        private Vector2 position; //自分の位置
        private Vector2 distance; //目的地
        private Vector2 velocity; //移動量
        private float speed = 0.1f; //スピード
        public bool moveFlag = false; //動くかどうか

        public Ball(Vector2 position)
        {
            this.position = position;
        }

        public void Update(GameTime gameTime)
        {
            Move();
        }

        public void Move()
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

        public Vector2 GetBallPos()
        {
            return position;
        }

        public void SetBallPos(Vector2 pos)
        {
            position = pos;
        }
    }
}
