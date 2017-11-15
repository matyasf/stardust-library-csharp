
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json.Serialization;
using Stardust.Fields;
using Stardust.Initializers;
using Stardust.Zones;

namespace Stardust.Serialization
{
    public class CustomSerializationBinder : DefaultSerializationBinder
    {
        private readonly Dictionary<string, Type> _nameToType;
        private readonly Dictionary<Type, string> _typeToName;

        // sets the $type property in the JSON to the class name
        public CustomSerializationBinder()
        {
            var customDisplayNameTypes =
                GetType()
                    .Assembly
                    .GetTypes()
                    .Where(x => x.IsSubclassOf(typeof(StardustElement)));

            _nameToType = customDisplayNameTypes.ToDictionary(
                t => t.Name,
                t => t);
            
            _nameToType.Add("Array.Action", typeof(List<Actions.Action>));
            _nameToType.Add("Array.Initializer", typeof(List<Initializer>));
            _nameToType.Add("Array.Zone", typeof(List<Zone>));
            _nameToType.Add("Array.Field", typeof(List<Field>));

            _typeToName = _nameToType.ToDictionary(
                t => t.Value,
                t => t.Key);
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            if (false == _typeToName.ContainsKey(serializedType))
            {
                Debug.WriteLine("WARNING: Serializing possibly unknown type " + serializedType);
                base.BindToName(serializedType, out assemblyName, out typeName);
                return;
            }

            var name = _typeToName[serializedType];

            assemblyName = null;
            typeName = name;
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            if (_nameToType.ContainsKey(typeName))
            {
                return _nameToType[typeName];   
            }
            // Not good, we're deserializing an unknown type!
            return base.BindToType(assemblyName, typeName); 
        }
    }
}