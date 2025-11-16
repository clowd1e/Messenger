using Messenger.Application.Features.Chats.DTO.Responses;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Messenger.WebAPI.Extensions.DI
{
    public static class JsonExtensions
    {
        public static IMvcBuilder ConfigureJsonOptions(this IMvcBuilder builder)
        {
            builder.AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.TypeInfoResolver =
                    new DefaultJsonTypeInfoResolver
                    {
                        Modifiers = { 
                            JsonPolymorphicModifier,
                            PrivateChatModifier,
                            GroupChatModifier
                        }
                    };
            });

            return builder;
        }

        #region chat response polymorphism

        private static void JsonPolymorphicModifier(JsonTypeInfo typeInfo)
        {
            if (typeInfo.Type == typeof(ChatResponse))
            {
                typeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                {
                    TypeDiscriminatorPropertyName = "type",
                    IgnoreUnrecognizedTypeDiscriminators = true,
                    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType
                };

                typeInfo.PolymorphismOptions.DerivedTypes.Add(
                    new JsonDerivedType(typeof(GroupChatResponse), "group"));

                typeInfo.PolymorphismOptions.DerivedTypes.Add(
                    new JsonDerivedType(typeof(PrivateChatResponse), "private"));
            }
        }

        private static void PrivateChatModifier(JsonTypeInfo typeInfo)
        {
            if (typeInfo.Type == typeof(PrivateChatResponse))
            {
                var typeProperty = typeInfo.CreateJsonPropertyInfo(typeof(string), "type");
                typeProperty.Get = obj => "private";
                typeProperty.Set = null;
                typeInfo.Properties.Add(typeProperty);
            }
        }

        private static void GroupChatModifier(JsonTypeInfo typeInfo)
        {
            if (typeInfo.Type == typeof(GroupChatResponse))
            {
                var typeProperty = typeInfo.CreateJsonPropertyInfo(typeof(string), "type");
                typeProperty.Get = obj => "group";
                typeProperty.Set = null;
                typeInfo.Properties.Add(typeProperty);
            }
        }

        #endregion
    }
}
