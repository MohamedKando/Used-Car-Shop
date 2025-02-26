using Microsoft.ML;

namespace Used_Car_Shop.Ai_Model
{
    public class ImageValidationService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;

        public ImageValidationService()
        {
            _mlContext = new MLContext();
            var modelPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/AiModelsWeights/MLModel.mlnet");
            _model = _mlContext.Model.Load(modelPath, out _);
        }

        public string PredictCarImage(byte[] imageBytes)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<MLModel.ModelInput, MLModel.ModelOutput>(_model);

            var input = new MLModel.ModelInput() { ImageSource = imageBytes };
            var result = predictionEngine.Predict(input);

            float carConfidence = result.Score[1];

            if (carConfidence >= 0.95f) 
            {
                return "Car";
            }

            return "NotCar";
        }
    }
}
