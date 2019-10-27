using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            renderer.DrawTexture(name, position, null, new Color(255, 255, rnd.Next(0, 255)), 0, Vector2.Zero, new Vector2(rnd.Next(2, 5)), SpriteEffects.None, 0, 1);
        }

        public override void Hit(Character other)
        {

        }

        public override void Initialize()
        {
            timer = new CountDownTimer(1.0f);
        }

        public override void Shutdown()
        {

        }

        public override void Update(GameTime gameTime)
        {
            position += new Vector2(rnd.Next(-10, 10), rnd.Next(-10, 10));
            timer.Update(gameTime);
            if (timer.IsTime())
            {
                isDeadFlag = true;
            }
        }
    }
}
