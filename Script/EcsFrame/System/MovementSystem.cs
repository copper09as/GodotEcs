using Godot;
using System;

public partial class MovementSystem : EcsSystem
{
    public override void Update(World world, float deltaTime)
    {
        var view = world.GetStore<VelocityComponent>().GetView();
        var transformStore = world.GetStore<TransformComponent>();
        while (view.MoveNext())
        {
            var entity = view.Entity;
            ref var velocity = ref view.Current;
            if (world.HasComponent<TransformComponent>(entity))
            {
                ref var transform = ref world.GetComponent<TransformComponent>(entity);
                var vx = GD.RandRange(-50, 50);
                var vy = GD.RandRange(-50, 50);
                velocity.value = new Vector2(vx, vy);
                transform.position += velocity.value * deltaTime;

                world.AddComponent<TransformComponent>(entity, transform);
            }
        }
    }

}
