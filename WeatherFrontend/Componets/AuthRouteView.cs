using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Threading.Tasks;
using WeatherFrontend.Pages;
using WeatherFrontend.Service;

namespace WeatherFrontend.Components
{
    public class AuthRouteView : ComponentBase
    {
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public AuthService AuthService { get; set; }

        [Parameter] public RouteData RouteData { get; set; }
        [Parameter] public Type DefaultLayout { get; set; }

        private bool isAuthenticated;
        private bool isPublicPage;

        protected override async Task OnInitializedAsync()
        {
            if (RouteData == null || AuthService == null || NavigationManager == null)
            {
                NavigationManager.NavigateTo("/login", forceLoad: true);
                return;
            }

            isPublicPage = IsPublicPage(RouteData.PageType);
            if (!isPublicPage)
            {
                isAuthenticated = await AuthService.IsAuthenticated();
                if (!isAuthenticated)
                {
                    NavigationManager.NavigateTo("/login", forceLoad: true);
                }
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            // Always render public pages (Login, Register) regardless of authentication
            if (isPublicPage || isAuthenticated)
            {
                builder.OpenComponent<LayoutView>(0);
                builder.AddAttribute(1, "Layout", DefaultLayout);
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(childBuilder =>
                {
                    childBuilder.OpenComponent(3, RouteData.PageType);
                    foreach (var parameter in RouteData.RouteValues)
                    {
                        childBuilder.AddAttribute(4, parameter.Key, parameter.Value);
                    }
                    childBuilder.CloseComponent();
                }));
                builder.CloseComponent();
            }
        }

        private bool IsPublicPage(Type pageType)
        {
            var publicPages = new[] { typeof(Login), typeof(Register) };
            return publicPages.Contains(pageType);
        }
    }
}