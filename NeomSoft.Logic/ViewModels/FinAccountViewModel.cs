using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NeomSoft.Logic.ViewModels
{
    public class FinAccountViewModel
    {
        [Key]
        public int AccountID { get; set; }
        [Required(ErrorMessage = "الحقل {0} مطلوب")]
        [Display(Name = "رقم الحساب")]
        public string Code { get; set; }
        [Required(ErrorMessage = "الحقل {0} مطلوب")]
        [Display(Name = "اسم الحساب")]        
        public string NameAr { get; set; }
        [Display(Name = "اسم الأب")]
        public int? ParentAccountID { get; set; }
        [Display(Name = "اسم الأب")]
        public string ParentAccountName { get; set; }
        [Display(Name = "ملاحظة")]
        public string Note { get; set; }
        [Display(Name = "تاريخ الإضافة")]
        public DateTime AddedDate { get; set; }
        [Display(Name = "أضيف بواسطة")]
        public string AddedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

        [Display(Name = "عملة الحساب")]
        [Required(ErrorMessage = "الحقل {0} مطلوب")]
        public List<int> SelectedCurrencies { get; set; }
        [Display(Name = "عملة الحساب")]
        public List<CurrencyViewModel> Currencies { get; set; }
    }

    public class CurrencyViewModel
    {
        [Key]
        public int CurrencyID { get; set; }
        public string Code { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }

    public class AccountCurrencyViewModel
    {
        [Key]
        public int AccountCurrencyID { get; set; }
        public int AccountID { get; set; }
        public int CurrencyID { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime AddedDate { get; set; }
        public string AddedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
    }

}