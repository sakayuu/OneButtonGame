using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBG.Device
{
    public class MathCollision
    {

        /// <summary>
        /// 円と線の衝突判定（境界円の戻りあり）
        /// </summary>
        /// <param name="p1">線分始点</param>
        /// <param name="p2">線分終点</param>
        /// <param name="pc">円の中心</param>
        /// <param name="r">円の半径</param>
        public static bool Circle_Segment(
            ref Vector2 pc,
            float r,
            Vector2 p1,
            Vector2 p2)
        {
            Vector2 v = p2 - p1;
            float r2 = r * r;
            //p1の外側にあって境界線より中心までの距離が半径以下か？
            Vector2 v1 = pc - p1; //中心からp1までのベクトル

            //中心から線分に下した点とp1までの距離の比
            float t = Vector2.Dot(v, v1) / Vector2.Dot(v, v);
            if ((t < 0) && (v1.LengthSquared() <= r2))
            {
                pc = p1 + r * Vector2.Normalize(v1); //中心を移動
                return true;
            }

            //p2の外側にあって境界円より中心までの距離が半径以下か？
            Vector2 v2 = pc - p2; //中心からp2までのベクトル
            if ((1 < t) && (v2.LengthSquared() <= r2))
            {
                pc = p2 + r * Vector2.Normalize(v2);//中心を移動
                return true;
            }

            //境界円の中心がp1p2の間にあって線分に接触している場合
            Vector2 vn = v1 - v * t; //法線方向のベクトル
            //p1p2線分の間
            if ((0 <= t) && (t <= 1) && (vn.LengthSquared() <= r2))
            {
                pc = p1 + v * t + r * Vector2.Normalize(vn); //中心点を移動
                return true;
            }
            return false;
        }

        /// <summary>
        /// 円と線の衝突（判定のみで、協会円の戻りなし）
        /// </summary>
        /// <param name="p1">線分視点</param>
        /// <param name="p2">線分終点</param>
        /// <param name="pc">境界円中心</param>
        /// <param name="r">境界円半径</param>
        /// <returns></returns>
        public static bool Circle_Segment(Vector2 pc, float r, Vector2 p1, Vector2 p2)
        {
            float r2 = r * r;
            Vector2 v = p2 - p1;
            Vector2 v1 = pc - p1;
            float t = Vector2.Dot(v, v1) / Vector2.Dot(v, v);

            //p1の外側で円の半径内
            if (t < 0 && v1.LengthSquared() <= r2) return true;

            //p2の外側で円の半径内
            Vector2 v2 = pc - p2;
            if (1 < t && v2.LengthSquared() <= r2) return true;
            //p1p2間で円の半径内
            Vector2 vn = v1 - t * v; //線分の法線ベクトル
            if (0 < t && t < 1 && vn.LengthSquared() <= r2) return true;
            return false;
        }


        /// <summary>
        /// 円とレイの衝突
        /// </summary>
        /// <param name="p0">円の中心</param>
        /// <param name="radius">円の半径</param>
        /// <param name="p1">レイの始点</param>
        /// <param name="p2">レイの終点</param>
        /// <param name="Intersection">交差点</param>
        /// <returns></returns>
        public static bool Circle_Ray(Vector2 p0,
            float radius,
            Vector2 p1,
            Vector2 p2/*,
            ref Vector2 Intersection*/)
        {
            Vector2 v = Vector2.Normalize(p2 - p1); //レイの進行ベクトル
            Vector2 v1 = p1 - p0; //レイの始点から境界円の中心までのベクトル
            float b = Vector2.Dot(v, v1); //レイの方向ベクトルと内積をとる
            float r2 = radius * radius; //半径の2乗
            float L2 = v1.LengthSquared(); //p1から中心までの距離の2乗
            float D = b * b - L2 + r2; //√の中が衝突判定条件となる:判別式
            float DS = (float)Math.Sqrt(D); //解の公式の√の部分
            if (D >= 0)
            {
                float t = -b - DS;
                if (t < 0) //レイの始点位置の判定
                {
                    if (L2 < r2) //中心までの距離の2乗と判定の2乗を比較
                    {
                        t = -b + DS; //反対側の交点にする
                    }
                    else
                        return false;
                }
                //Intersection = p1 + t * v;
                return true;
            }
            return false;
        }
    }
}
