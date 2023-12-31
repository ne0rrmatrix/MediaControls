﻿using CommunityToolkit.Maui;

namespace MediaControls;
public static class AppHostBuilderExtensions
{
    public static MauiAppBuilder UseMediaControls(this MauiAppBuilder builder)
    {
		builder
	   .UseMauiCommunityToolkit().UseMauiCommunityToolkitMediaElement();
#if WINDOWS
		Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
		{
			var nativeWindow = handler.PlatformView;
			nativeWindow.Activate();
			nativeWindow.ExtendsContentIntoTitleBar = false;

			var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
			var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
			var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

			if (appWindow.Presenter is Microsoft.UI.Windowing.OverlappedPresenter p)
			{
				p.IsResizable = true;
				// these only have effect if XAML isn't responsible for drawing the titlebar.
				p.IsMaximizable = true;
				p.IsMinimizable = true;
			}
		});
#endif
        return builder;
    }
}