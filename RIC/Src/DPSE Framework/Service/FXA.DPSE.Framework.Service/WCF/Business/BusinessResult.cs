using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;

namespace FXA.DPSE.Framework.Service.WCF.Business
{
    public class BusinessResult
    {
        private DpseBusinessException _businessExceptionInstance;
        private readonly IList<DpseBusinessWarning> _businessWarningList = new List<DpseBusinessWarning>();

        public bool HasWarning
        {
            get { return _businessWarningList.Count > 0; }
        }

        public bool HasException
        {
            get { return _businessExceptionInstance != null; }
        }

        public IEnumerable<DpseBusinessWarning> BusinessWarnings
        {
            get { return new ReadOnlyCollection<DpseBusinessWarning>(_businessWarningList); }
        }

        public void AddBusinessException(DpseBusinessException exception)
        {
            _businessExceptionInstance = new DpseBusinessException(exception.ExceptionType, exception.ErrorCode, exception.Message, exception.Details);
        }

        public void AddBusinessWarnings(IEnumerable<DpseBusinessWarning> warnings)
        {
            warnings.ToList().ForEach(warning => _businessWarningList.Add(warning));
        }

        public DpseBusinessException BusinessException
        {
            get { return _businessExceptionInstance; }
        }
    }
}