# README

----------------------------------------------------------

## KooFrame 是我个人学习使用的知识整理框架

* Koo是Knowledge Of Organization的缩写
  此框架是我学习了很多成熟框架 只是一个为了统一合作 知识整理 编程学习用的框架

1. 框架的部分System静态API 学习自JKFrame、GameFramework、QFramework等各种知名框架 代码著作权依然归属于这些开源作者

**并不是一定要按照框架来开发 请以自己喜欢的方式进行 这里提供的仅仅是一些方便开发的工具集**

来源URL
> JKFrame: http://www.yfjoker.com/  
> GameFramework: https://gameframework.cn/  
> QFramework: https://github.com/liangxiegame/QFramework

2. 本框架做为整理归纳 用作个人学习，团队Demo内部学习使用

部分API名称和内容 根据 游戏项目的不同 都有部分修改调整

----------------------------------------------------------

# 框架本地文档：

## 框架-游戏工具系统

框架的目的是

1. 统一API的使用
2. 对项目的信息共享做出自定义
3. 为开发做方便的工具

命名意义：

但如果这些API只是像一堆小巧精良的工具 那么 称之为            **Tools 工具集**

Tools里有各类的作为细粒度最小的工具方法 来进行辅助开发称为：   **Kit 工具**

当游戏某个功能依赖一些单例或静态的管理器 我们就称之为       **Managers 管理器**

当一类API贯穿了整体游戏底层 那么我们称之为                  **System 工具系统**

## **对象池 PoolSystem**

用于减少性能开销 ，实现重复利用对象实例，减少触发垃圾回收

对象池的结构为3层结构 可以简单理解为

**Data(抽屉) -> Module(柜子) -> System (统一调用API)**

两类对象池（GameObject对象池和Object对象池）

分别负责对需要在场景中实际激活/隐藏的GameObject和不需要显示在场景里的对象（脚本类、材质资源）进行管理

框架提供对象池容量的限制，且初始化时，可以预先传入要放入的对象根据默认容量实例化放入对象池

注意！对象池的初始化并不意味着对象池容量有限制，容量可以设置为无限大

之所以需要初始化 理由如下

1. 便于利用字典 进行对象池可视化和统一管理
2. 如果不想去初始化、声明对象池 鼓励使用资源管理API 直接加载对象 再统一推入对象池

### **GameObject对象池**

#### **主要API**

```
//根据keyName初始化GOP
//(string keyName, int maxCapacity = -1,GameObject prefab = null,int defaultQuantity = 0)
PoolSystem.InitGameObjectPool(keyName, maxCapacity, prefab, defaultQuanity);
PoolSystem.InitGameObjectPool(keyName, maxCapacity);
PoolSystem.InitGameObjectPool(keyName);
//根据prefab.name初始化GOP
//  //(GameObject prefab = null, int maxCapacity = -1,GameObject prefab = null,int defaultQuantity = 0)
PoolSystem.InitGameObjectPool(prefab, maxCapacity, defaultQuanity);
PoolSystem.InitGameObjectPool(prefab, maxCapacity);
PoolSystem.InitGameObjectPool(prefab);
//根据GameObject数组进行默认容量设置
//(string keyName, int maxCapacity = -1, GameObject[] gameObjects = null)
PoolSystem.InitGameObjectPool(keyName, maxCapacity, gameObject);

//简单示例
// 设定一个子弹Bullet对象池，最大容量30,默认填满
Gameobject bullet = GameObject.Find("bullet");
PoolSystem.InitGameObjectPool("Bullet", 30, bullet, 30);
PoolSystem.InitGameObjectPool(bullet, 30, 30);

//最简形式
PoolSystem.InitGameObjectPool(“对象池名字”);
```

**参数说明：**

- 通过keyName或者直接传入prefab根据prefab.name 指定对象池的名字。
- 可设置对象池最大容量maxCapacity（超过maxCapacity再放入对象会被Destroy掉）。
- 可通过prefab和defaultQuanity设置默认容量（初始化时会自动按默认容量和最大容量的最小值自动生成GameObject放入对象池）。
- 可通过传入GameObject数组初始化对象池的默认容量。
- maxCapacity, prefab, defaultQuantity可不填，默认无限容量maxCapacity = -1，不预先放入对象，prefab = null，
  defaultQuantity = 0。
- defaultQuantity必须小于maxCapacity且如果想使用defaultQuantity则必须填入maxCapacity。
-

**拓展功能****：**

- 可以通过重复初始化一个对象池的maxCapacity实现容量的更改，此时如果重新指定了defaultQuanity，则会补齐差量个数的对象进对象池。

```
//根据keyName/obj.name放入对象池
//(string assetName, GameObject obj)
PoolSystem.PushGameObject(keyName, obj);
PoolSystem.PushGameObject(obj);

//简单示例
//将一个子弹对象bullet放入Bullet对象池
PoolSystem.PushGameObject("Bullet", bullet);

//拓展方法
//当前对象放入对象池
//(this GameObject go)
xx.GameObjectPushPool();
//(this Component com)
xx.GameObjectPushPool();
//(this object obj)
xx.ObjectPushPool();
```

**参数说明：**

- 通过keyName指定对象池名字放入对象obj，keyName不填则默认对象池名字为obj.name。

**拓展功能：**

- 可以使用拓展方法直接将对象放入同名对象池内。
- 如果传入的keyName/prefab找不到对应的对象池（未Init），则会直接初始化生成一个同名的，无限容量的对象池并放入本次对象。
- obj为null时本次放入操作无效，会进行报错提示。

```
//根据keyName加载GameObject
//(string keyName, Transform parent)
PoolSystem.GetGameObject(keyName, parent);
PoolSystem.GetGameObject(keyName);
//根据keyName和T加载GameObject并获取组件
PoolSystem.GetGameObject<T>(keyName, parent);
PoolSystem.GetGameObject<T>(keyName);

//先查看对象池 如果没有就通过资源管理直接实例化
PoolSystem.GetOrNewObject<T>(GameObject prefab)
PoolSystem.GetOrNewObject<T>(string assetName)

//简单实例
//将一个子弹对象从对象池中取出
GameObject bullet = PoolSystem.GetGameObject("Bullet");
//将一个子弹对象从对象池中取出并获取其刚体组件
GameObject bullet = PoolSystem.GetGameOjbect<Rigidbody>("Bullet");
```

**参数说明：**

- 通过keyName指定对象池名字取出GameObject对象并设置父物体为parent，parent不填则默认无父物体在最顶层。
- 可以通过**泛型T**传参获取对象上的某个组件，组件依托于GameObject存在，因此物体此时也已被从对象池中取出。

**拓展功能：**

- 当某个对象池内无对象时，其对象池仍会被保存，只有通过Clear才能彻底清空对象池。
- 当对象池中无对象仍要取出时，会返回null。

### **Object对象池**

#### **主要API ：**

```
//根据keyName初始化OP
//(string keyName, int maxCapacity = -1, int defaultQuantity = 0)
PoolSystem.InitObjectPool<T>(keyName, maxCapacity, defaultQuanity);
PoolSystem.InitObjectPool<T>(keyName, maxCapacity);
PoolSystem.InitObjectPool<T>(keyName);
//根据T的类型名初始化OP
PoolSystem.InitObjectPool<T>(maxCapacity, defaultQuanity);
PoolSystem.InitObjectPool<T>(maxCapacity);
PoolSystem.InitObjectPool<T>();
//根据keyName初始化OP，不考虑默认容量，无需传T
PoolSystem.InitObjectPool(keyName, maxCapacity);
PoolSystem.InitObjectPool(keyName);
//根据type类型名初始化OP
//System.Type type, int maxCapacity = -1
PoolSystem.InitObjectPool(xx.GetType());

//简单示例
// 设定一个Data数据类对象池，最大容量30,默认填满
PoolSystem.InitObjectPool<Data>("myData",30,30);
PoolSystem.InitObjectPool<Data>(30, 30);
```

说明：

- 通过keyName或者直接传入T根据T的类型名指定对象池的名字。
- 可设置对象池最大容量maxCapacity（超过maxCapacity再放入对象会被Destroy掉）。
- 可通过T和defaultQuanity设置默认容量（初始化时会自动按默认容量和最大容量的最小值自动生成Object放入对象池）。
- 泛型T起两个作用，
    - 一个是不指定keyName时用于充当type名称，
    - 另一个是进行默认容量设置时指定预先放入对象池的对象类型，所以如果不想用默认容量功能可以使用不传T的API。
- maxCapacity, prefab, defaultQuantity可不填，默认无限容量maxCapacity = -1，不预先放入对象，prefab = null，
  defaultQuantity = 0。
- defaultQuantity必须小于maxCapacity且如果想使用defaultQuantity则必须填入maxCapacity。

用法：

- 可以通过重复初始化一个对象池的maxCapacity实现容量的更改，此时如果重新指定了defaultQuanity，则会补齐差量个数的对象进对象池。
- OP的初始化和GOP略有不同，使用了泛型T传递类型，参数列表更加精简，但只有有泛型参数的重载方法可以进行默认容量的初始化（需要指定泛型T进行类型转换）。
- 可以选择通过传入某个实例的type类型，初始化同名的无限容量ObjectPool。

```
//根据keyName/obj.getType().FullName即obj对应的类型名放入对象池
//(object obj, string keyName)
PoolSystem.PushObject(obj, keyName);
PoolSystem.PushObject(obj);

//简单示例
//将一个Data数据类对象data放入Data对象池
PoolSystem.PushObject(data, "Data");
PoolSystem.PushObject(data);

// 扩展方法
bullet.ObjectPushPool();
```

说明

- 通过keyName指定对象池名字放入对象obj，keyName不填则默认对象池名字为obj.name。

用法

- 可以使用拓展方法直接将对象放入同名对象池内。
- 如果传入的keyName/obj找不到对应的对象池（未Init），则会直接初始化生成一个同名的，无限容量的对象池并放入本次对象。
- obj为null时本次放入操作无效,会进行报错提示。

```
//根据keyName返回System.object类型对象
//(string keyName)
PoolSystem.GetObject(keyName);
//根据keyName返回T类型的对象
PoolSystem.GetObject<T>(keyName);
//根据T类型名称返回对象
PoolSystem.GetObject<T>();
//根据type类型名返回对象
//(System.Type type)
PoolSystem.GetObject(xx.getType());

//这里同样提供GetOrNew 如果并没有Init对象池 就直接通过资源中心进行实例化(不进行计数) 
//这里我想说的是 你为什么不直接new一个出来呢？？？ 也许你不知道这个对象是否在对象池吧
PoolSystem.GetOrNewObject<T>();

//简单实例
//将一个Data数据类对象data从对象池中取出
Data data = PoolSystem.GetObject("Data");
Data data = PoolSystem.GetObject<Data>();
```

**参数说明：**

- 通过keyName，泛型T，type类型指定对象池名字取出Object对象。
- 优先根据keyName索引，不存在keyName时，则通过泛型T的反射类型和type类型名索引
- 这里同样提供GetOrNew 如果并没有Init对象池 就直接通过资源中心进行实例化(不进行计数) 这里我想说的是 你为什么不直接new一个出来呢？？？
  也许你不知道这个对象是否在对象池吧

**拓展功能：**

- 推荐使用泛型方法，否则返回值是object类型还需要手动进行转换。

```
//清空（GameObject/Object）对象池
//(bool clearGameObject = true, bool clearCSharpObject = true)
PoolSystem.ClearAll(true, false);
//清空GameObject类对象池中keyName索引的对象池
//(string assetName)
PoolSystem.ClearGameObject(keyName);

//简单实例
//清空所有GOP对象池
PoolSystem.ClearAll(true,false);
//清空Bullet对象池
PoolSystem.ClearGameObject("Bullet");
```

### 对象池的可视化

通过KooFrame/查询/PoolSystemViewer 打开对象池查看器

### **注意**

- 对象池的名字可以和放入的对象名字不同，并且每一个放入对象池的对象名词也可以不同（只要类型一致），但为了避免混淆，我们推荐同名（同类名或者同GameObject名）或者使用配置、枚举来记录对象池名。
- PoolSystem可以直接使用，但大多情况下，推荐使用ResSystem来获取GameObject/Object对象来保证返回值不为null。

## **资源系统 ResSystem**

资源系统封装继承了两种资源管理方式的API

资源管理方式：

- Resources版本，关联对象池进行资源的加载、卸载。
- Assetbundle版本 待完善
- Addressables版本，除关联对象池进行资源的加载、卸载外，
    - 结合事件工具实现对象Destroy时Adressables自动unload。

###  

在这里 我们优先介绍

### **Addressable版本**

这是我们项目使用的模式 (基于的是UnityAddressable包) 能更系统的管理资源路径

资源管理与对象池深度结合

大部分API都很类似

#### **普通类对象Object**

类对象资源不涉及异步加载、Resources和Addressables的区别，直接调用对象池系统API，**两个版本的API完全相同**。

##### **初始化**

```
//根据keyName初始化OP
//(string keyName, int maxCapacity = -1, int defaultQuantity = 0)
ResSystem.InitObjectPool<T>(keyName, maxCapacity, defaultQuanity);
ResSystem.InitObjectPool<T>(keyName, maxCapacity);
ResSystem.InitObjectPool<T>(keyName);
//根据T的类型名初始化OP
ResSystem.InitObjectPool<T>(maxCapacity, defaultQuanity);
ResSystem.InitObjectPool<T>(maxCapacity);
ResSystem.InitObjectPool<T>();
//根据keyName初始化OP，不考虑默认容量，无需传T
ResSystem.InitObjectPool(keyName, maxCapacity);
ResSystem.InitObjectPool(keyName);
//根据type类型名初始化OP
//System.Type type, int maxCapacity = -1
ResSystem.InitObjectPool(xx.GetType());

//简单示例
// 设定一个Data数据类对象池，最大容量30,默认填满
ResSystem.InitObjectPool<Data>("myData",30,30);
ResSystem.InitObjectPool<Data>(30, 30);
```

**参数说明：**

- 通过keyName或者直接传入T根据T的类型名指定对象池的名字。
- 可设置对象池最大容量maxCapacity（超过maxCapacity再放入对象会被Destroy掉）。
- 可通过T和defaultQuanity设置默认容量（初始化时会自动按默认容量和最大容量的最小值自动生成T类型的对象放入对象池）。
- 泛型T起两个作用，一个是不指定keyName时用于充当type名称，另一个是进行默认容量设置时指定预先放入对象池的对象类型，所以如果不想用默认容量功能可以使用不传T的API。
- maxCapacity, prefab, defaultQuantity可不填，默认无限容量maxCapacity = -1，不预先放入对象，prefab = null，
  defaultQuantity = 0。
- defaultQuantity必须小于maxCapacity且如果想使用defaultQuantity则必须填入maxCapacity。

**扩展功能:**

- 可以通过重复初始化一个对象池的maxCapacity实现容量的更改，此时如果重新指定了defaultQuanity，则会补齐差量个数的对象进对象池。
- 只有有泛型参数的重载方法可以进行默认容量的初始化（需要指定泛型T进行类型转换）。
- 可以选择通过传入某个实例的type类型，初始化同名的无限容量OP。

##### **Obj资源的加载**

```
//根据keyName从对象池中获取一个T类型对象，没有则new
//string keyName
ResSystem.GetOrNew<T>(keyName);
//根据T类型名从对象池中获取一个T类型对象，没有则new
ResSystem.GetOrNew<T>();

//简单示例,获取Data数据类的一个对象
GameObject go1 = ResSystem.GetOrNew<Data>("Data");
```

**参数说明**

- 通过keyName指定加载的类对象名，不填keyName则按照T的类型名加载。

**扩展功能:**

- 加载时优先通过对象池获取，如果对象池中无对应资源，自动new一个类对象返回，保证返回值不为null。

######  

##### **Obj资源的卸载**

```
//根据keyName/obj类型名将obj放回对象池
//object obj, string keyName
ResSystem.PushObjectInPool(obj);
ResSystem.PushObjectInPool(obj, string keyName);

//简单示例，卸载Data类的对象data
ResSystem.PushObjectInPool(data, "Data");
```

**参数说明**

- 通过obj指定卸载的对象，keyName指定对象池名，不填则按照obj的类型名卸载。

**扩展功能:**

- 卸载对象时如果没有初始化过对象池，则对应自动创建一个同名无限量对象池并将obj放入。

#### **游戏对象GameObject**

##### **初始化资源对象池**

资源系统的底层基于对象池系统，所以在资源系统层面也开放对对象池的初始化设置，API和PoolSystem有区别，Addressables通过Addressables
name直接实例化GameObject数组设置默认容量（避免资源加载依赖预制体导致卸载时产生BUG），Res需要传路径来获取prefab。

```
//根据keyName初始化GOP
//(string keyName, int maxCapacity = -1, string assetName = null, int defaultQuantity = 0)
ResSystem.InitGameObjectPoolForKeyName(keyName, maxCapacity,assetName, defaultQuantity);
ResSystem.InitGameObjectPoolForKeyName(keyName, maxCapacity);
ResSystem.InitGameObjectPoolForKeyName(keyName);
//根据assetName在Addressables中的资源名初始化GOP
//(string assetName, int maxCapacity = -1, int defaultQuantity = 0)
ResSystem.InitGameObjectPoolForAssetName(assetName, maxCapacity, defaultQuantity);
ResSystem.InitGameObjectPoolForAssetName(assetName, maxCapacity);
ResSystem.InitGameObjectPoolForAssetName(assetName);


//简单示例
// 设定一个子弹Bullet对象池（假设Addressable资源名称为Bullet），最大容量30,默认填满
Gameobject bullet = GameObject.Find("bullet");
ResSystem.InitGameObjectPool("Bullet", 30, bullet, 30);
ResSystem.InitGameObjectPool(bullet, 30, 30);

//最简形式
ResSystem.InitGameObjectPool(“对象池名字”);
```

**参数说明：**

- 通过keyName或者直接传入assetName（Addressable资源的名称）根据获取的资源名指定对象池的名字。
- 可设置对象池最大容量maxCapacity（超过maxCapacity再放入对象会被Destroy掉）。
- 可通过assetName和defaultQuanity设置默认容量（初始化时会自动按默认容量和最大容量的最小值自动加载GameObject放入对象池）。
- 默认无限容量maxCapacity = -1，不预先放入对象，prefab = null， defaultQuantity = 0。
- defaultQuantity必须小于maxCapacity且如果想使用defaultQuantity则必须填入maxCapacity。

**扩展功能:**

- 可以通过重复初始化一个对象池的maxCapacity实现容量的更改，此时如果重新指定了defaultQuanity，则会补齐差量个数的对象进对象池。

##### **加载并实例化**

Addressable版本中游戏物体参数通过Addressable资源名assetName（Res是资源路径assetPath）指定，支持加载出的对象Destroy时在Addressables中自动释放。

```
//加载游戏物体
//(string assetName, Transform parent = null, string keyName = null, bool autoRelease = true)
ResSystem.InstantiateGameObject(assetName, parent, keyName, autoRelease);
ResSystem.InstantiateGameObject(assetName, parent, keyName);
ResSystem.InstantiateGameObject(assetName, parent);
ResSystem.InstantiateGameObject(assetName);
ResSystem.InstantiateGameObject(parent, keyName, autoRelease);
ResSystem.InstantiateGameObject(parent, keyName);
//加载游戏物体并获取组件T
ResSystem.InstantiateGameObject<T>(assetName, parent, keyName, autoRelease);
ResSystem.InstantiateGameObject<T>(assetName, parent, keyName);
ResSystem.InstantiateGameObject<T>(assetName, parent);
ResSystem.InstantiateGameObject<T>(assetName);
ResSystem.InstantiateGameObject<T>(parent, keyName, autoRelease);
ResSystem.InstantiateGameObject<T>(parent, keyName);
//异步加载(void)游戏物体
//(string assetName, Action<GameObject> callBack = null, Transform parent = null, string keyName = null, bool autoRelease = true)
ResSystem.InstantiateGameObjectAsync(assetName, callBack, parent, keyName, autoRelease);
ResSystem.InstantiateGameObjectAsync(assetName, callBack, parent, keyName);
ResSystem.InstantiateGameObjectAsync(assetName, callBack, parent);
ResSystem.InstantiateGameObjectAsync(assetName, callBack);
ResSystem.InstantiateGameObjectAsync(assetName);
//异步加载(void)游戏物体并获取组件T
//(string assetName, Action<T> callBack = null, Transform parent = null, string keyName = null, bool autoRelease = true)
ResSystem.InstantiateGameObjectAsync<T>(assetName, callBack, parent, keyName, autoRelease);
ResSystem.InstantiateGameObjectAsync<T>(assetName, callBack, parent, keyName);
ResSystem.InstantiateGameObjectAsync<T>(assetName, callBack, parent);
ResSystem.InstantiateGameObjectAsync<T>(assetName, callBack);
ResSystem.InstantiateGameObjectAsync<T>(assetName, callBack);
ResSystem.InstantiateGameObjectAsync<T>(assetName);

//简单示例
//实例化一个子弹对象（假设AB资源名称为Bullet）
GameObject bullet = ResSystem.InstantiateGameObject("Bullet");
//实例化一个子弹对象取出并获取其刚体组件
GameObject bullet = ResSystem.InstantiateGameObject("Bullet");
//异步实例化一个子弹对象,并在其加载完后坐标归零
void getBullet(GameObject bullet)
{
    bullet.transform.position = Vector3.zero;
    }
ResSystem.InstantiateGameObjectAsync("Bullet", getBullet);
```

**参数说明：**

- 通过assetPath加载游戏物体并实例化返回
- 实例化的游戏物体会设置父物体为parent，不填则默认为null无父物体在最顶层。
- 实例化的物体名称优先为keyName，keyName为null时则为assetName。
- 优先根据keyName从对象池获取，不填keyName则根据assetName在对象池中查找。
- 对象池中无缓存，则根据assetName从Addressable中获取资源,不填assetName则根据keyName从Addressable中获取资源。
- 可以通过**泛型T**传参获取对象上的某个组件，组件依托于GameObject存在，因此物体此时也已被从对象池中取出。
- autoRelease为true则通过事件工具为加载出的对象添加事件监听，会在其Destroy时自动调用Addressables的Release API。
- 异步加载游戏物体及其组件的方法返回值为void类型，无法直接加载的游戏物体，需要通过传入callback回调获取加载的对象并进行使用。

**扩展功能:**

- 如果资源路径正确，则返回值必不为空，优先从对象池中获取，对象池中不存在则根据Load的对象进行实例化返回。

##### **GameObject卸载**

放入对象池回收

```
//根据keyName/gameObject.name卸载gameObject
//(string keyName, GameObject gameObject)
ResSystem.PushGameObjectInPool(keyName, gameObject);
ResSystem.PushGameObjectInPool(gameObject);

//简单示例，卸载子弹对象bullet
ResSystem.PushGameObjectInPool(bullet, "Bullet");
```

**说明**

- 通过gameObject指定卸载的对象，keyName指定对象池名，不填则按照gameObject的对象名卸载。

- 卸载对象时如果没有初始化过对象池，则对应自动创建一个同名无限量对象池并将gameObject放入。

#### **Unity Asset资源**

这类资源不需要进行实例化，所以不需要过对象池，只需要使用数据或者引用，比如AudioClip，Sprite，prefab

##### **加载Asset**

```
//根据assetName异步加载T类型资源
//(string assetName, Action<T> callBack)
ResSystem.LoadAssetAsync<T>(string assetName, Action<T> callBack);
//根据assetName加载T类型资源
ResSystem.LoadAsset<T>(assetName);
//根据keyName异步加载所有资源（void）
//(string keyName, Action<AsyncOperationHandle<IList<T>>> callBack = null, Action<T> callBackOnEveryOne = null)
ResSystem.LoadAssetsAsync<T>(keyName, callBack, callBackOnEveryOne);
//根据keyName加载所有资源(IList<T>)
//(tring keyName, Action<T> callBack = null)
ResSystem.LoadAssets<T>(keyName, callBackOnEveryOne);

//简单示例，加载Addressable clip音频资源
ResSystem.LoadAssets<AudioClip>("clip");
```

**说明**

- 通过path路径加载资源，T用来指明加载的资源类型。
- 异步加载单个资源需要通过传入callback回调获取加载的资源并进行使用。
- 加载所有资源时不指定T则返回object数组，keyName是Addressable中的Labels。
- Addressable加载指定keyName的所有资源时，支持每加载一个资源调用一次callBackOnEveryOne。
- 异步加载完指定keyName所有资源时，调用callback获取加载的资源集合并进行使用。
- 注意加载的资源不会被自动释放。

##### **卸载Asset**

```
//释放某个资源
//（T obj）
ResSystem.UnloadAsset<T>(T obj);
//销毁对象并释放资源
//(GameObject obj)
ResSystem.UnloadInstance(obj);
```

卸载Asset即释放资源,可以在Destroy游戏对象的同时释放Addressable资源。

#### **生成资源引用代码**

对于Addressables 使用字符串来加载资源方式比较麻烦，而且容易输错，这里提供一种基于引用加载的方式

(注意 命名空间无法自动生成 请确保资源类型所在的命名空间都在 GameBuild SubSystem 内)

通过 KooFrame/资源工具/生成资源引用代码 Res.cs 在5.FrameSystems/2.ResSystem 内

```
//返回一个资源
Res.GroupName.AddressableName;
//返回一个资源的实例
//(Transform parent = null,string keyName=null,bool autoRelease = true)
Res.GroupName.AddressableName(parent, keyName, autoRelease);
Res.GroupName.AddressableName(parent, keyName);
Res.GroupName.AddressableName(parent);

//使用示例
//获取一个Bullet预制体资源（不实例化）
Gameobject bullet = Res.DefaultLocalGroup.Bullet;
//获取一个Bullet预制体资源并实例化
Gameobject bullet = Res.DefaultLocalGroup.Bullet(transform);
```

- Res是资源脚本的命名空间，固定。
- GroupName是Addressable的组名。
- AddressableName是资源名。
- 如果填写keyName，则先去对象池中找资源实例，找不着再通过Addressable获取资源并实例化。
- parent为实例的父物体。
- autoRelease为true则实例会在Destroy时自动释放Addressable中对应的资源。

##### **注意**

每次生成也都需要检查生成的代码是否有问题 本质是生成一个静态方法库

*请不要把资源名称设置成组的名称 这样会导致生成引用资源代码 字段名与类名相同*

- 对于Sprite的子图，也支持直接引用。

```
//子图
Res.LV2.Img_Img_0;
//总图
Res.Lv2.Img;
```

引用代码的维护指南：

### **Resources版本**

(初期原型 快速开发时候使用)

Resourcces的API与 Addressab大致相同

因为项目基本也不使用Resources了 所以这里基本也不会花时间去维护

## **事件中转系统 EventSystem**

为什么叫中转呢
框架里的事件系统主要负责高效的方法调用与数据传递，实现各功能之间的解耦，通常在调用某个实例的方法时，必须先获得这个实例的引用或者新实例化一个对象，低耦合度的框架结构希望程序本身不去关注被调用的方法所依托的实例对象是否存在，通过事件系统做中转将功能的调用封装成事件，使用事件监听注册、移除和事件触发完成模块间的功能调用管理。常用在UI事件、跨模块事件上。事件系统支持无返回值的Action，Func实际应用意义不大。

### 事件监听添加

```
//添加无参数的事件监听
//string eventName, Action action
EventSystem.AddEventListener(eventName, action); 
//添加多个参数的事件监听
//string eventName
EventSystem.AddEventListener<T>(eventName, action);
EventSystem.AddEventListener<T0, T1>(eventName, action);
EventSystem.AddEventListener<T0, T1, T2>(eventName, action);
...
EventSystem.AddEventListener<T0, T1, ..., T15>(eventName, action);

//简单示例
//添加无参数的事件监听,Doit方法对应名称为Test的事件
EventSystem.AddEventListener("Test", Doit);
void Doit()
{
    Debug.Log("Doit");
}
//添加多个参数的事件监听，Doit2对应名称为TestM的事件，参数为int，string
EventSystem.AddEventListener<int, string>("TestM", Doit2);
void Doit2(int a, string b)
{
    Debug.Log(a);
    Debug.Log(b); 
}
```

说明：

- eventName是解耦执行的方法的标记，即事件名，是触发事件时的唯一依据。
- action传无返回值方法。
- T0~T15是泛型，用于指定参数表,支持最多16个参数的action。

### **事件监听移除**

```
//添加无参数的事件监听
//string eventName, Action action
EventSystem.RemoveEventListener(eventName, action); 
//添加多个参数的事件监听
//string eventName
EventSystem.RemoveEventListener<T>(eventName, action);
EventSystem.RemoveEventListener<T0, T1>(eventName, action);
EventSystem.RemoveEventListener<T0, T1, T2>(eventName, action);
...
EventSystem.RemoveEventListener<T0, T1, ..., T15>(eventName, action);

//简单示例
//移除无参数的事件监听,Doit方法对应名称为Test的事件
EventSystem.RemoveEventListener("Test", Doit);
void Doit()
{
    Debug.Log("Doit");
}
//移除多个参数的事件监听，Doit2对应名称为TestM的事件，参数为int，string
EventSystem.RemoveEventListener<int, string>("TestM", Doit2);
void Doit2(int a, string b)
{
    Debug.Log(a);
    Debug.Log(b); 
}
```

**参数说明**

- eventName是解耦执行的方法的标记，即事件名，是触发事件时的唯一依据。
- action传无返回值方法。
- T0~T15是泛型，用于指定参数表,支持最多16个参数的action。

### **事件触发**

```
//触发无参数事件
EventSystem.EventTrigger(string eventName);
//触发多个参数事件
EventSystem.EventTrigger<T>(string eventName, T arg);
EventSystem.EventTrigger<T0, T1>(string eventName, T0 arg0, T1 arg1);
EventSystem.EventTrigger<T0, T1,..., T15>(string eventName, T0 arg0, T1 arg1, ..., T15 arg15);
 
//简单示例，使用添加监听的方法例子
EventSystem.EventTrigger("Test");
EventSystem.EventTrigger<int,string>("TestM",1,"test");
```

**参数说明**

- eventName是解耦执行的方法的标记，即事件名，是触发事件时的唯一依据。
- T0~T15是泛型，用于指定参数表,支持最多16个参数的action。
- 事件的查询底层使用TryGetValue所以触发不存在的事件并不会报错。

### **事件移除**

事件移除和事件监听移除的区别参与：

- 事件监听移除只移除一条Action，比如添加了3次同名事件监听，则移除一次后触发还是会执行两次，且eventName记录不会被移除。
- 事件移除会将事件中心字典中有关eventName的记录连带存储的Action一同清空。

```
//移除一类事件
//(string eventName)
EventSystem.RemoveEvent(eventName);
//移除事件中心中所有事件
EventSystem.Clear();
```

**参数说明**

- eventName是解耦执行的方法的标记，即事件名，是触发事件时的唯一依据。

### **注意**

事件系统的运行逻辑是，预先添加/移除事件监听，再在能够获取相应参数的类内触发事件。

## **音效系统 AudioSystem**

音效服务集成了背景、特效音乐播放，音量、播放控制功能。包含了全局音量globalVolume、背景音量bgVolume、特效音量effectVolume、静音布尔量isMute、暂停布尔量isPause等音量相关的属性，播放背景音乐的PlayBGAudio方法且，播放特效音乐PlayOnShot方法且重载后支持在指定位置或绑定游戏对象播放特定的音乐，特效音乐由于要重复使用，可以从对象池中获取播放器并自动回收，支持播放后执行回调事件。

**注意 这个系统的好处只是集成了对象池 方便管理资源缓存 但很多时候 游戏音乐都需要具体情况具体处理 所以这个系统还需要魔改
**

### **播放背景音乐**

音量、播放属性控制

音效服务支持在Inspector面板上的值发生变化时自动执行相应的方法更新音量属性,也可以在属性值变化时自动调用相应的更新方法。

```
//全局音量(float,0~1),音量设定为50%
AudioSystem.GlobalVolume = 0.5f;
//背景音乐音量(float,0~1)，音量设定为50%
AudioSystem.BGVolume = 0.5f;
//特效音乐音量(flaot,0~1)
AudioStystem.EffectVolume = 0.5f;
//是否全局静音(bool),true则静音
AudioSystem.IsMute = true;
//背景音乐是否循环,true则循环
AudioSystem.IsLoop = true;
//背景音乐是否暂停，true则暂停
AudioSystem.IsPause = true;
```

**参数说明**

- GlobalVolume是全局音量，同时影响背景、特效音乐音量。
- BGVolume是背景音乐音量
- EffectVolume是特效音乐音量。
- IsMute控制全局音量是否静音。
- IsLoop控制背景音乐是否循环。
- IsPause控制背景音乐是否暂停。

**拓展**

支持通过面板更新音量属性。

![img](https://alidocs.oss-cn-zhangjiakou.aliyuncs.com/res/8oLl97KK5xdgqapY/img/85201ba8-0232-4632-9a6e-129ddb8e1e79.png)

### **播放背景音乐**

```
//播放背景音乐
//(AudioClip clip, bool loop = true, float volume = -1)
AudioSystem.PlayBGAudio(clip, loop, volume);
//轮播多个背景音乐
//(AudioClip[] clips, float volume = -1)
AudioSystem.PlayBGAudioWithClips(clips, volume);
//停止当前背景音乐
AudioSystem.StopBGAudio();
//暂停当前背景音乐
AudioSystem.PauseBGAudio();
//取消暂停当前音乐
AudioSystem.UnPauseBGAudio();

//简单示例
AudioClip clip = ResSystem.LoadAsset<AudioClip>("music");
AudioSystem.PlayBGAudio(clip);
```

**参数说明**

- clip是音乐片段，可以传clip数组来轮播音乐。
- volume是音乐的音量，不指定则按原来的背景音量。
- 停止当前背景音乐会将当前背景音乐置空。
- 暂停音乐可取消暂停恢复。

### **播放特效音乐**

```
//播放一次音效并绑定到游戏物体上，位置随物体变化
//(AudioClip clip, Component component = null, bool autoReleaseClip = false, float volumeScale = 1, bool is3d = true, Action callBack = null)
audioSystem.PlayOnShot(clip, component, autoReleaseClip, volumeScale, is3d, callBack);
audioSystem.PlayOnShot(clip, component, autoReleaseClip, volumeScale, is3d);
audioSystem.PlayOnShot(clip, component, autoReleaseClip, volumeScale);
audioSystem.PlayOnShot(clip, component, autoReleaseClip);
audioSystem.PlayOnShot(clip, component);
audioSystem.PlayOnShot(clip);
//在指定位置上播放一次音效
//(AudioClip clip, Vector3 position, bool autoReleaseClip = false, float volumeScale = 1, bool is3d = true, Action callBack = null)
audioSystem.PlayOnShot(clip, position, autoReleaseClip, volumeScale, is3d, callBack);
audioSystem.PlayOnShot(clip, position, autoReleaseClip, volumeScale, is3d);
audioSystem.PlayOnShot(clip, position, autoReleaseClip, volumeScale);
audioSystem.PlayOnShot(clip, position, autoReleaseClip);
audioSystem.PlayOnShot(clip, position);
//简单示例
//在玩家位置播放一次音效
AudioClip clip = ResSystem.LoadAsset<AudioClip>("music");
audioSystem.PlayOnShot(clip,player.transform.position);
//绑定玩家组件播放一次音效（等同于玩家位置）
audioSystem.PlayOnShot(clip,player.transform);
```

**参数说明**

- clip是音乐片段，音效系统中特效音乐在每次播放时优先从对象池中取出挂载了AudioSource的GameObject实例生成并会在音效播放完成后自动回收。
- postion是播放的位置，必填。
- component是绑定的组件，这个API的目的是让音效随着物体移动一起移动，不填则默认不绑定。
- autoReleaseClip代表是否需要在音乐播放结束后自动释放clip资源，Res和Addressable均可。
- volumeScle是音乐的音量，不指定默认按最大音量。
- is3D是启用空间音效，默认开启。
- callBack是回调事件，会在音效播放完执行一个无参无返回值方法。

**拓展功能**

使用Compoent绑定播放音效时，如果绑定物体如果在播放中被销毁了，那么AudioSource会提前解绑避免一同被销毁（通过事件工具提前添加监听），之后播放完毕会自动回收。

## **存档系统 SaveSystem**

完成对存档的创建，获取，保存，加载，删除，缓存，支持多存档。存档有两类，一类是用户型存档，存储着某个游戏用户具体的信息，如血量，武器，游戏进度，一类是设置型存档，与任何用户存档都无关，是通用的存储信息，比如屏幕分辨率、音量设置等。

后期会集合加入数据库的工具API

### **存档方式**

在FrameSettings中可以调整

存档系统支持两类本地文件：**二进制流文件、Json**
，两者通过框架设置面板进行切换，切换时，原本地文件存档会清空！二进制流文件可读性较差不易修改，Json可读性较强，易修改，存档的数据存在Application.persistentDataPath下。

![img](https://alidocs.oss-cn-zhangjiakou.aliyuncs.com/res/8oLl97KK5xdgqapY/img/d23e8181-e6cb-4273-bec8-d514117d774c.png)

SaveData和setting分别存储用户存档和设置型存档。SaveData和setting分别存储用户存档和设置型存档。

用户存档下根据saveID分成若干文件夹用于存储具体的对象。

### **设置型存档**

设置存档实际就是一个全局唯一的存档，可以向其中存储全局通用数据。

#### **保存设置**

```
//保存设置到全局存档
//(object saveObject, string fileName)
SaveSystem.SaveSetting(saveObject, fileName);
SaveSystem.SaveSetting(saveObject)
//简单示例
//见下一小节结合加载说明
```

**参数说明**

- saveObject是要保存的对象，System.Object类型。
- fileName是保存的文件名称，不填默认取saveObject的类型名。

#### 加载设置

```
//从设置存档中加载设置
// string fileName
SaveSystem.LoadSetting<T>(fileName);
SaveSystem.LoadSetting<T>();

//简单示例
// GameSetting类中存储着游戏名称，作为全局数据
[Serializable]
public class GameSetting
{
    public string gameName;
}
GameSetting gameSetting = new GameSetting();
gameSetting.gameName = "测试";
//保存设置
SaveSystem.SaveSetting(gameSetting);
//取出来用
String gameName = SaveSystem.LoadSetting<gameSetting>().gameName;
```

#### **删除设置**

```
//删除用户存档和设置存档
SaveSystem.DeleteAll();
```

**参数说明**

- fileName是加载设置存档的文件名，T限定了所存储的数据类型，不填fileName则默认以T的类型名作为文件名加载。

### **用户存档**

用户存档与具体的用户相关，不同用户存档位置不同，数据也不同，索引为SaveID。

#### **创建用户存档**

创建的存档索引默认自增。

```
SaveSystem.CreateSaveItem();
//简单示例
SaveItem saveItem = SaveSystem.CreateSaveItem();
```

#### **获取用户存档**

根据一定规则获取所有用户存档，返回List。

```
//最新的在最后面
SaveSystem.GetAllSaveItem();
//最近创建的在最前面
SaveSystem.GetAllSaveItemByCreatTime();
//最近更新的在最前面
SaveSystem.GetAllSaveItemByUpdateTime();
//万能解决方案，自定义规则
GetAllSaveItem<T>(Func<SaveItem, T> orderFunc, bool isDescending = false)

//简单示例,万能方案，按照SaveID倒序获得存档
GameSetting gameSetting = new GameSetting();
List<SaveItem> testList = SaveSystem.GetAllSaveItem<int>(oderFunc, true);
//List<SaveItem> testList = SaveSystem.GetAllSaveItem();
foreach (var item in testList)
{
    Debug.Log(item.saveID);
}
//排序依据Func
int oderFunc(SaveItem item)
{
    return item.saveID;
}
```

**参数说明**

- 提供多种重载方法获取存档List。
- 支持自定义排序依据的万解决方案，T传比较参数类型，orderFunc传比较方法。

##### 获取某一项用户存档

```
//(int id, SaveItem saveItem)
SaveSystem.GetSaveItem(id);
SaveSystem.GetSaveItem(saveItem);

//简单示例
SaveItem saveItem = SaveSystem.CreateSaveItem();
SaveSystem.GetSaveItem(saveItem);
```

**参数说明**

- id是用户存档的编号，存档系统会在创建时指定默认ID，使用时透明，因此推荐使用saveItem传参，saveItem是可维护的。

#### 删除用户存档

##### 删除所有用户存档

```
//删除所有用户存档
SaveSystem.DeleteAllSaveItem();
```

##### 删除某一项用户存档

```
//(int id, SaveItem saveItem)
SaveSystem.DeleteSaveItem(id);
SaveSystem.DeleteSaveItem(saveItem);

//简单示例
SaveItem saveItem = SaveSystem.CreateSaveItem();
SaveSystem.DeleteSaveItem(saveItem);
```

**参数说明**

- id是用户存档的编号，存档系统会在创建时指定默认ID，使用时透明，因此推荐使用saveItem传参，saveItem是可维护的。

### 存档对象层面

#### 保存用户存档中某一对象

```
//(object saveObject, string saveFileName, SaveItem saveItemint, saveID = 0)
SaveSystem.SaveObject(saveObject, saveFileName, saveID);
SaveSystem.SaveObject(saveObject, saveFileName, saveItem);
SaveSystem.SaveObject(saveObject, saveID);
SaveSystem.SaveObject(saveObject, saveItem);

//简单示例
SaveItem saveItem = SaveSystem.CreateSaveItem();
GameSetting gameSetting = new GameSetting();
SaveSystem.SaveObject(gameSetting, saveItem);
```

**参数说明**

- saveObject是要保存的对象。
- saveFileName是保存后生成的本地文件名（对象会单独作为一个文件存储在对应saveID的文件夹下），不填则以对象的类型名为文件名。
- saveID/SaveItem是对象存储的存档。
- 保存对象时会更新用户存档缓存。

#### **获取用户存档中某一对象**

```
//(string saveFileName, SaveItem saveItem, int saveID = 0)
SaveSystem.LoadObject<T>(saveFileName, saveID);
SaveSystem.LoadObject<T>(saveFileName, saveItem);
SaveSystem.LoadObject<T>(saveID);
SaveSystem.LoadObject<T>(saveItem);

//简单示例
SaveItem saveItem = SaveSystem.CreateSaveItem();
GameSetting gameSetting = new GameSetting();
SaveSystem.SaveObject(gameSetting, saveItem);
GameSetting gameSetting = SaveSystem.LoadObject<GameSetting>(saveItem);
```

**参数说明**

- T指定获取对象类型。
- saveFileName是获取对象的文件名，不填则默认以T的类型名作为文件名。
- saveID/SaveItem是对象存储的存档。
- 获取对象优先从缓存中读取，不存在则IO读文件获取，并加入缓存。

#### 删除用户存档中某一对象

```
//(string saveFileName, SaveItem saveItem, int saveID = 0)
SaveSystem.DeleteObject<T>(saveFileName, saveID);
SaveSystem.DeleteObject<T>(saveFileName, saveItem);
SaveSystem.DeleteObject<T>(saveID);
SaveSystem.DeleteObject<T>(saveItem);

//简单示例
SaveItem saveItem = SaveSystem.CreateSaveItem();
GameSetting gameSetting = new GameSetting();
SaveSystem.DeleteObject(gameSetting, saveItem);
GameSetting gameSetting = SaveSystem.DeleteObject<GameSetting>(saveItem);
```

**参数说明**

- T指定获取对象类型。
- saveFileName是获取对象的文件名，不填则默认以T的类型名作为文件名。
- saveID/SaveItem是对象存储的存档。
- 删除某一对象时，如果存在对应的缓存，则一并删除。

#### 注意

在从用户存档中取出对象时，底层优先从缓存中读取，避免读时IO，使用时无需关注。

## **Mono代理 MonoSystem**

一些非Mono类型的脚本 如果想实现Updata FixedUpdate等功能 我们统一托管给GameRoot这个游戏物体

将需要在Update、LateUpdate。FixedUpdate实际执行的逻辑托管给MonoSystem。

```
MonoSystem.AddUpdateListener(action);
MonoSystem.RemoveUpdateListener(action);
MonoSystem.AddLateUpdateListener(action);
MonoSystem.RemoveLateUpdateListener(action);
MonoSystem.AddFixedUpdateListener(action);
MonoSystem.RemoveFixedUpdateListener(action);
```

**参数说明**

- action是要在生命周期执行的无参无返回值方法。

### **协程**

```
//启动/停止一个协程
//(IEnumerator coroutine)
MonoSystem.Start_Coroutine(coroutine);
//(Coroutine routine)
MonoSystem.Stop_Coroutine(routine);

//启动/停止一个协程序并且绑定某个对象
//(object obj,IEnumerator coroutine)
MonoSystem.Start_Coroutine(obj, coroutine);
//(object obj,Coroutine routine)
MonoSystem.Stop_Coroutine(obj, routine);

//停止某个对象绑定的所有协程
MonoSystem.StopAllCoroutine(obj);

//停止所有协程
MonoSystem.StopAllCoroutine();
```

**参数说明**

- coroutine是一个迭代器，定义了协程。
- routine是要停止的协程。
- obj是与协程绑定的对象，可以用于区分不同对象上的相同协程。

### **协程工具**

提前new好协程所需要的WaitForEndOfFram、WaitForFixedUpdate、YieldInstruction类的对象,避免GC。

```
CoroutineTool.WaitForEndOfFrame();
CoroutineTool.WaitForFixedUpdate();
//(float time)
CoroutineTool.WaitForSeconds(float time);
//(float time)不受TimeScale影响
CoroutineTool.WaitForSecondsRealtime(float time);
//(int count=1)
CoroutineTool.WaitForFrames();

//使用示例
private static IEnumerator DoLoadSceneAsync(...)
{
...
    yield return CoroutineTool.WaitForFrames();
}
```

PS 根据实测 作用不是很大 还是会有GC 所以用不用无所谓

## UI框架 UISystem

UI框架实现对窗口的生命周期管理，层级遮罩管理，按键物理响应等功能，对外提供窗口的打开、关闭、窗口复用API，对内优化好窗口的缓存、层级问题，能够和场景加载、事件系统联动，将Model、View、Controller完全解耦。通过与配置系统、脚本可视化合作，实现新UI窗口对象的快速开发和已有UI窗口的方便接入。

### **数据结构**

UI框架的数据结构:

```
//UI窗口数据字典
    Dictionary<string, UIWindowData> UIWindowDataDic;

    //UI窗口数据类
    public class UIWindowData
    {
        [LabelText("是否需要缓存")] public bool isCache;
        [LabelText("预制体Path或AssetKey")] public string assetPath;
        [LabelText("UI层级")] public int layerNum;
        /// <summary>
        /// 这个元素的窗口对象
        /// </summary>
        [LabelText("窗口实例")] public UI_WindowBase instance;

        public UIWindowData(bool isCache, string assetPath, int layerNum)
        {
            this.isCache = isCache;
            this.assetPath = assetPath;
            this.layerNum = layerNum;
            instance = null;
        }
    }
```

UI框架的核心在于维护字典UIWindowDataDic，通过windowKey索引了不同的UI窗口数据UiWindowData，其中包含了窗口是否要缓存，资源路径，UI层级，以及窗口类实例（脚本作为窗口对象的组件，持有他就相当于持有了窗口gameObject），UIWindowData可以通过运行时动态加载也可以在Editor时通过特性静态加载，设计windowKey的原因是如果不额外标定windowKey直接用资源路径作为索引，则同一个窗口资源无法复用，换句话说，同一个UI窗口游戏对象及窗口类，通过不同的windowKey和实例可以进行重用。

### **UI窗口对象及类配置**

使用UI框架需要先为UI窗口游戏对象添加控制类，该类继承自UI_WindowBase,并将UI窗口游戏对象加入Addressable列表/Resources文件夹下。

### **UI窗口特性-Editor静态加载**

可以选择为UI窗口类打上UIWindowData特性（Attribute可省略）用于配置数据。

![img](https://alidocs.oss-cn-zhangjiakou.aliyuncs.com/res/8oLl97KK5xdgqapY/img/ace772fd-9c13-44d8-8eaf-cda4da9fef33.png)

```
UIWindowDataAttribute(string windowKey, bool isCache, string assetPath, int layerNum){}
UIWindowDataAttribute(Type type,bool isCache, string assetPath, int layerNum){}
```

**参数说明**

- 特性中windowKey是UI窗口的名字唯一索引，可以直接传string也可以传Type使用其FullName。
- isCache指明UI窗口游戏对象是否需要缓存重用，true则在窗口关闭时不会被销毁，下次使用时可以通过windowKey调用且不需要实例化。
- assetPath是资源的路径，在Resources中是UI窗口对象在Resources文件夹下的路径，Addressable中是UI窗口对象的Addressable Name。
- layerNum是UI窗口对象的层级，从0开始，越大则越接近顶层。
- 支持一个窗口类多特性，复用同一份窗口类资源，n个特性，则有n份UI窗口数据，本质上对应了多个windowKey，因此windowKey必须不同。

经过配置后，在Editor模式下该UI类特性数据及UI窗口游戏对象（此时还没有实例化为空）会自动保存到GameRoot的配置文件中，即静态加载。

### **UI窗口运行时动态加载**

在运行时动态加载UI窗口，不需要给窗口类打特性，窗口数据直接给出，与Onshow/OnClose不同，其不包含窗口游戏物体对象的显示/隐藏/销毁逻辑。

```
//(string windowKey, UIWindowData windowData, bool instantiateAtOnce = false)
UISystem.AddUIWindowData(windowKey, windowData, instantiateAtOnce);
UISystem.AddUIWindowData(windowKey, windowData, instantiateAtOnce);
//(Type type, UIWindowData windowData, bool instantiateAtOnce = false)
UISystem.AddUIWindowData(type, windowData, instantiateAtOnce);
UISystem.AddUIWindowData(type, windowData);
UISystem.AddUIWindowData<T>(windowData, instantiateAtOnce);
UISystem.AddUIWindowData<T>(windowData);

//简单实例
UISystem.AddUIWindowData("Test1", new UIWindowData(true, "TestWindow", 1));
//上一步只添加了数据，显示在面板上还需要激活
UISystem.Show<TestWindow>("Test1");
```

**参数说明**

- 通过泛型T指定UI窗口子类类型，windowKey为UI窗口类的索引，对应UIWindowData中的windowKey，不指定则使用T的类型名作为索引。
- instantiateAtOnce指明窗口对象及其类是否要进行实例化，默认为null，会在窗口打开时加载资源进行实例化且设置为不激活，若窗口资源较大，可以提前在动态加载时就进行实例化，如图。

![img](https://alidocs.oss-cn-zhangjiakou.aliyuncs.com/res/8oLl97KK5xdgqapY/img/13ee857b-23b6-469f-b70b-ca4a761c9258.png)

### **UI窗口数据管理**

获取UI窗口数据，其中包含UI的windowKey，层级，资源路径，以及对象实例，可以对其进行操作。

```
//(string windowKey) (Type windowType)
UISystem.GetUIWindowData(windowKey);
UISystem.GetUIWindowData<T>();
UISystem.GetUIWindowData(windowType);
//尝试获取UI窗口数据，返回bool
//(string windowKey, out UIWindowData windowData
UISystem.TryGetUIWindowData(windowKey, windowData);
//移除某条UI窗口数据
//(string windowKey, bool destoryWidnow = false)
UISystem.RemoveUIWindowData(windowKey, destoryWidnow);
//清除所有UI窗口数据
UISystem.ClearUIWindowData();

//简单实例
//获取testWindow的层级
UISystem.GetUIWindowData<testWindow>().layerNum;
```

**参数说明**

- 通过windowKey/泛型类型名/窗口对象类型传索引。
- 支持Try方式获取窗口数据，成功返回true并将数据赋给输出参数。
- 移除UI窗口数据,已存在的窗口对象实例会被强行删除。

### **UI窗口对象管理**

这里的UI窗口对象只UI窗口数据UIWIndowData持有的那一份窗口脚本对象实例，其生命周期由框架管理，整体分为打开和关闭。

#### **UI窗口打开**

```
//返回值为UI窗口类T，T受泛型约束必须为UI窗口基类子类
//(string windowKey, int layer = -1)
UISystem.Show<T>(windowKey, layer);
UISystem.Show<T>(windowKey);
UISystem.Show<T>(layer);
UISystem.Show<T>();
//返回值为UI_WindowBase类，对应不能确定窗口类型的情况, xx是窗口类的对象
//(Type type, int layer = -1)
UISystem.Show(xx.getType(), layer);
UISystem.Show(xx.getType());
//(string windowKey, int layer = -1)
UISystem.Show(windowKey, layer);
UISystem.Show(windowKey);

//简单实例，打开窗口UI_WindowTest并置于第三层
UISystem.Show<UI_WindowTest>(2);
```

**参数说明**

-
通过泛型T指定UI窗口子类类型，windowKey为UI窗口类的索引，对应UIWindowData中的windowKey，不指定则使用T的类型名作为索引，layer代表UI的层级，不填则默认-1表示使用数据中原有的层级（通过静态配置或者动态加载指定）。
- 在明确UI窗口类型的时候可以直接通过泛型T指定，不明确则可以通过传对象反射来获取类型。
- 简单解释逻辑为根据windowKey找到对应的窗口数据UIWindowData，根据数据中的assetPath加载UI窗口对象并根据T返回窗口类，无T则返回UI_WindowBase类。

由于UI窗口类继承了UIWIndowBase，其中提供了一些可供重写的方法，这些方法会在UI窗口打开时自动执行。

```
//初始化相关方法，只有在窗口第一次打开时执行
    public override void Init()
    {
        base.Init();
    }

    //窗口每次打开时执行，可用于数初始化，并会自动调用事件监听注册方法
    public override void OnShow()
    {
        base.OnShow();
    }
    //事件监听注册
    protected override void RegisterEventListener()
    {
        base.RegisterEventListener();
    }
```

#### **UI窗口关闭**

```
//(Type type) (string windowKey)
UISystem.Close<T>();
UISystem.Close(type);
UISystem.Close(windowKey);
UISystem.CloseAllWindow();

//简单实例，关闭窗口UI_WindowTest
UISystem.Close<UI_WindowTest>();
```

**参数说明**

- 相比打开，关闭不需要返回值也不需要管理层级，通过T/Type/windowKey传入窗口的索引即可。

由于UI窗口类继承了UIWIndowBase，其中提供了一些可供重写的方法，这些方法会在UI窗口关闭时自动执行。

```
//窗口每次关闭时执行，会动调用事件监听注销方法
    public override void OnClose()
    {
        base.OnClose();
    }

    //事件监听注销
    protected override void RegisterEventListener()
    {
        base.RegisterEventListener();
    }
```

#### 窗口内获取组件

原理：

```
protected virtual void Awake()
{
    FindChildrenControl<Button>();
    FindChildrenControl<Image>();
    FindChildrenControl<Text>();
    FindChildrenControl<Toggle>();
    FindChildrenControl<Slider>();
}
```

仅仅只是在Awake的时候都获取了一遍 序列化是更好的选择 但这样方便快速开发

支持五种常用的Controller获取

```
var testBtn = GetControl<Button>("testBtn");
var testText = GetControl<Text>("testText");
```

设定组件对象的时候 请保证对象名称和字段名称一致 使用小驼峰命名法

### **获取/销毁UI窗口对象**

获取/销毁UIWindowData持有的UI窗口对象实例，与Onshow/OnClose不同，其只获取实例，不包含窗口游戏物体对象的显示/隐藏/销毁逻辑。

```
//返回值为UI窗口类T，T受泛型约束必须为UI窗口基类子类
//(string windowKey)
UISystem.GetWindow<T>(windowKey);
UISystem.GetWindow<T>(Type windowType);
UISystem.GetWindow<T>();
//返回值为UI_WindowBase类，对应不能确定窗口类型的情况
UISystem.GetWindow(windowKey);
//返回值为bool,表示窗口对是否存在
//(string windowKey, out T window)
UISystem.TryGetWindow(windowKey, window);
//(string windowKey, out T window)
UISystem.TryGetWindow<T>(windowKey, window);
//销毁窗口对象
UISystem.DestroyWindow(windowKey);

//简单实例，获取TestWindow上的UI Text组件Name
Text name = UISystem.GetWindow<TestWindow>().Name;
```

**参数说明**

- 通过windowKey/type Name/T类型名查找窗口对象。
- 支持Try方式，查询成功则对象传递到输出参数out上，并返回bool为true，否则输出参数为null并返回false。
- 销毁窗口对象API会直接销毁游戏内的窗口gameObject、控制类，但UIWindowData还存在。

#### U

### **UI层级管理**

框架内部实现了对UI的层级管理，可以在面板的UISystem上每一层是否启用遮罩，默认每一层UI是层层堆叠覆盖的，一旦某一层中有UI窗口对象，则层级比它低的层级都不可以交互，同一层级中比它早打开的UI窗口不可以交互（保证每一层内最顶层只有一个窗口），可以勾选不启用遮罩，则这一层层内和层外都不存在遮罩关系。

启用遮罩如下图。

![img](https://alidocs.oss-cn-zhangjiakou.aliyuncs.com/res/8oLl97KK5xdgqapY/img/e0dbe12f-67ca-4ccd-afdc-c22b46103463.png)

Mask保证了每一层内最顶层只有一个窗口进行交互。

![img](https://alidocs.oss-cn-zhangjiakou.aliyuncs.com/res/8oLl97KK5xdgqapY/img/6ccfd1ce-fa50-4673-9e1a-39213e4f08be.png)

另外框架单独提供了最顶层dragLayer，用于拖拽时临时需要把某个UI窗口置于最上层，可以通过UISystem.dragLayer获取。

```
UISystem.dragLayer;
```

### **UI Tips**

弹窗工具。

```
// 在窗口右下角弹出字符串tips提醒。
//(string tips)
UISystem.AddTips(tips)
```

### **判断鼠标是否在UI上**

返回当前鼠标位置是否在UI上，（用于替换EventSystem.current.IsPointerOverGameObject()
，避免当前窗口因启用交互或同时需要考虑多层UI的层级关系，而启用覆盖全屏幕的遮罩Mask的RaycastTaret，使得鼠标处于UI窗口外时，Unity
API一直错误的返回在UI上）。

```
//bool
UISystem.CheckMouseOnUI();
```

## 事件工具

用于给游戏对象快速绑定事件，而无需手动给游戏对象挂载脚本，功能逻辑在当前脚本实现。与事件系统区分：事件系统重点在于提供了一个事件监听添加和事件触发解耦的中间模块，使得事件的触发无需关注依赖的对象，但事件执行的功能逻辑还是要实现在对象挂载的脚本上的。而事件工具重点在于快速为游戏对象绑定常见的响应事件，这类事件不由脚本触发（后续支持自定义脚本触发条件），而是在特定的时机如碰撞、鼠标点击、对象销毁时自动触发，因此重点关注事件监听添加的简化，所有逻辑在当前脚本完成。

### **框架内置事件绑定与移除**

#### **鼠标相关事件**

鼠标进入、鼠标移出、鼠标点击、鼠标按下、鼠标抬起、鼠标拖拽、鼠标拖拽开始、鼠标拖拽结束事件的绑定与移除。

```
//鼠标进入
//(this Component com, Action<PointerEventData, TEventArg> action, TEventArg args = default(TEventArg))
xx.OnMouseEnter<TEventArg>(action, args);
xx.OnMouseEnter(action, args); //指定参数类型的泛型可以不填，可以通过参数推断出
xx.OnMouseEnter(action); //无参Action
xx.RemoveOnMouseEnter<TEventArg>(action) //Remove时不传参，参数类型必须传，无法推断

//鼠标移出
//(this Component com, Action<PointerEventData, TEventArg> action, TEventArg args = default(TEventArg))
xx.OnMouseExit<TEventArg>(action, args);
xx.OnMouseExit(action, args); //指定参数类型的泛型可以不填，可以通过参数推断出
xx.OnMouseExit(action); //无参Action
xx.RemoveOnMouseExit<TEventArg>(action) //Remove时不传参，参数类型必须传，无法推断

//鼠标点击
//(this Component com, Action<PointerEventData, TEventArg> action, TEventArg args = default(TEventArg))
xx.OnClick<TEventArg>(action, args);
xx.OnClick(action, args); //指定参数类型的泛型可以不填，可以通过参数推断出
xx.OnClick(action); //无参Action
xx.RemoveOnClick<TEventArg>(action) //Remove时不传参，参数类型必须传，无法推断

//鼠标按下
//(this Component com, Action<PointerEventData, TEventArg> action, TEventArg args = default(TEventArg))
xx.OnClickDown<TEventArg>(action, args);
xx.OnClickDown(action, args); //指定参数类型的泛型可以不填，可以通过参数推断出
xx.OnClickDown(action); //无参Action
xx.RemoveOnClickDown<TEventArg>(action) //Remove时不传参，参数类型必须传，无法推断

//鼠标抬起
//(this Component com, Action<PointerEventData, TEventArg> action, TEventArg args = default(TEventArg))
xx.OnClickUp<TEventArg>(action, args);
xx.OnClickUp(action, args); //指定参数类型的泛型可以不填，可以通过参数推断出
xx.OnClickUp(action); //无参Action
xx.RemoveOnClickUp<TEventArg>(action) //Remove时不传参，参数类型必须传，无法推断

//鼠标拖拽
//(this Component com, Action<PointerEventData, TEventArg> action, TEventArg args = default(TEventArg))
xx.OnDrag<TEventArg>(action, args);
xx.OnDrag(action, args); //指定参数类型的泛型可以不填，可以通过参数推断出
xx.OnDrag(action); //无参Action
xx.RemoveOnDrag<TEventArg>(action) //Remove时不传参，参数类型必须传，无法推断

//鼠标拖拽开始
//(this Component com, Action<PointerEventData, TEventArg> action, TEventArg args = default(TEventArg))
xx.OnBeginDrag<TEventArg>(action, args);
xx.OnBeginDrag(action, args); //指定参数类型的泛型可以不填，可以通过参数推断出
xx.OnBeginDrag(action); //无参Action
xx.RemoveOnBeginDrag<TEventArg>(action) //Remove时不传参，参数类型必须传，无法推断

//鼠标拖拽结束
//(this Component com, Action<PointerEventData, TEventArg> action, TEventArg args = default(TEventArg))
xx.OnEndDrag<TEventArg>(action, args);
xx.OnEndDrag(action, args); //指定参数类型的泛型可以不填，可以通过参数推断出
xx.OnEndDrag(action); //无参Action
xx.RemoveOnEndDrag<TEventArg>(action) //Remove时不传参，参数类型必须传，无法推断

//使用示例
Transform cube;
void Start()
{
    cube.OnClick<int>(Test1,1);
}
private void Test1(PointerEventData arg1, int arg2)
{
    Debug.Log(1);
    cube.RemoveOnClick<int>(Test1);
}
```

**参数列表**

- xx为绑定事件的对象组件，事件工具基于拓展方法调用，xx使用游戏对象的transform即可。
- TEventArg指定事件的参数类型，添加监听时可以不填，可以通过参数args推断出，移除监听时则必须显示指出。
-
action是绑定的事件，根据事件类型（鼠标、碰撞、自定义事件），其方法的参数列表包含两部分，第一部分是事件本身的参数（PointerEventData、Collision），第二部分是参数列表TEventArg，可以通过值元组传入多个参数。

#### **碰撞相关事件**

2D、3D相关的碰撞事件绑定与移除。

```
//3D碰撞进入
//(this Component com, Action<Collision, TEventArg> action, TEventArg args = default(TEventArg))
xx.OnCollisionEnter<TEventArg>(action, args);
xx.OnCollisionEnter(action, args); //指定参数类型的泛型可以不填，可以通过参数推断出
xx.OnCollisionEnter(action); //无参Action
xx.RemoveOnCollisionEnter<TEventArg>(action) //Remove时不传参，参数类型必须传，无法推断

//3D碰撞持续
//(this Component com, Action<Collision, TEventArg> action, TEventArg args = default(TEventArg))
xx.OnCollisionStay<TEventArg>(action, args);
xx.OnCollisionStay(action, args); //指定参数类型的泛型可以不填，可以通过参数推断出
xx.OnCollisionStay(action); //无参Action
xx.RemoveOnCollisionStay<TEventArg>(action) //Remove时不传参，参数类型必须传，无法推断

//3D碰撞脱离
//(this Component com, Action<Collision, TEventArg> action, TEventArg args = default(TEventArg))
xx.OnCollisionExit<TEventArg>(action, args);
xx.OnCollisionExit(action, args); //指定参数类型的泛型可以不填，可以通过参数推断出
xx.OnCollisionExit(action); //无参Action
xx.RemoveOnCollisionExit<TEventArg>(action) //Remove时不传参，参数类型必须传，无法推断

//2D碰撞进入
//(this Component com, Action<Collision2D, TEventArg> action, TEventArg args = default(TEventArg))
xx.OnCollisionEnter2D<TEventArg>(action, args);
xx.OnCollisionEnter2D(action, args); //指定参数类型的泛型可以不填，可以通过参数推断出
xx.OnCollisionEnter2D(action); //无参Action
xx.RemoveOnCollisionEnter2D<TEventArg>(action) //Remove时不传参，参数类型必须传，无法推断

//2D碰撞持续
//(this Component com, Action<Collision2D, TEventArg> action, TEventArg args = default(TEventArg))
xx.OnCollisionStay2D<TEventArg>(action, args);
xx.OnCollisionStay2D(action, args); //指定参数类型的泛型可以不填，可以通过参数推断出
xx.OnCollisionStay2D(action); //无参Action
xx.RemoveOnCollisionStay2D<TEventArg>(action) //Remove时不传参，参数类型必须传，无法推断

//2D碰撞脱离
//(this Component com, Action<Collision2D, TEventArg> action, TEventArg args = default(TEventArg))
xx.OnCollisionExit2D<TEventArg>(action, args);
xx.OnCollisionExit2D(action, args); //指定参数类型的泛型可以不填，可以通过参数推断出
xx.OnCollisionExit2D(action); //无参Action
xx.RemoveOnCollisionExit2D<TEventArg>(action) //Remove时不传参，参数类型必须传，无法推断

//简单示例
void Start()
{
    cube.OnCollisionEnter(Test2, 2);
}

private void Test2(Collision arg1, int arg2)
{
    Debug.Log(arg2);
    cube.RemoveOnCollisionEnter<int>(Test2);
}
```

**参数列表**

- 碰撞事件和鼠标事件的API类似，区别参与action的第一个事件本身参数不同，为Collision/Collision2D。
- xx为绑定事件的对象组件，使用游戏对象的transform即可。
- TEventArg指定事件的参数类型，添加监听时可以不填，可以通过参数args推断出，移除监听时则必须显示指出。
-
action是绑定的事件，根据事件类型（鼠标、碰撞、自定义事件），其方法的参数列表包含两部分，第一部分是事件本身的参数（PointerEventData、Collision），第二部分是参数列表TEventArg，可以通过值元组传入多个参数。

#### **触发相关事件**

2D、3D相关的触发事件绑定。

```
//3D触发进入
//(this Component com, Action<Collider, TEventArg> action, TEventArg args = default(TEventArg))
xx.OnTriggerEnter<TEventArg>(action, args);
xx.OnTriggerEnter(action, args); //指定参数类型的泛型可以不填，可以通过参数推断出
xx.OnTriggerEnter(action); //无参Action
xx.RemoveOnTriggerEnter<TEventArg>(action) //Remove时不传参，参数类型必须传，无法推断

//3D触发持续
//(this Component com, Action<Collider, TEventArg> action, TEventArg args = default(TEventArg))
xx.OnTriggerStay<TEventArg>(action, args);
xx.OnTriggerStay(action, args); //指定参数类型的泛型可以不填，可以通过参数推断出
xx.OnTriggerStay(action); //无参Action
xx.RemoveOnTriggerStay<TEventArg>(action) //Remove时不传参，参数类型必须传，无法推断

//3D触发脱离
//(this Component com, Action<Collider, TEventArg> action, TEventArg args = default(TEventArg))
xx.OnTriggerExit<TEventArg>(action, args);
xx.OnTriggerExit(action, args); //指定参数类型的泛型可以不填，可以通过参数推断出
xx.OnTriggerExit(action); //无参Action
xx.RemoveOnTriggerExit<TEventArg>(action) //Remove时不传参，参数类型必须传，无法推断

//2D触发进入
//(this Component com, Action<Collider2D, TEventArg> action, TEventArg args = default(TEventArg))
xx.OnTriggerEnter2D<TEventArg>(action, args);
xx.OnTriggerEnter2D(action, args); //指定参数类型的泛型可以不填，可以通过参数推断出
xx.OnTriggerEnter2D(action); //无参Action
xx.RemoveOnTriggerEnter2D<TEventArg>(action) //Remove时不传参，参数类型必须传，无法推断

//2D碰撞持续
//(this Component com, Action<Collider2D, TEventArg> action, TEventArg args = default(TEventArg))
xx.OnTriggerStay2D<TEventArg>(action, args);
xx.OnTriggerStay2D(action, args); //指定参数类型的泛型可以不填，可以通过参数推断出
xx.OnTriggerStay2D(action); //无参Action
xx.RemoveOnTriggerStay2D<TEventArg>(action) //Remove时不传参，参数类型必须传，无法推断

//2D触发脱离
//(this Component com, Action<Collider2D, TEventArg> action, TEventArg args = default(TEventArg))
xx.OnTriggerExit2D<TEventArg>(action, args);
xx.OnTriggerExit2D(action, args); //指定参数类型的泛型可以不填，可以通过参数推断出
xx.OnTriggerExit2D(action); //无参Action
xx.RemoveOnTriggerExit2D<TEventArg>(action) //Remove时不传参，参数类型必须传，无法推断

//简单示例

void Start()
{
    cube.OnTriggerEnter(Test3, 2);
}

private void Test3(Collider arg1, int arg3)
{
    Debug.Log(arg3);
    cube.RemoveOnTriggerEnter<int>(Test3);
}
```

**参数列表**

- 触发事件和碰撞事件的API类似，区别参与action的第一个事件本身参数不同，为Collider/Collider2D。
- xx为绑定事件的对象组件，使用游戏对象的transform即可。
- TEventArg指定事件的参数类型，添加监听时可以不填，可以通过参数args推断出，移除监听时则必须显示指出。
-
action是绑定的事件，根据事件类型（鼠标、碰撞、自定义事件），其方法的参数列表包含两部分，第一部分是事件本身的参数（PointerEventData、Collision），第二部分是参数列表TEventArg，可以通过值元组传入多个参数。

#### **资源相关事件**

资源释放，对象销毁时绑定的事件。

```
//资源释放（Addressable）
//(this Component com, Action<GameObject, TEventArg> action, TEventArg args = default(TEventArg))
xx.OnReleaseAddressableAsset<TEventArg>(action, args);
xx.OnReleaseAddressableAsset(action, args); //指定参数类型的泛型可以不填，可以通过参数推断出
xx.OnReleaseAddressableAssetOnReleaseAddressableAsset(action); //无参Action
xx.RemoveOnReleaseAddressableAsset<TEventArg>(action) //Remove时不传参，参数类型必须传，无法推断

//对象销毁
//(this Component com, Action<GameObject, TEventArg> action, TEventArg args = default(TEventArg))
xx.OnDestroy<TEventArg>(action, args);
xx.OnDestroy(action, args); //指定参数类型的泛型可以不填，可以通过参数推断出
xx.OnDestroy(action); //无参Action
xx.RemoveOnDestroy<TEventArg>(action) //Remove时不传参，参数类型必须传，无法推断

//简单实例
void Start()
{
    cube.OnDestroy(Test4, 4);
}
private void Test4(GameObject arg1, int arg4)
{
    Debug.Log(arg4);
    cube.RemoveOnDestroy<int>(Test4);
}
```

**参数列表**

- xx为绑定事件的对象组件，使用游戏对象的transform即可。
- TEventArg指定事件的参数类型，添加监听时可以不填，可以通过参数args推断出，移除监听时则必须显示指出。
-
action是绑定的事件，根据事件类型（鼠标、碰撞、自定义事件），其方法的参数列表包含两部分，第一部分是事件本身的参数（PointerEventData、Collision），第二部分是参数列表TEventArg，可以通过值元组传入多个参数。

#### **移除一类事件**

移除鼠标/碰撞/触发/资源的一类所有事件。

```
//int customEventTypeInt, JKEventType eventType
RemoveAllListener(customEventTypeInt);
RemoveAllListener(eventType);
RemoveAllListener();
```

**参数说明**

- customEventTypeInt/eventType为事件的类型，对应碰撞、鼠标事件对应的枚举类型或自定义事件的类型int值。
- 不填则移除所有事件。

### **使用值元组传递多个事件参数**

通过ValueTuple封装一个简单的参数列表结构体。

```
void Start()
{
    cube.OnClick(Test5, (arg1: 2, arg2: "test", arg3: true));
    //等同于下一行代码，上一行更简便，参数类型可以自动推断出
    cube.OnClick(Test5, ValueTuple.Create<int,string,bool>(1,"test",true));
}
private void Test5(PointerEventData arg1, (int arg1, string arg2, bool arg3) args)
{
    Debug.Log($"{args.arg1},{args.arg2},{args.arg3}");
    cube.RemoveOnClick<(int arg1, string arg2, bool arg3)>(Test5);
}
```

### **自定义事件类型**

以上鼠标、碰撞等事件的触发由事件工具结合特定的时机自动完成，如果希望自定义事件的触发逻辑，则需要添加新的事件类型,对应在适合的地方触发事件，此时事件工具的作用与事件系统类似，区别在于不需要为对象挂载脚本。

```
//(this Component com, int customEventTypeInt, Action<T, TEventArg> action, TEventArg args = default(TEventArg))
xx.AddEventListener<T, TEventArg>(customEventTypeInt, action, args);
xx.RemoveEventListener<T, TEventArg>(customEventTypeInt, action);
cube.TriggerCustomEvent<Transform>((int)myType.CustomType, transform);

//使用示例
    void Start()
    {
        cube.AddEventListener<Transform, int>((int)myType.CustomType, Test6, 1);
    }

    private void Test6(Transform arg1, int arg2)
    {
        Debug.Log(arg1.position = Vector3.zero);
        cube.RemoveEventListener<Transform, int>((int)myType.CustomType, Test6);
    }
    enum myType
    {
        CustomType = 0,
    }
```

**参数说明**

- customEventTypeInt是自定义的事件类型，是一个int值，可以使用枚举对应事件的类型。
- T指明了自定义事件所使用的eventData，可以在触发的时候传入T以供使用，等同于Collision/Collider/PointerEventData。
- args是参数列表。

### **补充说明**

-
事件工具针对的事件触发时都会提供eventData用于获取触发时的特定数据用于操作（有点类似于异步callback回调时传的那个参数），比如PointerEventData，因此他们与对象绑定，就算不传任何参数，触发时还是可以根据eventData去获取一些信息，比如碰撞发生的位置。
-
开发事件工具的目的在于快速为游戏对象添加一类事件的监听，而不需要为其手动挂载脚本（类似于button.OnClick.AddEventListener,但Unity只支持按钮的自动添加，而事件工具支持常见的所有事件类型），实际上会自动为其自动挂载JKEventListener脚本，其中有对应事件的监听方法以及内置碰撞/鼠标等事件的触发方法。
- 自定义事件类型是支持的，但此时事件的类型，触发需要自己实现。
-
事件系统作用在于解耦对象和事件触发的逻辑，让事件中心保存监听的方法，触发时不需要访问对象。而事件工具所负责的是一类与对象强关联的事件，用于解耦对象和事件监听添加的逻辑，不需要手动挂载脚本。二者联动的效果就是A使用事件工具直接为B添加事件监听C，C内部再通过事件系统包一层添加事件监听D，这样外界就可以通过直接访问事件中心触发D（可能的应用场景比如要给所有子弹添加碰撞分解效果，这样无论是事件监听添加还是事件的触发都可以在一个脚本中完成，不需要手动给所有子弹度挂载脚本，也不需要触发时访问所有子弹对象）。

###  





