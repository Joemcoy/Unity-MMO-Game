using System;
using System.Reflection;
using System.Collections.Generic;

namespace tFramework.DataDriver
{
	using System.Linq.Expressions;
	using Data.Interfaces;
	using Interfaces;
    using tFramework.Helper;

    public enum Function
    {
        Date,
        Day,
        Month,
        Year,
        Time,
        Hour,
        Minute,
        Second,
        Absolute,
        Sin,
        Cos,
        Tan
    }

    public class QueryBuilder<TModel>
		where TModel : IModel, new()
	{
		public Dictionary<string, object> Parameters { get; private set; }
		public string LeftPartial { get; private set; }
        public string RightPartial { get; private set; }
        private IDriver<TModel> driver;

		int counter = 0;

		public QueryBuilder(IDriver<TModel> driver)
		{
            this.driver = driver;

			Parameters = new Dictionary<string, object>();
            LeftPartial = string.Empty;
            RightPartial = string.Empty;
		}		

		private QueryOperand BaseOperation<TProperty>(Expression<Func<TModel, TProperty>> expr, string op)
		{
			var property = ReflectionHelper.ExtractProperty(expr);
			RightPartial += string.Format(op, driver.Settings.GetColumn(property.Name));

			return new QueryOperand(this, driver.Settings.GetColumn(property.Name));
		}

        private string GetFunctionString(Function func)
        {
            switch(func)
            {
                case Function.Date:
                    return "DATE";
                case Function.Day:
                    return "DAY";
                case Function.Month:
                    return "MONTH";
                case Function.Year:
                    return "YEAR";
                case Function.Time:
                    return "TIME";
                case Function.Hour:
                    return "HOUR";
                case Function.Minute:
                    return "MINUTE";
                case Function.Second:
                    return "SECOND";
                case Function.Absolute:
                    return "ABS";
                case Function.Sin:
                    return "SIN";
                case Function.Cos:
                    return "COS";
                case Function.Tan:
                    return "TAN";
            }
            throw new NotSupportedException();
        }

        public QueryBuilder<TModel> WhereID(int id)
        {
            return Where(m => m.ID).Equal(id);
        }

        public QueryOperand Where<TProperty>(Expression<Func<TModel, TProperty>> expr)
		{
			return BaseOperation(expr, $" WHERE {driver.Settings.OpenDefinitionChar}{{0}}{driver.Settings.CloseDefinitionChar}");
		}

        public QueryOperand Where<TProperty>(Expression<Func<TModel, TProperty>> expr, Function func)
        {
            return BaseOperation(expr, $" WHERE {GetFunctionString(func)}({driver.Settings.OpenDefinitionChar}{{0}}{driver.Settings.CloseDefinitionChar})");
        }

        public QueryOperand Where<TProperty>(Expression<Func<TModel, TProperty>> expr, OperationAlignment align, int n)
        {
            return BaseOperation(expr, $" WHERE {(align == OperationAlignment.Left ? "LEFT" : "RIGHT")}({{0}}, {n})");
        }

        public QueryOperand Or<TProperty>(Expression<Func<TModel, TProperty>> expr)
		{
			return BaseOperation(expr, $" OR {driver.Settings.OpenDefinitionChar}{{0}}{driver.Settings.CloseDefinitionChar}");
		}

        public QueryOperand Or<TProperty>(Expression<Func<TModel, TProperty>> expr, Function func)
        {
            return BaseOperation(expr, $" OR {GetFunctionString(func)}({driver.Settings.OpenDefinitionChar}{{0}}{driver.Settings.CloseDefinitionChar})");
        }

        public QueryOperand Or<TProperty>(Expression<Func<TModel, TProperty>> expr, OperationAlignment align, int n)
        {
            return BaseOperation(expr, $" OR {(align == OperationAlignment.Left ? "LEFT" : "RIGHT")}({{0}}, {n})");
        }

        public QueryOperand And<TProperty>(Expression<Func<TModel, TProperty>> expr)
        {
            return BaseOperation(expr, $" AND {driver.Settings.OpenDefinitionChar}{{0}}{driver.Settings.CloseDefinitionChar}");
        }

        public QueryOperand And<TProperty>(Expression<Func<TModel, TProperty>> expr, Function func)
        {
            return BaseOperation(expr, $" AND {GetFunctionString(func)}({driver.Settings.OpenDefinitionChar}{{0}}{driver.Settings.CloseDefinitionChar})");
        }

        public QueryOperand And<TProperty>(Expression<Func<TModel, TProperty>> expr, OperationAlignment align, int n)
        {
            return BaseOperation(expr, $" AND {(align == OperationAlignment.Left ? "LEFT" : "RIGHT")}({{0}}, {n})");
        }

        public QueryBuilder<TModel> OrderBy<TProperty>(Expression<Func<TModel, TProperty>> expr, bool desc = true)
        {
            var property = ReflectionHelper.ExtractProperty(expr);
            RightPartial += $" ORDER BY {driver.Settings.OpenDefinitionChar}{driver.Settings.GetColumn(property.Name)}{driver.Settings.CloseDefinitionChar} {(desc ? "DESC" : "ASC")}";

            return this;
        }

        public QueryBuilder<TModel> OpenParenthesis()
        {
            RightPartial += "(";
            return this;
        }

        public QueryBuilder<TModel> CloseParenthesis()
        {
            RightPartial += ")";
            return this;
        }

        public QueryBuilder<TModel> Limit(int Number)
        {
            var q = $" {string.Format(driver.Settings.LimitSettings.Keyword, Number)}";
            if (driver.Settings.LimitSettings.Alignment == OperationAlignment.Left)
                LeftPartial += q;
            else
                RightPartial += q;
            return this;
        }

        public QueryBuilder<TModel> Offset(int Number)
        {
            var q = $" {string.Format(driver.Settings.OffsetSettings.Keyword, Number)}";
            if (driver.Settings.OffsetSettings.Alignment == OperationAlignment.Left)
                LeftPartial += q;
            else
                RightPartial += q;
            return this;
        }

        public QueryBuilder<TModel> Paginate(int offset, int cnt)
        {
            var q = $" {string.Format(driver.Settings.PaginationSettings.Keyword, offset, cnt)}";
            if (driver.Settings.OffsetSettings.Alignment == OperationAlignment.Left)
                LeftPartial += q;
            else
                RightPartial += q;
            return this;
        }

        public class QueryOperand
        {
            QueryBuilder<TModel> builder;
            string name;

            public QueryOperand(QueryBuilder<TModel> builder, string name)
            {
                this.builder = builder;
                this.name = name;
            }

            private void Append(object value, string op = null)
            {
                var name = string.Format("value{0}", builder.counter++);
                builder.RightPartial +=
                    op == null ?
                    string.Format("@{0}", name) :
                    string.Format(" {1} @{0}", name, op);

                if (value is IModel)
                    value = (value as IModel).ID;
                builder.Parameters[name] = value;
            }

            private QueryBuilder<TModel> BaseOperand(object value, string op)
            {
                Append(value, op);
                return builder;
            }

            public QueryBuilder<TModel> Equal(object value)
            {
                return BaseOperand(value, "=");
            }

            public QueryBuilder<TModel> NotEqual(object value)
            {
                return BaseOperand(value, "<>");
            }

            public QueryBuilder<TModel> LessThan(object value)
            {
                return BaseOperand(value, "<");
            }

            public QueryBuilder<TModel> LessOrEqual(object value)
            {
                return BaseOperand(value, "<=");
            }

            public QueryBuilder<TModel> BiggerThan(object value)
            {
                return BaseOperand(value, ">");
            }

            public QueryBuilder<TModel> BiggerOrEqual(object value)
            {
                return BaseOperand(value, ">=");
            }

            public QueryBuilder<TModel> NotIn<T>(params T[] values)
            {
                builder.RightPartial += " NOT";
                return In(values);
            }

            public QueryBuilder<TModel> In<T>(params T[] values)
            {
                builder.RightPartial += " IN (";

                var c = 0;
                foreach (var value in values)
                {
                    Append(value);
                    builder.RightPartial += ++c < values.Length ? ", " : ")";
                }
                return builder;
            }

            public QueryBuilder<TModel> IsNull()
            {
                builder.RightPartial += " IS NULL";
                return builder;
            }

            public QueryBuilder<TModel> IsNotNull()
            {
                builder.RightPartial += " IS NOT NULL";
                return builder;
            }
        }
	}
}