namespace Used_Car_Shop
{
    public class FileSettings
    {
        public const string GovFilePath = "wwwroot/JsonFiles/Test.json";
        public const string EngineFilePath = "wwwroot/JsonFiles/engine_types.json";
        public const string ModelsFilePath = "wwwroot/JsonFiles/models.json";
        public const string BrandsFilePath = "wwwroot/JsonFiles/brands.json";
        public const string TransmissionsFilePath = "wwwroot/JsonFiles/transmissions.json";
        public const string AllowedExtentions = ".jpg,.jpeg,.png,.mp4";
        public const int MaxAllowedSizeInMB = 1;
        public const int MaxAllowedSizeInBytes = MaxAllowedSizeInMB * 1024 * 1024;
    }
}
