using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using Autobarn.Data.Entities;

namespace Autobarn.Website.Controllers.api {
    public static class HypermediaExtensions {
        public static dynamic ToDynamic(this object value) {
            IDictionary<string, object> expando = new ExpandoObject();
            var properties = TypeDescriptor.GetProperties(value.GetType());
            foreach (PropertyDescriptor property in properties) {
                if (Ignore(property)) continue;
                expando.Add(property.Name, property.GetValue(value));
            }
            return expando;
        }

        public static dynamic ToResource(this Model m) {
            var result = m.ToDynamic();
            result._links = new {
                self = new {
                    href = $"/api/models/{m.Code}"
                }
            };
            return result;
        }

        public static dynamic ToResource(this Vehicle v) {
            var result = v.ToDynamic();
            result._links = new {
                self = new {
                    href = $"/api/vehicles/{v.Registration}"
                },
                model = new {
                    href = $"/api/models/{v.ModelCode}"
                }
            };
            return result;
        }

        private static bool Ignore(PropertyDescriptor property) => property
                .Attributes
                .OfType<Newtonsoft.Json.JsonIgnoreAttribute>()
                .Any();
    }
}
