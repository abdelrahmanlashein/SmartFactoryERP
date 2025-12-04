using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using SmartFactoryERP.Domain.Interfaces.AI;
using SmartFactoryERP.Domain.Models.AI;
using SmartFactoryERP.Infrastructure.Services.AI.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Infrastructure.Services.AI
{
    public class DemandForecastingService : IForecastingService
    {
        private readonly MLContext _mlContext;

        public DemandForecastingService()
        {
            _mlContext = new MLContext(seed: 0);
        }

        public ForecastResult PredictNextMonth(List<SalesHistoryRecord> history)
        {
            // 1. التحقق من كفاية البيانات (SSA algorithm needs at least 6-10 points usually)
            if (history == null || history.Count < 5)
            {
                // لو الداتا قليلة، نرجع 0 أو المتوسط الحسابي البسيط
                return new ForecastResult { PredictedSales = 0, LowerBound = 0, UpperBound = 0 };
            }

            // 2. تحويل بيانات الـ Domain لبيانات الـ ML
            var mlData = history.Select(x => new ModelInput
            {
                Date = x.Date,
                Sales = x.TotalQuantity
            });

            var dataView = _mlContext.Data.LoadFromEnumerable(mlData);

            // 3. بناء خطة التوقع (Time Series Pipeline)
            var pipeline = _mlContext.Forecasting.ForecastBySsa(
                outputColumnName: nameof(ModelOutput.ForecastedSales),
                inputColumnName: nameof(ModelInput.Sales),
                windowSize: 3,       // تحليل نمط كل 3 فترات
                seriesLength: history.Count,
                trainSize: history.Count,
                horizon: 1,          // توقع الفترة القادمة فقط
                confidenceLevel: 0.95f,
                confidenceLowerBoundColumn: nameof(ModelOutput.LowerBoundSales),
                confidenceUpperBoundColumn: nameof(ModelOutput.UpperBoundSales)
            );

            // 4. تدريب النموذج
            var model = pipeline.Fit(dataView);

            // 5. إنشاء محرك التوقع
            var forecastingEngine = model.CreateTimeSeriesEngine<ModelInput, ModelOutput>(_mlContext);

            // 6. التنبؤ
            var prediction = forecastingEngine.Predict();

            // 7. إرجاع النتيجة بتنسيق الـ Domain
            return new ForecastResult
            {
                PredictedSales = Math.Max(0, prediction.ForecastedSales[0]), // مفيش مبيعات بالسالب
                LowerBound = Math.Max(0, prediction.LowerBoundSales[0]),
                UpperBound = prediction.UpperBoundSales[0]
            };
        }
    }
}
