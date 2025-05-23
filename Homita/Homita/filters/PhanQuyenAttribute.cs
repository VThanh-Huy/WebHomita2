using System.Web;
using System.Web.Mvc;

public class PhanQuyenAttribute : AuthorizeAttribute
{
    public string Quyen { get; set; }

    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        var role = httpContext.Session["VaiTro"]?.ToString();
        return role == Quyen;
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        filterContext.Result = new RedirectResult("~/Home/KhongCoQuyen");
    }
}
