using System.Web;
using System.Web.Optimization;

namespace Krakostop
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/assets/js/jquery.dcjqaccordion.2.7.js",
                        "~/assets/js/jquery.scrollTo.min.js",
                        "~/assets/js/jquery.nicescroll.js",
                        "~/assets/js/jquery.sparkline.js",
                        "~/assets/js/common-scripts.js",
                        "~/assets/js/gritter/js/jquery.gritter.js",
                        "~/assets/js/gritter-conf.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/assets/css/site.css",
                      "~/assets/css/style.css",
                      "~/assets/css/style-responsive.css",
                      "~/assets/font-awesome/css/font-awesome.css",
                      "~/assets/js/gritter/css/jquery.gritter.css",
                      "~/assets/lineicons/style.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}
