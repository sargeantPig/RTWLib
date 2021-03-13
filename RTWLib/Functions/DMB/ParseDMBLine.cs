using RTWLib.Data;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Functions;
using RTWLib.Functions.DMB.ModelData;

namespace RTWLib.Functions.DMB
{
    public partial class DMB
    {
        protected bool ParseLine(string line, List<BattleModel> battleModels)
        {
            string[] data = LibFuncs.RemoveFirstWord(line, '\t').Trim().TrimEnd(',').Split('\t',',').CleanStringArray();
            string firstWord = LibFuncs.GetFirstWord(line, null, 0, '\t').Trim();

            switch (firstWord)
            {
                case "type":
                    battleModels.Add(new BattleModel());
                    battleModels.Last().modelType.ProcessLine(data);
                    break;
                case "skeleton":
                case "skeleton_horse":
                case "skeleton_elephant":
                case "skeleton_camel":
                case "skeleton_chariot":
                    battleModels.Last().skeletons.ProcessLine(firstWord, data);
                    break;
                case "scale":
                    battleModels.Last().modelScale.ProcessLine(data);                    
                    break;
                case "indiv_range":
                    battleModels.Last().modelIRange.ProcessLine(data);
                    break;
                case "texture":
                    battleModels.Last().texture.Add(new ModelData.Texture());
                    battleModels.Last().texture.Last().ProcessLine(data);
                    break;
                case "model_flexi_m":
                case "model_flexi":
                    battleModels.Last().modelFlexi.Add(new ModelFlexi());
                    battleModels.Last().modelFlexi.Last().ProcessLine(data, (FlexiTypes)Enum.Parse(typeof(FlexiTypes), firstWord));
                    break;
                case "model_sprite":
                    battleModels.Last().modelSprite.Add(new ModelSprite());
                    battleModels.Last().modelSprite.Last().ProcessLine(data);
                    break;
                case "model_tri":
                    battleModels.Last().modelTri.ProcessLine(data);
                    break;
            }

            return true;
        } 
    }
}
