using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace MyFirstBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IDistributedCache _cache;

        public AuthController(IDistributedCache cache)
        {
            _cache = cache;
        }

        [HttpPost("register-step1")]
        public async Task<IActionResult> RegisterStep1(string email)
        {
            // 1. Tạo mã OTP ngẫu nhiên
            string otp = new Random().Next(100000, 999999).ToString();

            // 2. Lưu OTP vào Redis với thời gian hết hạn là 5 phút
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            await _cache.SetStringAsync(email, otp, cacheOptions);

            // 3. Giả sử gửi email cho người dùng (ở đây chỉ trả về để bạn test)
            return Ok(new { message = "Mã OTP đã gửi vào Email", debug_otp = otp });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(string email, string userOtp)
        {
            // 1. Lấy mã OTP từ Redis dựa trên Email
            var savedOtp = await _cache.GetStringAsync(email);

            if (savedOtp == null || savedOtp != userOtp)
                return BadRequest("Mã OTP sai hoặc đã hết hạn");

            // 2. Nếu khớp, tiến hành lưu User vào PostgreSQL
            return Ok("Xác thực thành công! Đã tạo tài khoản.");
        }
    }
}
