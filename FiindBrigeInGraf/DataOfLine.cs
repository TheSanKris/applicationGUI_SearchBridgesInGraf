using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace FiindBrigeInGraf
{
    internal class DataOfLine
    {
        public MyPair dataContext;
        public System.Windows.Media.Brush color;
        public void CreateData(MyPair dataContext, System.Windows.Media.Brush color)
        {
            this.dataContext = dataContext;
            this.color = color;
        }
    }
}
