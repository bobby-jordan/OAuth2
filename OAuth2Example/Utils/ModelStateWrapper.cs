using OAuth2Example.DataServices;
using System;
using System.Web.Http.ModelBinding;

namespace OAuth2Example.Utils
{
    internal class ModelStateWrapper : IValidationDictionary
    {
        private ModelStateDictionary modelState;

        public ModelStateWrapper(ModelStateDictionary modelState)
        {
            this.modelState = modelState ?? throw new ArgumentNullException(nameof(modelState));
        }

        public bool IsValid => modelState.IsValid;

        public void AddError(string key, string value)
        {
            modelState.AddModelError(key, value);
        }

        public void Clear()
        {
            modelState.Clear();
        }
    }
}
