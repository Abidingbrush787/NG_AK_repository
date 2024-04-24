using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;
using TPW_Projekt.Helpers;
using Data_Layer;
using Logic_Layer;
using System.Windows.Media.Media3D;
using System.ComponentModel;
using System.Windows;

namespace TPW_Projekt.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private BallService _ballService;
        private DispatcherTimer _timer;

        public ObservableCollection<BallViewModel> Balls { get; }

        public ICommand GenerateBallsCommand { get; }
        public ICommand StartMovingBallsCommand { get; }

        public MainViewModel()
        {
            _ballService = new BallService(0, 0, new BallRepository()); // Załóżmy, że wymiary będą ustalone później
            Balls = new ObservableCollection<BallViewModel>();

            GenerateBallsCommand = new RelayCommand(_ => GenerateBalls());
            StartMovingBallsCommand = new RelayCommand(_ => ToggleBallsMovement());

            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(15) };
            _timer.Tick += (sender, e) => UpdateBallPositions();
            System.Diagnostics.Debug.WriteLine($"Timer Tick");


        }

        private int? _numberOfBalls;
        public int? NumberOfBalls
        {
            get => _numberOfBalls;
            set
            {
                if (value < 0 || value > 10)
                {
                    MessageBox.Show("Liczba kul musi być między 0 a 16.", "Nieprawidłowa wartość", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_numberOfBalls != value)
                {
                    _numberOfBalls = value;
                    OnPropertyChanged(nameof(NumberOfBalls));
                }
            }
        }


        private void GenerateBalls()
        {
            Balls.Clear();
            System.Diagnostics.Debug.WriteLine($"Clear Balls");
            for (int i = 0; i < _numberOfBalls; i++) // 'numberOfBalls' powinno być parametrem metody lub właściwością ViewModel
            {
                Ball newBall = _ballService.CreateBall();
                Balls.Add(new BallViewModel(newBall)); // Tworzy nowy ViewModel dla kuli i dodaje do kolekcji
                System.Diagnostics.Debug.WriteLine($"Created Ball : {Balls[i].X} , {Balls[i].Y}, {Balls[i].Color}");

            }
        }

        private void ToggleBallsMovement()
        {
            if (_timer.IsEnabled)
            {
                _timer.Stop(); // Sprawdź, czy to jest wywoływane
                System.Diagnostics.Debug.WriteLine("Timer stopped");  // Dodaj logowanie do debugowania
            }
            else
            {
                _timer.Start(); // Sprawdź, czy to jest wywoływane
                System.Diagnostics.Debug.WriteLine("Timer started");  // Dodaj logowanie do debugowania
            }
        }


        private void UpdateBallPositions()
        {
            _ballService.UpdateBallPositions(0.015); // Czy to się aktualizuje?

            foreach (BallViewModel ballVM in Balls)
            {
                System.Diagnostics.Debug.WriteLine($"Updating ball position: X = {ballVM.X}, Y = {ballVM.Y}");  // Dodaj logowanie do debugowania
                ballVM.UpdatePosition(1);
            }
        }

        private double _canvasWidth;
        public double CanvasWidth
        {
            get => _canvasWidth;
            set
            {
                if (_canvasWidth != value)
                {
                    _canvasWidth = value;
                    OnPropertyChanged(nameof(CanvasWidth));
                }
            }
        }

        private double _canvasHeight;
        public double CanvasHeight
        {
            get => _canvasHeight;
            set
            {
                if (_canvasHeight != value)
                {
                    _canvasHeight = value;
                    OnPropertyChanged(nameof(CanvasHeight));
                }
            }
        }

        public void SetCanvasSize(double width, double height)
        {
            _ballService.SetCanvasSize(width, height);
            CanvasWidth = width;
            CanvasHeight = height;
        }
    }
}
