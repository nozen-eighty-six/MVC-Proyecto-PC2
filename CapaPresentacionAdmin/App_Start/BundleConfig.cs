using System.Web;
using System.Web.Optimization;

namespace CapaPresentacionAdmin
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new Bundle("~/bundles/Complementos").Include(
                        "~/Scripts/scripts.js",
                        "~/Scripts/Datatables/jquery.dataTables.js",
                        "~/Scripts/Datatables/dataTables.responsive.js",
                        "~/Scripts/fontawesome/all.min.js",
                        "~/Scripts/loadingoverlay/loadingoverlay.min.js",
                         "~/Scripts/sweetalert.min.js",
                          "~/Scripts/jquery.validate.js",
                          "~/Scripts/jquery-ui.js",
                          "~/Scripts/popper.min.js",
                          "~/Scripts/perfect-scrollbar.min.js",
                          "~/Scripts/smooth-scrollbar.min.js"
                        ));
            /*
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));*/

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(

                      "~/Content/Datatables/css/jquery.dataTables.css",
                      "~/Content/Datatables/css/responsive.dataTables.css",
                      "~/Content/sweetalert.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/material-dashboard.css"

                      ));
        }
    }
}
