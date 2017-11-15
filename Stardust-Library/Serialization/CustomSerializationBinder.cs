using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json.Serialization;

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

            _typeToName = _nameToType.ToDictionary(
                t => t.Value,
                t => t.Key);
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            if (false == _typeToName.ContainsKey(serializedType))
            {
                // throw error instead? how to handle lists?
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
            return base.BindToType(assemblyName, typeName); 
        }
    }
}