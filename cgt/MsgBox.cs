namespace cgt
{
    /// <summary>
    /// Message Box class yang dibuat dengan menggunakan jAlertV3 yang bisa didownload di sini http://flwebsites.biz/jAlert/
    /// </summary>
    public class MsgBox
    {
        public static string CreateScript(string message, int type)
        {
            string r = "";
            switch (type)
            {
                case 0: r = "$.alert({title: 'Info Message',content: '" + message + "'});";
                    break;
                case 1: r = "$.alert({title: 'Error Message',content: '" + message + "'});";
                    break;
                case 2: r = "$.alert({title: 'Success Message',content: '" + message + "'});";
                    break;
                case 3: r = "$.alert({title: 'Confirmation Message',content: '" + message + "'});";
                    break;
            }
            return r;
        }
    }
}