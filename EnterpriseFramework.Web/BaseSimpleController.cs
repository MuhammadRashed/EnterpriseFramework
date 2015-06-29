using EnterpriseFramework.Data;
using EnterpriseFramework.Web.Alerts;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using EnterpriseFramework.Web.Filters;
using EnterpriseFramework.Data.UnitOfWork;

namespace EnterpriseFramework.Web
{
    public class BaseMasterController<M, TUow, TController> : BaseMasterController<M, M, TUow, TController>
        where M : class , new ()
        //where TContext : IDbContext
        where TUow : IUow
        where TController : BaseMasterController<M, TUow, TController> 

    {

        public BaseMasterController(TUow _uow)
            : base(_uow)
        {}

    }
}
