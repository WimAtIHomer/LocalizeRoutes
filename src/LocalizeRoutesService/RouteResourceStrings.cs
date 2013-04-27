using System.Dynamic;

namespace IHomer.Services.LocalizeRoutes
{
    public class RouteResourceStrings : DynamicObject
    {
        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            result = RouteResourceService.GetResource(binder.Name);
            return true;
        }
    }
}
