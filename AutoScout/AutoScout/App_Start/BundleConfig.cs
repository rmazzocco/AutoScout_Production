﻿using System.Web;
using System.Web.Optimization;

namespace AutoScout
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                        "~/Scripts/knockout-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

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
                      "~/Content/Site.css",
                      "~/Content/yeti.bootstrap.css"));

            bundles.Add(new ScriptBundle("~/bundles/filtersearch").Include(
                "~/Scripts/filtersearch.viewmodel.js"));

            bundles.Add(new ScriptBundle("~/bundles/dealership").Include(
                "~/Scripts/dealership.viewmodel.js"));

            bundles.Add(new ScriptBundle("~/bundles/vehicle").Include(
                "~/Scripts/vehicle.viewmodel.js"));
        }
    }
}
