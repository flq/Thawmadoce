using System.Linq;
using System.Reflection;
using Caliburn.Micro;

namespace Thawmadoce.Frame.Extensions
{
    public static class ViewModelExtensions
    {
        /// <summary>
        /// Checks public instance properties for ones whose return types end with "ViewModel"
        /// and tries to invoke IActivate on it
        /// </summary>
        public static void ActivateAllChilds(this object parentViewModel)
        {
            var props =
                from prop in parentViewModel.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where prop.CanRead && prop.PropertyType.Name.EndsWith("ViewModel")
                select prop.GetGetMethod();

            props.ForEach(mi=>mi.Invoke(parentViewModel, new object[0]).As<IActivate>(a=>a.Activate()));

        }
    }
}