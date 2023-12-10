using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> CompanyList = _unitOfWork.Company.GetAll().ToList();
            return View(CompanyList);
        }
       
        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                //create
                return View(new Company());
            }
            else
            {
               //update
                Company CompanyFromDb = _unitOfWork.Company.Get(u => u.Id == id);

                return View(CompanyFromDb);
            }

        }

        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        {
            if (ModelState.IsValid)
            {
                if (CompanyObj.Id == null)
                {
                    _unitOfWork.Company.Add(CompanyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(CompanyObj);
                }
                _unitOfWork.Save();
                TempData["success"] = "Company Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(CompanyObj);
            }
        }

        public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
                return NotFound();

            Company CompanyFromDb = _unitOfWork.Company.Get(u => u.Id == id);

            if (CompanyFromDb == null)
                NotFound();

            return View(CompanyFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Company? CompanyFromDb = _unitOfWork.Company.Get(u => u.Id == id);
            if (CompanyFromDb == null)
                return NotFound();

            _unitOfWork.Company.Remove(CompanyFromDb);
            _unitOfWork.Save();
            TempData["success"] = "Company Deleted Successfully";
            return RedirectToAction("Index");
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> CompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new {data= CompanyList });
        }

        [HttpDelete]
        public IActionResult DeleteApi(int? id)
        {
            Company? CompanyToDelete = _unitOfWork.Company.Get(u => u.Id == id);
            if (CompanyToDelete == null)
                return Json(new {success = false, message = "Error  while Deleting"});

            _unitOfWork.Company.Remove(CompanyToDelete);
            _unitOfWork.Save();

            return Json(new { success=true, message="Deleted Successfully"});

        }
        #endregion
    }
}
