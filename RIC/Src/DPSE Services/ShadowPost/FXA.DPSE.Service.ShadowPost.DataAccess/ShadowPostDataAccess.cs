using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;

namespace FXA.DPSE.Service.ShadowPost.DataAccess
{
    public class ShadowPostDataAccess : IShadowPostDataAccess
    {
        private readonly IAuditProxy _auditProxy;
        private readonly ShadowPostDb _shadowPostDb;

        public ShadowPostDataAccess(DbContext shadowPostDb, IAuditProxy auditProxy)
        {
            _auditProxy = auditProxy;
            _shadowPostDb = (ShadowPostDb) shadowPostDb;
        }

        public bool FindByRequestId(string requestId, string trackingId, string channelType, string sessionId)
        {
            var auditInsertResource = new StringBuilder();
            auditInsertResource.AppendFormat("Server:{0}{1}", _shadowPostDb.Database.Connection.DataSource, Environment.NewLine);
            auditInsertResource.AppendFormat("Database:{0}{1}", _shadowPostDb.Database.Connection.Database, Environment.NewLine);
            auditInsertResource.AppendFormat("Tables:{0}{1}", _shadowPostDb.GetTableName<ShadowPost>(), Environment.NewLine);
            auditInsertResource.AppendFormat("Resources: RequestId {0}", requestId);

            var auditResult = _auditProxy.AuditAsync(trackingId, "DatabaseRead",
                "Read access to shadow post database", auditInsertResource.ToString(), channelType,
                sessionId, Environment.MachineName, "ShadowPost", "ShadowPost");
            if (auditResult.HasException)
            {
                //?
            }

            return _shadowPostDb.ShadowPosts.Any(x => x.RequestGuid.ToString().Equals(requestId));
        }

        public void CreateShadowPost(ShadowPost data, string channelType, string sessionId)
        {
            _shadowPostDb.ShadowPosts.Add(data);
            var shadowPostId = _shadowPostDb.SaveChanges();

            var auditInsertResource = new StringBuilder();
            auditInsertResource.AppendFormat("Server:{0}{1}", _shadowPostDb.Database.Connection.DataSource, Environment.NewLine);
            auditInsertResource.AppendFormat("Database:{0}{1}", _shadowPostDb.Database.Connection.Database, Environment.NewLine);
            auditInsertResource.AppendFormat("Tables:{0}{1}", _shadowPostDb.GetTableName<ShadowPost>(), Environment.NewLine);
            auditInsertResource.AppendFormat("Resources: ShadowPostId {0}", shadowPostId);

            var auditResult = _auditProxy.AuditAsync(data.TrackingId, "DatabaseInsert",
            "Insert access to shadow post database", auditInsertResource.ToString(), channelType,
            sessionId, Environment.MachineName, "ShadowPost", "ShadowPost");
            if (auditResult.HasException)
            {
                //?
            }
        }

        public void Dispose()
        {
            if (_shadowPostDb != null) _shadowPostDb.Dispose();
        }
    }
}

 