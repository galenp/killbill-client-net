using System.Collections.Generic;

namespace KillBill.Client.Net.Infrastructure
{
    public class MultiMap<TV>
    {
        public MultiMap<TV> Create(MultiMap<TV> from)
        {
            this.dictionary = from.Dictionary;
            return this;
        }

        private Dictionary<string, List<TV>> dictionary = new Dictionary<string, List<TV>>();
        public Dictionary<string, List<TV>> Dictionary => dictionary;

        public void Add(string key, TV value)
        {
            List<TV> list;
            if (dictionary.TryGetValue(key, out list))
            {
                list.Add(value);
            }
            else
            {
                list = new List<TV> {value};
                dictionary[key] = list;
            }
        }
        private void Add(string key, List<TV> queryParam)
        {
            queryParam.ForEach(x => Add(key, x));
        }

        public void RemoveAll(string key)
        {
            dictionary.Remove(key);
        }

        public IEnumerable<string> Keys => dictionary.Keys;
        
        public List<TV> this[string key]
        {
            get
            {

                if (dictionary.TryGetValue(key, out List<TV> list))
                    return list;

                list = new List<TV>();
                dictionary[key] = list;
                return list;
            }
        }

        public void PutAll(MultiMap<TV> queryParams)
        {
            foreach (var key in queryParams.Keys)
            {
                Add(key, queryParams[key]);
            }
        }

        public void PutAll(string key, List<TV> queryParams)
        {
            foreach (var value in queryParams) {
                Add(key, value);
            }
        }
    }
}