using GhostNetwork.Profiles.SecuritySettings;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace GhostNetwork.Profiles.Api.Helpers.OpenApi
{
    public class EnumDocumentFilter<TEnum> : IDocumentFilter
        where TEnum : struct, IConvertible
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException($"{typeof(TEnum)} is not an enum");
            }

            var property = swaggerDoc.Components.Schemas[typeof(TEnum).Name];

            var propertyEnums = property.Enum;
            var enumNames = new OpenApiArray();
            if (propertyEnums.Any())
            {
                property.Enum = new List<IOpenApiAny>();
                foreach (var @enum in propertyEnums)
                {
                    var enumValue = Enum.Parse<TEnum>((@enum as OpenApiString).Value);
                    property.Enum.Add(new OpenApiInteger(enumValue.ToInt32(default)));
                    enumNames.Add(new OpenApiString(GetEnumDescription(enumValue as Enum)));
                }
            }

            property.Type = "int";
            property.Description = DescribeEnum(property.Enum);
            property.Extensions.Add("x-enum-varnames", enumNames);
        }

        private string DescribeEnum(IList<IOpenApiAny> enums)
        {
            var enumDescriptions = new List<string>();

            foreach (IOpenApiAny enumOption in enums)
            {
                if (enumOption is OpenApiInteger integer)
                {
                    int enumInt = integer.Value;

                    enumDescriptions.Add(string.Format("{0} - {1}", enumInt, Enum.GetName(typeof(Access), enumInt)));
                }
            }

            return string.Join(", ", enumDescriptions.ToArray());
        }

        private static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
    }
}
