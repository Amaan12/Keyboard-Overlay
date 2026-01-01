using System.Collections.Generic;

public static class DictionaryUtils
{
    /// <summary>
    /// Inverts a dictionary (K -> V) into (V -> K).
    /// If duplicate values exist, the first one wins.
    /// </summary>
    public static Dictionary<TValue, TKey> Invert<TKey, TValue>(
        IDictionary<TKey, TValue> source
    )
    {
        var inverted = new Dictionary<TValue, TKey>();

        foreach (var kv in source)
        {
            // Skip duplicates to avoid exceptions
            if (!inverted.ContainsKey(kv.Value))
                inverted.Add(kv.Value, kv.Key);
        }

        return inverted;
    }
}
