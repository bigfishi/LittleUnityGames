# LittleUnityGames
little games by Unity.

## 环境
- Unity版本 2022.3.42f1c1

## Snake
- 简单的2D贪吃蛇游戏
- [参考链接](https://www.bilibili.com/video/BV1cv4y1r7Qp/?spm_id_from=333.788.videopod.sections&vd_source=f33a259cffbdc537ff6ba43e110937bf)
- 功能说明
	- 按键判断 Input.GetKeyDown(KeyCode.W)
	- 固定间隔刷新函数 OnFixedUpdate，可以在项目配置中修改间隔时间，默认是0.02s（Edit->Project Settings->Time->Fixed timestep）
	- 预制体制作和代码中初始化 Instantiate()，类型是Transform，销毁是Destroy(transform.gameObject)
	- 触发式碰撞检测 OnTriggerEnter2D
- 蹚坑
	- 修改了Snake.cs，运行游戏不生效，原来是没附加到游戏对象中
	- 添加物理碰撞触发函数，不生效，发现是函数写错了，争取的函数是OnTriggerEnter2D，我写的OTriggerEnter2D

## PingPang
- 简单的2D乒乓球游戏
- [参考链接](https://www.bilibili.com/video/BV1kM4y197Uy?spm_id_from=333.788.videopod.sections&vd_source=f33a259cffbdc537ff6ba43e110937bf)
- 功能说明
	- 按键判断
	- 刚体、施加外力、物理纹理、弹性、质量、阻力等
	- 纹理和材质
- 蹚坑
	- 无

## Minesweeper
- 扫雷游戏
- [参考链接](https://www.bilibili.com/video/BV1gk4y187mZ?spm_id_from=333.788.videopod.sections&vd_source=f33a259cffbdc537ff6ba43e110937bf)
- 功能说明
	- 鼠标按键判断，按键位置转换为世界坐标，世界坐标转换为TileMap的cell坐标
	- TileMap sprite图片的设置，创建TileMap的Tile
	- struct定义，二维数组使用，列表容器List使用
	- 只读属性
	- Unity脚本属性的校验 OnValidate函数和 Mathf.clamp
	- 从子节点获取组件
	- 相机位置的移动
	- 洗牌算法
	- 扫雷逻辑

## 2048
- 2048
- [参考链接](https://www.bilibili.com/video/BV1y8411f7xt?spm_id_from=333.788.videopod.sections&vd_source=f33a259cffbdc537ff6ba43e110937bf)
- 功能说明
	- sprite 修改修改九宫配置，Sprite->Sprite Editor 修改border
	- Package Manager 使用
	- TextMesh Pro添加 Window->TextMeshPro->Import TMP Essential Resources，确定导入，会自动在Assets中添加TextMesh Pro资源
	- 生成Font Asset 全选所有ttf文件，右键->Create->TextMeshPro->Font Asset（Shift+Ctrl+F12）
	- Canvas介绍以及适配说明，创建Canvas会自动创建EventSystem，用于处理UI相关事件
	- 布局管理 Horizontal Layout Group
	- 预制体的创建和使用
	- 创建ScriptableObject类型的脚本对象，不挂载在GameObject上，可以在Unity中创建这种类型的脚本
	- 修改成Android项目，使用手势识别控制
- 蹚坑
	- 显示的2，不是很清晰

## Asteroids
- 行星游戏
- [功能参考](https://www.bilibili.com/video/BV1pM411i7Yp?spm_id_from=333.788.videopod.sections&vd_source=f33a259cffbdc537ff6ba43e110937bf)
- 功能说明
	- thrusting 向前进 2d游戏是 trasform.up 3D游戏是 transform.forward
	- 转向 torque 恒定力矩，可使物体进行匀速旋转
	- 陨石的欧拉角
	- 子弹 延时删除 Destroy(gameObject, 10.0f);
	- InvokeRepeating(nameof(Spawn), this.spawnRate, this.spawnRate); // 循环调用Spawn函数
	- trajectory 
	- 直接调用GameManager（不建议使用） FindObjectOfType<GameManager>().PlayerDied();
		- 直接使用单例处理
	- 修改刚体的Layer this.player.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
	- 例子特效
- 蹚坑

## Bomberman
- 炸弹人
- [功能参考](https://www.bilibili.com/video/BV1FW4y1R7Vk?spm_id_from=333.788.videopod.sections&vd_source=f33a259cffbdc537ff6ba43e110937bf)
- 功能说明
	- Sprite的Multi模式，可以作为精灵集拆分多个精灵
	- Tilemap 绘制模式
	- Tilemap Collider 2D
	- Sorting Layer -> Order in Layer 层级关系
	- MovementController.cs 中定义的 刚体 rigidbody 与 父类 MonoBehaviour 中的属性同名了，可以将定义改为 public new Regidbody2D { get; private set; } 防止警告，即添加一个new关键词
	- MovementController.cs 中定义按键 public KeyCode inputUp = KeyCode.W; 可以在编辑器中修改，嗯，很方便
	- AnimatedSpriteRenderer 帧动画，通过 InvokeRepeating(nameof(NextFrame), animationTime, animationTime); 实现
	- PlaceBomb 协程

## Centipede
- 大蜈蚣
- [功能参考](https://www.bilibili.com/video/BV1v8411f7dq?spm_id_from=333.788.player.switch&vd_source=f33a259cffbdc537ff6ba43e110937bf)
- 功能说明
	- 控制方向，使用Unity内置的方向`Input.getAxis("Horizontal");` Project Settings->Input Manager->Axes ->Horizontal  包括WSAD 方向键 鼠标等
- 蹚坑