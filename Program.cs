using System.Text;
using sheargenius_backend.Context;
using sheargenius_backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserServices>();

builder.Services.AddCors(options =>{
    options.AddPolicy("AllowAll",
    policy =>{
        policy.AllowAnyOrigin().
            AllowAnyHeader().
            AllowAnyMethod();
    });
});

var secretKey = builder.Configuration["JWT:key"];
var signingCredentials = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
builder.Services.AddDbContext<DataContext>(options =>options.UseSqlServer(connectionString));

// our secret key should match the secret key that we use to issue the token
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        //this is setting our authenitication to know what to expect and check to see if our token is valid
        //these options are defining what is valid in out token as well, and should coorelate to the options that we set upon generating our token
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        //this is a list of all the places a token should be allowed to get generated from
        ValidIssuers = new[]
        {
            "sheargenius-awakhjcph2deb6b9.westus-01.azurewebsites.net"
        },
        //this is a list of all the places a token should be allowed to get used
        ValidAudiences = new[]
        {
            "sheargenius-awakhjcph2deb6b9.westus-01.azurewebsites.net"
        },
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)) // Secret key
    };
});
//now this is set up, you can make a call to an endpoint that is protected by [Authoruze] by adding a "Authorization" header as the Key with a value of "Bearer (your token here)"


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
