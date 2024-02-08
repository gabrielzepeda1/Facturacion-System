using System.Web.Mvc;

namespace WebAppVB.Areas.MVCArea
{
    public class MVCAreaAreaRegistration : AreaRegistration
    {

        public override string AreaName
        {
            get
            {
                return "MVCArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("MVCArea_default", "MVCArea/{controller}/{action}/{id}", new { action = "Index", id = UrlParameter.Optional });
        }
    }
}