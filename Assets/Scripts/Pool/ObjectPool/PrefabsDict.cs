using UnityEngine;
using System.Collections.Generic;

namespace PathologicalGames
{
    /// <summary>
    /// 预制体字典
    /// </summary>
    public class PrefabsDict : IDictionary<string, Transform>
    {
        #region 公有的自定义方法
        /// <summary>
        /// 重写字典的Tostring（），显示所有预制体的名称
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // 通过join函数格式化元素为key的数组
            var keysArray = new string[this._prefabs.Count];
            this._prefabs.Keys.CopyTo(keysArray, 0);

            //，作为分隔符
            return string.Format("[{0}]", System.String.Join(", ", keysArray));
        }
        #endregion Public Custom Memebers


        #region 内部字典功能
        // 内部的添加和移除
        internal void _Add(string prefabName, Transform prefab)
        {
            this._prefabs.Add(prefabName, prefab);
        }

        internal bool _Remove(string prefabName)
        {
            return this._prefabs.Remove(prefabName);
        }

        internal void _Clear()
        {
            this._prefabs.Clear();
        }
        #endregion Internal Dict Functionality


        #region 字典功能
        /// <summary>
        /// 预制体字典
        /// </summary>
        private readonly Dictionary<string, Transform> _prefabs = new Dictionary<string, Transform>();

        /// <summary>
        /// 获取字典中预制体的数量
        /// </summary>
        public int Count { get { return this._prefabs.Count; } }

        /// <summary>
        /// 通过字符串，比较是否存在预制体
        /// </summary>
        /// <param name="prefabName">The name to look for</param>
        /// <returns>True if the prefab exists, otherwise, false.</returns>
        public bool ContainsKey(string prefabName)
        {
            return this._prefabs.ContainsKey(prefabName);
        }

        /// <summary>
        /// 获取字符串对应的预制体
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string prefabName, out Transform prefab)
        {
            return this._prefabs.TryGetValue(prefabName, out prefab);
        }

        #region Not Implimented

        public void Add(string key, Transform value)
        {
            throw new System.NotImplementedException("Read-Only");
        }

        public bool Remove(string prefabName)
        {
            throw new System.NotImplementedException("Read-Only");
        }

        public bool Contains(KeyValuePair<string, Transform> item)
        {
            string msg = "Use Contains(string prefabName) instead.";
            throw new System.NotImplementedException(msg);
        }

        public Transform this[string key]
        {
            get
            {
                Transform prefab;
                try
                {
                    prefab = this._prefabs[key];
                }
                catch (KeyNotFoundException)
                {
                    string msg = string.Format("A Prefab with the name '{0}' not found. " +
                                                "\nPrefabs={1}",
                                                key, this.ToString());
                    throw new KeyNotFoundException(msg);
                }

                return prefab;
            }
            set
            {
                throw new System.NotImplementedException("Read-only.");
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                return this._prefabs.Keys;
            }
        }


        public ICollection<Transform> Values
        {
            get
            {
                return this._prefabs.Values;
            }
        }


        #region ICollection<KeyValuePair<string, Transform>> Members
        private bool IsReadOnly { get { return true; } }
        bool ICollection<KeyValuePair<string, Transform>>.IsReadOnly { get { return true; } }

        public void Add(KeyValuePair<string, Transform> item)
        {
            throw new System.NotImplementedException("Read-only");
        }

        public void Clear() { throw new System.NotImplementedException(); }

        private void CopyTo(KeyValuePair<string, Transform>[] array, int arrayIndex)
        {
            string msg = "Cannot be copied";
            throw new System.NotImplementedException(msg);
        }

        void ICollection<KeyValuePair<string, Transform>>.CopyTo(KeyValuePair<string, Transform>[] array, int arrayIndex)
        {
            string msg = "Cannot be copied";
            throw new System.NotImplementedException(msg);
        }

        public bool Remove(KeyValuePair<string, Transform> item)
        {
            throw new System.NotImplementedException("Read-only");
        }
        #endregion ICollection<KeyValuePair<string, Transform>> Members
        #endregion Not Implimented




        #region IEnumerable<KeyValuePair<string, Transform>> Members
        public IEnumerator<KeyValuePair<string, Transform>> GetEnumerator()
        {
            return this._prefabs.GetEnumerator();
        }
        #endregion



        #region IEnumerable Members
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this._prefabs.GetEnumerator();
        }
        #endregion

        #endregion Dict Functionality

    }
}
