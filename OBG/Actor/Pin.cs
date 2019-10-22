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

        Collider collider;

        public int pinNum;

        public Pin(string name, Vector2 position, int pinNum, IGameMediator mediator)
        {
            radius = 0;
            this.position = position;
            this.name = name;
            pixelSize = 64;
            this.mediator = mediator;
            angle = 0;
            collider = new Collider(position, 0);
            this.pinNum = pinNum;
        }

        public override void Update(GameTime gameTime)
        {
            if (LRflag)
                LR = 1;
            else
                LR = -1;
            if (catchFlag)
            {
                angle += speed / (radius / 100) * LR;
                if (Math.Abs(angle / 360) >= 1 && Math.Abs(angle / 360) < 1.1f)
                {
                    //Collider collider = new Collider(position, (radius * 2) - 80);
                    collider.SetPixelSize(radius * 2 - 80);
                    AddCollider(collider,pinNum);
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
            if (character is RayLine)
                mediator.AddActor((RayLine)character);
        }

        public void AddCollider(Collider collider, int pinNum)
        {
            mediator.AddCollider(collider,pinNum);
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
