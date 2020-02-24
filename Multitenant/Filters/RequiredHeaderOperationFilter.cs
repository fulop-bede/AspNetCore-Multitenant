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

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Tenant-Id",
                Description = "Tenant id for the request",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "String"
                }
            });
        }
    }
}