using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using static System.Net.Mime.MediaTypeNames;

namespace Messenger.WebAPI.MultipartJsonSupport
{
    public class MultipartJsonOperationFilter : IOperationFilter
    {
        public void Apply(
            OpenApiOperation operation,
            OperationFilterContext context)
        {
            var hasMultipart = context.ApiDescription
               .SupportedRequestFormats
               .Any(x => x.MediaType == Multipart.FormData);
            
            if (!hasMultipart)
                return;

            operation.Parameters.Clear();

            var schema = new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>()
            };

            foreach (var parameter in context.ApiDescription.ParameterDescriptions)
            {
                if (parameter.Source == Microsoft.AspNetCore.Mvc.ModelBinding.BindingSource.FormFile ||
                    parameter.Source == Microsoft.AspNetCore.Mvc.ModelBinding.BindingSource.Custom)
                {
                    var name = parameter.Name;
                    var type = parameter.Type;

                    if (type == typeof(IFormFile) || type == typeof(IFormFile[]))
                    {
                        schema.Properties.Add(name, new OpenApiSchema
                        {
                            Type = "string",
                            Format = "binary"
                        });
                    }
                    else
                    {
                        schema.Properties.Add(name, new OpenApiSchema
                        {
                            Type = "object",
                            Reference = context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository).Reference
                        });
                    }
                }
            }

            operation.RequestBody = new OpenApiRequestBody
            {
                Content =
                {
                    [Multipart.FormData] = new OpenApiMediaType
                    {
                        Schema = schema
                    }
                }
            };
        }
    }
}
