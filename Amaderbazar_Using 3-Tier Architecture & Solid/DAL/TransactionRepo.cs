using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace DAL
{
    public class TransactionRepo: ITransactionRepository<Transaction, int>
    {
        AmaderbazarEntities db;

        public TransactionRepo(AmaderbazarEntities db)
        {
            this.db = db;
        }

        public void Add(Transaction e)
        {
            this.db.Transactions.Add(e);
            this.db.SaveChanges();
        }

        public void Delete(int id)
        {
            var transaction = db.Transactions.FirstOrDefault(c => c.TID == id);
            db.Transactions.Remove(transaction);
            db.SaveChanges();
        }

        public void Edit(Transaction e)
        {
            var t = db.Transactions.FirstOrDefault(en => en.TID == e.TID);
            db.Entry(t).CurrentValues.SetValues(e);
            db.SaveChanges();
        }

        public List<Transaction> Get()
        {
            return db.Transactions.ToList();
        }

        public Transaction Get(int id)
        {
            return db.Transactions.FirstOrDefault(c => c.TID == id);
        }

        
    }
}
