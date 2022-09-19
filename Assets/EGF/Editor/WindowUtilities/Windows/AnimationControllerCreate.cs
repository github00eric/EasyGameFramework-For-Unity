using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace EGF.Editor
{
    public class AnimationControllerCreate : BaseEgfUtilityWindow
    {
        // [MenuItem(EgfEditorUtility.MenuToolMenu+"Create Animation Controller")]
        // [MenuItem(EgfEditorUtility.AssetsToolMenu+"Create Animation Controller")]
        // private static void OpenWindow()
        // {
        //     var window = GetWindow<AnimationControllerCreate>();
        //     window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
        // }
        [Title("通用动画控制器 创建工具")]
        [HideLabel, ReadOnly, DisplayAsString] public string Describe = "根据传入的动画片段，创建由 statusId 参数控制的索引动画树。";
        
        public override void OnWindowCreated()
        {
            
        }

        private const string FileExtension = ".controller";
        
        [LabelText("动画控制器文件名")]
        public string controllerFileName;

        [LabelText("创建位置"),
         FolderPath]
        public string createPath;

        [Title("动画状态名",bold:false),
         HideLabel,
         Multiline]
        public string outputEnumName;
        
        [LabelText("选中的动画片段文件")]
        [InfoBox("创建前请检查动画片段文件名，不要含符号，且文件名能概述动画内容。")]
        public List<AnimationClip> selectedAnimationClips = new List<AnimationClip>();

        // private void OnSelectionChange()
        // {
        //     selectedAnimationClips.Clear();
        //     Object[] selects = Selection.objects;
        //     if(selects == null) return;
        //     
        //     foreach (var obj in selects)
        //     {
        //         if (obj is AnimationClip clip)
        //         {
        //             selectedAnimationClips.Add(clip);
        //         }
        //     }
        // }

        private bool CheckIfReady()
        {
            if (string.IsNullOrEmpty(controllerFileName) || string.IsNullOrEmpty(createPath) || selectedAnimationClips.Count < 1)
            {
                return false;
            }
            return true;
        }

        [Button(ButtonSizes.Large),EnableIf("CheckIfReady")]
        void CreateController()
        {
            var fullPath = createPath + "/" + controllerFileName + FileExtension;
            if (File.Exists(fullPath))
            {
                Debug.LogWarning("指定路径下文件已存在，请手动删除或选择新路径和文件名。");
                return;
            }
            if(selectedAnimationClips.Count == 0)
                return;
            
            outputEnumName = ""; 
            
            // 先创建这个animator controller
            var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(createPath + "/" + controllerFileName + FileExtension);
            
            // 然后在layer 0 里加一个状态机
            var rootStateMachine = controller.layers[0].stateMachine;

            // 创建一个参数“statusId”，后面就通过修改这个参数改变动画
            controller.AddParameter("statusId", AnimatorControllerParameterType.Int);

            int clipId = 0;
            foreach (var t in selectedAnimationClips)
            {
                // 在状态机里创建state
                var state = rootStateMachine.AddState(t.name);

                // 输出状态名，方便开发者创建enum进行管理
                var tempName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(t.name);
                tempName = tempName.Replace(" ", "");
                outputEnumName += tempName + ",\n";
                
                // 把animation clip填入state的Motion里
                state.motion = t as Motion;
                // 创建跳转，这里就直接从anyState里跳转到各个state
                var stateMachineTransition = rootStateMachine.AddAnyStateTransition(state);
                // 设置跳转条件：当参数“statusId”等于一个id值时，这里id从0开始递增。
                stateMachineTransition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, clipId, "statusId");
                // 把“canTransitionToSelf ”设成false，避免卡死在动画第一帧。
                stateMachineTransition.canTransitionToSelf = false;
                // 跳转过度周期设成0.2秒
                stateMachineTransition.duration = 0.2f;

                clipId += 1; // 最后id递增
            }
        }
        
        
    }
}
