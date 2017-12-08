using System;

namespace FXA.DPSE.Service.ShadowPost.DataAccess
{
    public interface IShadowPostDataAccess : IDisposable
    {
        bool FindByRequestId(string requestId, string trackingId, string channelType, string sessionId);
        void CreateShadowPost(ShadowPost shadowPost, string channelType, string sessionId);
    }
}