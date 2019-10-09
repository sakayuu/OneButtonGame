using OBG.Device;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBG
{
    class Pin
    {
        Vector2 potision;
        Vector2 catchPos;

        float m_PosX;       // 描画座標X
        float m_PosY;       // 描画座標Y
        float m_Radius;     // 半径(描画用)
        float m_CenterX;    // 中心座標X
        float m_CenterY;    // 中心座標Y
        float m_Angle;      // 角度
        float m_Length;     // 半径の長さ

        float angle = 0;
        float radius;
        float hankei;

        public Pin(Vector2 potision)
        {
            hankei = 0;
            this.potision = potision;
            catchPos = potision + new Vector2(hankei, 0);


        }

        public void Update(GameTime gameTime)
        {
            catchPos = new Vector2(potision.X + (float)Math.Cos(angle) * hankei, potision.Y + (float)Math.Sin(angle) * hankei);
            angle += 0.1f;
            if (Input.GetKeyTrigger(Microsoft.Xna.Framework.Input.Keys.E))
            {
                hankei += 20;
            }
        }


        //public Vector2 RotationCatchPos()
        //{
        //    float add_x = Math.Cos()
        //}

        public Vector2 GetPinPosition()
        {
            return potision;
        }

        public Vector2 GetCatchPos()
        {
            return catchPos;
        }

        private double CheckDistance(Vector2 bPos, Vector2 pPos)
        {
            double dist = Math.Sqrt((pPos.X - bPos.X) * (pPos.X - bPos.X) + (pPos.Y - bPos.Y) * (pPos.Y - bPos.Y));
            return dist;
        }
    }
}
