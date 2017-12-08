using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSE.Service.Audit.DataAccess
{
    public class AuditDataAccess : IAuditDataAccess
    {
        private readonly AuditDb _auditDb;

        public AuditDataAccess(DbContext auditDb)
        {
            _auditDb = (AuditDb)auditDb;
        }

        public long Insert(Audit auditEntity)
        {
            _auditDb.Audits.Add(auditEntity);
            _auditDb.SaveChanges();
            return auditEntity.Id;
        }
    }
}