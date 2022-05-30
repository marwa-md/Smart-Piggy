using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget_Tracker_Library;

namespace Smart_Piggy
{
    internal class ChartCategories
    {
        public  Category Category { get; set; }    
       // public  double Percentage { get; set; }

        public ChartCategories(Category category)
        {
            Category = category;    
        }
        
    }
}
