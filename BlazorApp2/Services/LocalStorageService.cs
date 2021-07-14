using System.Collections.Generic;

namespace BlazorApp2.Services
{
    public interface ILocalStorageService
    {
        string GetItem(string key);
        void RemoveItem(string key);
        void SetItem(string key, string value);
    }

    public class LocalStorageService : ILocalStorageService
    {
        private readonly List<KeyValuePair<string, string>> Items = new List<KeyValuePair<string, string>>();
        public void SetItem(string key, string value)
        {
            if (Items.Find(k => k.Key == key).Key != null)
            {
                RemoveItem(key);
            }
            Items.Add(new KeyValuePair<string, string>(key, value));
        }
        public void RemoveItem(string key)
        {
            var item = Items.Find(k => k.Key == key);
            Items.Remove(item);
        }
        public string GetItem(string key)
        {
            return Items.Find(k => k.Key == key).Value;
        }

    }
}
