using Messenger.Application.Features.Chats.DTO.Responses;
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
                        Modifiers = { JsonPolymorphicModifier }
                    };
            });

            return builder;
        }

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
    }
}
