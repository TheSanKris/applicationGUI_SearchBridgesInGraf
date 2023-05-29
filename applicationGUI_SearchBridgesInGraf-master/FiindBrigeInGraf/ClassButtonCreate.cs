using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace FiindBrigeInGraf
{
    internal class ClassButtonCreate
    {
        public void CreateRButton(System.Windows.Point point, Canvas canvas, Button rButton)
        {
            
            canvas.Children.Add(rButton);
            if (point.X + rButton.Width / 2 > canvas.ActualWidth & point.Y + rButton.Height / 2 > canvas.ActualHeight)
            {
                Canvas.SetLeft(rButton, canvas.ActualWidth - rButton.Width);
                Canvas.SetTop(rButton, canvas.ActualHeight - rButton.Height);
            }
            else if (point.X - rButton.Width / 2 < 0 & point.Y - rButton.Height / 2 < 0)
            {
                Canvas.SetLeft(rButton, 0);
                Canvas.SetTop(rButton, 0);
            }
            else if (point.X + rButton.Width / 2 > canvas.ActualWidth & point.Y - rButton.Height / 2 < 0)
            {
                Canvas.SetLeft(rButton, canvas.ActualWidth - rButton.Width);
                Canvas.SetTop(rButton, 0);
            }
            else if (point.X - rButton.Width / 2 < 0 & point.Y + rButton.Height / 2 > canvas.ActualHeight)
            {
                Canvas.SetLeft(rButton, 0);
                Canvas.SetTop(rButton, canvas.ActualHeight - rButton.Height);
            }
            else if (point.X + rButton.Width / 2 > canvas.ActualWidth)
            {
                Canvas.SetLeft(rButton, canvas.ActualWidth - rButton.Width);
                Canvas.SetTop(rButton, point.Y - rButton.Height / 2);
            }
            else if(point.Y + rButton.Height / 2 > canvas.ActualHeight) 
            {
                Canvas.SetLeft(rButton, point.X - rButton.Width / 2);
                Canvas.SetTop(rButton, canvas.ActualHeight - rButton.Height);
            }
            else if (point.X - rButton.Width / 2 < 0)
            {
                Canvas.SetLeft(rButton, 0);
                Canvas.SetTop(rButton, point.Y - rButton.Height / 2);
            }
            else if (point.Y - rButton.Height / 2 < 0)
            {
                Canvas.SetLeft(rButton, point.X - rButton.Width / 2);
                Canvas.SetTop(rButton, 0);
            }
     
            else
            {
                Canvas.SetLeft(rButton, point.X - rButton.Width / 2);
                Canvas.SetTop(rButton, point.Y - rButton.Height / 2);
            }

            



        }

        public void CreateRButton(Canvas canvas, Button rButton)
        {
            
            canvas.Children.Add(rButton);
            Random top = new Random();
            Random left = new Random();
            int t = top.Next(0, (int)canvas.ActualWidth);
            int l = left.Next(0, (int)canvas.ActualHeight);
            Canvas.SetLeft(rButton, top.Next(0, (int)canvas.ActualWidth));
            Canvas.SetTop(rButton, left.Next(0, (int)canvas.ActualHeight));
            if (t + rButton.Width / 2 > canvas.ActualWidth & l + rButton.Height / 2 > canvas.ActualHeight)
            {
                Canvas.SetLeft(rButton, canvas.ActualWidth - rButton.Width);
                Canvas.SetTop(rButton, canvas.ActualHeight - rButton.Height);
            }
            else if (t - rButton.Width / 2 < 0 & l - rButton.Height / 2 < 0)
            {
                Canvas.SetLeft(rButton, 0);
                Canvas.SetTop(rButton, 0);
            }
            else if (t + rButton.Width / 2 > canvas.ActualWidth & l - rButton.Height / 2 < 0)
            {
                Canvas.SetLeft(rButton, canvas.ActualWidth - rButton.Width);
                Canvas.SetTop(rButton, 0);
            }
            else if (t - rButton.Width / 2 < 0 & l + rButton.Height / 2 > canvas.ActualHeight)
            {
                Canvas.SetLeft(rButton, 0);
                Canvas.SetTop(rButton, canvas.ActualHeight - rButton.Height);
            }
            else if (t + rButton.Width / 2 > canvas.ActualWidth)
            {
                Canvas.SetLeft(rButton, canvas.ActualWidth - rButton.Width);
                Canvas.SetTop(rButton, l - rButton.Height / 2);
            }
            else if (l + rButton.Height / 2 > canvas.ActualHeight)
            {
                Canvas.SetLeft(rButton, t - rButton.Width / 2);
                Canvas.SetTop(rButton, canvas.ActualHeight - rButton.Height);
            }
            else if (t - rButton.Width / 2 < 0)
            {
                Canvas.SetLeft(rButton, 0);
                Canvas.SetTop(rButton, l - rButton.Height / 2);
            }
            else if (l - rButton.Height / 2 < 0)
            {
                Canvas.SetLeft(rButton, t - rButton.Width / 2);
                Canvas.SetTop(rButton, 0);
            }

            else
            {
                Canvas.SetLeft(rButton, t - rButton.Width / 2);
                Canvas.SetTop(rButton, l - rButton.Height / 2);
            }
        }

    }
}
