using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DBu;

namespace BAMUtil
{
    [DatabaseName("AppLogCS")]
    public class AppLog : DBuClass
    {
        //private System.Guid _id = System.Guid.NewGuid();
        //public override System.Guid Id
        //{
        //    get { return _id; }
        //    set { _id = value; }
        //}

        public string Message { get; set; }
        public string ExceptionMessage { get; set; }

        private DateTime _createDate = DateTime.Now;
        public DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        public System.Guid? TransactionId { get; set; }

        public AppLog() { }

        public AppLog(string message)
        {
            this.LoadAppLog(null, message);
        }

        public AppLog(Exception ex)
        {
            this.LoadAppLog(ex, "");
        }

        public AppLog(Exception ex, string message)
        {
            this.LoadAppLog(ex, message);
        }

        public AppLog(Exception ex, string message, System.Guid transactionId)
        {
            this.TransactionId = transactionId;
            this.LoadAppLog(ex, message);
        }

        public void LoadAppLog(Exception ex, string message)
        {
            if (ex != null)
            {
                this.ExceptionMessage = ex.ToString().Length > 2000 ? ex.ToString().Substring(0, 2000) : ex.ToString();
            }

            this.Message = message;
        }
    }

    public static class Log
    {
        public static void L(string message)
        {
            (new AppLog(message)).Save();
        }

        public static void L(Exception ex)
        {
            (new AppLog(ex)).Save();
        }

        public static void L(Exception ex, string message)
        {
            (new AppLog(ex, message)).Save();
        }
    }
}
