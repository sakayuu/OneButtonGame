using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using OBG.Device;

namespace OBG.Actor
{
    class RayLine : Character
    {
        private Vector2 distance;

        public RayLine(string name, Vector2 ball, Vector2 pin)
        {
            this.name = name;
            position = ball + new Vector2(32, 32);
            distance = pin + new Vector2(32, 32);
        }


        public override void Hit(Character other)
        {
            if (other is Pin)
            {
                isDeadFlag = true;
            }
        }

        public override void Initialize()
        {

        }

        public override void Shutdown()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            var velocity = (distance - position);
            float leng = (distance - position).Length();
            velocity.Normalize();

            position += velocity * 30;
            if ((distance - position).Length() < 128)
            {
                isDeadFlag = true;
            }


        }

        public override void Draw(Renderer renderer)
        {
            //renderer.DrawTexture(name,position,null,Color.White,0.0f,Vector2.Zero,new Vector2())
            base.Draw(renderer);
        }

        public void SetMyPosition(Vector2 pos)
        {
            position = pos;
        }

        
    }
}
