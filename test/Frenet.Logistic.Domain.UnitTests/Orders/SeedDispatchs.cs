using System;
using Frenet.Logistic.Domain.Dispatchs;
using Frenet.Logistic.Domain.Orders;

namespace Frenet.Logistic.Domain.UnitTests.Orders;

public class SeedDispatchs
{
   public static Dispatch CreateDispatch(Dispatch dispatch) => new(
        Guid.NewGuid(),
        new PackageParams(
            dispatch.Package.Weight,
            dispatch.Package.Height,
            dispatch.Package.Width,
            dispatch.Package.Length
        ));

    public static ZipCode CreateZipCode(ZipCode zipCode) => new(
        zipCode.CodeFrom,
        zipCode.CodeTo
       );
}
