using System.Linq.Expressions;

namespace WebApplication2.TunableList
{
    public class FilterField<TModel, TDbEntity>
    {
        public string Name { get; init; }

        public string? Title { get; set; }

        public Expression<Func<TDbEntity, TModel>> SelectExpression { get; set; }

        // public Expression<Func<TEntity, TKey>>? SortKeySelector<TEntity, TKey>{ get; set; }

        public Expression<Func<TModel, bool>>? FilterExpression { get; set; }
    }

    // public class FilterFieldCalculated<TEntity, TDto, TField>: FilterField<TEntity, TDto>
    // {
    //     // public FilterField(T1 filterFieldType, string name)
    //     // {
    //     //     FilterFieldType = filterFieldType;
    //     //     Name = name;
    //     // }
    //
    //     //public T1 FilterFieldType { get; init; }
    //
    //
    //     public Expression<Func<TEntity, TField>>? Value { get; set; }


    //
    // public Expression<Func<TSource, TKey>>? SortKeySelector<TSource, TKey>{ get; set; }
    // public Expression<Func<T2, bool>>? SelectExpression { get; set; }
    // public Expression<Func<T2, bool>>? FilterExpression { get; set; }


    // }
}