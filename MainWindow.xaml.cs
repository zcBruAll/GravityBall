using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace GravityBall
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double speed = 0;
        private const double G_EARTH = 9.80665;
        private double GRAVITATIONAL_CONSTANT = 6.67430 * Math.Pow(10, -11);

        private double ballRadius = 25;
        private Point ballLocation = new Point();

        private double[] borders = new double[4];

        private double timeSinceFall = 0;

        private DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private Point BordersLeftTop
        {
            get => new Point(borders[0], borders[1]);
            set
            {
                borders[0] = value.X;
                borders[1] = value.Y;
                borders[2] = value.X + mainGrid.ActualWidth - (2 * ballRadius);
                borders[3] = value.Y + mainGrid.ActualHeight - (2 * ballRadius);
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            BordersLeftTop = mainGrid.PointToScreen(new Point(mainGrid.Margin.Left, mainGrid.Margin.Top));

            ballLocation = new Point(ball.Margin.Left + borders[1], ball.Margin.Top + borders[0]);

            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timeSinceFall += 1 * 0.001;
            ballLocation.Y += (speed * timeSinceFall) + (0.5 * G_EARTH * Math.Pow(timeSinceFall, 2));
            if (ballLocation.Y >= borders[3] && speed > 0)
            {
                speed *= -1;
                ballLocation.Y = borders[3];
            }

            if (ballLocation.X < borders[0]) ballLocation.X = borders[0];
            else if (ballLocation.X > borders[2]) ballLocation.X = borders[2];

            Point ballMargin = new Point(ballLocation.X - borders[0], ballLocation.Y - borders[1]);
            ball.Margin = new Thickness(ballMargin.X, ballMargin.Y, 0, 0);

            speed += G_EARTH * timeSinceFall;
        }

        private void Window_LocationChanged(object sender, EventArgs e) => BordersLeftTop = mainGrid.PointToScreen(new Point(mainGrid.Margin.Left, mainGrid.Margin.Top));

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e) => BordersLeftTop = mainGrid.PointToScreen(new Point(mainGrid.Margin.Left, mainGrid.Margin.Top));
    }
}
