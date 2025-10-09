using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Interfaces;
using FaysalFunds.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FaysalFunds.Persistence.Repositories
{
    public class TpinTransactionRepository : BaseRepository<TransactionPin>, ITransactionPinRepository
    {
        private readonly DbSet<TransactionPin> _transactionPinSet;

        public TpinTransactionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _transactionPinSet = dbContext.Set<TransactionPin>();
        }
        public async Task<bool> GenerateTransactionPin(TransactionPin model)
        {
            var existing = await _transactionPinSet
                .FirstOrDefaultAsync(p => p.ACCOUNTOPENING_ID == model.ACCOUNTOPENING_ID);

            if (existing != null)
            {
                // T-PIN already exists; do not insert again
                return false;
            }

            model.CREATEDON = DateTime.UtcNow;
            await AddAsync(model);
            return await SaveChangesAsync() > 0;
        }

        public async Task<TransactionPin> GetTpinByAccountOpeningId(long accountOpeningId)
        {
            var result = await _transactionPinSet
                .Where(x => x.ACCOUNTOPENING_ID == accountOpeningId)
                .Select(x => new TransactionPin
                {
                    ID = x.ID,
                    ACCOUNTOPENING_ID = x.ACCOUNTOPENING_ID,
                    PINHASH = x.PINHASH,
                   FAILEDATTEMPTS = x.FAILEDATTEMPTS,
                   ISLOCKED =x.ISLOCKED,
                   LOCKEDON =x.LOCKEDON
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<bool> UpdateTransactionPin(TransactionPin model)
        {

            AttachAndMarkModified(model,
              x => x.PINHASH,
              x => x.UPDATEDON,
              x => x.FAILEDATTEMPTS,
              x => x.ISLOCKED,
              x => x.LOCKEDON
          );
            return await SaveChangesAsync() > 0;
        }


        private void AttachAndMarkModified<T>(T model, params Expression<Func<T, object>>[] properties) where T : class, new()
        {
            var contextEntry = _context.Entry(model);
            var keyProperty = _context.Model.FindEntityType(typeof(T))?
                .FindPrimaryKey()?.Properties.FirstOrDefault();

            if (keyProperty == null)
                throw new InvalidOperationException($"No primary key defined for {typeof(T).Name}");

            var keyValue = typeof(T).GetProperty(keyProperty.Name)?.GetValue(model);
            var stub = new T();
            typeof(T).GetProperty(keyProperty.Name)?.SetValue(stub, keyValue);

            _context.Attach(stub);

            foreach (var propExpr in properties)
            {
                var propertyName = ((MemberExpression)(propExpr.Body is UnaryExpression unary ? unary.Operand : propExpr.Body)).Member.Name;
                var value = typeof(T).GetProperty(propertyName)?.GetValue(model);
                typeof(T).GetProperty(propertyName)?.SetValue(stub, value);
                _context.Entry(stub).Property(propertyName).IsModified = true;
            }
        }

    }
}

