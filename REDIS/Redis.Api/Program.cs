using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var redisConnectionString = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
builder.Services.AddSingleton<ConnectionMultiplexer>(_ =>
{
	var configuration = ConfigurationOptions.Parse(redisConnectionString);
	configuration.AbortOnConnectFail = false;
	return ConnectionMultiplexer.Connect(configuration);
});
builder.Services.AddSingleton<IConnectionMultiplexer>(services =>
	services.GetRequiredService<ConnectionMultiplexer>());

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/redis/ping", async (IConnectionMultiplexer redis) =>
{
	try
	{
		var latency = await redis.GetDatabase().PingAsync();
		return Results.Ok(new { status = "ok", latencyMs = latency.TotalMilliseconds });
	}
	catch (RedisException)
	{
		return Results.Problem(
			statusCode: StatusCodes.Status503ServiceUnavailable,
			title: "Redis is unavailable.");
	}
});

// Body should have Key and Value = { "key": "someKey", "value": "Some Value Data" }
app.MapPost("/redis/data", async (RedisDataRequest request, IConnectionMultiplexer redis) =>
{
	if (string.IsNullOrWhiteSpace(request.Key))
	{
		return Results.BadRequest(new { error = "Key is required." });
	}

	try
	{
		await redis.GetDatabase().StringSetAsync(request.Key, request.Value);
		return Results.Ok(new { request.Key, request.Value });
	}
	catch (RedisException)
	{
		return Results.Problem(
			statusCode: StatusCodes.Status503ServiceUnavailable,
			title: "Redis is unavailable.");
	}
});

app.MapGet("/redis/data/{key}", async (string key, IConnectionMultiplexer redis) =>
{
	try
	{
		var value = await redis.GetDatabase().StringGetAsync(key);
		return value.HasValue
			? Results.Ok(new { key, value = value.ToString() })
			: Results.NotFound(new { error = "Data was not found.", key });
	}
	catch (RedisException)
	{
		return Results.Problem(
			statusCode: StatusCodes.Status503ServiceUnavailable,
			title: "Redis is unavailable.");
	}
});

app.Run();

public sealed record RedisDataRequest(string Key, string Value);
