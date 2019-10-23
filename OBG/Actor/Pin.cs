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

        public float field;

        public float effect;
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
            effect = 1;
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
                    AddCollider(collider, pinNum);
                    field = radius * radius * (float)Math.PI;
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
            if(effect<=0)
            {
                effect = 1;
            }else
            {
                effect -= 0.01f;
            }
            renderer.DrawTexture("pinwaku1",new Vector2(position.X+32- (32 * (1.5f - effect)), position.Y+32 - (32 * (1.5f - effect))), null, Color.White *effect, 0.0f, new Vector2(1f,1f),
                    new Vector2((1*(1.5f-effect)),(1 * (1.5f - effect))));
        }

        public void AddActor(Character character)
        {
            if (character is RayLine)
                mediator.AddActor((RayLine)character);
        }

        public void AddCollider(Collider collider, int pinNum)
        {
            mediator.AddCollider(collider, pinNum);
        }

        public void GetBPos(Vector2 pos)
        {
            bPos = pos;
        }

        public int GetPinNum()
        {
            return pinNum;
        }

        public float SetAngle()
        {
            return angle;
        }
        public void GetField(float fil)
        {
            field = fil;
        }
        public float SetField()
        {
            return field;
        }
    }
}
