using Microsoft.Xna.Framework;
using OBG.Device;
using OBG.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBG.Actor
{
    class DeathEffect : Character
    {
        Timer timer;
        Random rnd = new Random();
        int sacle;
        public DeathEffect(string name, Vector2 position)
        {
            Initialize();
            this.name = name;
            this.position = position;
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position, null, Color.White * 1.0f, 0, Vector2.Zero, new Vector2(sacle/100, sacle/100));
        }

        public override void Hit(Character other)
        {

        }

        public override void Initialize()
        {
            timer = new CountDownTimer(0.5f);
        }

        public override void Shutdown()
        {

        }

        public override void Update(GameTime gameTime)
        {
            position += new Vector2(rnd.Next(-10, 10), rnd.Next(-10, 10));
            sacle = rnd.Next(150, 200);
            timer.Update(gameTime);
            if (timer.IsTime())
            {
                isDeadFlag = true;
            }
        }
    }
}
