using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Constants
{
    public class AlertMessages
    {
        public static string NoInternetMessage { get; set; } = "Brak dostępu do internetu";
        public static string InvalidSessionMessage { get; set; } = "Sesja wygasła. Nastąpi wylogowanie";
        public static string FailedToLoadDataMessage { get; set; } = "Nie udało się pobrać danych";

        public static string LoginFailedMessage { get; set; } = "Nie udało się zalogować. Spróbuj ponownie później";
    }
}
