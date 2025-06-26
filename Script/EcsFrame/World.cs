using Godot;
using System;
using System.Collections.Generic;

public partial class World
{
    private int _nextEntityId = 1;
    private int _nextVersion = 1;
    private readonly Dictionary<Type, IComponentStore> _stores = new Dictionary<Type, IComponentStore>();
    private readonly List<EcsSystem> _system = new List<EcsSystem>();
    private readonly List<Entity> _entities = new List<Entity>();

    public World()
    {
        //初始化系统
    }
    public Entity CreateEntity()
    {
        var entity = new Entity(_nextEntityId++, _nextVersion++);
        _entities.Add(entity);
        return entity;
    }
    public void RemoveComponent<T>(Entity entity) where T : struct
    {
        GetStore<T>().Remove(entity);
    }
    public bool HasComponent<T>(Entity entity) where T : struct
    {
        return GetStore<T>().Has(entity);
    }
    public void AddComponent<T>(Entity entity, T component) where T : struct
    {
        GetStore<T>().Add(entity, component);
    }
    public ref T GetComponent<T>(Entity entity)where T:struct
    {
        return ref GetStore<T>().Get(entity);
    }
    private ComponentStore<T> GetStore<T>() where T : struct
    {
        var type = typeof(T);
        if (!_stores.TryGetValue(type, out var store))
        {
            store = new ComponentStore<T>();
            _stores[type] = store;
        }
        return (ComponentStore<T>)store;
    }
}

