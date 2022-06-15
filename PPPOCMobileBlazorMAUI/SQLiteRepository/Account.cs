using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPPOCMobileBlazorMAUI.SQLiteRepository
{
    public class Account
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public decimal Balance { get; set; }
    }
}
