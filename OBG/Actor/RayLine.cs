using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OBG.Device;
using OBG.Util;

namespace OBG.Actor
{
    class RayLine : Character
    {
        private Vector2 distance;
        private Timer timer;
        //private Timer vanishTimer;
        public float deathTime = 1;
        Random rnd = new Random();
        float rndNum = 0;

        public RayLine(string name, Vector2 ball)
        {
            this.name = name;
            Vector2 thisPos = ball + new Vector2(32, 32);
            position = new Vector2(thisPos.X + rnd.Next(-10, 11), thisPos.Y + rnd.Next(-10, 10));
        }



        public override void Hit(Character other)
        {

        }

        public override void Initialize()
        {
            timer = new CountDownTimer(deathTime);
            rndNum = rnd.Next(1, 3);
            isDeadFlag = false;
        }

        public override void Shutdown()
        {

        }

        public override void Update(GameTime gameTime)
        {
            timer.Update(gameTime);
            if (timer.IsTime())
                isDeadFlag = true;

        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position, null, 0.0f, Vector2.Zero, new Vector2(rndNum), SpriteEffects.None, 1, 1 - timer.Rate());
        }

        public void SetMyPosition(Vector2 pos)
        {
            position = pos;
        }


    }
}
