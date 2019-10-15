using OBG.Device;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OBG.Scene;
using System.Diagnostics;

namespace OBG.Actor
{
    class Pin : Character, IGameMediator
    {
        public Vector2 catchPos; //プレイヤーが回り始める位置

        float angle = 0;
        public float radius; //プレイヤーとピンの半径

        public bool catchFlag;

        int LR;

        public bool LRflag;

        float speed = 10;

        int x = 1;

        IGameMediator mediator;

        public Vector2 bPos;

        public Pin(string name, Vector2 position, IGameMediator mediator)
        {
            radius = 0;
            this.position = position;
            this.name = name;
            pixelSize = 64;
            this.mediator = mediator;
            angle = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (LRflag)
                LR = 1;
            else
                LR = -1;
            if (catchFlag)
            {
                angle += (speed / 10) * LR;
                if (Math.Abs(angle / 360) >= 1 && Math.Abs(angle / 360) < 2)
                {
                    Collider collider = new Collider(position, radius);
                    AddActor(collider);
                }
            }
            else
                angle = 0;
        }

        public override Vector2 GetPosition()
        {
            return base.GetPosition();
        }
        
        public Vector2 SetCatchPos(Vector2 pos)
        {
            catchPos = pos;
            return catchPos;
        }

        public override void Initialize()
        {
            LR = 0;
        }

        public override void Shutdown()
        {

        }

        public override void Hit(Character other)
        {

        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
        }

        public void AddActor(Character character)
        {
            if (character is Collider)
                mediator.AddActor((Collider)character);
            else if (character is RayLine)
                mediator.AddActor((RayLine)character);
        }

        public void GetBPos(Vector2 pos)
        {
            bPos = pos;
        }

        public float SetAngle()
        {
            return angle;
        }
    }
}
