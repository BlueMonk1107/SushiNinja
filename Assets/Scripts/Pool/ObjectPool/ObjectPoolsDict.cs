using UnityEngine;
using System.Collections.Generic;

namespace PathologicalGames
{
    /// <summary>
    /// 自定义内存池字典类
    /// </summary>
    public class ObjectPoolsDict : IDictionary<string, ObjectPool>
    {
        #region 事件处理
        /// <summary>
        /// 在创建池对象时，调用的委托
        /// </summary>
        /// <param name="pool"></param>
        public delegate void OnCreatedDelegate(ObjectPool pool);

        /// <summary>
        /// 保存委托的字典
        /// </summary>
        private readonly Dictionary<string, OnCreatedDelegate> onCreatedDelegates = new Dictionary<string, OnCreatedDelegate>();

        public void AddOnCreatedDelegate(string poolName, OnCreatedDelegate createdDelegate)
        {
            // 第一次添加poolName关键字时执行
            if (!this.onCreatedDelegates.ContainsKey(poolName))
            {
                this.onCreatedDelegates.Add(poolName, createdDelegate);
                return;
            }

            this.onCreatedDelegates[poolName] += createdDelegate;
        }

        public void RemoveOnCreatedDelegate(string poolName, OnCreatedDelegate createdDelegate)
        {
            if (!this.onCreatedDelegates.ContainsKey(poolName))
                throw new KeyNotFoundException
                (
                    "没有发现Key：" + poolName + "."
                );

            this.onCreatedDelegates[poolName] -= createdDelegate;
        }

        #endregion Event Handling

        #region 自定义公有成员
        /// <summary>
        /// 创建一个对象池组件
        /// </summary>
        /// <param name="poolName">
        /// The name for the new SpawnPool. The GameObject will have the word "Pool"
        /// Added at the end.
        /// </param>
        /// <returns>A reference to the new SpawnPool component</returns>
        public ObjectPool Create(string poolName)
        {
            //给游戏物体上加pool更容易辨认，在objectPool的awake（）里，创建时就去掉了
            var owner = new GameObject(poolName + "Pool");
            return owner.AddComponent<ObjectPool>();
        }


        /// <summary>
        /// 在指定物体上添加池脚本
        /// </summary>
        /// <param name="poolName">
        /// The name for the new SpawnPool. The GameObject will have the word "Pool"
        /// Added at the end.
        /// </param>
        /// <param name="owner">A GameObject to add the SpawnPool Component</param>
        /// <returns>A reference to the new SpawnPool component</returns>
        public ObjectPool Create(string poolName, GameObject owner)
        {
            if (!this.ValidityOfPoolName(poolName))
                return null;
            
            string ownerName = owner.gameObject.name;

            try
            {
                owner.gameObject.name = poolName;
                
                return owner.AddComponent<ObjectPool>();
            }
            finally
            {
                owner.gameObject.name = ownerName;
            }
        }


        /// <summary>
        /// 在创建任何组件之前，确保名字是有效的
        /// </summary>
        /// <param name="poolName">The name to test</param>
        /// <returns>True if sucessful, false if failed.</returns>
        private bool ValidityOfPoolName(string poolName)
        {
            //去掉名字中的pool
            var tmpPoolName = poolName.Replace("Pool", "");
            //若名字中包含pool发出警告
            if (tmpPoolName != poolName)
            {
                // Log a warning and continue on with the fixed name
                string msg = string.Format("'{0}' 名字中包含pool " +
                       "pool来自游戏物体的默认命名 " +
                       "poolname变更为 '{1}'",
                       poolName, tmpPoolName);

                Debug.LogWarning(msg);
                poolName = tmpPoolName;
            }

            if (this.ContainsKey(poolName))
            {
                Debug.Log(string.Format(" 名为'{0}' 的对象池已经存在",
                                        poolName));
                return false;
            }

            return true;
        }


        /// <summary>
        /// 重写字典的Tostring（），显示所有对象池的名称
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // 通过join函数格式化元素为key的数组
            var keysArray = new string[this._pools.Count];
            this._pools.Keys.CopyTo(keysArray, 0);
            //，作为分隔符
            return string.Format("[{0}]", System.String.Join(", ", keysArray));
        }



        /// <summary>
        /// 无法获取引用时，用字符串销毁对象池
        /// </summary>
        /// <param name="spawnPool"></param>
        public bool Destroy(string poolName)
        {
            ObjectPool objectPool;

            //用TryGetValue比Contains()或访问字典速度更快
            if (!this._pools.TryGetValue(poolName, out objectPool))
            {
                Debug.LogError(
                    string.Format("PoolManager:无法销毁'{0}'. 没有在PoolManager中",
                                  poolName));
                return false;
            }
            else
            {
                Object.Destroy(objectPool.gameObject);

                return true;
            }
            
           
        }

        /// <summary>
        /// 销毁所有对象池
        /// </summary>
        /// <param name="spawnPool"></param>
        public void DestroyAll()
        {
            //销毁所有对象池
            foreach (KeyValuePair<string, ObjectPool> pair in this._pools)
            {
                Object.Destroy(pair.Value);
            }
            //清空数组
            this._pools.Clear();
        }
        #endregion Public Custom Memebers

        #region 字典功能（实现IDictionary接口）
        // 内部字典
        private readonly Dictionary<string, ObjectPool> _pools = new Dictionary<string, ObjectPool>();

        /// <summary>
        /// Used internally by SpawnPools to add themseleves on Awake().
        /// Use PoolManager.CreatePool() to create an entirely new SpawnPool GameObject
        /// </summary>
        /// <param name="spawnPool"></param>
        internal void Add(ObjectPool spawnPool)
        {
            // 避免添加同名池对象
            if (this.ContainsKey(spawnPool.poolName))
            {
                Debug.LogError(string.Format("已经存在一个名为 '{0}' 的pool. " ,
                                             spawnPool.poolName));
                return;
            }

            this._pools.Add(spawnPool.poolName, spawnPool);

            if (this.onCreatedDelegates.ContainsKey(spawnPool.poolName))
                this.onCreatedDelegates[spawnPool.poolName](spawnPool);
        }

        // Keeping here so I remember we have a NotImplimented overload (original signature)
        public void Add(string key, ObjectPool value)
        {
            string msg = "SpawnPools add themselves to PoolManager.Pools when created, so " +
                         "there is no need to Add() them explicitly. Create pools using " +
                         "PoolManager.Pools.Create() or add a SpawnPool component to a " +
                         "GameObject.";
            throw new System.NotImplementedException(msg);
        }


        /// <summary>
        /// Used internally by SpawnPools to remove themseleves on Destroy().
        /// Use PoolManager.Destroy() to destroy an entire SpawnPool GameObject.
        /// </summary>
        /// <param name="spawnPool"></param>
        internal bool Remove(ObjectPool spawnPool)
        {
            if (!this.ContainsKey(spawnPool.poolName))
            {
                Debug.LogError(string.Format("PoolManager: Unable to remove '{0}'. " +
                                                "Pool not in PoolManager",
                                            spawnPool.poolName));
                return false;
            }

            this._pools.Remove(spawnPool.poolName);
            return true;
        }

        // Keeping here so I remember we have a NotImplimented overload (original signature)
        public bool Remove(string poolName)
        {
            string msg = "SpawnPools can only be destroyed, not removed and kept alive" +
                         " outside of PoolManager. There are only 2 legal ways to destroy " +
                         "a SpawnPool: Destroy the GameObject directly, if you have a " +
                         "reference, or use PoolManager.Destroy(string poolName).";
            throw new System.NotImplementedException(msg);
        }

        /// <summary>
        /// Get the number of SpawnPools in PoolManager
        /// </summary>
        public int Count { get { return this._pools.Count; } }

        /// <summary>
        /// Returns true if a pool exists with the passed pool name.
        /// </summary>
        /// <param name="poolName">The name to look for</param>
        /// <returns>True if the pool exists, otherwise, false.</returns>
        public bool ContainsKey(string poolName)
        {
            return this._pools.ContainsKey(poolName);
        }

        /// <summary>
        /// Used to get a SpawnPool when the user is not sure if the pool name is used.
        /// This is faster than checking IsPool(poolName) and then accessing Pools][poolName.]
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string poolName, out ObjectPool spawnPool)
        {
            return this._pools.TryGetValue(poolName, out spawnPool);
        }



        #region Not Implimented
        public bool Contains(KeyValuePair<string, ObjectPool> item)
        {
            string msg = "Use PoolManager.Pools.Contains(string poolName) instead.";
            throw new System.NotImplementedException(msg);
        }

        public ObjectPool this[string key]
        {
            get
            {
                ObjectPool pool;
                try
                {
                    pool = this._pools[key];
                }
                catch (KeyNotFoundException)
                {
                    string msg = string.Format("A Pool with the name '{0}' not found. " +
                                                "\nPools={1}",
                                                key, this.ToString());
                    throw new KeyNotFoundException(msg);
                }

                return pool;
            }
            set
            {
                string msg = "Cannot set PoolManager.Pools[key] directly. " +
                    "SpawnPools add themselves to PoolManager.Pools when created, so " +
                    "there is no need to set them explicitly. Create pools using " +
                    "PoolManager.Pools.Create() or add a SpawnPool component to a " +
                    "GameObject.";
                throw new System.NotImplementedException(msg);
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                string msg = "If you need this, please request it.";
                throw new System.NotImplementedException(msg);
            }
        }


        public ICollection<ObjectPool> Values
        {
            get
            {
                string msg = "If you need this, please request it.";
                throw new System.NotImplementedException(msg);
            }
        }


        #region ICollection<KeyValuePair<string,SpawnPool>> Members
        private bool IsReadOnly { get { return true; } }
        bool ICollection<KeyValuePair<string, ObjectPool>>.IsReadOnly { get { return true; } }

        public void Add(KeyValuePair<string, ObjectPool> item)
        {
            string msg = "SpawnPools add themselves to PoolManager.Pools when created, so " +
                         "there is no need to Add() them explicitly. Create pools using " +
                         "PoolManager.Pools.Create() or add a SpawnPool component to a " +
                         "GameObject.";
            throw new System.NotImplementedException(msg);
        }

        public void Clear()
        {
            string msg = "Use PoolManager.Pools.DestroyAll() instead.";
            throw new System.NotImplementedException(msg);

        }

        private void CopyTo(KeyValuePair<string, ObjectPool>[] array, int arrayIndex)
        {
            string msg = "PoolManager.Pools cannot be copied";
            throw new System.NotImplementedException(msg);
        }

        void ICollection<KeyValuePair<string, ObjectPool>>.CopyTo(KeyValuePair<string, ObjectPool>[] array, int arrayIndex)
        {
            string msg = "PoolManager.Pools cannot be copied";
            throw new System.NotImplementedException(msg);
        }

        public bool Remove(KeyValuePair<string, ObjectPool> item)
        {
            string msg = "SpawnPools can only be destroyed, not removed and kept alive" +
                         " outside of PoolManager. There are only 2 legal ways to destroy " +
                         "a SpawnPool: Destroy the GameObject directly, if you have a " +
                         "reference, or use PoolManager.Destroy(string poolName).";
            throw new System.NotImplementedException(msg);
        }
        #endregion ICollection<KeyValuePair<string, SpawnPool>> Members
        #endregion Not Implimented




        #region IEnumerable<KeyValuePair<string,SpawnPool>> Members
        public IEnumerator<KeyValuePair<string, ObjectPool>> GetEnumerator()
        {
            return this._pools.GetEnumerator();
        }
        #endregion



        #region IEnumerable Members
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this._pools.GetEnumerator();
        }
        #endregion

        #endregion Dict Functionality

    }
}
