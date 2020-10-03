using NeomSoft.Logic.Models;
using NeomSoft.Logic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace NeomSoft.API2.Controllers
{
    public class FinAccountController : ApiController
    {
        // GET api/values
        public async Task<List<FinAccountViewModel>> GetAccountsList(string searchText)
        {
            return await FinAccountModel.GetAccountsList(searchText);
        }

        public async Task<FinAccountViewModel> GetAccount(int AccountID)
        {
            return await FinAccountModel.GetAccount(AccountID);
        }
    }
}
