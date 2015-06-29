using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace EnterpriseFramework.Helpers
{
	public static class BootstrapHelpers
	{
        public static IHtmlString BootstrapLabelFor<TModel, TProp>(
				this HtmlHelper<TModel> helper,
				Expression<Func<TModel, TProp>> property)
		{
			return helper.LabelFor(property , new
			{
				@class = "col-md-2 control-label"
			});
		}

		public static IHtmlString BootstrapLabel(
				this HtmlHelper helper,
				string propertyName)
		{
            try
            {
                return helper.Label(propertyName ?? string.Empty , new
                    {
                        @class = "col-md-2 control-label"
                    });
            }
            catch (Exception xpn)
            {
                return helper.Label (xpn.Message + " " + xpn.StackTrace);   
            }
		}
	}
}