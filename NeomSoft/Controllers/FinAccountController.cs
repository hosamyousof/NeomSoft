using NeomSoft.Logic.Models;
using NeomSoft.Logic.ViewModels;
using NeomSoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NeomSoft.Controllers
{
    public class FinAccountController : Controller
    {
        // GET: FinAccount
        public async Task<ActionResult> Index(string searchText)
        {
            var list = await FinAccountModel.GetAccountsList(searchText);
            return View(list);
        }

        public async Task<ActionResult> AccountData(int id)
        {
            var model = await FinAccountModel.GetAccount(id);

            ViewData["AcctsList"] = await FinAccountModel.GetAccountsList();
            ViewData["CurrenciesList"] = await FinAccountModel.GetCurrenciesList();
            return View(model);
        }

        public async Task<ActionResult> CreateAccount()
        {
            ViewData["AcctsList"] = await FinAccountModel.GetAccountsList();
            ViewData["CurrenciesList"] = await FinAccountModel.GetCurrenciesList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAccount(FinAccountViewModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var resp = await FinAccountModel.CreateAccount(model, User.Identity.Name);

                    if (resp.IsSuccess)
                    {
                        ViewData["SuccessAdd"] = $"تم إضافة الحساب {model.Code} - {model.NameAr} بنجاح";
                        model = null;
                    }
                    else
                    {
                        ModelState.AddModelError("", resp.Message);
                    }
                }

            }
            catch (Exception ex)
            {

            }

            ViewData["CurrenciesList"] = await FinAccountModel.GetCurrenciesList();
            ViewData["AcctsList"] = await FinAccountModel.GetAccountsList();
            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> EditAccount(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = await FinAccountModel.GetAccount(id.Value);
            if (model == null)
            {
                return HttpNotFound();
            }

            model.SelectedCurrencies = model.Currencies.Select(e => e.CurrencyID).ToList();

            ViewData["AcctsList"] = await FinAccountModel.GetAccountsList();
            ViewData["CurrenciesList"] = await FinAccountModel.GetCurrenciesList();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAccount(FinAccountViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resp = await FinAccountModel.EditAccount(model, User.Identity.Name);

                    if (resp.IsSuccess)
                    {
                        ViewData["SuccessAdd"] = $"تم تعديل بيانات الحساب {model.Code} - {model.NameAr} بنجاح";
                    }
                    else
                    {
                        ModelState.AddModelError("", resp.Message);
                    }
                }

            }
            catch (Exception ex)
            {

            }

            ViewData["CurrenciesList"] = await FinAccountModel.GetCurrenciesList();
            ViewData["AcctsList"] = await FinAccountModel.GetAccountsList();
            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> DeleteAccount(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var finAccountViewModel = await FinAccountModel.GetAccount(id.Value);

            if (finAccountViewModel == null)
            {
                return HttpNotFound();
            }

            return View(finAccountViewModel);
        }

        [Authorize]
        [HttpPost, ActionName("DeleteAccount")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var result = await FinAccountModel.DeleteAccount(id, User.Identity.Name);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["ErrorMsg"] = result.Message;
                var finAccountViewModel = await FinAccountModel.GetAccount(id);
                return View(finAccountViewModel);
            }
        }


    }
}