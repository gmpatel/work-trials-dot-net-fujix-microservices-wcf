using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSE.Service.Audit.DataAccess
{
    public interface IAuditDataAccess
    {
        long Insert(Audit auditEntity);
    }
}