namespace FujiXerox.RangerClient.Enums
{
    public enum RangerTransportState
    {
        TransportUnknownState = -1,
        TransportShutDown = 0,
        TransportStartingUp = 1,
        TransportChangeOptions = 2,
        TransportEnablingOptions = 3,
        TransportReadyToFeed = 4,
        TransportFeeding = 5,
        TransportExceptionInProgress = 6,
        TransportSnuttingDown = 7
    }
}