using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using NeomSoft.DB;
using NeomSoft.Logic.ViewModels;

namespace NeomSoft.Logic.Models
{
    public class FinAccountModel
    {


        public static async Task<FinAccountViewModel> GetAccount(int AccountID)
        {
            try
            {
                using (var context = new NeomSoftDBEntities())
                {
                    return await context.Accounts.Where(e => e.AccountID == AccountID && !e.IsDeleted).ToFinAccountsViewModel().FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static async Task<List<FinAccountViewModel>> GetAccountsList(string searchText = null)
        {
            try
            {                
                using (var context = new NeomSoftDBEntities())
                {
                    return await context.Accounts.Where(e => 
                    (searchText == null || e.Code.Contains(searchText) || e.NameAr.Contains(searchText) || e.Note.Contains(searchText))
                    && !e.IsDeleted).ToFinAccountsViewModel().ToListAsync();
                }
            }
            catch (Exception ex)
            {

                return new List<FinAccountViewModel>();
            }
        }

        public static async Task<GenericResponse<FinAccountViewModel>> CreateAccount(FinAccountViewModel model, string AddedBy)
        {
            var resp = new GenericResponse<FinAccountViewModel>();
            try
            {
                using (var context = new NeomSoftDBEntities())
                {
                    var isExist = context.Accounts.Any(e => e.Code == model.Code);
                    if (isExist)
                    {
                        resp.Message = "يوجد حساب بنفس الرقم";
                        return resp;
                    }
                    else
                    {
                        var newAcct = new Account
                        {
                            Code = model.Code,
                            NameAr = model.NameAr,
                            ParentAccountID = model.ParentAccountID,
                            Note = model.Note,
                            AddedBy = AddedBy,
                            AddedDate = DateTime.Now,
                        };

                        if (model.SelectedCurrencies?.Any() == true)
                        {
                            newAcct.AccountCurrencies = model.SelectedCurrencies.Select(e => new AccountCurrency
                            {
                                CurrencyID = e,
                                IsDeleted = false,
                                AddedBy = AddedBy,
                                AddedDate = newAcct.AddedDate,
                            }).ToList();
                        }

                        context.Accounts.Add(newAcct);
                        await context.SaveChangesAsync();

                        model.AccountID = newAcct.AccountID;

                        resp.IsSuccess = true;
                        resp.ReturnedValue = model;
                    }

                }
            }
            catch (Exception ex)
            {
                resp.IsSuccess = false;
            }

            return resp;
        }



        public static async Task<GenericResponse<FinAccountViewModel>> EditAccount(FinAccountViewModel model, string UserName)
        {
            var resp = new GenericResponse<FinAccountViewModel>();

            try
            {
                using (var context = new NeomSoftDBEntities())
                {
                    var account = await context.Accounts.FirstOrDefaultAsync(e => e.AccountID == model.AccountID && !e.IsDeleted);

                    if (account == null)
                    {
                        resp.Message = "الحساب غير موجود";
                        return resp;
                    }

                    var isExist = context.Accounts.Any(e => e.Code == model.Code && e.AccountID != model.AccountID);
                    if (isExist)
                    {
                        resp.Message = "يوجد حساب بنفس الرقم";
                        return resp;
                    }
                    else
                    {
                        account.Code = model.Code;
                        account.NameAr = model.NameAr;
                        account.ParentAccountID = model.ParentAccountID;
                        account.Note = model.Note;
                        account.LastUpdatedBy = UserName;
                        account.LastUpdatedDate = DateTime.Now;

                        
                        var currAcctCurrencies = account.AccountCurrencies.Where(e => !e.IsDeleted).ToList();
                        var currCurrencyIDs = currAcctCurrencies.Select(e => e.CurrencyID).ToList();

                        //delete the deleted currencies
                        var deletedCurrentyIDs = currCurrencyIDs.Where(e => !model.SelectedCurrencies.Contains(e)).ToList();
                        var deletedAcctCurrencies = currAcctCurrencies.Where(e => deletedCurrentyIDs.Contains(e.CurrencyID)).ToList();
                        deletedAcctCurrencies.ForEach(e=> e.IsDeleted = true);

                       // add new currencies to the account 
                        var newCurrencies = model.SelectedCurrencies.Where(e => !currCurrencyIDs.Contains(e))
                            .Select(e=> new AccountCurrency {

                                CurrencyID = e,
                                AccountID = account.AccountID,
                                IsDeleted = false,
                                AddedBy = UserName,
                                AddedDate = account.LastUpdatedDate.Value,
                            }).ToList();

                    


                        context.AccountCurrencies.AddRange(newCurrencies);
                        await context.SaveChangesAsync();


                        resp.IsSuccess = true;
                        resp.ReturnedValue = model;
                    }

                }
            }
            catch (Exception ex)
            {
                resp.IsSuccess = false;
            }

            return resp;
        }


        public static async Task<GenericResponse<int>> DeleteAccount(int AccountID, string By)
        {
            var resp = new GenericResponse<int>();
            try
            {
                using (var context = new NeomSoftDBEntities())
                {
                    var account = await context.Accounts.FirstOrDefaultAsync(e => e.AccountID == AccountID);
                    if (account == null)
                    {
                        resp.Message = "الحساب غير موجود";
                        return resp;
                    }
                    else
                    {
                        var existsActiveChilds = account.Account1.Any(e => !e.IsDeleted);

                        if (existsActiveChilds)
                        {
                            resp.Message = "يوجد أبناء مرتبطة بالحساب الحالي، الرجاء حذف الحسابات الأبناء أولا";
                            return resp;
                        }

                        account.IsDeleted = true;
                        await context.SaveChangesAsync();

                        resp.IsSuccess = true;
                        resp.ReturnedValue = AccountID;
                    }

                }
            }
            catch (Exception ex)
            {
                resp.IsSuccess = false;
            }

            return resp;
        }


        public static async Task<List<CurrencyViewModel>> GetCurrenciesList()
        {
            try
            {
                using (var context = new NeomSoftDBEntities())
                {
                    return await context.Currencies.Where(e => e.IsActive).ToCurrencyViewModel().ToListAsync();
                }
            }
            catch (Exception ex)
            {

                return new List<CurrencyViewModel>();
            }
        }
    }
}