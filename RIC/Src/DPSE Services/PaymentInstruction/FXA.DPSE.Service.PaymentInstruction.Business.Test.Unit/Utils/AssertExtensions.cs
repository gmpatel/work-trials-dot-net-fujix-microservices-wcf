using System.Collections.Generic;
using System.Linq;
using FXA.DPSE.Service.PaymentInstruction.Business.Entities;

namespace FXA.DPSE.Service.PaymentInstruction.Business.Test.Unit.Utils
{
    public static class AssertExtensions
    {
        public static void AssignedChequesTrackingIdentifier(List<string> trackingIdentifiers, 
            TrackingIdentifierResult trackingIdentifierResult)
        {
            var assertionResult = !trackingIdentifierResult.ForCheques
                .Where((t, i) => t.TrackingId != trackingIdentifiers[i + 2])
                .Any();

            Xunit.Assert.True(assertionResult);
        }
    }
}
