using Microsoft.Xna.Framework;
using OBG.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBG.Actor
{
    abstract class Character
    {
        protected string name; //画像の名前
        protected Vector2 position; //自分の位置
        protected bool isDeadFlag; //死亡フラグ
        protected float pixelSize; //画像の大きさ（衝突判定で使用）
        protected bool Xflag = false;
        protected bool Yflag = false;


        /// <summary>
        /// 衝突判定（2点間の距離と円の半径）
        /// </summary>
        /// <param name="other">相手オブジェクト</param>
        /// <returns></returns>
        public bool IsCollision(Character other)
        {
            //自分と相手の位置の長さを計算（2点間の距離）
            float length = (position - other.position).Length();
            float lengthx = Math.Abs(position.X - other.position.X);
            float lengthy = Math.Abs(position.Y - other.position.Y);
            
            //画像のサイズにより変化
            //自分半径と相手半径の和
            float radiusSum = (pixelSize / 2) + (other.pixelSize / 2);
            //半径の和と距離を比べて、等しいかまたは小さいか（以下か）
            
            if(length <= radiusSum ) 
            {
                float ra = Math.Abs(radiusSum + 1 - length);
                Xflag = false;
                Yflag = false;
                //if (lengthx == lengthy)
                //{
                //    position.X *= ra;
                //    position.Y *= ra;
                //    return true;
                //}
                if (lengthx >= lengthy)
                {
                    if(position.X <= other.position.X)
                    {
                        position.X -= ra;
                    }
                    if(position.X >= other.position.X)
                    {
                        position.X += ra;
                    }
                    
                    Xflag = true;
                    return true;
                }
                if (lengthx <= lengthy)
                {
                    if(position.Y <= other.position.Y)
                    {
                        position.Y -= ra;
                    }
                    if(position.Y >= other.position.Y)
                    {
                        position.Y += ra;
                    }
                    Yflag = true;
                    return true;
                }

            }
            
            return false;
        }

        public abstract void Initialize();//初期化
        public abstract void Update(GameTime gameTime);//更新
        public abstract void Shutdown();//終了
        public abstract void Hit(Character other);//ヒット通知

        /// <summary>
        /// 死んでいるか？
        /// </summary>
        /// <returns></returns>
        public bool IsDead()
        {
            return isDeadFlag;
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public virtual void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position);
        }

        /// <summary>
        /// 位置を渡す（そのまま）
        /// </summary>
        /// <returns></returns>
        public virtual Vector2 GetPosition()
        {
            return position;
        }

        /// <summary>
        /// 位置の受け渡し
        /// （引数で渡された変数に自分の位置を渡す）
        /// </summary>
        /// <param name="other">位置を送りたい相手</param>
        public void SetPosition(ref Vector2 other)
        {
            other = position;
        }

        public virtual void Move()
        {

        }
    }
}
