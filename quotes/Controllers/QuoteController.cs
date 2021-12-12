namespace Quotes.Controllers;

using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class QuoteController : ControllerBase
{
    private static readonly Quote[] Quotes = new[]
    {
        new Quote
        {
            Text = "To alcohol! The cause of... and solution to... all of life's problems.",
            Author = "Homer Simpson", 
            Url = "https://en.wikipedia.org/wiki/Homer_vs._the_Eighteenth_Amendment",
        },
        new Quote
        {
            Text = "You got to help me. I don't know what to do. I can't make decisions. I'm a president!",
            Author = "President Skroob, Spaceballs", 
            Url = "https://en.wikipedia.org/wiki/Spaceballs",
        },
        new Quote
        {
            Text = "Beware of he who would deny you access to information, for in his heart he dreams himself your master.",
            Author = "Pravin Lal", 
            Url = "https://alphacentauri.gamepedia.com/Peacekeeping_Forces",
        },
        new Quote
        {
            Text = "About the use of language: it is impossible to sharpen a pencil with a blunt axe. It is equally vain to try to do it with ten blunt axes instead.",
            Author = "Edsger W. Dijkstra", 
            Url = "https://www.cs.utexas.edu/users/EWD/transcriptions/EWD04xx/EWD498.html",
        },
        new Quote
        {
            Text = "Those hours of practice, and failure, are a necessary part of the learning process.",
            Author = "Gina Sipley", 
            Url = null,
        },
        new Quote
        {
            Text = "Engineering is achieving function while avoiding failure.",
            Author = "Henry Petroski", 
            Url = null,
        },
        new Quote
        {
            Text = "Leadership is defined by what you do, not what you're called.",
            Author = "Jen Heemstra", 
            Url = "https://twitter.com/jenheemstra/status/1260186699021287424",
        },
        new Quote
        {
            Text = "Don't only practice your art, but force your way into its secrets; art deserves that, for it and knowledge can raise man to the Divine.",
            Author = "Ludwig van Beethoven", 
            Url = null,
        },
    };

    private readonly ILogger<QuoteController> _logger;

    public QuoteController(ILogger<QuoteController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetRandomQuote")]
    public Quote GetRandomQuote([FromQuery] string? opsi)
    {
        _logger.LogInformation("At GetRandomQuote");

        var activity = HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;

        activity?.SetTag("x.foo", "bar");

        _logger.LogInformation("Current Activity Id={activityId} TraceId={traceId} SpanId={spanId}", activity?.Id, activity?.TraceId, activity?.SpanId);

        if (opsi != null)
        {
            throw new ApplicationException(opsi);
        }

        return Quotes[Random.Shared.Next(0, Quotes.Length)];
    }
}
