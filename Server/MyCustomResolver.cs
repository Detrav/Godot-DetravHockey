using DetravHockey.Server.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace DetravHockey.Server
{
    public class MyCustomResolver : DefaultJsonTypeInfoResolver
    {
        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

            Type basePointType = typeof(PacketBase);
            if (jsonTypeInfo.Type == basePointType)
            {

                jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                {
                    TypeDiscriminatorPropertyName = "$T",
                    IgnoreUnrecognizedTypeDiscriminators = true,
                    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization
                };

                foreach (var t in GetType().Assembly.DefinedTypes.Where(m => m.IsAssignableTo(typeof(PacketBase)) && !m.IsAbstract))
                {
                    jsonTypeInfo.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(t, t.Name));
                }
            }

            return jsonTypeInfo;
        }
    }
}
