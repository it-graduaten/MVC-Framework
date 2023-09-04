using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _09_01_InterimKantoor.Models;

namespace _09_01_InterimKantoor
{
    public interface IUnitOfWork
    {
        IRepository<Job> JobRepository { get; }
        IRepository<Klant> KlantRepository { get; }
        IRepository<KlantJob> KlantJobRepository { get; }
        public void SaveChanges();
    }
}
