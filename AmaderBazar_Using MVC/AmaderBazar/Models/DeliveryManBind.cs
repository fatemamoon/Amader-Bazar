using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmaderBazar.Models
{
    public class DeliveryManBind
    {
        public virtual TransactionBind Processing { get; set; }
        public virtual TransactionBind Delivered { get; set; }
        public virtual Delivery UserInfo { get; set; }
        public virtual double  IncreasingRate { get; set; }

        public void Rate()
        {
            this.IncreasingRate = ((this.UserInfo.DeliveryCharge - 50) / 50) * 100;
        }
    }
}