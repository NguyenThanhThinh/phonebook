using System.Collections.Generic;
using System.Web.Routing;

namespace phonebook.ViewModels
{
    public class BaseListViewModel<Entity>
    {
        public List<Entity> Entities { get; set; }

        public RouteValueDictionary RouteDictionary { get; set; }
    }
}