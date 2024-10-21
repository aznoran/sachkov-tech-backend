using NotificationService.Entities;

namespace NotificationService.Extensions
{
    public static class ModelExtensions
    {
        public static void UpdatePreferences(
            this NotificationSettings settings,
            bool? email = null,
            bool? telegram = null,
            bool? web = null)
        {
            settings.Email = email ?? settings.Email;
            settings.Telegram = telegram ?? settings.Telegram;
            settings.Web = web ?? settings.Web;
        }
    }
}
