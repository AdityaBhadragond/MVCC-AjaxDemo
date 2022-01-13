using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var db = new Model1();
            List<Employee> employees = db.Employees.ToList();

            return View(employees);
        }
        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var db = new Model1();
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View("Index");
        }
        [HttpGet]
        public JsonResult GetEmployeeById(int id)
        {
            var db = new Model1();
            var result = db.Employees.Where(x => x.ID == id).SingleOrDefault();

            string values = String.Empty;

            values = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        
            return Json(values, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Update(Employee employee)
        {
            var db = new Model1();
            try
            {
                var entity = db.Employees.Where(x => x.ID == employee.ID).FirstOrDefault();
                if(entity!=null)
                {
                    entity.Name = employee.Name;  
                    entity.Address = employee.Address;
                    entity.Phone = employee.Phone;

                    db.SaveChanges();
                    
                    return Json(new
                    {
                        status = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    message = ex 
                });
            }
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var db = new Model1();
            try
            {
                var entity = db.Employees.Where(x => x.ID == id).FirstOrDefault();
                if (entity != null)
                {
                    db.Employees.Remove(entity);

                    db.SaveChanges();

                    return Json(new
                    {
                        status = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    message = ex
                });
            }
        }
    }
}