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