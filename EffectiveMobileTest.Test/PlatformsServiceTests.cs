using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace AdService.Tests

{
    public class PlatformsServiceTests
    {
        private readonly PlatformsService _service;

        public PlatformsServiceTests()
        {
            ILogger<PlatformsService> logger = NullLogger<PlatformsService>.Instance;
            _service = new PlatformsService(logger);
        }

        [Fact]
        public void Search_Returns_CorrectPlatforms_ForSimpleLocation()
        {
            string text = @"Яндекс.Директ:/ru
                Газета уральских москвичей:/ru/msk";

            _service.LoadFromText(text);

            var result = _service.Search("/ru/msk");

            Assert.Contains("Яндекс.Директ", result);
            Assert.Contains("Газета уральских москвичей", result);
            Assert.Equal(2, ((ICollection<string>)result).Count);
        }

        [Fact]
        public void Search_Returns_CorrectPlatforms_ForNestedLocations()
        {
            string text = @"Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik
                Крутая реклама:/ru/svrd";

            _service.LoadFromText(text);

            var result1 = _service.Search("/ru/svrd/revda");
            Assert.Contains("Ревдинский рабочий", result1);
            Assert.Contains("Крутая реклама", result1);
            Assert.Equal(2, ((ICollection<string>)result1).Count);

            var result2 = _service.Search("/ru/svrd");
            Assert.Contains("Крутая реклама", result2);
            Assert.Single(result2);
        }

        [Fact]
        public void Search_Returns_OnlyGlobalPlatforms_ForRootLocation()
        {
            string text = @"Яндекс.Директ:/ru
                Крутая реклама:/ru/svrd";

            _service.LoadFromText(text);

            var result = _service.Search("/ru");

            Assert.Contains("Яндекс.Директ", result);
            Assert.Single(result);
        }

        [Fact]
        public void Search_Returns_Empty_ForUnknownLocation()
        {
            string text = @"Яндекс.Директ:/ru";

            _service.LoadFromText(text);

            var result = _service.Search("/unknown/location");

            Assert.Empty(result);
        }

        [Fact]
        public void LoadFromText_Ignores_InvalidLines()
        {
            string text = @"Яндекс.Директ:/ru
                InvalidLine
                :NoName
                NoColonLine";

            var report = _service.LoadFromText(text);

            Assert.Equal(1, report.LinesProcessed);
            Assert.Equal(3, report.LinesIgnored);
        }
    }
}
