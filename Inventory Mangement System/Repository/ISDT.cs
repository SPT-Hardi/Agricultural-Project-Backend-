using Inventory_Mangement_System.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public class ISDT
    {
        public DateTime GetISDT(DateTime datetime)
        {  
            //TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata");
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime ISTDate = TimeZoneInfo.ConvertTimeFromUtc(datetime.ToUniversalTime(), INDIAN_ZONE);
            return ISTDate;
        }
        public Result DateValue() 
        {
            return new Result()
            {
                Status=Result.ResultStatus.success,
                Message="dateime in different formate",
                Data=new 
                {
                    ISDT=GetISDT(DateTime.Now),
                    DateTime=DateTime.Now,
                    UTC=DateTime.UtcNow,
                },
            };
        }
    }
}
