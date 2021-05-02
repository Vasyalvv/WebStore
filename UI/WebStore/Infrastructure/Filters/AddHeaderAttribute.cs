using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace WebStore.Infrastructure.Filters
{
    public class AddHeaderAttribute : ResultFilterAttribute
    {
        private readonly string _Name;
        private readonly string _Value;
        public AddHeaderAttribute(string Name, string Value)
        {
            _Name = Name ?? throw new ArgumentNullException(nameof(Name));
            _Value = Value;
        }



        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (_Value != null)
                context.HttpContext.Response.Headers.Add(_Name, new StringValues(_Value));
            base.OnResultExecuting(context);
        }
    }
}
