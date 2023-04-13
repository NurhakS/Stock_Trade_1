using System.Web;
using System.Web.Optimization;

namespace Stock_Trade_1
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
                      "~/Scripts/bootstrap.js","~/Scripts/jquery.min.js"
                      ,"~/Scripts/bootstrap.min.js"
                      ,"~/Scripts/slick.min.js"
                      ,"~/Scripts/script.js"
                       ,"~/Scripts/main.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                     
                   
                      "~/Content/bootstrap.min.css", "~/Content/font-awesome.min.css"
                      , "~/Content/animate.css"
                      , "~/Content/all.css"
                      ,"~/Content/Pe-icon-7-stroke.css"
                      ,"~/Content/themify-icons.css"
                      ,"~/Content/slick.css"
                      ,"~/Content/slick-theme.css"
                      ,"~/Content/style.css"));
        }
    }
}
