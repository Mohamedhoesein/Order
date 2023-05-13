using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Order.API
{
    /// <summary>
    /// Update the routing of every controller as to prefix the route with /api/
    /// </summary>
    public class ModeRouteConvention : IApplicationModelConvention
    {
        /// <inheritdoc/>
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var centralPrefix = new AttributeRouteModel(new RouteAttribute("api/"));
                var routeSelector = controller.Selectors.FirstOrDefault(selectorModel => selectorModel.AttributeRouteModel != null);

                if (
                    controller.Attributes.Any(attribute =>
                        attribute is AreaAttribute areaAttribute &&
                        areaAttribute.RouteValue == Areas.ADMIN
                    )
                )
                {
                    centralPrefix = new AttributeRouteModel(new RouteAttribute("api/admin/"));
                }

                if (routeSelector != null)
                {
                    routeSelector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(centralPrefix,routeSelector.AttributeRouteModel);
                }
                else
                {
                    controller.Selectors.Add(
                        new SelectorModel
                        {
                            AttributeRouteModel = centralPrefix
                        }
                    );
                }
            }
        }
    }
}