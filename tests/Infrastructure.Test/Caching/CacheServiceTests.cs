using FluentAssertions;
using MultiMart.Application.Common.Caching;
using Xunit;

namespace Infrastructure.Test.Caching;

public abstract class CacheServiceTests
{
    private record TestRecord(Guid Id, string StringValue, DateTime DateTimeValue);

    private const string TestKey = "testkey";
    private const string TestValue = "testvalue";

    private readonly ICacheService _sut;

    protected CacheServiceTests(ICacheService cacheService) => _sut = cacheService;

    [Fact]
    public void ThrowsGivenNullKey()
    {
        var action = () => { string? result = _sut.Get<string>(null!); };

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ReturnsNullGivenNonExistingKey()
    {
        string? result = _sut.Get<string>(TestKey);

        result.Should().BeNull();
    }

#pragma warning disable RCS1158
    public static IEnumerable<object[]> ValueData => new List<object[]>
#pragma warning restore RCS1158
        {
            new object[] { TestKey, TestValue },
            new object[] { "integer", 1 },
            new object[] { "long", 1L },
            new object[] { "double", 1.0 },
            new object[] { "bool", true },
            new object[] { "date", new DateTime(2022, 1, 1) },
        };

    [Theory]
    [MemberData(nameof(ValueData))]
    public void ReturnsExistingValueGivenExistingKey<T>(string testKey, T testValue)
    {
        _sut.Set(testKey, testValue);
        T? result = _sut.Get<T>(testKey);

        result.Should().Be(testValue);
    }

    [Fact]
    public void ReturnsExistingObjectGivenExistingKey()
    {
        var expected = new TestRecord(Guid.NewGuid(), TestValue, DateTime.UtcNow);

        _sut.Set(TestKey, expected);
        var result = _sut.Get<TestRecord>(TestKey);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ReturnsNullGivenAnExpiredKey()
    {
        _sut.Set(TestKey, TestValue, TimeSpan.FromMilliseconds(200));

        string? result = _sut.Get<string>(TestKey);
        Assert.Equal(TestValue, result);

        await Task.Delay(250);
        result = _sut.Get<string>(TestKey);

        result.Should().BeNull();
    }
}