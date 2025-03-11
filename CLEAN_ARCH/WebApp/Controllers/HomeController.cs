using System.Diagnostics;
using Domain;
using Infrastructure.Persistanse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly CalDbContext _context;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly ILogger<HomeController> _logger;

    private const int _historyLines = 40;

    public HomeController(CalDbContext context, ILogger<HomeController> logger, IHostEnvironment hostEnvironment)
    {
        _context = context;
        _hostEnvironment = hostEnvironment;
        _logger = logger;

        ViewBag.CssChanged =
            new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "/wwwroot/css/site.min.css")).LastWriteTime.ToString("yyMMddHHmm");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    [AllowAnonymous]
    public IActionResult Privacy() => View();

    [AllowAnonymous]
    public ContentResult StartPage() => base.Content(System.IO.File.ReadAllText("index.html"), "text/html");

    [AllowAnonymous]
    public async Task<IActionResult> Index(string pMonth = "")
    {
        await ProcessRequestHeaders(Request.Headers);

        TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time");
        DateTime today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);

        var todayForCurrentView = today;

        if (pMonth == "next") todayForCurrentView = today.AddMonths(1);
        else if (pMonth == "prev") todayForCurrentView = today.AddMonths(-1);

        DateTime currMonStart = new DateTime(todayForCurrentView.Year, todayForCurrentView.Month, 1);

        int monBeginDayOfWeek = (int)currMonStart.DayOfWeek; // Mon = 1 \ .. \ Sat = 6 \ Sun = 0

        int sheetSize = 7 * 6;
        int prevMonDaysOnSheet = monBeginDayOfWeek == 0 ? 6 : monBeginDayOfWeek - 1;
        int currMonDays = new DateTime(todayForCurrentView.Year, todayForCurrentView.Month, 1).AddMonths(1).AddDays(-1).Day;
        int nextMonDaysOnSheet = sheetSize - (currMonDays + prevMonDaysOnSheet);

        DateTime sheetFirstDay = monBeginDayOfWeek == 1 ? currMonStart : currMonStart.AddDays(-prevMonDaysOnSheet);
        DateTime sheetLastDay = new DateTime(todayForCurrentView.Year, todayForCurrentView.Month, nextMonDaysOnSheet).AddMonths(1);

        // Gat All already started Events with applicable non-repeating:
        var events = await _context.CalEvents
            .Where(e => e.Started <= sheetLastDay && (e.Repeat != CalEventRepeat.Once || e.Repeat == CalEventRepeat.Once && e.Started >= sheetFirstDay))
            .ToListAsync();

        var allEventsCount = await _context.CalEvents.CountAsync();

        var eventsModel = GenerateRepeatingEvents(events, sheetFirstDay, sheetLastDay, todayForCurrentView);

        ViewBag.EnvtsCount = eventsModel.Count;

        // Fill empty days:
        for (var i = 0; i < sheetSize; i++) { eventsModel.Add(new CalEvent(sheetFirstDay.AddDays(i))); }

        ViewBag.EnvtsFullCount = allEventsCount;
        ViewBag.IsDevEnv = _hostEnvironment.IsDevelopment();
        ViewBag.TodayCurrent = todayForCurrentView;
        ViewBag.TodayReal = today;
        ViewBag.SheetFirstDay = sheetFirstDay;

        return View(eventsModel);
    }

    private async Task ProcessRequestHeaders(IHeaderDictionary headers)
    {
        await ProcessHeader(new RequestHeaderVM(ReqHeadFieldType.Accept, headers["Accept"]));
        // await ProcessHeader(new RequestHeaderField(ReqHeadFieldType.Encode, headers["Accept-Encoding"]));
        // await ProcessHeader(new RequestHeaderField(ReqHeadFieldType.Language, headers["Accept-Language"]));
        // await ProcessHeader(new RequestHeaderField(ReqHeadFieldType.Referer, headers["Referer"]));
        await ProcessHeader(new RequestHeaderVM(ReqHeadFieldType.UsrAgent, headers["User-Agent"]));
    }

    private async Task ProcessHeader(RequestHeaderVM reqHead)
    {
        if (!string.IsNullOrWhiteSpace(reqHead.Text))
        {
            if (!_context.RequestsHeaders.Any(old =>
                old.Field == reqHead.Field && old.Text == reqHead.Text && old.Created.AddHours(1) > reqHead.Created))
            {
                _context.RequestsHeaders.Add(reqHead);
                _context.SaveChanges();
            }
        }
    }
}
