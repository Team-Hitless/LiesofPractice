namespace LiesOfPractice.Interfaces;

public interface IJsonService
{
    public Task<T> DeserializeAsync<T>(string inputPath) where T : new();
    public void SerializeAsync<T>(T obj, string outputPath);
}