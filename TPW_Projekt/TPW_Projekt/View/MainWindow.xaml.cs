using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Data_Layer;
using Logic_Layer;
using TPW_Projekt.ViewModel;

namespace TPW_Projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SizeChanged += MainWindow_SizeChanged; // Dodajemy obsługę zdarzenia SizeChanged
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var viewModel = DataContext as MainViewModel;
            if (viewModel != null)
            {
                viewModel.SetCanvasSize(mainGrid.ActualWidth, mainGrid.ActualHeight);
            }
        }

    }

}
       
         
