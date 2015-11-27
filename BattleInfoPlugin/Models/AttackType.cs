using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin.Models
{
    public enum AttackType
    {
        Normal, //1.0
        DoubleAttack, //1.2*2
        Cut_inMainMain,    //1.5
        Cut_inMainTorp,    //1.3
        Cut_inMainRadar,    //1.2
        Cut_inMainAux,    //1.1
        Cut_inTorp,     //1.5*2
        Cut_inMainMainMain,   //2.0
        Cut_inMainMainAux,   //1.75
        Cut_inMainMainTorp,    //1.3*2
    }
}
