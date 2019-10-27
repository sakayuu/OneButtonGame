using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using OBG.Device;

namespace OBG.Actor
{
    class Collider : Character
    {
        public BallState ballState;
        public bool alphaFlag;
        private float effect;
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
            alphaFlag = true;
        }

        public override void Shutdown()
        {

        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(Renderer renderer)
        {
            if (ballState == BallState.Free)
            {
                renderer.DrawTexture("kiiro", new Vector2(position.X + (-pixelSize / 2 + 32),
                    position.Y + (-pixelSize / 2 + 32)), null,new Color(14,104,146) * 0.5f, 0.0f, Vector2.Zero,
                    new Vector2(pixelSize / 1280, pixelSize / 1280));
                if (effect <= 0)
                {
                    effect = 1;
                }
                else
                {
                    effect -= 0.01f;
                }
                renderer.DrawTexture("Playerwaku1", new Vector2(position.X + 32 - ((pixelSize / 2) * (2f - effect)), position.Y + 32 - ((pixelSize / 2) * (2f - effect))), null, Color.White * effect, 0.0f, new Vector2(1f, 1f),
                        new Vector2((pixelSize / 64 * (2f - effect)), (pixelSize / 64 * (2f - effect))));
            }

            else
                renderer.DrawTexture("kiiro", new Vector2(position.X + (-pixelSize / 2 + 32),
                    position.Y + (-pixelSize / 2 + 32)), null, Color.DarkRed * 0.5f, 0.0f, Vector2.Zero,
                    new Vector2(pixelSize / 1280, pixelSize / 1280));
            
            //Debug.WriteLine(ballState);

        }

        public void GetBallState(BallState ballState)
        {
            this.ballState = ballState;
        }

        public void SetPixelSize(float pxSize)
        {
            pixelSize = pxSize;
        }
    }
}
