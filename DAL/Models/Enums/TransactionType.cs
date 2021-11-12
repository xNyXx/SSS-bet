using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models.Enums
{
    public enum TransactionType
    {
        //Пополнение
        Replenishment = 1,
        //Снятие 
        Withdrawal = 2,
    }
}
