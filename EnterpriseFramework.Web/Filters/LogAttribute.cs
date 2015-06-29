using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EnterpriseFramework.Web.Filters
{
    public class LogAttribute : ActionFilterAttribute
    {
        private IDictionary<string, object> _parameters;
        //public BaseUow Context { get; set; }
        public string  CurrentUserName { get; set; }

        public string Description { get; set; }

        public LogAttribute(string description)//, string currentUserName)
        {
            Description = description;
            //CurrentUserName = currentUserName;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _parameters = filterContext.ActionParameters;
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var description = Description;

            foreach (var kvp in _parameters)
            {
                description = description.Replace("{" + kvp.Key + "}", kvp.Value.ToString());
            }
            //var areaName = "";
            //areaName = ViewContext.RouteData.DataTokens["area"];
            LogAction(CurrentUserName, filterContext.ActionDescriptor.ActionName,
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, description);
        }

        public virtual void LogAction(string CurrentUserName, string Action, string Controller, string description)
        {
        }
    }
}
