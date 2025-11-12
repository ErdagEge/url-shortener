using Microsoft.EntityFrameworkCore;
using ShortUrl.Api.Data;
using ShortUrl.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDb>(o =>
o.UseSqlite(builder.Configuration.GetConnectionString("Default") ?? "Data Source=shorturl.db"));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


// Health
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));


// Create short link
app.MapPost("/shorten", async (CreateRequest req, AppDb db) =>
{
    if (!Uri.TryCreate(req.Url, UriKind.Absolute, out var uri) ||
    (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        return Results.BadRequest(new { error = "Invalid URL" });


    string code;
    do { code = ShortCode(6); }
    while (await db.Links.AnyAsync(l => l.Code == code));


    var link = new Link { Code = code, Url = uri.ToString() };
    db.Links.Add(link);
    await db.SaveChangesAsync();


    return Results.Ok(new { code, link.Url, hits = link.Hits, createdAt = link.CreatedAt });
});


// Redirect
app.MapGet("/{code}", async (string code, AppDb db) =>
{
    var link = await db.Links.SingleOrDefaultAsync(l => l.Code == code);
    if (link is null) return Results.NotFound();


    link.Hits++;
    await db.SaveChangesAsync();
    return Results.Redirect(link.Url, permanent: false);
});


// Stats
app.MapGet("/stats/{code}", async (string code, AppDb db) =>
{
    var link = await db.Links.SingleOrDefaultAsync(l => l.Code == code);
    return link is null
    ? Results.NotFound()
    : Results.Ok(new { link.Code, link.Url, link.Hits, link.CreatedAt });
});


app.Run();


static string ShortCode(int len)
{
    const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    var rng = Random.Shared;
    return new string(Enumerable.Range(0, len).Select(_ => chars[rng.Next(chars.Length)]).ToArray());
}


record CreateRequest(string Url);