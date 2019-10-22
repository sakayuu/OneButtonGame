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
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(Renderer renderer)
        {
            if (ballState == BallState.Free)
                renderer.DrawTexture("kiiro", new Vector2(position.X + (-pixelSize / 2 + 32), position.Y + (-pixelSize / 2 + 32)), null, 0.0f, Vector2.Zero, new Vector2(pixelSize / 1280, pixelSize / 1280));
            else
                renderer.DrawTexture("kiiro", new Vector2(position.X + (-pixelSize / 2 + 32),
                    position.Y + (-pixelSize / 2 + 32)), null, Color.DarkRed * 0.5f, 0.0f, Vector2.Zero,
                    new Vector2(pixelSize / 1280, pixelSize / 1280));
            //描画範囲をレクタングルで指定して横幅ずつずらして描画


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
