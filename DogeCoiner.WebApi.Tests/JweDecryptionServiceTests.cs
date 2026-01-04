using DogeCoiner.Data.Auth;
using FluentAssertions;
using System.Text.Json;
using Xunit.Abstractions;

namespace DogeCoiner.WebApi.Tests
{
    public class JweDecryptionServiceTests : IClassFixture<JweDecryptionServiceTestFixture>
    {
        private readonly JweDecryptionServiceTestFixture _fixture;
        private readonly ITestOutputHelper _outputHelper;

        public JweDecryptionServiceTests(JweDecryptionServiceTestFixture fixture, ITestOutputHelper outputHelper)
        {
            _fixture = fixture;
            _outputHelper = outputHelper;

            _fixture.Configure(_outputHelper);
        }

        [Theory]
        [InlineData("TestData\\jwe-token.txt", "TestData\\jwe-token-decrypted.json")]
        [InlineData("TestData\\jwe-token2.txt", "TestData\\jwe-token2-decrypted.json")]
        public void DecryptAndValidate(string tokenFilename, string expectedFilename)
        {
            var token = File.ReadAllText(tokenFilename).Trim();
            var decrypted = File.ReadAllText(expectedFilename).Trim();

            token.Count(c => c == '.').Should().Be(4); // 4 dots
            token.Split('.').Length.Should().Be(5); // 5 segments

            var res = _fixture.JweService.DecryptAndValidate(token);

            res.Should().NotBeNull();

            var user = res.ToAuthenticatedUser();

            var expected = JsonSerializer.Deserialize<AuthUserJwt>(decrypted, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            expected.Should().NotBeNull();

            // Validate all user properties from the JWT payload
            user.Name.Should()
                .NotBeNullOrWhiteSpace()
                .And.Be(expected.Name);

            user.Email.Should()
                .NotBeNullOrWhiteSpace()
                .And.Be(expected.Email);

            user.Picture.Should()
                .NotBeNullOrWhiteSpace()
                .And.Be(expected.Picture);
            
            user.FirstName.Should()
                .NotBeNullOrWhiteSpace()
                .And.Be(expected.FirstName);
            
            user.LastName.Should()
                .NotBeNullOrWhiteSpace()
                .And.Be(expected.LastName);
            
            user.Provider.Should()
                .NotBeNullOrWhiteSpace()
                .And.Be(expected.Provider);
            
            user.ProviderSub.Should()
                .NotBeNullOrWhiteSpace()
                .And.Be(expected.ProviderSub);

            // Validate timestamps
            user.IssuedAtDateUtc.Should().Be(DateTimeOffset.FromUnixTimeSeconds(expected.Iat));
            user.ExpirationDateUtc.Should().Be(DateTimeOffset.FromUnixTimeSeconds(expected.Exp));

            // Validate authentication status
            user.IsAuthenticated.Should().BeTrue();
            user.IsExpired.Should().BeFalse(); // Token expires in 2027
        }
    }
}
