using AmaderBazar.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace AmaderBazar.Models
{
    public class TransactionBind
    {
        public virtual List<List<CartProduct>> Map { get; set; }
        public virtual List<TransactionDetials> Detials { get; set; }
        public virtual List<Transaction> Transactions { get; set; }

        public void MapData()
        {
            int t = 0;
            List<List<CartProduct>> m = new List<List<CartProduct>>();
            List<TransactionDetials> d1 = new List<TransactionDetials>();
            foreach (var i in this.Transactions)
            {
                List<CartProduct> d = new List<CartProduct>();
                d = new JavaScriptSerializer().Deserialize<List<CartProduct>>(i.TDetials);
                m.Add(d);
            }

            this.Map = m;

            t = 0;

            foreach(var i in this.Map)
            {
                TransactionDetials temp = new TransactionDetials();
                int c = 0;
                foreach(var j in i)
                {
                    c++;
                    if(c != i.Count())
                        temp.ItemDetials = temp.ItemDetials + j.PName + " - x" + j.Qty + ", ";
                    else
                        temp.ItemDetials = temp.ItemDetials + j.PName + " - x" + j.Qty;

                }

                d1.Add(temp);
                t++;
            }

            this.Detials = d1;
        }


    }
}