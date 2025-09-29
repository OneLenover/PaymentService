using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentService.DataAccess.Postgres.Entities;

namespace PaymentService.DataAccess.Postgres
{
    public interface IAppDbContext
    {
        DbSet<Payment> Payments { get; } 

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
