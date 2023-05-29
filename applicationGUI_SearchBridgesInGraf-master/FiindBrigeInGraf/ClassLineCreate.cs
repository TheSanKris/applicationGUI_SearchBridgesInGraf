using FiindBrigeInGraf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FiindBrigeInGraf
{
    internal class ClassLineCreate
    {
        public void CreateRebro(double[] coordsLine, Canvas canvas, List<MyPair> rebroPairs, int a, int b)
        {
            bool can = true;
            if (a != b) 
            {

                foreach (var pair in rebroPairs)
                {
                    if ((pair.first == a & pair.second ==b) |(pair.first == b & pair.second == a)) 
                    {
                        can = false;
                    }
                }
                if (can)
                {
                    MyPair rebro = new MyPair();
                    Line line = new Line();
                    line.X1 = coordsLine[1];
                    line.X2 = coordsLine[3];
                    line.Y1 = coordsLine[0];
                    line.Y2 = coordsLine[2];
                    line.Stroke = Brushes.Black;

                  /* TextBlock textBlock = new TextBlock();
                   textBlock.Text = $"{a} {b}";
                   canvas.Children.Add(textBlock);
                   Canvas.SetLeft(textBlock, (line.X1 + line.X2) / 2);
                   Canvas.SetTop(textBlock, (line.Y1 + line.Y2) / 2);*/
                    line.StrokeThickness = 2;
                    rebro.Insert(a, b);
                    rebroPairs.Add(rebro);
                    line.DataContext = rebro;
                    canvas.Children.Add(line);


                }

            }           
        }
        public void CreateRebro(Canvas canvas, List<MyPair> rebroPairs, Button aButton, Button bButton)
        {
            bool can = true;
            if (aButton.Content != bButton.Content)
            {

                foreach (var pair in rebroPairs)
                {
                    if ((pair.first == Convert.ToInt32(aButton.Content) & pair.second == Convert.ToInt32(bButton.Content)) | (pair.first == Convert.ToInt32(bButton.Content) & pair.second == Convert.ToInt32(aButton.Content)))
                    {
                        can = false;
                    }
                }
                if (can)
                {
                    MyPair rebro = new MyPair();
                    Line line = new Line();
                    
                    line.X1 = (double)aButton.GetValue(Canvas.LeftProperty) + aButton.Width / 2;
                    line.X2 = (double)bButton.GetValue(Canvas.LeftProperty) + bButton.Width / 2;
                    line.Y1 = (double)aButton.GetValue(Canvas.TopProperty) + aButton.Height / 2;
                    line.Y2 = (double)bButton.GetValue(Canvas.TopProperty) + bButton.Height / 2;
                    line.Stroke = Brushes.Black;
                    /* TextBlock textBlock = new TextBlock();
                     textBlock.Text = $"{a} {b}";
                     canvas.Children.Add(textBlock);
                     Canvas.SetLeft(textBlock, (line.X1 + line.X2) / 2);
                     Canvas.SetTop(textBlock, (line.Y1 + line.Y2) / 2);*/
                    line.StrokeThickness = 2;
                    rebro.Insert(Convert.ToInt32(aButton.Content), Convert.ToInt32(bButton.Content));
                    rebroPairs.Add(rebro);
                    line.DataContext = rebro;
                    canvas.Children.Add(line);

                }

            }
        }

        public void RecursCreateRebroRand(List<Button> buttonList, int count, Canvas canvas, List<MyPair> rebroPairs)
        {
            ClassLineCreate create = new ClassLineCreate();
            if (count == 0)
            {
                return;
            }
            else
            {
                Random aRand = new Random();
                Random bRand = new Random();
                int a = aRand.Next(0, count);
                int b = bRand.Next(0, count);

                while (true)
                {
                    if (b != a)
                    {
                        break;
                    }
                    else if (b ==0 & a == 0)
                    {
                        break;
                    }
                    else
                    {
                        b = bRand.Next(0, count);
                    }
                }
                create.CreateRebro(canvas, rebroPairs, buttonList[a], buttonList[b]);
                count--;
                RecursCreateRebroRand(buttonList, count, canvas, rebroPairs);   

            }
        }

    }
        
    
}
