using Sneat.MVC.DAL;
using Sneat.MVC.Models.DTO.Bank;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Sneat.MVC.Services
{
    public class BankService
    {
        private readonly SneatContext _dbContext;

        public BankService(SneatContext dbContext = null)
        {
            if(dbContext == null)
            {
                dbContext = new SneatContext();
            }
            _dbContext = dbContext;
        }

        public async Task<List<BankVietQROutputModel>> GetListBanks()
        {
            try
            {
                var banks = await _dbContext.Banks
                    .Select(x =>  new BankVietQROutputModel
                    {
                        ID = x.ID,
                        Name = x.Name,
                        ShortName = x.ShortName,
                        Bin = x.Bin,
                        Logo = x.Logo,
                        Code = x.Code,
                        SwiftCode = x.SwiftCode
                    })
                    .ToListAsync();

                return banks;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new List<BankVietQROutputModel>();
            }
        }
    }
}