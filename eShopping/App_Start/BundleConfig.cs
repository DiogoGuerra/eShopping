using System.Web;
using System.Web.Optimization;

namespace eShopping
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.bundle.min.js", "~/Scripts/bootstrap.js",
                      "~/Scripts/datatables/datatbles.bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/adminlte/plugins/fontawesome-free/css/all.min.css",
                      "~/adminlte/plugins/summernote/summernote-bs4.min.css",
                      "~/adminlte/css/adminlte.min.css",
                      "~/Content/site.css",
                      "~/adminlte/plugins/datatables-bs4/css/dataTables.bootstrap4.css"));

            bundles.Add(new ScriptBundle("~/adminlte/js").Include(
                "~/adminlte/js/adminlte.min.js",
                 "~/adminlte/plugins/summernote/summernote-bs4.min.js",
                 "~/adminlte/plugins/datatables/jquery.dataTables.js",
                "~/adminlte/plugins/datatables-bs4/js/dataTables.bootstrap4.js"));

            bundles.Add(new Bundle("~/crew/css").Include(
               "~/crew/css/bootstrap.css",
               "~/crew/css/animate.css",
               "~/crew/css/icomoon.css",
               "~/crew/css/simple-line-icons.css",
               "~/crew/css/owl.carousel.min.css",
               "~/crew/css/owl.theme.default.min.css",
               "~/crew/css/lightblue.css"
           ));

            bundles.Add(new ScriptBundle("~/crew/js").Include(
                "~/crew/js/modernizr-2.6.2.min.js",
                "~/crew/js/jquery.min.js",
                "~/crew/js/jquery.easing.1.3.js",
                "~/crew/js/bootsrap.min.js",
                "~/crew/js/jquery.waypoints.min.js",
                "~/crew/js/owl.carousel.min.js",
                "~/crew/js/main.js"
            ));
        }
    }
}
