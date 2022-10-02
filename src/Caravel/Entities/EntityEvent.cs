namespace Caravel.Entities;

public class EntityEvent : Entity
{
    public string Name { get; set; }
    public string Data { get; set; }

    public EntityEvent(string name, string data)
    {
        Data = data;
        Name = name;
    }
}