using System.Web;
using System.Web.Mvc;

namespace ean_eic_checker_service {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }
    }
}