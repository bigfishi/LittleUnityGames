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