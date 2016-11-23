using System;
using System.Globalization;
using System.Web.Mvc;

namespace Explorify.Web
{
    public class DateTimeModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (value == null)
                return base.BindModel(controllerContext, bindingContext);

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, bindingContext.ValueProvider.GetValue(bindingContext.ModelName));

            DateTime dt;
            var success = DateTime.TryParse(value.AttemptedValue, CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.None, out dt);
            if (success)
            {
                return dt;
            }
            bindingContext.ModelState.AddModelError(bindingContext.ModelName, String.Format("\"{0}\" is invalid.", bindingContext.ModelName));
            return base.BindModel(controllerContext, bindingContext);
        } 
    }
}