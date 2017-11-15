using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Stardust.Emitters;

namespace Stardust.Serialization
{
    public class StardustSerializer
    {

        public JsonSerializerSettings Settings;
        
        public StardustSerializer()
        {
            Settings = new JsonSerializerSettings();
            Settings.TypeNameHandling = TypeNameHandling.All;
            Settings.SerializationBinder = new CustomSerializationBinder();
            Settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
        
        public string Serialize(Emitter2D emitter)
        {
            string json = JsonConvert.SerializeObject(emitter, Formatting.Indented, Settings);
            return json;
        }

        public Emitter2D Deserialize(string jsonString)
        {
            var back = JsonConvert.DeserializeObject<Emitter2D>(jsonString, Settings);
            return back;
        }
    }
}