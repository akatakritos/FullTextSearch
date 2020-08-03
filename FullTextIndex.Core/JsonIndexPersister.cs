using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FullTextIndex.Core
{
    public class JsonIndexPersister
    {
        public Task PersistAsync(string filename, SingleFieldIndex index)
        {
            return Task.Run(() =>
            {
                using (var fs = File.Create(filename))
                using (var stream = new JsonTextWriter(new StreamWriter(fs)))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(stream, index.GetStateForSerialization());
                }
            });
        }

        public Task<SingleFieldIndex> RestpreAsync(string filename)
        {
            return Task.Run(() =>
            {
                using (var fs = File.OpenRead(filename))
                using (var stream = new JsonTextReader(new StreamReader(fs)))
                {
                    var serializer = new JsonSerializer();
                    var state = serializer.Deserialize<SingleIndexState>(stream);
                    return new SingleFieldIndex(state);
                }
            });
        }
    }
}
