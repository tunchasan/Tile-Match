namespace TileMatch.Scripts.Core.NotifySystem
{
    public static class NotificationTag
    {
        public const string OnGameStart = "OnGameStart";
        public const string OnGameStateChanged = "OnGameStateChanged";
        public const string OnLevelLoaded = "OnLevelLoaded";
        public const string OnLevelPreUnload = "OnLevelPreUnload";
        public const string OnLevelStateProcessed = "OnLevelStateProcessed";
        public const string OnLevelProgressChanged = "OnLevelProgressChanged";
        public const string OnRequestReloadLevel = "OnRequestReloadLevel";
        public const string OnRequestLoadNextLevel = "OnRequestLoadNextLevel";
        public const string OnTileSelect = "OnTileSelect";
        public const string OnTilePlacedToSlot = "OnTilePlacedToSlot";
        public const string OnTilesMatched = "OnTilesMatched";
        public const string OnTilePress = "OnTilePress";
        public const string OnTileRelease = "OnTileRelease";
        public const string OnDrawAction = "OnDrawAction";
        public const string OnActionProcess = "OnActionProcess";
        public const string OnActionProcessComplete = "OnActionProcessComplete";
        public const string OnReverseAction = "OnReverseAction";        
        public const string OnReverseActionCompleted = "OnReverseActionCompleted";
        public const string OnRandomizeBoardAction = "OnRandomizeBoardAction";
        public const string AllSlotsFilled = "AllSlotsFilled";
    }
}