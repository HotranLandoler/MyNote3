using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MyNote3.Models
{
    class SkinManager
    {
        /// <summary>
        /// 支持的皮肤集合
        /// </summary>
        private List<Skin> skins;

        public SkinManager()
        {
            skins = new List<Skin>()
            {
                new Skin("Green", "GreenSkin.xaml", (Color)ColorConverter.ConvertFromString("#16a596")),
                new Skin("Blue", "BlueSkin.xaml", (Color)ColorConverter.ConvertFromString("#3797a4")),
                new Skin("Purple", "PurpleSkin.xaml", (Color)ColorConverter.ConvertFromString("#FF6A3985")),
                new Skin("Red", "RedSkin.xaml", (Color)ColorConverter.ConvertFromString("#e97171")),
                new Skin("Orange", "OrangeSkin.xaml", (Color)ColorConverter.ConvertFromString("#ffa502")),
                new Skin("Coral", "CoralSkin.xaml", (Color)ColorConverter.ConvertFromString("#ff7f50")),
                new Skin("Gray", "GraySkin.xaml", (Color)ColorConverter.ConvertFromString("#747d8c")),
            };
        }

        /// <summary>
        /// 获取支持的皮肤集合
        /// </summary>
        /// <returns></returns>
        public List<Skin> GetSkinList()
        {
            return skins;
        }
    }
}
