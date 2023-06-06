using System.Linq.Expressions;
using WebApplication2.TunableList;

namespace WebApplication2
{
    public class NestedTunableListModel : TunableListModel<Class1Dto1, Detail>
    {
        private readonly Dictionary<string, Expression<Func<Detail, Class1Dto1>>> _selectExpressions = new()
        {
            { nameof(Class1Dto1.NewA), d => new Class1Dto1 { NewA = d.a * d.a } },
            { nameof(Class1Dto1.NewB), d => new Class1Dto1 { NewA = d.b * 2 } },
            { nameof(Class1Dto1.CountryId), d => new Class1Dto1 { CountryId = d.Country.Id + d.c } }
        };

        public ICollection<FilterField<Class1Dto1, Detail>> AddSelected(params string[] fieldNames)
        {
            foreach (var fieldName in fieldNames)
            {
                if (_selectExpressions.ContainsKey(fieldName))
                {
                    base.AddField(
                        fieldName,
                        _selectExpressions[fieldName],
                        title: nameof(Class1Dto1.NewA),
                        selected: true);
                }
            }


            return base.SelectedFields;
        }

        public override IQueryable<Class1Dto1> Order1(IQueryable<Class1Dto1> query)
        {
            return query.OrderBy(x => x.NewA);
        }

        public NestedTunableListModel()
        {
        }

        public FilterField<Class1Dto1, Detail> AddNewA(
            bool sorted = false,
            SortDirection sortDirection = SortDirection.Ascending,
            bool selected = true,
            bool filtered = false,
            Expression<Func<Class1Dto1, bool>>? filterExpression = null)
        {
            var field = base.AddField(
                nameof(Class1Dto1.NewA),
                d => new Class1Dto1 { NewA = d.a * d.a },
                title: nameof(Class1Dto1.NewA),
                sorted: false,
                selected: true,
                filtered: filtered,
                filterExpression: filterExpression);

            return field;
        }

        // public FilterField<Detail, Class1Dto1> AddField(
        //     string name,
        //     string? title = null,
        //     bool sorted = false,
        //     SortDirection sortDirection = SortDirection.Ascending,
        //     bool selected = true,
        //     bool filtered = false,
        //     Expression<Func<Class1Dto1, bool>>? filterExpression = null)
        // {
        //
        // }
    }
}