using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace Messenger.WebAPI.MultipartJsonSupport
{
    public class FormDataJsonBinder : IModelBinder
    {
        private readonly static JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelName = bindingContext.ModelName;

            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
                return Task.CompletedTask;

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            var json = valueProviderResult.FirstValue;

            if (string.IsNullOrWhiteSpace(json))
                return Task.CompletedTask;

            try
            {
                var model = JsonSerializer.Deserialize(json, bindingContext.ModelType, _options);

                bindingContext.Result = ModelBindingResult.Success(model);
            }
            catch (JsonException ex)
            {
                bindingContext.ModelState.TryAddModelError(modelName, ex, bindingContext.ModelMetadata);
            }

            return Task.CompletedTask;
        }
    }
}
