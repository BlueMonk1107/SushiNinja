using UnityEngine;
using System.Collections.Generic;

namespace PathologicalGames
{
    public static class PoolManager
    {
        /// <summary>
        /// 池对象字典（用一个字典(sting，spawnpool)来保存池对象）
        /// 初始化只能放在start里
        /// </summary>
        public static readonly ObjectPoolsDict Pools = new ObjectPoolsDict();
    }


    public static class PoolManagerUtils
    {
        internal static void SetActive(GameObject obj, bool state)
        {
#if (UNITY_3_5 || UNITY_3_4 || UNITY_3_3 || UNITY_3_2 || UNITY_3_1 || UNITY_3_0)
            obj.SetActiveRecursively(state);
#else
        obj.SetActive(state);
#endif
        }

        internal static bool activeInHierarchy(GameObject obj)
        {
#if (UNITY_3_5 || UNITY_3_4 || UNITY_3_3 || UNITY_3_2 || UNITY_3_1 || UNITY_3_0)
            return obj.active;
#else
        return obj.activeInHierarchy;
#endif

        }
    }



}