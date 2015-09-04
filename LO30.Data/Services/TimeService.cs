using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LO30.Data.Services
{
  public class TimeService
  {
    public DateTime ConvertYYYYMMDDIntoDateTime(int yyyymmdd)
    {
      if (yyyymmdd.ToString().Length != 8)
      {
        throw new ArgumentOutOfRangeException("yyyymmdd", yyyymmdd, "Must be length of 8");
      }

      var year = Convert.ToInt32(yyyymmdd.ToString().Substring(0, 4));
      var month = Convert.ToInt32(yyyymmdd.ToString().Substring(4, 2));
      var day = Convert.ToInt32(yyyymmdd.ToString().Substring(6, 2));
      var result = new DateTime(year, month, day);

      return result;
    }

    public int ConvertDateTimeIntoYYYYMMDD(DateTime? toConvert, bool ifNullReturnMax)
    {
      int result = -1;

      if (toConvert == null)
      {
        if (ifNullReturnMax)
        {
          result = GetMaxYYYYMMDD();
        }
        else
        {
          result = GetMinYYYYMMDD();
        }
      }
      else
      {
        result = (toConvert.Value.Year * 10000) + (toConvert.Value.Month * 100) + toConvert.Value.Day;
      }

      return result;
    }

    public int GetMinYYYYMMDD()
    {
      int result = 12345678;
      return result;
    }

    public int GetMaxYYYYMMDD()
    {
      int result = 87654321;
      return result;
    }
  }
}
