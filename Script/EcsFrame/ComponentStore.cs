using Godot;
using System;

public partial class ComponentStore<T> : IComponentStore where T : struct
{
    private const int Initialsize = 128;//数组容量
    private T[] _dense = new T[Initialsize];//组件数组
    private Entity[] _entities = new Entity[Initialsize];//实体数组
    private int[] _sparse = new int[Initialsize];//实体稀疏矩阵，次序代表实体id，值代表在组件数组与实体中的位置
    private int _count;//当前组件与实体数量
    public ComponentStore()
    {
        Array.Fill(_sparse, -1);//初始化稀疏矩阵（在实体数组中无对应索引）
    }
    public void Add(Entity entity, T component)//添加组件
    {
        EnsureCapacity(entity.id);
        if (_sparse[entity.id] != -1)//对应位置的实体已储存在实体数组中
        {
            _dense[_sparse[entity.id]] = component;
            return;
        }
        if (_count >= _dense.Length)//当组件与实体数量超出数组大小时进行扩充
        {
            ResizeArrays(_dense.Length * 2);
        }
        //对应位置的实体未储存在实体数组中
        _dense[_count] = component;
        _entities[_count] = entity;
        _sparse[entity.id] = _count;
        _count++;
    }
    public ref T Get(Entity entity)
    {
        if (_sparse[entity.id] == -1)
        {
            GD.PrintErr(entity.ToString() + "没有组件" + typeof(T).Name);
        }
        return ref _dense[_sparse[entity.id]];
    }
    public bool Has(Entity entity)//当对应实体的id小于系数矩阵的大小并且系数矩阵有对实体数组的索引时，存在对应实体
    {
        return entity.id < _sparse.Length && _sparse[entity.id] != -1;
    }
    public void Remove(Entity entity)
    {
        if (!Has(entity))
            return;
        int denseIndex = _sparse[entity.id];//获取实体索引
        if (denseIndex < _count - 1)//当待移除实体不在数组最后一个时，把数组末尾实体与待移除实体交换
        {
            _dense[denseIndex] = _dense[_count - 1];
            _entities[denseIndex] = _entities[_count - 1];
            _sparse[_entities[denseIndex].id] = denseIndex;//把稀疏矩阵中被移动的实体，指向移动位置
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
        //实体id等于系数矩阵长度时，进行扩充
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
        for (int i = _count+1; i < newSize; i++)//把新扩充的位置置-1
        {
            _sparse[i] = -1;
        }
        Array.Resize(ref _dense, newSize);
        Array.Resize(ref _entities, newSize);
    }

}
