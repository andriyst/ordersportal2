using System.Web.Optimization;

namespace OrdersPortal.WebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            //bundles.Add(new ScriptBundle("~/bundles/slider-radios").Include(
            //            "~/Scripts/extra/jquery.radios-to-slider.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					 // "~/Scripts/extra/popper.js",
					 // "~/Scripts/bootstrap5/bootstrap.js",
					  //"~/Scripts/bootstrap5/bootstrap.bundle.js",
					  "~/Scripts/extra/bootstrap-table.js",
					  "~/Scripts/extra/bootstrap-table-uk-UA.js",
					  "~/Scripts/extra/bootstrap-datepicker.js",
					  "~/Scripts/extra/bootstrap-switch.js",
					  "~/Scripts/extra/fontawesome/all.min.js",
					  "~/Scripts/extra/bootstrap-select/bootstrap-select.js",
					  "~/Scripts/moment.min.js",
					  "~/Scripts/site.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap5/bootstrap.css",
					  "~/Content/ordersportal.css",
					  "~/Content/extra/bootstrap-table.css",
					  "~/Content/extra/bootstrap-switch.css",
					  "~/Content/extra/bootstrap-datepicker3.css",
					  "~/Content/extra/fontawesome/all.min.css",
					  "~/Content/extra/bootstrap-select/bootstrap-select.css",
                      "~/Content/site.css"));
        }
    }
}
