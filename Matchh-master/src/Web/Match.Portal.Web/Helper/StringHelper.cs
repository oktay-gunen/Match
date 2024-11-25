using Match.Business.Constants.Enums;

namespace Match.Web.Helper;

public static class StringHelper
{

    public static string GetUserStatus(int statusId)
    {
        switch (statusId)
        {
            case (int)UserStatus.Passive:
                return "Pasif";
                break;
            case (int)UserStatus.Active:
                return "Aktif";
                break;
            case (int)UserStatus.Deleted:
                return "Silinen";
                break;
            default:
                return "Tanımlanmamış statu";
                break;
        }
    }
}
