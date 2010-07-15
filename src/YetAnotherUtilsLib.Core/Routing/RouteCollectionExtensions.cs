using System.Web.Mvc;
using System.Web.Routing;

namespace YetAnotherUtilsLib.Core.Routing
{
    /// <summary>
    /// http://stackoverflow.com/questions/878578/how-can-i-have-lowercase-routes-in-asp-net-mvc
    /// </summary>
    public static class RouteCollectionExtensions
    {
        public static Route MapRouteLowerCase(this RouteCollection routes, string name, string url, object defaults)
        {
            return routes.MapRouteLowerCase(name, url, defaults, null);
        }

        public static Route MapRouteLowerCase(this RouteCollection routes, string name, string url, object defaults, string constraints)
        {
            Route route = new LowercaseRoute(url, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints)
            };

            routes.Add(name, route);

            return route;
        }
    }
}