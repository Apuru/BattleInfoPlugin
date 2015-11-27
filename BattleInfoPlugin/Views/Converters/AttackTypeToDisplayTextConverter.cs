using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using BattleInfoPlugin.Models;

namespace BattleInfoPlugin.Views.Converters
{
    public class AttackTypeToDisplayTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is AttackType)) return "";
            var type = (AttackType)value;
            return type == AttackType.Cut_inMainMain ? "Cut-in (x1.5)"
                : type == AttackType.Cut_inMainTorp ? "Cut-in (x1.3)"
                : type == AttackType.Cut_inMainRadar ? "Cut-in (x1.2)"
                : type == AttackType.Cut_inMainAux ? "Cut-in (x1.1)"
                : type == AttackType.Cut_inTorp ? "Cut-in (x1.5 x2)"
                : type == AttackType.Cut_inMainMainMain ? "Cut-in (x2.0)"
                : type == AttackType.Cut_inMainMainAux ? "Cut-in (x1.75)"
                : type == AttackType.Cut_inMainMainTorp ? "Cut-in (x1.3 x2)"
                : type == AttackType.DoubleAttack ? "Double Attack (x1.2 x2)"
                : "Normal";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
