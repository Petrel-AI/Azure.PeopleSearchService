using MG.Utils.Abstract.NonNullableObjects;
using Xunit;

namespace Petrel.Utils.Azure.PSS.Tests
{
    public class PssEndpointTest
    {
        [Theory]
        [InlineData("https://pss.com/api/Pss/")]
        [InlineData("https://pss.com/api/Pss")]
        public void ToString_BaseUrlAndApiPartCases_Ok(string baseUrlPart)
        {
            var target = new PssEndpoint(
                new NonNullableString(baseUrlPart), "GetPerson");

            Assert.Equal("https://pss.com/api/Pss/GetPerson", target.ToString());
        }

        [Theory]
        [InlineData("https://pss.com")]
        [InlineData("https://pss.com/")]
        public void ToString_BaseUrlCases_Ok(string baseUrlPart)
        {
            var target = new PssEndpoint(
                new NonNullableString(baseUrlPart), "GetPerson");

            Assert.Equal("https://pss.com/GetPerson", target.ToString());
        }

        [Theory]
        [InlineData("GetPerson")]
        [InlineData("/GetPerson")]
        public void ToString_EndpointCases_Ok(string endpointPart)
        {
            var target = new PssEndpoint(
                new NonNullableString("https://pss.com/api/Pss"), endpointPart);

            Assert.Equal("https://pss.com/api/Pss/GetPerson", target.ToString());
        }

        [Theory]
        [InlineData("GetPerson/")]
        [InlineData("/GetPerson/")]
        public void ToString_EndpointWithSlashCases_Ok(string endpointPart)
        {
            var target = new PssEndpoint(
                new NonNullableString("https://pss.com/api/Pss"), endpointPart);

            Assert.Equal("https://pss.com/api/Pss/GetPerson/", target.ToString());
        }

        [Theory]
        [InlineData("GetPerson/?q=123")]
        [InlineData("/GetPerson/?q=123")]
        public void ToString_EndpointWithSlashCases_WithQuery_Ok(string endpointPart)
        {
            var target = new PssEndpoint(
                new NonNullableString("https://pss.com/api/Pss"), endpointPart);

            Assert.Equal("https://pss.com/api/Pss/GetPerson/?q=123", target.ToString());
        }

        [Theory]
        [InlineData("GetPerson?q=123")]
        [InlineData("/GetPerson?q=123")]
        public void ToString_Endpoint_WithQuery_Ok(string endpointPart)
        {
            var target = new PssEndpoint(
                new NonNullableString("https://pss.com/api/Pss"), endpointPart);

            Assert.Equal("https://pss.com/api/Pss/GetPerson?q=123", target.ToString());
        }
    }
}
