// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using MauiPageFullScreen;

namespace MediaControls.Control;
public partial class MediaControl : ContentView
{
    #region Properties and Bindable Properties
    public bool IsNotMac { get; set; }
    public string PlayPosition { get; set; }
    public bool MenuIsVisible { get; set; } = false;

    private static bool s_fullScreen = false;
    private bool PlaybackControls { get; set; } = false;

    public bool FullScreen { get; set; } = false;
    #endregion
    #region Bindably Properties

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Name), typeof(MediaElement), typeof(MediaControl));
    public static readonly BindableProperty AspectProperty = BindableProperty.Create(nameof(Aspect), typeof(Aspect), typeof(MediaControl), propertyChanged: (bindableProperty, oldValue, newValue) =>
    {
        var control = (MediaControl)bindableProperty;
        control.mediaElement.Aspect = (Aspect)newValue;
    });
    public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(MediaSource), typeof(MediaControl), propertyChanged: (bindableProperty, oldValue, newValue) =>
    {
        var control = (MediaControl)bindableProperty;
        control.mediaElement.Source = newValue as MediaSource;
    });
    public static readonly BindableProperty StateChangedProperty = BindableProperty.Create(nameof(StateChanged), typeof(EventHandler<MediaStateChangedEventArgs>), typeof(MediaControl), propertyChanged: (bindableProperty, oldValue, newValue) =>
    {
        var control = (MediaControl)bindableProperty;
        control.mediaElement.StateChanged += (EventHandler<MediaStateChangedEventArgs>)newValue;
    });
    public static readonly BindableProperty MediaOpenedProperty = BindableProperty.Create(nameof(MediaOpened), typeof(EventHandler), typeof(MediaControl), propertyChanged: (bindableProperty, oldValue, newValue) =>
    {
        var control = (MediaControl)bindableProperty;
        control.mediaElement.MediaOpened += (EventHandler)newValue;
    });
    public static readonly BindableProperty ShouldKeepScreenOnProperty = BindableProperty.Create(nameof(ShouldKeepScreenOn), typeof(bool), typeof(MediaControl), propertyChanged: (bindableProperty, oldValue, newValue) =>
    {
        var control = (MediaControl)bindableProperty;
        control.mediaElement.ShouldKeepScreenOn = (bool)newValue;
    });
    public static readonly BindableProperty PositionProperty = BindableProperty.Create(nameof(Position), typeof(TimeSpan), typeof(MediaElement), TimeSpan.Zero);
    public static readonly BindableProperty ShouldAutoPlayProperty = BindableProperty.Create(nameof(ShouldAutoPlay), typeof(bool), typeof(MediaControl), propertyChanged: (bindableProperty, oldValue, newValue) =>
    {
        var control = (MediaControl)bindableProperty;
        control.mediaElement.ShouldAutoPlay = (bool)newValue;
    });
    public static readonly BindableProperty PositionChangedProperty = BindableProperty.Create(nameof(PositionChanged), typeof(EventHandler<MediaPositionChangedEventArgs>), typeof(MediaControl), propertyChanged: (bindableProperty, oldValue, newValue) =>
    {
        var control = (MediaControl)bindableProperty;
        control.mediaElement.PositionChanged += (EventHandler<MediaPositionChangedEventArgs>)newValue;
    });
    public static readonly BindableProperty ShouldMuteProperty = BindableProperty.Create(nameof(ShouldMute), typeof(bool), typeof(MediaControl), false, propertyChanged: (bindableProperty, oldValue, newValue) =>
    {
        var control = (MediaControl)bindableProperty;
        control.mediaElement.ShouldMute = (bool)newValue;
    });
    public bool ShouldMute
    {
        get => (bool)GetValue(ShouldMuteProperty);
        set => SetValue(ShouldMuteProperty, value);
    }
    public EventHandler MediaOpened
    {
        get => GetValue(MediaOpenedProperty) as EventHandler;
        set => SetValue(MediaOpenedProperty, value);
    }
    public EventHandler<MediaStateChangedEventArgs> StateChanged
    {
        get => GetValue(StateChangedProperty) as EventHandler<MediaStateChangedEventArgs>;
        set => SetValue(StateChangedProperty, value);
    }
    public EventHandler<MediaPositionChangedEventArgs> PositionChanged
    {
        get => GetValue(PositionChangedProperty) as EventHandler<MediaPositionChangedEventArgs>;
        set => SetValue(PositionChangedProperty, value);
    }
    public MediaSource Source
    {
        get => GetValue(SourceProperty) as MediaSource;
        set => SetValue(SourceProperty, value);
    }
    public TimeSpan Position => mediaElement.Position;
    public MediaElement Name
    {
        get => GetValue(TitleProperty) as MediaElement;
        set => SetValue(TitleProperty, value);
    }
    public bool ShowCustomControls
    {
        get => PlaybackControls;
        set => PlaybackControls = value;
    }
    public bool ShouldAutoPlay
    {
        get => (bool)GetValue(ShouldAutoPlayProperty);
        set => SetValue(ShouldAutoPlayProperty, value);
    }
    public bool ShouldKeepScreenOn
    {
        get => (bool)GetValue(ShouldKeepScreenOnProperty);
        set => SetValue(ShouldKeepScreenOnProperty, value);
    }
    public Aspect Aspect
    {
        get => (Aspect)GetValue(AspectProperty);
        set => SetValue(AspectProperty, value);
    }
    #endregion
    public MediaControl()
    {
        InitializeComponent();
#if MACCATALYST
        IsNotMac = false;
#endif
#if ANDROID || WINDOWS || IOS
        IsNotMac = true;
#endif
        PlayPosition = string.Empty;
        mediaElement.PropertyChanged += MediaElement_PropertyChanged;
        mediaElement.PositionChanged += ChangedPosition;
        mediaElement.PositionChanged += OnPositionChanged;
        _ = Moved();
    }
    #region Methods
    public void SeekTo(TimeSpan position)
    {
        mediaElement.Pause();
        mediaElement.SeekTo(position);
        mediaElement.Play();
    }
    public void Play()
    {
        mediaElement.Play();
        BtnPLay.Source = "pause.png";
    }
    public void Pause()
    {
        mediaElement.Pause();
        BtnPLay.Source = "play.png";
    }
    public void Stop()
    {
        mediaElement.Stop();
        BtnPLay.Source = "pause.png";
    }

    /// <summary>
    /// A method that converts <see cref="TimeSpan"/> into a usable <see cref="string"/> for displaying position in <see cref="MediaElement"/>
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private static string TimeConverter(TimeSpan time)
    {
        var interval = new TimeSpan(time.Hours, time.Minutes, time.Seconds);
        return interval.ToString();
    }
    #endregion

    #region Events

#nullable enable
    private void MediaElement_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == MediaElement.DurationProperty.PropertyName)
        {
            PositionSlider.Maximum = mediaElement.Duration.TotalSeconds;
        }
    }
    private void OnPositionChanged(object? sender, MediaPositionChangedEventArgs e)
    {
        PositionSlider.Value = e.Position.TotalSeconds;
    }
    private void Slider_DragCompleted(object? sender, EventArgs e)
    {
        ArgumentNullException.ThrowIfNull(sender);

        var newValue = ((Slider)sender).Value;
        mediaElement.SeekTo(TimeSpan.FromSeconds(newValue));
        mediaElement.Play();
    }

#nullable disable
    private void Slider_DragStarted(object sender, EventArgs e)
    {
        mediaElement.Pause();
    }
    private void ChangedPosition(object sender, EventArgs e)
    {
        var playDuration = TimeConverter(mediaElement.Duration);
        var position = TimeConverter(mediaElement.Position);
        PlayPosition = $"{position}/{playDuration}";
        OnPropertyChanged(nameof(PlayPosition));
    }
    #endregion

    #region Buttons
    private void BtnRewind_Clicked(object sender, EventArgs e)
    {
        var time = mediaElement.Position - TimeSpan.FromSeconds(15);
        mediaElement.Pause();
        mediaElement.SeekTo(time);
        mediaElement.Play();
    }

    private void BtnForward_Clicked(object sender, EventArgs e)
    {
        var time = mediaElement.Position + TimeSpan.FromSeconds(15);
        mediaElement.Pause();
        mediaElement.SeekTo(time);
        mediaElement.Play();
    }
    private void BtnPlay_Clicked(object sender, EventArgs e)
    {
        if (mediaElement.CurrentState is MediaElementState.Stopped or
       MediaElementState.Paused)
        {
            mediaElement.Play();
            BtnPLay.Source = "pause.png";
        }
        else if (mediaElement.CurrentState == MediaElementState.Playing)
        {
            mediaElement.Pause();
            BtnPLay.Source = "play.png";
        }
    }

    private void BtnFullScreen_Clicked(object sender, EventArgs e)
    {
        SetVideoSize();
    }
    private void OnMuteClicked(object sender, EventArgs e)
    {
        mediaElement.ShouldMute = !mediaElement.ShouldMute;
        ImageButtonMute.Source = mediaElement.ShouldMute ? (ImageSource)"mute.png" : (ImageSource)"muted.png";
        OnPropertyChanged(nameof(ImageButtonMute.Source));
    }
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (!PlaybackControls)
        {
            return;
        }
        _ = Moved();
    }
    private void Button_Pressed(object sender, EventArgs e)
    {
        if (!PlaybackControls)
        { 
            return;
        }
        MenuIsVisible = !MenuIsVisible;
        OnPropertyChanged(nameof(MenuIsVisible));
    }
    private void AspectButton(object sender, EventArgs e)
    {
        mediaElement.Aspect = mediaElement.Aspect == Aspect.AspectFit ? Aspect.AspectFill : Aspect.AspectFit;
    }
    private void MediaControl_Loaded(object sender, EventArgs e)
    {
        if (mediaElement.CurrentState is MediaElementState.Stopped or
       MediaElementState.Paused)
        {
            BtnPLay.Source = "play.png";
        }
        else if (mediaElement.CurrentState == MediaElementState.Playing)
        {
            BtnPLay.Source = "pause.png";
        }
    }

    [RelayCommand]
    public void Tapped(string url)
    {
#if ANDROID
        mediaElement.Stop();
#endif
        mediaElement.Source = new Uri(url);
        MenuIsVisible = false;
#if ANDROID
        mediaElement.Play();
#endif
        OnPropertyChanged(nameof(MenuIsVisible));
    }

    private void PointerGestureRecognizer_PointerMoved(object sender, PointerEventArgs e)
    {
        MenuIsVisible = false;
        OnPropertyChanged(nameof(MenuIsVisible));
    }
    #endregion

    #region Full Screen Functions
    private void TapGestureRecognizer_DoubleTapped(object sender, TappedEventArgs e)
    {
#if WINDOWS
        if (!PlaybackControls)
        {
            return;
        }
        SetVideoSize();
#endif
    }
    private static void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
    {
        if (e.Direction == SwipeDirection.Up)
        {
            Controls.FullScreen();
        }
        if (e.Direction == SwipeDirection.Down)
        {
            Controls.RestoreScreen();
        }
    }
    private static void SetVideoSize()
    {
        if (s_fullScreen)
        {
            Controls.RestoreScreen();
            s_fullScreen = false;
        }
        else
        {
            Controls.FullScreen();
            s_fullScreen = true;
        }
    }

    private async Task Moved()
    {
        if (!FullScreen && PlaybackControls)
        {
            FullScreen = true;
            OnPropertyChanged(nameof(FullScreen));
            await Task.Delay(7000);
            FullScreen = false;
            MenuIsVisible = false;
            OnPropertyChanged(nameof(MenuIsVisible));
            OnPropertyChanged(nameof(FullScreen));
        }
    }
    #endregion
}
