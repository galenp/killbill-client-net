using System.Collections.Generic;

namespace KillBill.Client.Net.Infrastructure
{
    public class MultiMap<TV>
    {
        readonly Dictionary<string, List<TV>> dictionary =
        new Dictionary<string, List<TV>>();

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

        public IEnumerable<string> Keys
        {
            get
            {
                return dictionary.Keys;
            }
        }
        
        public List<TV> this[string key]
        {
            get
            {
                List<TV> list;
                
                if (dictionary.TryGetValue(key, out list)) 
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

       
    }
}