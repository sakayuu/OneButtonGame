using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using OBG.Device;

namespace OBG.Actor
{
    class Collider : Character
    {
        public Collider(Vector2 position, float pixelSize)
        {
            Initialize();
            this.position = position;
            this.pixelSize = pixelSize;
        }

        public override void Hit(Character other)
        {

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

        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture("kiiro", new Vector2(position.X+(-pixelSize/2+32),position.Y+(-pixelSize / 2 + 32)), null, 0.0f, Vector2.Zero, new Vector2(pixelSize/1280, pixelSize/1280));
        }
    }
}
