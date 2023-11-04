using FirstAPI.Entityes;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


List<ModeCar> cars = new()
{
    new ModeCar()
    {
        Id = 1,
        CarBrand = "BMW",
        CarModel = "e60",
        CarAge = 10,
    },

    new ModeCar()
    {
        Id = 2,
        CarBrand = "Audi",
        CarModel = "a4",
        CarAge = 5,
    },

    new ModeCar()
    {
        Id = 3,
        CarBrand = "Mersedes-Benz",
        CarModel = "w222",
        CarAge = 8,
    }
};



app.MapGet("/", (IWebHostEnvironment env) =>
{
    var filePath = Path.Combine(env.WebRootPath, "index.html");
    return Results.File(filePath, "text/html");
});


app.MapGet("/cars", () => cars);


app.MapGet("/cars/{id}", (int id) => 
{
    var car = cars.FirstOrDefault(x => x.Id == id);
    return car is not null ? Results.Ok(car): Results.NotFound();
});

app.MapPost("/cars", (ModeCar car) => {

    car.Id = cars.Max(c => c.Id) + 1;

    cars.Add(car);
    return Results.NoContent();

});

app.MapPut("/cars/{id}", (int id, ModeCar NewCar) =>
{
    ModeCar? modeCar = cars.FirstOrDefault(car => car.Id == id);

        var findIndex = cars.FindIndex(car => car.Id == id);

        modeCar.Id = id;
        modeCar.CarBrand = NewCar.CarBrand;
        modeCar.CarModel = NewCar.CarModel;
        modeCar.CarAge = NewCar.CarAge;

        cars[findIndex] = modeCar;

        return Results.NoContent(); // Повертаємо оновлену колекцію автомобілів

});


app.MapDelete("/cars/{id}", (int id) =>
{

    ModeCar? car = cars.FirstOrDefault(c => c.Id == id);

    if (car != null)
    {
        cars.Remove(car);
    }

    return Results.NoContent();

});



app.Run();
