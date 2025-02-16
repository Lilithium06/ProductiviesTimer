using System.Windows.Input;
using System.Windows.Threading;
using ProductiviesTimer.Core;

namespace ProductiviesTimer.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    private readonly DispatcherTimer _timer;
    private bool _isRunning;
    
    public MainWindowViewModel()
    {
        MoneyUnit = 1;
        TimeUnit = 10;
        _timeWorkedInSeconds = 0;
        TimeWorked = "00:00:00";
        MoneyMadeSoFar = "0.00";
        
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += Timer_Tick;

        // Initialize commands
        StartCommand = new RelayCommand(StartTimer, () => !_isRunning);
        StopCommand = new RelayCommand(StopTimer, () => _isRunning);
    }

    private int _timeUnit;

    public int TimeUnit
    {
        get { return _timeUnit; }
        set
        {
            _timeUnit = value;
            RaisePropertyChangedEvent(nameof(TimeUnit));
        }
    }
    
    private double _moneyUnit;

    public double MoneyUnit
    {
        get { return _moneyUnit; }
        set
        {
            _moneyUnit = value;
            RaisePropertyChangedEvent(nameof(MoneyUnit));
        }
    }
    
    private string _moneyMadeSoFar;

    public string MoneyMadeSoFar
    {
        get { return _moneyMadeSoFar; }
        set
        {
            _moneyMadeSoFar = value;
            RaisePropertyChangedEvent(nameof(MoneyMadeSoFar));
        }
    }
    
    private int _timeWorkedInSeconds;

    private string _timeWorked;
    public string TimeWorked
    {
        get => _timeWorked;
        set
        {
            _timeWorked = value;
            OnPropertyChanged();
        }
    }
    
    public ICommand StartCommand { get; }
    public ICommand StopCommand { get; }
    
    private void StartTimer()
    {
        _isRunning = true;
        _timer.Start();
        CommandManager.InvalidateRequerySuggested();
    }

    private void StopTimer()
    {
        _isRunning = false;
        _timer.Stop();
        CommandManager.InvalidateRequerySuggested();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        _timeWorkedInSeconds += 1;
        TimeWorked = TimeSpan.FromSeconds(_timeWorkedInSeconds).ToString(@"hh\:mm\:ss");

        var moneyPerMinute = MoneyUnit / TimeUnit;
        var number = _timeWorkedInSeconds * (moneyPerMinute / 60);
        MoneyMadeSoFar = (Math.Truncate(number * 100) / 100).ToString("0.00");
    }
}