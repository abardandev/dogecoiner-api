using DogeCoiner.WebApi.Extensions;
using DogeCoiner.WebApi.Models;
using FluentAssertions;
using System.Runtime.Serialization;
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
        public async Task DecryptAndValidate(string tokenFilename, string expectedFilename)
        {
            var token = File.ReadAllText(tokenFilename).Trim();
            var decrypted = File.ReadAllText(expectedFilename).Trim();

            token.Count(c => c == '.').Should().Be(4); // 4 dots
            token.Split('.').Length.Should().Be(5); // 5 segments

            var res = await _fixture.JweService.DecryptAndValidateAsync(token);

            res.Should().NotBeNull();

            var user = res.ToAuthenticatedUser();

            var expected = JsonSerializer.Deserialize<AuthUserJwt>(decrypted, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            expected.Should().NotBeNull();

            // Validate all user properties from the JWT payload
            user.Name.Should().Be(expected.Name);
            user.Email.Should().Be(expected.Email);
            user.Picture.Should().Be(expected.Picture);
            user.Sub.Should().Be(expected.Sub);
            user.UserId.Should().Be(expected.UserId);
            user.Jti.Should().Be(expected.Jti);

            // Validate timestamps
            user.IssuedAt.Should().Be(expected.Iat);
            user.Expiration.Should().Be(expected.Exp);
            user.IssuedAtDate.Should().Be(DateTimeOffset.FromUnixTimeSeconds(expected.Iat));
            user.ExpirationDate.Should().Be(DateTimeOffset.FromUnixTimeSeconds(expected.Exp));

            // Validate authentication status
            user.IsAuthenticated.Should().BeTrue();
            user.IsExpired.Should().BeFalse(); // Token expires in 2027
        }
    }
}
