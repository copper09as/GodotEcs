using Godot;
using System;

public partial class ComponentStore<T> : IComponentStore where T : struct
{
    private const int Initialsize = 128;
    private T[] _dense = new T[Initialsize];//组件数组
    private Entity[] _entities = new Entity[Initialsize];//实体数组
    private int[] _sparse = new int[Initialsize];//实体稀疏矩阵，次序代表实体id，值代表在数组中的位置
    private int _count;
    public ComponentStore()
    {
        Array.Fill(_sparse, -1);
    }
    public void Add(Entity entity, T component)
    {
        EnsureCapacity(entity.id);
        if (_sparse[entity.id] != -1)//对应位置的实体已储存在数组中
        {
            _dense[_sparse[entity.id]] = component;
            return;
        }
        if (_count >= _dense.Length)
        {
            ResizeArrays(_dense.Length * 2);
        }
        _dense[_count] = component;
        _entities[_count] = entity;
        _sparse[entity.id] = _count;
        _count++;
    }
    public ref T Get(Entity entity)
    {
        if (_sparse[entity.id] != -1)
        {
            GD.PrintErr(entity.ToString() + "没有组件" + typeof(T).Name);
        }
        return ref _dense[_sparse[entity.id]];
    }
    public bool Has(Entity entity)
    {
        return entity.id < _sparse.Length && _sparse[entity.id] != -1;
    }
    public void Remove(Entity entity)
    {
        if (!Has(entity))
            return;
        int denseIndex = _sparse[entity.id];
        if (denseIndex < _count - 1)
        {
            _dense[denseIndex] = _dense[_count - 1];
            _entities[denseIndex] = _entities[_count - 1];
            _sparse[denseIndex] = denseIndex;
        }
        _sparse[entity.id] = -1;
        _dense[_count - 1] = default;
        _entities[_count - 1] = default;
        _count--;
    }
    public Componentview<T> GetView()
    {
        return new Componentview<T>(_dense, _entities, _count);
    }
    private void EnsureCapacity(int entityId)
    {
        if (entityId < _sparse.Length)
        {
            return;
        }
        int newSize = Math.Max(entityId + 1, _sparse.Length * 2);
        ResizeArrays(newSize);
    }

    private void ResizeArrays(int newSize)
    {
        Array.Resize(ref _sparse, newSize);
        for (int i = _count; i < newSize; i++)
        {
            _sparse[i] = -1;
        }
        Array.Resize(ref _dense, newSize);
        Array.Resize(ref _entities, newSize);
    }

}
