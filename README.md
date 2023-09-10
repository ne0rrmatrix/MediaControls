# MediaControls Is a Custom UI for Dotnet Maui MediaElement

Currently it works with Navigation Page, Tabbed page, and Shell page.
Windows, Android, IOS and Mac supported. Full screen is supported by Windows,
android and IOS. It is not supported on Mac Catalyst.
You can set the page to full screen using MediaControls full screen option
which is the image button in upper right of screen. Controls show at start
of playback.

Clicking or tapping on page will cause the controls to appear for 7 seconds.
In windows double clicking on page will set page to full screen or restore 
the screen to default size. On Android swiping up will enter full screen
and swiping down will exit full screen. If you have MediaControls as 
only element on page full screen controls will work properly.

You don't need to worry about tab bar, title, or nav bar it will be hidden or shown
with state preserved so that if you go full screen then back again it will
show the previous state upon restore default page size.

## API Examples:
Play, Pause, Stop, Mute, UnMute, Fast Forward, Rewind, ShowCustomControls,
ShouldAutoPlay, ShouldKeepScreenOn, SeekTo, Aspect, Source, ShouldMute

## Code Behind Examples
```
  mediaControl.ShouldMute = false;
        mediaControl.Play();
        mediaControl.Pause();
        mediaControl.Stop();
        mediaControl.ShouldKeepScreenOn = false;
        mediaControl.ShouldAutoPlay = false;
        mediaControl.Aspect = Aspect.AspectFit;
        mediaControl.Source = "https://somevideo.mp4";
```

## Xaml Examples
```
 <controls:MediaControl
            ShouldAutoPlay="True"
            ShouldKeepScreenOn="True"
            ShowCustomControls="True"
            Source="https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4" />
```

## Example for xaml usage:
```
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage
    x:Class="MauiApp1.MainPage"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:controls="clr-namespace:MediaControls.Controls;assembly=MediaControls">
    <Grid BackgroundColor="Black">
        <controls:MediaControl
            x:Name="mediaControl"
            ShouldAutoPlay="True"
            ShouldKeepScreenOn="True"
            ShowCustomControls="True"
            Source="https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4" />
    </Grid>
</ContentPage>
```

You need to add the lines below but you do not need to add the packages for CommunityToolkit or MediaElement
directly unless you want to use them for something else. But you do need to add the code below in MauiPorgram.cs
as described for this to work.

## The important part is:
```
.UseMauiCommunityToolkit().UseMauiCommunityToolkitMediaElement().UseMediaControls();
```

## Example for MauiProgram.cs
```
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using MediaControls;

namespace MauiApp1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            }).UseMauiCommunityToolkit().UseMauiCommunityToolkitMediaElement().UseMediaControls();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
```
Link to nuget package: [Nuget](https://www.nuget.org/packages/MediaControls.Maui/)