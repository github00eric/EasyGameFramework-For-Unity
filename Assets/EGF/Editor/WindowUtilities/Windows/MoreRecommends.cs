using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EGF.Editor
{
    public class MoreRecommends : BaseEgfUtilityWindow
    {
        public override void OnWindowCreated()
        {
            
        }
        
        [Title("More Recommend Packages | 更多推荐功能仓库",null,TitleAlignments.Centered)]
        [DisplayAsString(false),HideLabel]
        public string mainIntroduce = "\n" +
                                      "Unity Editor Toolbox | 免费版Odin \n" +
                                      "https://github.com/arimger/Unity-Editor-Toolbox \n" +
                                      "\n" +
                                      "AssetReferenceViewer | 资源引用关系可视化检查 \n" +
                                      "https://github.com/SilverdaleGames/AssetReferenceViewer \n";
    }
}
