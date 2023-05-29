using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
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

namespace FiindBrigeInGraf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public enum Flags
    {
        NEXT,
        NO_BRIDGE,
        IS_BRIDGE,
        END
    }
    public partial class MainWindow : Window
    {
       

        bool rButtonCan = false;
        bool rebroCan = false;
        bool RB_getCoords = false;

        int countRButton =0;
        int a = 0;
        int b = 0;
        List<MyPair> rebroPairs = new List<MyPair>();

        double[] coordsLine = new double[4];

        Graf graf = new Graf();
        int da = 0;

        ClassButtonCreate buttonCreate = new ClassButtonCreate();
        ClassLineCreate lineCreate = new ClassLineCreate();
        
        // просто создай кнопку для пошагового поиска и другую для автоматического

        public MainWindow()
        {
            InitializeComponent();            
            
        }

        

        void CreateRButtonOnCanvas() 
        {
            Button rButton = new Button
            {
                Height = 30,
                Width = 30,
                Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb( 50, 0, 245, 215))
                
            };
            rButton.Click += RoundButton_Click;
            ++countRButton;
            rButton.Content = countRButton;
            rButton.Template = (ControlTemplate)Application.Current.Resources["ellipseButton"];
            System.Windows.Point p = Mouse.GetPosition(canvas);
            buttonCreate.CreateRButton(p,  canvas, rButton);

            
            
        }
        
        private void obj_Click(object sender, RoutedEventArgs e)
        {
            rButtonCan = true;
            rebroCan = false;
        }
        private void reb_Click(object sender, RoutedEventArgs e)
        {
            rButtonCan = false;
            rebroCan = true;
        }
        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (rButtonCan & canvas.Children.OfType<Button>().ToList().Count()<50)
            {
                CreateRButtonOnCanvas();
            }
        }
        void RoundButton_Click(object sender, RoutedEventArgs e)
        {
            var rButton = sender as Button;
            if (rebroCan)
            {
                if (!RB_getCoords)
                {
                    coordsLine[0] = (double)rButton.GetValue(Canvas.TopProperty) + rButton.Height / 2;
                    coordsLine[1] = (double)rButton.GetValue(Canvas.LeftProperty) + rButton.Width / 2;
                    a = Convert.ToInt32(rButton.Content.ToString());
                    RB_getCoords = true;
                }
                else
                {
                    coordsLine[2] = (double)rButton.GetValue(Canvas.TopProperty) + rButton.Height / 2;
                    coordsLine[3] = (double)rButton.GetValue(Canvas.LeftProperty) + rButton.Width / 2;
                    b = Convert.ToInt32(rButton.Content.ToString());
                    RB_getCoords = false;
                    if (a != b)
                    {
                        lineCreate.CreateRebro(coordsLine, canvas, rebroPairs, a, b);
                    }
                    

                }
            }
       
        }

        private void incr_Click(object sender, RoutedEventArgs e)
        {
            int x = Convert.ToInt32(chetchik.Text.ToString());
            if (x < 50)
            {
                x++;
            }
            
            chetchik.Text = Convert.ToString(x);
        }

        private void decr_Click(object sender, RoutedEventArgs e)
        {
            int x = Convert.ToInt32(chetchik.Text.ToString());
            if (x > 0) 
            {
                x--;
            }
            chetchik.Text = Convert.ToString(x);

        }

        private void buttonRandomCreate_Click(object sender, RoutedEventArgs e)
        {
            int count = Convert.ToInt32(chetchik.Text);
            countRButton = 0;
            canvas.Children.Clear();
            rebroPairs.Clear();
            Recurs(count);
            List<Button> buttonList = canvas.Children.OfType<Button>().ToList();

            lineCreate.RecursCreateRebroRand(buttonList, buttonList.Count, canvas, rebroPairs);
            foreach (var b  in canvas.Children.OfType<Button>().ToList()) 
            {
                info.Text = info.Text + b.Content.ToString();
            }




        }
        void Recurs(int i)
        {
            Button rButton = new Button { Width = 30, Height = 30,
                Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(50, 0, 245, 215))
            };
            rButton.Template = (ControlTemplate)Application.Current.Resources["ellipseButton"];
            rButton.Click += RoundButton_Click;
            countRButton++;
            rButton.Content = countRButton;
            if (i == 0)
            {
                
                countRButton--;
                return;
                
            }
            else
            {
                buttonCreate.CreateRButton(canvas, rButton);
                --i;
                Recurs(i);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            info.Text = "";
            rButtonCan = false;
            rebroCan = false;
            RB_getCoords = false;

            countRButton = 0;
            a = 0;
            b = 0;
            rebroPairs.Clear();
            double[] coordsLine = new double[4];
        }


        private void chetchik_KeyUp(object sender, KeyEventArgs e)
        {
            string text = chetchik.Text.ToString();
            int x = 0;
            if (text != "" & text !="-")
            {
                try
                {
                    x = Convert.ToInt32(text);
                }
                finally
                {
                    x = Convert.ToInt32(text);
                    if (x > 15)
                    {
                        x = 15;
                        chetchik.Text = x.ToString();
                    }
                    else if (x < 0)
                    {
                        x = 0;
                        chetchik.Text = x.ToString();
                    }
                }
            }
            

            
        }

        public void ChangeColor(Flags flag, Canvas canvas)
        {
            MyPair myPair = graf.m_mpThisRebro;
            int a = myPair.first;
            int b = myPair.second;
            List <int[]> ints = new List <int[]>();
            switch (flag)
            {
                case Flags.NEXT:
                    
                    foreach (Button button in canvas.Children.OfType<Button>().ToList())
                    {
                        if (Convert.ToInt32(button.Content) == a)
                        {
                            button.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(100, 128, 128, 128));
                        }
                        else if (Convert.ToInt32(button.Content) == b)
                        {
                            button.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(100, 128, 128, 128));

                        }
                    }
                    foreach(Line line in canvas.Children.OfType<Line>().ToList())
                    {
                        if(line.DataContext == myPair)
                        {
                            line.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(100, 128, 128, 128));
                            break;
                        }
                    }
                    int[] ab = { a, b };
                    ints.Add(ab);
                    break;
                case Flags.NO_BRIDGE:
                    foreach (int[] i in ints)
                    {
                        foreach (Button button in canvas.Children.OfType<Button>().ToList())
                        {
                            if (Convert.ToInt32(button.Content) == i[0])
                            {
                                button.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(100, 0, 130, 255));
                            }
                            else if (Convert.ToInt32(button.Content) == i[b])
                            {
                                button.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(100, 0, 130, 255));

                            }
                        }
                        foreach (Line line in canvas.Children.OfType<Line>().ToList())
                        {
                            MyPair pair = new MyPair();
                            pair.Insert(i[0], i[1]);
                            if (line.DataContext == pair)
                            {
                                line.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(100, 0, 130, 255));
                                break;
                            }
                        }
                    }
                    
                    break;
                case Flags.IS_BRIDGE:
                    foreach (Line line in canvas.Children.OfType<Line>().ToList())
                    {
                        if (line.DataContext == myPair)
                        {
                            line.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(100, 255, 0, 0));
                            break;
                        }
                    }
                    break;
            }
        }

        private void BackStep_Click(object sender, RoutedEventArgs e)
        {

        }
        private void NextStep_Click(object sender, RoutedEventArgs e)
        {
            if (da == 0)
                graf.Init(rebroPairs);
            da++;
            Flags flag = graf.NextStep();
            ChangeColor(flag, canvas);
        }
        private void buttonA_Click(object sender, RoutedEventArgs e)
        {

        }

        private void incrA_Click(object sender, RoutedEventArgs e)
        {
            int x = Convert.ToInt32(chetchik.Text.ToString());
            if (x < 50000)
            {
                x+=100;
            }

            chetchik.Text = Convert.ToString(x);
        }

        private void decrA_Click(object sender, RoutedEventArgs e)
        {
            int x = Convert.ToInt32(chetchik.Text.ToString());
            if (x > 0)
            {
                x-=100;
            }
            chetchik.Text = Convert.ToString(x);

        }

        private void chetchikA_KeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}

/* 
 * Создвать линии за кнопкой
 * 
 * Сделать кнопки для шагов алгоритма и для автоматического прохода
 * Сделать функции для изменения цвета ребра и вершины относительно флага
 * Механизм для шага назад
 * Сдлеть так, чтобы все вершины были соединены ребрами
 * */