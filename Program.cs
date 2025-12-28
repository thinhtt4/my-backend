
using Microsoft.EntityFrameworkCore;
using MyFirstBackend.Data;

namespace MyFirstBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Khai báo chính sách CORS (Thêm vào trước builder.Build())
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    policy =>
                    {
                        policy.WithOrigins("https://my-frontend-six-ebon.vercel.app", "http://localhost:5173") // Cổng của React (Vite)
                               .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });
            // Trước: options.UseSqlServer(...)
            // Sau khi sửa:
            builder.Services.AddDbContext<TodoDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Đọc chuỗi kết nối từ Environment Variable (đã thiết lập trên Render)
            // Đọc chuỗi kết nối từ phần RedisCache -> ConnectionString
            var redisConn = builder.Configuration.GetSection("RedisCache:ConnectionString").Value;

            if (string.IsNullOrEmpty(redisConn))
            {
                // Log cảnh báo nếu không đọc được chuỗi kết nối
                Console.WriteLine("CẢNH BÁO: Không tìm thấy chuỗi kết nối Redis!");
            }

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConn;
                options.InstanceName = "UserAuth_";
            });

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowReactApp");
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
