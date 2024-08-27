using Business.Services.Abstract;

namespace Business.Services
{
    public class HomeService : IHomeService
    {
        public string GetHttpErrorMessage(int id)
        {
            var messages = new Dictionary<int, string>
            {
                { 400, "Geçersiz istek!" },
                { 401, "Yetkisiz erişim!" },
                { 403, "Erişim engellendi!" },
                { 404, "Sayfa bulunamadı!" },
                { 500, "Sunucu hatası!" },
                { 502, "Sunucudan geçersiz yanıt!" },
                { 503, "Sunucu kullanılamıyor!" },
                { 504, "Ağ geçidi zaman aşımına uğradı" }
            };
            try
            {
                return messages[id];
            }
            catch
            {
                return "Bir hata oluştu!";
            }
        }
    }
}