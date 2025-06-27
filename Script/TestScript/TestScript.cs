using Godot;
using System;
using System.Xml;

public partial class TestScript : Node2D
{
    private int _frameCount = 0;
    private const int TestInterval = 60; // 每60帧测试一次性能
    private World world;
    public override void _Ready()
    {
        base._Ready();
        //InitEcs(40000);
        Init(40000);
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        //world.Update((float)delta);
        PrintFps();
    }
    private void PrintFps()
    {
        _frameCount++;
 
        // 每隔TestInterval帧打印一次性能数据
        if (_frameCount % TestInterval == 0)
        {
            // 1. 获取当前帧率（FPS）
            int fps = (int)Engine.GetFramesPerSecond();
 
            // 2. 获取当前内存占用（MB）
            float memoryUsedMB = OS.GetStaticMemoryUsage() / 1024f / 1024f; // 转换为MB
 
            // 打印结果
            GD.Print($"FPS: {fps}, Memory Used: {memoryUsedMB:F2} MB");
        }
    }
    private void Init(int number)
    {
        for (int i = 0; i < number; i++)
        {
            var playerObj = ResManager.Instance.CreateInstance<Sprite2D>("res://TestTscn/player2.tscn", this);
        }
    }
    private void InitEcs(int number)
    {
        world = new World();

        for (int i = 0; i < number; i++)
        {
            var player = world.CreateEntity();
            var playerObj = ResManager.Instance.CreateInstance<Sprite2D>("res://TestTscn/player1.tscn", this);
            world.AddComponent<GameObjectComponent>(player, new GameObjectComponent { gameObject = playerObj });
            world.AddComponent<TransformComponent>(player, new TransformComponent() { position = new Vector2(500, 500) });
            world.AddComponent<VelocityComponent>(player, new VelocityComponent() { value = Vector2.Zero });
        }
    }

}
