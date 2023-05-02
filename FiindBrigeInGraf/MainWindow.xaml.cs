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
using FiindBrigeInGraf;

namespace FiindBrigeInGraf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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

        private void NextStep_Click(object sender, RoutedEventArgs e)
        {
            if (da == 0)
                graf.Init(rebroPairs);
            da++;
            graf.NextStep();
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
    }
}

/* 
 * Создвать линии за кнопкой
 * Короче, линии можно создавать от границы кнопки, главное учитыаеть расположение кнопок относительно друг друга
 * 
 * 
 * Собирать инфу для Сани +
 * 
 * */