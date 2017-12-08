namespace FXA.DPSE.Service.Audit.Business
{
    public interface IAuditBusiness
    {
        AuditBusinessResult Audit(AuditLog auditRequest);
    }
}
