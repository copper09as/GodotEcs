using Godot;

public abstract class EcsSystem
{
    public abstract void Update(World world,float deltaTime);
}
