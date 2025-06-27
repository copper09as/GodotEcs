using Godot;
using System;

public partial class TransformSystem : EcsSystem
{
    public override void Update(World world, float deltaTime)
    {
        var view = world.GetStore<TransformComponent>().GetView();
        var gameObjectStore = world.GetStore<GameObjectComponent>();
        while (view.MoveNext())
        {
            view.IsMoveNext();
            var entity = view.Entity;
            ref var transform = ref view.Current;
            if (world.HasComponent<GameObjectComponent>(entity))
            {
                ref var gameObject = ref world.GetComponent<GameObjectComponent>(entity);
                if (gameObject.gameObject != null)
                {
                    gameObject.gameObject.Position = transform.position;
                }
            }
        }
        
    }
}
