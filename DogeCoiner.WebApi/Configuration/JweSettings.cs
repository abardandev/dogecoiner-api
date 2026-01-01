namespace DogeCoiner.WebApi.Configuration;

public class JweSettings
{
    public const string SectionName = "JweSettings";

    /// <summary>
    /// The NextAuth AUTH_SECRET value (can be base64 encoded or plain text)
    /// When UseNextAuthKeyDerivation is true, this will be used with HKDF to derive the encryption key
    /// </summary>
    public string EncryptionKey { get; set; } = string.Empty;

    /// <summary>
    /// Whether to use NextAuth's HKDF key derivation (true for NextAuth tokens, false for pre-derived keys)
    /// </summary>
    public bool UseNextAuthKeyDerivation { get; set; } = true;

    /// <summary>
    /// The session cookie name used as salt in HKDF (Auth.js v5 default: "authjs.session-token")
    /// For NextAuth v4, this was an empty string
    /// </summary>
    public string SessionCookieName { get; set; } = "authjs.session-token";

    public bool ValidateLifetime { get; set; } = true;
    public int ClockSkewSeconds { get; set; } = 300;
}
