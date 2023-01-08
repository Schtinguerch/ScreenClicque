using System.IO;
using Newtonsoft.Json;

namespace ScreenClicque;

public abstract class LoadableModel : ObservableModel
{
    public abstract string FilePath { get; }
    private JsonSerializerSettings _settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Auto};

    public void Load<T>() where T : LoadableModel
    {
        if (!File.Exists(FilePath))
        {
            Save<T>();
            return;
        }

        var code = File.ReadAllText(FilePath);
        try
        {
            var loadedSettings = JsonConvert.DeserializeObject<T>(code, _settings);
            PullFrom(loadedSettings);
        }
        catch
        {
            Save<T>();
        }
    }

    public void Save<T>() where T : LoadableModel
    {
        var code = JsonConvert.SerializeObject(this as T, _settings);
        File.WriteAllText(FilePath, code);
    }

    public abstract void PullFrom(LoadableModel? model);
}