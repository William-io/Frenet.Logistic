﻿using Dapper;
using System.Data;

namespace Frenet.Logistic.Infrastructure.Data;

internal sealed class DateHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override DateOnly Parse(object value) => DateOnly.FromDateTime((DateTime)value);

    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.DbType = DbType.Date;
        parameter.Value = value;
    }
}
