﻿using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace PatteDoie.Components;

public partial class BasePage : ComponentBase
{
    [Inject]
    protected ProtectedLocalStorage ProtectedLocalStorage { get; set; } = default!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    protected ToastService ToastService { get; set; } = default!;

    protected async Task<bool> IsAuthenticated()
    {
        try
        {
            var uuid = await ProtectedLocalStorage.GetAsync<string>("uuid");
            var name = await ProtectedLocalStorage.GetAsync<string>("name");

            return (uuid.Value ?? "") != "" && (name.Value ?? "") != "";
        }
        catch
        {
            return false;
        }
    }
    protected async Task<string> GetUUID()
    {
       try
        {
            var result = await ProtectedLocalStorage.GetAsync<string>("uuid");
            if (!result.Success)
            {
                return "";
            }
            return result.Value ?? "";
        } catch
        {
            return "";
        }
    }

    protected async Task<string> GetName()
    {
        try
        {
            var result = await ProtectedLocalStorage.GetAsync<string>("name");
            if (!result.Success)
            {
                return "";
            }
            return result.Value ?? "";
        } catch
        {
            return "";
        }
    }
}
