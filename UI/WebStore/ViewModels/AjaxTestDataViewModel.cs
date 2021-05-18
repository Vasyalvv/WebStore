using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.ViewModels
{
    public record AjaxTestDataViewModel(int? Id, string Message, DateTime ServerTime);
}
