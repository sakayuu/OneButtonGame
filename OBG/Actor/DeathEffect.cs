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

        public DeathEffect(string name, Vector2 position)
        {
            Initialize();
            this.name = name;
            this.position = position;
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position);
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
            position += new Vector2(rnd.Next(1, 10), rnd.Next(0, 10));
            timer.Update(gameTime);
            if (timer.IsTime())
            {
                isDeadFlag = true;
            }
        }
    }
}
