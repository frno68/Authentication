// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace BlazorApp2.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\frno\source\repos\Authentication\BlazorApp2\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\frno\source\repos\Authentication\BlazorApp2\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\frno\source\repos\Authentication\BlazorApp2\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\frno\source\repos\Authentication\BlazorApp2\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\frno\source\repos\Authentication\BlazorApp2\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\frno\source\repos\Authentication\BlazorApp2\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\frno\source\repos\Authentication\BlazorApp2\_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\frno\source\repos\Authentication\BlazorApp2\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\frno\source\repos\Authentication\BlazorApp2\_Imports.razor"
using BlazorApp2;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "C:\Users\frno\source\repos\Authentication\BlazorApp2\_Imports.razor"
using BlazorApp2.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "C:\Users\frno\source\repos\Authentication\BlazorApp2\_Imports.razor"
using BlazorApp2.Services.Authentication;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\frno\source\repos\Authentication\BlazorApp2\Pages\Index.razor"
using System.Security.Claims;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Components.RouteAttribute("/")]
    public partial class Index : global::Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 31 "C:\Users\frno\source\repos\Authentication\BlazorApp2\Pages\Index.razor"
      
    [CascadingParameter]
    private Task<AuthenticationState> authenticationState { get; set; }
    private IList<Claim> userClaim
    {
        get
        {
            var auth = authenticationState.Result;
            if (auth.User.Identity.IsAuthenticated)
            {
                return auth.User.Claims.ToList();
            }
            return new List<Claim>();
        }
    }
    private async void Register()
    {
        await _accountService.RegisterAsync("frno","frno","frno","frno@gmail.com");
    }

    private async void Login()
    {
        await _accountService.LoginAsync("frno","frno");
    }

    private async void Logout()
    {
        await _accountService.LogoutAsync();
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IAccountService _accountService { get; set; }
    }
}
#pragma warning restore 1591
