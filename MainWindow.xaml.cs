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
using System.Timers;



namespace Projekt
{


    public enum Movements {Left, Right, Up, Down, None};

 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        const int MAX_SNAKE_PART = 70;
        private double[] snakePositionLeft = new double[MAX_SNAKE_PART];
        private double[] snakePositionTop  = new double[MAX_SNAKE_PART];
        private double MainStackWidth;
        private double MainStackHeight;
        private int snakeWidth = 10;
        private int snakeHeight = 10;
        private int snakeCount = 0;
        private int score = 0;
        private Movements _actualMovement;
        private int elapsedTime = 0;

        private double foodLeft;
        private double foodTop;


        private int m_nStart = 0;
        public Timer oTimer;
        public Timer oFoodTimer;
        public Timer oAnimationTimer;
    

        public Movements Movement
        {
            get
            {
                return _actualMovement;
            }
            set
            {
                _actualMovement = value;
            }
        }

      
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
           
            if (e.Key == Key.Left && _actualMovement != Movements.Right)
            {
                _actualMovement = Movements.Left;
            }

            if (e.Key == Key.Right && _actualMovement != Movements.Left)
            {
                _actualMovement = Movements.Right;
            }

            if (e.Key == Key.Up && _actualMovement != Movements.Down)
            {
                _actualMovement = Movements.Up;
            }

            if (e.Key == Key.Down && _actualMovement != Movements.Up)
            {
                _actualMovement = Movements.Down;
            }


        }

        private void createNewSnakePart(double posLeft, double posTop)
        {
            Ellipse newEllipse = new Ellipse();
            newEllipse.Fill = Brushes.Orange; 
            newEllipse.Stroke = Brushes.Black; 
            newEllipse.Name = "SnakePart" + snakeCount;
            snakePositionTop[snakeCount] = posTop;
            snakePositionLeft[snakeCount] = posLeft;


            snakeCount++;
            newEllipse.Width = snakeWidth;
            newEllipse.Height = snakeHeight;

            newEllipse.Visibility = Visibility.Visible;
   
            mainCanvas.Children.Add(newEllipse);

           
            Dispatcher.Invoke(new Action(() => { Canvas.SetLeft(newEllipse, posLeft); ; }));
            Dispatcher.Invoke(new Action(() => { Canvas.SetTop(newEllipse, posTop); ; }));
             
        }
        Random FoodRandom = new Random();


        private void createFood(int width, int height)
        {          
         
            foodLeft = FoodRandom.Next(0, Convert.ToInt32(MainStackWidth / snakeWidth) ) * 10;
            foodTop = FoodRandom.Next(0, Convert.ToInt32(MainStackHeight / snakeHeight) ) * 10;

             
                   

            Dispatcher.Invoke(new Action(() => { Canvas.SetLeft(Food, foodLeft); ; }));
            Dispatcher.Invoke(new Action(() => { Canvas.SetTop(Food, foodTop); ; }));

        }


        public void StartTimer()
        {
            m_nStart = Environment.TickCount;
            oTimer = new Timer();
            oTimer.Elapsed += new ElapsedEventHandler(OnTimeEvent);
            oTimer.Interval = 600;
            oTimer.Enabled = true;
        }

        public void StartTimeTimer()
        {
            oFoodTimer = new Timer();
            oFoodTimer.Elapsed += new ElapsedEventHandler(OnTimeEventShowFood);
            oFoodTimer.Interval = 1000;
            oFoodTimer.Enabled = true;
        }

       
        Random rnd = new Random();

        private void OnTimeEvent(object oSource, ElapsedEventArgs oElapsedEventArgs)
        {
            bool Error = false;
            if (snakePositionLeft[0] == 0 && _actualMovement == Movements.Left) Error = true;
            if (snakePositionTop[0] == 0 && _actualMovement == Movements.Up) Error = true;
            if (snakePositionLeft[0] == MainStackWidth - snakeWidth && _actualMovement == Movements.Right) Error = true;
            if (snakePositionTop[0] == MainStackHeight - snakeHeight && _actualMovement == Movements.Down) Error = true;

            for (int i = 1; i < snakeCount; i++)
            {
                if (snakePositionLeft[i] == snakePositionLeft[0]
                    && snakePositionTop[i] == snakePositionTop[0])
                    Error = true;
            }

                if (Error == true)
            {
                oTimer.Stop();
                elapsedTime = 0;
                oFoodTimer.Stop();
                score = 0;

                for (int i = 1; i <= snakeCount; i++)
                {
                   
                    Dispatcher.Invoke(new Action(() => { mainCanvas.Children.Remove(mainCanvas.Children[1]); ; }));
                   
                }
                snakeCount = 0;
                Dispatcher.Invoke(new Action(() => { start_Button.Visibility = Visibility.Visible; ; }));
                Dispatcher.Invoke(new Action(() => { Food.Visibility = Visibility.Hidden; ; }));
                Dispatcher.Invoke(new Action(() => { start_Button.Content = "Play Again"; ; }));
                Dispatcher.Invoke(new Action(() => { higscore_Button.Visibility = Visibility.Visible; ; ; }));
                

            }
            else
            {
                double oldSnakePositionLeft = snakePositionLeft[0];
                double oldSnakePositionTop = snakePositionTop[0];
                for (int i = 0; i < snakeCount; i++)
                {
                    if (i == snakeCount - 1)
                    {
                        snakePositionLeft[i + 1] = oldSnakePositionLeft;
                        snakePositionTop[i + 1] = oldSnakePositionTop;
                    }

                    if (i == 0)
                    {

                        int horizontalShift = _actualMovement == Movements.Right ? snakeWidth : _actualMovement == Movements.Left ? -snakeWidth : 0;
                        int verticalShift = _actualMovement == Movements.Up ? -snakeHeight : _actualMovement == Movements.Down ? snakeHeight : 0;
                        Dispatcher.Invoke(new Action(() => { Canvas.SetLeft(mainCanvas.Children[i + 1], snakePositionLeft[i] + horizontalShift); ; }));
                        Dispatcher.Invoke(new Action(() => { Canvas.SetTop(mainCanvas.Children[i + 1], snakePositionTop[i] + verticalShift); ; }));


                        snakePositionLeft[i] += horizontalShift;
                        snakePositionTop[i] += verticalShift;
                        
                    }
                    else
                    {
                        double tempSnakePosLeft = snakePositionLeft[i];
                        double tempSnakePosTop = snakePositionTop[i];
                        

                        Dispatcher.Invoke(new Action(() => { Canvas.SetLeft(mainCanvas.Children[i + 1], oldSnakePositionLeft); ; }));
                        Dispatcher.Invoke(new Action(() => { Canvas.SetTop(mainCanvas.Children[i + 1], oldSnakePositionTop); ; }));

                        snakePositionLeft[i] = oldSnakePositionLeft;
                        snakePositionTop[i] = oldSnakePositionTop;

                        oldSnakePositionLeft = tempSnakePosLeft;
                        oldSnakePositionTop = tempSnakePosTop;
                       
                        
                    }
                }


                if (foodLeft == snakePositionLeft[0] && foodTop == snakePositionTop[0])
                {
                    Dispatcher.Invoke(new Action(() => { createNewSnakePart(snakePositionLeft[snakeCount], snakePositionTop[snakeCount]); ; ; }));
                    createFood(snakeWidth / 2, snakeHeight / 2);
                    score += 10;
                    Dispatcher.Invoke(new Action(() => { Score.Content = score; ; }));
                    oTimer.Interval = oTimer.Interval - 50;
                    


                }

            }
        }

        private void OnTimeEventShowFood(object oSource, ElapsedEventArgs oElapsedEventArgs)
        {
            Dispatcher.Invoke(new Action(() => { Time.Content = ++elapsedTime ; ; }));

        }



        public void BT_startGry_Click(object sender, RoutedEventArgs e)
        {
            higscore_Button.Visibility = Visibility.Hidden;
            start_Button.Visibility = Visibility.Hidden;
            Food.Visibility = Visibility.Visible;
            MainStackHeight = MainStack.Height;
            MainStackWidth = MainStack.Width;

            double snakePositionLeft = MainStackWidth / 2;
            double snakePositionTop = MainStackHeight / 2;
            createFood(snakeWidth / 2, snakeHeight / 2);
            createNewSnakePart(snakePositionLeft, snakePositionTop);
            
            StartTimer();
            StartTimeTimer();
            createFood(snakeWidth/2, snakeHeight/2);
            _actualMovement = Movements.None;

        }
      

    }
}

