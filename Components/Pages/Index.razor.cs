namespace TimeTracker.Components.Pages;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

/// <summary>
/// The index.
/// </summary>
public partial class Index
{
    /// <summary>
    /// Gets or sets the j s runtime.
    /// </summary>
    [Inject]
    protected IJSRuntime JSRuntime { get; set; }

    /// <summary>
    /// Gets or sets the navigation manager.
    /// </summary>
    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    /// <summary>
    /// Gets or sets the dialog service.
    /// </summary>
    [Inject]
    protected DialogService DialogService { get; set; }

    /// <summary>
    /// Gets or sets the tooltip service.
    /// </summary>
    [Inject]
    protected TooltipService TooltipService { get; set; }

    /// <summary>
    /// Gets or sets the context menu service.
    /// </summary>
    [Inject]
    protected ContextMenuService ContextMenuService { get; set; }

    /// <summary>
    /// Gets or sets the notification service.
    /// </summary>
    [Inject]
    protected NotificationService NotificationService { get; set; }
}