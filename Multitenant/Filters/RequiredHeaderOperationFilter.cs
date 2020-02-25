using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Multitenant.Filters
{
    public class RequiredHeaderOperationFilter : IOperationFilter
    {
        public RequiredHeaderOperationFilter()
        {
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            var schema = new OpenApiSchema
            {
                Type = "String"
            };
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Tenant-Id",
                Description = "Tenant id for the request",
                In = ParameterLocation.Header,
                Required = false,
                Example = OpenApiAnyFactory.CreateFor(schema, "first-tenant"),
                Schema = schema
            });
        }
    }
}