using Microsoft.AspNetCore.Mvc;

namespace Order.API.Util.Middleware
{
    /// <summary>
    /// A class to contain functions to handle the model state.
    /// </summary>
    public static class ModelStateHandler
    {
        /// <summary>
        /// A handler for invalid model state.
        /// It sets the return just have the properties for the model with an array of all the errors
        /// </summary>
        /// <param name="context">
        /// The <see cref="ActionContext"/> that contains the model.
        /// </param>
        /// <returns>
        /// A <see cref="JsonResult"/> with for each property in the model that has an error an array with those errors.
        /// </returns>
        public static IActionResult InvalidModelState(ActionContext context)
        {
            var errors = new Dictionary<string, List<string>>();
            foreach (var modelStateKey in context.ModelState.Keys)
            {
                var modelStateVal = context.ModelState[modelStateKey];
                if (modelStateVal is {Errors.Count: > 0})
                    errors.Add(modelStateKey, modelStateVal.Errors.Select(error => error.ErrorMessage).ToList());
            }

            return new JsonResult(errors)
            {
                StatusCode = 400
            };
        }
    }
}