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
    public class BaseMasterController<M, VM, TUow , TController > : Controller
        where M : class, new()
        where VM : class , new()
        //where TContext : IDbContext
        where TUow : IUow
        where TController : BaseMasterController<M, VM, TUow, TController> 

    {
        protected TUow db;


        protected Func<TUow, IDataRepository<M>> GetRepository;
        protected Func<ActionResult> DefaultRedirectTo;
        protected string Title { get; set; }

        public string IndexViewName { get; set; }
        public BaseMasterController(TUow _uow)
        {
            db = _uow;
            DefaultRedirectTo =() => RedirectToAction<TController>(c => c.Index());
            if (string.IsNullOrEmpty( Title )== null )
            {
                Title = typeof(VM).Name;
            }
            if (string.IsNullOrEmpty(IndexViewName))
            {
                IndexViewName = "IndexId";
            }
            //GetRepository = a => a.GetRepositoryFromFactory<M>() ?? a.GetStandardRepository<M>();
        }

        protected ActionResult RedirectToAction<Tcontroller>(Expression<Action<Tcontroller>> action)
            where Tcontroller : Controller
        {
            return ControllerExtensions.RedirectToAction(this, action);
        }

        public virtual ActionResult Index()
        {
            var result = Map(GetRepository(db).GetAll());
            BindForm();
            return View(IndexViewName, result);
        }

        public virtual void BindForm()
        {
            ViewBag.Title = Title;
            ViewBag.IndexViewName = IndexViewName;

        }

        IQueryable Map(IQueryable<M> Source)
        {
            if (typeof(M) == typeof( VM))
            {
                return Source;
            }
            else
            {
                return Source.Project().To<VM>();
            }
        }

        VM Map(M Source)
        {
            if (typeof(M) == typeof(VM))
            {
                return (VM)((object)Source);
            }
            else
            {
                return Mapper.Map<VM>(Source);
            }
        }
        M Map(VM Source)
        {
            if (typeof(M) == typeof(VM))
            {
                return (M)((object)Source);
            }
            else
            {
                return Mapper.Map<M>(Source);
            }
        }

        public ActionResult New()
        {
            BindForm();

            return View(new VM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(VM model)
        {
            BindForm();

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var newModel = Map(model);

            GetRepository(db).Add(newModel);

            db.SaveChanges();


            return DefaultRedirectTo()
                .WithSuccess("Issue created!");

        }
        public string  CurrentUserName { get; set; }

        [Log("Viewed issue {id}")]
        public ActionResult View(int id)
        {
            BindForm();

            var model = GetRepository(db).Find(id);
            var viewModel = Map(model);

            if (viewModel == null)
            {
                return RedirectToAction<TController>(c => c.Index())
                    //DefaultRedirectTo()
                    .WithError("Unable to find the issue.  Maybe it was deleted?");
            }

            return View(viewModel);
        }


        public ActionResult Edit(int id)
        {
            BindForm();

            var modelSource = GetRepository(db).Find(id);
            var model = Map(modelSource);
            if (model == null)
            {

                return DefaultRedirectTo()
                    .WithError("Unable to find the issue.  Maybe it was deleted?");
            }
            return View(model);
        }


        // POST: /models/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(VM model)
        {
            BindForm();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newModel = Map(model);
            GetRepository(db).Update(newModel);



            db.SaveChanges();



            return DefaultRedirectTo()
                .WithSuccess("Issue updated!");

        }

        public ActionResult Delete(int? id)
        {
            BindForm();

            var issue = GetRepository(db).Find(id ?? 0);

            if (issue == null)
            {
                return DefaultRedirectTo()
                    .WithError("Unable to find the issue.  Maybe it was deleted?");
            }

            GetRepository(db).Delete(issue);

            db.SaveChanges();

            return DefaultRedirectTo()
                .WithSuccess("Issue deleted!");
        }




    }
}
