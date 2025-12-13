using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Assembly_CSharp.Mod.Xmap
{
    internal class LocalizedStringsMap
    {
        internal static LocalizedStringsMap[] xmapCantGoHereKeywords = new LocalizedStringsMap[4]
        {
            new string[3] { "Bạn chưa thể đến khu vực này", "", "" },
            new string[3] { "Bang hội phải có từ 5 thành viên mới được tham gia", "", "" },
            new string[3] { "Chỉ tiếp các bang hội, miễn tiếp khách vãng lai", "", "" },
            new string[3] { "Gia nhập bang hội trên 2 ngày mới được tham gia", "", "" }
        };

        internal static LocalizedStringsMap free1hCharm = new string[3] { "thưởng bùa 1h ngẫu nhiên", "random 1 hour charm reward", "hadiah 1 jam charm" };

        internal static LocalizedStringsMap challengeKarin = new string[3] { "thách đấu thần mèo", "challenge with karin", "tantang karin" };

        internal static LocalizedStringsMap acceptChallenge = new string[3] { "đồng ý giao đấu", "accept fight", "accept fight" };

        internal static LocalizedStringsMap mercenaryTao = new string[3] { "Tàu Pảy Pảy", "Taopaipai", "Taopaipai" };

        internal static LocalizedStringsMap saoMayLuoiThe = new string[3] { "sao sư phụ không đánh đi", "", "" };

        internal static LocalizedStringsMap errorOccurred = new string[3] { "Có lỗi xảy ra vui lòng thử lại sau.", "", "" };

        internal static LocalizedStringsMap goHome = new string[3] { "Về nhà", "Go home", "Pulang" };

        internal static LocalizedStringsMap spaceshipStation = new string[3] { "Trạm tàu vũ trụ", "Spaceship station", "Spaceship station" };

        internal static LocalizedStringsMap backTo = new string[3] { "Về chỗ cũ", "Back to", "Kembali ke" };

        internal static LocalizedStringsMap stoneForest = new string[3] { "Rừng đá", "Stone forest", "Stone forest" };

        internal static LocalizedStringsMap arbitration = new string[3] { "Trọng tài", "Arbitration", "Arbitration" };

        internal static LocalizedStringsMap cantChangeZoneInThisMap = new string[3] { "Không thể đổi khu vực trong map này", "Can not change zone in this map", "Tidak bisa mengganti zone di map ini" };

        internal static LocalizedStringsMap senzuTreeUpgrading = new string[3] { "Đang nâng cấp", "Upgrading", "Sedang mengupgrade" };

        internal static LocalizedStringsMap senzuBeanHarvested = new string[3] { "Bạn vừa thu hoạch được", "You just harvested", "Kamu baru saja memanen" };

        internal static LocalizedStringsMap free1hCharmReceived = new string[3] { "Bạn vừa nhận thưởng bùa", "You receive award", "Kamu menerima hadiah" };

        internal static LocalizedStringsMap getGift = new string[3] { "Nhận quà", "Get Gift", "Menerima Gift" };

        internal static LocalizedStringsMap rejectGift = new string[3] { "Từ chối", "Reject", "Tolak" };

        internal static LocalizedStringsMap talk = new string[3] { "Nói chuyện", "Talk", "Bicara" };

        internal static LocalizedStringsMap mission = new string[3] { "Nhiệm vụ", "Quest", "Misi" };

        private string[] strings;

        public static readonly List<string> StopKeywords = new List<string>
        {
            "Bạn chưa thể đến khu vực này",
            "Bang hội phải có từ 5 thành viên mới được tham gia",
            "Chỉ tiếp các bang hội, miễn tiếp khách vãng lai",
            "Gia nhập bang hội trên 2 ngày mới được tham gia",
            "Chưa đủ điều kiện để vào",
            "Không thể vào khu vực này"
        };

        private LocalizedStringsMap(string[] strings)
        {
            this.strings = strings;
        }

        internal string Replace(string original, string newValue)
        {
            string[] array = strings;
            foreach (string oldValue in array)
            {
                original = original.Replace(oldValue, newValue);
            }
            return original;
        }

        internal bool Contains(string str)
        {
            return strings.Any((string s) => !string.IsNullOrEmpty(s) && s.Contains(str));
        }

        internal bool ContainsReversed(string str)
        {
            return strings.Any((string s) => !string.IsNullOrEmpty(s) && str.Contains(s));
        }

        internal bool StartsWith(string str)
        {
            return strings.Any((string s) => !string.IsNullOrEmpty(s) && s.StartsWith(str));
        }

        internal bool StartsWithReversed(string str)
        {
            return strings.Any((string s) => !string.IsNullOrEmpty(s) && str.StartsWith(s));
        }

        internal bool EndsWith(string str)
        {
            return strings.Any((string s) => !string.IsNullOrEmpty(s) && s.EndsWith(str));
        }

        internal bool EndsWithReversed(string str)
        {
            return strings.Any((string s) => !string.IsNullOrEmpty(s) && str.EndsWith(s));
        }

        internal bool IsEqual(string str)
        {
            return strings.Any((string s) => !string.IsNullOrEmpty(s) && s == str);
        }

        public static implicit operator LocalizedStringsMap(string[] str)
        {
            return new LocalizedStringsMap(str);
        }

        public static implicit operator string(LocalizedStringsMap localized)
        {
            return localized.strings[0];
        }

        public static bool operator ==(LocalizedStringsMap localized, string str)
        {
            return localized.IsEqual(str);
        }

        public static bool operator !=(LocalizedStringsMap localized, string str)
        {
            return !localized.IsEqual(str);
        }

        public static bool operator ==(string str, LocalizedStringsMap localized)
        {
            return localized.IsEqual(str);
        }

        public static bool operator !=(string str, LocalizedStringsMap localized)
        {
            return !localized.IsEqual(str);
        }

        public override bool Equals(object obj)
        {
            if (obj is LocalizedStringsMap localizedString)
            {
                return EqualityComparer<string[]>.Default.Equals(strings, localizedString.strings);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(strings);
        }
    }
}
