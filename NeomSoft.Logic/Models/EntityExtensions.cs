using NeomSoft.DB;
using NeomSoft.Logic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NeomSoft.Logic.Models
{
    public static class EntityExtensions
    {
        public static IQueryable<FinAccountViewModel> ToFinAccountsViewModel(this IQueryable<Account> list)
        {
            return list.Select(e => new FinAccountViewModel
            {
                AccountID = e.AccountID,
                Code = e.Code,
                NameAr = e.NameAr,
                Note = e.Note,
                ParentAccountID = e.ParentAccountID,
                ParentAccountName = e.ParentAccountID.HasValue ? e.Account2.NameAr : "",
                AddedBy = e.AddedBy,
                AddedDate = e.AddedDate,
                LastUpdatedBy = e.LastUpdatedBy,
                LastUpdatedDate = e.LastUpdatedDate,
                Currencies = e.AccountCurrencies.Where(c=> !c.IsDeleted).Select(c=> new CurrencyViewModel {
                    CurrencyID = c.CurrencyID,
                    Code = c.Currency.Code,
                    NameAr = c.Currency.NameAr,
                    NameEn = c.Currency.NameEn,
                }).ToList()
            }) ;
        }

        public static IQueryable<CurrencyViewModel> ToCurrencyViewModel(this IQueryable<Currency> list)
        {
            return list.Select(e => new CurrencyViewModel
            {
                CurrencyID = e.CurrencyID,
                Code = e.Code,
                NameAr = e.NameAr,
                NameEn = e.NameEn,
                
            });
        }
    }
}