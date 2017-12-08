using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSE.Service.PaymentInstruction.Common
{
    public enum PaymentInstructionError
    {
        DirectoryAccessError,
        FileAccessError,

        DatabaseAccessError,
        EntityNotFoundError
    }
}
