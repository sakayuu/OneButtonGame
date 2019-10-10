using OBG.Device;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBG.Actor
{
    class Pin : Character
    {
        Vector2 catchPos; //プレイヤーが回り始める位置

        float angle = 0;
        float radius; //プレイヤーとピンの半径

        public Pin(Vector2 position)
        {
            radius = 0;
            this.position = position;
            catchPos = position + new Vector2(radius, 0);


        }

        public override void Update(GameTime gameTime)
        {
            if(Input.GetKeyTrigger(Microsoft.Xna.Framework.Input.Keys.P))
            {

            }


            if (Input.GetKeyState(Microsoft.Xna.Framework.Input.Keys.P))
            {
                catchPos = new Vector2(position.X + (float)Math.Cos(angle) * radius, position.Y + (float)Math.Sin(angle) * radius);
                angle += 0.1f;

            }
            else
            {

            }
            if (Input.GetKeyTrigger(Microsoft.Xna.Framework.Input.Keys.E))
            {
                radius += 20;
            }
        }


        //public Vector2 RotationCatchPos()
        //{
        //    float add_x = Math.Cos()
        //}

        public override Vector2 GetPosition()
        {
            return base.GetPosition();
        }

        public Vector2 GetCatchPos()
        {
            return catchPos;
        }

        private double CheckDistance(Vector2 bPos, Vector2 pPos)
        {
            double dist = Math.Sqrt((pPos.X - bPos.X) * (pPos.X - bPos.X) + (pPos.Y - bPos.Y) * (pPos.Y - bPos.Y));
            return dist;
        }

        public override void Initialize()
        {

        }

        public override void Shutdown()
        {

        }

        public override void Hit(Character other)
        {

        }
    }
}
