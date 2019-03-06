using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PathologicalGames
{

    /// <description>
    ///	一个特殊的列表类，管理对象池并保持场景的秩序。
    /// 
    ///     只有活跃/生成 的实例，才能被迭代
    ///     不活跃/销毁 的实例只能保存在池内部
    /// 
    ///     实例化对象作为池的子对象（类似于一个组）来保持场景中hierachy界面的秩序
    ///		 
    ///     实例的名字将会附加上一个数字
    ///     例如"Enemy"的实例会被命名为"Enemy(Clone)001", "Enemy(Clone)002", "Enemy(Clone)003"
    ///     所有的实例都会被unity混淆为同一个名字
    ///		
    ///    Despawn()方法不会销毁对象
    ///    相反，对象会被隐藏并保存到内部的队列中以备再次使用
    ///    这样避免了销毁物体所耗费的时间，通过复用物体提升了性能
    ///		   
    ///  * Two events are implimented to enable objects to handle their own reset needs. 
    ///    Both are optional.
    ///      1) When objects are Despawned BroadcastMessage("OnDespawned()") is sent.
    ///		 2) When reactivated, a BroadcastMessage("OnRespawned()") is sent. 
    ///		    This 
    ///    执行以下两个事件使对象处理他们自己的复位需求
    ///    都是可选的
    ///      1）当对象被销毁
    ///      2）当对象显示
    /// </description>
    /// <summary>
    /// 对象池，管理二级对象池（PrefabPool）
    /// </summary>
    [AddComponentMenu("PoolManager/ObjectPool")]
    public sealed class ObjectPool : MonoBehaviour, IList<Transform>
    {
        #region 检视面板中显示的参数
        /// <summary>
        /// 返回对象池的名字，它将永远是一个名字，
        /// 除非在工作中的对象池有相同的名字，正在工作的内存池将被剔除
        /// </summary>
        public string poolName = "";

        /// <summary>
        /// 如果是True，实例化的游戏对象的缩放比例将全是1，否则用Prefab默认的。
        /// </summary>
        public bool matchPoolScale = false;

        /// <summary>
        /// 如果是True，实例化的游戏对象的Layer将用Prefab默认的。
        /// </summary>
        public bool matchPoolLayer = false;

        /// <summary>
        /// 如果是True，就不把池对象的作为父物体
        /// </summary>
        public bool thisNotFather = false;
		
		/// <summary>
        /// 如果是True，切换场景将不释放资源
        /// </summary>
        public bool dontDestroyOnLoad
		{
			get
			{
				return this._dontDestroyOnLoad;
			}
			
			set
			{
				this._dontDestroyOnLoad = value;
				
				if (this.CloneFather != null)
					Object.DontDestroyOnLoad(this.CloneFather.gameObject);
			}
		}
        public bool _dontDestroyOnLoad = false;  // Property backer and used by GUI.
		
        /// <summary>
        /// 打印信息到控制台
        /// </summary>
        public bool logMessages = false;

        /// <summary>
        /// 预制池缓存列表
        /// </summary>
        public readonly List<PrefabPool> _perPrefabPoolOptions = new List<PrefabPool>();

        /// <summary>
        /// Used by the inspector to store this instances foldout states.
        /// </summary>
        public Dictionary<object, bool> prefabsFoldOutStates = new Dictionary<object, bool>();
        #endregion Inspector Parameters


        #region 代码中的公有参数
        /// <summary>
        /// The time in seconds to stop waiting for particles to die.
        /// A warning will be logged if this is triggered.
        /// 这是等待粒子销毁的时间，触发时将会打印一个错误
        /// </summary>
        public float maxParticleDespawnTime = 300;

        /// <summary>
        /// 在池中作为实例的父物体，它能帮助场景更好的工作
        /// </summary>
        public Transform CloneFather { get; private set; }

        /// <summary>
        /// 通过名字返回预制体（字典关键字）
        /// </summary>
        public readonly PrefabsDict prefabs_Dict = new PrefabsDict();

        // Keeps the state of each individual foldout item during the editor session
        public Dictionary<object, bool> _editorListItemStates = new Dictionary<object, bool>();

        /// <summary>
        /// 通过字典读取预制池（只读）
        /// </summary>
        public Dictionary<string, PrefabPool> prefabPools
        {
            get
            {
                var dict = new Dictionary<string, PrefabPool>();

                for (int i = 0; i < this._prefabPools_List.Count; i++)
                    dict[this._prefabPools_List[i].referenceOfPrefab.name] = this._prefabPools_List[i];

                return dict;
            }
        }
        #endregion Public Code-only Parameters
        

        #region 私有属性
        /// <summary>
        /// 预制池
        /// </summary>
        private readonly List<PrefabPool> _prefabPools_List = new List<PrefabPool>();
        /// <summary>
        /// 显示对象的列表
        /// </summary>
        internal readonly List<Transform> _active_List = new List<Transform>();
        #endregion Private Properties

        
        #region 构造和初始化
        private void Awake()
        {
            // 切换场景不释放资源
            if (this._dontDestroyOnLoad) Object.DontDestroyOnLoad(this.gameObject);

            this.CloneFather = this.transform;

            // 若poolName为空，就以当前物体名字去掉Pool和(Clone)作为名字
            if (this.poolName == "")
            {
                this.poolName = this.CloneFather.name.Replace("Pool", "");
                this.poolName = this.poolName.Replace("(Clone)", "");
            }

            //开启打印信息
            if (this.logMessages)
                Debug.Log(string.Format("SpawnPool {0}: 初始化..", this.poolName));

            // 仅在检视面板中添加项目时执行
            for (int i = 0; i < this._perPrefabPoolOptions.Count; i++)
            {
                if (this._perPrefabPoolOptions[i].prefab == null)
                {
                    Debug.LogWarning(string.Format("初始化警告: Pool '{0}' " +
                              "中不包含预制体的引用.",
                               this.poolName));
                    continue;
                }

                //初始化预制池的缓存，因为它没有执行
                //仅在检视面板中创建时被执行，因为构造函数没有执行
                this._perPrefabPoolOptions[i].InspectorInstanceConstructor();
                this.CreatePrefabPool(this._perPrefabPoolOptions[i]);
            }

            //向池对象字典中添加此对象
            PoolManager.Pools.Add(this);
        }


        /// <summary>
        /// 销毁所有对象
        /// </summary>
        private void OnDestroy()
        {
            if (this.logMessages)
                Debug.Log(string.Format("SpawnPool {0}: 销毁中...", this.poolName));
            //移除字典中的对象
            PoolManager.Pools.Remove(this);
            //杀死所有携程
            this.StopAllCoroutines();

            //清空显示对象的引用
            this._active_List.Clear();

            //遍历销毁所有预制池
            foreach (PrefabPool pool in this._prefabPools_List) pool.SelfDestruct();
            
            this._prefabPools_List.Clear();
            this.prefabs_Dict._Clear();
        }


        /// <summary>
        /// <para>这是第一行</para>
        /// <para>在对象池中创建一个新的预制池并创建请求数量的实例对象（通过PrefabPool.preloadAmount设置),如果请求数量为0，将不会创建实例</para>
        /// <para>这个函数仅当你需要设置一个新的预制池时会被执行，比如在使用前的剔除或预加载</para>
        /// <para>另外，如果是第一次使用的预制池，用Active（）将会自动创建默认值</para>
        /// 
        /// <para>像下面这样创建预制池并设置参数      </para>
        /// <para>    PrefabPool prefabPool = new PrefabPool()</para>
        /// <para>    prefabPool.prefab = myPrefabReference;</para>
        /// <para>    prefabPool.preloadAmount = 0;</para>
        /// <para>    prefabPool.cullDespawned = True;</para>
        /// <para>    prefabPool.cullAbove = 50;</para>
        /// <para>    prefabPool.cullDelay = 30;</para>
        /// <para>//初始化内存池</para>
        /// <para>    spawnPool._perPrefabPoolOptions.Add(prefabPool);</para>
        /// <para>    spawnPool.CreatePrefabPool(spawnPool._perPrefabPoolOptions[spawnPool._perPrefabPoolOptions.Count - 1]);</para>
        /// </summary>
        /// <param name="prefabPool">A PrefabPool object</param>
        /// <returns>A List of instances spawned or an empty List</returns>
        public void CreatePrefabPool(PrefabPool prefabPool)
        {
            //判断内存池是否已经存在
            bool isAlreadyPool = this.GetPrefabPool(prefabPool.prefab) != null;

            if (!isAlreadyPool)
            {
                //给内存池对象赋值
                prefabPool.ObjectPool = this;

                //把预制池添加进列表和字典中
                this._prefabPools_List.Add(prefabPool);
                this.prefabs_Dict._Add(prefabPool.prefab.name, prefabPool.prefab);
            }
            
            // 预加载预制体的实例
            if (prefabPool.Preloaded != true)
            {
                if (this.logMessages)
                    Debug.Log(string.Format("SpawnPool {0}: Preloading {1} {2}",
                                               this.poolName,
                                               prefabPool.preloadAmount,
                                               prefabPool.prefab.name));

                prefabPool.PreloadInstances();
            }
        }


        /// <summary>
        /// 添加一个现存的实例到对象池中，
        /// 这个函数用于在游戏开始运行时没有被实例的池对象
        /// Add an existing instance to this pool. This is used during game start 
        /// to pool objects which are not instanciated at runtime
        /// </summary>
        /// <param name="instance">The instance to add</param>
        /// <param name="prefabName">
        /// The name of the prefab used to create this instance
        /// </param>
        /// <param name="despawn">True to depawn on start</param>
        /// <param name="parent">True to make a child of the pool's group</param>
        public void Add(Transform instance, string prefabName, bool despawn, bool parent)
        {
            for (int i = 0; i < this._prefabPools_List.Count; i++)
            {
                //判断预制体是否为空
                if (this._prefabPools_List[i].referenceOfPrefab == null)
                {
                    Debug.LogError("预制体 为 null");
                    return;
                }

                if (this._prefabPools_List[i].referenceOfPrefab.name == prefabName)
                {
                    //添加一个不在池中的预制体实例
                    this._prefabPools_List[i].AddInstanceNotInPool(instance, despawn);

                    if (this.logMessages)
                        Debug.Log(string.Format(
                                "SpawnPool {0}: 添加一个不在池中的预制体实例 {1}",
                                                this.poolName,
                                                instance.name));

                    if (parent)
                    {
                        instance.parent = this.CloneFather;
                    }

                    // 添加到显示列表中
                    if (!despawn) this._active_List.Add(instance);

                    return;
                }
            }
            
            Debug.LogError(string.Format("SpawnPool {0}: PrefabPool {1} 没有找到",
                                         this.poolName,
                                         prefabName));

        }
        #endregion Constructor and Init



        #region 重写列表
        /// <summary>
        /// Not Implimented. Use Spawn() to properly add items to the pool.
        /// This is required because the prefab needs to be stored in the internal
        /// data structure in order for the pool to function properly. Items can
        /// only be added by instencing them using SpawnInstance().
        /// </summary>
        /// <param name="item"></param>
        public void Add(Transform item)
        {
            string msg = "Use SpawnPool.Spawn() to properly add items to the pool.";
            throw new System.NotImplementedException(msg);
        }


        /// <summary>
        /// Not Implimented. Use Despawn() to properly manage items that should remain 
        /// in the Queue but be deactivated. There is currently no way to safetly
        /// remove items from the pool permentantly. Destroying Objects would
        /// defeat the purpose of the pool.
        /// </summary>
        /// <param name="item"></param>
        public void Remove(Transform item)
        {
            string msg = "Use Despawn() to properly manage items that should " +
                         "remain in the pool but be deactivated.";
            throw new System.NotImplementedException(msg);
        }

        #endregion List Overrides



        #region 池功能

        #region 游戏物体显示函数
        /// <description>
        ///	Spawns an instance or creates a new instance if none are available.
        ///	Either way, an instance will be set to the passed position and 
        ///	rotation.
        /// 
        /// Detailed Information:
        /// Checks the Data structure for an instance that was already created
        /// using the prefab. If the prefab has been used before, such as by
        /// setting it in the Unity Editor to preload instances, or just used
        /// before via this function, one of its instances will be used if one
        /// is available, or a new one will be created.
        /// 
        /// If the prefab has never been used a new PrefabPool will be started 
        /// with default options. 
        /// 
        /// To alter the options on a prefab pool, use the Unity Editor or see
        /// the documentation for the PrefabPool class and 
        /// SpawnPool.SpawnPrefabPool()
        ///		
        /// Broadcasts "OnSpawned" to the instance. Use this to manage states.
        ///		
        /// An overload of this function has the same initial signature as Unity's 
        /// Instantiate() that takes position and rotation. The return Type is different 
        /// though. Unity uses and returns a GameObject reference. PoolManager 
        /// uses and returns a Transform reference (or other supported type, such 
        /// as AudioSource and ParticleSystem)
        /// </description>
        /// <param name="prefab">
        /// The prefab used to spawn an instance. Only used for reference if an 
        /// instance is already in the pool and available for respawn. 
        /// NOTE: Type = Transform
        /// </param>
        /// <param name="pos">The position to set the instance to</param>
        /// <param name="rot">The rotation to set the instance to</param>
        /// <param name="parent">An optional parent for the instance</param>
        /// <returns>
        /// The instance's Transform. 
        /// 
        /// If the Limit option was used for the PrefabPool associated with the
        /// passed prefab, then this method will return null if the limit is
        /// reached. You DO NOT need to test for null return values unless you 
        /// used the limit option.
        /// </returns>
        public Transform Active(Transform prefab, Vector3 pos, Quaternion rot, Transform parent)
        {
            Transform inst;

            #region 若对象在预制池中
            for (int i = 0; i < this._prefabPools_List.Count; i++)
            {
                if (this._prefabPools_List[i].referenceOfPrefab == prefab.gameObject)
                {
                    //显示一个实例，若没有实例可以显示，就创建一个新的
                    inst = this._prefabPools_List[i].ActiveInstance(pos, rot);

                    //若开启了限制选项，返回空
                    if (inst == null) return null;
					
                    //通过参数设置父物体
					if (parent != null) 
					{
						inst.parent = parent;
					}
                    else if (!this.thisNotFather && inst.parent != this.CloneFather)
					{
                        //设置父物体
                        inst.parent = this.CloneFather;
					}

                    //加入到显示列表
                    this._active_List.Add(inst);
					
	                // 广播信息
	                inst.gameObject.BroadcastMessage(
						"OnSpawned",
						this,
						SendMessageOptions.DontRequireReceiver
					);
                    
                    return inst;
                }
            }
            #endregion Use from Pool

            #region 若不在池中，创建新预制池对象
            PrefabPool newPrefabPool = new PrefabPool(prefab);
            this.CreatePrefabPool(newPrefabPool);

            //显示实例
            inst = newPrefabPool.ActiveInstance(pos, rot);
			
            //根据参数，设置父物体
			if (parent != null)  
			{
				inst.parent = parent;
			}
            else  
			{
            	inst.parent = this.CloneFather;  
			}


            // 加入显示列表 
            this._active_List.Add(inst);
            #endregion New PrefabPool

            // 广播信息
            inst.gameObject.BroadcastMessage(
				"OnSpawned",
				this,
				SendMessageOptions.DontRequireReceiver
			);
            
            return inst;
        }


        /// <summary>
        /// 生成预制方法
        /// See primary Spawn method for documentation.
        /// </summary>
        public Transform Active(Transform prefab, Vector3 pos, Quaternion rot)
        {
            Transform inst = this.Active(prefab, pos, rot, null);
            // Can happen if limit was used
            if (inst == null) return null;

            return inst;
        }


        /// <summary>
        /// See primary Spawn method for documentation.
        /// 生成预制方法，生成的预制体position为0，方向默认
        /// Overload to take only a prefab and instance using an 'empty' 
        /// position and rotation.
        /// </summary>
        public Transform Active(Transform prefab)
        {
            return this.Active(prefab, Vector3.zero, Quaternion.identity);
        }


        /// <summary>
        /// See primary Spawn method for documentation.
        /// 第一个参数是预制，第二个参数是预知的父物体
        /// Convienince overload to take only a prefab  and parent the new 
        /// instance under the given parent
        /// </summary>
        public Transform Active(Transform prefab, Transform parent)
        {
            return this.Active(prefab, Vector3.zero, Quaternion.identity, parent);
        }


        /// <summary>
        /// See primary Spawn method for documentation.
        /// 根据字符串生成预制
        /// Overload to take only a prefab name. The cached reference is pulled  
        /// from the SpawnPool.prefabs dictionary.
        /// </summary>
        public Transform Active(string prefabName)
        {
            Transform prefab = this.prefabs_Dict[prefabName];
            return this.Active(prefab);
        }

        /// <summary>
        /// See primary Spawn method for documentation.
        ///  第一个参数是预制名字，第二个参数是预知的父物体
        /// Convienince overload to take only a prefab name and parent the new 
        /// instance under the given parent
        /// </summary>
        public Transform Active(string prefabName, Transform parent)
        {
            Transform prefab = this.prefabs_Dict[prefabName];
            return this.Active(prefab, parent);
        }


        /// <summary>
        /// See primary Spawn method for documentation.
        /// 
        /// Overload to take only a prefab name. The cached reference is pulled from 
        /// the SpawnPool.prefabs dictionary. An instance will be set to the passed 
        /// position and rotation.
        /// </summary>
        public Transform Active(string prefabName, Vector3 pos, Quaternion rot)
        {
            Transform prefab = this.prefabs_Dict[prefabName];
            return this.Active(prefab, pos, rot);
        }


        /// <summary>
        /// See primary Spawn method for documentation.
        /// 
        /// Convienince overload to take only a prefab name and parent the new 
        /// instance under the given parent. An instance will be set to the passed 
        /// position and rotation.
        /// </summary>
        public Transform Active(string prefabName, Vector3 pos, Quaternion rot, 
                               Transform parent)
        {
            Transform prefab = this.prefabs_Dict[prefabName];
            return this.Active(prefab, pos, rot, parent);
        }
        #endregion 游戏物体显示函数

        #region 声音显示函数
        public AudioSource ActiveAudio(AudioSource prefab,
                            Vector3 pos, Quaternion rot)
        {
            return this.ActiveAudio(prefab, pos, rot, null);  // parent = null
        }


        public AudioSource ActiveAudio(AudioSource prefab)
        {
            return this.ActiveAudio
            (
                prefab, 
                Vector3.zero, Quaternion.identity,
                null  // parent = null
            );
        }
		
	 	
		public AudioSource ActiveAudio(AudioSource prefab, Transform parent)
        {
            return this.ActiveAudio
            (
                prefab, 
                Vector3.zero, 
				Quaternion.identity,
                parent
            );
        }
		
		
        public AudioSource ActiveAudio(AudioSource prefab,
                            	 Vector3 pos, Quaternion rot,
                            	 Transform parent)
        {
            // Instance using the standard method before doing particle stuff
            Transform inst = Active(prefab.transform, pos, rot, parent);

            // Can happen if limit was used
            if (inst == null) return null;

            // Get the emitter and start it
            var src = inst.GetComponent<AudioSource>();
            src.Play();

            this.StartCoroutine(this.ListForAudioStop(src));

            return src;
        }

        private IEnumerator ListForAudioStop(AudioSource src)
        {
            // Safer to wait a frame before testing if playing.
            yield return null;

            while (src.isPlaying)
                yield return null;

            this.Inactive(src.transform);
        }

        #endregion 声音显示函数

        #region 粒子显示函数
        /// <summary>
        ///	See docs for SpawnInstance(Transform prefab, Vector3 pos, Quaternion rot)
        ///	for basic functionalty information.
        ///		
        /// Pass a ParticleSystem component of a prefab to instantiate, trigger 
        /// emit, then listen for when all particles have died to "auto-destruct", 
        /// but instead of destroying the game object it will be deactivated and 
        /// added to the pool to be reused.
        /// 
        /// IMPORTANT: 
        ///     * You must pass a ParticleSystem next time as well, or the emitter
        ///       will be treated as a regular prefab and simply activate, but emit
        ///       will not be triggered!
        ///     * The listner that waits for the death of all particles will 
        ///       time-out after a set number of seconds and log a warning. 
        ///       This is done to keep the developer aware of any unexpected 
        ///       usage cases. Change the public property "maxParticleDespawnTime"
        ///       to adjust this length of time.
        /// 
        /// Broadcasts "OnSpawned" to the instance. Use this instead of Awake()
        ///		
        /// This function has the same initial signature as Unity's Instantiate() 
        /// that takes position and rotation. The return Type is different though.
        /// </summary>
        public ParticleSystem ActiveParticle(ParticleSystem prefab,
                                    Vector3 pos, Quaternion rot)
        {
            return ActiveParticle(prefab, pos, rot, null);  // parent = null

        }

        /// <summary>
        /// See primary Spawn ParticleSystem method for documentation.
        /// 
        /// Convienince overload to take only a prefab name and parent the new 
        /// instance under the given parent. An instance will be set to the passed 
        /// position and rotation.
        /// </summary>
        public ParticleSystem ActiveParticle(ParticleSystem prefab,
                                    Vector3 pos, Quaternion rot,
                                    Transform parent)
        {
            // Instance using the standard method before doing particle stuff
            Transform inst = this.Active(prefab.transform, pos, rot, parent);

            // Can happen if limit was used
            if (inst == null) return null;

            // Get the emitter and start it
            var emitter = inst.GetComponent<ParticleSystem>();
            //emitter.Play(true);  // Seems to auto-play on activation so this may not be needed

            this.StartCoroutine(this.ListenForEmitInactive(emitter));

            return emitter;
        }


        /// <summary>
        ///	See docs for SpawnInstance(ParticleSystems prefab, Vector3 pos, Quaternion rot)
        ///	This is the same but for ParticleEmitters
        ///	
        /// IMPORTANT: 
        ///     * This function turns off Unity's ParticleAnimator autodestruct if
        ///       one is found.
        /// </summary>
        public ParticleEmitter ActiveParticle(ParticleEmitter prefab,
                                     Vector3 pos, Quaternion rot)
        {
            // Instance using the standard method before doing particle stuff
            Transform inst = this.Active(prefab.transform, pos, rot);

            // Can happen if limit was used
            if (inst == null) return null;

            // Make sure autodestrouct is OFF as it will cause null references
            var animator = inst.GetComponent<ParticleAnimator>();
            if (animator != null) animator.autodestruct = false;

            // Get the emitter
            var emitter = inst.GetComponent<ParticleEmitter>();
            emitter.emit = true;

            this.StartCoroutine(this.ListenForEmitInactive(emitter));

            return emitter;
        }


        /// <summary>
        /// This will not be supported for Shuriken particles. This will eventually 
        /// be depricated.
        /// </summary>
        /// <param name="prefab">
        /// The prefab to instance. Not used if an instance already exists in 
        /// the scene that is queued for reuse. Type = ParticleEmitter
        /// </param>
        /// <param name="pos">The position to set the instance to</param>
        /// <param name="rot">The rotation to set the instance to</param>
        /// <param name="colorPropertyName">Same as Material.SetColor()</param>
        /// <param name="color">a Color object. Same as Material.SetColor()</param>
        /// <returns>The instance's ParticleEmitter</returns>
        public ParticleEmitter ActiveParticle(ParticleEmitter prefab,
                                     Vector3 pos, Quaternion rot,
                                     string colorPropertyName, Color color)
        {
            // Instance using the standard method before doing particle stuff
            Transform inst = this.Active(prefab.transform, pos, rot);

            // Can happen if limit was used
            if (inst == null) return null;

            // Make sure autodestrouct is OFF as it will cause null references
            var animator = inst.GetComponent<ParticleAnimator>();
            if (animator != null) animator.autodestruct = false;

            // Get the emitter
            var emitter = inst.GetComponent<ParticleEmitter>();

            // Set the color of the particles, then emit
            emitter.GetComponent<Renderer>().material.SetColor(colorPropertyName, color);
            emitter.emit = true;

            this.StartCoroutine(ListenForEmitInactive(emitter));

            return emitter;
        }

        /// <summary>
        /// Used to determine when a particle emiter should be despawned
        /// </summary>
        /// <param name="emitter">ParticleEmitter to process</param>
        /// <returns></returns>
        private IEnumerator ListenForEmitInactive(ParticleEmitter emitter)
        {
            // This will wait for the particles to emit. Without this, there will
            //   be no particles in the while test below. I don't know why the extra 
            //   frame is required but should never be noticable. No particles can
            //   fade out that fast and still be seen to change over time.
            yield return null;
            yield return new WaitForEndOfFrame();

            // Do nothing until all particles die or the safecount hits a max value
            float safetimer = 0;   // Just in case! See Spawn() for more info
            while (emitter.particleCount > 0)
            {
                safetimer += Time.deltaTime;
                if (safetimer > this.maxParticleDespawnTime)
                    Debug.LogWarning
                    (
                        string.Format
                        (
                            "SpawnPool {0}: " +
                                "Timed out while listening for all particles to die. " +
                                "Waited for {1}sec.",
                            this.poolName,
                            this.maxParticleDespawnTime
                        )
                    );

                yield return null;
            }

            // Turn off emit before despawning
            emitter.emit = false;
            this.Inactive(emitter.transform);
        }


        // ParticleSystem (Shuriken) Version...
        private IEnumerator ListenForEmitInactive(ParticleSystem emitter)
        {
            // Wait for the delay time to complete
            // Waiting the extra frame seems to be more stable and means at least one 
            //  frame will always pass
            yield return new WaitForSeconds(emitter.startDelay + 0.25f);

            // Do nothing until all particles die or the safecount hits a max value
            float safetimer = 0;   // Just in case! See Spawn() for more info
            while (emitter.IsAlive(true))
            {
                if (!PoolManagerUtils.activeInHierarchy(emitter.gameObject))
                {
                    emitter.Clear(true);
                    yield break;  // Do nothing, already despawned. Quit.
                }

                safetimer += Time.deltaTime;
                if (safetimer > this.maxParticleDespawnTime)
                    Debug.LogWarning
                    (
                        string.Format
                        (
                            "SpawnPool {0}: " +
                                "Timed out while listening for all particles to die. " +
                                "Waited for {1}sec.",
                            this.poolName,
                            this.maxParticleDespawnTime
                        )
                    );

                yield return null;
            }

            // Turn off emit before despawning
            //emitter.Clear(true);
            this.Inactive(emitter.transform);
        }
        #endregion 粒子显示函数

        #region 隐藏函数
        /// <summary>
        ///	If the passed object is managed by the SpawnPool, it will be 
        ///	deactivated and made available to be spawned again.
        ///	释放函数
        /// Despawned instances are removed from the primary list.
        /// </summary>
        /// <param name="item">The transform of the gameobject to process</param>
        public void Inactive(Transform instance)
        {
            // Find the item and despawn it
            bool despawned = false;
            for (int i = 0; i < this._prefabPools_List.Count; i++)
            {
                if (this._prefabPools_List[i]._active_List.Contains(instance))
                {
                    despawned = this._prefabPools_List[i].InactiveInstance(instance);
                    break;
                }  // Protection - Already despawned?
                else if (this._prefabPools_List[i]._inactive_List.Contains(instance))
                {
                    //Debug.Log(
                    //    string.Format("SpawnPool {0}: {1} has already been despawned. " +
                    //                   "You cannot despawn something more than once!",
                    //                    this.poolName,
                    //                    instance.name));
                    return;
                }
            }

            // If still false, then the instance wasn't found anywhere in the pool
            if (!despawned)
            {
                Debug.LogError(string.Format("SpawnPool {0}: {1} not found in SpawnPool",
                               this.poolName,
                               instance.gameObject.name));
                return;
            }

            // Remove from the internal list. Only active instances are kept. 
            // 	 This isn't needed for Pool functionality. It is just done 
            //	 as a user-friendly feature which has been needed before.
            this._active_List.Remove(instance);
        }


        /// <summary>
        ///	See docs for Despawn(Transform instance) for basic functionalty information.
        ///		
        /// Convienince overload to provide the option to re-parent for the instance 
        /// just before despawn.
        /// </summary>
        public void Inactive(Transform instance, Transform parent)
        {
            instance.parent = parent;
            this.Inactive(instance);
        }


        /// <description>
        /// See docs for Despawn(Transform instance). This expands that functionality.
        ///   If the passed object is managed by this SpawnPool, it will be 
        ///   deactivated and made available to be spawned again.
        /// </description>
        /// <param name="item">The transform of the instance to process</param>
        /// <param name="seconds">The time in seconds to wait before despawning</param>
        /// <summary>
        /// 在规定时间之后，释放对象
        ///  </summary>
        public void Inactive(Transform instance, float seconds)
        {
            this.StartCoroutine(this.DoInactivityAfterSeconds(instance, seconds, false, null));
        }


        /// <summary>
        ///	See docs for Despawn(Transform instance) for basic functionalty information.
        ///		
        /// Convienince overload to provide the option to re-parent for the instance 
        /// just before despawn.
        /// </summary>
        public void Inactive(Transform instance, float seconds, Transform parent)
        {
            this.StartCoroutine(this.DoInactivityAfterSeconds(instance, seconds, true, parent));
        }


        /// <summary>
        /// Waits X seconds before despawning. See the docs for DespawnAfterSeconds()
        /// the argument useParent is used because a null parent is valid in Unity. It will 
        /// make the scene root the parent
        /// </summary>
        private IEnumerator DoInactivityAfterSeconds(Transform instance, float seconds, bool useParent, Transform parent)
        {
            GameObject go = instance.gameObject;
            while (seconds > 0)
            {
                yield return null;

                // If the instance was deactivated while waiting here, just quit
                if (!go.activeInHierarchy)
                    yield break;
                
                seconds -= Time.deltaTime;
            }

            if (useParent)
                this.Inactive(instance, parent);
            else
                this.Inactive(instance);
        }


        /// <description>
        /// Despawns all active instances in this SpawnPool
        /// </description>
        /// <summary>
        /// 释放全部对象
        ///  </summary>
        public void InactiveAll()
        {
            var spawned = new List<Transform>(this._active_List);
            for (int i = 0; i < spawned.Count; i++)
                this.Inactive(spawned[i]);
        }
        #endregion 隐藏函数

        #region 工具函数
        /// <description>
        ///	Returns true if the passed transform is currently spawned.
        /// </description>
        /// <param name="item">The transform of the gameobject to test</param>
        /// <summary>
        /// 判断是否生成了对象
        ///  </summary>
        public bool IsActive(Transform instance)
        {
            return this._active_List.Contains(instance);
        }
        #endregion 工具函数

        #endregion 池功能
        
        #region 实用函数
        /// <summary>
        /// 通过实例返回此实例所在的池对象，参数类型Transform
        /// </summary>
        /// <param name="prefab">The Transform of an instance</param>
        /// <returns>PrefabPool</returns>
        public PrefabPool GetPrefabPool(Transform prefab)
        {
            for (int i = 0; i < this._prefabPools_List.Count; i++)
            {
                if (this._prefabPools_List[i].referenceOfPrefab == null)
                    Debug.LogError(string.Format("SpawnPool {0}: PrefabPool.prefabGO is null",
                                                 this.poolName));

                if (this._prefabPools_List[i].referenceOfPrefab == prefab.gameObject)
                    return this._prefabPools_List[i];
            }

            // Nothing found
            return null;
        }


        /// <summary>
        ///  通过实例返回此实例所在的池对象，参数类型GameObject
        /// </summary>
        /// <param name="prefab">The GameObject of an instance</param>
        /// <returns>PrefabPool</returns>
        public PrefabPool GetPrefabPool(GameObject prefab)
        {
            for (int i = 0; i < this._prefabPools_List.Count; i++)
            {
                if (this._prefabPools_List[i].referenceOfPrefab == null)
                    Debug.LogError(string.Format("SpawnPool {0}: PrefabPool.prefabGO is null",
                                                 this.poolName));

                if (this._prefabPools_List[i].referenceOfPrefab == prefab)
                    return this._prefabPools_List[i];
            }

            // Nothing found
            return null;
        }


        /// <summary>
        /// 返回此实例的预制体
        /// </summary>
        /// <param name="instance">The Transform of an instance</param>
        /// <returns>Transform</returns>
        public Transform GetPrefab(Transform instance)
        {
            for (int i = 0; i < this._prefabPools_List.Count; i++)
                if (this._prefabPools_List[i].Contains(instance))
                    return this._prefabPools_List[i].prefab;

            // Nothing found
            return null;
        }


        /// <summary>
        /// 返回此实例的预制体
        /// </summary>
        /// <param name="instance">The GameObject of an instance</param>
        /// <returns>GameObject</returns>
        public GameObject GetPrefab(GameObject instance)
        {
            for (int i = 0; i < this._prefabPools_List.Count; i++)
                if (this._prefabPools_List[i].Contains(instance.transform))
                    return this._prefabPools_List[i].referenceOfPrefab;

            // Nothing found
            return null;
        }

        #endregion Utility Functions


        #region 继承自接口的方法
        /// <summary>
        /// 返回一个格式化字符串显示所有显示成员的名字
        /// </summary>
        public override string ToString()
        {
            // Get a string[] array of the keys for formatting with join()
            var name_list = new List<string>();
            foreach (Transform item in this._active_List)
                name_list.Add(item.name);

            // Return a comma-sperated list inside square brackets (Pythonesque)
            return System.String.Join(", ", name_list.ToArray());
        }


        /// <summary>
        /// Read-only index access. You can still modify the instance at the given index.
        /// Read-only reffers to setting an index to a new instance reference, which would
        /// change the list. Setting via index is never needed to work with index access.
        /// </summary>
        /// <param name="index">int address of the item to get</param>
        /// <returns></returns>
        public Transform this[int index]
        {
            get { return this._active_List[index]; }
            set { throw new System.NotImplementedException("Read-only."); }
        }

        /// <summary>
        /// The name "Contains" is misleading so IsSpawned was implimented instead.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(Transform item)
        {
            string message = "Use IsSpawned(Transform instance) instead.";
            throw new System.NotImplementedException(message);
        }


        /// <summary>
        /// Used by OTHERList.AddRange()
        /// This adds this list to the passed list
        /// </summary>
        /// <param name="array">The list AddRange is being called on</param>
        /// <param name="arrayIndex">
        /// The starting index for the copy operation. AddRange seems to pass the last index.
        /// </param>
        public void CopyTo(Transform[] array, int arrayIndex)
        {
            this._active_List.CopyTo(array, arrayIndex);
        }


        /// <summary>
        /// Returns the number of items in this (the collection). Readonly.
        /// </summary>
        public int Count
        {
            get { return this._active_List.Count; }
        }


        /// <summary>
        /// Impliments the ability to use this list in a foreach loop
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Transform> GetEnumerator()
        {
            for (int i = 0; i < this._active_List.Count; i++)
                yield return this._active_List[i];
        }

        /// <summary>
        /// Impliments the ability to use this list in a foreach loop
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < this._active_List.Count; i++)
                yield return this._active_List[i];
        }

        // Not implemented
        public int IndexOf(Transform item) { throw new System.NotImplementedException(); }
        public void Insert(int index, Transform item) { throw new System.NotImplementedException(); }
        public void RemoveAt(int index) { throw new System.NotImplementedException(); }
        public void Clear() { throw new System.NotImplementedException(); }
        public bool IsReadOnly { get { throw new System.NotImplementedException(); } }
        bool ICollection<Transform>.Remove(Transform item) { throw new System.NotImplementedException(); }


        #endregion
    }
}