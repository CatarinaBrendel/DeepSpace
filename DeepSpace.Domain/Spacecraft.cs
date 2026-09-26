namespace DeepSpace.Domain;

public sealed class Spacecraft
{
    private readonly Dictionary<string, object> _components = [];

    public IReadOnlyDictionary<string, object> Components => _components;

    public void AddComponent<T>(string id, T component) where T : class
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentNullException.ThrowIfNull(component);

        if (!_components.TryAdd(id, component))
            throw new InvalidOperationException($"A component with ID '{id}' already exists.");
    }

    public T? GetComponent<T>(string id) where T : class
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        return _components.TryGetValue(id, out var component)
            ? component as T
            : null;
    }

    public IEnumerable<T> GetComponents<T>() where T : class
    {
        return _components.Values.OfType<T>();
    }
}