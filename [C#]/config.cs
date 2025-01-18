using System;
using System.Collections.Generic;
using System.IO;

public class Config
{
    private readonly Dictionary<string, string> _settings;

    public Config(string content)
    {
        _settings = new Dictionary<string, string>();
        ParseContent(content);
    }

    private void ParseContent(string content)
    {
        if (string.IsNullOrEmpty(content))
            return;

        var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
            var parts = line.Split(new[] { '=' }, 2);
            if (parts.Length == 2)
            {
                _settings[parts[0].Trim()] = parts[1].Trim();
            }
        }
    }

    public string Get(string key, string defaultValue)
    {
        return _settings.TryGetValue(key, out var value) ? value : defaultValue;
    }
}