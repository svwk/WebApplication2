using Microsoft.AspNetCore.Mvc;
using WebApplication2.TunableList;

// using WebApplication2.TunableList;

namespace WebApplication2.Controllers;

[ApiController]
[Route("[controller]")]
public class WController : ControllerBase
{
    private static readonly Class2 Classe2 = new Class2();
    private readonly ApplicationContext _db;
    private readonly ILogger<WController> _logger;

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public WController(ILogger<WController> logger, ApplicationContext db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpGet, Route("GetW")]
    public IEnumerable<WeatherForecast> GetW()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }


    [HttpGet, Route("GetAllClasses")]
    public List<int> GetAllClasses()
    {
        return _db.Details.Select(x => x.a).ToList();
    }

    [HttpGet, Route("GetClassesWithWhere")]
    public List<Class1Dto1> GetClassesWithWhere(int dd)
    {
        // var fielda = new FilterField1<Class1, int>
        // {
        //     Name = "a",
        //     Title = "a",
        //     Value = x => x.a * x.a,
        //     FilterExpression = x=> x.a*x.a >2
        // };
        // var e = Classe2.Classes
        //     .AsQueryable();
        var e = _db.Details.AsQueryable();
        var a = true;

        //e = e.Where(x => x.a * x.a > dd);
        var t = e.Select(x => new Class1Dto1
        {
            NewA = a ? x.a * x.a : default,
            NewB = !a ? x.b * 2 : default,
            Country = x.Country.Name,
            CountryId = x.Country.Id + x.c,
            AnyCountry = x.Country.Details.Any(y => y.b >= y.Country.Id),
            d = x.d
        });
        t = t.Where(x => a || x.CountryId >= dd);

        t = t.OrderBy(x => x.NewA);

        var r = t.ToList();

        return r;
    }

    [HttpGet, Route("GetClassesWithWhere2")]
    public List<Class1Dto1> GetClassesWithWhere2(int dd)
    {
        var tunableList = new TunableListModel<Class1Dto1, Detail>
        {
            FilterExpression = c => c.NewB >= 100 || c.NewA >= 100
        };

        tunableList.AddField(
            nameof(Class1Dto1.NewA),
            d => new Class1Dto1 { NewA = d.a * d.a },
            sorted: true,
            selected: true);

        tunableList.AddField(
            nameof(Class1Dto1.NewB),
            d => new Class1Dto1 { NewB = d.b * 2 },
            selected: true);

        tunableList.AddField(
            nameof(Class1Dto1.CountryId),
            d => new Class1Dto1 { CountryId = d.Country.Id + d.c },
            selected: true,
            filtered: true,
            filterExpression: c => c.CountryId >= 8);


        var query = tunableList.GetQuery(_db.Details.AsQueryable());
        if (query != null)
        {
            return query.ToList();
        }

        return new List<Class1Dto1>();
    }

    [HttpGet, Route("GetClassesWithWhere3")]
    public List<Class1Dto1> GetClassesWithWhere3(int dd)
    {
        var tunableList = new NestedTunableListModel
        {
            FilterExpression = c => c.NewB >= 100 || c.NewA >= 100
        };

        var fields = tunableList.AddSelected(nameof(Class1Dto1.NewA), nameof(Class1Dto1.NewB), nameof(Class1Dto1.CountryId));
        //tunableList.MakeFieldSortable(nameof(Class1Dto1.NewA));
        var query = tunableList.GetQuery(_db.Details.AsQueryable());

        if (query != null)
        {
            return query.ToList();
        }

        return new List<Class1Dto1>();
    }

    [HttpGet, Route("AddDetail")]
    public List<Detail> AddDetail([FromQuery] Detail dd)
    {
        // создаем два объекта User
        Detail detail = dd;

        // добавляем их в бд
        _db.Details.Add(dd);
        _db.SaveChanges();

        return _db.Details.ToList();
    }
}