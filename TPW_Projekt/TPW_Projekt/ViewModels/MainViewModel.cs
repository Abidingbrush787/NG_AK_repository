using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;
using TPW_Projekt.Helpers;
using Data_Layer;
using Logic_Layer;
using System.Windows.Media.Media3D;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace TPW_Projekt.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private BallService _ballService;
        private bool _isMoving = false;
        public Boundary Boundary { get; set; }

        public ObservableCollection<BallViewModel> Balls { get; }

        public ICommand GenerateBallsCommand { get; }
        public ICommand StartMovingBallsCommand { get; }

        private Logger _logger; // Dodajemy pole Logger

        public MainViewModel()
        {
            Boundary = new Boundary(200, 125, 800, 500, Colors.White);
            _logger = new Logger(); // Inicjalizujemy Logger
            _ballService = new BallService(Boundary, new BallRepository(), _logger);
            Balls = new ObservableCollection<BallViewModel>();

            GenerateBallsCommand = new RelayCommand(_ => GenerateBalls());
            StartMovingBallsCommand = new RelayCommand(_ => ToggleBallsMovement());
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

        private int? _numberOfBalls;
        public int? NumberOfBalls
        {
            get => _numberOfBalls;
            set
            {
                if (value < 0 || value >= 100)
                {
                    MessageBox.Show("Liczba kul musi być między 0 a 100.", "Nieprawidłowa wartość", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_numberOfBalls != value)
                {
                    _numberOfBalls = value;
                    OnPropertyChanged(nameof(NumberOfBalls));
                }
            }
        }

        private void ToggleBallsMovement()
        {
            _isMoving = !_isMoving;

            if (_isMoving)
            {
                System.Diagnostics.Debug.WriteLine("Movement started");
                MoveBallsAsync();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Movement stopped");
            }
        }

        private async void MoveBallsAsync()
        {
            while (_isMoving)
            {
                UpdateBallPositions();
                await Task.Delay(TimeSpan.FromMilliseconds(5));
            }
        }

        private void UpdateBallPositions()
        {
            _ballService.UpdateBallPositions(0.010);

            foreach (BallViewModel ballVM in Balls)
            {
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
