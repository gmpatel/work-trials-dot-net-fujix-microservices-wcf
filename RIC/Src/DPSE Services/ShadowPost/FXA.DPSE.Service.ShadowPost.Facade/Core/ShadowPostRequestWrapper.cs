using System.Collections.Generic;
using FXA.DPSE.Service.DTO.ShadowPost;

namespace FXA.DPSE.Service.ShadowPost.Facade.Core
{
    public class ShadowPostRequestWrapper
    {
        public ShadowPostRequest ShadowPostRequest { get; set; }
        public Dictionary<string, string> Header { get; set; }
    }
}