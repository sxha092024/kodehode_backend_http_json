using System.Text.Json;

using DummyJson;

namespace task_3;

class Program
{
    static async Task Main(string[] args)
    {
        DummyJsonRecipeApi api = new DummyJsonRecipeApi();

        DummyJsonRecipe one = await api.GetOne();

        var many = await api.GetMany();

        FileStream fs_one = File.Open("./one.json", FileMode.OpenOrCreate);
        // Replicating truncation without needing to handle the failure modes for FileMode.Truncate
        fs_one.Position = 0;
        fs_one.SetLength(0);
        
        string serialized = JsonSerializer.Serialize(one); // Method one, intermediary string serialization
        var sw_one = new StreamWriter(fs_one);
        sw_one.AutoFlush = true;
        sw_one.WriteLine(serialized);

        FileStream fs_many = File.Open("./many.json", FileMode.OpenOrCreate);
        // Replicating truncation without needing to handle the failure modes for FileMode.Truncate
        fs_many.Position = 0;
        fs_many.SetLength(0);
        JsonSerializer.Serialize(fs_many, many); // Method two, directly writing serialized result to FileStream
    }
}
