using Godot;
using System;
using System.Collections.Generic;

public partial class World
{
    private int _nextEntityId = 1;
    private int _nextVersion = 1;
    private readonly Dictionary<Type, IComponentStore> _stores = new Dictionary<Type, IComponentStore>();//组件表
    private readonly List<EcsSystem> _system = new List<EcsSystem>();//系统列表
    public readonly List<Entity> _entities = new List<Entity>();//实体列表

    public World()
    {
        _system.Add(new TransformSystem());
        _system.Add(new MovementSystem());
    }
    public Entity CreateEntity()
    {
        var entity = new Entity(_nextEntityId++, _nextVersion++);//建立实体，并且设置实体id
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
    public ref T GetComponent<T>(Entity entity) where T : struct
    {
        return ref GetStore<T>().Get(entity);
    }
    public ComponentStore<T> GetStore<T>() where T : struct
    {
        var type = typeof(T);
        if (!_stores.TryGetValue(type, out var store))
        {
            store = new ComponentStore<T>();
            _stores[type] = store;
        }
        return (ComponentStore<T>)store;
    }
    public void DestoryEntity(Entity entity)
    {
        if (HasComponent<GameObjectComponent>(entity))
        {
            ref var gameObject = ref GetComponent<GameObjectComponent>(entity);
            if (gameObject.gameObject != null)
            {
                gameObject.gameObject.QueueFree();
            }
        }
        for (int i = 0; i < _entities.Count; i++)
        {
            if (_entities[i].Equals(entity))
            {
                _entities[i] = new Entity
                {
                    id = entity.id,
                    version = entity.version,
                    active = false
                };
                break;
            }
        }
    }
    public void Update(float deltaTime)
    {
        foreach (var system in _system)
        {
            system.Update(this, deltaTime);
        }
        CleanupInactiveEntities();
    }
    private void CleanupInactiveEntities()
    {
        for (int i = _entities.Count - 1; i >= 0; i--)
        {
            if (!_entities[i].active)
            {
                foreach (var store in _stores.Values)
                {
                    store.Remove(_entities[i]);
                }
                _entities.RemoveAt(i);
            }
        }
    }
}

