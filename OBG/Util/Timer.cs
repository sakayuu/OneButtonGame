using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBG.Util
{
    abstract class Timer
    {
        protected float limitTime; //制限時間
        protected float currentTime; //現在時間

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="second">制限時間</param>
        public Timer(float second)
        {
            limitTime = 60 * second; //60fpsと仮定
        }

        ///<summary>
        ///デフォルトコンストラクタ
        ///</summary>
        public Timer()
            : this(1) //デフォルトは1秒
        { }

        //抽象メソッド
        public abstract void Initialize();

        public abstract void Update(GameTime gameTime);

        public abstract bool IsTime();

        public abstract float Rate();

        ///<summary>
        ///制限時間を設定
        ///</summary>
        ///<param name="second"></param>
        public void SetTime(float second)
        {
            limitTime = 60 * second;
        }

        ///<summary>
        ///現在時間の取得
        ///</summary>
        ///<returns>秒</returns>
        public float Now()
        {
            return currentTime / 60;///60fps想定なので60で割る
        }

    }
}
