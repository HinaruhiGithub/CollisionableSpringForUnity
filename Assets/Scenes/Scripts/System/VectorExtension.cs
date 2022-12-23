using UnityEngine;

namespace Scenes.Scripts.System
{
    public static class VectorExtension
    {
        /// <summary>
        /// あるベクトルとの内積と、その方向のスカラにより成り立つベクトルを知る
        /// </summary>
        /// <param name="myVector"></param>
        /// <param name="needVector"></param>
        /// <returns></returns>
        public static Vector2 GetInnerProductWithDirection(
            this Vector2 myVector,
            Vector2 needVector
        )
        {
            var magnitude = Vector2.Dot(needVector, myVector);
            var direction = needVector.normalized;
            return magnitude * direction;
        }
        
        /// <summary>
        /// 2Dでdegreeで指定したい、あるベクトルとの内積と、そのスカラにより成り立つベクトル
        /// </summary>
        /// <param name="myVector"></param>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static Vector2 GetInnerProductWithDirection2D(
            this Vector2 myVector,
            float degree
        )
        {
            var needVector = 
                Quaternion.Euler(0, 0, degree) * Vector2.right;
            return GetInnerProductWithDirection(myVector, needVector);
        }
    }
}