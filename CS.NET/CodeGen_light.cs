static readonly char[] Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

private string GenerateSegment(int codeLength)
{
    Span<char> buf = stackalloc char[codeLength];
    for (var i = 0; i < codeLength; i++)
        buf[i] = Chars[Random.Shared.Next(Chars.Length)];
    return new string(buf);
}
