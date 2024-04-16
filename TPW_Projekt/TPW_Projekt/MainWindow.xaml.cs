using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Collections.Generic;



using Data_Layer;
using Logic_Layer;
using Presentation_Layer.Model;

namespace TPW_Projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Ball_Service ballService;
        private DispatcherTimer timer;
        
        //Slownik (Dane->Interfejs)
        
        private Dictionary<Ball, Ellipse> ballToEllipseMap = new Dictionary<Ball, Ellipse>();


        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;

            ballService = new Ball_Service(MainCanvas.ActualWidth, MainCanvas.ActualHeight);
            ballToEllipseMap = new Dictionary<Ball, Ellipse>();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(15);
            timer.Tick += Timer_Tick;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            double canvasWidth = MainCanvas.ActualWidth;
            double canvasHeight = MainCanvas.ActualHeight;
            ballService = new Ball_Service(canvasWidth, canvasHeight);
        }


        //----------------------------------------------------------------------------------


        private void Timer_Tick(object sender, EventArgs e)
        {
            ballService.UpdateBallPositions(1);  // Aktualizuje pozycje kulek

            foreach (var ball in ballService.balls)
            {
                var ellipse = ballToEllipseMap[ball];
                Canvas.SetLeft(ellipse, ball.X - ball.Radius);
                Canvas.SetTop(ellipse, ball.Y - ball.Radius);
            }
        }

        private void GenerateBalls_Click(object sender, RoutedEventArgs e)
        {
            int Ball_MAX = 16;

            if (int.TryParse(NumberOfBallsTextBox.Text, out int numberOfBalls))
            {
                if (numberOfBalls > 0 && numberOfBalls <= Ball_MAX) // Sprawdza, czy liczba kulek jest w odpowiednim zakresie
                {
                    ballService.ClearBalls();  // Usuwa istniejące kulki
                    ballToEllipseMap.Clear();
                    MainCanvas.Children.Clear();

                    for (int i = 0; i < numberOfBalls; i++)
                    {
                        var ball = ballService.CreateBall();
                        var ellipse = new Ellipse
                        {
                            Width = ball.Radius * 2,
                            Height = ball.Radius * 2,
                            Fill = new SolidColorBrush(ball.Color)
                        };
                        Canvas.SetLeft(ellipse, ball.X - ball.Radius);
                        Canvas.SetTop(ellipse, ball.Y - ball.Radius);

                        MainCanvas.Children.Add(ellipse);
                        ballToEllipseMap[ball] = ellipse;
                    }
                }
                else if (numberOfBalls > Ball_MAX)
                {
                    MessageBox.Show("Nie można dodać więcej niż " + Ball_MAX + " kulek.");
                }
                else
                {
                    MessageBox.Show("Proszę wpisać prawidłową liczbę kulek (więcej niż 0).");
                }
            }
            else
            {
                MessageBox.Show("Proszę wprowadzić liczbę.");
            }
        }



        private void StartMovingBalls_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (timer == null)
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(20);
                timer.Tick += Timer_Tick;
            }

            if (!timer.IsEnabled)
            {
                timer.Start(); // Uruchamia ruch kulek
                button.Content = "Zatrzymaj ruch";
            }
            else
            {
                timer.Stop(); // Zatrzymuje ruch kulek
                button.Content = "Rozpocznij ruch";
            }
        }



    }
}