using System.Diagnostics;
using AutoMapper;
using Infrastructure.Persistanse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly CalDbContext _context;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly ILogger<HomeController> _logger;
    private readonly Lazy<IMapper> _mapper;

    private const int _historyLines = 40;

    public HomeController(CalDbContext context, ILogger<HomeController> logger, IHostEnvironment hostEnvironment, Lazy<IMapper> mapper)
    {
        _context = context;
        _hostEnvironment = hostEnvironment;
        _logger = logger;
        _mapper = mapper;

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

    private List<CalEvent> GenerateRepeatingEvents(List<CalEvent> events, DateTime sheetFirstDay, DateTime sheetLastDay, DateTime today)
        {
            var eventsModel = new List<CalEvent>();
            int currMonMaxDay = Utils.Utils.GetMaxDayOfTheMonth(today);

            foreach (var evt in events)
            {
                if (evt.Repeat == CalEventRepeat.Monthly)
                {
                    int betweenStartedAndSheet = today.Month - evt.Started.Month;

                    for (int i = -1; i <= 1; i++) // Add for 3 month (Prev, Current, Next and Prev.):
                    {
                        //var startedDateForMonthlyOLD = new DateTime(today.Year, today.Month, evt.Day).AddMonths(i);

                        // TODO:

                        // Handle 29 of February!!!!!!!!!!!!!
                        // Handle 29 of February!!!!!!!!!!!!!
                        var startedDateForMonthly = new DateTime(today.Year, evt.Started.Month, evt.Day).AddMonths(betweenStartedAndSheet + i);

                        if (startedDateForMonthly >= sheetFirstDay && startedDateForMonthly <= sheetLastDay)
                        {
                            eventsModel.Add(new CalEvent()
                            {
                                Id = evt.Id,
                                Day = evt.Started.Day,
                                Description = evt.Description,
                                EveryXDays = evt.EveryXDays,
                                Modified = evt.Modified,
                                Month = startedDateForMonthly.Month,
                                Repeat = evt.Repeat,
                                Started = startedDateForMonthly,
                                Status = evt.Status,
                                Time = evt.Time,
                                Year = startedDateForMonthly.Year
                            });
                        }
                    }
                }
                else if (evt.Repeat == CalEventRepeat.Yearly)
                {
                    var startedDateForYearly = new DateTime(today.Year, evt.Month, evt.Day);

                    if (startedDateForYearly >= sheetFirstDay && startedDateForYearly <= sheetLastDay)
                    {
                        eventsModel.Add(new CalEvent()
                        {
                            Id = evt.Id,
                            Day = evt.Started.Day,
                            Description = evt.Description,
                            EveryXDays = evt.EveryXDays,
                            Modified = evt.Modified,
                            Month = startedDateForYearly.Month,
                            Repeat = evt.Repeat,
                            Started = startedDateForYearly,
                            Status = evt.Status,
                            Time = evt.Time,
                            Year = startedDateForYearly.Year
                        });
                    }
                }
                else if (evt.Repeat == CalEventRepeat.EveryXdays)
                {
                    var startedDateForCurrent = evt.Started;

                    while (startedDateForCurrent <= sheetLastDay)
                    {
                        if (startedDateForCurrent >= sheetFirstDay)
                        {
                            eventsModel.Add(new CalEvent()
                            {
                                Id = evt.Id,
                                Day = evt.Started.Day,
                                Description = evt.Description,
                                EveryXDays = evt.EveryXDays,
                                Modified = evt.Modified,
                                Month = (evt.Repeat == CalEventRepeat.Monthly) ? 0 : evt.Started.Month,
                                Repeat = evt.Repeat,
                                Started = startedDateForCurrent.Date,
                                Status = evt.Status,
                                Time = evt.Time,
                                Year = (evt.Repeat == CalEventRepeat.Yearly) ? 0 : evt.Started.Year
                            });
                        }
                        // date increment.
                        startedDateForCurrent = startedDateForCurrent.AddDays(evt.EveryXDays.Value);
                    }
                }
                else
                {
                    eventsModel.Add(evt);
                }
            }
            return eventsModel;
        }
    }
