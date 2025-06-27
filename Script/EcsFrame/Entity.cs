using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

public struct Entity
{
    public int id;
    public int version;
    public bool active = true;
    public Entity(int id, int version)
    {
        this.id = id;
        this.version = version;
    }
    public bool Equals(Entity other)
    {
        return id == other.id && version == other.version;
    }
    public override bool Equals([NotNullWhen(true)] object obj)
    {
        return obj is Entity other && Equals(other);
    }
    public override string ToString()
    {
        return "Entity" + id.ToString() + "." + version.ToString();
    }
}
