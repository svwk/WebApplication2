using System.Globalization;
using System.Linq.Expressions;
using WebApplication2.TunableList;

namespace WebApplication2
{
    public class TunableListModel<TModel, TDbEntity>
    {
        /// <summary>
        /// Поле, по которому будет производится сортировка
        /// </summary>
        /// SortField
        public FilterField<TModel, TDbEntity>? SelectedSortField { get; set; }

        /// <summary>
        /// Направление сортиировки по убыванию или возрастанию
        /// </summary>
        public SortDirection? SelectedSortDirection { get; set; }

        public Expression<Func<TModel, bool>>? FilterExpression { get; set; }

        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;

        /// <summary>
        /// Столбцы, которые будут в результирующем наборе
        /// </summary>
        public ICollection<FilterField<TModel, TDbEntity>> SelectedFields { get; set; } = new List<FilterField<TModel, TDbEntity>>();

        /// <summary>
        /// Столбцы, которые будут использоваться в фильтре
        /// </summary>
        public ICollection<FilterField<TModel, TDbEntity>> FilterFields { get; set; } = new List<FilterField<TModel, TDbEntity>>();

        // public FilterField<T1, T2> AddField(T1 filterFieldType, string name, string? title = null,
        //     bool sorted = false, SortDirection sortDirection = SortDirection.Ascending, //Expression<Func<T2, bool>>? sortExpression = null,
        //     bool selected = true, Expression<Func<T2, bool>>? selectExpression = null,
        //     bool filtered = false, Expression<Func<T2, bool>>? filterExpression = null)
        // {
        //     var field = new FilterField<T1, T2>(filterFieldType, name)
        //     {
        //         FilterFieldType = filterFieldType
        //     };
        //
        //
        //
        //     if (sorted && SortableFields.Contains(filterFieldType) ) //&& sortExpression is not null)
        //     {
        //         SelectedSortField = field;
        //         SelectedSortDirection = sortDirection;
        //         // field.SortkeySelector = sortExpression;
        //     }
        //
        //     if (selected && SelectableFields.Contains(filterFieldType) && selectExpression is not null)
        //     {
        //         SelectedFields.Add(field);
        //         field.SelectExpression = selectExpression;
        //     }
        //
        //     if (filtered && FiltableFields.Contains(filterFieldType) && filterExpression is not null)
        //     {
        //         FilterFields.Add(field);
        //         field.FilterExpression = filterExpression;
        //     }
        //
        //     return field;
        // }

        public virtual FilterField<TModel, TDbEntity> AddField(
            string name,
            Expression<Func<TDbEntity, TModel>>? selectExpression = null,
            string? title = null,
            bool sorted = false,
            SortDirection sortDirection = SortDirection.Ascending,
            bool selected = true,
            bool filtered = false,
            Expression<Func<TModel, bool>>? filterExpression = null)
        {
            var field = new FilterField<TModel, TDbEntity>
            {
                Name = name,
                Title = title,
                //Формирование выражения
                SelectExpression = selectExpression ?? (model => default),
                FilterExpression = filterExpression
            };

            if (sorted)
            {
                SelectedSortField = field;
                SelectedSortDirection = sortDirection;
            }

            if (filtered)
            {
                FilterFields.Add(field);
                if (filterExpression == null)
                {
                    // Формирование выражения
                    //field.FilterExpression= x=>
                    ;
                }
            }

            if (selected)
            {
                SelectedFields.Add(field);
            }

            //if ((selected || sorted || filtered) && field.SelectExpression)

            return field;
        }

        public FilterField<TModel, TDbEntity> MakeFieldSortable(
            FilterField<TModel, TDbEntity> field,
            SortDirection sortDirection = SortDirection.Ascending
        )
        {
            SelectedSortField = field;
            SelectedSortDirection = sortDirection;

            return field;
        }

        public FilterField<TModel, TDbEntity>? MakeFieldSortable(
            string name,
            SortDirection sortDirection = SortDirection.Ascending)
        {
            var field = SelectedFields.FirstOrDefault(f => f.Name == name);

            if (field != null) //&& sortExpression is not null)
            {
                SelectedSortField = field;
                SelectedSortDirection = sortDirection;
            }

            return field;
        }

        // public FilterField<T1, TEntity> MakeFieldSelected(
        //     FilterField<T1, TEntity> field,
        //     bool selected = true,
        //     Expression<Func<TEntity, bool>>? selectExpression = null)
        // {
        //     if (!selected && SelectedFields.Contains(field))
        //     {
        //         SelectedFields.Remove(field);
        //
        //         return field;
        //     }
        //
        //     if (selected && SelectableFields.Contains(field.FilterFieldType) && selectExpression is not null)
        //     {
        //         SelectedFields.Add(field);
        //         field.SelectExpression = selectExpression;
        //     }
        //
        //     return field;
        // }
        //
        // public FilterField<T1, TEntity> MakeFieldFiltered(
        //     FilterField<T1, TEntity> field,
        //     bool filtered = false,
        //     Expression<Func<TEntity, bool>>? filterExpression = null)
        // {
        //     if (!filtered && FilterFields.Contains(field))
        //     {
        //         FilterFields.Remove(field);
        //
        //         return field;
        //     }
        //
        //     if (filtered && FiltableFields.Contains(field.FilterFieldType) && filterExpression is not null)
        //     {
        //         FilterFields.Add(field);
        //         field.FilterExpression = filterExpression;
        //     }
        //
        //     return field;
        // }
        //
        // public Expression<Func<TModel, bool>>? GetFilterExpression()
        // {
        //     Expression<Func<TModel, bool>> expr = x => true;
        //     foreach (var field in FilterFields)
        //     {
        //         if (field.FilterExpression != null)
        //         {
        //             expr = expr.AndAlso(field.FilterExpression);
        //         }
        //     }
        //
        //     return expr;
        // }
        public virtual IQueryable<TModel>? GetQuery(IQueryable<TDbEntity> query)
        {
            var newQuery = Select(query);
            if (newQuery != null)
            {
                newQuery = Filter(newQuery);
                newQuery = Order1(newQuery);
            }

            return newQuery;
        }

        public virtual IQueryable<TModel> Filter(IQueryable<TModel> query)
        {
            if (FilterExpression != null)
            {
                query = query.Where(FilterExpression);
            }

            foreach (var field in FilterFields)
            {
                if (field.FilterExpression != null)
                {
                    query = query.Where(field.FilterExpression);
                }
            }

            return query;
        }

        // public Expression<Func<T2, bool>>? GetSortExpression()
        // {
        //     if (SelectedSortField != null && SelectedSortField.SortkeySelector != null)
        //     {
        //         return
        //     }
        //
        //     Expression<Func<T2, bool>> expr = x => true;
        //     foreach (var field in FilterFields)
        //     {
        //         if (field.SortkeySelector != null)
        //         {
        //             expr = expr.AndAlso(field.FilterExpression);
        //         }
        //     }
        //
        //     return expr;
        // }

        public virtual IQueryable<TModel> Order(IQueryable<TModel> query)
        {
            //SelectedSortField ??= SelectedFields?.FirstOrDefault();
            if (SelectedSortField is null)
            {
                return query;
            }

            var propertyName = SelectedSortField.Name;
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return query;
            }

            string orderByMethod = SelectedSortDirection == SortDirection.Ascending
                ? "OrderBy"
                : "OrderByDescending";

            try
            {
                ParameterExpression pe = Expression.Parameter(query.ElementType);
                MemberExpression me = Expression.Property(pe, propertyName);

                // var type = typeof(TModel);
                // var propertyInfo = type.GetProperty(propertyName);

                //MethodCallExpression
                var orderByCall = Expression.Call(
                    typeof(Queryable),
                    orderByMethod,
                    new Type[] { query.ElementType, me.Type },
                    query.Expression,
                    Expression.Quote(Expression.Lambda(me, pe)));

                var newQuery = query.Provider.CreateQuery(orderByCall) as IQueryable<TModel>;
                if (newQuery != null)
                {
                    query = newQuery;
                }

                return query;
            }
            catch (Exception e)
            {
                return query;
            }
        }

        public virtual IQueryable<TModel> Order1(IQueryable<TModel> query)
        {
            //SelectedSortField ??= SelectedFields?.FirstOrDefault();
            if (SelectedSortField is null || query == null)
            {
                return query;
            }

            var propertyName = SelectedSortField.Name;
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return query;
            }

            //propertyName = propertyName.First().ToString().ToUpper(new CultureInfo("en-US", false)) + propertyName.Substring(1);

            string methodName = SelectedSortDirection == SortDirection.Ascending
                ? "OrderBy"
                : "OrderByDescending";

            try
            {
                var type = typeof(TModel);
                var arg = Expression.Parameter(type, "x");

                var propertyInfo = type.GetProperty(propertyName);
                var mExpr = Expression.Property(arg, propertyInfo);
                type = propertyInfo.PropertyType;

                var delegateType = typeof(Func<,>).MakeGenericType(typeof(TModel), type);
                var lambda = Expression.Lambda(delegateType, mExpr, arg);

                var orderedSource = typeof(Queryable).GetMethods().Single(
                        method => method.Name == methodName
                                  && method.IsGenericMethodDefinition
                                  && method.GetGenericArguments().Length == 2
                                  && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(TModel), type)
                    .Invoke(null, new object[] { query, lambda });

                return (IQueryable<TModel>)orderedSource;
            }
            catch (Exception)
            {
                return query;
            }
        }

        public virtual IQueryable<TModel>? Select(IQueryable<TDbEntity> query)
        {
            Expression<Func<TDbEntity, TModel>>? selectExpression = null;
            foreach (var field in SelectedFields)
            {
                selectExpression = selectExpression == null
                    ? field.SelectExpression
                    : Combine1(selectExpression, field.SelectExpression);
            }

            if (selectExpression != null)
            {
                return query.Select(selectExpression);
            }
            else
            {
                return null;
            }
        }

        static Expression<Func<TSource, TDestination>> Combine<TSource, TDestination>(
            params Expression<Func<TSource, TDestination>>[] selectors)
        {
            var param = Expression.Parameter(typeof(TSource), "x");

            return Expression.Lambda<Func<TSource, TDestination>>(
                Expression.MemberInit(
                    Expression.New(typeof(TDestination).GetConstructor(Type.EmptyTypes)),
                    from selector in selectors
                    let replace = new ParameterReplaceVisitor(selector.Parameters[0], param)
                    from binding in ((MemberInitExpression)selector.Body).Bindings
                        .OfType<MemberAssignment>()
                    select Expression.Bind(binding.Member,
                        replace.VisitAndConvert(binding.Expression, "Combine")))
                , param);
        }

        static Expression<Func<TSource, TDestination>> Combine1<TSource, TDestination>(
            params Expression<Func<TSource, TDestination>>[] selectors)
        {
            var zeroth = ((MemberInitExpression)selectors[0].Body);
            var param = selectors[0].Parameters[0];
            List<MemberBinding> bindings = new List<MemberBinding>(zeroth.Bindings.OfType<MemberAssignment>());

            for (int i = 1; i < selectors.Length; i++)
            {
                var memberInit = (MemberInitExpression)selectors[i].Body;
                var replace = new ParameterReplaceVisitor(selectors[i].Parameters[0], param);
                foreach (var binding in memberInit.Bindings.OfType<MemberAssignment>())
                {
                    bindings.Add(Expression.Bind(binding.Member,
                        replace.VisitAndConvert(binding.Expression, "Combine")));
                }
            }

            return Expression.Lambda<Func<TSource, TDestination>>(
                Expression.MemberInit(zeroth.NewExpression, bindings), param);
        }
    }
}