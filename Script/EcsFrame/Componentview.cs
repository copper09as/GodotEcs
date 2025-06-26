using Godot;
using System;

public struct Componentview<T>
{
    private T[] dense;//组件数组
    private Entity[] entities;//实体数组
    private int[] sparse;//实体稀疏矩阵，次序代表实体id，值代表在数组中的位置
    private readonly int count;
    private int index;

    public Componentview(T[] dense, Entity[] entities, int count) : this()
    {
        this.dense = dense;
        this.entities = entities;
        this.count = count;
        index = -1;
    }
    public bool MoveNext()
    {
        return ++index < count;
    }
    public ref T Current => ref dense[index];
    public Entity Entity => entities[index];
    

}
