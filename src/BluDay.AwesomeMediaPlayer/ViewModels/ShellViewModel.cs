﻿namespace BluDay.AwesomeMediaPlayer.ViewModels;

/// <summary>
/// Represents the view model for instances of the <see cref="Shell"/> class.
/// </summary>
public sealed partial class ShellViewModel : ObservableObject
{
    private WindowConfiguration? _defaultConfiguration;

    private AppWindow? _appWindow;

    private AppWindowTitleBar? _appWindowTitleBar;

    private DisplayArea? _displayArea;

    private OverlappedPresenter? _overlappedPresenter;

    private UIElement? _titleBarControl;

    private Window? _shell;

    private string? _iconPath;

    private ContentAlignment? _defaultAlignment;

    /// <summary>
    /// Gets the default configuration instance.
    /// </summary>
    public WindowConfiguration? DefaultConfiguration
    {
        get => _defaultConfiguration;
        set => _defaultConfiguration = value;
    }

    /// <inheritdoc cref="WindowConfiguration.ExtendsContentIntoTitleBar"/>
    public bool? ExtendsContentIntoTitleBar
    {
        get => _appWindowTitleBar?.ExtendsContentIntoTitleBar;
        set
        {
            bool actualValue = value!.Value;

            _appWindowTitleBar!.ExtendsContentIntoTitleBar = actualValue;

            if (actualValue)
            {
                ShowCustomTitleBar();

                return;
            }

            ShowDefaultTitleBar();

            OnPropertyChanged(nameof(ExtendsContentIntoTitleBar));
        }
    }

    /// <inheritdoc cref="WindowConfiguration.IconPath"/>
    public string? IconPath
    {
        get => _iconPath;
        set
        {
            _appWindow!.SetIcon(_iconPath);

            _iconPath = value;

            OnPropertyChanged(nameof(IconPath));
        }
    }

    /// <inheritdoc cref="WindowConfiguration.Title"/>
    public string? Title
    {
        get => _appWindow?.Title;
        set
        {
            _appWindow!.Title = value;

            OnPropertyChanged(nameof(Title));
        }
    }

    /// <inheritdoc cref="WindowConfiguration.DefaultAlignment"/>
    public ContentAlignment? Alignment
    {
        get => _defaultAlignment;
        set
        {
            if (value is null) return;

            RectInt32 workArea = _displayArea!.WorkArea;

            int x = GetXFromAlignment(value.Value, workArea);
            int y = GetYFromAlignment(value.Value, workArea);

            Move(x, y);

            _defaultAlignment = value;

            OnPropertyChanged(nameof(Alignment));
        }
    }

    /// <inheritdoc cref="WindowConfiguration.Size"/>
    public SizeInt32? Size
    {
        get => _appWindow?.Size;
        set
        {
            _appWindow!.Resize(value!.Value);

            OnPropertyChanged(nameof(Size));
        }
    }

    /// <summary>
    /// Shows the custom <see cref="TitleBar"/> control and hides the default Win32 title bar.
    /// </summary>
    private void ShowCustomTitleBar()
    {
        _titleBarControl!.Visibility = Visibility.Visible;

        _appWindowTitleBar!.BackgroundColor
            = _appWindowTitleBar.ButtonBackgroundColor
            = _appWindowTitleBar.ButtonInactiveBackgroundColor
            = Colors.Transparent;

        _shell!.SetTitleBar(_titleBarControl);
    }

    /// <summary>
    /// Shows the defualt Win32 title bar and hides the custom <see cref="TitleBar"/> control.
    /// </summary>
    private void ShowDefaultTitleBar()
    {
        _titleBarControl!.Visibility = Visibility.Collapsed;

        _appWindowTitleBar!.ResetToDefault();

        _appWindow!.SetIcon(_iconPath);
    }

    /// <summary>
    /// Gets the current <see cref="DisplayArea"/> for this shell and updates
    /// <see cref="_displayArea"/> with the new instance.
    /// </summary>
    private void UpdateDisplayArea()
    {
        _displayArea = DisplayArea.GetFromWindowId(_appWindow!.Id, DisplayAreaFallback.Nearest);
    }

    /// <summary>
    /// The translated X value of the given <paramref name="alignment"/> value.
    /// </summary>
    /// <param name="alignment">
    /// The alignment value.
    /// </param>
    /// <param name="workArea">
    /// The targeted "work" area of a display.
    /// </param>
    /// <returns>
    /// The translated X value.
    /// </returns>
    private int GetXFromAlignment(ContentAlignment alignment, RectInt32 workArea)
    {
        SizeInt32 shellSize = _appWindow!.Size;

        switch (alignment)
        {
            case ContentAlignment.TopCenter:
            case ContentAlignment.MiddleCenter:
            case ContentAlignment.BottomCenter:
                return (workArea.Width - shellSize.Width) / 2;
            case ContentAlignment.TopRight:
            case ContentAlignment.MiddleRight:
            case ContentAlignment.BottomRight:
                return workArea.Width - shellSize.Width;
        }

        return 0;
    }

    /// <summary>
    /// The translated X value of the given <paramref name="alignment"/> value.
    /// </summary>
    /// <returns>
    /// The translated Y value.
    /// </returns>
    /// <inheritdoc cref="GetXFromAlignment(ContentAlignment, RectInt32)"/>
    private int GetYFromAlignment(ContentAlignment alignment, RectInt32 workArea)
    {
        SizeInt32 shellSize = _appWindow!.Size;

        switch (alignment)
        {
            case ContentAlignment.MiddleLeft:
            case ContentAlignment.MiddleCenter:
            case ContentAlignment.MiddleRight:
                return (workArea.Height - shellSize.Height) / 2;
            case ContentAlignment.BottomLeft:
            case ContentAlignment.BottomCenter:
            case ContentAlignment.BottomRight:
                return workArea.Height - shellSize.Height;
        }

        return 0;
    }

    /// <summary>
    /// Attempts to activate the shell and bring it into the foreground.
    /// </summary>
    public void Activate()
    {
        _shell!.Activate();
    }

    /// <summary>
    /// Applies default configuration values that was provided at instantiation.
    /// </summary>
    public void ApplyDefaultConfiguration()
    {
        if (_defaultConfiguration is null) return;

        Configure(_defaultConfiguration);
    }

    /// <summary>
    /// Closes the shell instance.
    /// </summary>
    public void Close()
    {
        _shell!.Close();
    }

    /// <summary>
    /// Configures the shell using the provided properties.
    /// </summary>
    /// <param name="config">
    /// The configuration instance.
    /// </param>
    public void Configure(WindowConfiguration config)
    {
        Title                      = config.Title;
        ExtendsContentIntoTitleBar = config.ExtendsContentIntoTitleBar;
        IconPath                   = config.IconPath;
        Size                       = config.Size;
        Alignment                  = config.Alignment;
    }

    /// <summary>
    /// Moves the shell to the provided x and y coordinates on the screen.
    /// </summary>
    /// <param name="x">
    /// The x coordinate.
    /// </param>
    /// <param name="y">
    /// The y coordinate.
    /// </param>
    public void Move(int x, int y)
    {
        _appWindow!.Move(new PointInt32(x, y));
    }

    /// <summary>
    /// Resizes the shell using the provided width and height integer values.
    /// </summary>
    /// <param name="width">
    /// The width of the shell.
    /// </param>
    /// <param name="height">
    /// The height of the shell.
    /// </param>
    public void Resize(int width, int height)
    {
        _appWindow!.Resize(new SizeInt32(width, height));
    }

    /// <summary>
    /// Sets the targeted shell.
    /// </summary>
    /// <param name="shell">
    /// The shell instance.
    /// </param>
    public void SetShell(Shell shell, UIElement? titleBar)
    {
        ArgumentNullException.ThrowIfNull(shell);

        _shell = shell;
        
        _appWindow = shell.AppWindow;

        _appWindowTitleBar = _appWindow.TitleBar;

        _overlappedPresenter = (OverlappedPresenter)_appWindow.Presenter;

        _titleBarControl = titleBar;

        OnPropertyChanged(nameof(ExtendsContentIntoTitleBar));
        OnPropertyChanged(nameof(IconPath));
        OnPropertyChanged(nameof(Title));
        OnPropertyChanged(nameof(Alignment));
        OnPropertyChanged(nameof(Size));

        UpdateDisplayArea();
    }
}