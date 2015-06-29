using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EnterpriseFramework.Core
{
    public abstract class BaseModel
    {
        [Key, HiddenInput]
        public virtual int Id { get; set; }

    }
}
