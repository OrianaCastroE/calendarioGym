using DarkKitchen.API.Filters;
using DarkKitchen.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace DarkKitchen.Tests.API.Filters;

[TestClass]
public class ExceptionFilterTest
{
    private ExceptionFilter? exceptionFilter;

    [TestInitialize]
    public void Setup()
    {
        exceptionFilter = new ExceptionFilter();
    }

    private static ExceptionContext BuildContext(Exception exception)
    {
        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            new ActionDescriptor());

        return new ExceptionContext(actionContext, [])
        {
            Exception = exception
        };
    }

    [TestMethod]
    public void OnException_BadRequestException_Returns400()
    {
        var context = BuildContext(new BadRequestException("Bad request."));

        exceptionFilter!.OnException(context);
        var result = context.Result as ObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(400, result.StatusCode);
    }

    [TestMethod]
    public void OnException_UnauthorizedException_Returns401()
    {
        var context = BuildContext(new UnauthorizedException("Unauthorized."));

        exceptionFilter!.OnException(context);
        var result = context.Result as ObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(401, result.StatusCode);
    }

    [TestMethod]
    public void OnException_AccessDeniedException_Returns403()
    {
        var context = BuildContext(new AccessDeniedException("Access denied."));

        exceptionFilter!.OnException(context);
        var result = context.Result as ObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(403, result.StatusCode);
    }

    [TestMethod]
    public void OnException_NotFoundException_Returns404()
    {
        var context = BuildContext(new NotFoundException("Not found."));

        exceptionFilter!.OnException(context);
        var result = context.Result as ObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(404, result.StatusCode);
    }

    [TestMethod]
    public void OnException_UnhandledException_Returns500()
    {
        var context = BuildContext(new Exception("Unexpected error."));

        exceptionFilter!.OnException(context);
        var result = context.Result as ObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(500, result.StatusCode);
    }
}
