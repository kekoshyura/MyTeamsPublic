using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using Microsoft.JSInterop.Infrastructure;
using System.Collections.Immutable;
using MyTeamsCore;
using MyTeamsCore.Common;

namespace MyTeams.Client.Components;
public static class 
JSRuntimeHelper {

    public static ImmutableDictionary<Guid, object> 
    InteropObjects = ImmutableDictionary<Guid, object>.Empty;
    
    public static Guid
    AddAsInteropObject(this object @object) {
        var guid = Guid.NewGuid();
        InteropObjects = InteropObjects.Add(guid, @object);
        return guid;
    }

    public static bool 
    TryGetInteropObject(this Guid objectGuid, out object result) => InteropObjects.TryGetValue(objectGuid, out result);
    
    public static void 
    RemoveInteropObject(this Guid objectGuid) => InteropObjects = InteropObjects.Remove(objectGuid);
    
    public static async Task<double>
    GetScrollPercentage(this ElementReference element, IJSRuntime jSRuntime) => 
        (await element.GetScrollTop(jSRuntime) + (await element.GetBoundingClientRect(jSRuntime)).Height) / await element.GetScrollHeight(jSRuntime) * 100;

    public static async Task<double>
    GetScrollTop(this ElementReference element, IJSRuntime jSRuntime) => 
        await element.GetProperty<double>("scrollTop", jSRuntime);
    
    public static async Task
    SetScrollTop(this ElementReference element, double value, IJSRuntime runtime) =>
        await element.SetProperty("scrollTop", value, runtime);

    public static async Task<double>
    GetScrollHeight(this ElementReference element, IJSRuntime jSRuntime) => 
        await element.GetProperty<double>("scrollHeight", jSRuntime);

    public static async Task<int>
    GetElementWidth(this ElementReference element, IJSRuntime jSRuntime) =>
         await element.GetProperty<int>("clientWidth", jSRuntime);

    public static async Task<int>
    GetElementHeight(this ElementReference element, IJSRuntime jSRuntime) =>
         await element.GetProperty<int>("clientHeight", jSRuntime);

    public static async Task<double>
    GetElementWidthByClass(this IJSRuntime jSRuntime, string className)  {
          var clientRect = await jSRuntime.InvokeAsync<DomRectangle>("hand2note.getBoundingClientRectByClass", className);
          return clientRect.width;
    }

    public static async Task<T> 
    GetProperty<T>(this ElementReference element, string property, IJSRuntime jsRuntime) => 
        await jsRuntime.InvokeAsync<T>("hand2note.getElementProperty", element, property);
    
    public static async Task 
    SetProperty(this ElementReference element, string property, object value, IJSRuntime jsRuntime) => 
        await jsRuntime.InvokeVoidAsync("hand2note.setElementProperty", element, property, value);

    public static async Task 
    SetStyleProperty(this ElementReference element, string property, object value, IJSRuntime jsRuntime) => 
        await jsRuntime.InvokeVoidAsync("hand2note.setElementStyleProperty", element, property, value);

    public static async Task<T> 
    GetStyleProperty<T>(this ElementReference element, string property, IJSRuntime jsRuntime) => 
        await jsRuntime.InvokeAsync<T>("hand2note.getElementStyleProperty", element, property);

    public static async Task 
    SetWidth(this ElementReference element, int newWidth, IJSRuntime jsRuntime) => 
        await element.SetStyleProperty("width", $"{newWidth}px", jsRuntime);

    public static async Task<Rectangle>
    GetBoundingClientRect(this ElementReference element, IJSRuntime runtime) =>
        (await runtime.InvokeAsync<DomRectangle>("hand2note.getBoundingClientRect", element)).ToRectangle();

    public static async Task<Rectangle>
    GetBoundingClientRectById(this string elementId, IJSRuntime runtime) =>
        (await runtime.InvokeAsync<DomRectangle>("hand2note.getBoundingClientRectById", elementId)).ToRectangle();
    
    public static async Task<Rectangle>
    GetBoundingClientRectByClass(this string @class, IJSRuntime runtime) =>
        (await runtime.InvokeAsync<DomRectangle>("hand2note.getBoundingClientRectByClass", @class)).ToRectangle();

    public static async Task<Point>
    GetNewDropdownElementPosition(this IJSRuntime runtime, ElementReference menu, ElementReference menuItems) {
        var menuRect = (await runtime.InvokeAsync<DomRectangle>("hand2note.getBoundingClientRect", menu)).ToRectangle();
        var menuItemsRect = (await runtime.InvokeAsync<DomRectangle>("hand2note.getBoundingClientRect", menuItems)).ToRectangle();
        var windowClientArea = await runtime.GetWindowBoundingClientRect();
        var hiddenWidth = menuRect.Left + menuItemsRect.Width - windowClientArea.Right;
        var hiddenHeight = menuRect.Bottom + menuItemsRect.Height - windowClientArea.Bottom;
        var newLeft = hiddenWidth > 0 
            ? hiddenHeight > 0 
                ? menuRect.Left - menuItemsRect.Width
                : menuRect.Left - hiddenWidth 
            : hiddenHeight > 0 
                ? menuRect.Right
                : menuRect.Left;
        var newTop = hiddenHeight > 0 ? menuRect.Bottom - hiddenHeight : menuRect.Bottom;
        return new Point(newLeft, newTop);
    }
    
    public static async Task<Point>
    GetNewCascadingMenuPosition(this IJSRuntime runtime, ElementReference menu, ElementReference menuItems) {
        var menuRect = (await runtime.InvokeAsync<DomRectangle>("hand2note.getBoundingClientRect", menu)).ToRectangle();
        var menuItemsRect = (await runtime.InvokeAsync<DomRectangle>("hand2note.getBoundingClientRect", menuItems)).ToRectangle();
        var windowClientArea = await runtime.GetWindowBoundingClientRect();
        var hiddenWidth = menuRect.Right + menuItemsRect.Width - windowClientArea.Right;
        var hiddenHeight = menuRect.Top + menuItemsRect.Height - windowClientArea.Bottom;
        var newLeft = hiddenWidth > 0 
            ? menuRect.Left - menuItemsRect.Width
            : menuRect.Right;
        var newTop = hiddenHeight > 0 ? menuRect.Top - hiddenHeight : menuRect.Top;
        return new Point(newLeft, newTop);
    }
    
    public static async Task<Rectangle>
    GetChildWithClassBoundingClientRect(this ElementReference element, string childClass, IJSRuntime runtime) =>
        (await runtime.InvokeAsync<DomRectangle>("hand2note.getChildWithClassBoundingClientRect", element, childClass)).ToRectangle();
    
    public static async Task<Rectangle>
    GetParentBoundingClientRect(this ElementReference element, IJSRuntime runtime) =>
        (await runtime.InvokeAsync<DomRectangle>("hand2note.getParentBoundingClientRect", element)).ToRectangle();

    public static async Task<Rectangle>
    GetBoundingClientRectRelativeToParent(this ElementReference element, IJSRuntime runtime){
        var rect = await element.GetBoundingClientRect(runtime);
        var parentRect = await element.GetParentBoundingClientRect(runtime);
        return rect.RelativeTo(parentRect.TopLeftCorner);
    }

    public static async Task<Rectangle>
    GetChildWithClassRelativeBoundingClientRect(this ElementReference parentElement, string childClass, IJSRuntime runtime){
        var rect = await parentElement.GetChildWithClassBoundingClientRect(childClass, runtime);
        var parentRect = await parentElement.GetBoundingClientRect(runtime);
        return rect.RelativeTo(parentRect.TopLeftCorner);
    }

    public static async Task<string>
    GetVisualBackgroundColor(this ElementReference element, IJSRuntime runtime) =>
        await runtime.InvokeAsync<string>("hand2note.getVisualBackgroundColor", element);
    
    public static async Task
    CapturePointer(this ElementReference element, IJSRuntime runtime) =>
        await runtime.InvokeVoidAsync("hand2note.setPointerCapture", element, 1);
    
    public static async Task
    ReleasePointer(this ElementReference element, IJSRuntime runtime) => 
        await runtime.InvokeVoidAsync("hand2note.releasePointerCapture", element, 1);

    public static async Task
    StopMouseUpEventPropagation(this ElementReference element, IJSRuntime runtime) => 
        await runtime.InvokeVoidAsync("hand2note.stopPropagateMouseUp", element);
    
    public static async Task
    StopClickEventPropagation(this ElementReference element, IJSRuntime runtime) => 
        await runtime.InvokeVoidAsync("hand2note.stopPropagateClick", element);
    
    public static async Task
    StopMouseDownEventPropagation(this ElementReference element, IJSRuntime runtime) => 
        await runtime.InvokeVoidAsync("hand2note.stopPropagateMouseDown", element);
    
    public static async Task
    FocusElement(this ElementReference element, IJSRuntime runtime) =>
        await runtime.InvokeVoidAsync("hand2note.focusElement", element);
    
    public static async Task 
    SelectElement(this ElementReference element, IJSRuntime runtime) => 
        await runtime.InvokeVoidAsync("hand2note.selectElement", element);

    public static async Task<bool>
    IsElementVisibleById(this IJSRuntime runtime, int id) => 
        await runtime.InvokeAsync<bool>("hand2note.isElementVisibleById", id);

    public static async Task 
    ScrollToElementById(this IJSRuntime runtime, int itemIndex, ElementReference parent, int itemsCount) => 
        await runtime.InvokeVoidAsync("hand2note.scrollToElementById", itemIndex, parent, itemsCount);
    
        public static async Task<Rectangle>
        GetWindowBoundingClientRect(this IJSRuntime runtime) {
            var result = await runtime.InvokeAsync<DomRectangle>("hand2note.getWindowBoundingClientRect");
            return new Rectangle(result.x, result.y, result.width, result.height);
        }
    
    public static async Task 
    AppendToWindow(this IJSRuntime runtime, ElementReference element) => 
        await runtime.InvokeVoidAsync("hand2note.appendToWindow", element);
    
    public static async Task 
    AppendToParent(this IJSRuntime runtime, ElementReference element, ElementReference parent) => 
        await runtime.InvokeVoidAsync("hand2note.appendToParent", element, parent);
    
    public static async Task 
    SetupMenuHandlers(this IJSRuntime runtime) => 
        await runtime.InvokeVoidAsync("hand2note.setupMenuHandlers");
    
    public static async Task 
    SetupCascadingMenuHandlers(this IJSRuntime runtime, ElementReference element, ElementReference items) => 
        await runtime.InvokeVoidAsync("hand2note.setupCascadingMenuHandlers", element, items);
    
    public static async Task 
    SendMessage(this IJSRuntime runtime, string message) => 
        await runtime.InvokeVoidAsync("hand2note.sendMessage", message);

    public static async Task
    AddClass(this ElementReference element, string className, IJSRuntime runtime) =>
        await runtime.InvokeVoidAsync("hand2note.addElementClass", element, className);

    public static async Task
    RemoveClass(this ElementReference element, string className, IJSRuntime runtime) =>
        await runtime.InvokeVoidAsync("hand2note.removeElementClass", element, className);

    public static async ValueTask
    AutoGrow(this ElementReference element, IJSRuntime runtime) =>
        await runtime.InvokeAsync<IJSVoidResult>("hand2note.autoGrow", element);

    public static async Task<string>
    GetSelectedText(this IJSRuntime runtime) =>
        await runtime.InvokeAsync<string>("hand2note.getSelectionText");

    public static async Task<string>
    InsertTextInSelection(this IJSRuntime runtime, string newText, ElementReference element) =>
        await runtime.InvokeAsync<string>("hand2note.insertTextInSelection", newText, element);

}
public struct DomRectangle {
    public double x {get;set;}
    public double y {get;set;}
    public double width {get;set;}
    public double height {get;set;}

    public Rectangle ToRectangle() => new(x, y, width, height);
}
