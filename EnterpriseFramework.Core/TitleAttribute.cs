using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseFramework.Core
{
    public class TitleAttribute : Attribute
    {
        public string Title { get; set; }
        public TitleAttribute (string title)
        {
            Title = title;
        }
    }
}
