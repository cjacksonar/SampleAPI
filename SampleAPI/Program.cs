using Data;
using Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using Services;
using Services.Logger;
using System.Reflection;
using System.Text;


// Visual Studio menu you can use Project/Export to export this project to a project template, then create a new project from that template
// Once you create a new project you should change the values below to match your new project and baseUrl to where you will deploy the production application. 
var baseUrl = "http://yourdomain.com";                 
var swaggerSpecName = "SampleOpenAPISpecification";     
var swaggerAPIName = "Sample API";                      
var swaggerAPIDeploymentFolder = "SampleAPI";           // change this when use project template to the folder name in production

var builder = WebApplication.CreateBuilder(args);
LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

builder.Services.ConfigureCors();
builder.Services.ConfigureLoggerService();

// Add services to the container, added problem details information, added AddCustomCSVFormatter() refer ServiceExtensions.cs class. 
builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = false;   
})
.AddNewtonsoftJson()
.AddCustomCSVFormatter()
.AddXmlDataContractSerializerFormatters()
.ConfigureApiBehaviorOptions(setupAction =>
{
    setupAction.InvalidModelStateResponseFactory = context =>
    {
        // create a problem details object
        var problemDetailsFactory = context.HttpContext.RequestServices
           .GetRequiredService<ProblemDetailsFactory>();
        var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                context.HttpContext,
                context.ModelState);

        // add additional info not added by default
        problemDetails.Detail = "See the errors field for details.";
        problemDetails.Instance = context.HttpContext.Request.Path;

        // find out which status code to use
        var actionExecutingContext =
             context as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

        // if there are modelstate errors & all keys were correctly
        // found/parsed we're dealing with validation errors
        if ((context.ModelState.ErrorCount > 0) &&
           (actionExecutingContext?.ActionArguments.Count == context.ActionDescriptor.Parameters.Count))
        {
            problemDetails.Type = "https://yourdomain.com/modelvalidationproblem";
            problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
            problemDetails.Title = "One or more validation errors occurred.";

            return new UnprocessableEntityObjectResult(problemDetails)
            {
                ContentTypes = { "application/problem+json" }
            };
        }

        // if one of the keys wasn't correctly found / couldn't be parsed
        // we're dealing with null/unparsable input
        problemDetails.Status = StatusCodes.Status400BadRequest;
        problemDetails.Title = "One or more errors on input occurred.";
        return new BadRequestObjectResult(problemDetails)
        {
            ContentTypes = { "application/problem+json" }
        };
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// adding api versioning use the [ApiVersion("1.0")] on the controllers. Each controller can support multiple version
// swagger uses Select a Defintion should shows the versions.
builder.Services.AddApiVersioning(setupAction =>
{
    setupAction.ReportApiVersions = true;
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new ApiVersion(1, 0);    
});
builder.Services.AddVersionedApiExplorer(setupAction =>
{
    setupAction.GroupNameFormat = "'v'VV";
});
// the produces a list of versions so it can generate multiple swagger documents using the for each on ApiVersionDescriptions
var apiVersionDescriptionProvider = builder.Services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    // https://rimdev.io/swagger-grouping-with-controller-name-fallback-using-swashbuckle-aspnetcore/
    // uses the  [ApiExplorerSettings(GroupName = "xxxx")] attribute to assign to controller name.
    c.TagActionsBy(api =>
    {
        if (api.GroupName != null) { return new[] { api.GroupName }; }
        var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
        if (controllerActionDescriptor != null)
        {
            return new[] { controllerActionDescriptor.ControllerName };
        }
        throw new InvalidOperationException("Unable to determine tag for endpoint.");
    });
    c.DocInclusionPredicate((name, api) => true);
    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"<h3><b>{swaggerAPIName} </b> using <font color='maroon'><b>.Net 8.0</b></font>.</h3>");
        sb.AppendLine("<b>Sample API uses a database designed for managing contributions to multiple funds. It supports multiple versions</b>.");
        sb.AppendLine("Select a version using the <b>Select a definition</b>. V1.0 actions require using");
        sb.AppendLine("Authentication. V2.0 actions do not require Authentication. The approach for maintaining multiple versions was ");
        sb.AppendLine("to place the version number in the api path so when trying out an action the version number must be entered");
        sb.AppendLine("such as 1.0 for V1.0 and 2.0 for V2.0. <b>To access all actions</b> in V1.0, click <b>POST</b> under Authentication, click <b>Try It Out</b>,");
        sb.AppendLine("enter <b>1.0</b> in for required version, <b>copy the sample request to the request body</b>, and then click ,<b>Execute</b>. ");
        sb.AppendLine("If successful, <b>copy the token value</b> from the Response body by clicking the clipboard icon next to the Download button. Click");
        sb.AppendLine("the <b>Authorize button</b> and paste the token into the Value area and click <b>Close</b> button. Now you should be authorized to use the ");
        sb.AppendLine("other API functions. Select from Media Type to return JSON,XML, or CSV");

        c.SwaggerDoc(
            $"{swaggerSpecName}{description.GroupName}",
            new Microsoft.OpenApi.Models.OpenApiInfo()
            {
                Title = $"{swaggerAPIName}",
                Version = description.ApiVersion.ToString(),
                Description = sb.ToString(),
            });
    }
    // need this for the operation specs to show up
    c.DocInclusionPredicate((documentName, apiDescription) =>
    {
        var actionApiVersionModel = apiDescription.ActionDescriptor
        .GetApiVersionModel(ApiVersionMapping.Explicit | ApiVersionMapping.Implicit);

        if (actionApiVersionModel == null)
        {
            return true;
        }

        if (actionApiVersionModel.DeclaredApiVersions.Any())
        {
            return actionApiVersionModel.DeclaredApiVersions.Any(v =>
            $"{swaggerSpecName}v{v.ToString()}" == documentName);
        }
        return actionApiVersionModel.ImplementedApiVersions.Any(v =>
            $"{swaggerSpecName}v{v.ToString()}" == documentName);
    });
    c.AddSecurityDefinition("ApiBearerAuth", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Input a valid token to access this API"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiBearerAuth" }
            }, new List<string>() }
    });
    // need to change project properties for file name to match project name such as
    // <DocumentationFile>SampleAPI.xml</DocumentationFile>
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// added data context with connection string and repositories
builder.Services.AddDbContext<APIDbContext>(dbContextOptions => dbContextOptions.UseSqlServer(Classes.Globals.ApplicationDatabaseConnectionString));
builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddScoped<IFundRepository, FundRepository>();
builder.Services.AddScoped<IContributorRepository, ContributorRepository>();
builder.Services.AddScoped<IContributionRepository, ContributionRepository>();
builder.Services.AddScoped<IRegisteredUserRepository, RegisteredUserRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
        };
    }
    );

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(setupAction =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            setupAction.SwaggerEndpoint($"/swagger/" +
                $"{swaggerSpecName}{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }

        setupAction.DefaultModelExpandDepth(2);
        setupAction.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
        setupAction.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        setupAction.EnableDeepLinking();
        setupAction.DisplayOperationId();
    });
}
else
{
    // production deployment settings for swagger
    app.UseSwagger();
    app.UseSwaggerUI(setupAction =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            setupAction.SwaggerEndpoint(baseUrl + $"/{swaggerAPIDeploymentFolder}/swagger/" +
                $"{swaggerSpecName}{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }

        setupAction.DefaultModelExpandDepth(2);
        setupAction.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
        setupAction.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        setupAction.EnableDeepLinking();
        setupAction.DisplayOperationId();
    });
}
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");  // at top of this class builder.Services.ConfigureCors(); which uses Extentions/ServiceExtensions.cs

app.UseAuthentication();    // this was essential to be added when adding token bearer authentication
app.UseAuthorization();
app.MapControllers();
app.Run();