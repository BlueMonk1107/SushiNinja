using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PathologicalGames
{
    /// <description>
    /// 这个类是用来在unity编辑界面显示一个更复杂的用户输入界面，
    /// 这样我们能收集预制间相互关联的更多选项，
    /// 这个类实现对象池中主要的池功能，
    /// 对象池通过这些方法和设置管理对象池。
    /// </description>
    /// <summary>
    /// 二级对象池，管理具体的对象
    /// </summary>
    [System.Serializable]
    public class PrefabPool
    {

        #region 在编辑器中可用的公共属性
        /// <summary>
        /// 准备加载的预制体
        /// </summary>
        public Transform prefab;

        /// <summary>
        /// 保存一个预制体的引用
        /// </summary>
        internal GameObject referenceOfPrefab;  // Hidden in inspector, but not Debug tab

        /// <summary>
        /// 缓存池这个Prefab的最大保存数量
        /// </summary>
        public int preloadAmount = 1;

        /// <summary>
        /// 如果勾选表示缓存池所有的gameobject可以“异步”加载
        /// </summary>
        public bool preloadTime = false;

        /// <summary>
        /// 加载所有请求的实例，需要的帧数(实际上预加载是每帧加载一个，这个参数什么用都没有)
        /// </summary>
        public int preloadFrames = 2;

        /// <summary>
        /// 延迟多久开始加载
        /// </summary>
        public float preloadDelay = 0;

        /// <summary>
        ///	是否开启对象实例化的限制功能
        /// </summary>
        public bool limitInstances = false;

        /// <summary>
        /// 限制实例化Prefab的数量，'limitInstances'为true时启用
        /// </summary>
        public int limitAmount = 100;

        /// <summary>
        /// <para>FIFO是指先进先出</para>
        /// <para>当超出限定缓存池限定数量，如果不勾选它，那么就会返回null。如果勾选它，就会返回缓存池里最不常用的那个。</para>
        /// </summary>
        public bool limitFIFO = false;  // Keep after limitAmount for auto-inspector

        /// <summary>
        /// 是否开启缓存池智能自动清理模式，就是当池子里面的对象setActive(false)也就是目前不用的时候，销毁对象
        /// 不要用这个,除非你需要管理内存问题!
        /// </summary>
        public bool AutoDestroy = false;

        /// <summary>
        /// 缓存池自动清理，但是始终保留几个对象不清理
        /// </summary>
        public int DestroyExceptTheNumber = 50;

        /// <summary>
        /// 每过多久执行一遍自动清理，单位是秒。从上一次清理过后开始计时
        /// </summary>
        public int DestroyEachTime = 60;

        /// <summary>
        /// 每次自动清理几个游戏对象
        /// </summary>
        public int DestroyNumberEverytime = 5;

        /// <summary>
        /// 是否打印信息
        /// </summary>
        public bool _logMessages = false;
        public bool LogMessages
        {
            get
            {
                if (DisableLogging) return false;

                if (this.ObjectPool.logMessages)
                    return this.ObjectPool.logMessages;
                else
                    return this._logMessages;
            }
        }

        // 是否禁用打印信息
        private bool DisableLogging = false;


        /// <summary>
        ///此预制池所在的对象池对象
        /// </summary>
        public ObjectPool ObjectPool { private get; set; }

        #endregion Public Properties Available in the Editor


        #region 构造和自销毁
        /// <description>
        ///	构造函数（参数为预制体对象）
        /// </description>
        public PrefabPool(Transform prefab)
        {
            this.prefab = prefab;
            this.referenceOfPrefab = prefab.gameObject;
        }

        /// <description>
        ///	构造函数仅供序列化检视面板使用
        /// </description>
        public PrefabPool() { }

        /// <description>
        /// 面板中实例的构造方法（在面板中的实例，没有执行构造函数）
        /// </description>
        internal void InspectorInstanceConstructor()
        {
            this.referenceOfPrefab = this.prefab.gameObject;
            this._active_List = new List<Transform>();
            this._inactive_List = new List<Transform>();
        }


        /// <summary>
        /// 对象池销毁时执行（销毁所有对象并清空数组）
        /// </summary>
        internal void SelfDestruct()
        {
            // Probably overkill but no harm done
            this.prefab = null;
            this.referenceOfPrefab = null;
            this.ObjectPool = null;

            // Go through both lists and destroy everything
            foreach (Transform inst in this._inactive_List)
                if (inst != null)
                    Object.Destroy(inst.gameObject);

            foreach (Transform inst in this._active_List)
                if (inst != null)
                    Object.Destroy(inst.gameObject);

            this._active_List.Clear();
            this._inactive_List.Clear();
        }
        #endregion Constructor and Self-Destruction


        #region 池功能
        /// <summary>
        /// 开启销毁实例的开关，为true时，销毁大于规定数目的实例（此开关为了避免多次执行销毁携程）
        /// </summary>
        private bool _destroy_Bool = false;

        /// <summary>
        /// 已经显示的对象数组
        /// </summary>
        internal List<Transform> _active_List = new List<Transform>();
        public List<Transform> ActiveList { get { return new List<Transform>(this._active_List); } }

        /// <summary>
        /// 隐藏对象的数组
        /// </summary>
        internal List<Transform> _inactive_List = new List<Transform>();
        public List<Transform> InactiveList { get { return new List<Transform>(this._inactive_List); } }


        /// <summary>
        /// Returns the total count of instances in the PrefabPool
        /// 返回缓存池内的最大数量
        /// </summary>
        public int totalCount
        {
            get
            {
                // Add all the items in the pool to get the total count
                int count = 0;
                count += this._active_List.Count;
                count += this._inactive_List.Count;
                return count;
            }
        }


        /// <summary>
        ///  用于开启PreloadInstances()中的一次性事件，只读 
        /// </summary>
        private bool _preloaded = false;
        internal bool Preloaded
        {
            get { return this._preloaded; }
            private set { this._preloaded = value; }
        }


        /// <summary>
        /// 隐藏实例
        /// </summary>
        /// <returns>
        /// True if successfull, false if xform isn't in the spawned list
        /// </returns>
        internal bool InactiveInstance(Transform xform)
        {
            return InactiveInstance(xform, true);
        }

        internal bool InactiveInstance(Transform xform, bool sendEventMessage)
        {
            if (this.LogMessages)
            {
                Debug.Log(string.Format("SpawnPool {0} ({1}): Despawning '{2}'",
                                       this.ObjectPool.poolName,
                                       this.prefab.name,
                                       xform.name));
            }
            //把实例从显示数组移到隐藏数组中
            this._active_List.Remove(xform);
            this._inactive_List.Add(xform);

            //通知自定义代码中添加的实例的事件OnDespawned(像除自身外所有子类发消息)
            if (sendEventMessage)
            {
                xform.gameObject.BroadcastMessage(
                   "OnDespawned",
                   this.ObjectPool,
                   SendMessageOptions.DontRequireReceiver
               );
            }
            
            PoolManagerUtils.SetActive(xform.gameObject, false);

            if (!this._destroy_Bool &&   // 销毁开关
                this.AutoDestroy &&      // 自动销毁的开关 
                this.totalCount > this.DestroyExceptTheNumber)   // 对象总数大于保留的对象数目
            {
                this._destroy_Bool = true;
                this.ObjectPool.StartCoroutine(Destroy());
            }
            return true;
        }

        internal IEnumerator Destroy()
        {
            if (this.LogMessages)
            {
                Debug.Log(string.Format("SpawnPool {0} ({1}): 销毁对象! " +
                                          "等待 {2}秒 开始检测销毁",
                                        this.ObjectPool.poolName,
                                        this.prefab.name,
                                        this.DestroyEachTime));
            }
                
            yield return new WaitForSeconds(this.DestroyEachTime);

            while (this.totalCount > this.DestroyExceptTheNumber)
            {
                // Attempt to delete an amount == this.cullMaxPerPass
                for (int i = 0; i < this.DestroyNumberEverytime; i++)
                {
                    if (this.totalCount <= this.DestroyExceptTheNumber)
                        break; 

                    //销毁显示列表中的最后一项
                    if (this._inactive_List.Count > 0)
                    {
                        Transform inst = this._inactive_List[0];
                        this._inactive_List.RemoveAt(0);
                        MonoBehaviour.Destroy(inst.gameObject);

                        if (this.LogMessages)
                        {
                            Debug.Log(string.Format("SpawnPool {0} ({1}): " +
                                                   "要销毁至 {2} 个对象 . 现在还有 {3} 个对象.",
                                               this.ObjectPool.poolName,
                                               this.prefab.name,
                                               this.DestroyExceptTheNumber,
                                               this.totalCount));
                        }
                           
                    }
                    else if (this.LogMessages)
                    {
                        Debug.Log(string.Format("SpawnPool {0} ({1}): " +
                                                    "等待 {2} 秒，开始下次销毁",
                                                this.ObjectPool.poolName,
                                                this.prefab.name,
                                                this.DestroyEachTime));

                        break;
                    }
                }
                //等待下次执行
                yield return new WaitForSeconds(this.DestroyEachTime);
            }

            if (this.LogMessages)
            {
                Debug.Log(string.Format("SpawnPool {0} ({1}): 销毁完毕",
                                       this.ObjectPool.poolName,
                                       this.prefab.name));
            }

            //复位开关
            this._destroy_Bool = false;
            yield return null;
        }



        /// <summary>
        ///<para>若开启了先进先出，则在没有隐藏对象的情况下，把第一个对象隐藏，再显示出来</para>
        /// </summary>
        /// <returns>
        /// The new instance's Transform. 
        /// 
        /// If the Limit option was used for the PrefabPool associated with the
        /// passed prefab, then this method will return null if the limit is
        /// reached.
        /// </returns>    
        internal Transform ActiveInstance(Vector3 pos, Quaternion rot)
        {
            //开启了限制实例数量和先进先出，而且显示实例数量大于等于限制数量
            if (this.limitInstances 
                && this.limitFIFO 
                &&this._active_List.Count >= this.limitAmount)
            {
                Transform firstIn = this._active_List[0];

                if (this.LogMessages)
                {
                    Debug.Log(string.Format
                    (
                        "SpawnPool {0} ({1}): " +
                            "限制完成，隐藏 {2}...",
                        this.ObjectPool.poolName,
                        this.prefab.name,
                        firstIn
                    ));
                }
                //隐藏实例
                this.InactiveInstance(firstIn);
                //从显示数组中移除
                this.ObjectPool._active_List.Remove(firstIn);
            }

            Transform inst;

            // 如果没有隐藏的实例，创建一个新的
            if (this._inactive_List.Count == 0)
            {
                inst = this.ActiveNew(pos, rot);
            }
            else
            {
                //从隐藏数组移到显示数组
                inst = this._inactive_List[0];
                this._inactive_List.RemoveAt(0);
                this._active_List.Add(inst);

                if (inst == null)
                {
                    var msg = "确保没有手动删除一个隐藏对象";
                    throw new MissingReferenceException(msg);
                }

                if (this.LogMessages)
                {
                    Debug.Log(string.Format("SpawnPool {0} ({1}): 显示 '{2}'.",
                                            this.ObjectPool.poolName,
                                            this.prefab.name,
                                            inst.name));
                }
                    
                
                inst.position = pos;
                inst.rotation = rot;
                PoolManagerUtils.SetActive(inst.gameObject, true);

            }
            
            return inst;
        }



        /// <summary>
        /// 显示一个新的实例
        /// </summary>
        /// <param name="pos">Vector3</param>
        /// <param name="rot">Quaternion</param>
        /// <returns>
        /// The new instance's Transform. 
        /// 
        /// If the Limit option was used for the PrefabPool associated with the
        /// passed prefab, then this method will return null if the limit is
        /// reached.
        /// </returns>
        private Transform ActiveNew() { return this.ActiveNew(Vector3.zero, Quaternion.identity); }

        private Transform ActiveNew(Vector3 pos, Quaternion rot)
        {
            // 开启了实例限制且总数量超出限制
            if (this.limitInstances && this.totalCount >= this.limitAmount)
            {
                if (this.LogMessages)
                {
                    Debug.Log(string.Format
                    (
                        "SpawnPool {0} ({1}): " +
                                "达到限制数量，不再产生新的实例",
                            this.ObjectPool.poolName,
                            this.prefab.name
                    ));
                }

                return null;
            }
            // 若是位置角度都是默认，就赋值池对象的参数
            if (pos == Vector3.zero) pos = this.ObjectPool.CloneFather.position;
            if (rot == Quaternion.identity) rot = this.ObjectPool.CloneFather.rotation;

            //创建新实例，并更名
            var inst = (Transform)Object.Instantiate(this.prefab, pos, rot);
            this.nameInstance(inst);  // Adds the number to the end

            //若池对象为父物体
            if (!this.ObjectPool.thisNotFather)
                inst.parent = this.ObjectPool.CloneFather;  // The group is the parent by default

            //若开启匹配缩放，则缩放赋值为1
            if (this.ObjectPool.matchPoolScale)
                inst.localScale = Vector3.one;

            //若开启匹配层级，则给物体及其所有子物体设置层级
            if (this.ObjectPool.matchPoolLayer)
                this.SetLayer(inst, this.ObjectPool.gameObject.layer);

            //实例加入显示列表中
            this._active_List.Add(inst);

            if (this.LogMessages)
                Debug.Log(string.Format("SpawnPool {0} ({1}): 创建了新实例 '{2}'.",
                                        this.ObjectPool.poolName,
                                        this.prefab.name,
                                        inst.name));

            return inst;
        }


        /// <summary>
        /// 设置物体及其所有子物体的层级
        /// </summary>
        /// <param name="xform">The transform to process</param>
        /// <param name="layer">The new layer</param>
        private void SetLayer(Transform xform, int layer)
        {
            //设置物体层级
            xform.gameObject.layer = layer;
            //递归子物体
            foreach (Transform child in xform)
            {
                SetLayer(child, layer);
            }
                
        }


        /// <summary>
        /// 用对象池添加一个现存的实例到预制池
        /// Used by a SpawnPool to add an existing instance to this PrefabPool.
        /// This is used during game start to pool objects which are not 
        /// instantiated at runtime
        /// </summary>
        /// <param name="inst">The instance to add</param>
        /// <param name="inactive">True to despawn on add</param>
        internal void AddInstanceNotInPool(Transform inst, bool inactive)
        {
            //给实例的名字加上数字
            this.nameInstance(inst);

            if (inactive)
            {
                PoolManagerUtils.SetActive(inst.gameObject, false);

                this._inactive_List.Add(inst);
            }
            else
            {
                this._active_List.Add(inst);
            }
                
        }


        /// <summary>
        /// Preload PrefabPool.preloadAmount instances if they don't already exist. In 
        /// otherwords, if there are 7 and 10 should be preloaded, this only creates 3.
        /// This is to allow asynchronous Spawn() usage in Awake() at game start
        /// 预加载PrefabPool.preloadAmount数量的实例
        /// </summary>
        /// <returns></returns>
        internal void PreloadInstances()
        {
            if (this.Preloaded)
            {
                Debug.Log(string.Format("SpawnPool {0} ({1}): " +
                                          "预制体已经加载过了，如果是在代码中定义的，看看检视面板中是否有重复定义",
                                        this.ObjectPool.poolName,
                                        this.prefab.name));

                return;
            }

            if (this.prefab == null)
            {
                Debug.LogError(string.Format("SpawnPool {0} ({1}): 预制体不能为空.",
                                             this.ObjectPool.poolName,
                                             this.prefab.name));

                return;
            }

            //开启实例数量限制
            if (this.limitInstances && this.preloadAmount > this.limitAmount)
            {
                Debug.LogWarning
                (
                    string.Format
                    (
                        "SpawnPool {0} ({1}): " +
                            "开启了'Limit Instances' 并且预加载数量大于限制数量，" +
                            "这里自动将预加载数量设置为限制数量",
                         this.ObjectPool.poolName,
                         this.prefab.name
                    )
                );

                this.preloadAmount = this.limitAmount;
            }

            //开启清理模式
            if (this.AutoDestroy && this.preloadAmount > this.DestroyExceptTheNumber)
            {
                Debug.LogWarning(string.Format("SpawnPool {0} ({1}): " +
                    "You turned ON Culling and entered a 'Cull Above' threshold " +
                    "greater than the 'Preload Amount'! This will cause the " +
                    "culling feature to trigger immediatly, which is wrong " +
                    "conceptually. Only use culling for extreme situations. " +
                    "See the docs.",
                    this.ObjectPool.poolName,
                    this.prefab.name
                ));
            }

            //开启异步加载
            if (this.preloadTime)
            {
                if (this.preloadFrames > this.preloadAmount)
                {
                    Debug.LogWarning(string.Format("SpawnPool {0} ({1}): " +
                        "Preloading over-time is on but the frame duration is greater " +
                        "than the number of instances to preload. The minimum spawned " +
                        "per frame is 1, so the maximum time is the same as the number " +
                        "of instances. Changing the preloadFrames value...",
                        this.ObjectPool.poolName,
                        this.prefab.name
                    ));

                    this.preloadFrames = this.preloadAmount;
                }

                this.ObjectPool.StartCoroutine(this.PreloadOverTime());
            }
            else
            {
                //禁用打印信息
                this.DisableLogging = true;

                Transform inst;

                //实例数量小于预加载总数，则添加实例至符合要求
                while (this.totalCount < this.preloadAmount) 
                {
                    inst = this.ActiveNew();
                    this.InactiveInstance(inst, false);
                }

                // 开关复位
                this.DisableLogging = false;
            }
        }

        //实际上预加载是每帧加载一个，preloadFrames这个参数什么用都没有，就是分了几次执行继续循环加载而已。
        private IEnumerator PreloadOverTime()
        {
            yield return new WaitForSeconds(this.preloadDelay);

            Transform inst;

            // 获得要加载的数量
            int amount = this.preloadAmount - this.totalCount;

            if (amount <= 0)
                yield break;
            
            int remainder = amount % this.preloadFrames;
            int numPerFrame = amount / this.preloadFrames;

            //关闭打印信息
            this.DisableLogging = true;

            int numThisFrame = 0;

            for (int i = 0; i < this.preloadFrames; i++)
            {
                if (i < this.preloadFrames - 1)
                {
                    numThisFrame = numPerFrame;
                }
                else
                {
                    numThisFrame += remainder;
                }
                
                for (int n = 0; n < numThisFrame; n++)
                {
                    inst = this.ActiveNew();

                    if (inst != null)
                    {
                        this.InactiveInstance(inst, false);
                    }
                    //下帧继续执行
                    yield return null;
                }

                //如果提前加载完，退出
                if (this.totalCount > this.preloadAmount)
                    break;
            }

            // 开关复位
            this.DisableLogging = false;
        }

        #endregion Pool Functionality


        #region 实用工具
        /// <summary>
        /// 若显示和隐藏数组中包含所给对象，返回true，否则返回false
        /// </summary>
        /// <param name="transform">A transform to test.</param>
        /// <returns>bool</returns>
        public bool Contains(Transform transform)
        {
            if (this.referenceOfPrefab == null)
                Debug.LogError(string.Format("SpawnPool {0}: PrefabPool.prefabGO is null",
                                             this.ObjectPool.poolName));

            var contains = this.ActiveList.Contains(transform);
            if (contains)
                return true;

            contains = this.InactiveList.Contains(transform);
            if (contains)
                return true;

            return false;
        }

        /// <summary>
        /// 给新建的实例名字，添加数字后缀
        /// </summary>
        /// <param name="instance"></param>
        private void nameInstance(Transform instance)
        {
            instance.name += (this.totalCount + 1).ToString("#000");
        }
        #endregion Utilities

    }
}
