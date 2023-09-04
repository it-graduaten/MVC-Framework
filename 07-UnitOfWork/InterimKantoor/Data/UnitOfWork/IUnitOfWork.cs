using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterimKantoor.Models;

namespace InterimKantoor
{
    public interface IUnitOfWork
    {
        IRepository<Job> JobRepository { get; }
        IRepository<Klant> KlantRepository { get; }
        IRepository<KlantJob> KlantJobRepository { get; }
        public void SaveChanges();
    }
}
