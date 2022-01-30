using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime DoB)
        {
            var diff = DateTime.Now - DoB;
            double age = (diff.TotalDays)/365;
            return (int)age;
        }
    }
}